using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yorot.UI.SystemApp
{
    public partial class settings : Form
    {
        public settings()
        {
            InitializeComponent();
            Icon = YorotGlobal.IconFromImage(Properties.Resources.Settings);
            panel1.AutoScroll = false;
            pbHamMenu.Location = new Point(panel1.Width - (pbHamMenu.Width + 7), pbHamMenu.Location.Y);
            panel1.Invalidate();
            tmrAnimate.Start();
        }

        private void tmrAnimate_Tick(object sender, EventArgs e)
        {
            switch (panel1.AutoScroll)
            {
                case true: // Show menu
                    panel1.Location = new Point(panel1.Location.X + (panel1.Location.X < 0 ? 20 : 0), panel1.Location.Y);
                    if (panel1.Location.X >= 0)
                    {
                        panel1.Location = new Point(0, panel1.Location.Y);
                        panel1.Invalidate();
                        tmrAnimate.Stop();
                    }
                    break;
                case false: // Hide menu
                    panel1.Location = new Point(panel1.Location.X - (panel1.Location.X > (Math.Abs(panel1.Width - 39) * (-1)) ? 20 : 0), panel1.Location.Y);
                    if (panel1.Location.X <= (Math.Abs(panel1.Width - 39) * (-1)))
                    {
                        panel1.Location = new Point((Math.Abs(panel1.Width - 39) * (-1)), panel1.Location.Y);
                        panel1.Invalidate();
                        tmrAnimate.Stop();
                    }
                    break;
            }
            tabControl1.Width = (Width + 6) - (panel1.Width + panel1.Location.X);
            tabControl1.Location = new Point(((panel1.Width + panel1.Location.X) - 3), tabControl1.Location.Y);
        }
        bool allowSwitch = false;

        private void switchTab(Label senderLB, TabPage tp)
        {
            // Switch tab
            allowSwitch = true;
            tabControl1.SelectedTab = tp;
            // Set fonts
            lbSettings.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular);
            lbThemes.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular);
            lbApps.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular);
            lbAdvanced.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular);
            lbHakkinda.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular);
            // Make label bold
            senderLB.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold);
            pbHamMenu_Click(senderLB, new EventArgs());
        }

        private void pbHamMenu_Click(object sender, EventArgs e)
        {
            if (panel1.AutoScroll)
            {
                panel1.AutoScroll = false;
                pbHamMenu.Location = new Point(panel1.Width - (pbHamMenu.Width + 5), pbHamMenu.Location.Y);
            }
            else
            {
                panel1.AutoScroll = true;


                // Sidebar
                int[] biggestpp = new int[] {
                    lbSettings.Location.X + lbSettings.Width + 5,
                    lbApps.Location.X + lbApps.Width + 5,
                    lbThemes.Location.X + lbThemes.Width + 5,
                    lbHakkinda.Location.X + lbHakkinda.Width + 5,
                };
                int? maxVal = null;
                int index = -1;
                for (int i = 0; i < biggestpp.Length; i++)
                {
                    int thisNum = biggestpp[i];
                    if (!maxVal.HasValue || thisNum > maxVal.Value)
                    {
                        maxVal = thisNum;
                        index = i;
                    }
                }
                if (maxVal != null && maxVal > 0)
                {
                    panel1.Width = (int)maxVal + 52;
                    tabControl1.Width = Width - (panel1.Width + 6);
                }
                pbHamMenu.Location = new Point(panel1.Width - (25 + pbHamMenu.Width), pbHamMenu.Location.Y);
            }
            panel1.Invalidate();
            tmrAnimate.Start();
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (allowSwitch) { allowSwitch = false; } else { e.Cancel = true; }
        }

        private void lbSettings_Click(object sender, EventArgs e) => switchTab(lbSettings, tpSettings);
        private void lbApps_Click(object sender, EventArgs e) => switchTab(lbApps, tpApps);
        private void lbAdvanced_Click(object sender, EventArgs e) => switchTab(lbAdvanced, tpAdvanced);
        private void lbThemes_Click(object sender, EventArgs e) => switchTab(lbThemes, tpThemes);
        private void lbHakkinda_Click(object sender, EventArgs e) => switchTab(lbHakkinda, tpAbout);
    }
}
