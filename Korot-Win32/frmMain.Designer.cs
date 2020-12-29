
using System;
using System.Windows.Forms;

namespace Korot_Win32
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("Web Browser", 0);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("Store");
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("Settings");
            this.pAppDrawer = new System.Windows.Forms.Panel();
            this.pAppGrid = new System.Windows.Forms.Panel();
            this.flpFavApps = new System.Windows.Forms.FlowLayoutPanel();
            this.pbSettings = new System.Windows.Forms.PictureBox();
            this.pbKorot = new System.Windows.Forms.PictureBox();
            this.tcAppMan = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lvApps = new System.Windows.Forms.ListView();
            this.ilAppMan = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pAppDrawer.SuspendLayout();
            this.pAppGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbKorot)).BeginInit();
            this.tcAppMan.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pAppDrawer
            // 
            this.pAppDrawer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.pAppDrawer.Controls.Add(this.pAppGrid);
            this.pAppDrawer.Controls.Add(this.tcAppMan);
            this.pAppDrawer.Controls.Add(this.label1);
            this.pAppDrawer.Dock = System.Windows.Forms.DockStyle.Left;
            this.pAppDrawer.Location = new System.Drawing.Point(0, 0);
            this.pAppDrawer.Name = "pAppDrawer";
            this.pAppDrawer.Size = new System.Drawing.Size(600, 586);
            this.pAppDrawer.TabIndex = 1;
            // 
            // pAppGrid
            // 
            this.pAppGrid.Controls.Add(this.flpFavApps);
            this.pAppGrid.Controls.Add(this.pbSettings);
            this.pAppGrid.Controls.Add(this.pbKorot);
            this.pAppGrid.Dock = System.Windows.Forms.DockStyle.Right;
            this.pAppGrid.Location = new System.Drawing.Point(540, 0);
            this.pAppGrid.Name = "pAppGrid";
            this.pAppGrid.Size = new System.Drawing.Size(55, 586);
            this.pAppGrid.TabIndex = 3;
            // 
            // flpFavApps
            // 
            this.flpFavApps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpFavApps.Location = new System.Drawing.Point(8, 50);
            this.flpFavApps.Name = "flpFavApps";
            this.flpFavApps.Size = new System.Drawing.Size(40, 487);
            this.flpFavApps.TabIndex = 1;
            // 
            // pbSettings
            // 
            this.pbSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pbSettings.Image = ((System.Drawing.Image)(resources.GetObject("pbSettings.Image")));
            this.pbSettings.Location = new System.Drawing.Point(8, 543);
            this.pbSettings.Name = "pbSettings";
            this.pbSettings.Size = new System.Drawing.Size(40, 40);
            this.pbSettings.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbSettings.TabIndex = 0;
            this.pbSettings.TabStop = false;
            // 
            // pbKorot
            // 
            this.pbKorot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbKorot.Image = global::Korot_Win32.Properties.Resources.Korot;
            this.pbKorot.Location = new System.Drawing.Point(8, 3);
            this.pbKorot.Name = "pbKorot";
            this.pbKorot.Size = new System.Drawing.Size(40, 40);
            this.pbKorot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbKorot.TabIndex = 0;
            this.pbKorot.TabStop = false;
            // 
            // tcAppMan
            // 
            this.tcAppMan.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcAppMan.Controls.Add(this.tabPage1);
            this.tcAppMan.Location = new System.Drawing.Point(-9, -29);
            this.tcAppMan.Name = "tcAppMan";
            this.tcAppMan.SelectedIndex = 0;
            this.tcAppMan.Size = new System.Drawing.Size(556, 632);
            this.tcAppMan.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lvApps);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(548, 606);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tpApps";
            // 
            // lvApps
            // 
            this.lvApps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvApps.HideSelection = false;
            listViewItem1.Tag = "com.haltroy.korot";
            listViewItem2.Tag = "com.haltroy.store";
            listViewItem3.Tag = "com.korot.settings";
            this.lvApps.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3});
            this.lvApps.LargeImageList = this.ilAppMan;
            this.lvApps.Location = new System.Drawing.Point(3, 3);
            this.lvApps.Name = "lvApps";
            this.lvApps.Size = new System.Drawing.Size(542, 600);
            this.lvApps.SmallImageList = this.ilAppMan;
            this.lvApps.TabIndex = 0;
            this.lvApps.UseCompatibleStateImageBehavior = false;
            this.lvApps.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // ilAppMan
            // 
            this.ilAppMan.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilAppMan.ImageStream")));
            this.ilAppMan.TransparentColor = System.Drawing.Color.Transparent;
            this.ilAppMan.Images.SetKeyName(0, "Korot");
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.label1.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.label1.Dock = System.Windows.Forms.DockStyle.Right;
            this.label1.Location = new System.Drawing.Point(595, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(5, 586);
            this.label1.TabIndex = 0;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDown);
            this.label1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label1_MouseMove);
            this.label1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label1_MouseUp);
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(809, 586);
            this.Controls.Add(this.pAppDrawer);
            this.Name = "frmMain";
            this.Text = "Korot";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.MouseLeave += new System.EventHandler(this.Form1_MouseLeave);
            this.pAppDrawer.ResumeLayout(false);
            this.pAppGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbKorot)).EndInit();
            this.tcAppMan.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pAppDrawer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TabControl tcAppMan;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel pAppGrid;
        private System.Windows.Forms.FlowLayoutPanel flpFavApps;
        private System.Windows.Forms.PictureBox pbSettings;
        private System.Windows.Forms.PictureBox pbKorot;
        private ListView lvApps;
        private ImageList ilAppMan;
    }
}

