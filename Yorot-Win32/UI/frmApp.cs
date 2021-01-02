using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Yorot.UI
{
    public partial class frmApp : Form
    {
        public YorotApp assocApp;
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
                        pApp.Controls.Add(new SystemApp.calc() { TopLevel = false, Visible = true, Dock = DockStyle.Fill, FormBorderStyle = FormBorderStyle.None, });
                        break;
                    default:
                        Label lbPlaceHolder = new Label() { Text = app.AppCodeName, AutoSize = true, Visible = true };
                        Form gaster = new Form() { TopLevel = false, Visible = true, Dock = DockStyle.Fill, FormBorderStyle = FormBorderStyle.None, };
                        gaster.Controls.Add(lbPlaceHolder);
                        pApp.Controls.Add(gaster);
                        break;
                }
            }
            else
            {
                assocApp.AssocForm = this;
            }
            Text = app.AppName;
            label1.Text = app.AppName;
            pictureBox1.Image = app.GetAppIcon();
            Icon = YorotGlobal.IconFromImage(pictureBox1.Image);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            /// TODO: Add popping window feature.
        }

        private void label2_Click(object sender, EventArgs e)
        {
            TabControl tcappman = assocApp.AssocTab.Parent as TabControl;
            if (tcappman.InvokeRequired)
            {
                tcappman.Invoke(new Action(() =>
                {
                    tcappman.SelectedIndex = 0;
                    tcappman.TabPages.Remove(assocApp.AssocTab);
                }));
            }
            else
            {
                tcappman.SelectedIndex = 0;
                tcappman.TabPages.Remove(assocApp.AssocTab);
            }
            FlowLayoutPanel pappdrawer = assocApp.AssocPB.Parent as FlowLayoutPanel;
            if (pappdrawer.InvokeRequired)
            {
                pappdrawer.Invoke(new Action(() => pappdrawer.Controls.Remove(assocApp.AssocPB)));
            }
            else
            {
                pappdrawer.Controls.Remove(assocApp.AssocPB);
            }
            assocApp.AssocTab = null;
            assocApp.AssocForm = null;
            assocApp.AssocPB = null;
            Close();
        }
    }
}
