using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EasyImgur
{
    public partial class Form1 : Form
    {
        private bool CloseCommandWasSentFromExitButton = false;

        public Form1()
        {
            InitializeComponent();
            
            Application.ApplicationExit += new System.EventHandler(this.ApplicationExit);

            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_Closing);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.VisibleChanged += new System.EventHandler(this.Form1_VisibleChanged);
            notifyIcon1.BalloonTipClicked += new System.EventHandler(this.NotifyIcon1_BalloonTipClicked);
            notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon1_MouseDoubleClick);
            tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);

            ImgurAPI.obtainedAuthorization += new ImgurAPI.AuthorizationEventHandler(this.ObtainedAPIAuthorization);
            ImgurAPI.refreshedAuthorization += new ImgurAPI.AuthorizationEventHandler(this.ObtainedAPIAuthorization);
            ImgurAPI.lostAuthorization += new ImgurAPI.AuthorizationEventHandler(this.LostAPIAuthorization);

            notifyIcon1.ShowBalloonTip(2000, "EasyImgur", "Right-click EasyImgur's icon in the tray to use it!", ToolTipIcon.Info);

            ImgurAPI.AttemptRefreshTokensFromDisk();
        }

        private void ApplicationExit( object sender, EventArgs e )
        {
            ImgurAPI.OnMainThreadExit();
        }

        private void ObtainedAPIAuthorization()
        {
            uploadClipboardToolStripMenuItem.Enabled = true;
            uploadFromFileToolStripMenuItem.Enabled = true;
            label13.Text = "Authorized";
            label13.ForeColor = System.Drawing.Color.Green;
            buttonForceTokenRefresh.Enabled = true;
            buttonForgetTokens.Enabled = true;
            notifyIcon1.ShowBalloonTip(2000, "EasyImgur", "EasyImgur has received authorization to use your Imgur account!", ToolTipIcon.Info);
        }

        private void LostAPIAuthorization()
        {
            uploadClipboardToolStripMenuItem.Enabled = false;
            uploadFromFileToolStripMenuItem.Enabled = false;
            label13.Text = "Not authorized";
            label13.ForeColor = System.Drawing.Color.DarkBlue;
            buttonForceTokenRefresh.Enabled = false;
            buttonForgetTokens.Enabled = false;
            notifyIcon1.ShowBalloonTip(2000, "EasyImgur", "EasyImgur no longer has authorization to use your Imgur account!", ToolTipIcon.Info);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 2)
            {
                SelectedHistoryItemChanged();
            }
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseCommandWasSentFromExitButton = true;
            Application.Exit();
        }

        private void uploadClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UploadClipboard(false);
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

            if (Properties.Settings.Default.copyLinks)
            {
                Clipboard.SetText(resp.data.link);
            }
            if (resp.success)
            {
                notifyIcon1.ShowBalloonTip(2000, "Success!", Properties.Settings.Default.copyLinks ? "Link copied to clipboard" : "Upload placed in history: " + resp.data.link, ToolTipIcon.None);

                HistoryItem item = new HistoryItem();
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
                listBoxHistory.Items.Add(item);
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

        private void uploadFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UploadFile(false);
        }

        private void UploadFile( bool _Anonymous )
        {
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult res = dialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                using (System.IO.Stream stream = dialog.OpenFile())
                {
                    Image img = System.Drawing.Image.FromStream(stream);
                    notifyIcon1.ShowBalloonTip(2000, "Hold on...", "Attempting to upload image to Imgur...", ToolTipIcon.None);
                    APIResponses.ImageResponse resp = ImgurAPI.UploadImage(img, GetTitleString(), GetDescriptionString(), _Anonymous);
                    if (Properties.Settings.Default.copyLinks)
                    {
                        Clipboard.SetText(resp.data.link);
                    }
                    if (resp.success)
                    {
                        notifyIcon1.ShowBalloonTip(2000, "Success!", Properties.Settings.Default.copyLinks ? "Link copied to clipboard" : "Upload placed in history: " + resp.data.link, ToolTipIcon.None);

                        HistoryItem item = new HistoryItem();
                        item.id = resp.data.id;
                        item.link = resp.data.link;
                        item.deletehash = resp.data.deletehash;
                        item.title = resp.data.title;
                        item.description = resp.data.description;
                        item.thumbnail = img.GetThumbnailImage(pictureBox1.Width, pictureBox1.Height, null, System.IntPtr.Zero);
                        listBoxHistory.Items.Add(item);
                    }
                    else
                    {
                        notifyIcon1.ShowBalloonTip(2000, "Failed", "Could not upload image (" + resp.status + "):", ToolTipIcon.None);
                    }
                }
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Open settings form.
            this.Show();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            SaveSettings();
            //this.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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

            // Assign control values.
            checkBoxClearClipboard.Checked = Properties.Settings.Default.clearClipboardOnUpload;
            checkBoxCopyLinks.Checked = Properties.Settings.Default.copyLinks;
            textBoxTitleFormat.Text = Properties.Settings.Default.titleFormat;
            textBoxDescriptionFormat.Text = Properties.Settings.Default.descriptionFormat;
            comboBoxImageFormat.SelectedIndex = Properties.Settings.Default.imageFormat;


            // Check the registry for a key describing whether EasyImgur should be started on boot.
            Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            string value = (string)registryKey.GetValue("EasyImgur", string.Empty); // string.Empty is returned if no key is present.
            checkBoxLaunchAtBoot.Checked = value != string.Empty;
            if (value != string.Empty && value != Application.ExecutablePath)
            {
                // A key exists, make sure we're using the most up-to-date path!
                registryKey.SetValue("EasyImgur", Application.ExecutablePath);
                notifyIcon1.ShowBalloonTip(2000, "EasyImgur", "Updated registry path", ToolTipIcon.Info);
            }
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
            // Store control values.
            Properties.Settings.Default.clearClipboardOnUpload = checkBoxClearClipboard.Checked;
            Properties.Settings.Default.copyLinks = checkBoxCopyLinks.Checked;
            Properties.Settings.Default.titleFormat = textBoxTitleFormat.Text;
            Properties.Settings.Default.descriptionFormat = textBoxDescriptionFormat.Text;
            Properties.Settings.Default.imageFormat = comboBoxImageFormat.SelectedIndex;

            Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (checkBoxLaunchAtBoot.Checked)
            {
                // If the checkbox was marked, set a value which will make EasyImgur start at boot.
                registryKey.SetValue("EasyImgur", Application.ExecutablePath);
            }
            else
            {
                try
                {
                    // Delete our value if one is present.
                    registryKey.DeleteValue("EasyImgur");
                }
                catch (ArgumentException ex)
                {
                    // Don't care at all.
                }
            }

            Properties.Settings.Default.Save();
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
            if (item != null)
            {
                textBoxID.Text = item.id;
                textBoxLink.Text = item.link;
                textBoxDeleteHash.Text = item.deletehash;
                pictureBox1.Image = item.thumbnail;
                checkBoxTiedToAccount.Checked = !item.anonymous;

                buttonRemoveFromImgur.Enabled = !(!item.anonymous && !ImgurAPI.HasBeenAuthorized());
            }
            else
            {
                textBoxID.Text = string.Empty;
                textBoxLink.Text = string.Empty;
                textBoxDeleteHash.Text = string.Empty;
                pictureBox1.Image = null;
                checkBoxTiedToAccount.Checked = false;

                buttonRemoveFromImgur.Enabled = false;
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
            HistoryItem item = listBoxHistory.SelectedItem as HistoryItem;
            if (item == null)
            {
                return;
            }

            notifyIcon1.ShowBalloonTip(2000, "Hold on...", "Attempting to remove image from Imgur...", ToolTipIcon.None);
            if (ImgurAPI.DeleteImage(item.deletehash, item.anonymous))
            {
                listBoxHistory.Items.Remove(item);
                notifyIcon1.ShowBalloonTip(2000, "Success!", "Removed image from Imgur and history", ToolTipIcon.None);
            }
            else
            {
                notifyIcon1.ShowBalloonTip(2000, "Failed", "Failed to remove image from Imgur", ToolTipIcon.Error);
            }

            listBoxHistory.SelectedItem = null;
            listBoxHistory_SelectedIndexChanged(null, null);
        }

        private void buttonForceTokenRefresh_Click(object sender, EventArgs e)
        {
            ImgurAPI.ForceRefreshTokens();
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

        private void uploadClipboardAnonymousToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UploadClipboard(true);
        }

        private void uploadFromFileAnonymousToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UploadFile(true);
        }

        private void buttonForgetTokens_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to discard the tokens?\n\nWithout tokens, the app can no longer use your Imgur account.", "Forget tokens", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ImgurAPI.ForgetTokens();
                MessageBox.Show("Tokens have been forgotten. Remember that the app has still technically been authorized on the Imgur website, we can't change this for you!\n\nGo to http://imgur.com/account/settings/apps to revoke access.", "Tokens discarded", MessageBoxButtons.OK);
            }
        }
    }
}
