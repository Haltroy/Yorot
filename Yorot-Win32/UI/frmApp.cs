using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Yorot.UI
{
    public partial class frmApp : Form
    {
        public YorotApp assocApp;
        public frmMain assocForm;
        public YorotAppLayout assocLayout;
        object appContainer = null;
        public frmApp(string appcn)
        {
            InitializeComponent();
            assocAppAndForm(YorotGlobal.Settings.AppMan.FindByAppCN(appcn));
        }
        public frmApp(YorotApp app)
        {
            InitializeComponent();
            assocAppAndForm(app);
        }
        private void assocAppAndForm(YorotApp app)
        {
            assocApp = app;
            if (app.isSystemApp)
            {
                Form appc = null;
                switch (app.AppCodeName)
                {
                    case "com.haltroy.calc":
                        appc = new SystemApp.calc() { TopLevel = false, Visible = true, Dock = DockStyle.Fill, FormBorderStyle = FormBorderStyle.None, };
                        appContainer = appc;
                        pApp.Controls.Add(appc);
                        break;
                    case "com.haltroy.settings":
                        appc = new SystemApp.settings() { TopLevel = false, Visible = true, Dock = DockStyle.Fill, FormBorderStyle = FormBorderStyle.None, };
                        appContainer = appc;
                        pApp.Controls.Add(appc);
                        break;
                    case "com.haltroy.store":
                        appc = new SystemApp.store() { TopLevel = false, Visible = true, Dock = DockStyle.Fill, FormBorderStyle = FormBorderStyle.None, };
                        appContainer = appc;
                        pApp.Controls.Add(appc);
                        break;
                    case "com.haltroy.calendar":
                        appc = new SystemApp.calendar() { TopLevel = false, Visible = true, Dock = DockStyle.Fill, FormBorderStyle = FormBorderStyle.None, };
                        appContainer = appc;
                        pApp.Controls.Add(appc);
                        break;
                    case "com.haltroy.notepad":
                        appc = new SystemApp.notepad() { TopLevel = false, Visible = true, Dock = DockStyle.Fill, FormBorderStyle = FormBorderStyle.None, };
                        appContainer = appc;
                        pApp.Controls.Add(appc);
                        break;
                    case "com.haltroy.console":
                        appc = new SystemApp.console() { TopLevel = false, Visible = true, Dock = DockStyle.Fill, FormBorderStyle = FormBorderStyle.None, };
                        appContainer = appc;
                        pApp.Controls.Add(appc);
                        break;
                    case "com.haltroy.colman":
                        appc = new SystemApp.collections() { TopLevel = false, Visible = true, Dock = DockStyle.Fill, FormBorderStyle = FormBorderStyle.None, };
                        appContainer = appc;
                        pApp.Controls.Add(appc);
                        break;
                    case "com.haltroy.fileman":
                        appc = new SystemApp.fileman() { TopLevel = false, Visible = true, Dock = DockStyle.Fill, FormBorderStyle = FormBorderStyle.None, };
                        appContainer = appc;
                        pApp.Controls.Add(appc);
                        break;
                    case "com.haltroy.packdist":
                        appc = new SystemApp.yopad() { TopLevel = false, Visible = true, Dock = DockStyle.Fill, FormBorderStyle = FormBorderStyle.None, };
                        appContainer = appc;
                        pApp.Controls.Add(appc);
                        break;
                    case "com.haltroy.spacepass":
                        appc = new SystemApp.spacepass() { TopLevel = false, Visible = true, Dock = DockStyle.Fill, FormBorderStyle = FormBorderStyle.None, };
                        appContainer = appc;
                        pApp.Controls.Add(appc);
                        break;
                }
            }
            else
            {
                assocLayout.AssocForm = this;
            }
            Text = app.AppName;
            lbTitle.Text = app.AppName;
            pbIcon.Image = app.GetAppIcon();
            Icon = YorotTools.IconFromImage(pbIcon.Image);
        }

        public bool freeMode { get; set; } = false;
        private void p_mdown(object sender,MouseEventArgs e)
        {
            OnMouseDown(e); 
        }

        private void p_mdclick(object sender, MouseEventArgs e)
        {
            OnMouseDoubleClick(e);
        }
        private void frmApp_FormClosing(object sender, FormClosingEventArgs e)
        {
            TabControl tcappman = assocLayout.AssocTab.Parent as TabControl;
            if (tcappman.SelectedTab == assocLayout.AssocTab)
            {
                assocForm.loadMainTab();
            }
            if (tcappman.InvokeRequired)
            {
                tcappman.Invoke(new Action(() =>
                {
                    tcappman.TabPages.Remove(assocLayout.AssocTab);
                }));
            }
            else
            {
                tcappman.TabPages.Remove(assocLayout.AssocTab);
            }
            assocLayout.AssocTab = null;
            assocApp.Layouts.Remove(assocLayout);
            var hasMoreSessions = assocApp.Layouts.FindAll(i => i.AssocItem.AssocFrmMain == assocForm).Count > 0;
            if (assocApp.isPinned) { if (assocLayout.AssocItem.InvokeRequired) { assocLayout.AssocItem.Invoke(new Action(() => assocLayout.AssocItem.Invalidate())); } else { assocLayout.AssocItem.Invalidate(); } }
            else
            {
                if (assocApp.AppCodeName != "com.haltroy.settings" && !hasMoreSessions)
                {
                    FlowLayoutPanel pappdrawer = assocLayout.AssocItem.Parent as FlowLayoutPanel;
                    if (pappdrawer.InvokeRequired)
                    {
                        pappdrawer.Invoke(new Action(() => pappdrawer.Controls.Remove(assocLayout.AssocItem)));
                    }
                    else
                    {
                        pappdrawer.Controls.Remove(assocLayout.AssocItem);
                    }
                }
                else if (assocApp.AppCodeName != "com.haltroy.settings" && hasMoreSessions) // Redraw it cuzz its updated.
                {
                    if (assocLayout.AssocItem.InvokeRequired) { assocLayout.AssocItem.Invoke(new Action(() => assocLayout.AssocItem.Invalidate())); } else { assocLayout.AssocItem.Invalidate(); }
                }
            }
        }
        System.Drawing.Size fmSize { get; set; } = new System.Drawing.Size(600, 500);

        private void htButton4_Click(object sender, EventArgs e)
        {
            if (!freeMode)
            {
                Hide();
                TopMost = false;
                btPopOut.Text = "▌";
                btMaximize.Visible = true;
                assocForm.loadMainTab();
                btMaximize.Enabled = true;
                btMinimize.Visible = true;
                btMinimize.Enabled = true;
                Parent = null;
                assocLayout.AssocTab.Controls.Remove(this);
                Dock = DockStyle.None;
                TopLevel = true;
                FormBorderStyle = FormBorderStyle.Sizable;
                pTitle.MouseDown += p_mdown;
                pTitle.MouseDoubleClick += p_mdclick;
                lbTitle.MouseDown += p_mdown;
                lbTitle.MouseDoubleClick += p_mdclick;
                Size = fmSize;
                Show();
                freeMode = true;
            }
            else
            {
                Hide();
                fmSize = Size;
                TopMost = false;
                TopLevel = false;
                btPopOut.Text = "□";
                assocForm.loadSpecificTab(assocLayout.AssocTab);
                btMaximize.Visible = false;
                btMaximize.Enabled = false;
                btMinimize.Visible = false;
                btMinimize.Enabled = false;
                Parent = assocLayout.AssocTab;
                FormBorderStyle = FormBorderStyle.None;
                pTitle.MouseDown -= p_mdown;
                pTitle.MouseDoubleClick -= p_mdclick;
                lbTitle.MouseDown -= p_mdown;
                lbTitle.MouseDoubleClick -= p_mdclick;
                Dock = DockStyle.Fill;
                assocLayout.AssocTab.Controls.Add(this);
                Show();
                freeMode = false;
            }
        }

        private void htButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void htButton3_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void htButton2_Click(object sender, EventArgs e)
        {
            WindowState = WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (assocApp.isSystemApp)
            {
                var frm = appContainer as Form;
                Text = frm.Text;
                lbTitle.Text = frm.Text;
                Icon = frm.Icon;
            }
        }

        private void pbIcon_Click(object sender, EventArgs e)
        {
            if (freeMode)
            {
                TopMost = !TopMost;
                pbIcon.BorderStyle = TopMost ? BorderStyle.FixedSingle: BorderStyle.None;
            }
        }
    }
}
