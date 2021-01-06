using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Yorot.UI
{
    public partial class frmApp : Form
    {
        public YorotApp assocApp;
        public frmMain assocForm;
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
                switch (app.AppCodeName)
                {
                    case "com.haltroy.calc":
                        SystemApp.calc calc = new SystemApp.calc() { TopLevel = false, Visible = true, Dock = DockStyle.Fill, FormBorderStyle = FormBorderStyle.None, };
                        appContainer = calc;
                        pApp.Controls.Add(calc);
                        break;
                    default:
                        Label lbPlaceHolder = new Label() { Text = app.AppCodeName, AutoSize = true, Visible = true };
                        Form gaster = new Form() { TopLevel = false, Visible = true, Dock = DockStyle.Fill, FormBorderStyle = FormBorderStyle.None, };
                        gaster.Controls.Add(lbPlaceHolder);
                        appContainer = gaster;
                        pApp.Controls.Add(gaster);
                        break;
                }
            }
            else
            {
                assocApp.AssocForm = this;
            }
            Text = app.AppName;
            lbTitle.Text = app.AppName;
            pbIcon.Image = app.GetAppIcon();
            Icon = YorotGlobal.IconFromImage(pbIcon.Image);
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
            TabControl tcappman = assocApp.AssocTab.Parent as TabControl;
            assocForm.loadMainTab();
            if (tcappman.InvokeRequired)
            {
                tcappman.Invoke(new Action(() =>
                {
                    tcappman.TabPages.Remove(assocApp.AssocTab);
                }));
            }
            else
            {
                tcappman.TabPages.Remove(assocApp.AssocTab);
            }
            assocApp.AssocTab = null;
            assocApp.AssocForm = null;
            if (assocApp.AppCodeName != "com.haltroy.settings")
            {
                FlowLayoutPanel pappdrawer = assocApp.AssocPB.Parent as FlowLayoutPanel;
                if (pappdrawer.InvokeRequired)
                {
                    pappdrawer.Invoke(new Action(() => pappdrawer.Controls.Remove(assocApp.AssocPB)));
                }
                else
                {
                    pappdrawer.Controls.Remove(assocApp.AssocPB);
                }
                assocApp.AssocPB = null;
            }
        }

        private void htButton4_Click(object sender, EventArgs e)
        {
            if (!freeMode)
            {
                Hide();
                btPopOut.Text = "▌";
                btMaximize.Visible = true;
                assocForm.loadMainTab();
                btMaximize.Enabled = true;
                btMinimize.Visible = true;
                btMinimize.Enabled = true;
                Parent = null;
                assocApp.AssocTab.Controls.Remove(this);
                Dock = DockStyle.None;
                TopLevel = true;
                FormBorderStyle = FormBorderStyle.Sizable;
                pTitle.MouseDown += p_mdown;
                pTitle.MouseDoubleClick += p_mdclick;
                pbIcon.MouseDown += p_mdown;
                pbIcon.MouseDoubleClick += p_mdclick;
                lbTitle.MouseDown += p_mdown;
                lbTitle.MouseDoubleClick += p_mdclick;
                Show();
                freeMode = true;
            }
            else
            {
                Hide();
                TopLevel = false;
                btPopOut.Text = "□";
                assocForm.loadSpecificTab(assocApp.AssocTab);
                btMaximize.Visible = false;
                btMaximize.Enabled = false;
                btMinimize.Visible = false;
                btMinimize.Enabled = false;
                Parent = assocApp.AssocTab;
                FormBorderStyle = FormBorderStyle.None;
                pTitle.MouseDown -= p_mdown;
                pTitle.MouseDoubleClick -= p_mdclick;
                pbIcon.MouseDown -= p_mdown;
                pbIcon.MouseDoubleClick -= p_mdclick;
                lbTitle.MouseDown -= p_mdown;
                lbTitle.MouseDoubleClick -= p_mdclick;
                Dock = DockStyle.Fill;
                assocApp.AssocTab.Controls.Add(this);
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
    }
}
