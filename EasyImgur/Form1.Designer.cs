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
            this.trayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.uploadClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadFromFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.uploadClipboardAnonymousToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadFromFileAnonymousToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.checkBoxClearClipboard = new System.Windows.Forms.CheckBox();
            this.buttonApplyGeneral = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
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
            this.checkBoxCopyLinks = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.buttonForgetTokens = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.buttonForceTokenRefresh = new System.Windows.Forms.Button();
            this.buttonAuthorize = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxTiedToAccount = new System.Windows.Forms.CheckBox();
            this.buttonRemoveFromImgur = new System.Windows.Forms.Button();
            this.textBoxDeleteHash = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBoxID = new System.Windows.Forms.TextBox();
            this.textBoxLink = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.listBoxHistory = new System.Windows.Forms.ListBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label14 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.trayMenu.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // trayMenu
            // 
            this.trayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uploadClipboardToolStripMenuItem,
            this.uploadFromFileToolStripMenuItem,
            this.toolStripSeparator3,
            this.uploadClipboardAnonymousToolStripMenuItem,
            this.uploadFromFileAnonymousToolStripMenuItem,
            this.toolStripSeparator1,
            this.settingsToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.trayMenu.Name = "trayMenu";
            this.trayMenu.Size = new System.Drawing.Size(322, 166);
            // 
            // uploadClipboardToolStripMenuItem
            // 
            this.uploadClipboardToolStripMenuItem.Enabled = false;
            this.uploadClipboardToolStripMenuItem.Name = "uploadClipboardToolStripMenuItem";
            this.uploadClipboardToolStripMenuItem.Size = new System.Drawing.Size(321, 24);
            this.uploadClipboardToolStripMenuItem.Text = "Upload from clipboard (to account)";
            this.uploadClipboardToolStripMenuItem.Click += new System.EventHandler(this.uploadClipboardToolStripMenuItem_Click);
            // 
            // uploadFromFileToolStripMenuItem
            // 
            this.uploadFromFileToolStripMenuItem.Enabled = false;
            this.uploadFromFileToolStripMenuItem.Name = "uploadFromFileToolStripMenuItem";
            this.uploadFromFileToolStripMenuItem.Size = new System.Drawing.Size(321, 24);
            this.uploadFromFileToolStripMenuItem.Text = "Upload from file...  (to account)";
            this.uploadFromFileToolStripMenuItem.Click += new System.EventHandler(this.uploadFromFileToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(318, 6);
            // 
            // uploadClipboardAnonymousToolStripMenuItem
            // 
            this.uploadClipboardAnonymousToolStripMenuItem.Name = "uploadClipboardAnonymousToolStripMenuItem";
            this.uploadClipboardAnonymousToolStripMenuItem.Size = new System.Drawing.Size(321, 24);
            this.uploadClipboardAnonymousToolStripMenuItem.Text = "Upload from clipboard (anonymous)";
            this.uploadClipboardAnonymousToolStripMenuItem.Click += new System.EventHandler(this.uploadClipboardAnonymousToolStripMenuItem_Click);
            // 
            // uploadFromFileAnonymousToolStripMenuItem
            // 
            this.uploadFromFileAnonymousToolStripMenuItem.Name = "uploadFromFileAnonymousToolStripMenuItem";
            this.uploadFromFileAnonymousToolStripMenuItem.Size = new System.Drawing.Size(321, 24);
            this.uploadFromFileAnonymousToolStripMenuItem.Text = "Upload from file... (anonymous)";
            this.uploadFromFileAnonymousToolStripMenuItem.Click += new System.EventHandler(this.uploadFromFileAnonymousToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(318, 6);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(321, 24);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(318, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(321, 24);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.trayMenu;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "EasyImgur";
            this.notifyIcon1.Visible = true;
            // 
            // checkBoxClearClipboard
            // 
            this.checkBoxClearClipboard.AutoSize = true;
            this.checkBoxClearClipboard.Location = new System.Drawing.Point(71, 20);
            this.checkBoxClearClipboard.Name = "checkBoxClearClipboard";
            this.checkBoxClearClipboard.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxClearClipboard.Size = new System.Drawing.Size(192, 21);
            this.checkBoxClearClipboard.TabIndex = 1;
            this.checkBoxClearClipboard.Text = "Clear clipboard on upload";
            this.checkBoxClearClipboard.UseVisualStyleBackColor = true;
            // 
            // buttonApplyGeneral
            // 
            this.buttonApplyGeneral.Location = new System.Drawing.Point(581, 233);
            this.buttonApplyGeneral.Name = "buttonApplyGeneral";
            this.buttonApplyGeneral.Size = new System.Drawing.Size(108, 36);
            this.buttonApplyGeneral.TabIndex = 2;
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
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(715, 312);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
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
            this.tabPage1.Controls.Add(this.checkBoxCopyLinks);
            this.tabPage1.Controls.Add(this.buttonApplyGeneral);
            this.tabPage1.Controls.Add(this.checkBoxClearClipboard);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(707, 283);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label4.Location = new System.Drawing.Point(269, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(408, 17);
            this.label4.TabIndex = 15;
            this.label4.Text = "Moving the .exe requires one manual launch to restore the path";
            // 
            // checkBoxLaunchAtBoot
            // 
            this.checkBoxLaunchAtBoot.AutoSize = true;
            this.checkBoxLaunchAtBoot.Location = new System.Drawing.Point(8, 159);
            this.checkBoxLaunchAtBoot.Name = "checkBoxLaunchAtBoot";
            this.checkBoxLaunchAtBoot.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxLaunchAtBoot.Size = new System.Drawing.Size(255, 21);
            this.checkBoxLaunchAtBoot.TabIndex = 14;
            this.checkBoxLaunchAtBoot.Text = "Launch EasyImgur at Windows start";
            this.checkBoxLaunchAtBoot.UseVisualStyleBackColor = true;
            // 
            // buttonFormatHelp
            // 
            this.buttonFormatHelp.Location = new System.Drawing.Point(626, 103);
            this.buttonFormatHelp.Name = "buttonFormatHelp";
            this.buttonFormatHelp.Size = new System.Drawing.Size(75, 50);
            this.buttonFormatHelp.TabIndex = 13;
            this.buttonFormatHelp.Text = "Format?";
            this.buttonFormatHelp.UseVisualStyleBackColor = true;
            this.buttonFormatHelp.Click += new System.EventHandler(this.buttonFormatHelp_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label8.Location = new System.Drawing.Point(435, 77);
            this.label8.Name = "label8";
            this.label8.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label8.Size = new System.Drawing.Size(159, 17);
            this.label8.TabIndex = 10;
            this.label8.Text = "Imgur may change this *";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(76, 77);
            this.label7.Name = "label7";
            this.label7.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label7.Size = new System.Drawing.Size(163, 17);
            this.label7.TabIndex = 9;
            this.label7.Text = "* Preferred image format";
            // 
            // comboBoxImageFormat
            // 
            this.comboBoxImageFormat.FormattingEnabled = true;
            this.comboBoxImageFormat.Items.AddRange(new object[] {
            "Auto",
            "JPEG",
            "PNG",
            "GIF",
            "BMP",
            "ICON",
            "TIFF",
            "EMF",
            "WMF"});
            this.comboBoxImageFormat.Location = new System.Drawing.Point(245, 74);
            this.comboBoxImageFormat.Name = "comboBoxImageFormat";
            this.comboBoxImageFormat.Size = new System.Drawing.Size(184, 24);
            this.comboBoxImageFormat.TabIndex = 8;
            // 
            // textBoxDescriptionFormat
            // 
            this.textBoxDescriptionFormat.Location = new System.Drawing.Point(245, 131);
            this.textBoxDescriptionFormat.Name = "textBoxDescriptionFormat";
            this.textBoxDescriptionFormat.Size = new System.Drawing.Size(377, 22);
            this.textBoxDescriptionFormat.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(63, 134);
            this.label6.Name = "label6";
            this.label6.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label6.Size = new System.Drawing.Size(176, 17);
            this.label6.TabIndex = 6;
            this.label6.Text = "Use this description format";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(110, 106);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label5.Size = new System.Drawing.Size(129, 17);
            this.label5.TabIndex = 5;
            this.label5.Text = "Use this title format";
            // 
            // textBoxTitleFormat
            // 
            this.textBoxTitleFormat.Location = new System.Drawing.Point(245, 103);
            this.textBoxTitleFormat.Name = "textBoxTitleFormat";
            this.textBoxTitleFormat.Size = new System.Drawing.Size(377, 22);
            this.textBoxTitleFormat.TabIndex = 4;
            // 
            // checkBoxCopyLinks
            // 
            this.checkBoxCopyLinks.AutoSize = true;
            this.checkBoxCopyLinks.Location = new System.Drawing.Point(6, 47);
            this.checkBoxCopyLinks.Name = "checkBoxCopyLinks";
            this.checkBoxCopyLinks.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxCopyLinks.Size = new System.Drawing.Size(257, 21);
            this.checkBoxCopyLinks.TabIndex = 2;
            this.checkBoxCopyLinks.Text = "Automatically copy links to clipboard";
            this.checkBoxCopyLinks.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.buttonForgetTokens);
            this.tabPage2.Controls.Add(this.label15);
            this.tabPage2.Controls.Add(this.label13);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.buttonForceTokenRefresh);
            this.tabPage2.Controls.Add(this.buttonAuthorize);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(707, 283);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Account";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // buttonForgetTokens
            // 
            this.buttonForgetTokens.Enabled = false;
            this.buttonForgetTokens.Location = new System.Drawing.Point(258, 64);
            this.buttonForgetTokens.Name = "buttonForgetTokens";
            this.buttonForgetTokens.Size = new System.Drawing.Size(180, 27);
            this.buttonForgetTokens.TabIndex = 11;
            this.buttonForgetTokens.Text = "Forget tokens";
            this.buttonForgetTokens.UseVisualStyleBackColor = true;
            this.buttonForgetTokens.Click += new System.EventHandler(this.buttonForgetTokens_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(27, 181);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(651, 85);
            this.label15.TabIndex = 10;
            this.label15.Text = resources.GetString("label15.Text");
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.LightGray;
            this.label13.ForeColor = System.Drawing.Color.DarkBlue;
            this.label13.Location = new System.Drawing.Point(338, 145);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(102, 17);
            this.label13.TabIndex = 9;
            this.label13.Text = "Not Authorized";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(280, 145);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(52, 17);
            this.label10.TabIndex = 8;
            this.label10.Text = "Status:";
            // 
            // buttonForceTokenRefresh
            // 
            this.buttonForceTokenRefresh.Enabled = false;
            this.buttonForceTokenRefresh.Location = new System.Drawing.Point(258, 97);
            this.buttonForceTokenRefresh.Name = "buttonForceTokenRefresh";
            this.buttonForceTokenRefresh.Size = new System.Drawing.Size(180, 27);
            this.buttonForceTokenRefresh.TabIndex = 6;
            this.buttonForceTokenRefresh.Text = "Force token refresh";
            this.buttonForceTokenRefresh.UseVisualStyleBackColor = true;
            this.buttonForceTokenRefresh.Click += new System.EventHandler(this.buttonForceTokenRefresh_Click);
            // 
            // buttonAuthorize
            // 
            this.buttonAuthorize.Location = new System.Drawing.Point(258, 31);
            this.buttonAuthorize.Name = "buttonAuthorize";
            this.buttonAuthorize.Size = new System.Drawing.Size(180, 27);
            this.buttonAuthorize.TabIndex = 5;
            this.buttonAuthorize.Text = "Authorize this app...";
            this.buttonAuthorize.UseVisualStyleBackColor = true;
            this.buttonAuthorize.Click += new System.EventHandler(this.buttonChangeCredentials_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Controls.Add(this.listBoxHistory);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(707, 283);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "History";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxTiedToAccount);
            this.groupBox1.Controls.Add(this.buttonRemoveFromImgur);
            this.groupBox1.Controls.Add(this.textBoxDeleteHash);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.textBoxID);
            this.groupBox1.Controls.Add(this.textBoxLink);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(236, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(465, 276);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Info";
            // 
            // checkBoxTiedToAccount
            // 
            this.checkBoxTiedToAccount.AutoSize = true;
            this.checkBoxTiedToAccount.Enabled = false;
            this.checkBoxTiedToAccount.Location = new System.Drawing.Point(319, 21);
            this.checkBoxTiedToAccount.Name = "checkBoxTiedToAccount";
            this.checkBoxTiedToAccount.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxTiedToAccount.Size = new System.Drawing.Size(128, 21);
            this.checkBoxTiedToAccount.TabIndex = 15;
            this.checkBoxTiedToAccount.Text = "Tied to account";
            this.checkBoxTiedToAccount.UseVisualStyleBackColor = true;
            // 
            // buttonRemoveFromImgur
            // 
            this.buttonRemoveFromImgur.Enabled = false;
            this.buttonRemoveFromImgur.Location = new System.Drawing.Point(304, 240);
            this.buttonRemoveFromImgur.Name = "buttonRemoveFromImgur";
            this.buttonRemoveFromImgur.Size = new System.Drawing.Size(155, 30);
            this.buttonRemoveFromImgur.TabIndex = 14;
            this.buttonRemoveFromImgur.Text = "Remove from Imgur";
            this.buttonRemoveFromImgur.UseVisualStyleBackColor = true;
            this.buttonRemoveFromImgur.Click += new System.EventHandler(this.buttonRemoveFromImgur_Click);
            // 
            // textBoxDeleteHash
            // 
            this.textBoxDeleteHash.Location = new System.Drawing.Point(101, 76);
            this.textBoxDeleteHash.Name = "textBoxDeleteHash";
            this.textBoxDeleteHash.ReadOnly = true;
            this.textBoxDeleteHash.Size = new System.Drawing.Size(197, 22);
            this.textBoxDeleteHash.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 17);
            this.label3.TabIndex = 12;
            this.label3.Text = "Delete Hash";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(6, 104);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(292, 166);
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // textBoxID
            // 
            this.textBoxID.Location = new System.Drawing.Point(101, 22);
            this.textBoxID.Name = "textBoxID";
            this.textBoxID.ReadOnly = true;
            this.textBoxID.Size = new System.Drawing.Size(197, 22);
            this.textBoxID.TabIndex = 6;
            // 
            // textBoxLink
            // 
            this.textBoxLink.Location = new System.Drawing.Point(101, 48);
            this.textBoxLink.Name = "textBoxLink";
            this.textBoxLink.ReadOnly = true;
            this.textBoxLink.Size = new System.Drawing.Size(197, 22);
            this.textBoxLink.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(61, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Link";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(74, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "ID";
            // 
            // listBoxHistory
            // 
            this.listBoxHistory.DisplayMember = "listName";
            this.listBoxHistory.FormattingEnabled = true;
            this.listBoxHistory.ItemHeight = 16;
            this.listBoxHistory.Location = new System.Drawing.Point(3, 3);
            this.listBoxHistory.Name = "listBoxHistory";
            this.listBoxHistory.Size = new System.Drawing.Size(227, 276);
            this.listBoxHistory.TabIndex = 0;
            this.listBoxHistory.SelectedIndexChanged += new System.EventHandler(this.listBoxHistory_SelectedIndexChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.label14);
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(707, 283);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "About";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(114, 80);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(472, 68);
            this.label14.TabIndex = 0;
            this.label14.Text = "EasyImgur is a small and simple tool to easily upload images to imgur.com\r\n\r\nThis" +
                " application was created by Bryan Keiren\r\nhttp://bryankeiren.com/";
            this.label14.TextAlign = System.Drawing.ContentAlignment.TopCenter;
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
            // Form1
            // 
            this.AcceptButton = this.buttonApplyGeneral;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 318);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "EasyImgur";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.trayMenu.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip trayMenu;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolStripMenuItem uploadClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uploadFromFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
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
        private System.Windows.Forms.PictureBox pictureBox1;
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
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox checkBoxTiedToAccount;
        private System.Windows.Forms.Button buttonFormatHelp;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ToolStripMenuItem uploadClipboardAnonymousToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uploadFromFileAnonymousToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.Button buttonForgetTokens;
        private System.Windows.Forms.CheckBox checkBoxLaunchAtBoot;
        private System.Windows.Forms.Label label4;
    }
}

