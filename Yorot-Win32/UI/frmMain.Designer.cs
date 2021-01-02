
using System;
using System.Windows.Forms;

namespace Yorot
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
            this.pAppDrawer = new System.Windows.Forms.Panel();
            this.pAppGrid = new System.Windows.Forms.Panel();
            this.flpFavApps = new System.Windows.Forms.FlowLayoutPanel();
            this.pbSettings = new System.Windows.Forms.PictureBox();
            this.pbYorot = new System.Windows.Forms.PictureBox();
            this.tcAppMan = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lvApps = new System.Windows.Forms.ListView();
            this.ilAppMan = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pAppDrawer.SuspendLayout();
            this.pAppGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbYorot)).BeginInit();
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
            this.pAppGrid.Controls.Add(this.pbYorot);
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
            this.pbSettings.Image = global::Yorot.Properties.Resources.Settings;
            this.pbSettings.Location = new System.Drawing.Point(8, 543);
            this.pbSettings.Name = "pbSettings";
            this.pbSettings.Size = new System.Drawing.Size(40, 40);
            this.pbSettings.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbSettings.TabIndex = 0;
            this.pbSettings.TabStop = false;
            // 
            // pbYorot
            // 
            this.pbYorot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbYorot.Image = global::Yorot.Properties.Resources.Yorot;
            this.pbYorot.Location = new System.Drawing.Point(8, 3);
            this.pbYorot.Name = "pbYorot";
            this.pbYorot.Size = new System.Drawing.Size(40, 40);
            this.pbYorot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbYorot.TabIndex = 0;
            this.pbYorot.TabStop = false;
            this.pbYorot.Click += new System.EventHandler(this.pbYorot_Click);
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
            this.ilAppMan.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.ilAppMan.ImageSize = new System.Drawing.Size(64, 64);
            this.ilAppMan.TransparentColor = System.Drawing.Color.Transparent;
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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "Yorot";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.MouseLeave += new System.EventHandler(this.Form1_MouseLeave);
            this.pAppDrawer.ResumeLayout(false);
            this.pAppGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbYorot)).EndInit();
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
        private System.Windows.Forms.PictureBox pbYorot;
        private ListView lvApps;
        private ImageList ilAppMan;
    }
}

