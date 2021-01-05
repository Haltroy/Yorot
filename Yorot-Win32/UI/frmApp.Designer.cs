
namespace Yorot.UI
{
    partial class frmApp
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

        #region HTForms
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(System.IntPtr hWnd, int Msg, int wParam, int lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                if (this.FormBorderStyle == System.Windows.Forms.FormBorderStyle.None) { return base.CreateParams; } else 
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

                    System.Windows.Forms.CreateParams cp = base.CreateParams;


                    cp.Style = WS_CLIPCHILDREN | WS_CLIPSIBLINGS
                | WS_MAXIMIZEBOX | WS_MINIMIZEBOX
                | WS_SYSMENU | WS_THICKFRAME;

                    if (DesignMode)
                    {
                        cp.Style = cp.Style | WS_CHILDWINDOW;
                    }

                    int ClassFlags = CS_DBLCLKS;
                    int OSVER = System.Environment.OSVersion.Version.Major * 10;
                    OSVER += System.Environment.OSVersion.Version.Minor;

                    if (OSVER >= 51)
                    {
                        ClassFlags = ClassFlags | CS_DROPSHADOW;
                    }

                    cp.ClassStyle = ClassFlags;

                    return cp;
                }
            }
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
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

        protected override void OnMouseDoubleClick(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (WindowState == System.Windows.Forms.FormWindowState.Maximized)
            {
                WindowState = System.Windows.Forms.FormWindowState.Normal;
            }
            else if (WindowState == System.Windows.Forms.FormWindowState.Normal)
            {
                MaximizedBounds = System.Windows.Forms.Screen.FromHandle(Handle).WorkingArea;
                WindowState = System.Windows.Forms.FormWindowState.Maximized;
            }
            Invalidate();
        }
        #endregion HTForms

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pApp = new System.Windows.Forms.Panel();
            this.pTitle = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btClose = new HTAlt.WinForms.HTButton();
            this.btMaximize = new HTAlt.WinForms.HTButton();
            this.btMinimize = new HTAlt.WinForms.HTButton();
            this.btPopOut = new HTAlt.WinForms.HTButton();
            this.pTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pApp
            // 
            this.pApp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pApp.Location = new System.Drawing.Point(1, 44);
            this.pApp.Name = "pApp";
            this.pApp.Size = new System.Drawing.Size(400, 304);
            this.pApp.TabIndex = 0;
            // 
            // pTitle
            // 
            this.pTitle.Controls.Add(this.flowLayoutPanel1);
            this.pTitle.Controls.Add(this.label1);
            this.pTitle.Controls.Add(this.pictureBox1);
            this.pTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTitle.Location = new System.Drawing.Point(0, 0);
            this.pTitle.Name = "pTitle";
            this.pTitle.Size = new System.Drawing.Size(400, 44);
            this.pTitle.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.5F);
            this.label1.Location = new System.Drawing.Point(43, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "AppTitle";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Yorot.Properties.Resources.Yorot;
            this.pictureBox1.Location = new System.Drawing.Point(9, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(28, 28);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btClose);
            this.flowLayoutPanel1.Controls.Add(this.btMaximize);
            this.flowLayoutPanel1.Controls.Add(this.btMinimize);
            this.flowLayoutPanel1.Controls.Add(this.btPopOut);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(291, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(109, 44);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // btClose
            // 
            this.btClose.AutoColor = true;
            this.btClose.ButtonImage = null;
            this.btClose.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btClose.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btClose.DrawImage = false;
            this.btClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btClose.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btClose.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btClose.Location = new System.Drawing.Point(81, 9);
            this.btClose.Margin = new System.Windows.Forms.Padding(0, 9, 3, 0);
            this.btClose.Name = "btClose";
            this.btClose.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btClose.Size = new System.Drawing.Size(25, 25);
            this.btClose.TabIndex = 0;
            this.btClose.Text = "X";
            this.btClose.Click += new System.EventHandler(this.htButton1_Click);
            // 
            // btMaximize
            // 
            this.btMaximize.AutoColor = true;
            this.btMaximize.ButtonImage = null;
            this.btMaximize.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btMaximize.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btMaximize.DrawImage = false;
            this.btMaximize.Enabled = false;
            this.btMaximize.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btMaximize.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btMaximize.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btMaximize.Location = new System.Drawing.Point(56, 9);
            this.btMaximize.Margin = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.btMaximize.Name = "btMaximize";
            this.btMaximize.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btMaximize.Size = new System.Drawing.Size(25, 25);
            this.btMaximize.TabIndex = 0;
            this.btMaximize.Text = "□";
            this.btMaximize.Visible = false;
            this.btMaximize.Click += new System.EventHandler(this.htButton2_Click);
            // 
            // btMinimize
            // 
            this.btMinimize.AutoColor = true;
            this.btMinimize.ButtonImage = null;
            this.btMinimize.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btMinimize.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btMinimize.DrawImage = false;
            this.btMinimize.Enabled = false;
            this.btMinimize.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btMinimize.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btMinimize.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btMinimize.Location = new System.Drawing.Point(31, 9);
            this.btMinimize.Margin = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.btMinimize.Name = "btMinimize";
            this.btMinimize.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btMinimize.Size = new System.Drawing.Size(25, 25);
            this.btMinimize.TabIndex = 0;
            this.btMinimize.Text = "-";
            this.btMinimize.Visible = false;
            this.btMinimize.Click += new System.EventHandler(this.htButton3_Click);
            // 
            // btPopOut
            // 
            this.btPopOut.AutoColor = true;
            this.btPopOut.ButtonImage = null;
            this.btPopOut.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btPopOut.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btPopOut.DrawImage = false;
            this.btPopOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btPopOut.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btPopOut.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btPopOut.Location = new System.Drawing.Point(6, 9);
            this.btPopOut.Margin = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.btPopOut.Name = "btPopOut";
            this.btPopOut.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btPopOut.Size = new System.Drawing.Size(25, 25);
            this.btPopOut.TabIndex = 0;
            this.btPopOut.Text = "□";
            this.btPopOut.Click += new System.EventHandler(this.htButton4_Click);
            // 
            // frmApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 350);
            this.Controls.Add(this.pApp);
            this.Controls.Add(this.pTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmApp";
            this.Text = "frmApp";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmApp_FormClosing);
            this.pTitle.ResumeLayout(false);
            this.pTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pApp;
        private System.Windows.Forms.Panel pTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private HTAlt.WinForms.HTButton btClose;
        private HTAlt.WinForms.HTButton btMaximize;
        private HTAlt.WinForms.HTButton btMinimize;
        private HTAlt.WinForms.HTButton btPopOut;
    }
}