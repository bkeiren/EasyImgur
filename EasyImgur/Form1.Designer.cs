namespace EasyImgur
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.buttonApplyGeneral = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.clipboardSettingsContainer = new System.Windows.Forms.GroupBox();
            this.checkBoxCopyHttpsLinks = new System.Windows.Forms.CheckBox();
            this.checkBoxClearClipboard = new System.Windows.Forms.CheckBox();
            this.labelPortableModeNote = new System.Windows.Forms.Label();
            this.checkBoxCopyLinks = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.checkBoxGalleryUpload = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.checkBoxEnableContextMenu = new System.Windows.Forms.CheckBox();
            this.checkBoxShowTokenRefreshNotification = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBoxLaunchAtBoot = new System.Windows.Forms.CheckBox();
            this.buttonFormatHelp = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxImageFormat = new System.Windows.Forms.ComboBox();
            this.textBoxDescriptionFormat = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxTitleFormat = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.buttonForgetTokens = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.buttonForceTokenRefresh = new System.Windows.Forms.Button();
            this.buttonAuthorize = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label19 = new System.Windows.Forms.Label();
            this.checkBoxAlbum = new System.Windows.Forms.CheckBox();
            this.historyItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBoxHistorySelection = new System.Windows.Forms.GroupBox();
            this.buttonRemoveFromImgur = new System.Windows.Forms.Button();
            this.buttonRemoveFromHistory = new System.Windows.Forms.Button();
            this.btnOpenImageLinkInBrowser = new System.Windows.Forms.Button();
            this.textBoxTimestamp = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.checkBoxTiedToAccount = new System.Windows.Forms.CheckBox();
            this.textBoxDeleteHash = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBoxHistoryThumb = new System.Windows.Forms.PictureBox();
            this.textBoxID = new System.Windows.Forms.TextBox();
            this.textBoxLink = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.listBoxHistory = new System.Windows.Forms.ListBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.versionLabel = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.contributorsList = new System.Windows.Forms.ListBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.appDescriptionLabel = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.trayMenu = new System.Windows.Forms.ContextMenu();
            this.uploadClipboardAccountTrayMenuItem = new System.Windows.Forms.MenuItem();
            this.uploadFileAccountTrayMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.uploadClipboardAnonymousTrayMenuItem = new System.Windows.Forms.MenuItem();
            this.uploadFileAnonymousTrayMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.settingsTrayMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.exitTrayMenuItem = new System.Windows.Forms.MenuItem();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.clipboardSettingsContainer.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.historyItemBindingSource)).BeginInit();
            this.groupBoxHistorySelection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHistoryThumb)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "EasyImgur";
            this.notifyIcon1.Visible = true;
            // 
            // buttonApplyGeneral
            // 
            this.buttonApplyGeneral.Location = new System.Drawing.Point(500, 236);
            this.buttonApplyGeneral.Margin = new System.Windows.Forms.Padding(2);
            this.buttonApplyGeneral.Name = "buttonApplyGeneral";
            this.buttonApplyGeneral.Size = new System.Drawing.Size(81, 29);
            this.buttonApplyGeneral.TabIndex = 11;
            this.buttonApplyGeneral.Text = "Apply";
            this.buttonApplyGeneral.UseVisualStyleBackColor = true;
            this.buttonApplyGeneral.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(2, 2);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(595, 297);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.clipboardSettingsContainer);
            this.tabPage1.Controls.Add(this.labelPortableModeNote);
            this.tabPage1.Controls.Add(this.checkBoxCopyLinks);
            this.tabPage1.Controls.Add(this.label17);
            this.tabPage1.Controls.Add(this.checkBoxGalleryUpload);
            this.tabPage1.Controls.Add(this.label16);
            this.tabPage1.Controls.Add(this.checkBoxEnableContextMenu);
            this.tabPage1.Controls.Add(this.checkBoxShowTokenRefreshNotification);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.checkBoxLaunchAtBoot);
            this.tabPage1.Controls.Add(this.buttonFormatHelp);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.comboBoxImageFormat);
            this.tabPage1.Controls.Add(this.textBoxDescriptionFormat);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.textBoxTitleFormat);
            this.tabPage1.Controls.Add(this.buttonApplyGeneral);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(587, 271);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // clipboardSettingsContainer
            // 
            this.clipboardSettingsContainer.Controls.Add(this.checkBoxCopyHttpsLinks);
            this.clipboardSettingsContainer.Controls.Add(this.checkBoxClearClipboard);
            this.clipboardSettingsContainer.Enabled = global::EasyImgur.Properties.Settings.Default.copyLinks;
            this.clipboardSettingsContainer.Location = new System.Drawing.Point(204, 7);
            this.clipboardSettingsContainer.Name = "clipboardSettingsContainer";
            this.clipboardSettingsContainer.Size = new System.Drawing.Size(378, 63);
            this.clipboardSettingsContainer.TabIndex = 23;
            this.clipboardSettingsContainer.TabStop = false;
            this.clipboardSettingsContainer.Text = "Clipboard";
            // 
            // checkBoxCopyHttpsLinks
            // 
            this.checkBoxCopyHttpsLinks.AutoSize = true;
            this.checkBoxCopyHttpsLinks.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxCopyHttpsLinks.Checked = global::EasyImgur.Properties.Settings.Default.copyHttpsLinks;
            this.checkBoxCopyHttpsLinks.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::EasyImgur.Properties.Settings.Default, "copyHttpsLinks", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxCopyHttpsLinks.Location = new System.Drawing.Point(5, 18);
            this.checkBoxCopyHttpsLinks.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxCopyHttpsLinks.Name = "checkBoxCopyHttpsLinks";
            this.checkBoxCopyHttpsLinks.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxCopyHttpsLinks.Size = new System.Drawing.Size(113, 17);
            this.checkBoxCopyHttpsLinks.TabIndex = 22;
            this.checkBoxCopyHttpsLinks.Text = "Copy HTTPS links";
            this.checkBoxCopyHttpsLinks.UseVisualStyleBackColor = true;
            // 
            // checkBoxClearClipboard
            // 
            this.checkBoxClearClipboard.AutoSize = true;
            this.checkBoxClearClipboard.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxClearClipboard.Checked = global::EasyImgur.Properties.Settings.Default.clearClipboardOnUpload;
            this.checkBoxClearClipboard.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxClearClipboard.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::EasyImgur.Properties.Settings.Default, "clearClipboardOnUpload", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxClearClipboard.Location = new System.Drawing.Point(5, 39);
            this.checkBoxClearClipboard.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxClearClipboard.Name = "checkBoxClearClipboard";
            this.checkBoxClearClipboard.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxClearClipboard.Size = new System.Drawing.Size(203, 17);
            this.checkBoxClearClipboard.TabIndex = 1;
            this.checkBoxClearClipboard.Text = "Clear clipboard immediately on upload";
            this.checkBoxClearClipboard.UseVisualStyleBackColor = true;
            // 
            // labelPortableModeNote
            // 
            this.labelPortableModeNote.AutoSize = true;
            this.labelPortableModeNote.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.labelPortableModeNote.Location = new System.Drawing.Point(15, 247);
            this.labelPortableModeNote.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPortableModeNote.Name = "labelPortableModeNote";
            this.labelPortableModeNote.Size = new System.Drawing.Size(251, 13);
            this.labelPortableModeNote.TabIndex = 21;
            this.labelPortableModeNote.Text = "NOTE: Some options are disabled in portable mode.";
            // 
            // checkBoxCopyLinks
            // 
            this.checkBoxCopyLinks.AutoSize = true;
            this.checkBoxCopyLinks.Checked = global::EasyImgur.Properties.Settings.Default.copyLinks;
            this.checkBoxCopyLinks.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::EasyImgur.Properties.Settings.Default, "copyLinks", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxCopyLinks.Location = new System.Drawing.Point(67, 25);
            this.checkBoxCopyLinks.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxCopyLinks.Name = "checkBoxCopyLinks";
            this.checkBoxCopyLinks.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxCopyLinks.Size = new System.Drawing.Size(132, 17);
            this.checkBoxCopyLinks.TabIndex = 2;
            this.checkBoxCopyLinks.Text = "Copy links to clipboard";
            this.checkBoxCopyLinks.UseVisualStyleBackColor = true;
            this.checkBoxCopyLinks.CheckedChanged += new System.EventHandler(this.checkBoxCopyLinks_CheckedChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label17.Location = new System.Drawing.Point(204, 80);
            this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(279, 13);
            this.label17.TabIndex = 20;
            this.label17.Text = "Only used when multiple files are selected in the file dialog";
            // 
            // checkBoxGalleryUpload
            // 
            this.checkBoxGalleryUpload.AutoSize = true;
            this.checkBoxGalleryUpload.Checked = global::EasyImgur.Properties.Settings.Default.uploadMultipleImagesAsGallery;
            this.checkBoxGalleryUpload.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxGalleryUpload.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::EasyImgur.Properties.Settings.Default, "uploadMultipleImagesAsGallery", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxGalleryUpload.Location = new System.Drawing.Point(18, 79);
            this.checkBoxGalleryUpload.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxGalleryUpload.Name = "checkBoxGalleryUpload";
            this.checkBoxGalleryUpload.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxGalleryUpload.Size = new System.Drawing.Size(181, 17);
            this.checkBoxGalleryUpload.TabIndex = 3;
            this.checkBoxGalleryUpload.Text = "Upload multiple images as gallery";
            this.checkBoxGalleryUpload.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label16.Location = new System.Drawing.Point(204, 216);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(305, 13);
            this.label16.TabIndex = 18;
            this.label16.Text = "Moving the .exe requires one manual launch to restore the path";
            // 
            // checkBoxEnableContextMenu
            // 
            this.checkBoxEnableContextMenu.AutoSize = true;
            this.checkBoxEnableContextMenu.Checked = global::EasyImgur.Properties.Settings.Default.enableContextMenu;
            this.checkBoxEnableContextMenu.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::EasyImgur.Properties.Settings.Default, "enableContextMenu", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxEnableContextMenu.Location = new System.Drawing.Point(73, 215);
            this.checkBoxEnableContextMenu.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxEnableContextMenu.Name = "checkBoxEnableContextMenu";
            this.checkBoxEnableContextMenu.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxEnableContextMenu.Size = new System.Drawing.Size(126, 17);
            this.checkBoxEnableContextMenu.TabIndex = 10;
            this.checkBoxEnableContextMenu.Text = "Enable context menu";
            this.checkBoxEnableContextMenu.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowTokenRefreshNotification
            // 
            this.checkBoxShowTokenRefreshNotification.AutoSize = true;
            this.checkBoxShowTokenRefreshNotification.Checked = global::EasyImgur.Properties.Settings.Default.showNotificationOnTokenRefresh;
            this.checkBoxShowTokenRefreshNotification.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::EasyImgur.Properties.Settings.Default, "showNotificationOnTokenRefresh", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxShowTokenRefreshNotification.Location = new System.Drawing.Point(12, 194);
            this.checkBoxShowTokenRefreshNotification.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxShowTokenRefreshNotification.Name = "checkBoxShowTokenRefreshNotification";
            this.checkBoxShowTokenRefreshNotification.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxShowTokenRefreshNotification.Size = new System.Drawing.Size(187, 17);
            this.checkBoxShowTokenRefreshNotification.TabIndex = 9;
            this.checkBoxShowTokenRefreshNotification.Text = "Show notification on token refresh";
            this.checkBoxShowTokenRefreshNotification.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label4.Location = new System.Drawing.Point(204, 174);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(305, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Moving the .exe requires one manual launch to restore the path";
            // 
            // checkBoxLaunchAtBoot
            // 
            this.checkBoxLaunchAtBoot.AutoSize = true;
            this.checkBoxLaunchAtBoot.Location = new System.Drawing.Point(3, 173);
            this.checkBoxLaunchAtBoot.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxLaunchAtBoot.Name = "checkBoxLaunchAtBoot";
            this.checkBoxLaunchAtBoot.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxLaunchAtBoot.Size = new System.Drawing.Size(196, 17);
            this.checkBoxLaunchAtBoot.TabIndex = 8;
            this.checkBoxLaunchAtBoot.Text = "Launch EasyImgur at Windows start";
            this.checkBoxLaunchAtBoot.UseVisualStyleBackColor = true;
            // 
            // buttonFormatHelp
            // 
            this.buttonFormatHelp.Location = new System.Drawing.Point(525, 124);
            this.buttonFormatHelp.Margin = new System.Windows.Forms.Padding(2);
            this.buttonFormatHelp.Name = "buttonFormatHelp";
            this.buttonFormatHelp.Size = new System.Drawing.Size(56, 46);
            this.buttonFormatHelp.TabIndex = 7;
            this.buttonFormatHelp.Text = "Format help";
            this.buttonFormatHelp.UseVisualStyleBackColor = true;
            this.buttonFormatHelp.Click += new System.EventHandler(this.buttonFormatHelp_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label8.Location = new System.Drawing.Point(328, 103);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label8.Size = new System.Drawing.Size(120, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Imgur may change this *";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(61, 103);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label7.Size = new System.Drawing.Size(120, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "* Preferred image format";
            // 
            // comboBoxImageFormat
            // 
            this.comboBoxImageFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxImageFormat.FormattingEnabled = true;
            this.comboBoxImageFormat.Items.AddRange(new object[] {
            "Automatic",
            "JPEG",
            "PNG",
            "GIF",
            "BMP",
            "ICON",
            "TIFF",
            "EMF",
            "WMF"});
            this.comboBoxImageFormat.Location = new System.Drawing.Point(185, 100);
            this.comboBoxImageFormat.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxImageFormat.Name = "comboBoxImageFormat";
            this.comboBoxImageFormat.Size = new System.Drawing.Size(139, 21);
            this.comboBoxImageFormat.TabIndex = 4;
            // 
            // textBoxDescriptionFormat
            // 
            this.textBoxDescriptionFormat.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::EasyImgur.Properties.Settings.Default, "descriptionFormat", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxDescriptionFormat.Location = new System.Drawing.Point(185, 149);
            this.textBoxDescriptionFormat.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxDescriptionFormat.Name = "textBoxDescriptionFormat";
            this.textBoxDescriptionFormat.Size = new System.Drawing.Size(336, 20);
            this.textBoxDescriptionFormat.TabIndex = 6;
            this.textBoxDescriptionFormat.Text = global::EasyImgur.Properties.Settings.Default.descriptionFormat;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(50, 152);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label6.Size = new System.Drawing.Size(131, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Use this description format";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(85, 128);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label5.Size = new System.Drawing.Size(96, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Use this title format";
            // 
            // textBoxTitleFormat
            // 
            this.textBoxTitleFormat.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::EasyImgur.Properties.Settings.Default, "titleFormat", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxTitleFormat.Location = new System.Drawing.Point(185, 125);
            this.textBoxTitleFormat.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxTitleFormat.Name = "textBoxTitleFormat";
            this.textBoxTitleFormat.Size = new System.Drawing.Size(336, 20);
            this.textBoxTitleFormat.TabIndex = 5;
            this.textBoxTitleFormat.Text = global::EasyImgur.Properties.Settings.Default.titleFormat;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.linkLabel3);
            this.tabPage2.Controls.Add(this.buttonForgetTokens);
            this.tabPage2.Controls.Add(this.label15);
            this.tabPage2.Controls.Add(this.label13);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.buttonForceTokenRefresh);
            this.tabPage2.Controls.Add(this.buttonAuthorize);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(587, 271);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Account";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.Location = new System.Drawing.Point(355, 210);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(199, 13);
            this.linkLabel3.TabIndex = 5;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "http://imgur.com/account/settings/apps";
            this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
            // 
            // buttonForgetTokens
            // 
            this.buttonForgetTokens.Enabled = false;
            this.buttonForgetTokens.Location = new System.Drawing.Point(232, 52);
            this.buttonForgetTokens.Margin = new System.Windows.Forms.Padding(2);
            this.buttonForgetTokens.Name = "buttonForgetTokens";
            this.buttonForgetTokens.Size = new System.Drawing.Size(135, 22);
            this.buttonForgetTokens.TabIndex = 2;
            this.buttonForgetTokens.Text = "Forget tokens";
            this.buttonForgetTokens.UseVisualStyleBackColor = true;
            this.buttonForgetTokens.Click += new System.EventHandler(this.buttonForgetTokens_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(54, 158);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(498, 65);
            this.label15.TabIndex = 10;
            this.label15.Text = resources.GetString("label15.Text");
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.LightGray;
            this.label13.ForeColor = System.Drawing.Color.DarkBlue;
            this.label13.Location = new System.Drawing.Point(292, 118);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(77, 13);
            this.label13.TabIndex = 4;
            this.label13.Text = "Not Authorized";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(248, 118);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(40, 13);
            this.label10.TabIndex = 8;
            this.label10.Text = "Status:";
            // 
            // buttonForceTokenRefresh
            // 
            this.buttonForceTokenRefresh.Enabled = false;
            this.buttonForceTokenRefresh.Location = new System.Drawing.Point(232, 79);
            this.buttonForceTokenRefresh.Margin = new System.Windows.Forms.Padding(2);
            this.buttonForceTokenRefresh.Name = "buttonForceTokenRefresh";
            this.buttonForceTokenRefresh.Size = new System.Drawing.Size(135, 22);
            this.buttonForceTokenRefresh.TabIndex = 3;
            this.buttonForceTokenRefresh.Text = "Force token refresh";
            this.buttonForceTokenRefresh.UseVisualStyleBackColor = true;
            this.buttonForceTokenRefresh.Click += new System.EventHandler(this.buttonForceTokenRefresh_Click);
            // 
            // buttonAuthorize
            // 
            this.buttonAuthorize.Location = new System.Drawing.Point(232, 25);
            this.buttonAuthorize.Margin = new System.Windows.Forms.Padding(2);
            this.buttonAuthorize.Name = "buttonAuthorize";
            this.buttonAuthorize.Size = new System.Drawing.Size(135, 22);
            this.buttonAuthorize.TabIndex = 1;
            this.buttonAuthorize.Text = "Authorize this app...";
            this.buttonAuthorize.UseVisualStyleBackColor = true;
            this.buttonAuthorize.Click += new System.EventHandler(this.buttonChangeCredentials_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Controls.Add(this.listBoxHistory);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(587, 271);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "History";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.checkBoxAlbum);
            this.groupBox1.Controls.Add(this.groupBoxHistorySelection);
            this.groupBox1.Controls.Add(this.btnOpenImageLinkInBrowser);
            this.groupBox1.Controls.Add(this.textBoxTimestamp);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.checkBoxTiedToAccount);
            this.groupBox1.Controls.Add(this.textBoxDeleteHash);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.pictureBoxHistoryThumb);
            this.groupBox1.Controls.Add(this.textBoxID);
            this.groupBox1.Controls.Add(this.textBoxLink);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(177, 2);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(404, 264);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Info";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(27, 151);
            this.label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(45, 13);
            this.label19.TabIndex = 22;
            this.label19.Text = "Preview";
            // 
            // checkBoxAlbum
            // 
            this.checkBoxAlbum.AutoSize = true;
            this.checkBoxAlbum.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.historyItemBindingSource, "Album", true));
            this.checkBoxAlbum.Enabled = false;
            this.checkBoxAlbum.Location = new System.Drawing.Point(235, 85);
            this.checkBoxAlbum.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxAlbum.Name = "checkBoxAlbum";
            this.checkBoxAlbum.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkBoxAlbum.Size = new System.Drawing.Size(80, 17);
            this.checkBoxAlbum.TabIndex = 21;
            this.checkBoxAlbum.TabStop = false;
            this.checkBoxAlbum.Text = "Is an album";
            this.checkBoxAlbum.UseVisualStyleBackColor = true;
            // 
            // historyItemBindingSource
            // 
            this.historyItemBindingSource.DataSource = typeof(EasyImgur.HistoryItem);
            // 
            // groupBoxHistorySelection
            // 
            this.groupBoxHistorySelection.Controls.Add(this.buttonRemoveFromImgur);
            this.groupBoxHistorySelection.Controls.Add(this.buttonRemoveFromHistory);
            this.groupBoxHistorySelection.Location = new System.Drawing.Point(230, 186);
            this.groupBoxHistorySelection.Name = "groupBoxHistorySelection";
            this.groupBoxHistorySelection.Size = new System.Drawing.Size(169, 74);
            this.groupBoxHistorySelection.TabIndex = 19;
            this.groupBoxHistorySelection.TabStop = false;
            this.groupBoxHistorySelection.Text = "Selection: 0 items";
            // 
            // buttonRemoveFromImgur
            // 
            this.buttonRemoveFromImgur.Enabled = false;
            this.buttonRemoveFromImgur.Location = new System.Drawing.Point(5, 46);
            this.buttonRemoveFromImgur.Margin = new System.Windows.Forms.Padding(2);
            this.buttonRemoveFromImgur.Name = "buttonRemoveFromImgur";
            this.buttonRemoveFromImgur.Size = new System.Drawing.Size(156, 24);
            this.buttonRemoveFromImgur.TabIndex = 8;
            this.buttonRemoveFromImgur.Text = "Delete from Imgur";
            this.buttonRemoveFromImgur.UseVisualStyleBackColor = true;
            this.buttonRemoveFromImgur.Click += new System.EventHandler(this.buttonRemoveFromImgur_Click);
            // 
            // buttonRemoveFromHistory
            // 
            this.buttonRemoveFromHistory.Enabled = false;
            this.buttonRemoveFromHistory.Location = new System.Drawing.Point(5, 18);
            this.buttonRemoveFromHistory.Margin = new System.Windows.Forms.Padding(2);
            this.buttonRemoveFromHistory.Name = "buttonRemoveFromHistory";
            this.buttonRemoveFromHistory.Size = new System.Drawing.Size(156, 24);
            this.buttonRemoveFromHistory.TabIndex = 7;
            this.buttonRemoveFromHistory.Text = "Clear from history";
            this.buttonRemoveFromHistory.UseMnemonic = false;
            this.buttonRemoveFromHistory.UseVisualStyleBackColor = true;
            this.buttonRemoveFromHistory.Click += new System.EventHandler(this.buttonRemoveFromHistory_Click);
            // 
            // btnOpenImageLinkInBrowser
            // 
            this.btnOpenImageLinkInBrowser.Location = new System.Drawing.Point(234, 10);
            this.btnOpenImageLinkInBrowser.Name = "btnOpenImageLinkInBrowser";
            this.btnOpenImageLinkInBrowser.Size = new System.Drawing.Size(160, 46);
            this.btnOpenImageLinkInBrowser.TabIndex = 6;
            this.btnOpenImageLinkInBrowser.Text = "Open in browser";
            this.btnOpenImageLinkInBrowser.UseVisualStyleBackColor = true;
            this.btnOpenImageLinkInBrowser.Click += new System.EventHandler(this.btnOpenImageLinkInBrowser_Click);
            // 
            // textBoxTimestamp
            // 
            this.textBoxTimestamp.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.historyItemBindingSource, "Timestamp", true));
            this.textBoxTimestamp.Location = new System.Drawing.Point(76, 83);
            this.textBoxTimestamp.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxTimestamp.Name = "textBoxTimestamp";
            this.textBoxTimestamp.ReadOnly = true;
            this.textBoxTimestamp.Size = new System.Drawing.Size(149, 20);
            this.textBoxTimestamp.TabIndex = 5;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 86);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "Timestamp";
            // 
            // checkBoxTiedToAccount
            // 
            this.checkBoxTiedToAccount.AutoSize = true;
            this.checkBoxTiedToAccount.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.historyItemBindingSource, "TiedToAccount", true));
            this.checkBoxTiedToAccount.Enabled = false;
            this.checkBoxTiedToAccount.Location = new System.Drawing.Point(235, 61);
            this.checkBoxTiedToAccount.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxTiedToAccount.Name = "checkBoxTiedToAccount";
            this.checkBoxTiedToAccount.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkBoxTiedToAccount.Size = new System.Drawing.Size(105, 17);
            this.checkBoxTiedToAccount.TabIndex = 15;
            this.checkBoxTiedToAccount.TabStop = false;
            this.checkBoxTiedToAccount.Text = "On your account";
            this.checkBoxTiedToAccount.UseVisualStyleBackColor = true;
            // 
            // textBoxDeleteHash
            // 
            this.textBoxDeleteHash.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.historyItemBindingSource, "Deletehash", true));
            this.textBoxDeleteHash.Location = new System.Drawing.Point(76, 59);
            this.textBoxDeleteHash.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxDeleteHash.Name = "textBoxDeleteHash";
            this.textBoxDeleteHash.ReadOnly = true;
            this.textBoxDeleteHash.Size = new System.Drawing.Size(149, 20);
            this.textBoxDeleteHash.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 62);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Delete Hash";
            // 
            // pictureBoxHistoryThumb
            // 
            this.pictureBoxHistoryThumb.DataBindings.Add(new System.Windows.Forms.Binding("Image", this.historyItemBindingSource, "Thumbnail", true));
            this.pictureBoxHistoryThumb.Location = new System.Drawing.Point(76, 148);
            this.pictureBoxHistoryThumb.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBoxHistoryThumb.Name = "pictureBoxHistoryThumb";
            this.pictureBoxHistoryThumb.Size = new System.Drawing.Size(148, 112);
            this.pictureBoxHistoryThumb.TabIndex = 11;
            this.pictureBoxHistoryThumb.TabStop = false;
            // 
            // textBoxID
            // 
            this.textBoxID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.historyItemBindingSource, "Id", true));
            this.textBoxID.Location = new System.Drawing.Point(76, 11);
            this.textBoxID.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxID.Name = "textBoxID";
            this.textBoxID.ReadOnly = true;
            this.textBoxID.Size = new System.Drawing.Size(149, 20);
            this.textBoxID.TabIndex = 2;
            // 
            // textBoxLink
            // 
            this.textBoxLink.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.historyItemBindingSource, "Link", true));
            this.textBoxLink.Location = new System.Drawing.Point(76, 35);
            this.textBoxLink.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxLink.Name = "textBoxLink";
            this.textBoxLink.ReadOnly = true;
            this.textBoxLink.Size = new System.Drawing.Size(149, 20);
            this.textBoxLink.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 38);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Link";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "ID";
            // 
            // listBoxHistory
            // 
            this.listBoxHistory.DataSource = this.historyItemBindingSource;
            this.listBoxHistory.DisplayMember = "ListName";
            this.listBoxHistory.FormattingEnabled = true;
            this.listBoxHistory.Location = new System.Drawing.Point(2, 2);
            this.listBoxHistory.Margin = new System.Windows.Forms.Padding(2);
            this.listBoxHistory.Name = "listBoxHistory";
            this.listBoxHistory.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxHistory.Size = new System.Drawing.Size(171, 264);
            this.listBoxHistory.TabIndex = 1;
            this.listBoxHistory.SelectedIndexChanged += new System.EventHandler(this.listBoxHistory_SelectedIndexChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.versionLabel);
            this.tabPage4.Controls.Add(this.label18);
            this.tabPage4.Controls.Add(this.contributorsList);
            this.tabPage4.Controls.Add(this.pictureBox2);
            this.tabPage4.Controls.Add(this.linkLabel2);
            this.tabPage4.Controls.Add(this.linkLabel1);
            this.tabPage4.Controls.Add(this.appDescriptionLabel);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(587, 271);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "About";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(217, 95);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(0, 13);
            this.versionLabel.TabIndex = 6;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(359, 21);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(225, 13);
            this.label18.TabIndex = 5;
            this.label18.Text = "A massive thanks to the following contributors:";
            // 
            // contributorsList
            // 
            this.contributorsList.FormattingEnabled = true;
            this.contributorsList.Location = new System.Drawing.Point(362, 48);
            this.contributorsList.Name = "contributorsList";
            this.contributorsList.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.contributorsList.Size = new System.Drawing.Size(218, 212);
            this.contributorsList.TabIndex = 4;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::EasyImgur.Properties.Resources.ei_logo;
            this.pictureBox2.Location = new System.Drawing.Point(150, 21);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(65, 65);
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(155, 95);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(56, 13);
            this.linkLabel2.TabIndex = 1;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "EasyImgur";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(121, 198);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(121, 13);
            this.linkLabel1.TabIndex = 2;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "http://bryankeiren.com/";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // appDescriptionLabel
            // 
            this.appDescriptionLabel.AutoSize = true;
            this.appDescriptionLabel.Location = new System.Drawing.Point(8, 146);
            this.appDescriptionLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.appDescriptionLabel.Name = "appDescriptionLabel";
            this.appDescriptionLabel.Size = new System.Drawing.Size(349, 39);
            this.appDescriptionLabel.TabIndex = 0;
            this.appDescriptionLabel.Text = "EasyImgur is a small and simple tool to easily upload images to imgur.com\r\n\r\nThis" +
    " application was created by Bryan Keiren\r\n";
            this.appDescriptionLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(315, 185);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(52, 17);
            this.label11.TabIndex = 8;
            this.label11.Text = "Status:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(148, 109);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(422, 34);
            this.label12.TabIndex = 7;
            this.label12.Text = "Authorizing this app enables you to add uploaded images to your \r\naccount, rather" +
    " than uploading them anonymously.";
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(258, 64);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(180, 27);
            this.button1.TabIndex = 6;
            this.button1.Text = "Force token refresh";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(258, 31);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(180, 27);
            this.button2.TabIndex = 5;
            this.button2.Text = "Authorize this app...";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // trayMenu
            // 
            this.trayMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.uploadClipboardAccountTrayMenuItem,
            this.uploadFileAccountTrayMenuItem,
            this.menuItem9,
            this.uploadClipboardAnonymousTrayMenuItem,
            this.uploadFileAnonymousTrayMenuItem,
            this.menuItem8,
            this.settingsTrayMenuItem,
            this.menuItem7,
            this.exitTrayMenuItem});
            // 
            // uploadClipboardAccountTrayMenuItem
            // 
            this.uploadClipboardAccountTrayMenuItem.Enabled = false;
            this.uploadClipboardAccountTrayMenuItem.Index = 0;
            this.uploadClipboardAccountTrayMenuItem.Text = "Upload from clipboard (to account)";
            this.uploadClipboardAccountTrayMenuItem.Click += new System.EventHandler(this.uploadClipboardAccountTrayMenuItem_Click);
            // 
            // uploadFileAccountTrayMenuItem
            // 
            this.uploadFileAccountTrayMenuItem.Enabled = false;
            this.uploadFileAccountTrayMenuItem.Index = 1;
            this.uploadFileAccountTrayMenuItem.Text = "Upload from file(s)... (to account)";
            this.uploadFileAccountTrayMenuItem.Click += new System.EventHandler(this.uploadFileAccountTrayMenuItem_Click);
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 2;
            this.menuItem9.Text = "-";
            // 
            // uploadClipboardAnonymousTrayMenuItem
            // 
            this.uploadClipboardAnonymousTrayMenuItem.Index = 3;
            this.uploadClipboardAnonymousTrayMenuItem.Text = "Upload from clipboard (anonymously)";
            this.uploadClipboardAnonymousTrayMenuItem.Click += new System.EventHandler(this.uploadClipboardAnonymousTrayMenuItem_Click);
            // 
            // uploadFileAnonymousTrayMenuItem
            // 
            this.uploadFileAnonymousTrayMenuItem.Index = 4;
            this.uploadFileAnonymousTrayMenuItem.Text = "Upload from file(s)... (anonymously)";
            this.uploadFileAnonymousTrayMenuItem.Click += new System.EventHandler(this.uploadFileAnonymousTrayMenuItem_Click);
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 5;
            this.menuItem8.Text = "-";
            // 
            // settingsTrayMenuItem
            // 
            this.settingsTrayMenuItem.Index = 6;
            this.settingsTrayMenuItem.Text = "Settings...";
            this.settingsTrayMenuItem.Click += new System.EventHandler(this.settingsTrayMenuItem_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 7;
            this.menuItem7.Text = "-";
            // 
            // exitTrayMenuItem
            // 
            this.exitTrayMenuItem.Index = 8;
            this.exitTrayMenuItem.Text = "Exit";
            this.exitTrayMenuItem.Click += new System.EventHandler(this.exitTrayMenuItem_Click);
            // 
            // Form1
            // 
            this.AcceptButton = this.buttonApplyGeneral;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 301);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "EasyImgur";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.clipboardSettingsContainer.ResumeLayout(false);
            this.clipboardSettingsContainer.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.historyItemBindingSource)).EndInit();
            this.groupBoxHistorySelection.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHistoryThumb)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.CheckBox checkBoxClearClipboard;
        private System.Windows.Forms.Button buttonApplyGeneral;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button buttonAuthorize;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListBox listBoxHistory;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxID;
        private System.Windows.Forms.TextBox textBoxLink;
        private System.Windows.Forms.CheckBox checkBoxCopyLinks;
        private System.Windows.Forms.PictureBox pictureBoxHistoryThumb;
        private System.Windows.Forms.TextBox textBoxDescriptionFormat;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxTitleFormat;
        private System.Windows.Forms.ComboBox comboBoxImageFormat;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxDeleteHash;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonRemoveFromImgur;
        private System.Windows.Forms.Button buttonForceTokenRefresh;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label appDescriptionLabel;
        private System.Windows.Forms.CheckBox checkBoxTiedToAccount;
        private System.Windows.Forms.Button buttonFormatHelp;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button buttonForgetTokens;
        private System.Windows.Forms.CheckBox checkBoxLaunchAtBoot;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBoxShowTokenRefreshNotification;
        private System.Windows.Forms.Button buttonRemoveFromHistory;
        private System.Windows.Forms.ContextMenu trayMenu;
        private System.Windows.Forms.MenuItem uploadClipboardAccountTrayMenuItem;
        private System.Windows.Forms.MenuItem uploadFileAccountTrayMenuItem;
        private System.Windows.Forms.MenuItem uploadClipboardAnonymousTrayMenuItem;
        private System.Windows.Forms.MenuItem uploadFileAnonymousTrayMenuItem;
        private System.Windows.Forms.MenuItem menuItem9;
        private System.Windows.Forms.MenuItem menuItem8;
        private System.Windows.Forms.MenuItem settingsTrayMenuItem;
        private System.Windows.Forms.MenuItem menuItem7;
        private System.Windows.Forms.MenuItem exitTrayMenuItem;
        private System.Windows.Forms.TextBox textBoxTimestamp;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.GroupBox groupBoxHistorySelection;
        private System.Windows.Forms.Button btnOpenImageLinkInBrowser;
        private System.Windows.Forms.CheckBox checkBoxEnableContextMenu;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.CheckBox checkBoxGalleryUpload;
        private System.Windows.Forms.CheckBox checkBoxAlbum;
        private System.Windows.Forms.BindingSource historyItemBindingSource;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ListBox contributorsList;
        private System.Windows.Forms.Label labelPortableModeNote;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.CheckBox checkBoxCopyHttpsLinks;
        private System.Windows.Forms.GroupBox clipboardSettingsContainer;
    }
}

