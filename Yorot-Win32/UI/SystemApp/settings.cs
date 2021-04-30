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
        public settings(string[] args = null)
        {
            InitializeComponent();
            Icon = HTAlt.Tools.IconFromImage(Properties.Resources.Settings);
            // Load User Image
            if (System.IO.File.Exists(YorotGlobal.Main.Profiles.Current.Path + "picture.png"))
            {
                var img = YorotGlobal.Main.Profiles.Current.Picture;
                pbHomeUser.Image = YorotTools.ClipToCircle(img, new PointF(img.Width / 2, img.Height / 2), img.Width / 2);
            }
            else
            {
                pbHomeUser.Image = Properties.Resources.default_pofile_pic;
            }
            ApplyLanguage(true);
            ApplyTheme(true);
            RefreshAppList(true);
            if (args != null)
            {
                if (args.Length > 0)
                {
                    var s = args[0];
                    var s1 = s.Substring(0, s.IndexOf(':'));
                    var s2 = s.Substring(s1.Length + 1);
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
                                    var appItem = FindLVI(lvApps, s2);
                                    GenerateAppTap(appItem.Tag as YorotApp);
                                    switch1((sbyte)8);
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
                                    var extItem = FindLVI(lvExt, s2);
                                    GenerateExtTap(extItem.Tag as YorotExtension);
                                    switch1((sbyte)8);
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
                                    var weItem = FindLVI(lvWE, s2);
                                    GenerateWETap(weItem.Tag as YorotWebEngine);
                                    switch1((sbyte)8);
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
                                    var siteItem = FindLVI(lvSites, s2);
                                    GenerateSiteTap(siteItem.Tag as YorotSite);
                                    switch1((sbyte)8);
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

        private ListViewItem FindLVI(ListView lv, string codeName)
        {
            ListViewItem item = null;
            for(int i = 0; i < lv.Items.Count;i++)
            {
                if (lv.Items[i].ToolTipText == codeName)
                {
                    item = lv.Items[i];
                }
            }
            return item;
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
                    btApps_Click(btApps, new EventArgs());
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

        void GenerateAppTap(YorotApp app)
        {
            string randomAppID = HTAlt.Tools.GenerateRandomText(17);
            // TODO: Add all language specific ones to lists.
            System.Windows.Forms.PictureBox pbAppIcon = new System.Windows.Forms.PictureBox();
            System.Windows.Forms.PictureBox pbASBack = new System.Windows.Forms.PictureBox();
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
            lbContainerTitle.Text = app.AppName;
            // 
            // pbAppIcon
            // 
            pbAppIcon.Location = new System.Drawing.Point(18,18);
            pbAppIcon.Name = "pbAppIcon_" + randomAppID;
            pbAppIcon.Size = new System.Drawing.Size(64, 64);
            pbAppIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pbAppIcon.Image = YorotTools.GetAppIcon(app);
            pbAppIcon.TabStop = false;
            // 
            // lbAppName
            // 
            lbAppName.AutoSize = true;
            lbAppName.Font = new System.Drawing.Font("Ubuntu", 15F);
            lbAppName.Location = new System.Drawing.Point(95, 18);
            lbAppName.Name = "lbAppName_" + randomAppID;
            lbAppName.Text = app.AppName;
            // 
            // lbAppCName
            // 
            lbAppCName.AutoSize = true;
            lbAppCName.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAppCName.Location = new System.Drawing.Point(95,lbAppName.Location.Y  + lbAppName.Height + 2);
            lbAppCName.Name = "lbAppCName_" + randomAppID;
            lbAppCName.Text = app.AppCodeName;
            // 
            // lbAppVer
            // 
            lbAppVer.AutoSize = true;
            lbAppVer.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAppVer.Location = new System.Drawing.Point(lbAppName.Location.X + lbAppName.Width, 13);
            lbAppVer.Name = "lbAppVer_" + randomAppID;
            lbAppVer.Text = "Version " + app.Version + " [" + app.VersionNo + "]";
            // 
            // lbAppAuthor
            // 
            lbAppAuthor.AutoSize = true;
            lbAppAuthor.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAppAuthor.Location = new System.Drawing.Point(95, lbAppCName.Location.Y + lbAppCName.Height +2);
            lbAppAuthor.Name = "lbAppAuthor_" + randomAppID;
            lbAppAuthor.Text = app.Author;
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
            btAppReset.Location = new System.Drawing.Point(18, pbAppIcon.Location.Y + pbAppIcon.Height + 5);
            btAppReset.Name = "btAppReset_" + randomAppID;
            btAppReset.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            btAppReset.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            btAppReset.Size = new System.Drawing.Size(pContainer.Width - 36, 23);
            btAppReset.Text = app.isSystemApp ? "Cannot reset system apps" : "Reset";
            btAppReset.Enabled = !app.isSystemApp;
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
            btAppUninstall.Location = new System.Drawing.Point(18, btAppReset.Location.Y + btAppReset.Height + 5);
            btAppUninstall.Name = "btAppUninstall_" + randomAppID;
            btAppUninstall.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
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
            lbAppOrigin.Size = new System.Drawing.Size(pContainer.Width - (36 + lbOrigin.Width),lbOrigin.Height * 4);
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
            //hsNotifications.Checked = app.
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
            lbNotificationsInfo.Location = new System.Drawing.Point(lbNotifications.Location.X,lbNotifications.Location.Y + lbNotifications.Height +5);
            lbNotificationsInfo.Name = "lbNotificationsInfo_" + randomAppID;
            lbNotificationsInfo.AutoSize = true;
            lbNotificationsInfo.Text = "Allows this app to show notifications";
            // 
            // hsPrioritize
            // 
            hsPrioritize.Location = new System.Drawing.Point(lbNotificationsInfo.Location.X, lbNotificationsInfo.Location.Y + lbNotificationsInfo.Height + 20);
            hsPrioritize.Name = "hsPrioritize_" + randomAppID;
            hsPrioritize.Size = new System.Drawing.Size(50,19);
            // 
            // lbPrioritize
            // 
            lbPrioritize.AutoSize = true;
            lbPrioritize.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbPrioritize.Location = new System.Drawing.Point(hsPrioritize.Location.X + hsPrioritize.Width,hsPrioritize.Location.Y - 2);
            lbPrioritize.Name = "lbPrioritize_" + randomAppID;
            lbPrioritize.Text = "Prioritize";
            // 
            // lbPrioritieInfo
            // 
            lbPrioritieInfo.AutoSize = true;
            lbPrioritieInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbPrioritieInfo.Location = new System.Drawing.Point(lbPrioritize.Location.X,lbPrioritize.Location.Y + lbPrioritize.Height + 5);
            lbPrioritieInfo.Name = "lbPrioritieInfo_" + randomAppID;
            lbPrioritieInfo.Text = "Prioritizes this app\'s notification from other notifications.";
            // 
            // hsNotifListener
            // 
            hsNotifListener.Location = new System.Drawing.Point(hsPrioritize.Location.X ,lbPrioritieInfo.Location.Y + lbPrioritieInfo.Height + 20);
            hsNotifListener.Name = "hsNotifListener_" + randomAppID;
            hsNotifListener.Size = new System.Drawing.Size(50, 19);
            // 
            // lbNotifListener
            // 
            lbNotifListener.AutoSize = true;
            lbNotifListener.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbNotifListener.Location = new System.Drawing.Point(hsNotifListener.Location.X + hsNotifListener.Width,hsNotifListener.Location.Y - 2);
            lbNotifListener.Name = "lbNotifListener_" + randomAppID;
            lbNotifListener.Text = "Run notification listener at background";
            // 
            // lbNotifListenerInfo
            // 
            lbNotifListenerInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbNotifListenerInfo.Location = new System.Drawing.Point(lbNotifListener.Location.X,lbNotifListener.Location.Y + lbNotifListener.Height + 5);
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
            lbRunOnStartupInfo.Location = new System.Drawing.Point(lbRunOnStartup.Location.X,lbRunOnStartup.Location.Y + lbRunOnStartup.Height + 5);
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
            pContainer.Controls.Add(pbASBack);
            pContainer.Controls.Add(lbAppCName);
            pContainer.Controls.Add(lbAppVer);
            pContainer.Controls.Add(lbAppAuthor);
            pContainer.Controls.Add(lbAppName);

        }
        void GenerateExtTap(YorotExtension ext)
        {
            string randomAppID = HTAlt.Tools.GenerateRandomText(17);
            // TODO: Add all language specific ones to lists.
            System.Windows.Forms.PictureBox pbAppIcon = new System.Windows.Forms.PictureBox();
            System.Windows.Forms.PictureBox pbASBack = new System.Windows.Forms.PictureBox();
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
            // lbAppCName
            // 
            lbAppCName.AutoSize = true;
            lbAppCName.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAppCName.Location = new System.Drawing.Point(95, lbAppName.Location.Y + lbAppName.Height + 2);
            lbAppCName.Name = "lbExtCName_" + randomAppID;
            lbAppCName.Text = ext.CodeName;
            // 
            // lbAppVer
            // 
            lbAppVer.AutoSize = true;
            lbAppVer.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAppVer.Location = new System.Drawing.Point(lbAppName.Location.X + lbAppName.Width, 13);
            lbAppVer.Name = "lbextVer_" + randomAppID;
            lbAppVer.Text = "Version " + ext.Version;
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
            btAppUninstall.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            btAppUninstall.DrawImage = false;
            btAppUninstall.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            btAppUninstall.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            btAppUninstall.Location = new System.Drawing.Point(18, pbAppIcon.Location.Y + pbAppIcon.Height + 5);
            btAppUninstall.Name = "btAppUninstall_" + randomAppID;
            btAppUninstall.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
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
            pContainer.Controls.Add(pbASBack);
            pContainer.Controls.Add(lbAppCName);
            pContainer.Controls.Add(lbAppVer);
            pContainer.Controls.Add(lbAppAuthor);
            pContainer.Controls.Add(lbAppName);

        }
        void GenerateSiteTap(YorotSite site)
        {
            string randomAppID = HTAlt.Tools.GenerateRandomText(17);
            // TODO: Add all language specific ones to lists.
            System.Windows.Forms.PictureBox pbAppIcon = new System.Windows.Forms.PictureBox();
            System.Windows.Forms.PictureBox pbASBack = new System.Windows.Forms.PictureBox();
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
            lbContainerTitle.Text = site.Name;
            // 
            // pbAppIcon
            // 
            pbAppIcon.Location = new System.Drawing.Point(18, 18);
            pbAppIcon.Name = "pbAppIcon_" + randomAppID;
            pbAppIcon.Size = new System.Drawing.Size(64, 64);
            pbAppIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            //pbAppIcon.Image = YorotTools.GetSiteIcon(site);
            pbAppIcon.TabStop = false;
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
            btAppReset.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            btAppReset.DrawImage = false;
            btAppReset.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            btAppReset.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            btAppReset.Location = new System.Drawing.Point(18, pbAppIcon.Location.Y + pbAppIcon.Height + 5);
            btAppReset.Name = "btAppReset_" + randomAppID;
            btAppReset.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            btAppReset.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            btAppReset.Size = new System.Drawing.Size(pContainer.Width - 36, 23);
            btAppReset.Text = "Reset all to default";
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
            btAppUninstall.Location = new System.Drawing.Point(18, btAppReset.Location.Y + btAppReset.Height + 5);
            btAppUninstall.Name = "btAppUninstall_" + randomAppID;
            btAppUninstall.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            btAppUninstall.Size = new System.Drawing.Size(pContainer.Width - 36, 23);
            btAppUninstall.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            btAppUninstall.TabIndex = 5;
            btAppUninstall.Text = "Remove";
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
            //hsNotifications.Checked = app.
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
            pContainer.Controls.Add(pbASBack);
            pContainer.Controls.Add(lbAppCName);
            pContainer.Controls.Add(lbAppVer);
            pContainer.Controls.Add(lbAppAuthor);
            pContainer.Controls.Add(lbAppName);

        }
        void GenerateWETap(YorotWebEngine we)
        {
            string randomAppID = HTAlt.Tools.GenerateRandomText(17);
            // TODO: Add all language specific ones to lists.
            System.Windows.Forms.PictureBox pbAppIcon = new System.Windows.Forms.PictureBox();
            System.Windows.Forms.PictureBox pbASBack = new System.Windows.Forms.PictureBox();
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
            lbContainerTitle.Text = app.AppName;
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
            lbAppName.AutoSize = true;
            lbAppName.Font = new System.Drawing.Font("Ubuntu", 15F);
            lbAppName.Location = new System.Drawing.Point(95, 18);
            lbAppName.Name = "lbAppName_" + randomAppID;
            lbAppName.Text = app.AppName;
            // 
            // lbAppCName
            // 
            lbAppCName.AutoSize = true;
            lbAppCName.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAppCName.Location = new System.Drawing.Point(95, lbAppName.Location.Y + lbAppName.Height + 2);
            lbAppCName.Name = "lbAppCName_" + randomAppID;
            lbAppCName.Text = app.AppCodeName;
            // 
            // lbAppVer
            // 
            lbAppVer.AutoSize = true;
            lbAppVer.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAppVer.Location = new System.Drawing.Point(lbAppName.Location.X + lbAppName.Width, 13);
            lbAppVer.Name = "lbAppVer_" + randomAppID;
            lbAppVer.Text = "Version " + app.Version + " [" + app.VersionNo + "]";
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
            btAppReset.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            btAppReset.DrawImage = false;
            btAppReset.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            btAppReset.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            btAppReset.Location = new System.Drawing.Point(18, pbAppIcon.Location.Y + pbAppIcon.Height + 5);
            btAppReset.Name = "btAppReset_" + randomAppID;
            btAppReset.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            btAppReset.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            btAppReset.Size = new System.Drawing.Size(pContainer.Width - 36, 23);
            btAppReset.Text = app.isSystemApp ? "Cannot reset system apps" : "Reset";
            btAppReset.Enabled = !app.isSystemApp;
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
            btAppUninstall.Location = new System.Drawing.Point(18, btAppReset.Location.Y + btAppReset.Height + 5);
            btAppUninstall.Name = "btAppUninstall_" + randomAppID;
            btAppUninstall.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
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
            //hsNotifications.Checked = app.
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
            pContainer.Controls.Add(pbASBack);
            pContainer.Controls.Add(lbAppCName);
            pContainer.Controls.Add(lbAppVer);
            pContainer.Controls.Add(lbAppAuthor);
            pContainer.Controls.Add(lbAppName);

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

        private void btProfilesAndSync_Click(object sender, EventArgs e) => switch1(2);

        private void btSecurity_Click(object sender, EventArgs e) => switch1(3);

        private void btAddons_Click(object sender, EventArgs e) => switch1(5);

        private void btCustomization_Click(object sender, EventArgs e) => switch1(4);

        private void btUpdatesAbout_Click(object sender, EventArgs e) => switch1(6);

        private void settings_Load(object sender, EventArgs e)
        {

        }
        private string appliedLang = string.Empty;
        private void ApplyLanguage(bool force = false)
        {
            if (force || appliedLang != YorotGlobal.Main.CurrentLanguage.CodeName)
            {
                appliedLang = YorotGlobal.Main.CurrentLanguage.CodeName;
                var l = YorotGlobal.Main.CurrentLanguage;
                Text = l.GetItemText("Win32.DefaultApps.Settings");
                lbHomeHello.Text = l.GetItemText("Win32.Settings.HelloUser").Replace("[USERNAME]",YorotGlobal.Main.Profiles.Current.Text);
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
                tp1Container.BackColor = theme.BackColor;
                tp1Container.ForeColor = theme.ForeColor;
                btContainerBack.Image = theme.BackColor.IsBright() ? Properties.Resources.back_b : Properties.Resources.back_w;
                pContainer.BackColor = theme.BackColor2;
                pContainer.ForeColor = theme.ForeColor;
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
        private List<YorotApp> loadedApps;
        private void RefreshAppList(bool force = false)
        {
            if (force || loadedApps != YorotGlobal.Main.AppMan.Apps)
            {
                loadedApps = YorotGlobal.Main.AppMan.Apps;
                lvApps.Items.Clear();
                for (int i = 0; i < loadedApps.Count;i++)
                {
                    var app = loadedApps[i];
                    ListViewItem item = new ListViewItem()
                    {
                        Text = app.AppName,
                        ToolTipText = app.AppCodeName,
                        Tag = app,
                    };
                    ilAppMan.Images.Add(YorotTools.GetAppIcon(app));
                    item.ImageIndex = ilAppMan.Images.Count - 1;
                    lvApps.Items.Add(item);
                }
            }
        }
        private void tmrAppSync_Tick(object sender, EventArgs e)
        {
            ApplyTheme();
            ApplyLanguage();
            
        }

        private void lvApps_DoubleClick(object sender, EventArgs e)
        {
            if (lvApps.SelectedItems.Count > 0)
            {
                GenerateAppTap(lvApps.SelectedItems[0].Tag as YorotApp);
                switch1((sbyte)8);
            }
        }

        private void lvWE_DoubleClick(object sender, EventArgs e)
        {
            if (lvWE.SelectedItems.Count > 0)
            {
                GenerateWETap(lvWE.SelectedItems[0].Tag as YorotWebEngine);
                switch1((sbyte)8);
            }
        }

        private void lvSites_DoubleClick(object sender, EventArgs e)
        {
            if (lvSites.SelectedItems.Count > 0)
            {
                GenerateSiteTap(lvSites.SelectedItems[0].Tag as YorotSite);
                switch1((sbyte)8);
            }
        }

        private void lvExt_DoubleClick(object sender, EventArgs e)
        {
            if (lvExt.SelectedItems.Count > 0)
            {
                GenerateExtTap(lvExt.SelectedItems[0].Tag as YorotExtension);
                switch1((sbyte)8);
            }
        }
    }
}
