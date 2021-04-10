using HTAlt;
using HTAlt.WinForms;
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
            Icon = HTAlt.Tools.IconFromImage(Properties.Resources.Settings);
        }

        /// <summary>
        /// tabControl1
        /// </summary>
        bool _s1 = false;
        /// <summary>
        /// tabControl2
        /// </summary>
        bool _s2 = false;

        private void switchTab2(Control sender, TabPage tp)
        { 
            // Switch tab
            _s2 = true;
            tabControl2.SelectedTab = tp;
            // Set fonts
            for (int i = 0; i < flpSidebar.Controls.Count;i++)
            {
                flpSidebar.Controls[i].Font = new Font("Ubuntu", 15F, FontStyle.Regular);
            }
            // Make label bold
            sender.Font = new Font("Ubuntu", 15F, FontStyle.Bold);
        }

        private void switch1(sbyte type)
        {
            // Hide all
            btGeneralSettings.Enabled = false;
            btLanguages.Enabled = false;
            btNotifications.Enabled = false;
            btAccessSettings.Enabled = false;
            btDownloads.Enabled = false;
            btHistory.Enabled = false;
            btProfiles.Enabled = false;
            btSync.Enabled = false;
            btSecuritySettings.Enabled = false;
            btSites.Enabled = false;
            btProxies.Enabled = false;
            btExtensions.Enabled = false;
            btApps.Enabled = false;
            btWE.Enabled = false;
            btThemes.Enabled = false;
            btAppearance.Enabled = false;
            btUpdates.Enabled = false;
            btAbout.Enabled = false;
            btLogs.Enabled = false;
            btGeneralSettings.Visible = false;
            btLanguages.Visible = false;
            btNotifications.Visible = false;
            btAccessSettings.Visible = false;
            btDownloads.Visible = false;
            btHistory.Visible = false;
            btProfiles.Visible = false;
            btSync.Visible = false;
            btSecuritySettings.Visible = false;
            btSites.Visible = false;
            btProxies.Visible = false;
            btExtensions.Visible = false;
            btApps.Visible = false;
            btWE.Visible = false;
            btThemes.Visible = false;
            btAppearance.Visible = false;
            btUpdates.Visible = false;
            btAbout.Visible = false;
            btLogs.Visible = false;
            // Set needed ones
            _s1 = true;
            
            switch (type)
            {
                case 0:
                    btGeneralSettings_Click(btGeneralSettings, new EventArgs());
                    btGeneralSettings.Visible = true;
                    btLanguages.Visible = true;
                    btNotifications.Visible = true;
                    btAccessSettings.Visible = true;
                    btGeneralSettings.Enabled = true;
                    btLanguages.Enabled = true;
                    btNotifications.Enabled = true;
                    btAccessSettings.Enabled = true;
                    tabControl1.SelectedTab = tp1Settings;
                    pContainer.Controls.Clear();
                    break;
                case 1:
                    btSHistory_Click(btHistory, new EventArgs());
                    btDownloads.Visible = true;
                    btHistory.Visible = true;
                    btDownloads.Enabled = true;
                    btHistory.Enabled = true;
                    tabControl1.SelectedTab = tp1Settings;
                    pContainer.Controls.Clear();
                    break;
                case 2:
                    btProfiles_Click(btProfiles, new EventArgs());
                    btProfiles.Visible = true;
                    btSync.Visible = true;
                    btProfiles.Enabled = true;
                    btSync.Enabled = true;
                    tabControl1.SelectedTab = tp1Settings;
                    pContainer.Controls.Clear();
                    break;
                case 3:
                    btSecuritySettings_Click(btSecuritySettings, new EventArgs());
                    btSecuritySettings.Enabled = true;
                    btSites.Enabled = true;
                    btProxies.Enabled = true;
                    btSecuritySettings.Visible = true;
                    btSites.Visible = true;
                    btProxies.Visible = true;
                    tabControl1.SelectedTab = tp1Settings;
                    pContainer.Controls.Clear();
                    break;
                case 4:
                    btAppearance_Click(btAppearance, new EventArgs());
                    btThemes.Visible = true;
                    btAppearance.Visible = true;
                    btThemes.Enabled = true;
                    btAppearance.Enabled = true;
                    tabControl1.SelectedTab = tp1Settings;
                    pContainer.Controls.Clear();
                    break;
                case 5:
                    btExtensions_Click(btExtensions, new EventArgs());
                    btExtensions.Enabled = true;
                    btApps.Enabled = true;
                    btWE.Enabled = true;
                    btExtensions.Visible = true;
                    btApps.Visible = true;
                    btWE.Visible = true;
                    tabControl1.SelectedTab = tp1Settings;
                    pContainer.Controls.Clear();
                    break;
                case 6:
                    btUpdates_Click(btUpdates, new EventArgs());
                    btUpdates.Visible = true;
                    btAbout.Visible = true;
                    btLogs.Visible = true;
                    btUpdates.Enabled = true;
                    btAbout.Enabled = true;
                    btLogs.Enabled = true;
                    tabControl1.SelectedTab = tp1Settings;
                    pContainer.Controls.Clear();
                    break;
                case 7:
                    tabControl1.SelectedTab = tp1Main;
                    pContainer.Controls.Clear();
                    break;
                case 8:
                    tabControl1.SelectedTab = tp1Container;
                    break;
            }
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (_s1) { _s1 = false; } else { e.Cancel = true; }
        }

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
            pbAppIcon.Image = YorotTools.GetAppIcon(app);
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
            lbAppCName.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAppCName.Location = new System.Drawing.Point(89, 101);
            lbAppCName.Name = "lbAppCName_" + randomAppID;
            lbAppCName.Size = new System.Drawing.Size(121, 20);
            lbAppCName.TabIndex = 3;
            lbAppCName.Text = app.AppCodeName;
            // 
            // lbAppVer
            // 
            lbAppVer.AutoSize = true;
            lbAppVer.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAppVer.Location = new System.Drawing.Point(194, 50);
            lbAppVer.Name = "lbAppVer_" + randomAppID;
            lbAppVer.Size = new System.Drawing.Size(64, 20);
            lbAppVer.TabIndex = 3;
            lbAppVer.Text = app.Version + " [" + app.VersionNo + "]";
            // 
            // lbAppAuthor
            // 
            lbAppAuthor.AutoSize = true;
            lbAppAuthor.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAppAuthor.Location = new System.Drawing.Point(89, 81);
            lbAppAuthor.Name = "lbAppAuthor_" + randomAppID;
            lbAppAuthor.Size = new System.Drawing.Size(87, 20);
            lbAppAuthor.TabIndex = 3;
            lbAppAuthor.Text = app.Author;
            // 
            // lbAppName
            // 
            lbAppName.AutoSize = true;
            lbAppName.Font = new System.Drawing.Font("Ubuntu", 15F);
            lbAppName.Location = new System.Drawing.Point(88, 56);
            lbAppName.Name = "lbAppName_" + randomAppID;
            lbAppName.Size = new System.Drawing.Size(100, 25);
            lbAppName.TabIndex = 3;
            lbAppName.Text = "AppName";
            // 
            // lbAppSettings
            // 
            lbAppSettings.AutoSize = true;
            lbAppSettings.Font = new System.Drawing.Font("Ubuntu", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
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
            lbSizeOnDisk.Font = new System.Drawing.Font("Ubuntu", 12F);
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
            lbNotifications.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbNotifications.Location = new System.Drawing.Point(74, 355);
            lbNotifications.Name = "lbNotifications_" + randomAppID;
            lbNotifications.Size = new System.Drawing.Size(102, 20);
            lbNotifications.TabIndex = 6;
            lbNotifications.Text = "Notifications";
            // 
            // lbPrioritize
            // 
            lbPrioritize.AutoSize = true;
            lbPrioritize.Font = new System.Drawing.Font("Ubuntu", 12.5F);
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
            lbRunOnStartup.Font = new System.Drawing.Font("Ubuntu", 12.5F);
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
            lbNotificationsInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbNotificationsInfo.Location = new System.Drawing.Point(75, 378);
            lbNotificationsInfo.Name = "lbNotificationsInfo_" + randomAppID;
            lbNotificationsInfo.Size = new System.Drawing.Size(478, 35);
            lbNotificationsInfo.TabIndex = 6;
            lbNotificationsInfo.Text = "Allows this app to show notifications";
            // 
            // lbNotifListener
            // 
            lbNotifListener.AutoSize = true;
            lbNotifListener.Font = new System.Drawing.Font("Ubuntu", 12.5F);
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
            lbPrioritieInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbPrioritieInfo.Location = new System.Drawing.Point(135, 451);
            lbPrioritieInfo.Name = "lbPrioritieInfo_" + randomAppID;
            lbPrioritieInfo.Size = new System.Drawing.Size(418, 35);
            lbPrioritieInfo.TabIndex = 6;
            lbPrioritieInfo.Text = "Prioritizes this app\'s notification from other notifications.";
            // 
            // lbNotifListenerInfo
            // 
            lbNotifListenerInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbNotifListenerInfo.Location = new System.Drawing.Point(135, 525);
            lbNotifListenerInfo.Name = "lbNotifListenerInfo_" + randomAppID;
            lbNotifListenerInfo.Size = new System.Drawing.Size(418, 35);
            lbNotifListenerInfo.TabIndex = 6;
            lbNotifListenerInfo.Text = "Allows Yorot to run a background service for retrieving new notifications.";
            // 
            // lbRunOnStartupInfo
            // 
            lbRunOnStartupInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbRunOnStartupInfo.Location = new System.Drawing.Point(75, 612);
            lbRunOnStartupInfo.Name = "lbRunOnStartupInfo_" + randomAppID;
            lbRunOnStartupInfo.Size = new System.Drawing.Size(478, 35);
            lbRunOnStartupInfo.TabIndex = 6;
            lbRunOnStartupInfo.Text = "Starts application on Yorot startup.";
            // 
            // lbOrigin
            // 
            lbOrigin.AutoSize = true;
            lbOrigin.Font = new System.Drawing.Font("Ubuntu", 12F);
            lbOrigin.Location = new System.Drawing.Point(14, 220);
            lbOrigin.Name = "lbOrigin_" + randomAppID;
            lbOrigin.Size = new System.Drawing.Size(58, 20);
            lbOrigin.TabIndex = 6;
            lbOrigin.Text = "Origin: ";
            // 
            // lbAppOrigin
            // 
            lbAppOrigin.AutoSize = true;
            lbAppOrigin.Font = new System.Drawing.Font("Ubuntu", 12F);
            lbAppOrigin.Location = new System.Drawing.Point(78, 220);
            lbAppOrigin.Name = "lbAppOrigin_" + randomAppID;
            lbAppOrigin.Size = new System.Drawing.Size(48, 20);
            lbAppOrigin.TabIndex = 6;
            lbAppOrigin.Text = app.AppOrigin.ToString();
            // 
            // lbAppOriginInfo
            // 
            lbAppOriginInfo.AutoSize = true;
            lbAppOriginInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbAppOriginInfo.Location = new System.Drawing.Point(79, 240);
            lbAppOriginInfo.Name = "lbAppOriginInfo_" + randomAppID;
            lbAppOriginInfo.Size = new System.Drawing.Size(330, 68);
            lbAppOriginInfo.TabIndex = 6;
            lbAppOriginInfo.Text = app.AppOriginInfo;
            // 
            // lbRunOnIncognitoInfo
            // 
            lbRunOnIncognitoInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbRunOnIncognitoInfo.Location = new System.Drawing.Point(75, 689);
            lbRunOnIncognitoInfo.Name = "lbRunOnIncognitoInfo_" + randomAppID;
            lbRunOnIncognitoInfo.Size = new System.Drawing.Size(478, 35);
            lbRunOnIncognitoInfo.TabIndex = 6;
            lbRunOnIncognitoInfo.Text = "Allows this app to run on Incognito mode.";
            // 
            // lbRunOnIncognito
            // 
            lbRunOnIncognito.AutoSize = true;
            lbRunOnIncognito.Font = new System.Drawing.Font("Ubuntu", 12.5F);
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
            lbAppSize.Font = new System.Drawing.Font("Ubuntu", 12F);
            lbAppSize.Location = new System.Drawing.Point(118, 185);
            lbAppSize.Name = "lbAppSize_" + randomAppID;
            lbAppSize.Size = new System.Drawing.Size(176, 20);
            lbAppSize.TabIndex = 6;
            lbAppSize.Text = app.GetAppSizeInfo(SizeInfoBytes);
            //
            // tcApps
            //
            
            /* this.tcApps.Controls.Add(tpAppPage); */

        }
        public string SizeInfoBytes = "bytes";

        private void tabControl2_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (_s2) { _s2 = false; } else { e.Cancel = true; }
        }

        private void btGeneralSettings_Click(object sender, EventArgs e) => switchTab2(sender as Control, tpGeneral);
        private void btLanguages_Click(object sender, EventArgs e) => switchTab2(sender as Control, tpLanguage);
        private void btDownloads_Click(object sender, EventArgs e) => switchTab2(sender as Control, tpDownloads);
        private void btNotifications_Click(object sender, EventArgs e) => switchTab2(sender as Control, tpNotifications);
        private void btAccessSettings_Click(object sender, EventArgs e) => switchTab2(sender as Control, tpAccessibility);
        private void btAppearance_Click(object sender, EventArgs e) => switchTab2(sender as Control, tpAppearance);
        private void btProfiles_Click(object sender, EventArgs e) => switchTab2(sender as Control, tpProfiles);
        private void btSecuritySettings_Click(object sender, EventArgs e) => switchTab2(sender as Control, tpSecurity);
        private void btSites_Click(object sender, EventArgs e) => switchTab2(sender as Control, tpSites);
        private void btProxies_Click(object sender, EventArgs e) => switchTab2(sender as Control, tpProxies);
        private void btExtensions_Click(object sender, EventArgs e) => switchTab2(sender as Control, tpExtensions);
        private void btApps_Click(object sender, EventArgs e) => switchTab2(sender as Control, tpApps);
        private void btWE_Click(object sender, EventArgs e) => switchTab2(sender as Control, tpWE);
        private void btThemes_Click(object sender, EventArgs e) => switchTab2(sender as Control, tpThemes);
        private void btUpdates_Click(object sender, EventArgs e) => switchTab2(sender as Control, tpUpdates);
        private void btAbout_Click(object sender, EventArgs e) => switchTab2(sender as Control, tpAbout);
        private void btLogs_Click(object sender, EventArgs e) => switchTab2(sender as Control, tpLogs);
        private void btSync_Click(object sender, EventArgs e) => switchTab2(sender as Control, tpSync);
        private void btSHistory_Click(object sender, EventArgs e) => switchTab2(sender as Control, tpHistory);

        private void btContainerBack_Click(object sender, EventArgs e) => switch1(btContainerBack.Tag != null ? (sbyte)btContainerBack.Tag : (sbyte)7);

        private void button1_Click(object sender, EventArgs e) => switch1(7);

        private void btGeneral_Click(object sender, EventArgs e) => switch1(0);

        private void btHistoryDownloads_Click(object sender, EventArgs e) => switch1(1);

        private void button2_Click(object sender, EventArgs e) => switch1(2);

        private void btSecurity_Click(object sender, EventArgs e) => switch1(3);

        private void btAddons_Click(object sender, EventArgs e) => switch1(5);

        private void btCustomization_Click(object sender, EventArgs e) => switch1(4);

        private void btUpdatesAbout_Click(object sender, EventArgs e) => switch1(6);

        private void settings_Load(object sender, EventArgs e)
        {
            // Load User Image
            if (System.IO.File.Exists(YorotGlobal.Main.Profiles.Current.Path + "picture.png"))
            {
                var img = YorotGlobal.Main.Profiles.Current.Picture;
                pbHomeUser.Image = YorotTools.ClipToCircle(img, new PointF(img.Width / 2, img.Height / 2), img.Width / 2);
            }else
            {
                // TODO: Set to default user logo
            }
            ApplyLanguage(true);
            ApplyTheme(true);
            
        }
        private string appliedLang = string.Empty;
        private void ApplyLanguage(bool force = false)
        {
            if (force || appliedLang != YorotGlobal.Main.Profiles.Current.Settings.CurrentLanguage.CodeName)
            {
                appliedLang = YorotGlobal.Main.Profiles.Current.Settings.CurrentLanguage.CodeName;
                // TODO: Load Language
            }
        }
        private string appliedTheme = string.Empty;
        private void ApplyTheme(bool force = false)
        {
            var theme = YorotGlobal.Main.CurrentTheme;
            var themeid = theme.BackColor.ToHex() + "-" + theme.ForeColor.ToHex() + "-" + theme.OverlayColor.ToHex() + "-" + theme.ArtColor.ToHex();
            if (force || appliedTheme != themeid)
            {
                appliedTheme = themeid;
                var isDark = !HTAlt.Tools.IsBright(theme.BackColor);
                BackColor = theme.BackColor;
                ForeColor = theme.ForeColor;
                panel1.BackColor = theme.BackColor2;
                panel1.ForeColor = theme.ForeColor;
                tp1Main.BackColor = theme.BackColor;
                tp1Main.ForeColor = theme.ForeColor;
                tp1Settings.BackColor = theme.BackColor;
                tp1Settings.ForeColor = theme.ForeColor;
                flpSidebar.BackColor = theme.BackColor2;
                flpSidebar.ForeColor = theme.ForeColor;
                for (int i = 0; i < tabControl2.TabPages.Count;i++)
                {
                    var tp = tabControl2.TabPages[i];
                    tp.BackColor = theme.BackColor;
                    tp.ForeColor = theme.ForeColor;
                    for(int ı = 0; ı < tp.Controls.Count; ı++)
                    {
                        var c = tp.Controls[ı];
                        switch (c)
                        {
                            case HTSwitch _:
                                var s = c as HTSwitch;
                                s.BackColor = theme.BackColor2;
                                s.ForeColor = theme.ForeColor;
                                s.OverlayColor = theme.OverlayColor;
                                s.ButtonColor = theme.BackColor2;
                                s.ButtonHoverColor = theme.BackColor3;
                                s.ButtonPressedColor = theme.BackColor4;
                                break;
                            case HTListView _:
                                var l = c as HTListView;
                                l.HeaderBackColor = theme.BackColor3;
                                l.OverlayColor = theme.OverlayColor;
                                l.BackColor = theme.BackColor2;
                                l.ForeColor = theme.ForeColor;
                                break;
                            case HTSlider _:
                                var sl = c as HTSlider;
                                sl.BackColor = theme.BackColor2;
                                sl.ForeColor = theme.ForeColor;
                                sl.OverlayColor = theme.OverlayColor;
                                break;
                            case HTButton _:
                                var b = c as HTButton;
                                b.BackColor = theme.BackColor2;
                                b.ForeColor = theme.ForeColor;
                                if (!b.AutoColor)
                                {
                                    b.ClickColor = theme.BackColor4.ShiftBrightness(20);
                                    b.HoverColor = theme.BackColor4;
                                    b.NormalColor = theme.BackColor3;
                                    b.ForeColor = theme.ForeColor;
                                }
                                break;
                            case LinkLabel _:
                                var ll = c as LinkLabel;
                                ll.BackColor = theme.BackColor;
                                ll.ForeColor = theme.ForeColor;
                                ll.ActiveLinkColor = theme.OverlayColor;
                                ll.DisabledLinkColor = theme.OverlayColor;
                                ll.LinkColor = theme.OverlayColor;
                                ll.VisitedLinkColor = theme.OverlayColor;
                                break;
                            case PictureBox _:
                            case Label _:
                                c.BackColor = theme.BackColor;
                                c.ForeColor = theme.ForeColor;
                                break;
                            case TextBox _:
                                var t = c as TextBox;
                                t.BackColor = t.ReadOnly && t.BorderStyle == BorderStyle.None ? theme.BackColor : theme.BackColor2;
                                t.ForeColor = theme.ForeColor;
                                break;
                            default:
                                c.BackColor = theme.BackColor2;
                                c.ForeColor = theme.ForeColor;
                                break;
                        }
                    }
                }
                btHome.Image = isDark ? Properties.Resources.back_w : Properties.Resources.back_b;
                btContainerBack.Image = isDark ? Properties.Resources.back_w : Properties.Resources.back_b;
            }
        }
    }
}
