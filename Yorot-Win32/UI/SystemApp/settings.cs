using HTAlt;
using HTAlt.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Yorot.UI.SystemApp
{
    public partial class settings : Form
    {
        #region Constructor

        public settings(string[] args = null)
        {
            InitializeComponent();
            Icon = HTAlt.Tools.IconFromImage(Properties.Resources.Settings);
            // Load User Image
            if (System.IO.File.Exists(YorotGlobal.Main.Profiles.Current.Path + "picture.png"))
            {
                Image img = YorotGlobal.Main.Profiles.Current.Picture;
                pbHomeUser.Image = YorotTools.ClipToCircle(img, new PointF(img.Width / 2, img.Height / 2), img.Width / 2);
            }
            else
            {
                pbHomeUser.Image = Properties.Resources.default_pofile_pic;
            }
            pbProfile.Image = pbHomeUser.Image;
            lbUserText.Text = YorotGlobal.Main.Profiles.Current.Text;
            llChangeUserText.Location = new Point(lbUserText.Location.X + lbUserText.Width, llChangeUserText.Location.Y);
            lbUsername.Text = YorotGlobal.Main.Profiles.Current.Name;
            llChangeUserName.Location = new Point(lbUsername.Location.X + lbUsername.Width, llChangeUserName.Location.Y);
            RefreshAppList(true);
            ApplyLanguage(true);
            ApplyTheme(true);
            genLogEntries(true);
            if (args != null)
            {
                if (args.Length > 0)
                {
                    string s = args[0];
                    string s1 = s.Substring(0, s.IndexOf(':'));
                    string s2 = s.Substring(s1.Length + 1);
                    switch (s1)
                    {
                        case "Apps":
                            btAddons_Click(this, new EventArgs());
                            btApps_Click(btApps, new EventArgs());
                            switch (s2)
                            {
                                case "Main":
                                    break;

                                default:
                                    ListViewItem appItem = FindLVI(lvApps, s2);
                                    GenerateAppTab(appItem);
                                    switch1(8);
                                    break;
                            }
                            break;

                        case "Ext":
                            btAddons_Click(this, new EventArgs());
                            btExtensions_Click(btExtensions, new EventArgs());
                            switch (s2)
                            {
                                case "Main":
                                    break;

                                default:
                                    ListViewItem extItem = FindLVI(lvExt, s2);
                                    GenerateExtTab(extItem.Tag as YorotExtension);
                                    switch1(8);
                                    break;
                            }
                            break;

                        case "WE":
                            btAddons_Click(this, new EventArgs());
                            btWE_Click(btWE, new EventArgs());
                            switch (s2)
                            {
                                case "Main":
                                    break;

                                default:
                                    ListViewItem weItem = FindLVI(lvWE, s2);
                                    GenerateWETab(weItem.Tag as YorotWebEngine);
                                    switch1(8);
                                    break;
                            }
                            break;

                        case "Site":
                            btSecurity_Click(this, new EventArgs());
                            btSites_Click(btSites, new EventArgs());
                            switch (s2)
                            {
                                case "Main":
                                    break;

                                default:
                                    ListViewItem siteItem = FindLVI(lvSites, s2);
                                    GenerateSiteTab(siteItem.Tag as YorotSite);
                                    switch1(8);
                                    break;
                            }
                            break;

                        case "General":
                            btGeneral_Click(this, new EventArgs());
                            switch (s2)
                            {
                                default:
                                case "Main":
                                    btGeneralSettings_Click(btGeneralSettings, new EventArgs());
                                    break;

                                case "Lang":
                                    btLanguages_Click(btLanguages, new EventArgs());
                                    break;

                                case "Notif":
                                    btNotifications_Click(btNotifications, new EventArgs());
                                    break;

                                case "Access":
                                    btAccessSettings_Click(btAccessSettings, new EventArgs());
                                    break;
                            }
                            break;

                        case "Profiles":
                            btProfilesAndSync_Click(this, new EventArgs());
                            btProfiles_Click(btProfiles, new EventArgs());
                            break;

                        case "Sync":
                            btProfilesAndSync_Click(this, new EventArgs());
                            btSync_Click(btSync, new EventArgs());
                            break;

                        case "Security":
                            btSecurity_Click(this, new EventArgs());
                            btSecuritySettings_Click(btSecuritySettings, new EventArgs());
                            break;

                        case "Proxy":
                            btSecurity_Click(this, new EventArgs());
                            btProxies_Click(btProxies, new EventArgs());
                            break;

                        case "Help":
                            btUpdatesAbout_Click(this, new EventArgs());
                            switch (s2)
                            {
                                case "Update":
                                    btUpdates_Click(btUpdates, new EventArgs());
                                    break;

                                default:
                                case "About":
                                    btAbout_Click(btAbout, new EventArgs());
                                    break;

                                case "Log":
                                    btLogs_Click(btLogs, new EventArgs());
                                    break;
                            }
                            break;

                        case "History":
                            btHistoryDownloads_Click(this, new EventArgs());
                            switch (s2)
                            {
                                default:
                                case "Main":
                                    btSHistory_Click(btHistory, new EventArgs());
                                    break;

                                case "Down":
                                    btDownloads_Click(btDownloads, new EventArgs());
                                    break;
                            }
                            break;

                        case "Custom":
                            btCustomization_Click(this, new EventArgs());
                            switch (s2)
                            {
                                default:
                                case "Main":
                                    btAppearance_Click(btAppearance, new EventArgs());
                                    break;

                                case "Theme":
                                    btThemes_Click(btThemes, new EventArgs());
                                    break;
                            }
                            break;
                    }
                }
            }
        }

        private void tmrAppSync_Tick(object sender, EventArgs e)
        {
            ApplyTheme();
            ApplyLanguage();
            genLogEntries();
        }

        private ListViewItem FindLVI(ListView lv, string codeName)
        {
            ListViewItem item = null;
            for (int i = 0; i < lv.Items.Count; i++)
            {
                if (lv.Items[i].ToolTipText == codeName)
                {
                    item = lv.Items[i];
                }
            }
            return item;
        }

        private void settings_Load(object sender, EventArgs e)
        {
        }

        #endregion Constructor

        #region Switch Tabs

        /// <summary>
        /// tabControl1
        /// </summary>
        private bool _s1 = false;

        /// <summary>
        /// tabControl2
        /// </summary>
        private bool _s2 = false;

        private void switchTab2(Control sender, TabPage tp)
        {
            // Switch tab
            _s2 = true;
            tabControl2.SelectedTab = tp;
            // Set fonts
            for (int i = 0; i < flpSidebar.Controls.Count; i++)
            {
                flpSidebar.Controls[i].Font = new Font("Ubuntu", 15F, FontStyle.Regular);
            }
            // Make label bold
            sender.Font = new Font("Ubuntu", 15F, FontStyle.Bold);
            resetSearches();
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
                    resetTab();
                    break;

                case 1:
                    btSHistory_Click(btHistory, new EventArgs());
                    btDownloads.Visible = true;
                    btHistory.Visible = true;
                    btDownloads.Enabled = true;
                    btHistory.Enabled = true;
                    tabControl1.SelectedTab = tp1Settings;
                    resetTab();
                    break;

                case 2:
                    btProfiles_Click(btProfiles, new EventArgs());
                    btProfiles.Visible = true;
                    btSync.Visible = true;
                    btProfiles.Enabled = true;
                    btSync.Enabled = true;
                    tabControl1.SelectedTab = tp1Settings;
                    resetTab();
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
                    resetTab();
                    break;

                case 4:
                    btAppearance_Click(btAppearance, new EventArgs());
                    btThemes.Visible = true;
                    btAppearance.Visible = true;
                    btThemes.Enabled = true;
                    btAppearance.Enabled = true;
                    tabControl1.SelectedTab = tp1Settings;
                    resetTab();
                    break;

                case 5:
                    btApps_Click(btApps, new EventArgs());
                    btExtensions.Enabled = true;
                    btApps.Enabled = true;
                    btWE.Enabled = true;
                    btExtensions.Visible = true;
                    btApps.Visible = true;
                    btWE.Visible = true;
                    tabControl1.SelectedTab = tp1Settings;
                    resetTab();
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
                    resetTab();
                    break;

                case 7:
                    tabControl1.SelectedTab = tp1Main;
                    resetTab();
                    break;

                case 8:
                    tabControl1.SelectedTab = tp1Container;
                    ApplyTheme(true);
                    break;
            }
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (_s1) { _s1 = false; } else { e.Cancel = true; }
        }

        private void tabControl2_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (_s2) { _s2 = false; } else { e.Cancel = true; }
        }

        #endregion Switch Tabs

        #region Generate Tabs

        private YorotWebEngine loadedWE;
        private Label lbWEVer;
        private HTButton lbWEReset;
        private HTButton lbWEUninst;
        private Label lbWESize;
        private Label lbWEDesc;
        private Label lbWEEnabled;
        private Label lbWEEnabledInfo;
        private Label lbWERunInc;
        private Label lbWERunIncInfo;

        private void resetTab()
        {
            pContainer.Controls.Clear();
            loadedWE = null;
            lbWEVer = null;
            lbWEReset = null;
            lbWEUninst = null;
            lbWESize = null;
            lbWEDesc = null;
            lbWEEnabled = null;
            lbWEEnabledInfo = null;
            lbWERunInc = null;
            lbWERunIncInfo = null;

        }
        private void GenerateAppTab(ListViewItem item)
        {
            YorotApp app = item.Tag as YorotApp;
            string randomAppID = HTAlt.Tools.GenerateRandomText(17);
            // TODO: Add all language specific ones to lists.
            System.Windows.Forms.PictureBox pbAppIcon = new System.Windows.Forms.PictureBox();
            System.Windows.Forms.Label lbAppCName = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAppVer = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAppAuthor = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAppName = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTButton btAppReset = new HTAlt.WinForms.HTButton();
            HTAlt.WinForms.HTButton btAppUninstall = new HTAlt.WinForms.HTButton();
            System.Windows.Forms.Label lbSizeOnDisk = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsNotifications = new HTAlt.WinForms.HTSwitch();
            System.Windows.Forms.Label lbNotifications = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbEnabled = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbEnabledInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbPrioritize = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsPrioritize = new HTAlt.WinForms.HTSwitch();
            HTAlt.WinForms.HTSwitch hsEnabled = new HTAlt.WinForms.HTSwitch();
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
            System.Windows.Forms.Label lbRunOnIncognitoInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbRunOnIncognito = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsRunOnIncognito = new HTAlt.WinForms.HTSwitch();
            lbContainerTitle.Text = item.Text;
            //
            // pbAppIcon
            //
            pbAppIcon.Location = new System.Drawing.Point(18, 18);
            pbAppIcon.Name = "pbAppIcon_" + randomAppID;
            pbAppIcon.Size = new System.Drawing.Size(64, 64);
            pbAppIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pbAppIcon.Image = YorotTools.GetAppIcon(app);
            pbAppIcon.TabStop = false;
            //
            // lbAppName
            //
            lbAppName.Font = new System.Drawing.Font("Ubuntu", 15F);
            lbAppName.Location = new System.Drawing.Point(95, 18);
            lbAppName.Name = "lbAppName_" + randomAppID;
            lbAppName.Text = item.Text;
            lbAppName.AutoSize = true;
            //
            // lbAppVer
            //
            lbAppVer.AutoSize = true;
            lbAppVer.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAppVer.Name = "lbAppVer_" + randomAppID;
            lbAppVer.Text = "Version " + app.Version + " [" + app.VersionNo + "]";
            lbAppVer.Location = new System.Drawing.Point(lbAppName.Location.X, lbAppName.Location.Y + lbAppName.Height + 2);
            //
            // lbAppCName
            //
            lbAppCName.AutoSize = true;
            lbAppCName.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAppCName.Location = new System.Drawing.Point(lbAppName.Location.X, lbAppVer.Location.Y + lbAppVer.Height + 2);
            lbAppCName.Name = "lbAppCName_" + randomAppID;
            lbAppCName.Text = app.AppCodeName;
            //
            // lbAppAuthor
            //
            lbAppAuthor.AutoSize = true;
            lbAppAuthor.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAppAuthor.Location = new System.Drawing.Point(95, lbAppCName.Location.Y + lbAppCName.Height + 2);
            lbAppAuthor.Name = "lbAppAuthor_" + randomAppID;
            lbAppAuthor.Text = app.Author;
            //
            // btAppReset
            //
            btAppReset.AutoColor = true;
            btAppReset.ButtonImage = null;
            btAppReset.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            btAppReset.ClickColor = System.Drawing.Color.FromArgb(180, 180, 180);
            btAppReset.DrawImage = false;
            btAppReset.HoverColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btAppReset.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            btAppReset.Location = new System.Drawing.Point(18, lbAppAuthor.Location.Y + lbAppAuthor.Height + 5);
            btAppReset.Name = "btAppReset_" + randomAppID;
            btAppReset.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            btAppReset.NormalColor = System.Drawing.Color.FromArgb(220, 220, 220);
            btAppReset.Size = new System.Drawing.Size(pContainer.Width - 36, 23);
            btAppReset.Text = app.isSystemApp ? "Cannot reset system apps" : "Reset";
            btAppReset.Enabled = !app.isSystemApp;
            //
            // btAppUninstall
            //
            btAppUninstall.AutoColor = true;
            btAppUninstall.ButtonImage = null;
            btAppUninstall.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            btAppUninstall.ClickColor = System.Drawing.Color.FromArgb(180, 180, 180);
            btAppUninstall.DrawImage = false;
            btAppUninstall.HoverColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btAppUninstall.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            btAppUninstall.Location = new System.Drawing.Point(18, btAppReset.Location.Y + btAppReset.Height + 5);
            btAppUninstall.Name = "btAppUninstall_" + randomAppID;
            btAppUninstall.NormalColor = System.Drawing.Color.FromArgb(220, 220, 220);
            btAppUninstall.Size = new System.Drawing.Size(pContainer.Width - 36, 23);
            btAppUninstall.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            btAppUninstall.TabIndex = 5;
            btAppUninstall.Text = app.isSystemApp ? "Cannot uninstall system apps" : "Uninstall";
            btAppUninstall.Enabled = !app.isSystemApp;
            //
            // lbSizeOnDisk
            //
            lbSizeOnDisk.AutoSize = true;
            lbSizeOnDisk.Font = new System.Drawing.Font("Ubuntu", 12F);
            lbSizeOnDisk.Location = new System.Drawing.Point(18, btAppUninstall.Location.Y + btAppReset.Height + 5);
            lbSizeOnDisk.Name = "lbSizeOnDisk_" + randomAppID;
            lbSizeOnDisk.Text = "Size on disk: " + app.GetAppSizeInfo(SizeInfoBytes);
            //
            // lbOrigin
            //
            lbOrigin.Font = new System.Drawing.Font("Ubuntu", 12F);
            lbOrigin.Location = new System.Drawing.Point(18, lbSizeOnDisk.Location.Y + lbSizeOnDisk.Height + 5);
            lbOrigin.Name = "lbOrigin_" + randomAppID;
            lbOrigin.AutoSize = true;
            lbOrigin.Text = "Origin: ";
            //
            // lbAppOrigin
            //
            lbAppOrigin.Font = new System.Drawing.Font("Ubuntu", 12F);
            lbAppOrigin.Location = new System.Drawing.Point(lbOrigin.Location.X + lbOrigin.Width, lbOrigin.Location.Y);
            lbAppOrigin.Name = "lbAppOrigin_" + randomAppID;
            lbAppOrigin.Size = new System.Drawing.Size(pContainer.Width - (36 + lbOrigin.Width), lbOrigin.Height * 4);
            lbAppOrigin.Text = app.AppOrigin.ToString() + Environment.NewLine + app.AppOriginInfo;
            //
            // hsEnabled
            //
            hsEnabled.Location = new System.Drawing.Point(18, lbAppOrigin.Location.Y + lbAppOrigin.Height + 20);
            hsEnabled.Name = "hsEnabled_" + randomAppID;
            hsEnabled.Size = new System.Drawing.Size(50, 19);
            hsEnabled.Checked = app.isEnabled;
            hsEnabled.Enabled = !app.isSystemApp;
            //
            // lbEnabled
            //
            lbEnabled.AutoSize = true;
            lbEnabled.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbEnabled.Location = new System.Drawing.Point(hsEnabled.Location.X + hsEnabled.Width + 5, hsEnabled.Location.Y - 2);
            lbEnabled.Name = "lbEnabled_" + randomAppID;
            lbEnabled.Text = "Enabled";
            //
            // lbEnabledInfo
            //
            lbEnabledInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbEnabledInfo.Location = new System.Drawing.Point(lbEnabled.Location.X, lbEnabled.Location.Y + lbEnabled.Height + 5);
            lbEnabledInfo.Name = "lbEnabledInfo_" + randomAppID;
            lbEnabledInfo.AutoSize = true;
            lbEnabledInfo.Text = app.isSystemApp ? "System apps cannot be disabled. " : "Determines if this app can be loaded or not.";
            //
            // hsNotifications
            //
            hsNotifications.Location = new System.Drawing.Point(18, lbEnabledInfo.Location.Y + lbEnabledInfo.Height + 20);
            hsNotifications.Name = "hsNotifications_" + randomAppID;
            hsNotifications.Size = new System.Drawing.Size(50, 19);
            //
            // lbNotifications
            //
            lbNotifications.AutoSize = true;
            lbNotifications.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbNotifications.Location = new System.Drawing.Point(hsNotifications.Location.X + hsNotifications.Width + 5, hsNotifications.Location.Y - 2);
            lbNotifications.Name = "lbNotifications_" + randomAppID;
            lbNotifications.AutoSize = true;
            lbNotifications.Text = "Notifications";
            //
            // lbNotificationsInfo
            //
            lbNotificationsInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbNotificationsInfo.Location = new System.Drawing.Point(lbNotifications.Location.X, lbNotifications.Location.Y + lbNotifications.Height + 5);
            lbNotificationsInfo.Name = "lbNotificationsInfo_" + randomAppID;
            lbNotificationsInfo.AutoSize = true;
            lbNotificationsInfo.Text = "Allows this app to show notifications";
            //
            // hsPrioritize
            //
            hsPrioritize.Location = new System.Drawing.Point(lbNotificationsInfo.Location.X, lbNotificationsInfo.Location.Y + lbNotificationsInfo.Height + 20);
            hsPrioritize.Name = "hsPrioritize_" + randomAppID;
            hsPrioritize.Size = new System.Drawing.Size(50, 19);
            //
            // lbPrioritize
            //
            lbPrioritize.AutoSize = true;
            lbPrioritize.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbPrioritize.Location = new System.Drawing.Point(hsPrioritize.Location.X + hsPrioritize.Width, hsPrioritize.Location.Y - 2);
            lbPrioritize.Name = "lbPrioritize_" + randomAppID;
            lbPrioritize.Text = "Prioritize";
            //
            // lbPrioritieInfo
            //
            lbPrioritieInfo.AutoSize = true;
            lbPrioritieInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbPrioritieInfo.Location = new System.Drawing.Point(lbPrioritize.Location.X, lbPrioritize.Location.Y + lbPrioritize.Height + 5);
            lbPrioritieInfo.Name = "lbPrioritieInfo_" + randomAppID;
            lbPrioritieInfo.Text = "Prioritizes this app\'s notification from other notifications.";
            //
            // hsNotifListener
            //
            hsNotifListener.Location = new System.Drawing.Point(hsPrioritize.Location.X, lbPrioritieInfo.Location.Y + lbPrioritieInfo.Height + 20);
            hsNotifListener.Name = "hsNotifListener_" + randomAppID;
            hsNotifListener.Size = new System.Drawing.Size(50, 19);
            //
            // lbNotifListener
            //
            lbNotifListener.AutoSize = true;
            lbNotifListener.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbNotifListener.Location = new System.Drawing.Point(hsNotifListener.Location.X + hsNotifListener.Width, hsNotifListener.Location.Y - 2);
            lbNotifListener.Name = "lbNotifListener_" + randomAppID;
            lbNotifListener.Text = "Run notification listener at background";
            //
            // lbNotifListenerInfo
            //
            lbNotifListenerInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbNotifListenerInfo.Location = new System.Drawing.Point(lbNotifListener.Location.X, lbNotifListener.Location.Y + lbNotifListener.Height + 5);
            lbNotifListenerInfo.Name = "lbNotifListenerInfo_" + randomAppID;
            lbNotifListenerInfo.AutoSize = true;
            lbNotifListenerInfo.Text = "Allows Yorot to run a background service for retrieving new notifications.";
            //
            // hsRunOnStartup
            //
            hsRunOnStartup.Location = new System.Drawing.Point(hsEnabled.Location.X, lbNotifListenerInfo.Location.Y + lbNotifListenerInfo.Height + 20);
            hsRunOnStartup.Name = "hsRunOnStartup_" + randomAppID;
            hsRunOnStartup.Size = new System.Drawing.Size(50, 19);
            //
            // lbRunOnStartup
            //
            lbRunOnStartup.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbRunOnStartup.Location = new System.Drawing.Point(hsRunOnStartup.Location.X + hsRunOnStartup.Width, hsRunOnStartup.Location.Y - 2);
            lbRunOnStartup.Name = "lbRunOnStartup_" + randomAppID;
            lbRunOnStartup.AutoSize = true;
            lbRunOnStartup.Text = "Run on startup";
            //
            // lbRunOnStartupInfo
            //
            lbRunOnStartupInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbRunOnStartupInfo.Location = new System.Drawing.Point(lbRunOnStartup.Location.X, lbRunOnStartup.Location.Y + lbRunOnStartup.Height + 5);
            lbRunOnStartupInfo.Name = "lbRunOnStartupInfo_" + randomAppID;
            lbRunOnStartupInfo.AutoSize = true;
            lbRunOnStartupInfo.Text = "Starts application on Yorot startup.";
            //
            // hsRunOnIncognito
            //
            hsRunOnIncognito.Location = new System.Drawing.Point(hsEnabled.Location.X, lbRunOnStartupInfo.Location.Y + lbRunOnStartupInfo.Height + 20);
            hsRunOnIncognito.Name = "hsRunOnIncognito_" + randomAppID;
            hsRunOnIncognito.Size = new System.Drawing.Size(50, 19);
            //
            // lbRunOnIncognito
            //
            lbRunOnIncognito.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbRunOnIncognito.Location = new System.Drawing.Point(hsRunOnIncognito.Location.X + hsRunOnIncognito.Width, hsRunOnIncognito.Location.Y - 2);
            lbRunOnIncognito.Name = "lbRunOnIncognito_" + randomAppID;
            lbRunOnIncognito.AutoSize = true;
            lbRunOnIncognito.Text = "Run on Incognito mode";
            //
            // lbRunOnIncognitoInfo
            //
            lbRunOnIncognitoInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbRunOnIncognitoInfo.Location = new System.Drawing.Point(lbRunOnIncognito.Location.X, lbRunOnIncognito.Location.Y + lbRunOnIncognito.Height + 5);
            lbRunOnIncognitoInfo.Name = "lbRunOnIncognitoInfo_" + randomAppID;
            lbRunOnIncognitoInfo.AutoSize = true;
            lbRunOnIncognitoInfo.Text = "Allows this app to run on Incognito mode.";

            btContainerBack.Tag = (sbyte)5;

            //
            // tpAppPage
            //
            pContainer.Controls.Add(hsRunOnIncognito);
            pContainer.Controls.Add(hsRunOnStartup);
            pContainer.Controls.Add(hsNotifListener);
            pContainer.Controls.Add(hsPrioritize);
            pContainer.Controls.Add(hsEnabled);
            pContainer.Controls.Add(hsNotifications);
            pContainer.Controls.Add(lbNotifListener);
            pContainer.Controls.Add(lbRunOnIncognito);
            pContainer.Controls.Add(lbRunOnStartup);
            pContainer.Controls.Add(lbPrioritize);
            pContainer.Controls.Add(lbEnabled);
            pContainer.Controls.Add(lbEnabledInfo);
            pContainer.Controls.Add(lbRunOnIncognitoInfo);
            pContainer.Controls.Add(lbNotifications);
            pContainer.Controls.Add(lbRunOnStartupInfo);
            pContainer.Controls.Add(lbNotifListenerInfo);
            pContainer.Controls.Add(lbPrioritieInfo);
            pContainer.Controls.Add(lbNotificationsInfo);
            pContainer.Controls.Add(lbAppOrigin);
            pContainer.Controls.Add(lbOrigin);
            pContainer.Controls.Add(lbSizeOnDisk);
            pContainer.Controls.Add(btAppUninstall);
            pContainer.Controls.Add(btAppReset);
            pContainer.Controls.Add(pbAppIcon);
            pContainer.Controls.Add(lbAppCName);
            pContainer.Controls.Add(lbAppVer);
            pContainer.Controls.Add(lbAppAuthor);
            pContainer.Controls.Add(lbAppName);
        }

        private void GenerateExtTab(YorotExtension ext)
        {
            string randomAppID = HTAlt.Tools.GenerateRandomText(17);
            // TODO: Add all language specific ones to lists.
            System.Windows.Forms.PictureBox pbAppIcon = new System.Windows.Forms.PictureBox();
            System.Windows.Forms.Label lbAppCName = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAppVer = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAppAuthor = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAppName = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTButton btAppReset = new HTAlt.WinForms.HTButton();
            HTAlt.WinForms.HTButton btAppUninstall = new HTAlt.WinForms.HTButton();
            System.Windows.Forms.Label lbSizeOnDisk = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsNotifications = new HTAlt.WinForms.HTSwitch();
            System.Windows.Forms.Label lbNotifications = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbEnabled = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbEnabledInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbPrioritize = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsPrioritize = new HTAlt.WinForms.HTSwitch();
            HTAlt.WinForms.HTSwitch hsEnabled = new HTAlt.WinForms.HTSwitch();
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
            System.Windows.Forms.Label lbRunOnIncognitoInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbRunOnIncognito = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsRunOnIncognito = new HTAlt.WinForms.HTSwitch();
            lbContainerTitle.Text = ext.Name;
            //
            // pbAppIcon
            //
            pbAppIcon.Location = new System.Drawing.Point(18, 18);
            pbAppIcon.Name = "pbExtIcon_" + randomAppID;
            pbAppIcon.Size = new System.Drawing.Size(64, 64);
            pbAppIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pbAppIcon.Image = Properties.Resources.addon_settings;
            pbAppIcon.TabStop = false;
            //
            // lbAppName
            //
            lbAppName.AutoSize = true;
            lbAppName.Font = new System.Drawing.Font("Ubuntu", 15F);
            lbAppName.Location = new System.Drawing.Point(95, 18);
            lbAppName.Name = "lbExtName_" + randomAppID;
            lbAppName.Text = ext.Name;
            //
            // lbAppVer
            //
            lbAppVer.AutoSize = true;
            lbAppVer.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAppVer.Location = new System.Drawing.Point(lbAppName.Location.X, lbAppName.Location.Y + lbAppName.Height);
            lbAppVer.Name = "lbextVer_" + randomAppID;
            lbAppVer.Text = "Version " + ext.Version;
            //
            // lbAppCName
            //
            lbAppCName.AutoSize = true;
            lbAppCName.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAppCName.Location = new System.Drawing.Point(lbAppVer.Location.X, lbAppVer.Location.Y + lbAppVer.Height + 2);
            lbAppCName.Name = "lbExtCName_" + randomAppID;
            lbAppCName.Text = ext.CodeName;
            //
            // lbAppAuthor
            //
            lbAppAuthor.AutoSize = true;
            lbAppAuthor.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAppAuthor.Location = new System.Drawing.Point(95, lbAppCName.Location.Y + lbAppCName.Height + 2);
            lbAppAuthor.Name = "lbExtAuthor_" + randomAppID;
            lbAppAuthor.Text = ext.Author;
            //
            // btAppUninstall
            //
            btAppUninstall.AutoColor = true;
            btAppUninstall.ButtonImage = null;
            btAppUninstall.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            btAppUninstall.ClickColor = System.Drawing.Color.FromArgb(180, 180, 180);
            btAppUninstall.DrawImage = false;
            btAppUninstall.HoverColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btAppUninstall.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            btAppUninstall.Location = new System.Drawing.Point(18, lbAppAuthor.Location.Y + lbAppAuthor.Height + 5);
            btAppUninstall.Name = "btAppUninstall_" + randomAppID;
            btAppUninstall.NormalColor = System.Drawing.Color.FromArgb(220, 220, 220);
            btAppUninstall.Size = new System.Drawing.Size(pContainer.Width - 36, 23);
            btAppUninstall.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            btAppUninstall.TabIndex = 5;
            btAppUninstall.Text = ext.isSystemExt ? "Cannot uninstall system extensions" : "Uninstall";
            btAppUninstall.Enabled = !ext.isSystemExt;
            //
            // hsEnabled
            //
            hsEnabled.Location = new System.Drawing.Point(18, btAppUninstall.Location.Y + btAppReset.Height + 5);
            hsEnabled.Name = "hsEnabled_" + randomAppID;
            hsEnabled.Size = new System.Drawing.Size(50, 19);
            hsEnabled.Checked = ext.Enabled;
            hsEnabled.Enabled = !ext.isSystemExt;
            //
            // lbEnabled
            //
            lbEnabled.AutoSize = true;
            lbEnabled.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbEnabled.Location = new System.Drawing.Point(hsEnabled.Location.X + hsEnabled.Width + 5, hsEnabled.Location.Y - 2);
            lbEnabled.Name = "lbEnabled_" + randomAppID;
            lbEnabled.Text = "Enabled";
            //
            // lbEnabledInfo
            //
            lbEnabledInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbEnabledInfo.Location = new System.Drawing.Point(lbEnabled.Location.X, lbEnabled.Location.Y + lbEnabled.Height + 5);
            lbEnabledInfo.Name = "lbEnabledInfo_" + randomAppID;
            lbEnabledInfo.AutoSize = true;
            lbEnabledInfo.Text = ext.isSystemExt ? "System extensions cannot be disabled. " : "Determines if this extension can be loaded or not.";
            //
            // hsNotifications
            //
            hsNotifications.Location = new System.Drawing.Point(18, lbEnabledInfo.Location.Y + lbEnabledInfo.Height + 20);
            hsNotifications.Name = "hsNotifications_" + randomAppID;
            hsNotifications.Size = new System.Drawing.Size(50, 19);
            hsNotifications.Checked = true;
            //
            // lbNotifications
            //
            lbNotifications.AutoSize = true;
            lbNotifications.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbNotifications.Location = new System.Drawing.Point(hsNotifications.Location.X + hsNotifications.Width + 5, hsNotifications.Location.Y - 2);
            lbNotifications.Name = "lbNotifications_" + randomAppID;
            lbNotifications.AutoSize = true;
            lbNotifications.Text = "Show menu options";
            //
            // lbNotificationsInfo
            //
            lbNotificationsInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbNotificationsInfo.Location = new System.Drawing.Point(lbNotifications.Location.X, lbNotifications.Location.Y + lbNotifications.Height + 5);
            lbNotificationsInfo.Name = "lbNotificationsInfo_" + randomAppID;
            lbNotificationsInfo.AutoSize = true;
            lbNotificationsInfo.Text = "Allows this extension to show up in menus such as the right-click menu.";
            //
            // hsRunOnStartup
            //
            hsRunOnStartup.Location = new System.Drawing.Point(lbNotificationsInfo.Location.X, lbNotificationsInfo.Location.Y + lbNotificationsInfo.Height + 20);
            hsRunOnStartup.Name = "hsRunOnStartup_" + randomAppID;
            hsRunOnStartup.Size = new System.Drawing.Size(50, 19);
            //
            // lbRunOnStartup
            //
            lbRunOnStartup.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbRunOnStartup.Location = new System.Drawing.Point(hsRunOnStartup.Location.X + hsRunOnStartup.Width, hsRunOnStartup.Location.Y - 2);
            lbRunOnStartup.Name = "lbRunOnStartup_" + randomAppID;
            lbRunOnStartup.AutoSize = true;
            lbRunOnStartup.Text = "Run on startup";
            //
            // lbRunOnStartupInfo
            //
            lbRunOnStartupInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbRunOnStartupInfo.Location = new System.Drawing.Point(lbRunOnStartup.Location.X, lbRunOnStartup.Location.Y + lbRunOnStartup.Height + 5);
            lbRunOnStartupInfo.Name = "lbRunOnStartupInfo_" + randomAppID;
            lbRunOnStartupInfo.AutoSize = true;
            lbRunOnStartupInfo.Text = "Starts extension on Yorot startup.";
            //
            // hsRunOnIncognito
            //
            hsRunOnIncognito.Location = new System.Drawing.Point(hsEnabled.Location.X, lbRunOnStartupInfo.Location.Y + lbRunOnStartupInfo.Height + 20);
            hsRunOnIncognito.Name = "hsRunOnIncognito_" + randomAppID;
            hsRunOnIncognito.Size = new System.Drawing.Size(50, 19);
            //
            // lbRunOnIncognito
            //
            lbRunOnIncognito.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbRunOnIncognito.Location = new System.Drawing.Point(hsRunOnIncognito.Location.X + hsRunOnIncognito.Width, hsRunOnIncognito.Location.Y - 2);
            lbRunOnIncognito.Name = "lbRunOnIncognito_" + randomAppID;
            lbRunOnIncognito.AutoSize = true;
            lbRunOnIncognito.Text = "Run on Incognito mode";
            //
            // lbRunOnIncognitoInfo
            //
            lbRunOnIncognitoInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbRunOnIncognitoInfo.Location = new System.Drawing.Point(lbRunOnIncognito.Location.X, lbRunOnIncognito.Location.Y + lbRunOnIncognito.Height + 5);
            lbRunOnIncognitoInfo.Name = "lbRunOnIncognitoInfo_" + randomAppID;
            lbRunOnIncognitoInfo.AutoSize = true;
            lbRunOnIncognitoInfo.Text = "Allows this extension to run on Incognito mode.";

            btContainerBack.Tag = (sbyte)5;

            //
            // tpAppPage
            //
            pContainer.Controls.Add(hsRunOnIncognito);
            pContainer.Controls.Add(hsRunOnStartup);
            pContainer.Controls.Add(hsNotifListener);
            pContainer.Controls.Add(hsPrioritize);
            pContainer.Controls.Add(hsEnabled);
            pContainer.Controls.Add(hsNotifications);
            pContainer.Controls.Add(lbNotifListener);
            pContainer.Controls.Add(lbRunOnIncognito);
            pContainer.Controls.Add(lbRunOnStartup);
            pContainer.Controls.Add(lbPrioritize);
            pContainer.Controls.Add(lbEnabled);
            pContainer.Controls.Add(lbEnabledInfo);
            pContainer.Controls.Add(lbRunOnIncognitoInfo);
            pContainer.Controls.Add(lbNotifications);
            pContainer.Controls.Add(lbRunOnStartupInfo);
            pContainer.Controls.Add(lbNotifListenerInfo);
            pContainer.Controls.Add(lbPrioritieInfo);
            pContainer.Controls.Add(lbNotificationsInfo);
            pContainer.Controls.Add(lbAppOrigin);
            pContainer.Controls.Add(lbOrigin);
            pContainer.Controls.Add(lbSizeOnDisk);
            pContainer.Controls.Add(btAppUninstall);
            pContainer.Controls.Add(btAppReset);
            pContainer.Controls.Add(pbAppIcon);
            pContainer.Controls.Add(lbAppCName);
            pContainer.Controls.Add(lbAppVer);
            pContainer.Controls.Add(lbAppAuthor);
            pContainer.Controls.Add(lbAppName);
        }

        private void GenerateSiteTab(YorotSite site)
        {
            string randomAppID = HTAlt.Tools.GenerateRandomText(17);
            // TODO: Add all language specific ones to lists.
            System.Windows.Forms.PictureBox pbSiteIcon = new System.Windows.Forms.PictureBox();
            System.Windows.Forms.Label lbAppCName = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAppVer = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAppAuthor = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAppName = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTButton btAppReset = new HTAlt.WinForms.HTButton();
            HTAlt.WinForms.HTButton btAppUninstall = new HTAlt.WinForms.HTButton();
            System.Windows.Forms.Label lbSizeOnDisk = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsNotifications = new HTAlt.WinForms.HTSwitch();
            System.Windows.Forms.Label lbNotifications = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbEnabled = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbEnabledInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbPrioritize = new System.Windows.Forms.Label();
            ComboBox cbPriority = new ComboBox();
            HTAlt.WinForms.HTSwitch hsEnabled = new HTAlt.WinForms.HTSwitch();
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
            System.Windows.Forms.Label lbRunOnIncognitoInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbRunOnIncognito = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsRunOnIncognito = new HTAlt.WinForms.HTSwitch();
            System.Windows.Forms.Label lbAllowWEInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAllowWE = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsAllowWE = new HTAlt.WinForms.HTSwitch();
            lbContainerTitle.Text = site.Name;
            //
            // pbAppIcon
            //
            pbSiteIcon.Location = new System.Drawing.Point(18, 18);
            pbSiteIcon.Name = "pbAppIcon_" + randomAppID;
            pbSiteIcon.Size = new System.Drawing.Size(64, 64);
            pbSiteIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            //pbAppIcon.Image = YorotTools.GetSiteIcon(site);
            //
            // lbAppName
            //
            lbAppName.AutoSize = true;
            lbAppName.Font = new System.Drawing.Font("Ubuntu", 15F);
            lbAppName.Location = new System.Drawing.Point(95, 18);
            lbAppName.Name = "lbAppName_" + randomAppID;
            lbAppName.Text = site.Name;
            //
            // lbAppCName
            //
            lbAppCName.AutoSize = true;
            lbAppCName.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAppCName.Location = new System.Drawing.Point(95, lbAppName.Location.Y + lbAppName.Height + 2);
            lbAppCName.Name = "lbAppCName_" + randomAppID;
            lbAppCName.Text = site.Url;
            //
            // btAppReset
            //
            btAppReset.AutoColor = true;
            btAppReset.ButtonImage = null;
            btAppReset.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            btAppReset.ClickColor = System.Drawing.Color.FromArgb(180, 180, 180);
            btAppReset.DrawImage = false;
            btAppReset.HoverColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btAppReset.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            btAppReset.Location = new System.Drawing.Point(18, pbSiteIcon.Location.Y + pbSiteIcon.Height + 5);
            btAppReset.Name = "btAppReset_" + randomAppID;
            btAppReset.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            btAppReset.NormalColor = System.Drawing.Color.FromArgb(220, 220, 220);
            btAppReset.Size = new System.Drawing.Size(pContainer.Width - 36, 23);
            btAppReset.Text = "Reset all to default";
            //
            // btAppUninstall
            //
            btAppUninstall.AutoColor = true;
            btAppUninstall.ButtonImage = null;
            btAppUninstall.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            btAppUninstall.ClickColor = System.Drawing.Color.FromArgb(180, 180, 180);
            btAppUninstall.DrawImage = false;
            btAppUninstall.HoverColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btAppUninstall.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            btAppUninstall.Location = new System.Drawing.Point(18, btAppReset.Location.Y + btAppReset.Height + 5);
            btAppUninstall.Name = "btAppUninstall_" + randomAppID;
            btAppUninstall.NormalColor = System.Drawing.Color.FromArgb(220, 220, 220);
            btAppUninstall.Size = new System.Drawing.Size(pContainer.Width - 36, 23);
            btAppUninstall.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            btAppUninstall.TabIndex = 5;
            btAppUninstall.Text = "Remove";
            //
            // hsEnabled
            //
            hsEnabled.Location = new System.Drawing.Point();
            hsEnabled.Name = "hsEnabled_" + randomAppID;
            hsEnabled.Size = new System.Drawing.Size(50, 19);
            hsEnabled.Checked = site.Permissions.allowYS.Allowance == YorotPermissionMode.Allow;
            //
            // lbEnabled
            //
            lbEnabled.AutoSize = true;
            lbEnabled.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbEnabled.Location = new System.Drawing.Point(hsEnabled.Location.X + hsEnabled.Width + 5, hsEnabled.Location.Y - 2);
            lbEnabled.Name = "lbEnabled_" + randomAppID;
            lbEnabled.Text = "Allow access to Yorot Special";
            //
            // lbEnabledInfo
            //
            lbEnabledInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbEnabledInfo.Location = new System.Drawing.Point(lbEnabled.Location.X, lbEnabled.Location.Y + lbEnabled.Height + 5);
            lbEnabledInfo.Name = "lbEnabledInfo_" + randomAppID;
            lbEnabledInfo.AutoSize = true;
            lbEnabledInfo.Text = "Determines if this site can access to Yorot Specials such as theme or hardware information.";
            //
            // hsNotifications
            //
            hsNotifications.Location = new System.Drawing.Point(18, lbEnabledInfo.Location.Y + lbEnabledInfo.Height + 20);
            hsNotifications.Name = "hsNotifications_" + randomAppID;
            hsNotifications.Size = new System.Drawing.Size(50, 19);
            hsNotifications.Checked = site.Permissions.allowNotif.Allowance == YorotPermissionMode.Allow;
            //
            // lbNotifications
            //
            lbNotifications.AutoSize = true;
            lbNotifications.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbNotifications.Location = new System.Drawing.Point(hsNotifications.Location.X + hsNotifications.Width + 5, hsNotifications.Location.Y - 2);
            lbNotifications.Name = "lbNotifications_" + randomAppID;
            lbNotifications.AutoSize = true;
            lbNotifications.Text = "Notifications";
            //
            // lbNotificationsInfo
            //
            lbNotificationsInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbNotificationsInfo.Location = new System.Drawing.Point(lbNotifications.Location.X, lbNotifications.Location.Y + lbNotifications.Height + 5);
            lbNotificationsInfo.Name = "lbNotificationsInfo_" + randomAppID;
            lbNotificationsInfo.AutoSize = true;
            lbNotificationsInfo.Text = "Allows this site to show notifications";
            //
            // lbPrioritize
            //
            lbPrioritize.AutoSize = true;
            lbPrioritize.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbPrioritize.Location = new System.Drawing.Point(lbNotificationsInfo.Location.X, lbNotificationsInfo.Location.Y + lbNotificationsInfo.Height + 5);
            lbPrioritize.Name = "lbPrioritize_" + randomAppID;
            lbPrioritize.Text = "Prioritize";
            //
            // cbPriority
            //
            cbPriority.Location = new System.Drawing.Point(lbPrioritize.Location.X + lbPrioritize.Width, lbPrioritize.Location.Y - 2);
            cbPriority.Name = "cbPriority_" + randomAppID;
            cbPriority.Size = new System.Drawing.Size(100, 19);
            cbPriority.DropDownStyle = ComboBoxStyle.DropDownList;
            cbPriority.Items.Add("Low");
            cbPriority.Items.Add("Normal");
            cbPriority.Items.Add("High");
            cbPriority.SelectedIndex = site.Permissions.notifPriority + 1;
            //
            // lbPrioritieInfo
            //
            lbPrioritieInfo.AutoSize = true;
            lbPrioritieInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbPrioritieInfo.Location = new System.Drawing.Point(lbPrioritize.Location.X, lbPrioritize.Location.Y + lbPrioritize.Height + 5);
            lbPrioritieInfo.Name = "lbPrioritieInfo_" + randomAppID;
            lbPrioritieInfo.Text = "Prioritizes this site\'s notification from other notifications.";
            //
            // hsNotifListener
            //
            hsNotifListener.Location = new System.Drawing.Point(lbPrioritize.Location.X, lbPrioritieInfo.Location.Y + lbPrioritieInfo.Height + 20);
            hsNotifListener.Name = "hsNotifListener_" + randomAppID;
            hsNotifListener.Size = new System.Drawing.Size(50, 19);
            hsNotifListener.Checked = site.Permissions.startNotifOnBoot;
            //
            // lbNotifListener
            //
            lbNotifListener.AutoSize = true;
            lbNotifListener.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbNotifListener.Location = new System.Drawing.Point(hsNotifListener.Location.X + hsNotifListener.Width, hsNotifListener.Location.Y - 2);
            lbNotifListener.Name = "lbNotifListener_" + randomAppID;
            lbNotifListener.Text = "Run notification listener at background";
            //
            // lbNotifListenerInfo
            //
            lbNotifListenerInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbNotifListenerInfo.Location = new System.Drawing.Point(lbNotifListener.Location.X, lbNotifListener.Location.Y + lbNotifListener.Height + 5);
            lbNotifListenerInfo.Name = "lbNotifListenerInfo_" + randomAppID;
            lbNotifListenerInfo.AutoSize = true;
            lbNotifListenerInfo.Text = "Allows Yorot to run a background service for retrieving new notifications for this site.";
            //
            // hsRunOnStartup
            //
            hsRunOnStartup.Location = new System.Drawing.Point(hsEnabled.Location.X, lbNotifListenerInfo.Location.Y + lbNotifListenerInfo.Height + 20);
            hsRunOnStartup.Name = "hsRunOnStartup_" + randomAppID;
            hsRunOnStartup.Size = new System.Drawing.Size(50, 19);
            hsRunOnStartup.Checked = site.Permissions.allowCam.Allowance == YorotPermissionMode.Allow;
            //
            // lbRunOnStartup
            //
            lbRunOnStartup.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbRunOnStartup.Location = new System.Drawing.Point(hsRunOnStartup.Location.X + hsRunOnStartup.Width, hsRunOnStartup.Location.Y - 2);
            lbRunOnStartup.Name = "lbRunOnStartup_" + randomAppID;
            lbRunOnStartup.AutoSize = true;
            lbRunOnStartup.Text = "Allow Camera Access";
            //
            // lbRunOnStartupInfo
            //
            lbRunOnStartupInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbRunOnStartupInfo.Location = new System.Drawing.Point(lbRunOnStartup.Location.X, lbRunOnStartup.Location.Y + lbRunOnStartup.Height + 5);
            lbRunOnStartupInfo.Name = "lbRunOnStartupInfo_" + randomAppID;
            lbRunOnStartupInfo.AutoSize = true;
            lbRunOnStartupInfo.Text = "Allows this site to access cameras.";
            //
            // hsRunOnIncognito
            //
            hsRunOnIncognito.Location = new System.Drawing.Point(hsEnabled.Location.X, lbRunOnStartupInfo.Location.Y + lbRunOnStartupInfo.Height + 20);
            hsRunOnIncognito.Name = "hsRunOnIncognito_" + randomAppID;
            hsRunOnIncognito.Size = new System.Drawing.Size(50, 19);
            hsRunOnIncognito.Checked = site.Permissions.allowMic.Allowance == YorotPermissionMode.Allow;
            //
            // lbRunOnIncognito
            //
            lbRunOnIncognito.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbRunOnIncognito.Location = new System.Drawing.Point(hsRunOnIncognito.Location.X + hsRunOnIncognito.Width, hsRunOnIncognito.Location.Y - 2);
            lbRunOnIncognito.Name = "lbRunOnIncognito_" + randomAppID;
            lbRunOnIncognito.AutoSize = true;
            lbRunOnIncognito.Text = "Allow Microphone Access";
            //
            // lbRunOnIncognitoInfo
            //
            lbRunOnIncognitoInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbRunOnIncognitoInfo.Location = new System.Drawing.Point(lbRunOnIncognito.Location.X, lbRunOnIncognito.Location.Y + lbRunOnIncognito.Height + 5);
            lbRunOnIncognitoInfo.Name = "lbRunOnIncognitoInfo_" + randomAppID;
            lbRunOnIncognitoInfo.AutoSize = true;
            lbRunOnIncognitoInfo.Text = "Allows this site to access microphones.";
            //
            // hsAllowWE
            //
            hsAllowWE.Location = new System.Drawing.Point(hsEnabled.Location.X, lbRunOnIncognitoInfo.Location.Y + lbRunOnIncognitoInfo.Height + 20);
            hsAllowWE.Name = "hsRunOnIncognito_" + randomAppID;
            hsAllowWE.Size = new System.Drawing.Size(50, 19);
            hsAllowWE.Checked = site.Permissions.allowWE.Allowance == YorotPermissionMode.Allow;
            //
            // lbAllowWE
            //
            lbAllowWE.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAllowWE.Location = new System.Drawing.Point(hsAllowWE.Location.X + hsAllowWE.Width, hsAllowWE.Location.Y - 2);
            lbAllowWE.Name = "lbRunOnIncognito_" + randomAppID;
            lbAllowWE.AutoSize = true;
            lbAllowWE.Text = "Allow Web Engines";
            //
            // lbAllwoWEInfo
            //
            lbAllowWEInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbAllowWEInfo.Location = new System.Drawing.Point(lbAllowWE.Location.X, lbAllowWE.Location.Y + lbAllowWE.Height + 5);
            lbAllowWEInfo.Name = "lbRunOnIncognitoInfo_" + randomAppID;
            lbAllowWEInfo.AutoSize = true;
            lbAllowWEInfo.Text = "Allows this site to access web engines.";

            btContainerBack.Tag = (sbyte)5;

            //
            // tpAppPage
            //
            pContainer.Controls.Add(hsRunOnIncognito);
            pContainer.Controls.Add(hsRunOnStartup);
            pContainer.Controls.Add(hsNotifListener);
            pContainer.Controls.Add(cbPriority);
            pContainer.Controls.Add(hsEnabled);
            pContainer.Controls.Add(hsNotifications);
            pContainer.Controls.Add(lbNotifListener);
            pContainer.Controls.Add(lbRunOnIncognito);
            pContainer.Controls.Add(lbRunOnStartup);
            pContainer.Controls.Add(lbPrioritize);
            pContainer.Controls.Add(lbEnabled);
            pContainer.Controls.Add(lbEnabledInfo);
            pContainer.Controls.Add(lbRunOnIncognitoInfo);
            pContainer.Controls.Add(lbNotifications);
            pContainer.Controls.Add(lbRunOnStartupInfo);
            pContainer.Controls.Add(lbNotifListenerInfo);
            pContainer.Controls.Add(lbPrioritieInfo);
            pContainer.Controls.Add(lbNotificationsInfo);
            pContainer.Controls.Add(lbAppOrigin);
            pContainer.Controls.Add(lbOrigin);
            pContainer.Controls.Add(lbSizeOnDisk);
            pContainer.Controls.Add(btAppUninstall);
            pContainer.Controls.Add(btAppReset);
            pContainer.Controls.Add(pbSiteIcon);
            pContainer.Controls.Add(lbAppCName);
            pContainer.Controls.Add(lbAppVer);
            pContainer.Controls.Add(lbAppAuthor);
            pContainer.Controls.Add(lbAppName);
        }
        private void GenerateWETab(YorotWebEngine we)
        {
            string randomAppID = HTAlt.Tools.GenerateRandomText(17);
            // TODO: Add all language specific ones to lists.
            System.Windows.Forms.PictureBox pbAppIcon = new System.Windows.Forms.PictureBox();
            System.Windows.Forms.Label lbAppCName = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAppVer = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAppAuthor = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAppName = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTButton btAppReset = new HTAlt.WinForms.HTButton();
            HTAlt.WinForms.HTButton btAppUninstall = new HTAlt.WinForms.HTButton();
            System.Windows.Forms.Label lbSizeOnDisk = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsNotifications = new HTAlt.WinForms.HTSwitch();
            System.Windows.Forms.Label lbNotifications = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbEnabled = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbEnabledInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbPrioritize = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsPrioritize = new HTAlt.WinForms.HTSwitch();
            HTAlt.WinForms.HTSwitch hsEnabled = new HTAlt.WinForms.HTSwitch();
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
            System.Windows.Forms.Label lbRunOnIncognitoInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbRunOnIncognito = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsRunOnIncognito = new HTAlt.WinForms.HTSwitch();
            lbContainerTitle.Text = we.Name;
            //
            // pbAppIcon
            //
            pbAppIcon.Location = new System.Drawing.Point(18, 18);
            pbAppIcon.Name = "pbAppIcon_" + randomAppID;
            pbAppIcon.Size = new System.Drawing.Size(64, 64);
            pbAppIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            //pbAppIcon.Image = YorotTools.GetWEIcon(we);
            pbAppIcon.TabStop = false;
            //
            // lbAppName
            //
            lbAppName.AutoSize = true;
            lbAppName.Font = new System.Drawing.Font("Ubuntu", 15F);
            lbAppName.Location = new System.Drawing.Point(95, 18);
            lbAppName.Name = "lbAppName_" + randomAppID;
            lbAppName.Text = we.Name;
            //
            // lbAppVer
            //
            lbAppVer.AutoSize = true;
            lbAppVer.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAppVer.Location = new System.Drawing.Point(lbAppName.Location.X, lbAppName.Location.Y + lbAppName.Height + 2);
            lbAppVer.Name = "lbAppVer_" + randomAppID;
            lbAppVer.Text = Version.Replace("[V]", "" + we.Version);
            lbWEVer = lbAppVer;
            //
            // lbAppCName
            //
            lbAppCName.AutoSize = true;
            lbAppCName.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAppCName.Location = new System.Drawing.Point(lbAppName.Location.X, lbAppVer.Location.Y + lbAppVer.Height + 2);
            lbAppCName.Name = "lbAppCName_" + randomAppID;
            lbAppCName.Text = we.CodeName;
            //
            // lbAppAuthor
            //
            lbAppAuthor.AutoSize = true;
            lbAppAuthor.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAppAuthor.Location = new System.Drawing.Point(95, lbAppCName.Location.Y + lbAppCName.Height + 2);
            lbAppAuthor.Name = "lbAppAuthor_" + randomAppID;
            lbAppAuthor.Text = we.Author;
            //
            // btAppReset
            //
            btAppReset.AutoColor = true;
            btAppReset.ButtonImage = null;
            btAppReset.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            btAppReset.ClickColor = System.Drawing.Color.FromArgb(180, 180, 180);
            btAppReset.DrawImage = false;
            btAppReset.HoverColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btAppReset.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            btAppReset.Location = new System.Drawing.Point(18, lbAppAuthor.Location.Y + lbAppAuthor.Height + 5);
            btAppReset.Name = "btAppReset_" + randomAppID;
            btAppReset.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            btAppReset.NormalColor = System.Drawing.Color.FromArgb(220, 220, 220);
            btAppReset.Size = new System.Drawing.Size(pContainer.Width - 36, 23);
            btAppReset.Text = Reset;
            lbWEReset = btAppReset;
            //
            // btAppUninstall
            //
            btAppUninstall.AutoColor = true;
            btAppUninstall.ButtonImage = null;
            btAppUninstall.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            btAppUninstall.ClickColor = System.Drawing.Color.FromArgb(180, 180, 180);
            btAppUninstall.DrawImage = false;
            btAppUninstall.HoverColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btAppUninstall.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            btAppUninstall.Location = new System.Drawing.Point(18, btAppReset.Location.Y + btAppReset.Height + 5);
            btAppUninstall.Name = "btAppUninstall_" + randomAppID;
            btAppUninstall.NormalColor = System.Drawing.Color.FromArgb(220, 220, 220);
            btAppUninstall.Size = new System.Drawing.Size(pContainer.Width - 36, 23);
            btAppUninstall.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            btAppUninstall.TabIndex = 5;
            btAppUninstall.Text = Uninstall;
            lbWEUninst = btAppUninstall;
            //
            // lbSizeOnDisk
            //
            lbSizeOnDisk.AutoSize = true;
            lbSizeOnDisk.Font = new System.Drawing.Font("Ubuntu", 12F);
            lbSizeOnDisk.Location = new System.Drawing.Point(18, btAppUninstall.Location.Y + btAppReset.Height + 5);
            lbSizeOnDisk.Name = "lbSizeOnDisk_" + randomAppID;
            lbSizeOnDisk.Text = WESizeOnDisk.Replace("[S]", we.GetWESizeInfo(SizeInfoBytes));
            lbWESize = lbSizeOnDisk;
            //
            // lbOrigin
            //
            lbOrigin.Font = new System.Drawing.Font("Ubuntu", 12F);
            lbOrigin.Location = new System.Drawing.Point(18, lbSizeOnDisk.Location.Y + lbSizeOnDisk.Height + 5);
            lbOrigin.Name = "lbOrigin_" + randomAppID;
            lbOrigin.AutoSize = true;
            lbOrigin.Text = WEDesc;
            lbWEDesc = lbOrigin;
            //
            // lbAppOrigin
            //
            lbAppOrigin.Font = new System.Drawing.Font("Ubuntu", 12F);
            lbAppOrigin.Location = new System.Drawing.Point(lbOrigin.Location.X + lbOrigin.Width, lbOrigin.Location.Y);
            lbAppOrigin.Name = "lbAppOrigin_" + randomAppID;
            lbAppOrigin.Size = new System.Drawing.Size(pContainer.Width - (36 + lbOrigin.Width), lbOrigin.Height * 4);
            lbAppOrigin.Text = we.Desc;
            //
            // hsEnabled
            //
            hsEnabled.Location = new System.Drawing.Point(18, lbAppOrigin.Location.Y + lbAppOrigin.Height + 20);
            hsEnabled.Name = "hsEnabled_" + randomAppID;
            hsEnabled.Size = new System.Drawing.Size(50, 19);
            hsEnabled.Checked = we.isEnabled;
            //
            // lbEnabled
            //
            lbEnabled.AutoSize = true;
            lbEnabled.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbEnabled.Location = new System.Drawing.Point(hsEnabled.Location.X + hsEnabled.Width + 5, hsEnabled.Location.Y - 2);
            lbEnabled.Name = "lbEnabled_" + randomAppID;
            lbEnabled.Text = AppEnabled;
            lbWEEnabled = lbEnabled;
            //
            // lbEnabledInfo
            //
            lbEnabledInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbEnabledInfo.Location = new System.Drawing.Point(lbEnabled.Location.X, lbEnabled.Location.Y + lbEnabled.Height + 5);
            lbEnabledInfo.Name = "lbEnabledInfo_" + randomAppID;
            lbEnabledInfo.AutoSize = true;
            lbEnabledInfo.Text = WEEnabledInfo;
            lbWEEnabledInfo = lbEnabledInfo;
            //
            // hsRunOnIncognito
            //
            hsRunOnIncognito.Location = new System.Drawing.Point(hsEnabled.Location.X, lbEnabledInfo.Location.Y + lbEnabledInfo.Height + 20);
            hsRunOnIncognito.Name = "hsRunOnIncognito_" + randomAppID;
            hsRunOnIncognito.Size = new System.Drawing.Size(50, 19);
            //
            // lbRunOnIncognito
            //
            lbRunOnIncognito.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbRunOnIncognito.Location = new System.Drawing.Point(hsRunOnIncognito.Location.X + hsRunOnIncognito.Width, hsRunOnIncognito.Location.Y - 2);
            lbRunOnIncognito.Name = "lbRunOnIncognito_" + randomAppID;
            lbRunOnIncognito.AutoSize = true;
            lbRunOnIncognito.Text = RunInc;
            lbWERunInc = lbRunOnIncognito;
            //
            // lbRunOnIncognitoInfo
            //
            lbRunOnIncognitoInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbRunOnIncognitoInfo.Location = new System.Drawing.Point(lbRunOnIncognito.Location.X, lbRunOnIncognito.Location.Y + lbRunOnIncognito.Height + 5);
            lbRunOnIncognitoInfo.Name = "lbRunOnIncognitoInfo_" + randomAppID;
            lbRunOnIncognitoInfo.AutoSize = true;
            lbRunOnIncognitoInfo.Text = WERunIncInfo;
            lbWERunIncInfo = lbRunOnIncognitoInfo;

            btContainerBack.Tag = (sbyte)5;
            //
            // tpAppPage
            //
            pContainer.Controls.Add(hsRunOnIncognito);
            pContainer.Controls.Add(hsRunOnStartup);
            pContainer.Controls.Add(hsNotifListener);
            pContainer.Controls.Add(hsPrioritize);
            pContainer.Controls.Add(hsEnabled);
            pContainer.Controls.Add(hsNotifications);
            pContainer.Controls.Add(lbNotifListener);
            pContainer.Controls.Add(lbRunOnIncognito);
            pContainer.Controls.Add(lbRunOnStartup);
            pContainer.Controls.Add(lbPrioritize);
            pContainer.Controls.Add(lbEnabled);
            pContainer.Controls.Add(lbEnabledInfo);
            pContainer.Controls.Add(lbRunOnIncognitoInfo);
            pContainer.Controls.Add(lbNotifications);
            pContainer.Controls.Add(lbRunOnStartupInfo);
            pContainer.Controls.Add(lbNotifListenerInfo);
            pContainer.Controls.Add(lbPrioritieInfo);
            pContainer.Controls.Add(lbNotificationsInfo);
            pContainer.Controls.Add(lbAppOrigin);
            pContainer.Controls.Add(lbOrigin);
            pContainer.Controls.Add(lbSizeOnDisk);
            pContainer.Controls.Add(btAppUninstall);
            pContainer.Controls.Add(btAppReset);
            pContainer.Controls.Add(pbAppIcon);
            pContainer.Controls.Add(lbAppCName);
            pContainer.Controls.Add(lbAppVer);
            pContainer.Controls.Add(lbAppAuthor);
            pContainer.Controls.Add(lbAppName);
            loadedWE = we;


        }

        #endregion Generate Tabs

        #region Buttons

        private void btGeneralSettings_Click(object sender, EventArgs e)
        {
            switchTab2(sender as Control, tpGeneral);
        }

        private void btLanguages_Click(object sender, EventArgs e)
        {
            switchTab2(sender as Control, tpLanguage);
        }

        private void btDownloads_Click(object sender, EventArgs e)
        {
            switchTab2(sender as Control, tpDownloads);
        }

        private void btNotifications_Click(object sender, EventArgs e)
        {
            switchTab2(sender as Control, tpNotifications);
        }

        private void btAccessSettings_Click(object sender, EventArgs e)
        {
            switchTab2(sender as Control, tpAccessibility);
        }

        private void btAppearance_Click(object sender, EventArgs e)
        {
            switchTab2(sender as Control, tpAppearance);
        }

        private void btProfiles_Click(object sender, EventArgs e)
        {
            switchTab2(sender as Control, tpProfiles);
        }

        private void btSecuritySettings_Click(object sender, EventArgs e)
        {
            switchTab2(sender as Control, tpSecurity);
        }

        private void btSites_Click(object sender, EventArgs e)
        {
            switchTab2(sender as Control, tpSites);
        }

        private void btProxies_Click(object sender, EventArgs e)
        {
            switchTab2(sender as Control, tpProxies);
        }

        private void btExtensions_Click(object sender, EventArgs e)
        {
            switchTab2(sender as Control, tpExtensions);
        }

        private void btApps_Click(object sender, EventArgs e)
        {
            switchTab2(sender as Control, tpApps);
        }

        private void btWE_Click(object sender, EventArgs e)
        {
            switchTab2(sender as Control, tpWE);
        }

        private void btThemes_Click(object sender, EventArgs e)
        {
            switchTab2(sender as Control, tpThemes);
        }

        private void btUpdates_Click(object sender, EventArgs e)
        {
            switchTab2(sender as Control, tpUpdates);
        }

        private void btAbout_Click(object sender, EventArgs e)
        {
            switchTab2(sender as Control, tpAbout);
        }

        private void btLogs_Click(object sender, EventArgs e)
        {
            switchTab2(sender as Control, tpLogs);
        }

        private void btSync_Click(object sender, EventArgs e)
        {
            switchTab2(sender as Control, tpSync);
        }

        private void btSHistory_Click(object sender, EventArgs e)
        {
            switchTab2(sender as Control, tpHistory);
        }

        private void btContainerBack_Click(object sender, EventArgs e)
        {
            switch1(btContainerBack.Tag != null ? (sbyte)btContainerBack.Tag : (sbyte)7);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch1(7);
        }

        private void btGeneral_Click(object sender, EventArgs e)
        {
            switch1(0);
        }

        private void btHistoryDownloads_Click(object sender, EventArgs e)
        {
            switch1(1);
        }

        private void btProfilesAndSync_Click(object sender, EventArgs e)
        {
            switch1(2);
        }

        private void btSecurity_Click(object sender, EventArgs e)
        {
            switch1(3);
        }

        private void btAddons_Click(object sender, EventArgs e)
        {
            switch1(5);
        }

        private void btCustomization_Click(object sender, EventArgs e)
        {
            switch1(4);
        }

        private void btUpdatesAbout_Click(object sender, EventArgs e)
        {
            switch1(6);
        }

        #endregion Buttons

        #region Theme & Lang

        #region Translate

        public string WESizeOnDisk = "Size on disk:[S]";
        public string WEDesc = "Description:";
        public string AppEnabled = "Enabled";
        public string WEEnabledInfo = "Determines if this web engine can be loaded or not.";
        public string RunInc = "Allow on Incognito mode";
        public string WERunIncInfo = "Allows this web engine to load on Incognito mode.";
        public string Uninstall = "Uninstall";
        public string Reset = "Reset";
        public string Version = "Version [V]";
        public string SizeInfoBytes = "bytes";
        public string Clear = "Clear";
        public string RemoveSelected = "Remove Selected";
        public string SearchApp = "Search an app..";
        public string SearchExt = "Search an extension..";
        public string SearchSite = "Search a site..";
        public string SearchWE = "Search a web engine..";
        public string LogInfo = "[I] information(s).[NEWLINE][W] warning(s).[NEWLINE][E] error(s).[NEWLINE][C] critical failure(s).";
        public string OpenInFiles = "Open in Files...";

        #endregion Translate

        private string appliedLang = string.Empty;

        private void ApplyLanguage(bool force = false)
        {
            if (force || appliedLang != YorotGlobal.Main.CurrentLanguage.CodeName)
            {
                appliedLang = YorotGlobal.Main.CurrentLanguage.CodeName;
                YorotLanguage l = YorotGlobal.Main.CurrentLanguage;
                Text = l.GetItemText("Win32.DefaultApps.Settings");
                lbHomeHello.Text = l.GetItemText("Win32.Settings.HelloUser").Replace("[USERNAME]", YorotGlobal.Main.Profiles.Current.Text);
                llChangeUserName.Text = l.GetItemText("Win32.Settings.Change");
                SearchApp = l.GetItemText("Win32.Settings.AppsSearch");
                SearchExt = l.GetItemText("Win32.Settings.ExtSearch");
                SearchWE = l.GetItemText("Win32.Settings.WESearch");
                SearchSite = l.GetItemText("Win32.Settings.SiteSearch");
                SizeInfoBytes = l.GetItemText("Win32.Settings.Bytes");
                RemoveSelected = l.GetItemText("Win32.Settings.RemSelected");
                LogInfo = l.GetItemText("Win32.Settings.LogsInfo");
                OpenInFiles = l.GetItemText("Win32.Settings.OpenInFiles");
                Clear = l.GetItemText("Win32.Settings.Clear");
                llChangeUserText.Text = l.GetItemText("Win32.Settings.Change");
                llChangePic.Text = l.GetItemText("Win32.Settings.Change");
                llChangePassword.Text = l.GetItemText("Win32.Settings.ChangePass");
                btHome.Text = l.GetItemText("Win32.Settings.Home");
                llHomeManage.Text = l.GetItemText("Win32.Settings.ManageAcc");
                btGeneral.Text = l.GetItemText("Win32.Settings.General");
                btGeneralSettings.Text = l.GetItemText("Win32.Settings.General");
                lbGeneral.Text = l.GetItemText("Win32.Settings.General");
                btLanguages.Text = l.GetItemText("Win32.Settings.Languages");
                lbLang.Text = l.GetItemText("Win32.Settings.Languages");
                btAccessSettings.Text = l.GetItemText("Win32.Settings.Accessibility");
                lbAccess.Text = l.GetItemText("Win32.Settings.Accessibility");
                lbSpeechSynthRate.Text = l.GetItemText("Win32.Settings.AccSynthRate");
                lbSpeechSynthVol.Text = l.GetItemText("Win32.Settings.AccSynthVol");
                btHistoryDownloads.Text = l.GetItemText("Win32.Settings.HistoryAndDown");
                btSecurity.Text = l.GetItemText("Win32.Settings.Security");
                btSecuritySettings.Text = l.GetItemText("Win32.Settings.Security");
                lbSecurity.Text = l.GetItemText("Win32.Settings.Security");
                btProfilesAndSync.Text = l.GetItemText("Win32.Settings.Profiles");
                btProfiles.Text = l.GetItemText("Win32.Settings.Profiles");
                lbProfiles.Text = l.GetItemText("Win32.Settings.Profiles");
                btUpdatesAbout.Text = l.GetItemText("Win32.Settings.UpdatesAndAbout");
                lbAppearance.Text = l.GetItemText("Win32.Settings.Appearance");
                btAppearance.Text = l.GetItemText("Win32.Settings.Appearance");
                lbShowFav.Text = l.GetItemText("Win32.Settings.ApShowFav");
                lbOpenAppDrawer.Text = l.GetItemText("Win32.Settings.ApShowApp");
                lbThemes.Text = l.GetItemText("Win32.Settings.Themes");
                btThemes.Text = l.GetItemText("Win32.Settings.Themes");
                btCustomization.Text = l.GetItemText("Win32.Settings.Customization");
                lbApps.Text = l.GetItemText("Win32.Settings.Apps");
                lbExtensions.Text = l.GetItemText("Win32.Settings.Extensions");
                btExtensions.Text = l.GetItemText("Win32.Settings.Extensions");
                lbWE.Text = l.GetItemText("Win32.Settings.WebEngines");
                btWE.Text = l.GetItemText("Win32.Settings.WebEngines");
                btAddons.Text = l.GetItemText("Win32.Settings.Addons");
                btApps.Text = l.GetItemText("Win32.Settings.Apps");
                lbDownloads.Text = l.GetItemText("Win32.Settings.Downloads");
                btDownloads.Text = l.GetItemText("Win32.Settings.Downloads");
                lbOpenAfterDown.Text = l.GetItemText("Win32.Settings.DowOpenFile");
                lbAutoDown.Text = l.GetItemText("Win32.Settings.DowAutoD");
                lbAutoDownFolder.Text = l.GetItemText("Win32.Settings.DowAutoDInfo");
                lbHomePage.Text = l.GetItemText("Win32.Settings.Homepage");
                rbNewTab.Text = l.GetItemText("Win32.Settings.NewTab");
                lbSearchEngine.Text = l.GetItemText("Win32.Settings.DefaultSE").Replace("[E]", YorotGlobal.Main.CurrentSettings.SearchEngine.Name);
                btSearchEngine.Text = l.GetItemText("Win32.Settings.SearchEngines");
                lbAtStartup.Text = l.GetItemText("Win32.Settings.AtStartup");
                rbLoadNewTab.Text = l.GetItemText("Win32.Settings.ASNewTab");
                rbLoadHome.Text = l.GetItemText("Win32.Settings.ASHome");
                rbLoadPage.Text = l.GetItemText("Win32.Settings.ASURL");
                lbRestore.Text = l.GetItemText("Win32.Settings.RestoreOld");
                lbDefaultBrowser.Text = l.GetItemText("Win32.Settings.DefaultBrowser");
                lbStartOnBoot.Text = l.GetItemText("Win32.Settings.StartYorot");
                lbStartInTray.Text = l.GetItemText("Win32.Settings.StartTray");
                lbNotifications.Text = l.GetItemText("Win32.Settings.Notifications");
                btNotifications.Text = l.GetItemText("Win32.Settings.Notifications");
                lbPlayNotifSound.Text = l.GetItemText("Win32.Settings.NotPlaySound");
                lbDefaultNotif.Text = l.GetItemText("Win32.Settings.NotUseDefault");
                lbNotifLoc.Text = l.GetItemText("Win32.Settings.NotSoundLoc");
                lbSilentMode.Text = l.GetItemText("Win32.Settings.NotSilentMode");
                lbSilentInfo.Text = l.GetItemText("Win32.Settings.NotSilentInfo");
                btOpenCalendar.Text = l.GetItemText("Win32.Settings.NotCalendar");
                btProxies.Text = l.GetItemText("Win32.Settings.Proxies");
                lbProxies.Text = l.GetItemText("Win32.Settings.Proxies");
                lbRememberLastProxy.Text = l.GetItemText("Win32.Settings.ProxyRemember");
                lbDoNotTrack.Text = l.GetItemText("Win32.Settings.DoNotTrack");
                lbDoNotTrackInfo.Text = l.GetItemText("Win32.Settings.DoNotTrackInfo");
                btSites.Text = l.GetItemText("Win32.Settings.Sites");
                lbSites.Text = l.GetItemText("Win32.Settings.Sites");
                btSync.Text = l.GetItemText("Win32.Settings.Sync");
                lbSync.Text = l.GetItemText("Win32.Settings.Sync");
                lbUpdate.Text = l.GetItemText("Win32.Settings.Updates");
                btUpdates.Text = l.GetItemText("Win32.Settings.Updates");
                lbUpdateHistory.Text = l.GetItemText("Win32.Settings.UpdateHistory");
                lbCurrentLang.Text = l.GetItemText("Win32.Settings.CurrentLang");
                lbDateFormat.Text = l.GetItemText("Win32.Settings.DateFormat");
                lbLocale.Text = l.GetItemText("Win32.Settings.Locale");
                btHistory.Text = l.GetItemText("Win32.Settings.History");
                lbHistory.Text = l.GetItemText("Win32.Settings.History");
                btLogs.Text = l.GetItemText("Win32.Settings.Logs");
                lbLogs.Text = l.GetItemText("Win32.Settings.Logs");
                btAbout.Text = l.GetItemText("Win32.Settings.About");
                lbAboutTitle.Text = l.GetItemText("Win32.Settings.About");
                lbYorotInfo.Text = l.GetItemText("Win32.Settings.AboutInfo").Replace("[VERSIONINFO]", YorotGlobal.Version + " [" + YorotGlobal.CodeName + "-" + YorotGlobal.VersionNo + "] - " + YorotGlobal.Main.YorotBranch);
                llEULA.Text = l.GetItemText("Win32.Settings.AboutEULA");
                llEULA.Text = l.GetItemText("Win32.Settings.AboutEULA");
                llLicenses.Text = l.GetItemText("Win32.Settings.AboutLicenses");
                btHistoryClear.Text = HistorySelected != null ? RemoveSelected : Clear;
                btDownloadsClear.Text = DownloadsSelected != null ? RemoveSelected : Clear;
                btLogsClear.Text = LogsSelected != null ? RemoveSelected : Clear;
                for (int i = 0; i < logOIFLile.Count;i++)
                {
                    logOIFLile[i].Text = OpenInFiles;
                }
                for(int i =0; i < logInfoLabels.Count;i++)
                {
                    var logInfo = logInfoLabels[i].Tag as int[];
                    logInfoLabels[i].Text = LogInfo.Replace("[W]", "" + logInfo[0]).Replace("[E]", "" + logInfo[1]).Replace("[C]", "" + logInfo[2]).Replace("[I]", "" + logInfo[3]).Replace("[NEWLINE]", Environment.NewLine);
                }
                if (loadedAppList)
                {
                    yopad_app.Text = l.GetItemText("Win32.DefaultApps.Yopad");
                    set_app.Text = l.GetItemText("Win32.DefaultApps.Settings");
                    calc_app.Text = l.GetItemText("Win32.DefaultApps.Calculator");
                    cal_app.Text = l.GetItemText("Win32.DefaultApps.Calendar");
                    col_app.Text = l.GetItemText("Win32.DefaultApps.ColMan");
                    con_app.Text = l.GetItemText("Win32.DefaultApps.Console");
                    file_app.Text = l.GetItemText("Win32.DefaultApps.FileMan");
                    note_app.Text = l.GetItemText("Win32.DefaultApps.Notepad");
                    sp_app.Text = l.GetItemText("Win32.DefaultApps.SpacePass");
                    st_app.Text = l.GetItemText("Win32.DefaultApps.Store");
                    yorot_app.Text = YorotGlobal.Main.Name;
                }
                if (loadedWE != null)
                {
                    lbWEVer.Text = Version.Replace("[V]", "" + loadedWE.Version);
                    lbWEReset.Text = Reset;
                    lbWEUninst.Text = Uninstall;
                    lbWESize.Text = WESizeOnDisk.Replace("[S]", loadedWE.GetWESizeInfo(SizeInfoBytes));
                    lbWEDesc.Text = WEDesc;
                    lbWEEnabled.Text = AppEnabled;
                    lbWEEnabledInfo.Text = WEEnabledInfo;
                    lbWERunInc.Text = RunInc;
                    lbWERunIncInfo.Text = WERunIncInfo;

                }
            }
        }

        private string appliedTheme = string.Empty;

        private void ApplyTheme(bool force = false)
        {
            YorotTheme theme = YorotGlobal.Main.CurrentTheme;
            string themeid = theme.BackColor.ToHex() + "-" + theme.ForeColor.ToHex() + "-" + theme.OverlayColor.ToHex() + "-" + theme.ArtColor.ToHex();
            if (force || appliedTheme != themeid)
            {
                appliedTheme = themeid;
                bool isDark = !HTAlt.Tools.IsBright(theme.BackColor);
                BackColor = theme.BackColor;
                ForeColor = theme.ForeColor;
                panel1.BackColor = theme.BackColor2;
                panel1.ForeColor = theme.ForeColor;
                tp1Main.BackColor = theme.BackColor;
                tp1Main.ForeColor = theme.ForeColor;
                tp1Settings.BackColor = theme.BackColor;
                tp1Settings.ForeColor = theme.ForeColor;
                tp1Container.BackColor = theme.BackColor;
                tp1Container.ForeColor = theme.ForeColor;
                btContainerBack.Image = theme.BackColor.IsBright() ? Properties.Resources.back_b : Properties.Resources.back_w;
                pContainer.BackColor = theme.BackColor2;
                pContainer.ForeColor = theme.ForeColor;
                flpSidebar.BackColor = theme.BackColor2;
                flpSidebar.ForeColor = theme.ForeColor;
                for (int i = 0; i < tabControl2.TabPages.Count; i++)
                {
                    TabPage tp = tabControl2.TabPages[i];
                    tp.BackColor = theme.BackColor;
                    tp.ForeColor = theme.ForeColor;
                    for (int ı = 0; ı < tp.Controls.Count; ı++)
                    {
                        Control c = tp.Controls[ı];
                        switch (c)
                        {
                            case HTSwitch _:
                                HTSwitch s = c as HTSwitch;
                                s.BackColor = theme.BackColor2;
                                s.ForeColor = theme.ForeColor;
                                s.OverlayColor = theme.OverlayColor;
                                s.ButtonColor = theme.BackColor2;
                                s.ButtonHoverColor = theme.BackColor3;
                                s.ButtonPressedColor = theme.BackColor4;
                                break;

                            case HTListView _:
                                HTListView l = c as HTListView;
                                l.HeaderBackColor = theme.BackColor3;
                                l.OverlayColor = theme.OverlayColor;
                                l.BackColor = theme.BackColor2;
                                l.ForeColor = theme.ForeColor;
                                break;

                            case HTSlider _:
                                HTSlider sl = c as HTSlider;
                                sl.BackColor = theme.BackColor2;
                                sl.ForeColor = theme.ForeColor;
                                sl.OverlayColor = theme.OverlayColor;
                                break;

                            case HTButton _:
                                HTButton b = c as HTButton;
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
                                LinkLabel ll = c as LinkLabel;
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
                                TextBox t = c as TextBox;
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
                for (int ı = 0; ı < pContainer.Controls.Count; ı++)
                {
                    Control c = pContainer.Controls[ı];
                    switch (c)
                    {
                        case HTSwitch _:
                            HTSwitch s = c as HTSwitch;
                            s.BackColor = theme.BackColor2;
                            s.ForeColor = theme.ForeColor;
                            s.OverlayColor = theme.OverlayColor;
                            s.ButtonColor = theme.BackColor2;
                            s.ButtonHoverColor = theme.BackColor3;
                            s.ButtonPressedColor = theme.BackColor4;
                            break;

                        case HTListView _:
                            HTListView l = c as HTListView;
                            l.HeaderBackColor = theme.BackColor3;
                            l.OverlayColor = theme.OverlayColor;
                            l.BackColor = theme.BackColor2;
                            l.ForeColor = theme.ForeColor;
                            break;

                        case HTSlider _:
                            HTSlider sl = c as HTSlider;
                            sl.BackColor = theme.BackColor2;
                            sl.ForeColor = theme.ForeColor;
                            sl.OverlayColor = theme.OverlayColor;
                            break;

                        case HTButton _:
                            HTButton b = c as HTButton;
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
                            LinkLabel ll = c as LinkLabel;
                            ll.BackColor = theme.BackColor;
                            ll.ForeColor = theme.ForeColor;
                            ll.ActiveLinkColor = theme.OverlayColor;
                            ll.DisabledLinkColor = theme.OverlayColor;
                            ll.LinkColor = theme.OverlayColor;
                            ll.VisitedLinkColor = theme.OverlayColor;
                            break;

                        case PictureBox _:
                        case Label _:
                            c.BackColor = theme.BackColor2;
                            c.ForeColor = theme.ForeColor;
                            break;

                        case TextBox _:
                            TextBox t = c as TextBox;
                            t.BackColor = t.ReadOnly && t.BorderStyle == BorderStyle.None ? theme.BackColor2 : theme.BackColor3;
                            t.ForeColor = theme.ForeColor;
                            break;

                        default:
                            c.BackColor = theme.BackColor2;
                            c.ForeColor = theme.ForeColor;
                            break;
                    }
                }
                for (int i = 0; i < pLogs.Controls.Count; i++)
                {
                    Control c = pLogs.Controls[i];
                    c.BackColor = theme.BackColor3;
                    c.ForeColor = theme.ForeColor;
                }
                for (int i = 0; i < LogsSelected.Count; i++)
                {
                    Control c = LogsSelected[i];
                    c.BackColor = theme.OverlayColor;
                    c.ForeColor = theme.ForeColor;
                }
                btHome.Image = isDark ? Properties.Resources.back_w : Properties.Resources.back_b;
                btContainerBack.Image = isDark ? Properties.Resources.back_w : Properties.Resources.back_b;
            }
        }

        #endregion Theme & Lang



        #region Apps

        private ListViewItem calc_app;
        private ListViewItem cal_app;
        private ListViewItem col_app;
        private ListViewItem con_app;
        private ListViewItem file_app;
        private ListViewItem note_app;
        private ListViewItem set_app;
        private ListViewItem sp_app;
        private ListViewItem yorot_app;
        private ListViewItem st_app;
        private ListViewItem yopad_app;
        private List<YorotApp> loadedApps;
        private bool loadedAppList = false;

        #endregion Apps

        #region List Refresh

        private void RefreshAppList(bool force = false)
        {
            if (force || loadedApps != YorotGlobal.Main.AppMan.Apps)
            {
                resetSearches();
                loadedApps = YorotGlobal.Main.AppMan.Apps;
                lvApps.Items.Clear();
                for (int i = 0; i < loadedApps.Count; i++)
                {
                    YorotApp app = loadedApps[i];
                    ListViewItem item = new ListViewItem()
                    {
                        Text = app.AppName,
                        ToolTipText = app.AppCodeName,
                        Tag = app,
                    };
                    ilAppMan.Images.Add(YorotTools.GetAppIcon(app));
                    item.ImageIndex = ilAppMan.Images.Count - 1;
                    switch (app.AppCodeName.ToLowerEnglish())
                    {
                        case "com.haltroy.yorot":
                            yorot_app = item; break;
                        case "com.haltroy.spacepass":
                            sp_app = item; lvApps.Items.Add(item); break;
                        case "com.haltroy.settings":
                            set_app = item; lvApps.Items.Add(item); break;
                        case "com.haltroy.store":
                            st_app = item; lvApps.Items.Add(item); break;
                        case "com.haltroy.console":
                            con_app = item; lvApps.Items.Add(item); break;
                        case "com.haltroy.calc":
                            calc_app = item; lvApps.Items.Add(item); break;
                        case "com.haltroy.calendar":
                            cal_app = item; lvApps.Items.Add(item); break;
                        case "com.haltroy.notepad":
                            note_app = item; lvApps.Items.Add(item); break;
                        case "com.haltroy.colman":
                            col_app = item; lvApps.Items.Add(item); break;
                        case "com.haltroy.fileman":
                            file_app = item; lvApps.Items.Add(item); break;
                        case "com.haltroy.yopad":
                            yopad_app = item; lvApps.Items.Add(item); break;
                        default:
                            lvApps.Items.Add(item);
                            break;
                    }
                }
                loadedAppList = true;
            }
        }

        #endregion List Refresh

        #region ListViews

        private void lvApps_DoubleClick(object sender, EventArgs e)
        {
            if (lvApps.SelectedItems.Count > 0)
            {
                GenerateAppTab(lvApps.SelectedItems[0]);
                switch1(8);
            }
        }

        private void lvWE_DoubleClick(object sender, EventArgs e)
        {
            if (lvWE.SelectedItems.Count > 0)
            {
                GenerateWETab(lvWE.SelectedItems[0].Tag as YorotWebEngine);
                switch1(8);
            }
        }

        private void lvSites_DoubleClick(object sender, EventArgs e)
        {
            if (lvSites.SelectedItems.Count > 0)
            {
                GenerateSiteTab(lvSites.SelectedItems[0].Tag as YorotSite);
                switch1(8);
            }
        }

        private void lvExt_DoubleClick(object sender, EventArgs e)
        {
            if (lvExt.SelectedItems.Count > 0)
            {
                GenerateExtTab(lvExt.SelectedItems[0].Tag as YorotExtension);
                switch1(8);
            }
        }

        #endregion ListViews

        #region Searches
        private void resetSearches()
        {
            tbSearchApp.Text = SearchApp;
            SearchingApps = false;
            tbSearchExt.Text = SearchExt;
            SearchingExt = false;
            tbSearchWE.Text = SearchWE;
            SearchingWE = false;
            tbSearchSite.Text = SearchSite;
            SearchingSite = false;
        }
        private void tbSearchApp_Click(object sender, EventArgs e)
        {
            if (!tbSearchApp.Focused || tbSearchApp.Text == SearchApp) { tbSearchApp.SelectAll(); }
        }

        private bool SearchingApps = false;
        private readonly List<ListViewItem> appsList = new List<ListViewItem>();
        private void tbSearchApp_TextChanged(object sender, EventArgs e)
        {
            if (tbSearchApp.Text == SearchApp || string.IsNullOrWhiteSpace(tbSearchApp.Text))
            {
                if (SearchingApps)
                {
                    lvApps.Clear();
                    for (int i = 0; i < appsList.Count; i++)
                    {
                        appsList[i].Selected = false;
                        lvApps.Items.Add(appsList[i]);
                    }
                }
                SearchingApps = false;
                tbSearchApp.Text = SearchApp;
                tbSearchApp.SelectAll();
            }
            else
            {
                if (!SearchingApps)
                {
                    appsList.Clear();
                    System.Collections.IList list1 = lvApps.Items;
                    for (int i = 0; i < list1.Count; i++)
                    {
                        appsList.Add((ListViewItem)list1[i]);
                    }
                }
                lvApps.Clear();
                SearchingApps = true;
                for (int i = 0; i < appsList.Count; i++)
                {
                    ListViewItem item = appsList[i];
                    if (item.Text.Contains(tbSearchApp.Text))
                    {
                        lvApps.Items.Add(item);
                    }
                }
                if (lvApps.Items.Count > 0)
                {
                    lvApps.Items[0].Selected = true;
                }
            }
        }

        private void tbSearchExt_Click(object sender, EventArgs e)
        {
            if (!tbSearchExt.Focused || tbSearchExt.Text == SearchExt) { tbSearchExt.SelectAll(); }
        }

        private bool SearchingExt = false;
        private readonly List<ListViewItem> extList = new List<ListViewItem>();
        private void tbSearchExt_TextChanged(object sender, EventArgs e)
        {
            if (tbSearchExt.Text == SearchExt || string.IsNullOrWhiteSpace(tbSearchExt.Text))
            {
                if (SearchingExt)
                {
                    lvExt.Clear();
                    for (int i = 0; i < extList.Count; i++)
                    {
                        extList[i].Selected = false;
                        lvExt.Items.Add(extList[i]);
                    }
                }
                SearchingExt = false;
                tbSearchExt.Text = SearchExt;
                tbSearchExt.SelectAll();
            }
            else
            {
                if (!SearchingExt)
                {
                    extList.Clear();
                    System.Collections.IList list1 = lvExt.Items;
                    for (int i = 0; i < list1.Count; i++)
                    {
                        extList.Add((ListViewItem)list1[i]);
                    }
                }
                lvExt.Clear();
                SearchingExt = true;
                for (int i = 0; i < extList.Count; i++)
                {
                    ListViewItem item = extList[i];
                    if (item.Text.Contains(tbSearchExt.Text))
                    {
                        lvExt.Items.Add(item);
                    }
                }
                if (lvExt.Items.Count > 0)
                {
                    lvExt.Items[0].Selected = true;
                }
            }
        }

        private void tbSearchSite_Click(object sender, EventArgs e)
        {
            if (!tbSearchSite.Focused || tbSearchSite.Text == SearchSite) { tbSearchSite.SelectAll(); }
        }

        private bool SearchingSite = false;
        private readonly List<ListViewItem> siteList = new List<ListViewItem>();
        private void tbSearchSite_TextChanged(object sender, EventArgs e)
        {
            if (tbSearchSite.Text == SearchSite || string.IsNullOrWhiteSpace(tbSearchSite.Text))
            {
                if (SearchingSite)
                {
                    lvSites.Clear();
                    for (int i = 0; i < siteList.Count; i++)
                    {
                        siteList[i].Selected = false;
                        lvSites.Items.Add(siteList[i]);
                    }
                }
                SearchingSite = false;
                tbSearchSite.Text = SearchSite;
                tbSearchSite.SelectAll();
            }
            else
            {
                if (!SearchingSite)
                {
                    siteList.Clear();
                    System.Collections.IList list1 = lvSites.Items;
                    for (int i = 0; i < list1.Count; i++)
                    {
                        siteList.Add((ListViewItem)list1[i]);
                    }
                }
                lvSites.Clear();
                SearchingSite = true;
                for (int i = 0; i < siteList.Count; i++)
                {
                    ListViewItem item = siteList[i];
                    if (item.Text.Contains(tbSearchSite.Text))
                    {
                        lvSites.Items.Add(item);
                    }
                }
                if (lvSites.Items.Count > 0)
                {
                    lvSites.Items[0].Selected = true;
                }
            }
        }

        private void tbSearchWE_Click(object sender, EventArgs e)
        {
            if (!tbSearchWE.Focused || tbSearchWE.Text == SearchWE) { tbSearchWE.SelectAll(); }
        }

        private bool SearchingWE = false;
        private readonly List<ListViewItem> weList = new List<ListViewItem>();
        private void tbSearchWE_TextChanged(object sender, EventArgs e)
        {
            if (tbSearchWE.Text == SearchWE || string.IsNullOrWhiteSpace(tbSearchWE.Text))
            {
                if (SearchingWE)
                {
                    lvWE.Clear();
                    for (int i = 0; i < weList.Count; i++)
                    {
                        weList[i].Selected = false;
                        lvWE.Items.Add(weList[i]);
                    }
                }
                SearchingWE = false;
                tbSearchWE.Text = SearchWE;
                tbSearchWE.SelectAll();
            }
            else
            {
                if (!SearchingWE)
                {
                    weList.Clear();
                    System.Collections.IList list1 = lvWE.Items;
                    for (int i = 0; i < list1.Count; i++)
                    {
                        weList.Add((ListViewItem)list1[i]);
                    }
                }
                lvWE.Clear();
                SearchingWE = true;
                for (int i = 0; i < weList.Count; i++)
                {
                    ListViewItem item = weList[i];
                    if (item.Text.Contains(tbSearchWE.Text))
                    {
                        lvWE.Items.Add(item);
                    }
                }
                if (lvWE.Items.Count > 0)
                {
                    lvWE.Items[0].Selected = true;
                }
            }
        }

        #endregion Searches

        #region History, Downloads and Logs
        public bool CompareLists(List<string> list1, List<string> list2)
        {
            if (list1.Count != list2.Count)
            {
                return false;
            }

            int sameCount = 0;

            for (int i = 0; i < list1.Count; i++)
            {
                if (list1[i] == list2[i])
                {
                    sameCount++;
                }
            }

            return sameCount == 0;
        }

        private List<Panel> HistorySelected = new List<Panel>();
        private List<Panel> DownloadsSelected = new List<Panel>();

        #region Logs
        private List<HTButton> logOIFLile = new List<HTButton>();
        private List<TextBox> logInfoLabels = new List<TextBox>();
        private List<Panel> LogsSelected = new List<Panel>();
        private List<string> syncedLogs = new List<string>();
        private void genLogEntries(bool force = false)
        {
            var list = new List<string>();
            list.AddRange(System.IO.Directory.GetFiles(YorotGlobal.Main.LogFolder));
            if (force || CompareLists(syncedLogs, list))
            {
                syncedLogs = list;
                pLogs.Controls.Clear();
                logOIFLile.Clear();
                logInfoLabels.Clear();
                pLogs.SuspendLayout();
                for(int i = 0; i < list.Count;i++)
                {
                    genLogEntry(list[i], System.IO.Path.GetFileNameWithoutExtension(list[i]), getLogInfo(list[i]));
                }
                pLogs.ResumeLayout(true);
                updateLogClearButton();
            }
        }
        private int[] getLogInfo(string logLoc)
        {
            string logFile = HTAlt.Tools.ReadFile(logLoc, System.Text.Encoding.Unicode);
            string[] lines = logFile.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            int w = 0;
            int e = 0;
            int c = 0;
            int ı = 0;
            for(int i = 0; i < lines.Length;i++)
            {
                if (lines[i].StartsWith(" [W]"))
                {
                    w++;
                }else if (lines[i].StartsWith(" [E]"))
                {
                    e++;
                }
                else if (lines[i].StartsWith(" [C]"))
                {
                    c++;
                }
                else if (lines[i].StartsWith(" [I]"))
                {
                    ı++;
                }
            }
            return new int[] {w,e,c,ı};
        }
        private void updateLogClearButton()
        {
            btLogsClear.Text = LogsSelected.Count >0 ? RemoveSelected : Clear;
        }
        private void genLogEntry(string logLoc, string logName, int[] logInfo)
        {
            System.Windows.Forms.Panel pLogEx = new System.Windows.Forms.Panel();
            HTAlt.WinForms.HTButton btLogOpenEx = new HTAlt.WinForms.HTButton();
            System.Windows.Forms.TextBox lbLogDescEx = new System.Windows.Forms.TextBox();
            System.Windows.Forms.Label lbLogCEx = new System.Windows.Forms.Label();
            System.Windows.Forms.TextBox lbLogTitleEx = new System.Windows.Forms.TextBox();
            pLogEx.SuspendLayout();
            // 
            // pLogEx
            // 
            pLogEx.Controls.Add(btLogOpenEx);
            pLogEx.Controls.Add(lbLogDescEx);
            pLogEx.Controls.Add(lbLogCEx);
            pLogEx.Controls.Add(lbLogTitleEx);
            pLogEx.Dock = System.Windows.Forms.DockStyle.Top;
            pLogEx.BackColor = YorotGlobal.Main.CurrentTheme.BackColor3;
            pLogEx.ForeColor = YorotGlobal.Main.CurrentTheme.ForeColor;
            pLogEx.Name = "pLogEx";
            pLogEx.Size = new System.Drawing.Size(pLogs.Width, 170);
            pLogEx.Tag = logLoc;
            pLogEx.BorderStyle = BorderStyle.FixedSingle;
            pLogEx.Click += new EventHandler((sender,e) => 
            { 
                if (LogsSelected.Contains(pLogEx))
                {
                    LogsSelected.Remove(pLogEx);
                    pLogEx.BackColor = YorotGlobal.Main.CurrentTheme.BackColor3;
                    pLogEx.ForeColor = YorotGlobal.Main.CurrentTheme.ForeColor;
                    lbLogTitleEx.BackColor = YorotGlobal.Main.CurrentTheme.BackColor3;
                    lbLogTitleEx.ForeColor = YorotGlobal.Main.CurrentTheme.ForeColor;
                    lbLogDescEx.BackColor = YorotGlobal.Main.CurrentTheme.BackColor3;
                    lbLogDescEx.ForeColor = YorotGlobal.Main.CurrentTheme.ForeColor;
                }else
                {
                    LogsSelected.Add(pLogEx);
                    pLogEx.BackColor = YorotGlobal.Main.CurrentTheme.OverlayColor;
                    pLogEx.ForeColor = YorotGlobal.Main.CurrentTheme.ForeColor;
                    lbLogTitleEx.BackColor = YorotGlobal.Main.CurrentTheme.OverlayColor;
                    lbLogTitleEx.ForeColor = YorotGlobal.Main.CurrentTheme.ForeColor;
                    lbLogDescEx.BackColor = YorotGlobal.Main.CurrentTheme.OverlayColor;
                    lbLogDescEx.ForeColor = YorotGlobal.Main.CurrentTheme.ForeColor;
                }
                updateLogClearButton();
            });
            // 
            // lbLogCEx
            // 
            lbLogCEx.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            lbLogCEx.AutoSize = true;
            lbLogCEx.Font = new System.Drawing.Font("Ubuntu", 12F);
            lbLogCEx.Location = new System.Drawing.Point(pLogs.Width - 20,5);
            lbLogCEx.Name = "lbLogCEx";
            lbLogCEx.Text = "X";
            lbLogCEx.Click += new EventHandler((sender, e) => 
            {
                if (System.IO.File.Exists(logLoc))
                {
                    System.IO.File.Delete(logLoc);
                }
                if (pLogs.InvokeRequired)
                {
                    pLogs.Invoke(new Action(() => pLogs.Controls.Remove(pLogEx)));
                }else
                {
                    pLogs.Controls.Remove(pLogEx);
                }
                if (LogsSelected.Contains(pLogEx))
                {
                    LogsSelected.Remove(pLogEx);
                }
            });
            // 
            // lbLogTitleEx
            // 
            lbLogTitleEx.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right);
            lbLogTitleEx.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lbLogTitleEx.BackColor = YorotGlobal.Main.CurrentTheme.BackColor3;
            lbLogTitleEx.ForeColor = YorotGlobal.Main.CurrentTheme.ForeColor;
            lbLogTitleEx.Font = new System.Drawing.Font("Ubuntu", 12F);
            lbLogTitleEx.Location = new System.Drawing.Point(5, lbLogCEx.Location.Y + lbLogCEx.Height + 5);
            lbLogTitleEx.Name = "lbLogTitleEx";
            lbLogTitleEx.ReadOnly = true;
            lbLogTitleEx.Size = new System.Drawing.Size(pLogs.Width - 10, lbLogTitleEx.Height);
            lbLogTitleEx.Text = logName;
            // 
            // lbLogDescEx
            // 
            lbLogDescEx.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right);
            lbLogDescEx.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lbLogDescEx.BackColor = YorotGlobal.Main.CurrentTheme.BackColor3;
            lbLogDescEx.ForeColor = YorotGlobal.Main.CurrentTheme.ForeColor;
            lbLogDescEx.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbLogDescEx.Location = new System.Drawing.Point(5, lbLogTitleEx.Location.Y + lbLogTitleEx.Height + 5);
            lbLogDescEx.Multiline = true;
            lbLogDescEx.Name = "lbLogDescEx";
            lbLogDescEx.ReadOnly = true;
            lbLogDescEx.Size = new System.Drawing.Size(lbLogTitleEx.Width, 65);
            lbLogDescEx.Text = LogInfo.Replace("[W]", "" + logInfo[0]).Replace("[E]", "" + logInfo[1]).Replace("[C]", "" + logInfo[2]).Replace("[I]", "" + logInfo[3]).Replace("[NEWLINE]",Environment.NewLine);
            logInfoLabels.Add(lbLogDescEx);
            // 
            // btLogOpenEx
            // 
            btLogOpenEx.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right);
            btLogOpenEx.AutoColor = true;
            btLogOpenEx.DrawImage = false;
            btLogOpenEx.Font = new System.Drawing.Font("Ubuntu", 10F);
            btLogOpenEx.Location = new System.Drawing.Point(5, lbLogDescEx.Location.Y+ lbLogDescEx.Height + 5);
            btLogOpenEx.Name = "btLogOpenEx";
            btLogOpenEx.Size = new System.Drawing.Size(lbLogDescEx.Width, 23);
            btLogOpenEx.Tag = logLoc;
            btLogOpenEx.Text = OpenInFiles;
            logOIFLile.Add(btLogOpenEx);
            
            pLogs.Controls.Add(pLogEx);
            pLogEx.ResumeLayout(false);
            pLogEx.PerformLayout();

        }
        private void btLogsClear_Click(object sender, EventArgs e)
        {
            if (LogsSelected.Count > 0)
            {
                for (int i = 0; i < LogsSelected.Count; i++)
                {
                    string logLoc = LogsSelected[i].Tag as string;
                    syncedLogs.Remove(logLoc);
                    System.IO.File.Delete(logLoc);
                    if (pLogs.InvokeRequired)
                    {
                        pLogs.Invoke(new Action(() => pLogs.Controls.Remove(LogsSelected[i])));
                    }
                    else
                    {
                        pLogs.Controls.Remove(LogsSelected[i]);
                    }
                }
            }
            else
            {
                pLogs.Controls.Clear();
                for (int i = 0; i < syncedLogs.Count; i++)
                {
                    try
                    {
                        System.IO.File.Delete(syncedLogs[i]);
                    }
                    catch (Exception) { } //ignored
                }
                syncedLogs.Clear();
            }
        }

        #endregion Logs
        #endregion


    }
}