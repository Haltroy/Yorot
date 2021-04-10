using HTAlt;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Yorot.UI
{
    public partial class frmApp : Form
    {
        public YorotApp assocApp;
        public frmMain assocForm;
        public WinAppLayout assocLayout;
        object appContainer = null;
        public frmApp(string appcn)
        {
            InitializeComponent();
            assocAppAndForm(YorotGlobal.Main.AppMan.FindByAppCN(appcn));
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
            pbIcon.Image = YorotTools.GetAppIcon(app);
            Icon = HTAlt.Tools.IconFromImage(pbIcon.Image);
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
            var hasMoreSessions = assocApp.Layouts.FindAll(i => (i is WinAppLayout) && (i as WinAppLayout).AssocItem.AssocFrmMain == assocForm).Count > 0;
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
                if (assocForm.InvokeRequired)
                {
                    assocForm.Invoke(new Action(() => assocForm.openDrawer()));
                }else
                {
                    assocForm.openDrawer();
                }
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
            MaximizedBounds = shiftFS ? Screen.FromHandle(Handle).Bounds : Screen.FromHandle(Handle).WorkingArea;
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
            LoadTheme();
        }
        private string syncedTheme = string.Empty;
        private void LoadTheme(bool force = false)
        {
            var theme = YorotGlobal.Main.CurrentTheme;
            var themeid = theme.BackColor.ToHex() + "-" + theme.ForeColor.ToHex() + "-" + theme.OverlayColor.ToHex() + "-" + theme.ArtColor.ToHex();
            if (force || syncedTheme != themeid)
            {
                syncedTheme = themeid;
                BackColor = theme.BackColor;
                ForeColor = theme.ForeColor;
                pTitle.BackColor = theme.BackColor2;
                pTitle.ForeColor = theme.ForeColor;
                flowLayoutPanel1.BackColor = theme.BackColor2;
                flowLayoutPanel1.ForeColor = theme.ForeColor;
                btClose.BackColor = theme.BackColor2;
                btClose.ClickColor = theme.OverlayColor3;
                btClose.HoverColor = theme.OverlayColor2;
                btClose.NormalColor = theme.OverlayColor;
                btClose.ForeColor = HTAlt.Tools.AutoWhiteBlack(theme.OverlayColor);
                btMaximize.BackColor = theme.BackColor2;
                btMaximize.ClickColor = theme.OverlayColor3;
                btMaximize.HoverColor = theme.OverlayColor2;
                btMaximize.NormalColor = theme.OverlayColor;
                btMaximize.ForeColor = HTAlt.Tools.AutoWhiteBlack(theme.OverlayColor);
                btMinimize.BackColor = theme.BackColor2;
                btMinimize.ClickColor = theme.OverlayColor3;
                btMinimize.HoverColor = theme.OverlayColor2;
                btMinimize.NormalColor = theme.OverlayColor;
                btMinimize.ForeColor = HTAlt.Tools.AutoWhiteBlack(theme.OverlayColor);
                btPopOut.BackColor = theme.BackColor2;
                btPopOut.ClickColor = theme.OverlayColor3;
                btPopOut.HoverColor = theme.OverlayColor2;
                btPopOut.NormalColor = theme.OverlayColor;
                btPopOut.ForeColor = HTAlt.Tools.AutoWhiteBlack(theme.OverlayColor);
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
        private bool shiftFS = false; 
        private void frmApp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Shift || e.KeyCode == Keys.ShiftKey || e.Shift || e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey) { shiftFS = true; }
        }

        private void frmApp_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Shift || e.KeyCode == Keys.ShiftKey || e.Shift || e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey) { shiftFS = false; }
        }
    }
}
