using System;
using System.Windows.Forms;

namespace YorotInstaller
{
    partial class frmMain
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                // If this form is inherited, the IDE needs this style
                // set so that its coordinate system is correct.
                const int WS_CHILDWINDOW = 0x40000000;
                // The following two styles are used to clip child
                // and sibling windows in Paint events.
                const int WS_CLIPCHILDREN = 0x2000000;
                const int WS_CLIPSIBLINGS = 0x4000000;
                // Add a Minimize button (or Minimize option in Window Menu).
                const int WS_MINIMIZEBOX = 0x20000;
                // Add a Maximum/Restore Button (or Options in Window Menu).
                const int WS_MAXIMIZEBOX = 0x10000;
                // Window can be resized.
                const int WS_THICKFRAME = 0x40000;
                // Add A Window Menu
                const int WS_SYSMENU = 0x80000;

                // Detect Double Clicks
                const int CS_DBLCLKS = 0x8;
                // Add a DropShadow (WinXP or greater).
                const int CS_DROPSHADOW = 0x20000;

                CreateParams cp = base.CreateParams;

                cp.Style = WS_CLIPCHILDREN | WS_CLIPSIBLINGS
            | WS_MAXIMIZEBOX | WS_MINIMIZEBOX
            | WS_SYSMENU | WS_THICKFRAME;

                if (DesignMode)
                {
                    cp.Style = cp.Style | WS_CHILDWINDOW;
                }

                int ClassFlags = CS_DBLCLKS;
                int OSVER = Environment.OSVersion.Version.Major * 10;
                OSVER += Environment.OSVersion.Version.Minor;

                if (OSVER >= 51)
                {
                    ClassFlags = ClassFlags | CS_DROPSHADOW;
                }

                cp.ClassStyle = ClassFlags;

                return cp;
            }
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                if (e.Clicks > 1)
                {
                    OnMouseDoubleClick(e);
                }
                else
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                    ReleaseCapture();
                }
            }
            Invalidate();
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }
            else if (WindowState == FormWindowState.Normal)
            {
                MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;
                WindowState = FormWindowState.Maximized;
            }
            Invalidate();
        }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbLang = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpStart = new System.Windows.Forms.TabPage();
            this.tpFirst = new System.Windows.Forms.TabPage();
            this.btInstall = new System.Windows.Forms.Button();
            this.lbReady = new System.Windows.Forms.Label();
            this.tpProgress = new System.Windows.Forms.TabPage();
            this.pInstall = new System.Windows.Forms.Panel();
            this.pbInstall = new System.Windows.Forms.PictureBox();
            this.pDownload = new System.Windows.Forms.Panel();
            this.pbDownload = new System.Windows.Forms.PictureBox();
            this.lbInstallInfo = new System.Windows.Forms.Label();
            this.lbInstallCount = new System.Windows.Forms.Label();
            this.lbDownloadCount = new System.Windows.Forms.Label();
            this.lbDownloadInfo = new System.Windows.Forms.Label();
            this.lbInstalling = new System.Windows.Forms.Label();
            this.lbDownloading = new System.Windows.Forms.Label();
            this.tpModify = new System.Windows.Forms.TabPage();
            this.pChangeVer = new System.Windows.Forms.Panel();
            this.rbOld = new System.Windows.Forms.RadioButton();
            this.rbStable = new System.Windows.Forms.RadioButton();
            this.rbPreOut = new System.Windows.Forms.RadioButton();
            this.btInstall1 = new System.Windows.Forms.Button();
            this.lbInstallError = new System.Windows.Forms.Label();
            this.lbVersionToInstall = new System.Windows.Forms.Label();
            this.cbOld = new System.Windows.Forms.ComboBox();
            this.lbPerOutReq = new System.Windows.Forms.Label();
            this.lbCloseChangeVer = new System.Windows.Forms.Label();
            this.lbChangeVerDesc = new System.Windows.Forms.Label();
            this.btChangeVer = new System.Windows.Forms.Button();
            this.btUninstall = new System.Windows.Forms.Button();
            this.btRepair = new System.Windows.Forms.Button();
            this.lbModifyDesc = new System.Windows.Forms.Label();
            this.tpDone = new System.Windows.Forms.TabPage();
            this.tbDoneError = new System.Windows.Forms.TextBox();
            this.lbDoneDesc = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btClose = new System.Windows.Forms.Button();
            this.btSendFeedback = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpStart.SuspendLayout();
            this.tpFirst.SuspendLayout();
            this.tpProgress.SuspendLayout();
            this.pInstall.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInstall)).BeginInit();
            this.pDownload.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbDownload)).BeginInit();
            this.tpModify.SuspendLayout();
            this.pChangeVer.SuspendLayout();
            this.tpDone.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.label1.Location = new System.Drawing.Point(62, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 31);
            this.label1.TabIndex = 1;
            this.label1.Text = "Yorot";
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(64, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Haltroy";
            this.label2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDown);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label3.Location = new System.Drawing.Point(553, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 25);
            this.label3.TabIndex = 1;
            this.label3.Text = "X";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::YorotInstaller.Properties.Resources.Yorot;
            this.pictureBox1.Location = new System.Drawing.Point(5, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(51, 51);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDown);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.Image = global::YorotInstaller.Properties.Resources.light;
            this.pictureBox2.Location = new System.Drawing.Point(514, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(33, 33);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label4.Location = new System.Drawing.Point(3, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(587, 399);
            this.label4.TabIndex = 4;
            this.label4.Text = "Gathering information...";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbLang);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(584, 63);
            this.panel1.TabIndex = 5;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDown);
            // 
            // cbLang
            // 
            this.cbLang.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbLang.FormattingEnabled = true;
            this.cbLang.Location = new System.Drawing.Point(387, 10);
            this.cbLang.Name = "cbLang";
            this.cbLang.Size = new System.Drawing.Size(121, 21);
            this.cbLang.TabIndex = 4;
            this.cbLang.DropDown += new System.EventHandler(this.cbLang_DropDown);
            this.cbLang.SelectedIndexChanged += new System.EventHandler(this.cbLang_SelectedIndexChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tpStart);
            this.tabControl1.Controls.Add(this.tpFirst);
            this.tabControl1.Controls.Add(this.tpProgress);
            this.tabControl1.Controls.Add(this.tpModify);
            this.tabControl1.Controls.Add(this.tpDone);
            this.tabControl1.Location = new System.Drawing.Point(-5, 40);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(601, 431);
            this.tabControl1.TabIndex = 6;
            this.tabControl1.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl1_Selecting);
            // 
            // tpStart
            // 
            this.tpStart.Controls.Add(this.label4);
            this.tpStart.Location = new System.Drawing.Point(4, 22);
            this.tpStart.Name = "tpStart";
            this.tpStart.Padding = new System.Windows.Forms.Padding(3);
            this.tpStart.Size = new System.Drawing.Size(593, 405);
            this.tpStart.TabIndex = 0;
            this.tpStart.Text = "GATHER INFO";
            this.tpStart.UseVisualStyleBackColor = true;
            // 
            // tpFirst
            // 
            this.tpFirst.Controls.Add(this.btInstall);
            this.tpFirst.Controls.Add(this.lbReady);
            this.tpFirst.Location = new System.Drawing.Point(4, 22);
            this.tpFirst.Name = "tpFirst";
            this.tpFirst.Padding = new System.Windows.Forms.Padding(3);
            this.tpFirst.Size = new System.Drawing.Size(593, 405);
            this.tpFirst.TabIndex = 1;
            this.tpFirst.Text = "FIRST INSTALL";
            this.tpFirst.UseVisualStyleBackColor = true;
            // 
            // btInstall
            // 
            this.btInstall.AutoSize = true;
            this.btInstall.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btInstall.FlatAppearance.BorderSize = 0;
            this.btInstall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btInstall.Location = new System.Drawing.Point(17, 40);
            this.btInstall.Name = "btInstall";
            this.btInstall.Size = new System.Drawing.Size(44, 23);
            this.btInstall.TabIndex = 1;
            this.btInstall.Text = "Install";
            this.btInstall.UseVisualStyleBackColor = true;
            this.btInstall.Click += new System.EventHandler(this.btInstall_Click);
            // 
            // lbReady
            // 
            this.lbReady.AutoSize = true;
            this.lbReady.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lbReady.Location = new System.Drawing.Point(13, 17);
            this.lbReady.Name = "lbReady";
            this.lbReady.Size = new System.Drawing.Size(147, 20);
            this.lbReady.TabIndex = 0;
            this.lbReady.Text = "Your Yorot is ready.";
            // 
            // tpProgress
            // 
            this.tpProgress.Controls.Add(this.pInstall);
            this.tpProgress.Controls.Add(this.pDownload);
            this.tpProgress.Controls.Add(this.lbInstallInfo);
            this.tpProgress.Controls.Add(this.lbInstallCount);
            this.tpProgress.Controls.Add(this.lbDownloadCount);
            this.tpProgress.Controls.Add(this.lbDownloadInfo);
            this.tpProgress.Controls.Add(this.lbInstalling);
            this.tpProgress.Controls.Add(this.lbDownloading);
            this.tpProgress.Location = new System.Drawing.Point(4, 22);
            this.tpProgress.Name = "tpProgress";
            this.tpProgress.Size = new System.Drawing.Size(593, 405);
            this.tpProgress.TabIndex = 2;
            this.tpProgress.Text = "PROGRESS";
            this.tpProgress.UseVisualStyleBackColor = true;
            // 
            // pInstall
            // 
            this.pInstall.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pInstall.Controls.Add(this.pbInstall);
            this.pInstall.Location = new System.Drawing.Point(18, 187);
            this.pInstall.Name = "pInstall";
            this.pInstall.Size = new System.Drawing.Size(500, 10);
            this.pInstall.TabIndex = 1;
            // 
            // pbInstall
            // 
            this.pbInstall.BackColor = System.Drawing.Color.DodgerBlue;
            this.pbInstall.Dock = System.Windows.Forms.DockStyle.Left;
            this.pbInstall.Location = new System.Drawing.Point(0, 0);
            this.pbInstall.Name = "pbInstall";
            this.pbInstall.Size = new System.Drawing.Size(10, 10);
            this.pbInstall.TabIndex = 0;
            this.pbInstall.TabStop = false;
            // 
            // pDownload
            // 
            this.pDownload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pDownload.Controls.Add(this.pbDownload);
            this.pDownload.Location = new System.Drawing.Point(18, 80);
            this.pDownload.Name = "pDownload";
            this.pDownload.Size = new System.Drawing.Size(500, 10);
            this.pDownload.TabIndex = 1;
            // 
            // pbDownload
            // 
            this.pbDownload.BackColor = System.Drawing.Color.DodgerBlue;
            this.pbDownload.Dock = System.Windows.Forms.DockStyle.Left;
            this.pbDownload.Location = new System.Drawing.Point(0, 0);
            this.pbDownload.Name = "pbDownload";
            this.pbDownload.Size = new System.Drawing.Size(10, 10);
            this.pbDownload.TabIndex = 0;
            this.pbDownload.TabStop = false;
            // 
            // lbInstallInfo
            // 
            this.lbInstallInfo.AutoSize = true;
            this.lbInstallInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lbInstallInfo.Location = new System.Drawing.Point(15, 157);
            this.lbInstallInfo.Name = "lbInstallInfo";
            this.lbInstallInfo.Size = new System.Drawing.Size(45, 15);
            this.lbInstallInfo.TabIndex = 0;
            this.lbInstallInfo.Text = "[Install]";
            // 
            // lbInstallCount
            // 
            this.lbInstallCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbInstallCount.AutoSize = true;
            this.lbInstallCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lbInstallCount.Location = new System.Drawing.Point(527, 182);
            this.lbInstallCount.Name = "lbInstallCount";
            this.lbInstallCount.Size = new System.Drawing.Size(46, 17);
            this.lbInstallCount.TabIndex = 0;
            this.lbInstallCount.Text = "[C]/[T]";
            // 
            // lbDownloadCount
            // 
            this.lbDownloadCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbDownloadCount.AutoSize = true;
            this.lbDownloadCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lbDownloadCount.Location = new System.Drawing.Point(527, 73);
            this.lbDownloadCount.Name = "lbDownloadCount";
            this.lbDownloadCount.Size = new System.Drawing.Size(46, 17);
            this.lbDownloadCount.TabIndex = 0;
            this.lbDownloadCount.Text = "[C]/[T]";
            // 
            // lbDownloadInfo
            // 
            this.lbDownloadInfo.AutoSize = true;
            this.lbDownloadInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lbDownloadInfo.Location = new System.Drawing.Point(15, 50);
            this.lbDownloadInfo.Name = "lbDownloadInfo";
            this.lbDownloadInfo.Size = new System.Drawing.Size(69, 15);
            this.lbDownloadInfo.TabIndex = 0;
            this.lbDownloadInfo.Text = "[Download]";
            // 
            // lbInstalling
            // 
            this.lbInstalling.AutoSize = true;
            this.lbInstalling.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lbInstalling.Location = new System.Drawing.Point(14, 128);
            this.lbInstalling.Name = "lbInstalling";
            this.lbInstalling.Size = new System.Drawing.Size(76, 20);
            this.lbInstalling.TabIndex = 0;
            this.lbInstalling.Text = "Installing:";
            // 
            // lbDownloading
            // 
            this.lbDownloading.AutoSize = true;
            this.lbDownloading.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lbDownloading.Location = new System.Drawing.Point(14, 21);
            this.lbDownloading.Name = "lbDownloading";
            this.lbDownloading.Size = new System.Drawing.Size(105, 20);
            this.lbDownloading.TabIndex = 0;
            this.lbDownloading.Text = "Downloading:";
            // 
            // tpModify
            // 
            this.tpModify.Controls.Add(this.pChangeVer);
            this.tpModify.Controls.Add(this.btChangeVer);
            this.tpModify.Controls.Add(this.btUninstall);
            this.tpModify.Controls.Add(this.btRepair);
            this.tpModify.Controls.Add(this.lbModifyDesc);
            this.tpModify.Location = new System.Drawing.Point(4, 22);
            this.tpModify.Name = "tpModify";
            this.tpModify.Size = new System.Drawing.Size(593, 405);
            this.tpModify.TabIndex = 3;
            this.tpModify.Text = "MODIFY";
            this.tpModify.UseVisualStyleBackColor = true;
            // 
            // pChangeVer
            // 
            this.pChangeVer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pChangeVer.Controls.Add(this.rbOld);
            this.pChangeVer.Controls.Add(this.rbStable);
            this.pChangeVer.Controls.Add(this.rbPreOut);
            this.pChangeVer.Controls.Add(this.btInstall1);
            this.pChangeVer.Controls.Add(this.lbInstallError);
            this.pChangeVer.Controls.Add(this.lbVersionToInstall);
            this.pChangeVer.Controls.Add(this.cbOld);
            this.pChangeVer.Controls.Add(this.lbPerOutReq);
            this.pChangeVer.Controls.Add(this.lbCloseChangeVer);
            this.pChangeVer.Controls.Add(this.lbChangeVerDesc);
            this.pChangeVer.Enabled = false;
            this.pChangeVer.Location = new System.Drawing.Point(17, 128);
            this.pChangeVer.Name = "pChangeVer";
            this.pChangeVer.Size = new System.Drawing.Size(556, 259);
            this.pChangeVer.TabIndex = 3;
            this.pChangeVer.Visible = false;
            // 
            // rbOld
            // 
            this.rbOld.AutoSize = true;
            this.rbOld.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.rbOld.Location = new System.Drawing.Point(3, 112);
            this.rbOld.Name = "rbOld";
            this.rbOld.Size = new System.Drawing.Size(104, 21);
            this.rbOld.TabIndex = 2;
            this.rbOld.Text = "Old Version:";
            this.rbOld.UseVisualStyleBackColor = true;
            this.rbOld.CheckedChanged += new System.EventHandler(this.rbOld_CheckedChanged);
            // 
            // rbStable
            // 
            this.rbStable.AutoSize = true;
            this.rbStable.Checked = true;
            this.rbStable.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.rbStable.Location = new System.Drawing.Point(3, 82);
            this.rbStable.Name = "rbStable";
            this.rbStable.Size = new System.Drawing.Size(236, 21);
            this.rbStable.TabIndex = 2;
            this.rbStable.TabStop = true;
            this.rbStable.Text = "Latest Stable Version ([LATEST])";
            this.rbStable.UseVisualStyleBackColor = true;
            this.rbStable.CheckedChanged += new System.EventHandler(this.rbStable_CheckedChanged);
            // 
            // rbPreOut
            // 
            this.rbPreOut.AutoSize = true;
            this.rbPreOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.rbPreOut.Location = new System.Drawing.Point(3, 32);
            this.rbPreOut.Name = "rbPreOut";
            this.rbPreOut.Size = new System.Drawing.Size(246, 21);
            this.rbPreOut.TabIndex = 2;
            this.rbPreOut.Text = "Latest PreOut Version ([PREOUT])";
            this.rbPreOut.UseVisualStyleBackColor = true;
            this.rbPreOut.CheckedChanged += new System.EventHandler(this.rbPerOut_CheckedChanged);
            // 
            // btInstall1
            // 
            this.btInstall1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btInstall1.AutoSize = true;
            this.btInstall1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btInstall1.FlatAppearance.BorderSize = 0;
            this.btInstall1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btInstall1.Location = new System.Drawing.Point(6, 227);
            this.btInstall1.Name = "btInstall1";
            this.btInstall1.Size = new System.Drawing.Size(44, 23);
            this.btInstall1.TabIndex = 2;
            this.btInstall1.Text = "Install";
            this.btInstall1.UseVisualStyleBackColor = true;
            this.btInstall1.Click += new System.EventHandler(this.btInstall1_Click);
            // 
            // lbInstallError
            // 
            this.lbInstallError.AutoSize = true;
            this.lbInstallError.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lbInstallError.Location = new System.Drawing.Point(9, 173);
            this.lbInstallError.Name = "lbInstallError";
            this.lbInstallError.Size = new System.Drawing.Size(0, 17);
            this.lbInstallError.TabIndex = 1;
            // 
            // lbVersionToInstall
            // 
            this.lbVersionToInstall.AutoSize = true;
            this.lbVersionToInstall.Enabled = false;
            this.lbVersionToInstall.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lbVersionToInstall.Location = new System.Drawing.Point(20, 145);
            this.lbVersionToInstall.Name = "lbVersionToInstall";
            this.lbVersionToInstall.Size = new System.Drawing.Size(116, 17);
            this.lbVersionToInstall.TabIndex = 1;
            this.lbVersionToInstall.Text = "Version to install:";
            // 
            // cbOld
            // 
            this.cbOld.Enabled = false;
            this.cbOld.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.cbOld.FormattingEnabled = true;
            this.cbOld.Location = new System.Drawing.Point(142, 142);
            this.cbOld.Name = "cbOld";
            this.cbOld.Size = new System.Drawing.Size(120, 24);
            this.cbOld.TabIndex = 2;
            this.cbOld.SelectedIndexChanged += new System.EventHandler(this.cbOld_SelectedIndexChanged);
            // 
            // lbPerOutReq
            // 
            this.lbPerOutReq.AutoSize = true;
            this.lbPerOutReq.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lbPerOutReq.Location = new System.Drawing.Point(20, 59);
            this.lbPerOutReq.Name = "lbPerOutReq";
            this.lbPerOutReq.Size = new System.Drawing.Size(183, 17);
            this.lbPerOutReq.TabIndex = 1;
            this.lbPerOutReq.Text = "You meet the requirements.";
            // 
            // lbCloseChangeVer
            // 
            this.lbCloseChangeVer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbCloseChangeVer.AutoSize = true;
            this.lbCloseChangeVer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lbCloseChangeVer.Location = new System.Drawing.Point(524, 9);
            this.lbCloseChangeVer.Name = "lbCloseChangeVer";
            this.lbCloseChangeVer.Size = new System.Drawing.Size(20, 20);
            this.lbCloseChangeVer.TabIndex = 1;
            this.lbCloseChangeVer.Text = "X";
            this.lbCloseChangeVer.Click += new System.EventHandler(this.label5_Click);
            // 
            // lbChangeVerDesc
            // 
            this.lbChangeVerDesc.AutoSize = true;
            this.lbChangeVerDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lbChangeVerDesc.Location = new System.Drawing.Point(3, 9);
            this.lbChangeVerDesc.Name = "lbChangeVerDesc";
            this.lbChangeVerDesc.Size = new System.Drawing.Size(174, 20);
            this.lbChangeVerDesc.TabIndex = 1;
            this.lbChangeVerDesc.Text = "Please select a version:";
            // 
            // btChangeVer
            // 
            this.btChangeVer.AutoSize = true;
            this.btChangeVer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btChangeVer.FlatAppearance.BorderSize = 0;
            this.btChangeVer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btChangeVer.Location = new System.Drawing.Point(17, 94);
            this.btChangeVer.Name = "btChangeVer";
            this.btChangeVer.Size = new System.Drawing.Size(92, 23);
            this.btChangeVer.TabIndex = 2;
            this.btChangeVer.Text = "Change Version";
            this.btChangeVer.UseVisualStyleBackColor = true;
            this.btChangeVer.Click += new System.EventHandler(this.btChangeVer_Click);
            // 
            // btUninstall
            // 
            this.btUninstall.AutoSize = true;
            this.btUninstall.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btUninstall.FlatAppearance.BorderSize = 0;
            this.btUninstall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btUninstall.Location = new System.Drawing.Point(17, 65);
            this.btUninstall.Name = "btUninstall";
            this.btUninstall.Size = new System.Drawing.Size(57, 23);
            this.btUninstall.TabIndex = 2;
            this.btUninstall.Text = "Uninstall";
            this.btUninstall.UseVisualStyleBackColor = true;
            this.btUninstall.Click += new System.EventHandler(this.btUninstall_Click);
            // 
            // btRepair
            // 
            this.btRepair.AutoSize = true;
            this.btRepair.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btRepair.FlatAppearance.BorderSize = 0;
            this.btRepair.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btRepair.Location = new System.Drawing.Point(17, 36);
            this.btRepair.Name = "btRepair";
            this.btRepair.Size = new System.Drawing.Size(48, 23);
            this.btRepair.TabIndex = 2;
            this.btRepair.Text = "Repair";
            this.btRepair.UseVisualStyleBackColor = true;
            this.btRepair.Click += new System.EventHandler(this.btRepair_Click);
            // 
            // lbModifyDesc
            // 
            this.lbModifyDesc.AutoSize = true;
            this.lbModifyDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lbModifyDesc.Location = new System.Drawing.Point(13, 13);
            this.lbModifyDesc.Name = "lbModifyDesc";
            this.lbModifyDesc.Size = new System.Drawing.Size(284, 20);
            this.lbModifyDesc.TabIndex = 1;
            this.lbModifyDesc.Text = "Please select one of the options below.";
            // 
            // tpDone
            // 
            this.tpDone.Controls.Add(this.tbDoneError);
            this.tpDone.Controls.Add(this.lbDoneDesc);
            this.tpDone.Controls.Add(this.flowLayoutPanel1);
            this.tpDone.Location = new System.Drawing.Point(4, 22);
            this.tpDone.Name = "tpDone";
            this.tpDone.Size = new System.Drawing.Size(593, 405);
            this.tpDone.TabIndex = 4;
            this.tpDone.Text = "DONE";
            this.tpDone.UseVisualStyleBackColor = true;
            // 
            // tbDoneError
            // 
            this.tbDoneError.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDoneError.Location = new System.Drawing.Point(17, 27);
            this.tbDoneError.Multiline = true;
            this.tbDoneError.Name = "tbDoneError";
            this.tbDoneError.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbDoneError.Size = new System.Drawing.Size(556, 331);
            this.tbDoneError.TabIndex = 2;
            // 
            // lbDoneDesc
            // 
            this.lbDoneDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbDoneDesc.AutoSize = true;
            this.lbDoneDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lbDoneDesc.Location = new System.Drawing.Point(13, 4);
            this.lbDoneDesc.Name = "lbDoneDesc";
            this.lbDoneDesc.Size = new System.Drawing.Size(147, 20);
            this.lbDoneDesc.TabIndex = 1;
            this.lbDoneDesc.Text = "Your Yorot is ready.";
            this.lbDoneDesc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btClose);
            this.flowLayoutPanel1.Controls.Add(this.btSendFeedback);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 364);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(593, 41);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // btClose
            // 
            this.btClose.AutoSize = true;
            this.btClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btClose.FlatAppearance.BorderSize = 0;
            this.btClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btClose.Location = new System.Drawing.Point(3, 3);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(43, 23);
            this.btClose.TabIndex = 3;
            this.btClose.Text = "Close";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // btSendFeedback
            // 
            this.btSendFeedback.AutoSize = true;
            this.btSendFeedback.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btSendFeedback.FlatAppearance.BorderSize = 0;
            this.btSendFeedback.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSendFeedback.Location = new System.Drawing.Point(52, 3);
            this.btSendFeedback.Name = "btSendFeedback";
            this.btSendFeedback.Size = new System.Drawing.Size(93, 23);
            this.btSendFeedback.TabIndex = 4;
            this.btSendFeedback.Text = "Send Feedback";
            this.btSendFeedback.UseVisualStyleBackColor = true;
            this.btSendFeedback.Click += new System.EventHandler(this.btSendFeedback_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 461);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmMain";
            this.Text = "Yorot Installer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.SizeChanged += new System.EventHandler(this.frmMain_SizeChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tpStart.ResumeLayout(false);
            this.tpFirst.ResumeLayout(false);
            this.tpFirst.PerformLayout();
            this.tpProgress.ResumeLayout(false);
            this.tpProgress.PerformLayout();
            this.pInstall.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbInstall)).EndInit();
            this.pDownload.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbDownload)).EndInit();
            this.tpModify.ResumeLayout(false);
            this.tpModify.PerformLayout();
            this.pChangeVer.ResumeLayout(false);
            this.pChangeVer.PerformLayout();
            this.tpDone.ResumeLayout(false);
            this.tpDone.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Label label3;
        private PictureBox pictureBox2;
        private Label label4;
        private Panel panel1;
        private TabControl tabControl1;
        private TabPage tpStart;
        private TabPage tpFirst;
        private Button btInstall;
        private Label lbReady;
        private TabPage tpProgress;
        private Label lbDownloadInfo;
        private Label lbDownloading;
        private Panel pInstall;
        private PictureBox pbInstall;
        private Panel pDownload;
        private PictureBox pbDownload;
        private Label lbInstallInfo;
        private Label lbInstalling;
        private TabPage tpModify;
        private TabPage tpDone;
        private Panel pChangeVer;
        private RadioButton rbOld;
        private RadioButton rbStable;
        private RadioButton rbPreOut;
        private Label lbVersionToInstall;
        private ComboBox cbOld;
        private Label lbPerOutReq;
        private Label lbChangeVerDesc;
        private Button btChangeVer;
        private Button btUninstall;
        private Button btRepair;
        private Label lbModifyDesc;
        private Button btInstall1;
        private ComboBox cbLang;
        private TextBox tbDoneError;
        private Label lbDoneDesc;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button btClose;
        private Button btSendFeedback;
        private Label lbCloseChangeVer;
        private Label lbInstallError;
        private Label lbInstallCount;
        private Label lbDownloadCount;
        private Timer timer1;
    }
}