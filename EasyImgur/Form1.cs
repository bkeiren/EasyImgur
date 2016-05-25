using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using EasyImgur.Properties;
using Microsoft.Win32;

namespace EasyImgur
{
    public partial class Form1 : Form
    {
        private bool closeCommandWasSentFromExitButton = false;
        private enum MessageVerbosity { Normal = 0, NoInfo = 1, NoError = 2 }
        private MessageVerbosity verbosity = MessageVerbosity.Normal;

        /// <summary>
        /// Property to easily access the path of the executable, quoted for safety.
        /// </summary>
        private string QuotedApplicationPath => "\"" + Application.ExecutablePath + "\"";

        public Form1(SingleInstance singleInstance, string[] args)
        {
            this.InitializeComponent();

            this.ImplementPortableMode();

            this.CreateHandle(); // force the handle to be created so Invoke succeeds; see issue #8 for more detail

            this.notifyIcon1.ContextMenu = this.trayMenu;

            this.versionLabel.Text = "Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            Application.ApplicationExit += this.ApplicationExit;

            this.InitializeEventHandlers();
            History.BindData(this.historyItemBindingSource); // to use the designer with data binding, we have to pass History our BindingSource, instead of just getting one from History
            History.InitializeFromDisk();

            // if we have arguments, we're going to show a tip when we handle those arguments. 
            if (args.Length == 0)
                this.ShowBalloonTip(2000, "EasyImgur is ready for use!", "Right-click EasyImgur's icon in the tray to use it!", ToolTipIcon.Info);

            ImgurAPI.AttemptRefreshTokensFromDisk();

            Statistics.GatherAndSend();

            singleInstance.ArgumentsReceived += this.singleInstance_ArgumentsReceived;
            if (args.Length > 0) // handle initial arguments
                this.singleInstance_ArgumentsReceived(this, new ArgumentsReceivedEventArgs() { Args = args });
        }

        private void InitializeEventHandlers()
        {
            this.FormClosing += this.Form1_Closing;
            this.Load += this.Form1_Load;
            this.notifyIcon1.BalloonTipClicked += this.NotifyIcon1_BalloonTipClicked;
            this.notifyIcon1.MouseDoubleClick += this.NotifyIcon1_MouseDoubleClick;
            this.tabControl1.SelectedIndexChanged += this.tabControl1_SelectedIndexChanged;

            ImgurAPI.ObtainedAuthorization += this.ObtainedAPIAuthorization;
            ImgurAPI.RefreshedAuthorization += this.RefreshedAPIAuthorization;
            ImgurAPI.LostAuthorization += this.LostAPIAuthorization;
            ImgurAPI.NetworkRequestFailed += this.APINetworkRequestFailed;
        }

        void ImplementPortableMode()
        {
            if (Program.InPortableMode)
            {
                this.checkBoxLaunchAtBoot.Enabled = false;
                this.checkBoxEnableContextMenu.Enabled = false;
                this.Text += " - Portable Mode";
            }
            else
            {
                this.labelPortableModeNote.Visible = false;
            }
        }

        bool ShouldShowMessage(MessageVerbosity verbosity)
        {
            return this.verbosity < verbosity;
        }

        void singleInstance_ArgumentsReceived(object sender, ArgumentsReceivedEventArgs e)
        {
            // Using "/exit" anywhere in the command list will cause EasyImgur to exit after uploading;
            // this will happen regardless of the execution sending the exit command was the execution that
            // launched the initial instance of EasyImgur.
            var exitWhenFinished = false;
            var anonymous = false;

            // mappings of switch names to actions
            var handlers = new Dictionary<string, Action>() {
                { "anonymous", () => anonymous = true },
                { "noinfo", () => this.verbosity = (MessageVerbosity)Math.Max((int) this.verbosity, (int)MessageVerbosity.NoInfo) },
                { "q", () => this.verbosity = (MessageVerbosity)Math.Max((int) this.verbosity, (int)MessageVerbosity.NoInfo) },
                { "noerr", () => this.verbosity = (MessageVerbosity)Math.Max((int) this.verbosity, (int)MessageVerbosity.NoError) },
                { "qq", () => this.verbosity = (MessageVerbosity)Math.Max((int) this.verbosity, (int)MessageVerbosity.NoError) },
                { "exit", () => exitWhenFinished = true },
                { "portable", () => { } } // ignore
            };

            try
            {
                // First scan for switches
                var badSwitchCount = 0;
                foreach (var str in e.Args.Where(s => s != null && s.StartsWith("/")))
                {
                    var param = str.Remove(0, 1); // Strip the leading '/' from the switch.

                    if (handlers.ContainsKey(param))
                    {
                        Log.Warning("Consuming command-line switch '" + param + "'.");
                        handlers[param]();
                    }
                    else
                    {
                        ++badSwitchCount;
                        Log.Warning("Ignoring unrecognized command-line switch '" + param + "'.");
                    }
                }

                if (badSwitchCount > 0 && this.ShouldShowMessage(MessageVerbosity.NoError))
                {
                    this.ShowBalloonTip(2000, "Invalid switch", badSwitchCount.ToString() + " invalid switch" + (badSwitchCount > 1 ? "es were" : " was") + " passed to EasyImgur (see log for details). No files were uploaded.", ToolTipIcon.Error, true);
                    return;
                }

                // Process actual arguments
                foreach (var path in e.Args.Where(s => s != null && !s.StartsWith("/")))
                {
                    if (!anonymous && !ImgurAPI.HasBeenAuthorized())
                    {
                        this.ShowBalloonTip(2000, "Not logged in", "You aren't logged in but you're trying to upload to an account. Authorize EasyImgur and try again.", ToolTipIcon.Error, true);
                        return;
                    }

                    if (Directory.Exists(path))
                    {
                        var fileTypes = new[] { ".jpg", ".jpeg", ".png", ".apng", ".bmp",
                            ".gif", ".tiff", ".tif", ".xcf" };
                        var files = new List<string>();
                        foreach (var s in Directory.GetFiles(path))
                        {
                            var cont = false;
                            foreach (var filetype in fileTypes)
                                if (s.EndsWith(filetype, true, null))
                                {
                                    cont = true;
                                    break;
                                }
                            if (!cont)
                                continue;

                            files.Add(s);
                        }

                        this.UploadAlbum(anonymous, files.ToArray(), path.Split('\\').Last());
                    }
                    else if (File.Exists(path))
                        this.UploadFile(anonymous, new string[] { path });
                }
            }
            catch (Exception ex)
            {
                Log.Error("Unhandled exception in context menu thread: " + ex.ToString());
                this.ShowBalloonTip(2000, "Error", "An unknown exception occurred during upload. Check the log for further information.", ToolTipIcon.Error, true);
            }

            if (exitWhenFinished)
                Application.Exit();

            // reset verbosity
            this.verbosity = MessageVerbosity.Normal;
        }

        private void ShowBalloonTip(int timeout, string title, string text, ToolTipIcon icon, bool error = false)
        {
            if (this.ShouldShowMessage(error ? MessageVerbosity.NoError : MessageVerbosity.NoInfo))
            {
                if (this.notifyIcon1 != null)
                    this.notifyIcon1.ShowBalloonTip(timeout, title, text, icon);
                Log.Info(string.Format("Showed tooltip with title \"{0}\" and text \"{1}\".", title, text));
            }
            else
            {
                Log.Info(string.Format("Tooltip with title \"{0}\" and text \"{1}\" was suppressed.", title, text));
            }
        }

        private void ApplicationExit(object sender, EventArgs e)
        {
            Statistics.GatherAndSend();

            ImgurAPI.OnMainThreadExit();
            if (this.notifyIcon1 != null)
                this.notifyIcon1.Visible = false;
        }

        private void ObtainedAPIAuthorization()
        {
            this.SetAuthorizationStatusUI(true);
            this.ShowBalloonTip(2000, "EasyImgur", "EasyImgur has received authorization to use your Imgur account!", ToolTipIcon.Info);
        }

        private void RefreshedAPIAuthorization()
        {
            this.SetAuthorizationStatusUI(true);
            if (Settings.Default.showNotificationOnTokenRefresh)
                this.ShowBalloonTip(2000, "EasyImgur", "EasyImgur has successfully refreshed authorization tokens!", ToolTipIcon.Info);
        }

        private void SetAuthorizationStatusUI(bool isAuthorized)
        {
            this.uploadClipboardAccountTrayMenuItem.Enabled = isAuthorized;
            this.uploadFileAccountTrayMenuItem.Enabled = isAuthorized;
            this.label13.Text = isAuthorized ? "Authorized" : "Not authorized";
            this.label13.ForeColor = isAuthorized ? Color.Green : Color.DarkBlue;
            this.buttonForceTokenRefresh.Enabled = isAuthorized;
            this.buttonForgetTokens.Enabled = isAuthorized;
        }

        private void LostAPIAuthorization()
        {
            this.SetAuthorizationStatusUI(false);
            this.ShowBalloonTip(2000, "EasyImgur", "EasyImgur no longer has authorization to use your Imgur account!", ToolTipIcon.Info);
        }

        private void APINetworkRequestFailed()
        {
            this.ShowBalloonTip(2000, "EasyImgur", "Network request failed. Check your internet connection.", ToolTipIcon.Error, true);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedIndex == 2)
            {
                this.SelectedHistoryItemChanged();
            }
        }

        private void UploadClipboard(bool _Anonymous)
        {
            APIResponses.ImageResponse resp = null;
            Image clipboardImage = null;
            var clipboardUrl = string.Empty;
            //bool anonymous = !Properties.Settings.Default.useAccount || !ImgurAPI.HasBeenAuthorized();
            var anonymous = _Anonymous;
            if (Clipboard.ContainsImage())
            {
                clipboardImage = Clipboard.GetImage();
                this.ShowBalloonTip(4000, "Hold on...", "Attempting to upload image to Imgur...", ToolTipIcon.None);
                resp = ImgurAPI.UploadImage(clipboardImage, this.GetTitleString(null), this.GetDescriptionString(null), _Anonymous);
            }
            else if (Clipboard.ContainsText())
            {
                clipboardUrl = Clipboard.GetText(TextDataFormat.UnicodeText);
                Uri uriResult;
                if (Uri.TryCreate(clipboardUrl, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                {
                    this.ShowBalloonTip(4000, "Hold on...", "Attempting to upload image to Imgur...", ToolTipIcon.None);
                    resp = ImgurAPI.UploadImage(clipboardUrl, this.GetTitleString(null), this.GetDescriptionString(null), _Anonymous);
                }
                else
                {
                    this.ShowBalloonTip(2000, "Can't upload clipboard!", "There's text on the clipboard but it's not a valid URL", ToolTipIcon.Error, true);
                    return;
                }
            }
            else
            {
                this.ShowBalloonTip(2000, "Can't upload clipboard!", "There's no image or URL there", ToolTipIcon.Error, true);
                return;
            }
            if (resp.Success)
            {
                // this doesn't need an invocation guard because this function can't be called from the context menu
                if (Settings.Default.copyLinks)
                {
                    Clipboard.SetText(Settings.Default.copyHttpsLinks
                        ? resp.ResponseData.Link.Replace("http://", "https://")
                        : resp.ResponseData.Link);
                }

                this.ShowBalloonTip(2000, "Success!", Settings.Default.copyLinks ? "Link copied to clipboard" : "Upload placed in history: " + resp.ResponseData.Link, ToolTipIcon.None);

                var item = new HistoryItem();
                item.Timestamp = DateTime.Now;
                item.Id = resp.ResponseData.Id;
                item.Link = resp.ResponseData.Link;
                item.Deletehash = resp.ResponseData.DeleteHash;
                item.Title = resp.ResponseData.Title;
                item.Description = resp.ResponseData.Description;
                item.Anonymous = anonymous;
                if (clipboardImage != null)
                    item.Thumbnail = clipboardImage.GetThumbnailImage(this.pictureBoxHistoryThumb.Width, this.pictureBoxHistoryThumb.Height, null, IntPtr.Zero);
                History.StoreHistoryItem(item);
            }
            else
                this.ShowBalloonTip(2000, "Failed", "Could not upload image (" + resp.Status + "):", ToolTipIcon.None, true);

            if (!Settings.Default.clearClipboardOnUpload)
            {
                if (clipboardImage != null)
                    Clipboard.SetImage(clipboardImage);
                else
                    Clipboard.SetText(clipboardUrl, TextDataFormat.UnicodeText);
            }

            Statistics.GatherAndSend();
        }

        private void UploadAlbum(bool anonymous, string[] paths, string albumTitle)
        {
            this.ShowBalloonTip(2000, "Hold on...", "Attempting to upload album to Imgur (this may take a while)...", ToolTipIcon.None);
            var images = new List<Image>();
            var titles = new List<string>();
            var descriptions = new List<string>();
            var i = 0;
            foreach (var path in paths)
            {
                try
                {
                    images.Add(Image.FromStream(new MemoryStream(File.ReadAllBytes(path))));
                    //ìmages.Add(System.Drawing.Image.FromStream(stream));
                    var title = string.Empty;
                    var description = string.Empty;

                    var formatContext = new FormattingHelper.FormattingContext();
                    formatContext.FilePath = path;
                    formatContext.AlbumIndex = ++i;
                    titles.Add(this.GetTitleString(formatContext));
                    descriptions.Add(this.GetDescriptionString(formatContext));
                }
                catch (ArgumentException)
                {
                    this.ShowBalloonTip(2000, "Failed", "File (" + path + ") is not a valid image file.", ToolTipIcon.Error, true);
                }
                catch (FileNotFoundException)
                {
                    this.ShowBalloonTip(2000, "Failed", "Could not find image file on disk (" + path + "):", ToolTipIcon.Error, true);
                }
                catch (IOException)
                {
                    this.ShowBalloonTip(2000, "Failed", "Image is in use by another program (" + path + "):", ToolTipIcon.Error, true);
                }
            }
            if (images.Count == 0)
            {
                Log.Error("Album upload failed: No valid images in selected images!");
                this.ShowBalloonTip(2000, "Failed", "Album upload cancelled: No valid images to upload!", ToolTipIcon.Error, true);
                return;
            }
            var response = ImgurAPI.UploadAlbum(images.ToArray(), albumTitle, anonymous, titles.ToArray(), descriptions.ToArray());
            if (response.Success)
            {
                // clipboard calls can only be made on an STA thread, threading model is MTA when invoked from context menu
                if (System.Threading.Thread.CurrentThread.GetApartmentState() != System.Threading.ApartmentState.STA)
                {
                    this.Invoke(new Action(() =>
                        Clipboard.SetText(Settings.Default.copyHttpsLinks
                            ? response.ResponseData.Link.Replace("http://", "https://")
                            : response.ResponseData.Link)));
                }
                else
                {
                    Clipboard.SetText(Settings.Default.copyHttpsLinks
                        ? response.ResponseData.Link.Replace("http://", "https://")
                        : response.ResponseData.Link);
                }

                this.ShowBalloonTip(2000, "Success!", Settings.Default.copyLinks ? "Link copied to clipboard" : "Upload placed in history: " + response.ResponseData.Link, ToolTipIcon.None);

                var item = new HistoryItem();
                item.Timestamp = DateTime.Now;
                item.Id = response.ResponseData.Id;
                item.Link = response.ResponseData.Link;
                item.Deletehash = response.ResponseData.DeleteHash;
                item.Title = response.ResponseData.Title;
                item.Description = response.ResponseData.Description;
                item.Anonymous = anonymous;
                item.Album = true;
                item.Thumbnail = response.CoverImage.GetThumbnailImage(this.pictureBoxHistoryThumb.Width, this.pictureBoxHistoryThumb.Height, null, IntPtr.Zero);
                this.Invoke(new Action(() => History.StoreHistoryItem(item)));
            }
            else
                this.ShowBalloonTip(2000, "Failed", "Could not upload album (" + response.Status + "): " + response.ResponseData.Error, ToolTipIcon.None, true);

            Statistics.GatherAndSend();
        }

        private void UploadFile(bool anonymous, string[] paths = null)
        {
            if (paths == null)
            {
                var dialog = new OpenFileDialog();
                dialog.Multiselect = true;
                var res = dialog.ShowDialog();
                if (res == DialogResult.OK)
                    paths = dialog.FileNames;
            }
            if (paths != null)
            {
                if (paths.Length > 1 && Settings.Default.uploadMultipleImagesAsGallery)
                {
                    this.UploadAlbum(anonymous, paths, "");
                    return;
                }

                var success = 0;
                var failure = 0;
                var i = 0;
                foreach (var fileName in paths)
                {
                    ++i;

                    if (fileName == null || fileName == string.Empty)
                        continue;

                    var fileCounterString = (paths.Length > 1) ? (" (" + i.ToString() + "/" + paths.Length.ToString() + ") ") : (string.Empty);

                    try
                    {
                        this.ShowBalloonTip(2000, "Hold on..." + fileCounterString, "Attempting to upload image to Imgur...", ToolTipIcon.None);
                        Image img;
                        APIResponses.ImageResponse resp;
                        using (var stream = File.Open(fileName, FileMode.Open))
                        {
                            // a note to the future: ImgurAPI.UploadImage called Image.Save(); Image.Save()
                            // requires that the stream it was loaded from still be open, or else you get
                            // an immensely generic error. 
                            img = Image.FromStream(stream);
                            var formatContext = new FormattingHelper.FormattingContext();
                            formatContext.FilePath = fileName;
                            resp = ImgurAPI.UploadImage(img, this.GetTitleString(formatContext), this.GetDescriptionString(formatContext), anonymous);
                        }
                        if (resp.Success)
                        {
                            success++;

                            if (Settings.Default.copyLinks)
                            {
                                // clipboard calls can only be made on an STA thread, threading model is MTA when invoked from context menu
                                if (System.Threading.Thread.CurrentThread.GetApartmentState() !=
                                    System.Threading.ApartmentState.STA)
                                {
                                    this.Invoke(new Action(() =>
                                        Clipboard.SetText(Settings.Default.copyHttpsLinks
                                            ? resp.ResponseData.Link.Replace("http://", "https://")
                                            : resp.ResponseData.Link)));
                                }
                                else
                                {
                                    Clipboard.SetText(Settings.Default.copyHttpsLinks
                                        ? resp.ResponseData.Link.Replace("http://", "https://")
                                        : resp.ResponseData.Link);
                                }
                            }

                            this.ShowBalloonTip(2000, "Success!" + fileCounterString, Settings.Default.copyLinks ? "Link copied to clipboard" : "Upload placed in history: " + resp.ResponseData.Link, ToolTipIcon.None);

                            var item = new HistoryItem();
                            item.Timestamp = DateTime.Now;
                            item.Id = resp.ResponseData.Id;
                            item.Link = resp.ResponseData.Link;
                            item.Deletehash = resp.ResponseData.DeleteHash;
                            item.Title = resp.ResponseData.Title;
                            item.Description = resp.ResponseData.Description;
                            item.Anonymous = anonymous;
                            item.Thumbnail = img.GetThumbnailImage(this.pictureBoxHistoryThumb.Width, this.pictureBoxHistoryThumb.Height, null, IntPtr.Zero);
                            this.Invoke(new Action(() => History.StoreHistoryItem(item)));
                        }
                        else
                        {
                            failure++;
                            this.ShowBalloonTip(2000, "Failed" + fileCounterString, "Could not upload image (" + resp.Status + "): " + resp.ResponseData.Error, ToolTipIcon.None, true);
                        }
                    }
                    catch (ArgumentException)
                    {
                        this.ShowBalloonTip(2000, "Failed" + fileCounterString, "File (" + fileName + ") is not a valid image file.", ToolTipIcon.Error, true);
                    }
                    catch (FileNotFoundException)
                    {
                        failure++;
                        this.ShowBalloonTip(2000, "Failed" + fileCounterString, "Could not find image file on disk (" + fileName + "):", ToolTipIcon.Error, true);
                    }
                    catch (IOException)
                    {
                        this.ShowBalloonTip(2000, "Failed" + fileCounterString, "Image is in use by another program (" + fileName + "):", ToolTipIcon.Error, true);
                    }
                }
                if (paths.Length > 1)
                {
                    this.ShowBalloonTip(2000, "Done", "Successfully uploaded " + success.ToString() + " files" + ((failure > 0) ? (" (Warning: " + failure.ToString() + " failed)") : (string.Empty)), ToolTipIcon.Info, failure > 0);
                }
            }

            Statistics.GatherAndSend();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.SaveSettings();
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
            this.tabControl1.SelectedIndex = 2;
            this.listBoxHistory.SelectedIndex = this.listBoxHistory.Items.Count - 1;
            this.BringToFront();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Settings.Default.Reload();

            // Assign control values. Most values are set using application binding on the control.
            this.comboBoxImageFormat.SelectedIndex = Settings.Default.imageFormat;

            // Check the registry for a key describing whether EasyImgur should be started on boot.
            if (!Program.InPortableMode)
            {
                var registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                var value = (string)registryKey.GetValue("EasyImgur", string.Empty); // string.Empty is returned if no key is present.
                this.checkBoxLaunchAtBoot.Checked = value != string.Empty;
                if (value != string.Empty && value != this.QuotedApplicationPath)
                {
                    // A key exists, make sure we're using the most up-to-date path!
                    registryKey.SetValue("EasyImgur", this.QuotedApplicationPath);
                    this.ShowBalloonTip(2000, "EasyImgur", "Updated registry path", ToolTipIcon.Info);
                }
                this.UpdateRegistry(true); // this will need to be updated too, if we're using it
            }

            // Bind the data source for the list of contributors.
            Contributors.BindingSource.DataSource = Contributors.ContributorList;
            this.contributorsList.DataSource = Contributors.BindingSource.DataSource;

            this.ProxyAddress.Text = Settings.Default.ProxyAddress ?? string.Empty;
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.SaveSettings();
            this.Hide();

            e.Cancel = !this.closeCommandWasSentFromExitButton;  // Don't want to *actually* close the form unless the Exit button was used.
        }

        private void SaveSettings()
        {
            // Store control values. Most values are stored using application binding on the control.
            Settings.Default.imageFormat = this.comboBoxImageFormat.SelectedIndex;

            if (!Program.InPortableMode)
            {
                using (var registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    if (this.checkBoxLaunchAtBoot.Checked)
                    {
                        // If the checkbox was marked, set a value which will make EasyImgur start at boot.
                        registryKey.SetValue("EasyImgur", this.QuotedApplicationPath);
                    }
                    else
                    {
                        // Delete our value if one is present; second argument supresses exception on missing value
                        registryKey.DeleteValue("EasyImgur", false);
                    }
                }

                this.UpdateRegistry(false);
            }

            var proxyAddress = this.ProxyAddress.Text;
            Settings.Default.ProxyAddress = !string.IsNullOrWhiteSpace(proxyAddress) &&
                Regex.Match(proxyAddress, "^\\d{1,3}.\\d{1,3}.\\d{1,3}.\\d{1,3}:\\d{1,5}$").Success
                ? proxyAddress
                : null;
            ImgurAPI.WebClientFactory.ReloadSetting();
            Settings.Default.Save();
        }

        private void UpdateRegistry(bool loadingSettings)
        {
            if (loadingSettings) // update enableContextMenu if necessary
            {
                // look for one of our handlers; Directory is the easiest and most reliable
                // Since HKCR is a merging of HKCU\Software\Classes and HKLM\Software\Classes, look there
                using (var dir = Registry.ClassesRoot.OpenSubKey("Directory"))
                using (var shell = dir.OpenSubKey("shell"))
                    Settings.Default.enableContextMenu = shell != null && shell.OpenSubKey("imguruploadanonymous") != null;
            }

            // a note: Directory doesn't work if within SystemFileAssociations, and 
            // the extensions don't work if not inside them. At least, this seems to be the case for me

            // another note: I discovered that I had the logic flipped, and the code actually did the opposite
            // of what I describe in the above note, and it was working. Earlier, though, when I wrote that,
            // it seemed to be true. Either way, the placement (inside or outside of SystemFileAssociations)
            // does affect where in the context menu they show up. Feel free to play with the placement and see
            // if you can get it to work better.
            var fileTypes = new[] { ".jpg", ".jpeg", ".png", ".apng", ".bmp",
            ".gif", ".tiff", ".tif", ".pdf", ".xcf", "Directory" };
            using (var root = Registry.CurrentUser.OpenSubKey("Software\\Classes", true))
            using (var fileAssoc = root.CreateSubKey("SystemFileAssociations"))
                foreach (var fileType in fileTypes)
                    using (var fileTypeKey = (fileType != "Directory" ? fileAssoc.CreateSubKey(fileType) : root.CreateSubKey(fileType)))
                    using (var shell = fileTypeKey.CreateSubKey("shell"))
                    {
                        if (Settings.Default.enableContextMenu)
                        {
                            using (var anonHandler = shell.CreateSubKey("imguruploadanonymous"))
                                this.EnableContextMenu(anonHandler, "Upload to Imgur" +
                                    (fileType == "Directory" ? " as album" : "") + " (anonymous)", true);
                            using (var accHandler = shell.CreateSubKey("imgurupload"))
                                this.EnableContextMenu(accHandler, "Upload to Imgur" +
                                    (fileType == "Directory" ? " as album" : ""), false);
                        }
                        else
                        {
                            shell.DeleteSubKeyTree("imguruploadanonymous", false);
                            shell.DeleteSubKeyTree("imgurupload", false);
                        }
                    }
        }

        private void EnableContextMenu(RegistryKey key, string commandText, bool anonymous)
        {
            key.SetValue("", commandText);
            key.SetValue("Icon", this.QuotedApplicationPath);
            using (var subKey = key.CreateSubKey("command"))
                subKey.SetValue("", this.QuotedApplicationPath + (anonymous ? " /anonymous" : "") + " \"%1\"");
        }

        private void buttonChangeCredentials_Click(object sender, EventArgs e)
        {
            var accountCredentialsForm = new AuthorizeForm();
            var res = accountCredentialsForm.ShowDialog(this);

            if (ImgurAPI.HasBeenAuthorized())
            {
                this.buttonForceTokenRefresh.Enabled = true;
            }
            else
            {
                this.buttonForceTokenRefresh.Enabled = false;
            }
        }

        private void SelectedHistoryItemChanged()
        {
            this.groupBoxHistorySelection.Text =
                String.Format("Selection: {0} {1}", this.listBoxHistory.SelectedItems.Count, this.listBoxHistory.SelectedItems.Count == 1 ? "item" : "items");
            var item = this.listBoxHistory.SelectedItem as HistoryItem;
            if (item == null)
            {
                this.buttonRemoveFromImgur.Enabled = false;
                this.buttonRemoveFromHistory.Enabled = false;
                this.btnOpenImageLinkInBrowser.Enabled = false;
            }
            else
            {
                this.buttonRemoveFromImgur.Enabled = item.Anonymous || (!item.Anonymous && ImgurAPI.HasBeenAuthorized());
                this.buttonRemoveFromHistory.Enabled = true;
                this.btnOpenImageLinkInBrowser.Enabled = true;
            }
        }

        private void listBoxHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SelectedHistoryItemChanged();
        }

        private string FormatInfoString(string input, FormattingHelper.FormattingContext formattingContext)
        {
            return FormattingHelper.Format(input, formattingContext);
        }

        private string GetTitleString(FormattingHelper.FormattingContext formattingContext)
        {
            return this.FormatInfoString(this.textBoxTitleFormat.Text, formattingContext);
        }

        private string GetDescriptionString(FormattingHelper.FormattingContext formattingContext)
        {
            return this.FormatInfoString(this.textBoxDescriptionFormat.Text, formattingContext);
        }

        private void buttonRemoveFromImgur_Click(object sender, EventArgs e)
        {
            var count = this.listBoxHistory.SelectedItems.Count;
            var isMultipleImages = count > 1;
            var currentCount = 0;

            this.listBoxHistory.BeginUpdate();
            var selectedItems = new List<HistoryItem>(this.listBoxHistory.SelectedItems.Cast<HistoryItem>());
            foreach (var item in selectedItems)
            {
                ++currentCount;

                if (item == null)
                    return;

                var balloonImageCounterText = (isMultipleImages ? (currentCount.ToString() + "/" + count.ToString()) : string.Empty);
                var balloonText = "Attempting to remove " + (item.Album ? "album" : "image") + " " + balloonImageCounterText + " from Imgur...";
                this.ShowBalloonTip(2000, "Hold on...", balloonText, ToolTipIcon.None);
                if (item.Album ? ImgurAPI.DeleteAlbum(item.Deletehash, item.Anonymous) : ImgurAPI.DeleteImage(item.Deletehash, item.Anonymous))
                {
                    this.ShowBalloonTip(2000, "Success!", "Removed " + (item.Album ? "album" : "image") + " " + balloonImageCounterText + " from Imgur and history", ToolTipIcon.None);
                    History.RemoveHistoryItem(item);
                }
                else
                    this.ShowBalloonTip(2000, "Failed", "Failed to remove " + (item.Album ? "album" : "image") + " " + balloonImageCounterText + " from Imgur", ToolTipIcon.Error);
            }
            this.listBoxHistory.EndUpdate();

            Statistics.GatherAndSend();
        }

        private void buttonForceTokenRefresh_Click(object sender, EventArgs e)
        {
            ImgurAPI.ForceRefreshTokens();
            this.SetAuthorizationStatusUI(ImgurAPI.HasBeenAuthorized());
        }

        private void buttonFormatHelp_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder(
                "You can use strings consisting of either static characters or " +
                "the following dynamic symbols, or a combination of both:\n\n");
            foreach (var scheme in FormattingHelper.GetSchemes())
            {
                sb.Append(scheme.Symbol);
                sb.Append("  :  ");
                sb.Append(scheme.Description);
                sb.Append('\n');
            }

            const string exampleFormattedString = "Image_%date%_%time%";

            sb.Append("\n\nEx.: '");
            sb.Append(exampleFormattedString);
            sb.Append("' would become: '");
            sb.Append(FormattingHelper.Format(exampleFormattedString, null));

            var loc = this.Location;
            loc.Offset(this.buttonFormatHelp.Location.X, this.buttonFormatHelp.Location.Y);
            Help.ShowPopup(this, sb.ToString(), loc);
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
            this.listBoxHistory.BeginUpdate();
            var selectedItems = new List<HistoryItem>(this.listBoxHistory.SelectedItems.Cast<HistoryItem>());
            foreach (var item in selectedItems)
            {
                if (item == null)
                {
                    return;
                }

                History.RemoveHistoryItem(item);
            }
            this.listBoxHistory.EndUpdate();
        }

        private void uploadClipboardAccountTrayMenuItem_Click(object sender, EventArgs e)
        {
            this.UploadClipboard(false);
        }

        private void uploadFileAccountTrayMenuItem_Click(object sender, EventArgs e)
        {
            this.UploadFile(false);
        }

        private void uploadClipboardAnonymousTrayMenuItem_Click(object sender, EventArgs e)
        {
            this.UploadClipboard(true);
        }

        private void uploadFileAnonymousTrayMenuItem_Click(object sender, EventArgs e)
        {
            this.UploadFile(true);
        }

        private void settingsTrayMenuItem_Click(object sender, EventArgs e)
        {
            // Open settings form.
            this.Show();
        }

        private void exitTrayMenuItem_Click(object sender, EventArgs e)
        {
            this.closeCommandWasSentFromExitButton = true;
            Application.Exit();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (Process.Start("https://github.com/Cologler/EasyImgur")) { }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://bryankeiren.com/");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://imgur.com/account/settings/apps");
        }

        private void btnOpenImageLinkInBrowser_Click(object sender, EventArgs e)
        {
            var item = this.listBoxHistory.SelectedItem as HistoryItem;
            if (item != null)
            {
                Process.Start(item.Link);
            }
        }

        private void checkBoxCopyLinks_CheckedChanged(object sender, EventArgs e)
        {
            this.clipboardSettingsContainer.Enabled = this.checkBoxCopyLinks.Checked;
        }
    }
}
