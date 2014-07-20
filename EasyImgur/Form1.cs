using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

namespace EasyImgur
{
    public partial class Form1 : Form
    {
        private bool CloseCommandWasSentFromExitButton = false;

        /// <summary>
        /// Property to easily access the path of the executable, quoted for safety.
        /// </summary>
        private string QuotedApplicationPath { get { return "\"" + Application.ExecutablePath + "\""; } }

        public Form1(SingleInstance _SingleInstance, string[] _Args)
        {
            InitializeComponent();

            Application.ApplicationExit += new System.EventHandler(this.ApplicationExit);

            this.notifyIcon1.ContextMenu = this.trayMenu;

            InitializeEventHandlers();
            History.BindData(historyItemBindingSource); // to use the designer with data binding, we have to pass History our BindingSource, instead of just getting one from History
            History.InitializeFromDisk();

            // if we have arguments, we're going to show a tip when we handle those arguments. 
            if(_Args.Length == 0) 
                notifyIcon1.ShowBalloonTip(2000, "EasyImgur is ready for use!", "Right-click EasyImgur's icon in the tray to use it!", ToolTipIcon.Info);

            ImgurAPI.AttemptRefreshTokensFromDisk();

            Statistics.GatherAndSend();

            _SingleInstance.ArgumentsReceived += singleInstance_ArgumentsReceived;
            if(_Args.Length > 0) // handle initial arguments
                singleInstance_ArgumentsReceived(this, new ArgumentsReceivedEventArgs() { Args = _Args });
        }

        private void InitializeEventHandlers()
        {
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_Closing);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.VisibleChanged += new System.EventHandler(this.Form1_VisibleChanged);
            notifyIcon1.BalloonTipClicked += new System.EventHandler(this.NotifyIcon1_BalloonTipClicked);
            notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon1_MouseDoubleClick);
            tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);

            ImgurAPI.obtainedAuthorization += new ImgurAPI.AuthorizationEventHandler(this.ObtainedAPIAuthorization);
            ImgurAPI.refreshedAuthorization += new ImgurAPI.AuthorizationEventHandler(this.RefreshedAPIAuthorization);
            ImgurAPI.lostAuthorization += new ImgurAPI.AuthorizationEventHandler(this.LostAPIAuthorization);
            ImgurAPI.networkRequestFailed += new ImgurAPI.NetworkEventHandler(this.APINetworkRequestFailed);
        }

        void singleInstance_ArgumentsReceived( object sender, ArgumentsReceivedEventArgs e )
        {
            // As a side effect of the way this function is implemented, something like
            // "EasyImgur.exe file1 file2 /anonymous file3 file4" will cause file1 and file2 to be
            // uploaded using the account and file3 and file4 to be uploaded anonymously. I left this in
            // as a neat little feature.
            // However, all uploads will fail if the user is not logged in.
            bool anonymous = false;
            foreach(string path in e.Args.Where(s => { return s != null; })) // e.Args may contain a single null string
            {
                if(path == "/anonymous")
                {
                    anonymous = true;
                    continue;
                }

                if(!anonymous && !ImgurAPI.HasBeenAuthorized())
                {
                    notifyIcon1.ShowBalloonTip(2000, "Not logged in", "You aren't logged in. Authorize EasyImgur and try again.", ToolTipIcon.Error);
                    return;
                }
                
                if(Directory.Exists(path))
                {
                    string[] fileTypes = new[] { ".jpg", ".jpeg", ".png", ".apng", ".bmp",
                        ".gif", ".tiff", ".tif", ".xcf" };
                    List<string> files = new List<string>();
                    foreach(string s in Directory.GetFiles(path))
                    {
                        bool cont = false;
                        foreach(string filetype in fileTypes)
                            if(s.EndsWith(filetype, true, null))
                            {
                                cont = true;
                                break;
                            }
                        if(!cont)
                            continue;

                        files.Add(s);
                    }
                    
                    UploadAlbum(anonymous, files.ToArray(), path.Split('\\').Last());
                }
                else if(File.Exists(path))
                    UploadFile(anonymous, new string[] { path });
            }
        }

        private void ApplicationExit( object sender, EventArgs e )
        {
            ImgurAPI.OnMainThreadExit();
            notifyIcon1.Visible = false;
        }

        private void ObtainedAPIAuthorization()
        {
            SetAuthorizationStatusUI(true);
            notifyIcon1.ShowBalloonTip(2000, "EasyImgur", "EasyImgur has received authorization to use your Imgur account!", ToolTipIcon.Info);
        }

        private void RefreshedAPIAuthorization()
        {
            SetAuthorizationStatusUI(true);
            if (Properties.Settings.Default.showNotificationOnTokenRefresh)
            {
                notifyIcon1.ShowBalloonTip(2000, "EasyImgur", "EasyImgur has successfully refreshed authorization tokens!", ToolTipIcon.Info);
            }
        }

        private void SetAuthorizationStatusUI( bool _IsAuthorized )
        {
            uploadClipboardAccountTrayMenuItem.Enabled = _IsAuthorized;
            uploadFileAccountTrayMenuItem.Enabled = _IsAuthorized;
            label13.Text = _IsAuthorized ? "Authorized" : "Not authorized";
            label13.ForeColor = _IsAuthorized ? System.Drawing.Color.Green : System.Drawing.Color.DarkBlue;
            buttonForceTokenRefresh.Enabled = _IsAuthorized;
            buttonForgetTokens.Enabled = _IsAuthorized;
        }

        private void LostAPIAuthorization()
        {
            SetAuthorizationStatusUI(false);
            notifyIcon1.ShowBalloonTip(2000, "EasyImgur", "EasyImgur no longer has authorization to use your Imgur account!", ToolTipIcon.Info);
        }

        private void APINetworkRequestFailed()
        {
            notifyIcon1.ShowBalloonTip(2000, "EasyImgur", "Network request failed. Check your internet connection.", ToolTipIcon.Error);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 2)
            {
                SelectedHistoryItemChanged();
            }
        }

        private void UploadClipboard( bool _Anonymous )
        {
            APIResponses.ImageResponse resp = null;
            Image clipboardImage = null;
            string clipboardURL = string.Empty;
            //bool anonymous = !Properties.Settings.Default.useAccount || !ImgurAPI.HasBeenAuthorized();
            bool anonymous = _Anonymous;
            if (Clipboard.ContainsImage())
            {
                clipboardImage = Clipboard.GetImage();
                notifyIcon1.ShowBalloonTip(4000, "Hold on...", "Attempting to upload image to Imgur...", ToolTipIcon.None);
                resp = ImgurAPI.UploadImage(clipboardImage, GetTitleString(), GetDescriptionString(), _Anonymous);
            }
            else if (Clipboard.ContainsText())
            {
                clipboardURL = Clipboard.GetText(TextDataFormat.UnicodeText);
                Uri uriResult;
                if (Uri.TryCreate(clipboardURL, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                {
                    notifyIcon1.ShowBalloonTip(4000, "Hold on...", "Attempting to upload image to Imgur...", ToolTipIcon.None);
                    resp = ImgurAPI.UploadImage(clipboardURL, GetTitleString(), GetDescriptionString(), _Anonymous);
                }
                else
                {
                    notifyIcon1.ShowBalloonTip(2000, "Can't upload clipboard!", "There's text on the clipboard but it's not a valid URL", ToolTipIcon.Error);
                    return;
                }
            }
            else
            {
                notifyIcon1.ShowBalloonTip(2000, "Can't upload clipboard!", "There's no image or URL there", ToolTipIcon.Error);
                return;
            }
            if (resp.success)
            {
                // this doesn't need an invocation guard because this function can't be called from the context menu
                if (Properties.Settings.Default.copyLinks)
                {
                    Clipboard.SetText(resp.data.link);
                }

                notifyIcon1.ShowBalloonTip(2000, "Success!", Properties.Settings.Default.copyLinks ? "Link copied to clipboard" : "Upload placed in history: " + resp.data.link, ToolTipIcon.None);

                HistoryItem item = new HistoryItem();
                item.timestamp = DateTime.Now;
                item.id = resp.data.id;
                item.link = resp.data.link;
                item.deletehash = resp.data.deletehash;
                item.title = resp.data.title;
                item.description = resp.data.description;
                item.anonymous = anonymous;
                if (clipboardImage != null)
                {
                    item.thumbnail = clipboardImage.GetThumbnailImage(pictureBox1.Width, pictureBox1.Height, null, System.IntPtr.Zero);
                }
                History.StoreHistoryItem(item);
            }
            else
            {
                notifyIcon1.ShowBalloonTip(2000, "Failed", "Could not upload image (" + resp.status + "):", ToolTipIcon.None);
            }

            if (!Properties.Settings.Default.clearClipboardOnUpload)
            {
                if (clipboardImage != null)
                {
                    Clipboard.SetImage(clipboardImage);
                }
                else
                {
                    Clipboard.SetText(clipboardURL, TextDataFormat.UnicodeText);
                }
            }
        }

        private void UploadAlbum( bool _Anonymous, string[] _Paths, string _AlbumTitle )
        {
            notifyIcon1.ShowBalloonTip(2000, "Hold on...", "Attempting to upload album to Imgur (this may take a while)...", ToolTipIcon.None);
            List<Image> images = new List<Image>();
            foreach(string path in _Paths)
                using(Stream stream = System.IO.File.Open(path, System.IO.FileMode.Open))
                    images.Add(System.Drawing.Image.FromStream(stream));

            APIResponses.AlbumResponse response = ImgurAPI.UploadAlbum(images.ToArray(), _AlbumTitle, _Anonymous, GetTitleString(), GetDescriptionString());
            if(response.success)
            {
                // clipboard calls can only be made on an STA thread, threading model is MTA when invoked from context menu
                if(System.Threading.Thread.CurrentThread.GetApartmentState() != System.Threading.ApartmentState.STA)
                    this.Invoke(new Action(delegate { Clipboard.SetText(response.data.link); }));
                else
                    Clipboard.SetText(response.data.link);

                notifyIcon1.ShowBalloonTip(2000, "Success!", Properties.Settings.Default.copyLinks ? "Link copied to clipboard" : "Upload placed in history: " + response.data.link, ToolTipIcon.None);

                HistoryItem item = new HistoryItem();
                item.timestamp = DateTime.Now;
                item.id = response.data.id;
                item.link = response.data.link;
                item.deletehash = response.data.deletehash;
                item.title = response.data.title;
                item.description = response.data.description;
                item.anonymous = _Anonymous;
                item.album = true;
                item.thumbnail = response.CoverImage.GetThumbnailImage(pictureBox1.Width, pictureBox1.Height, null, System.IntPtr.Zero);
                Invoke(new Action(() => { History.StoreHistoryItem(item); }));
            }
            else
            {
                notifyIcon1.ShowBalloonTip(2000, "Failed", "Could not upload album (" + response.status + "): " + response.data.error, ToolTipIcon.None);
            }
        }

        private void UploadFile( bool _Anonymous, string[] _Paths = null )
        {
            if(_Paths == null)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = true;
                DialogResult res = dialog.ShowDialog();
                if(res == DialogResult.OK)
                    _Paths = dialog.FileNames;
            }
            if (_Paths != null)
            {
                if(_Paths.Length > 1 && Properties.Settings.Default.uploadMultipleImagesAsGallery)
                {
                    UploadAlbum(_Anonymous, _Paths, "");
                    return;
                }

                int success = 0;
                int failure = 0;
                int i = 0;
                foreach (string fileName in _Paths)
                {
                    ++i;

                    if (fileName == null || fileName == string.Empty)
                    {
                        continue;
                    }

                    string fileCounterString = (_Paths.Length > 1) ? (" (" + i.ToString() + "/" + _Paths.Length.ToString() + ") ") : (string.Empty);

                    try
                    {
                        notifyIcon1.ShowBalloonTip(2000, "Hold on..." + fileCounterString, "Attempting to upload image to Imgur...", ToolTipIcon.None);
                        Image img;
                        APIResponses.ImageResponse resp;
                        using(System.IO.FileStream stream = System.IO.File.Open(fileName, System.IO.FileMode.Open))
                        {
                            // a note to the future: ImgurAPI.UploadImage called Image.Save(); Image.Save()
                            // requires that the stream it was loaded from still be open, or else you get
                            // an immensely generic error. 
                            img = System.Drawing.Image.FromStream(stream);
                            resp = ImgurAPI.UploadImage(img, GetTitleString(), GetDescriptionString(), _Anonymous);
                        }
                        if (resp.success)
                        {
                            success++;

                            if (Properties.Settings.Default.copyLinks)
                            {
                                // clipboard calls can only be made on an STA thread, threading model is MTA when invoked from context menu
                                if(System.Threading.Thread.CurrentThread.GetApartmentState() != System.Threading.ApartmentState.STA)
                                    this.Invoke(new Action(delegate { Clipboard.SetText(resp.data.link); }));
                                else
                                    Clipboard.SetText(resp.data.link);
                            }

                            notifyIcon1.ShowBalloonTip(2000, "Success!" + fileCounterString, Properties.Settings.Default.copyLinks ? "Link copied to clipboard" : "Upload placed in history: " + resp.data.link, ToolTipIcon.None);

                            HistoryItem item = new HistoryItem(); 
                            item.timestamp = DateTime.Now;
                            item.id = resp.data.id;
                            item.link = resp.data.link;
                            item.deletehash = resp.data.deletehash;
                            item.title = resp.data.title;
                            item.description = resp.data.description;
                            item.anonymous = _Anonymous;
                            item.thumbnail = img.GetThumbnailImage(pictureBox1.Width, pictureBox1.Height, null, System.IntPtr.Zero);
                            Invoke(new Action(() => { History.StoreHistoryItem(item); }));
                        }
                        else
                        {
                            failure++;
                            notifyIcon1.ShowBalloonTip(2000, "Failed" + fileCounterString, "Could not upload image (" + resp.status + "): " + resp.data.error, ToolTipIcon.None);
                        }
                    }
                    catch (System.IO.FileNotFoundException)
                    {
                        failure++;
                        notifyIcon1.ShowBalloonTip(2000, "Failed" + fileCounterString, "Could not find image file on disk (" + fileName + "):", ToolTipIcon.Error);
                    }
                }
                if(_Paths.Length > 1)
                {
                    notifyIcon1.ShowBalloonTip(2000, "Done", "Successfully uploaded " + success.ToString() + " files" + ((failure > 0) ? (" (Warning: " + failure.ToString() + " failed)") : (string.Empty)), ToolTipIcon.Info);
                }
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            SaveSettings();
            //this.Hide();
        }

        private void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.Activate();
            this.Focus();
            this.BringToFront();
        }

        private void NotifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            /*if (!Properties.Settings.Default.copyLinks)
            {

            }*/
            this.Show();
            tabControl1.SelectedIndex = 2;
            listBoxHistory.SelectedIndex = listBoxHistory.Items.Count - 1;
            this.BringToFront();
        }

        private void Form1_VisibleChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();

            // Assign control values. Most values are set using application binding on the control.
            comboBoxImageFormat.SelectedIndex = Properties.Settings.Default.imageFormat;

            // Check the registry for a key describing whether EasyImgur should be started on boot.
            Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            string value = (string)registryKey.GetValue("EasyImgur", string.Empty); // string.Empty is returned if no key is present.
            checkBoxLaunchAtBoot.Checked = value != string.Empty;
            if (value != string.Empty && value != QuotedApplicationPath)
            {
                // A key exists, make sure we're using the most up-to-date path!
                registryKey.SetValue("EasyImgur", QuotedApplicationPath);
                notifyIcon1.ShowBalloonTip(2000, "EasyImgur", "Updated registry path", ToolTipIcon.Info);
            }
            UpdateRegistry(true); // this will need to be updated too, if we're using it
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Form1_VisibleChanged(null, null);
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveSettings();
            this.Hide();

            e.Cancel = !CloseCommandWasSentFromExitButton;  // Don't want to *actually* close the form unless the Exit button was used.
        }

        private void SaveSettings()
        {
            // Store control values. Most values are stored using application binding on the control.
            Properties.Settings.Default.imageFormat = comboBoxImageFormat.SelectedIndex;

            using(Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                if(checkBoxLaunchAtBoot.Checked)
                {
                    // If the checkbox was marked, set a value which will make EasyImgur start at boot.
                    registryKey.SetValue("EasyImgur", QuotedApplicationPath);
                }
                else
                {
                    // Delete our value if one is present; second argument supresses exception on missing value
                    registryKey.DeleteValue("EasyImgur", false);
                }
            }

            UpdateRegistry(false);

            Properties.Settings.Default.Save();
        }

        private void UpdateRegistry(bool _LoadingSettings)
        {
            if(_LoadingSettings) // update enableContextMenu if necessary
            {
                // look for one of our handlers; Directory is the easiest and most reliable
                // Since HKCR is a merging of HKCU\Software\Classes and HKLM\Software\Classes, look there
                using(RegistryKey dir = Registry.ClassesRoot.OpenSubKey("Directory"))
                using(RegistryKey shell = dir.OpenSubKey("shell"))
                    Properties.Settings.Default.enableContextMenu = shell != null && shell.OpenSubKey("imguruploadanonymous") != null;
            }

            // a note: Directory doesn't work if within SystemFileAssociations, and 
            // the extensions don't work if not inside them. At least, this seems to be the case for me

            // another note: I discovered that I had the logic flipped, and the code actually did the opposite
            // of what I describe in the above note, and it was working. Earlier, though, when I wrote that,
            // it seemed to be true. Either way, the placement (inside or outside of SystemFileAssociations)
            // does affect where in the context menu they show up. Feel free to play with the placement and see
            // if you can get it to work.
            string[] fileTypes = new[] { ".jpg", ".jpeg", ".png", ".apng", ".bmp",
            ".gif", ".tiff", ".tif", ".pdf", ".xcf", "Directory" };
            using(RegistryKey root = Registry.CurrentUser.OpenSubKey("Software\\Classes", true))
            using(RegistryKey fileAssoc = root.CreateSubKey("SystemFileAssociations"))
                foreach(string fileType in fileTypes)
                    using(RegistryKey fileTypeKey = (fileType != "Directory" ? fileAssoc.CreateSubKey(fileType) : root.CreateSubKey(fileType)))
                    using(RegistryKey shell = fileTypeKey.CreateSubKey("shell"))
                    {
                        if(Properties.Settings.Default.enableContextMenu)
                        {
                            using(RegistryKey anonHandler = shell.CreateSubKey("imguruploadanonymous"))
                                EnableContextMenu(anonHandler, "Upload to Imgur" +
                                    (fileType == "Directory" ? " as album" : "") + " (anonymous)", true);
                            using(RegistryKey accHandler = shell.CreateSubKey("imgurupload"))
                                EnableContextMenu(accHandler, "Upload to Imgur" +
                                    (fileType == "Directory" ? " as album" : ""), false);
                        }
                        else
                        {
                            try { shell.DeleteSubKeyTree("imguruploadanonymous"); } catch { }
                            try { shell.DeleteSubKeyTree("imgurupload"); } catch { }
                        }
                    }
        }

        private void EnableContextMenu(RegistryKey key, string commandText, bool anonymous)
        {
            key.SetValue("", commandText);
            key.SetValue("Icon", QuotedApplicationPath);
            using(RegistryKey subKey = key.CreateSubKey("command"))
                subKey.SetValue("", QuotedApplicationPath + (anonymous ? " /anonymous" : "") + " \"%1\"");
        }

        private void buttonChangeCredentials_Click(object sender, EventArgs e)
        {
            AuthorizeForm accountCredentialsForm = new AuthorizeForm();
            DialogResult res = accountCredentialsForm.ShowDialog(this);
            
            if (ImgurAPI.HasBeenAuthorized())
            {
                buttonForceTokenRefresh.Enabled = true;
            }
            else
            {
                buttonForceTokenRefresh.Enabled = false;
            }
        }

        private void SelectedHistoryItemChanged()
        {
            HistoryItem item = listBoxHistory.SelectedItem as HistoryItem;
            if (item == null)
            {
                // I'm certain there's a way to get rid of this block with the help of data binding,
                // but I'm not sure how to and I'm tired of messing with it
                textBoxID.Text = string.Empty;
                textBoxLink.Text = string.Empty;
                textBoxDeleteHash.Text = string.Empty;
                textBoxTimestamp.Text = string.Empty;
                pictureBox1.Image = null;
                checkBoxTiedToAccount.Checked = false;
                checkBoxAlbum.Checked = false;

                buttonRemoveFromImgur.Enabled = false;
                buttonRemoveFromHistory.Enabled = false;
                btnOpenImageLinkInBrowser.Enabled = false;
            }
            else
            {
                buttonRemoveFromImgur.Enabled = item.anonymous || (!item.anonymous && ImgurAPI.HasBeenAuthorized());
                buttonRemoveFromHistory.Enabled = true;
                btnOpenImageLinkInBrowser.Enabled = true;
            }
        }

        private void listBoxHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedHistoryItemChanged();
        }

        private string FormatInfoString( string _Input )
        {
            return FormattingHelper.Format(_Input);
        }

        private string GetTitleString()
        {
            return FormatInfoString(textBoxTitleFormat.Text);
        }

        private string GetDescriptionString()
        {
            return FormatInfoString(textBoxDescriptionFormat.Text);
        }

        private void buttonRemoveFromImgur_Click(object sender, EventArgs e)
        {
            int count = listBoxHistory.SelectedItems.Count;
            bool isMultipleImages = count > 1;
            int currentCount = 0;

            listBoxHistory.BeginUpdate();
            List<HistoryItem> selectedItems = new List<HistoryItem>(listBoxHistory.SelectedItems.Cast<HistoryItem>());
            foreach(HistoryItem item in selectedItems)
            {
                ++currentCount;

                if (item == null)
                {
                    return;
                }

                string balloon_image_counter_text = (isMultipleImages ? (currentCount.ToString() + "/" + count.ToString()) : string.Empty);
                string balloon_text = "Attempting to remove " + (item.album ? "album" : "image") + " " + balloon_image_counter_text + " from Imgur...";
                notifyIcon1.ShowBalloonTip(2000, "Hold on...", balloon_text, ToolTipIcon.None);
                if (item.album ? ImgurAPI.DeleteAlbum(item.deletehash, item.anonymous) : ImgurAPI.DeleteImage(item.deletehash, item.anonymous))
                {
                    notifyIcon1.ShowBalloonTip(2000, "Success!", "Removed " + (item.album ? "album" : "image") + " " + balloon_image_counter_text + " from Imgur and history", ToolTipIcon.None);
                    History.RemoveHistoryItem(item);
                }
                else
                {
                    notifyIcon1.ShowBalloonTip(2000, "Failed", "Failed to remove " + (item.album ? "album" : "image") + " " + balloon_image_counter_text + " from Imgur", ToolTipIcon.Error);
                }
            }
            listBoxHistory.EndUpdate();

            if(listBoxHistory.Items.Count > 0)
                listBoxHistory.SelectedIndex = 0;
        }

        private void buttonForceTokenRefresh_Click(object sender, EventArgs e)
        {
            ImgurAPI.ForceRefreshTokens();
            SetAuthorizationStatusUI(ImgurAPI.HasBeenAuthorized());
        }

        private void buttonFormatHelp_Click(object sender, EventArgs e)
        {
            FormattingHelper.FormattingScheme[] formattingSchemes = FormattingHelper.GetSchemes();
            string helpString = "You can use strings consisting of either static characters or the following dynamic symbols, or a combination of both:\n\n";
            foreach (FormattingHelper.FormattingScheme scheme in formattingSchemes)
            {
                helpString += scheme.symbol + "  :  " + scheme.description + "\n";
            }
            string exampleFormattedString = "Image_%date%_%time%";
            helpString += "\n\nEx.: '" + exampleFormattedString + "' would become: '" + FormattingHelper.Format(exampleFormattedString);
            Point loc = this.Location;
            loc.Offset(buttonFormatHelp.Location.X, buttonFormatHelp.Location.Y);
            Help.ShowPopup(this, helpString, loc);
        }

        private void buttonForgetTokens_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to discard the tokens?\n\nWithout tokens, the app can no longer use your Imgur account.", "Forget tokens", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ImgurAPI.ForgetTokens();
                MessageBox.Show("Tokens have been forgotten. Remember that the app has still technically been authorized on the Imgur website, we can't change this for you!\n\nGo to http://imgur.com/account/settings/apps to revoke access.", "Tokens discarded", MessageBoxButtons.OK);
            }
        }

        private void buttonRemoveFromHistory_Click(object sender, EventArgs e)
        {
            listBoxHistory.BeginUpdate();
            List<HistoryItem> selectedItems = new List<HistoryItem>(listBoxHistory.SelectedItems.Cast<HistoryItem>());
            foreach(HistoryItem item in selectedItems)
            {
                if (item == null)
                {
                    return;
                }

                History.RemoveHistoryItem(item);
            }
            listBoxHistory.EndUpdate();

            if(listBoxHistory.Items.Count > 0)
                listBoxHistory.SelectedIndex = 0;
        }

        private void uploadClipboardAccountTrayMenuItem_Click(object sender, EventArgs e)
        {
            UploadClipboard(false);
        }

        private void uploadFileAccountTrayMenuItem_Click(object sender, EventArgs e)
        {
            UploadFile(false);
        }

        private void uploadClipboardAnonymousTrayMenuItem_Click(object sender, EventArgs e)
        {
            UploadClipboard(true);
        }

        private void uploadFileAnonymousTrayMenuItem_Click(object sender, EventArgs e)
        {
            UploadFile(true);
        }

        private void settingsTrayMenuItem_Click(object sender, EventArgs e)
        {
            // Open settings form.
            this.Show();
        }

        private void exitTrayMenuItem_Click(object sender, EventArgs e)
        {
            CloseCommandWasSentFromExitButton = true;
            Application.Exit();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://easyimgur.bryankeiren.com/");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://bryankeiren.com/");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://imgur.com/account/settings/apps");
        }

        private void btnOpenImageLinkInBrowser_Click(object sender, EventArgs e)
        {
            HistoryItem item = listBoxHistory.SelectedItem as HistoryItem;
            if (item != null)
            {
                System.Diagnostics.Process.Start(item.link);
            }
        }
    }
}
