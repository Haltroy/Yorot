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

        string lastupdate = "";
        // TODO: Generate App Settings page on demand for apps with this
        void GenerateAppTap(YorotApp app)
        {
            string randomAppID = HTAlt.Tools.GenerateRandomText(17);
            // TODO: Add all language specific ones to lists.
            System.Windows.Forms.TabPage tpAppPage = new System.Windows.Forms.TabPage();
            System.Windows.Forms.PictureBox pbAppIcon = new System.Windows.Forms.PictureBox();
            System.Windows.Forms.PictureBox pbASBack = new System.Windows.Forms.PictureBox();
            System.Windows.Forms.Label lbAppCName = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAppVer = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAppAuthor = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAppName = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAppSettings = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTButton btAppReset = new HTAlt.WinForms.HTButton();
            HTAlt.WinForms.HTButton btAppUninstall = new HTAlt.WinForms.HTButton();
            System.Windows.Forms.Label lbSizeOnDisk = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsNotifications = new HTAlt.WinForms.HTSwitch();
            System.Windows.Forms.Label lbNotifications = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbPrioritize = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsPrioritize = new HTAlt.WinForms.HTSwitch();
            System.Windows.Forms.Label lbRunOnStartup = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsRunOnStartup = new HTAlt.WinForms.HTSwitch();
            System.Windows.Forms.Label lbNotificationsInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbNotifListener = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsNotifListener = new HTAlt.WinForms.HTSwitch();
            System.Windows.Forms.Label lbPrioritieInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbNotifListenerInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbRunOnStartupInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbOrigin = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAppOrigin = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAppOriginInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbRunOnIncognitoInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbRunOnIncognito = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsRunOnIncognito = new HTAlt.WinForms.HTSwitch();
            System.Windows.Forms.Label lbAppSize = new System.Windows.Forms.Label();
            // 
            // tpAppPage
            // 
            tpAppPage.Controls.Add(hsRunOnIncognito);
            tpAppPage.Controls.Add(hsRunOnStartup);
            tpAppPage.Controls.Add(hsNotifListener);
            tpAppPage.Controls.Add(hsPrioritize);
            tpAppPage.Controls.Add(hsNotifications);
            tpAppPage.Controls.Add(lbNotifListener);
            tpAppPage.Controls.Add(lbRunOnIncognito);
            tpAppPage.Controls.Add(lbRunOnStartup);
            tpAppPage.Controls.Add(lbPrioritize);
            tpAppPage.Controls.Add(lbRunOnIncognitoInfo);
            tpAppPage.Controls.Add(lbNotifications);
            tpAppPage.Controls.Add(lbRunOnStartupInfo);
            tpAppPage.Controls.Add(lbNotifListenerInfo);
            tpAppPage.Controls.Add(lbPrioritieInfo);
            tpAppPage.Controls.Add(lbNotificationsInfo);
            tpAppPage.Controls.Add(lbAppOriginInfo);
            tpAppPage.Controls.Add(lbAppSize);
            tpAppPage.Controls.Add(lbAppOrigin);
            tpAppPage.Controls.Add(lbOrigin);
            tpAppPage.Controls.Add(lbSizeOnDisk);
            tpAppPage.Controls.Add(btAppUninstall);
            tpAppPage.Controls.Add(btAppReset);
            tpAppPage.Controls.Add(pbAppIcon);
            tpAppPage.Controls.Add(pbASBack);
            tpAppPage.Controls.Add(lbAppCName);
            tpAppPage.Controls.Add(lbAppVer);
            tpAppPage.Controls.Add(lbAppAuthor);
            tpAppPage.Controls.Add(lbAppName);
            tpAppPage.Controls.Add(lbAppSettings);
            tpAppPage.Name = "tpAppPage_" + randomAppID;
            tpAppPage.Text = app.AppName;
            tpAppPage.UseVisualStyleBackColor = true;
            // 
            // pbAppIcon
            // 
            pbAppIcon.Location = new System.Drawing.Point(18, 56);
            pbAppIcon.Name = "pbAppIcon_" + randomAppID;
            pbAppIcon.Size = new System.Drawing.Size(64, 64);
            pbAppIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pbAppIcon.TabIndex = 4;
            pbAppIcon.Image = app.GetAppIcon();
            pbAppIcon.TabStop = false;
            // 
            // pbASBack
            // 
            pbASBack.Image = global::Yorot.Properties.Resources.hamburger; //TODO: Change it to "Back" button.
            pbASBack.Location = new System.Drawing.Point(18, 14);
            pbASBack.Name = "pbASBack_" + randomAppID;
            pbASBack.Size = new System.Drawing.Size(25, 25);
            pbASBack.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pbASBack.TabIndex = 1;
            pbASBack.TabStop = false;
            // 
            // lbAppCName
            // 
            lbAppCName.AutoSize = true;
            lbAppCName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.5F);
            lbAppCName.Location = new System.Drawing.Point(89, 101);
            lbAppCName.Name = "lbAppCName_" + randomAppID;
            lbAppCName.Size = new System.Drawing.Size(121, 20);
            lbAppCName.TabIndex = 3;
            lbAppCName.Text = app.AppCodeName;
            // 
            // lbAppVer
            // 
            lbAppVer.AutoSize = true;
            lbAppVer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.5F);
            lbAppVer.Location = new System.Drawing.Point(194, 50);
            lbAppVer.Name = "lbAppVer_" + randomAppID;
            lbAppVer.Size = new System.Drawing.Size(64, 20);
            lbAppVer.TabIndex = 3;
            lbAppVer.Text = app.Version + " [" + app.VersionNo + "]";
            // 
            // lbAppAuthor
            // 
            lbAppAuthor.AutoSize = true;
            lbAppAuthor.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.5F);
            lbAppAuthor.Location = new System.Drawing.Point(89, 81);
            lbAppAuthor.Name = "lbAppAuthor_" + randomAppID;
            lbAppAuthor.Size = new System.Drawing.Size(87, 20);
            lbAppAuthor.TabIndex = 3;
            lbAppAuthor.Text = app.Author;
            // 
            // lbAppName
            // 
            lbAppName.AutoSize = true;
            lbAppName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            lbAppName.Location = new System.Drawing.Point(88, 56);
            lbAppName.Name = "lbAppName_" + randomAppID;
            lbAppName.Size = new System.Drawing.Size(100, 25);
            lbAppName.TabIndex = 3;
            lbAppName.Text = "AppName";
            // 
            // lbAppSettings
            // 
            lbAppSettings.AutoSize = true;
            lbAppSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            lbAppSettings.Location = new System.Drawing.Point(49, 14);
            lbAppSettings.Name = "lbAppSettings_" + randomAppID;
            lbAppSettings.Size = new System.Drawing.Size(136, 25);
            lbAppSettings.TabIndex = 3;
            lbAppSettings.Text = "App Settings";
            // 
            // btAppReset
            // 
            btAppReset.AutoColor = true;
            btAppReset.ButtonImage = null;
            btAppReset.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            btAppReset.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            btAppReset.DrawImage = false;
            btAppReset.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            btAppReset.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            btAppReset.Location = new System.Drawing.Point(18, 126);
            btAppReset.Name = "btAppReset_" + randomAppID;
            btAppReset.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            btAppReset.Size = new System.Drawing.Size(75, 23);
            btAppReset.TabIndex = 5;
            btAppReset.Text = "Reset";
            // 
            // btAppUninstall
            // 
            btAppUninstall.AutoColor = true;
            btAppUninstall.ButtonImage = null;
            btAppUninstall.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            btAppUninstall.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            btAppUninstall.DrawImage = false;
            btAppUninstall.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            btAppUninstall.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            btAppUninstall.Location = new System.Drawing.Point(18, 155);
            btAppUninstall.Name = "btAppUninstall_" + randomAppID;
            btAppUninstall.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            btAppUninstall.Size = new System.Drawing.Size(75, 23);
            btAppUninstall.TabIndex = 5;
            btAppUninstall.Text = "Uninstall";
            // 
            // lbSizeOnDisk
            // 
            lbSizeOnDisk.AutoSize = true;
            lbSizeOnDisk.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            lbSizeOnDisk.Location = new System.Drawing.Point(14, 185);
            lbSizeOnDisk.Name = "lbSizeOnDisk_" + randomAppID;
            lbSizeOnDisk.Size = new System.Drawing.Size(98, 20);
            lbSizeOnDisk.TabIndex = 6;
            lbSizeOnDisk.Text = "Size on disk:";
            // 
            // hsNotifications
            // 
            hsNotifications.Location = new System.Drawing.Point(18, 357);
            hsNotifications.Name = "hsNotifications_" + randomAppID;
            hsNotifications.Size = new System.Drawing.Size(50, 19);
            hsNotifications.TabIndex = 7;
            // 
            // lbNotifications
            // 
            lbNotifications.AutoSize = true;
            lbNotifications.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.5F);
            lbNotifications.Location = new System.Drawing.Point(74, 355);
            lbNotifications.Name = "lbNotifications_" + randomAppID;
            lbNotifications.Size = new System.Drawing.Size(102, 20);
            lbNotifications.TabIndex = 6;
            lbNotifications.Text = "Notifications";
            // 
            // lbPrioritize
            // 
            lbPrioritize.AutoSize = true;
            lbPrioritize.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.5F);
            lbPrioritize.Location = new System.Drawing.Point(134, 431);
            lbPrioritize.Name = "lbPrioritize_" + randomAppID;
            lbPrioritize.Size = new System.Drawing.Size(76, 20);
            lbPrioritize.TabIndex = 6;
            lbPrioritize.Text = "Prioritize";
            // 
            // hsPrioritize
            // 
            hsPrioritize.Location = new System.Drawing.Point(78, 432);
            hsPrioritize.Name = "hsPrioritize_" + randomAppID;
            hsPrioritize.Size = new System.Drawing.Size(50, 19);
            hsPrioritize.TabIndex = 7;
            // 
            // lbRunOnStartup
            // 
            lbRunOnStartup.AutoSize = true;
            lbRunOnStartup.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.5F);
            lbRunOnStartup.Location = new System.Drawing.Point(74, 587);
            lbRunOnStartup.Name = "lbRunOnStartup_" + randomAppID;
            lbRunOnStartup.Size = new System.Drawing.Size(119, 20);
            lbRunOnStartup.TabIndex = 6;
            lbRunOnStartup.Text = "Run on startup";
            // 
            // hsRunOnStartup
            // 
            hsRunOnStartup.Location = new System.Drawing.Point(18, 588);
            hsRunOnStartup.Name = "hsRunOnStartup_" + randomAppID;
            hsRunOnStartup.Size = new System.Drawing.Size(50, 19);
            hsRunOnStartup.TabIndex = 7;
            // 
            // lbNotificationsInfo
            // 
            lbNotificationsInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            lbNotificationsInfo.Location = new System.Drawing.Point(75, 378);
            lbNotificationsInfo.Name = "lbNotificationsInfo_" + randomAppID;
            lbNotificationsInfo.Size = new System.Drawing.Size(478, 35);
            lbNotificationsInfo.TabIndex = 6;
            lbNotificationsInfo.Text = "Allows this app to show notifications";
            // 
            // lbNotifListener
            // 
            lbNotifListener.AutoSize = true;
            lbNotifListener.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.5F);
            lbNotifListener.Location = new System.Drawing.Point(134, 497);
            lbNotifListener.Name = "lbNotifListener_" + randomAppID;
            lbNotifListener.Size = new System.Drawing.Size(295, 20);
            lbNotifListener.TabIndex = 6;
            lbNotifListener.Text = "Run notification listener at background";
            // 
            // hsNotifListener
            // 
            hsNotifListener.Location = new System.Drawing.Point(78, 497);
            hsNotifListener.Name = "hsNotifListener_" + randomAppID;
            hsNotifListener.Size = new System.Drawing.Size(50, 19);
            hsNotifListener.TabIndex = 7;
            // 
            // lbPrioritieInfo
            // 
            lbPrioritieInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            lbPrioritieInfo.Location = new System.Drawing.Point(135, 451);
            lbPrioritieInfo.Name = "lbPrioritieInfo_" + randomAppID;
            lbPrioritieInfo.Size = new System.Drawing.Size(418, 35);
            lbPrioritieInfo.TabIndex = 6;
            lbPrioritieInfo.Text = "Prioritizes this app\'s notification from other notifications.";
            // 
            // lbNotifListenerInfo
            // 
            lbNotifListenerInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            lbNotifListenerInfo.Location = new System.Drawing.Point(135, 525);
            lbNotifListenerInfo.Name = "lbNotifListenerInfo_" + randomAppID;
            lbNotifListenerInfo.Size = new System.Drawing.Size(418, 35);
            lbNotifListenerInfo.TabIndex = 6;
            lbNotifListenerInfo.Text = "Allows Yorot to run a background service for retrieving new notifications.";
            // 
            // lbRunOnStartupInfo
            // 
            lbRunOnStartupInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            lbRunOnStartupInfo.Location = new System.Drawing.Point(75, 612);
            lbRunOnStartupInfo.Name = "lbRunOnStartupInfo_" + randomAppID;
            lbRunOnStartupInfo.Size = new System.Drawing.Size(478, 35);
            lbRunOnStartupInfo.TabIndex = 6;
            lbRunOnStartupInfo.Text = "Starts application on Yorot startup.";
            // 
            // lbOrigin
            // 
            lbOrigin.AutoSize = true;
            lbOrigin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            lbOrigin.Location = new System.Drawing.Point(14, 220);
            lbOrigin.Name = "lbOrigin_" + randomAppID;
            lbOrigin.Size = new System.Drawing.Size(58, 20);
            lbOrigin.TabIndex = 6;
            lbOrigin.Text = "Origin: ";
            // 
            // lbAppOrigin
            // 
            lbAppOrigin.AutoSize = true;
            lbAppOrigin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            lbAppOrigin.Location = new System.Drawing.Point(78, 220);
            lbAppOrigin.Name = "lbAppOrigin_" + randomAppID;
            lbAppOrigin.Size = new System.Drawing.Size(48, 20);
            lbAppOrigin.TabIndex = 6;
            lbAppOrigin.Text = app.AppOrigin.ToString();
            // 
            // lbAppOriginInfo
            // 
            lbAppOriginInfo.AutoSize = true;
            lbAppOriginInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            lbAppOriginInfo.Location = new System.Drawing.Point(79, 240);
            lbAppOriginInfo.Name = "lbAppOriginInfo_" + randomAppID;
            lbAppOriginInfo.Size = new System.Drawing.Size(330, 68);
            lbAppOriginInfo.TabIndex = 6;
            lbAppOriginInfo.Text = app.AppOriginInfo;
            // 
            // lbRunOnIncognitoInfo
            // 
            lbRunOnIncognitoInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            lbRunOnIncognitoInfo.Location = new System.Drawing.Point(75, 689);
            lbRunOnIncognitoInfo.Name = "lbRunOnIncognitoInfo_" + randomAppID;
            lbRunOnIncognitoInfo.Size = new System.Drawing.Size(478, 35);
            lbRunOnIncognitoInfo.TabIndex = 6;
            lbRunOnIncognitoInfo.Text = "Allows this app to run on Incognito mode.";
            // 
            // lbRunOnIncognito
            // 
            lbRunOnIncognito.AutoSize = true;
            lbRunOnIncognito.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.5F);
            lbRunOnIncognito.Location = new System.Drawing.Point(74, 665);
            lbRunOnIncognito.Name = "lbRunOnIncognito_" + randomAppID;
            lbRunOnIncognito.Size = new System.Drawing.Size(180, 20);
            lbRunOnIncognito.TabIndex = 6;
            lbRunOnIncognito.Text = "Run on Incognito mode";
            // 
            // hsRunOnIncognito
            // 
            hsRunOnIncognito.Location = new System.Drawing.Point(18, 666);
            hsRunOnIncognito.Name = "hsRunOnIncognito_" + randomAppID;
            hsRunOnIncognito.Size = new System.Drawing.Size(50, 19);
            hsRunOnIncognito.TabIndex = 7;
            // 
            // lbAppSize
            // 
            lbAppSize.AutoSize = true;
            lbAppSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            lbAppSize.Location = new System.Drawing.Point(118, 185);
            lbAppSize.Name = "lbAppSize_" + randomAppID;
            lbAppSize.Size = new System.Drawing.Size(176, 20);
            lbAppSize.TabIndex = 6;
            lbAppSize.Text = app.GetAppSizeInfo(SizeInfoBytes);
            //
            // tcApps
            //
            this.tcApps.Controls.Add(tpAppPage);

        }
        public string SizeInfoBytes = "bytes";
    }
}
