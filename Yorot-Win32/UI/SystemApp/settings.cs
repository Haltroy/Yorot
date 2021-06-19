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
            RefreshSettings(true);
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
            RefreshSettings();
            RefreshAppList();
            // TODO: Load WE; Sites, History, Downloads, Profiles
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
                    ApplyLanguage(true);
                    break;
            }
            btContainerBack.Tag = null;
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

        private void resetTab()
        {
            pContainer.Controls.Clear();

        }

        public void RefreshSettings(bool force = false)
        {
            if (force)
            {
                tbHomepage.Text = YorotGlobal.Main.CurrentSettings.HomePage;
                rbNewTab.Checked = YorotGlobal.Main.CurrentSettings.HomePage.ToLowerEnglish().StartsWith("yorot://newtab");
                if (RefreshTabSettings != null)
                {
                    Invoke(RefreshTabSettings);
                }
            }
        }

        public Action RefreshTabSettings;

        private string GetAppOrigin(YorotApp app)
        {
            string x = string.Empty;
            switch (app.AppOrigin)
            {
                case YorotAppOrigin.Other:
                    x = OriginOther + Environment.NewLine;
                    break;
                case YorotAppOrigin.Yopad:
                    x = OriginYopad + Environment.NewLine;
                    break;
                case YorotAppOrigin.Store:
                    x = OriginStore + Environment.NewLine;
                    break;
                case YorotAppOrigin.Embedded:
                    x = OriginEmbedded + Environment.NewLine;
                    break;
                case YorotAppOrigin.Unknown:
                    x = OriginUnknown + Environment.NewLine;
                    break;
            }
            return x + app.AppOriginInfo;
        }
        private void GenerateAppTab(ListViewItem item)
        {
            YorotApp app = item.Tag as YorotApp;
            System.Windows.Forms.PictureBox pbAppIcon = new System.Windows.Forms.PictureBox();
            System.Windows.Forms.Label lbAppCName = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbVer = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAppAuthor = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAppName = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTButton btReset = new HTAlt.WinForms.HTButton();
            HTAlt.WinForms.HTButton btUninst = new HTAlt.WinForms.HTButton();
            System.Windows.Forms.Label lbSizeOnDisk = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsNotifications = new HTAlt.WinForms.HTSwitch();
            System.Windows.Forms.Label lbNotif = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbEnabled = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbEnabledInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbPrior = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsPrioritize = new HTAlt.WinForms.HTSwitch();
            HTAlt.WinForms.HTSwitch hsEnabled = new HTAlt.WinForms.HTSwitch();
            System.Windows.Forms.Label lbRunStart = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsRunOnStartup = new HTAlt.WinForms.HTSwitch();
            System.Windows.Forms.Label lbNotifInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbNotifListener = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsNotifListener = new HTAlt.WinForms.HTSwitch();
            System.Windows.Forms.Label lbPriorInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbNotifListenerInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbRunStartInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbOrigin = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAppOrigin = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbIncInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAllowIncognito = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsRunOnIncognito = new HTAlt.WinForms.HTSwitch();
            lbContainerTitle.Text = item.Text;
            //
            // pbAppIcon
            //
            pbAppIcon.Location = new System.Drawing.Point(18, 18);
            pbAppIcon.Name = "pbAppIcon";
            pbAppIcon.Size = new System.Drawing.Size(64, 64);
            pbAppIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pbAppIcon.Image = YorotTools.GetAppIcon(app);
            pbAppIcon.TabStop = false;
            //
            // lbAppName
            //
            lbAppName.Font = new System.Drawing.Font("Ubuntu", 15F);
            lbAppName.Location = new System.Drawing.Point(95, 18);
            lbAppName.Name = "lbAppName";
            lbAppName.Text = item.Text;
            lbAppName.AutoSize = true;
            //
            // lbVer
            //
            lbVer.AutoSize = true;
            lbVer.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbVer.Name = "lbVer";
            lbVer.Text = Version.Replace("[V]", app.Version + " [" + app.VersionNo + "]");
            lbVer.Location = new System.Drawing.Point(lbAppName.Location.X, lbAppName.Location.Y + lbAppName.Height + 2);
            //
            // lbAppCName
            //
            lbAppCName.AutoSize = true;
            lbAppCName.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAppCName.Location = new System.Drawing.Point(lbAppName.Location.X, lbVer.Location.Y + lbVer.Height + 2);
            lbAppCName.Name = "lbAppCName";
            lbAppCName.Text = app.AppCodeName;
            //
            // lbAppAuthor
            //
            lbAppAuthor.AutoSize = true;
            lbAppAuthor.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAppAuthor.Location = new System.Drawing.Point(95, lbAppCName.Location.Y + lbAppCName.Height + 2);
            lbAppAuthor.Name = "lbAppAuthor";
            lbAppAuthor.Text = app.Author;
            //
            // btReset
            //
            btReset.AutoColor = true;
            btReset.ButtonImage = null;
            btReset.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            btReset.ClickColor = System.Drawing.Color.FromArgb(180, 180, 180);
            btReset.DrawImage = false;
            btReset.HoverColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btReset.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            btReset.Location = new System.Drawing.Point(18, lbAppAuthor.Location.Y + lbAppAuthor.Height + 5);
            btReset.Name = "btReset";
            btReset.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            btReset.NormalColor = System.Drawing.Color.FromArgb(220, 220, 220);
            btReset.Size = new System.Drawing.Size(pContainer.Width - 36, 23);
            btReset.Text = app.isSystemApp ? "Cannot reset system apps" : "Reset";
            btReset.Enabled = !app.isSystemApp;
            btReset.Tag = app;
            btReset.Click += new EventHandler((sender, e) => 
            {
                HTMsgBox mesaj = new HTMsgBox(app.AppName, AppResetMessage, new HTDialogBoxContext(MessageBoxButtons.YesNo)) { BackColor = BackColor, Yes = Yes, No = No, AutoForeColor = true, };
                DialogResult res = mesaj.ShowDialog();
                if (res == DialogResult.Yes)
                {                     
                    app.Reset();
                    lbSizeOnDisk.Text = SizeOnDisk.Replace("[S]", app.GetAppSizeInfo(SizeInfoBytes));

                }
            });
            //
            // btUninst
            //
            btUninst.AutoColor = true;
            btUninst.ButtonImage = null;
            btUninst.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            btUninst.ClickColor = System.Drawing.Color.FromArgb(180, 180, 180);
            btUninst.DrawImage = false;
            btUninst.HoverColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btUninst.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            btUninst.Location = new System.Drawing.Point(18, btReset.Location.Y + btReset.Height + 5);
            btUninst.Name = "btUninst";
            btUninst.NormalColor = System.Drawing.Color.FromArgb(220, 220, 220);
            btUninst.Size = new System.Drawing.Size(pContainer.Width - 36, 23);
            btUninst.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            btUninst.Tag = app;
            btUninst.Text = app.isSystemApp ? "Cannot uninstall system apps" : "Uninstall";
            btUninst.Enabled = !app.isSystemApp;
            btUninst.Click += new EventHandler((sender, e) => {
                HTMsgBox mesaj = new HTMsgBox(app.AppName, AppUninstMessage, new HTDialogBoxContext(MessageBoxButtons.YesNo)) { BackColor = BackColor, Yes = Yes, No = No, AutoForeColor = true,  };
                DialogResult res = mesaj.ShowDialog();
                if (res == DialogResult.Yes)
                {
                    YorotGlobal.Main.AppMan.Apps.Remove(app);
                    btContainerBack_Click(this, new EventArgs());
                    RefreshAppList(true);
                    YorotGlobal.Main.MainForm.RefreshAppList(true);
                }
            });
            //
            // lbSizeOnDisk
            //
            lbSizeOnDisk.AutoSize = true;
            lbSizeOnDisk.Font = new System.Drawing.Font("Ubuntu", 12F);
            lbSizeOnDisk.Location = new System.Drawing.Point(18, btUninst.Location.Y + btReset.Height + 5);
            lbSizeOnDisk.Name = "lbSizeOnDisk";
            lbSizeOnDisk.Text = SizeOnDisk.Replace("[S]", app.GetAppSizeInfo(SizeInfoBytes));
            //
            // lbOrigin
            //
            lbOrigin.Font = new System.Drawing.Font("Ubuntu", 12F);
            lbOrigin.Location = new System.Drawing.Point(18, lbSizeOnDisk.Location.Y + lbSizeOnDisk.Height + 5);
            lbOrigin.Name = "lbOrigin";
            lbOrigin.AutoSize = true;
            lbOrigin.Text = "Origin: ";
            //
            // lbAppOrigin
            //
            lbAppOrigin.Font = new System.Drawing.Font("Ubuntu", 12F);
            lbAppOrigin.Location = new System.Drawing.Point(lbOrigin.Location.X + lbOrigin.Width, lbOrigin.Location.Y);
            lbAppOrigin.Name = "lbAppOrigin";
            lbAppOrigin.Size = new System.Drawing.Size(pContainer.Width - (36 + lbOrigin.Width), lbOrigin.Height * 4);
            lbAppOrigin.Text = GetAppOrigin(app);
            //
            // hsEnabled
            //
            hsEnabled.Location = new System.Drawing.Point(18, lbAppOrigin.Location.Y + lbAppOrigin.Height + 20);
            hsEnabled.Name = "hsEnabled";
            hsEnabled.Size = new System.Drawing.Size(50, 19);
            hsEnabled.Checked = app.isEnabled;
            hsEnabled.Enabled = !app.isSystemApp;
            hsEnabled.CheckedChanged += new HTAlt.WinForms.HTSwitch.CheckedChangedDelegate((sender, e) =>
            {
                app.isEnabled = hsEnabled.Checked;
            });
            //
            // lbEnabled
            //
            lbEnabled.AutoSize = true;
            lbEnabled.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbEnabled.Location = new System.Drawing.Point(hsEnabled.Location.X + hsEnabled.Width + 5, hsEnabled.Location.Y - 2);
            lbEnabled.Name = "lbEnabled";
            lbEnabled.Text = "Enabled";
            //
            // lbEnabledInfo
            //
            lbEnabledInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbEnabledInfo.Location = new System.Drawing.Point(lbEnabled.Location.X, lbEnabled.Location.Y + lbEnabled.Height + 5);
            lbEnabledInfo.Name = "lbEnabledInfo";
            lbEnabledInfo.AutoSize = true;
            lbEnabledInfo.Text = app.isSystemApp ? "System apps cannot be disabled. " : "Determines if this app can be loaded or not.";
            //
            // hsNotifications
            //
            hsNotifications.Location = new System.Drawing.Point(18, lbEnabledInfo.Location.Y + lbEnabledInfo.Height + 20);
            hsNotifications.Name = "hsNotifications";
            hsNotifications.Size = new System.Drawing.Size(50, 19);
            hsNotifications.Checked = app.Permissions.allowNotif.Allowance == YorotPermissionMode.Allow;
            hsPrioritize.Enabled = hsNotifications.Checked;
            hsNotifListener.Enabled = hsNotifications.Checked;
            hsNotifications.CheckedChanged += new HTAlt.WinForms.HTSwitch.CheckedChangedDelegate((sender, e) =>
            {
                app.Permissions.allowNotif.Allowance = hsNotifications.Checked ? YorotPermissionMode.Allow : YorotPermissionMode.Deny;
                hsPrioritize.Enabled = hsNotifications.Checked;
                hsNotifListener.Enabled = hsNotifications.Checked;
            });
            //
            // lbNotifi
            //
            lbNotif.AutoSize = true;
            lbNotif.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbNotif.Location = new System.Drawing.Point(hsNotifications.Location.X + hsNotifications.Width + 5, hsNotifications.Location.Y - 2);
            lbNotif.Name = "lbNotif";
            lbNotif.AutoSize = true;
            lbNotif.Text = "Notifications";
            //
            // lbNotifInfo
            //
            lbNotifInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbNotifInfo.Location = new System.Drawing.Point(lbNotif.Location.X, lbNotif.Location.Y + lbNotif.Height + 5);
            lbNotifInfo.Name = "lbNotifInfo";
            lbNotifInfo.AutoSize = true;
            lbNotifInfo.Text = "Allows this app to show notifications";
            //
            // hsPrioritize
            //
            hsPrioritize.Location = new System.Drawing.Point(lbNotifInfo.Location.X, lbNotifInfo.Location.Y + lbNotifInfo.Height + 20);
            hsPrioritize.Name = "hsPrioritize";
            hsPrioritize.Size = new System.Drawing.Size(50, 19);
            hsPrioritize.Checked = app.Permissions.notifPriority == 1;
            hsPrioritize.CheckedChanged += new HTAlt.WinForms.HTSwitch.CheckedChangedDelegate((sender, e) =>
            {
                app.Permissions.notifPriority = hsPrioritize.Checked ? 1 : 0;
            });
            //
            // lbPrior
            //
            lbPrior.AutoSize = true;
            lbPrior.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbPrior.Location = new System.Drawing.Point(hsPrioritize.Location.X + hsPrioritize.Width, hsPrioritize.Location.Y - 2);
            lbPrior.Name = "lbPrior";
            lbPrior.Text = "Prioritize";
            //
            // lbPriorInfo
            //
            lbPriorInfo.AutoSize = true;
            lbPriorInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbPriorInfo.Location = new System.Drawing.Point(lbPrior.Location.X, lbPrior.Location.Y + lbPrior.Height + 5);
            lbPriorInfo.Name = "lbPriorInfo";
            lbPriorInfo.Text = "Prioritizes this app\'s notification from other notifications.";
            //
            // hsNotifListener
            //
            hsNotifListener.Location = new System.Drawing.Point(hsPrioritize.Location.X, lbPriorInfo.Location.Y + lbPriorInfo.Height + 20);
            hsNotifListener.Name = "hsNotifListener";
            hsNotifListener.Size = new System.Drawing.Size(50, 19);
            hsNotifListener.Checked = app.Permissions.startNotifOnBoot;
            hsNotifListener.CheckedChanged += new HTAlt.WinForms.HTSwitch.CheckedChangedDelegate((sender, e) =>
            {
                app.Permissions.startNotifOnBoot = hsNotifListener.Checked;
            });
            //
            // lbNotifListener
            //
            lbNotifListener.AutoSize = true;
            lbNotifListener.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbNotifListener.Location = new System.Drawing.Point(hsNotifListener.Location.X + hsNotifListener.Width, hsNotifListener.Location.Y - 2);
            lbNotifListener.Name = "lbNotifListener";
            lbNotifListener.Text = "Run notification listener at background";
            //
            // lbNotifListenerInfo
            //
            lbNotifListenerInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbNotifListenerInfo.Location = new System.Drawing.Point(lbNotifListener.Location.X, lbNotifListener.Location.Y + lbNotifListener.Height + 5);
            lbNotifListenerInfo.Name = "lbNotifListenerInfo";
            lbNotifListenerInfo.AutoSize = true;
            lbNotifListenerInfo.Text = "Allows Yorot to run a background service for retrieving new notifications.";
            //
            // hsRunOnStartup
            //
            hsRunOnStartup.Location = new System.Drawing.Point(hsEnabled.Location.X, lbNotifListenerInfo.Location.Y + lbNotifListenerInfo.Height + 20);
            hsRunOnStartup.Name = "hsRunOnStartup";
            hsRunOnStartup.Size = new System.Drawing.Size(50, 19);
            hsRunOnStartup.Checked = app.Permissions.runStart.Allowance == YorotPermissionMode.Allow;
            hsRunOnStartup.CheckedChanged += new HTAlt.WinForms.HTSwitch.CheckedChangedDelegate((sender, e) =>
            {
                app.Permissions.runStart.Allowance = hsRunOnStartup.Checked ? YorotPermissionMode.Allow : YorotPermissionMode.Deny;
            });
            //
            //
            // lbRunStart
            //
            lbRunStart.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbRunStart.Location = new System.Drawing.Point(hsRunOnStartup.Location.X + hsRunOnStartup.Width, hsRunOnStartup.Location.Y - 2);
            lbRunStart.Name = "lbRunStart";
            lbRunStart.AutoSize = true;
            lbRunStart.Text = "Run on startup";
            //
            // lbRunStartInfo
            //
            lbRunStartInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbRunStartInfo.Location = new System.Drawing.Point(lbRunStart.Location.X, lbRunStart.Location.Y + lbRunStart.Height + 5);
            lbRunStartInfo.Name = "lbRunStartInfo";
            lbRunStartInfo.AutoSize = true;
            lbRunStartInfo.Text = "Starts application on Yorot startup.";
            //
            // hsRunOnIncognito
            //
            hsRunOnIncognito.Location = new System.Drawing.Point(hsEnabled.Location.X, lbRunStartInfo.Location.Y + lbRunStartInfo.Height + 20);
            hsRunOnIncognito.Name = "hsRunOnIncognito";
            hsRunOnIncognito.Size = new System.Drawing.Size(50, 19);
            hsRunOnIncognito.Checked = app.Permissions.runInc.Allowance == YorotPermissionMode.Allow;
            hsRunOnIncognito.CheckedChanged += new HTAlt.WinForms.HTSwitch.CheckedChangedDelegate((sender, e) =>
            {
                app.Permissions.runInc.Allowance = hsRunOnIncognito.Checked ? YorotPermissionMode.Allow : YorotPermissionMode.Deny;
            });
            //
            // lbAllowIncognito
            //
            lbAllowIncognito.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAllowIncognito.Location = new System.Drawing.Point(hsRunOnIncognito.Location.X + hsRunOnIncognito.Width, hsRunOnIncognito.Location.Y - 2);
            lbAllowIncognito.Name = "lbAllowIncognito";
            lbAllowIncognito.AutoSize = true;
            lbAllowIncognito.Text = "Run on Incognito mode";
            //
            // lbIncInfo
            //
            lbIncInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbIncInfo.Location = new System.Drawing.Point(lbAllowIncognito.Location.X, lbAllowIncognito.Location.Y + lbAllowIncognito.Height + 5);
            lbIncInfo.Name = "lbIncInfo";
            lbIncInfo.AutoSize = true;
            lbIncInfo.Text = "Allows this app to run on Incognito mode.";

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
            pContainer.Controls.Add(lbAllowIncognito);
            pContainer.Controls.Add(lbRunStart);
            pContainer.Controls.Add(lbPrior);
            pContainer.Controls.Add(lbEnabled);
            pContainer.Controls.Add(lbEnabledInfo);
            pContainer.Controls.Add(lbIncInfo);
            pContainer.Controls.Add(lbNotif);
            pContainer.Controls.Add(lbRunStartInfo);
            pContainer.Controls.Add(lbNotifListenerInfo);
            pContainer.Controls.Add(lbPriorInfo);
            pContainer.Controls.Add(lbNotifInfo);
            pContainer.Controls.Add(lbAppOrigin);
            pContainer.Controls.Add(lbOrigin);
            pContainer.Controls.Add(lbSizeOnDisk);
            pContainer.Controls.Add(btUninst);
            pContainer.Controls.Add(btReset);
            pContainer.Controls.Add(pbAppIcon);
            pContainer.Controls.Add(lbAppCName);
            pContainer.Controls.Add(lbVer);
            pContainer.Controls.Add(lbAppAuthor);
            pContainer.Controls.Add(lbAppName);

            System.Collections.IList list = pContainer.Controls;
            for (int i = 0; i < list.Count; i++)
            {
                ((Control)list[i]).Tag = app;
            }

            RefreshTabSettings = new Action(() => 
            {
                hsRunOnIncognito.Checked = app.Permissions.runInc.Allowance == YorotPermissionMode.Allow;
                hsRunOnStartup.Checked = app.Permissions.runStart.Allowance == YorotPermissionMode.Allow;
                hsNotifListener.Checked = app.Permissions.startNotifOnBoot;
                hsPrioritize.Checked = app.Permissions.notifPriority == 1;
                hsNotifications.Checked = app.Permissions.allowNotif.Allowance == YorotPermissionMode.Allow;
                hsPrioritize.Enabled = hsNotifications.Checked;
                hsNotifListener.Enabled = hsNotifications.Checked;
                hsEnabled.Checked = app.isEnabled;
                lbSizeOnDisk.Text = SizeOnDisk.Replace("[S]", app.GetAppSizeInfo(SizeInfoBytes));
                lbAppOrigin.Text = GetAppOrigin(app);
            });
        }

        private void GenerateExtTab(YorotExtension ext)
        {
            System.Windows.Forms.PictureBox pbAppIcon = new System.Windows.Forms.PictureBox();
            System.Windows.Forms.Label lbAppCName = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbVer = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAppAuthor = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAppName = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTButton btReset = new HTAlt.WinForms.HTButton();
            HTAlt.WinForms.HTButton btUninst = new HTAlt.WinForms.HTButton();
            System.Windows.Forms.Label lbSizeOnDisk = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsNotifications = new HTAlt.WinForms.HTSwitch();
            System.Windows.Forms.Label lbMenuOptions = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbEnabled = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbEnabledInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbPrior = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsPrioritize = new HTAlt.WinForms.HTSwitch();
            HTAlt.WinForms.HTSwitch hsEnabled = new HTAlt.WinForms.HTSwitch();
            System.Windows.Forms.Label lbRunStart = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsRunOnStartup = new HTAlt.WinForms.HTSwitch();
            System.Windows.Forms.Label lbMenuOptionsiInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbMenuOptionsListener = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsNotifListener = new HTAlt.WinForms.HTSwitch();
            System.Windows.Forms.Label lbPriorInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbMenuOptionsListenerInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbRunStartInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbOrigin = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAppOrigin = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbIncInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAllowIncognito = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsRunOnIncognito = new HTAlt.WinForms.HTSwitch();
            lbContainerTitle.Text = ext.Name;
            //
            // pbAppIcon
            //
            pbAppIcon.Location = new System.Drawing.Point(18, 18);
            pbAppIcon.Name = "pbExtIcon";
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
            lbAppName.Name = "lbExtName";
            lbAppName.Text = ext.Name;
            //
            // lbVer
            //
            lbVer.AutoSize = true;
            lbVer.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbVer.Location = new System.Drawing.Point(lbAppName.Location.X, lbAppName.Location.Y + lbAppName.Height);
            lbVer.Name = "lbextVer";
            lbVer.Text = "Version " + ext.Version;
            //
            // lbAppCName
            //
            lbAppCName.AutoSize = true;
            lbAppCName.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAppCName.Location = new System.Drawing.Point(lbVer.Location.X, lbVer.Location.Y + lbVer.Height + 2);
            lbAppCName.Name = "lbExtCName";
            lbAppCName.Text = ext.CodeName;
            //
            // lbAppAuthor
            //
            lbAppAuthor.AutoSize = true;
            lbAppAuthor.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAppAuthor.Location = new System.Drawing.Point(95, lbAppCName.Location.Y + lbAppCName.Height + 2);
            lbAppAuthor.Name = "lbExtAuthor";
            lbAppAuthor.Text = ext.Author;
            //
            // btUninst
            //
            btUninst.AutoColor = true;
            btUninst.ButtonImage = null;
            btUninst.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            btUninst.ClickColor = System.Drawing.Color.FromArgb(180, 180, 180);
            btUninst.DrawImage = false;
            btUninst.HoverColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btUninst.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            btUninst.Location = new System.Drawing.Point(18, lbAppAuthor.Location.Y + lbAppAuthor.Height + 5);
            btUninst.Name = "btUninst";
            btUninst.NormalColor = System.Drawing.Color.FromArgb(220, 220, 220);
            btUninst.Size = new System.Drawing.Size(pContainer.Width - 36, 23);
            btUninst.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            btUninst.Tag = ext;
            btUninst.Text = ext.isSystemExt ? "Cannot uninstall system extensions" : "Uninstall";
            btUninst.Enabled = !ext.isSystemExt;
            //
            // hsEnabled
            //
            hsEnabled.Location = new System.Drawing.Point(18, btUninst.Location.Y + btReset.Height + 5);
            hsEnabled.Name = "hsEnabled";
            hsEnabled.Size = new System.Drawing.Size(50, 19);
            hsEnabled.Checked = ext.Enabled;
            hsEnabled.Enabled = !ext.isSystemExt;
            hsEnabled.CheckedChanged += new HTAlt.WinForms.HTSwitch.CheckedChangedDelegate((sender, e) => { ext.Enabled = hsEnabled.Checked; });
            //
            // lbEnabled
            //
            lbEnabled.AutoSize = true;
            lbEnabled.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbEnabled.Location = new System.Drawing.Point(hsEnabled.Location.X + hsEnabled.Width + 5, hsEnabled.Location.Y - 2);
            lbEnabled.Name = "lbEnabled";
            lbEnabled.Text = "Enabled";
            //
            // lbEnabledInfo
            //
            lbEnabledInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbEnabledInfo.Location = new System.Drawing.Point(lbEnabled.Location.X, lbEnabled.Location.Y + lbEnabled.Height + 5);
            lbEnabledInfo.Name = "lbEnabledInfo";
            lbEnabledInfo.AutoSize = true;
            lbEnabledInfo.Text = ext.isSystemExt ? "System extensions cannot be disabled. " : "Determines if this extension can be loaded or not.";
            //
            // hsNotifications
            //
            hsNotifications.Location = new System.Drawing.Point(18, lbEnabledInfo.Location.Y + lbEnabledInfo.Height + 20);
            hsNotifications.Name = "hsNotifications";
            hsNotifications.Size = new System.Drawing.Size(50, 19);
            hsNotifications.Checked = true;
            hsNotifications.CheckedChanged += new HTAlt.WinForms.HTSwitch.CheckedChangedDelegate((sender, e) => { ext.Permissions.allowNotif.Allowance = hsNotifications.Checked ? YorotPermissionMode.Allow : YorotPermissionMode.Deny; });
            //
            // lbMenuOptions
            //
            lbMenuOptions.AutoSize = true;
            lbMenuOptions.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbMenuOptions.Location = new System.Drawing.Point(hsNotifications.Location.X + hsNotifications.Width + 5, hsNotifications.Location.Y - 2);
            lbMenuOptions.Name = "lbMenuOptions";
            lbMenuOptions.AutoSize = true;
            lbMenuOptions.Text = "Show menu options";
            //
            // lbMenuOptionsInfo
            //
            lbMenuOptionsiInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbMenuOptionsiInfo.Location = new System.Drawing.Point(lbMenuOptions.Location.X, lbMenuOptions.Location.Y + lbMenuOptions.Height + 5);
            lbMenuOptionsiInfo.Name = "lbMenuOptionsInfo";
            lbMenuOptionsiInfo.AutoSize = true;
            lbMenuOptionsiInfo.Text = "Allows this extension to show up in menus such as the right-click menu.";
            //
            // hsRunOnStartup
            //
            hsRunOnStartup.Location = new System.Drawing.Point(lbMenuOptionsiInfo.Location.X, lbMenuOptionsiInfo.Location.Y + lbMenuOptionsiInfo.Height + 20);
            hsRunOnStartup.Name = "hsRunOnStartup";
            hsRunOnStartup.Size = new System.Drawing.Size(50, 19);
            //
            // lbRunStart
            //
            lbRunStart.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbRunStart.Location = new System.Drawing.Point(hsRunOnStartup.Location.X + hsRunOnStartup.Width, hsRunOnStartup.Location.Y - 2);
            lbRunStart.Name = "lbRunStart";
            lbRunStart.AutoSize = true;
            lbRunStart.Text = "Run on startup";
            //
            // lbRunStartInfo
            //
            lbRunStartInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbRunStartInfo.Location = new System.Drawing.Point(lbRunStart.Location.X, lbRunStart.Location.Y + lbRunStart.Height + 5);
            lbRunStartInfo.Name = "lbRunStartInfo";
            lbRunStartInfo.AutoSize = true;
            lbRunStartInfo.Text = "Starts extension on Yorot startup.";
            //
            // hsRunOnIncognito
            //
            hsRunOnIncognito.Location = new System.Drawing.Point(hsEnabled.Location.X, lbRunStartInfo.Location.Y + lbRunStartInfo.Height + 20);
            hsRunOnIncognito.Name = "hsRunOnIncognito";
            hsRunOnIncognito.Size = new System.Drawing.Size(50, 19);
            //
            // lbAllowIncognito
            //
            lbAllowIncognito.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAllowIncognito.Location = new System.Drawing.Point(hsRunOnIncognito.Location.X + hsRunOnIncognito.Width, hsRunOnIncognito.Location.Y - 2);
            lbAllowIncognito.Name = "lbAllowIncognito";
            lbAllowIncognito.AutoSize = true;
            lbAllowIncognito.Text = "Run on Incognito mode";
            //
            // lbIncInfo
            //
            lbIncInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbIncInfo.Location = new System.Drawing.Point(lbAllowIncognito.Location.X, lbAllowIncognito.Location.Y + lbAllowIncognito.Height + 5);
            lbIncInfo.Name = "lbIncInfo";
            lbIncInfo.AutoSize = true;
            lbIncInfo.Text = "Allows this extension to run on Incognito mode.";

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
            pContainer.Controls.Add(lbMenuOptionsListener);
            pContainer.Controls.Add(lbAllowIncognito);
            pContainer.Controls.Add(lbRunStart);
            pContainer.Controls.Add(lbPrior);
            pContainer.Controls.Add(lbEnabled);
            pContainer.Controls.Add(lbEnabledInfo);
            pContainer.Controls.Add(lbIncInfo);
            pContainer.Controls.Add(lbMenuOptions);
            pContainer.Controls.Add(lbRunStartInfo);
            pContainer.Controls.Add(lbMenuOptionsListenerInfo);
            pContainer.Controls.Add(lbPriorInfo);
            pContainer.Controls.Add(lbMenuOptionsiInfo);
            pContainer.Controls.Add(lbAppOrigin);
            pContainer.Controls.Add(lbOrigin);
            pContainer.Controls.Add(lbSizeOnDisk);
            pContainer.Controls.Add(btUninst);
            pContainer.Controls.Add(btReset);
            pContainer.Controls.Add(pbAppIcon);
            pContainer.Controls.Add(lbAppCName);
            pContainer.Controls.Add(lbVer);
            pContainer.Controls.Add(lbAppAuthor);
            pContainer.Controls.Add(lbAppName);

            System.Collections.IList list = pContainer.Controls;
            for (int i = 0; i < list.Count; i++)
            {
                ((Control)list[i]).Tag = ext;
            }
        }

        private void GenerateSiteTab(YorotSite site)
        {
            System.Windows.Forms.PictureBox pbSiteIcon = new System.Windows.Forms.PictureBox();
            System.Windows.Forms.Label lbAppCName = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbVer = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAppAuthor = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAppName = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTButton btReset = new HTAlt.WinForms.HTButton();
            HTAlt.WinForms.HTButton btUninst = new HTAlt.WinForms.HTButton();
            System.Windows.Forms.Label lbSizeOnDisk = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsNotifications = new HTAlt.WinForms.HTSwitch();
            System.Windows.Forms.Label lbNotif = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbEnabled = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbEnabledInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbPrior = new System.Windows.Forms.Label();
            ComboBox cbPriority = new ComboBox();
            HTAlt.WinForms.HTSwitch hsEnabled = new HTAlt.WinForms.HTSwitch();
            System.Windows.Forms.Label lbAllowCamera = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsRunOnStartup = new HTAlt.WinForms.HTSwitch();
            System.Windows.Forms.Label lbNotifInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbNotifListener = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsNotifListener = new HTAlt.WinForms.HTSwitch();
            System.Windows.Forms.Label lbPriorInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbNotifListenerInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAllowCamInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbOrigin = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAppOrigin = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAllowMicInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAllowMic = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsRunOnIncognito = new HTAlt.WinForms.HTSwitch();
            System.Windows.Forms.Label lbAllowWEInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAllowWE = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsAllowWE = new HTAlt.WinForms.HTSwitch();
            lbContainerTitle.Text = site.Name;
            //
            // pbAppIcon
            //
            pbSiteIcon.Location = new System.Drawing.Point(18, 18);
            pbSiteIcon.Name = "pbAppIcon";
            pbSiteIcon.Size = new System.Drawing.Size(64, 64);
            pbSiteIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            //pbAppIcon.Image = YorotTools.GetSiteIcon(site);
            //
            // lbAppName
            //
            lbAppName.AutoSize = true;
            lbAppName.Font = new System.Drawing.Font("Ubuntu", 15F);
            lbAppName.Location = new System.Drawing.Point(95, 18);
            lbAppName.Name = "lbAppName";
            lbAppName.Text = site.Name;
            //
            // lbAppCName
            //
            lbAppCName.AutoSize = true;
            lbAppCName.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAppCName.Location = new System.Drawing.Point(95, lbAppName.Location.Y + lbAppName.Height + 2);
            lbAppCName.Name = "lbAppCName";
            lbAppCName.Text = site.Url;
            //
            // btReset
            //
            btReset.AutoColor = true;
            btReset.ButtonImage = null;
            btReset.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            btReset.ClickColor = System.Drawing.Color.FromArgb(180, 180, 180);
            btReset.DrawImage = false;
            btReset.HoverColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btReset.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            btReset.Location = new System.Drawing.Point(18, pbSiteIcon.Location.Y + pbSiteIcon.Height + 5);
            btReset.Name = "btReset";
            btReset.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            btReset.NormalColor = System.Drawing.Color.FromArgb(220, 220, 220);
            btReset.Size = new System.Drawing.Size(pContainer.Width - 36, 23);
            btReset.Text = "Reset all to default";
            btReset.Tag = site;
            //
            // btUninst
            //
            btUninst.AutoColor = true;
            btUninst.ButtonImage = null;
            btUninst.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            btUninst.ClickColor = System.Drawing.Color.FromArgb(180, 180, 180);
            btUninst.DrawImage = false;
            btUninst.HoverColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btUninst.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            btUninst.Location = new System.Drawing.Point(18, btReset.Location.Y + btReset.Height + 5);
            btUninst.Name = "btUninst";
            btUninst.NormalColor = System.Drawing.Color.FromArgb(220, 220, 220);
            btUninst.Size = new System.Drawing.Size(pContainer.Width - 36, 23);
            btUninst.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            btUninst.TabIndex = 5;
            btUninst.Text = "Remove";
            btUninst.Tag = site;
            //
            // hsEnabled
            //
            hsEnabled.Location = new System.Drawing.Point();
            hsEnabled.Name = "hsEnabled";
            hsEnabled.Size = new System.Drawing.Size(50, 19);
            hsEnabled.Checked = site.Permissions.allowYS.Allowance == YorotPermissionMode.Allow;
            //
            // lbEnabled
            //
            lbEnabled.AutoSize = true;
            lbEnabled.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbEnabled.Location = new System.Drawing.Point(hsEnabled.Location.X + hsEnabled.Width + 5, hsEnabled.Location.Y - 2);
            lbEnabled.Name = "lbEnabled";
            lbEnabled.Text = "Allow access to Yorot Special";
            //
            // lbEnabledInfo
            //
            lbEnabledInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbEnabledInfo.Location = new System.Drawing.Point(lbEnabled.Location.X, lbEnabled.Location.Y + lbEnabled.Height + 5);
            lbEnabledInfo.Name = "lbEnabledInfo";
            lbEnabledInfo.AutoSize = true;
            lbEnabledInfo.Text = "Determines if this site can access to Yorot Specials such as theme or hardware information.";
            //
            // hsNotifications
            //
            hsNotifications.Location = new System.Drawing.Point(18, lbEnabledInfo.Location.Y + lbEnabledInfo.Height + 20);
            hsNotifications.Name = "hsNotifications";
            hsNotifications.Size = new System.Drawing.Size(50, 19);
            hsNotifications.Checked = site.Permissions.allowNotif.Allowance == YorotPermissionMode.Allow;
            //
            // lbNotifi
            //
            lbNotif.AutoSize = true;
            lbNotif.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbNotif.Location = new System.Drawing.Point(hsNotifications.Location.X + hsNotifications.Width + 5, hsNotifications.Location.Y - 2);
            lbNotif.Name = "lbNotif";
            lbNotif.AutoSize = true;
            lbNotif.Text = "Notifications";
            //
            // lbNotifInfo
            //
            lbNotifInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbNotifInfo.Location = new System.Drawing.Point(lbNotif.Location.X, lbNotif.Location.Y + lbNotif.Height + 5);
            lbNotifInfo.Name = "lbNotifInfo";
            lbNotifInfo.AutoSize = true;
            lbNotifInfo.Text = "Allows this site to show notifications";
            //
            // lbPrior
            //
            lbPrior.AutoSize = true;
            lbPrior.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbPrior.Location = new System.Drawing.Point(lbNotifInfo.Location.X, lbNotifInfo.Location.Y + lbNotifInfo.Height + 5);
            lbPrior.Name = "lbPrior";
            lbPrior.Text = "Prioritize";
            //
            // cbPriority
            //
            cbPriority.Location = new System.Drawing.Point(lbPrior.Location.X + lbPrior.Width, lbPrior.Location.Y - 2);
            cbPriority.Name = "cbPriority";
            cbPriority.Size = new System.Drawing.Size(100, 19);
            cbPriority.DropDownStyle = ComboBoxStyle.DropDownList;
            cbPriority.Items.Add("Low");
            cbPriority.Items.Add("Normal");
            cbPriority.Items.Add("High");
            cbPriority.SelectedIndex = site.Permissions.notifPriority + 1;
            //
            // lbPriorInfo
            //
            lbPriorInfo.AutoSize = true;
            lbPriorInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbPriorInfo.Location = new System.Drawing.Point(lbPrior.Location.X, lbPrior.Location.Y + lbPrior.Height + 5);
            lbPriorInfo.Name = "lbPriorInfo";
            lbPriorInfo.Text = "Prioritizes this site\'s notification from other notifications.";
            //
            // hsNotifListener
            //
            hsNotifListener.Location = new System.Drawing.Point(lbPrior.Location.X, lbPriorInfo.Location.Y + lbPriorInfo.Height + 20);
            hsNotifListener.Name = "hsNotifListener";
            hsNotifListener.Size = new System.Drawing.Size(50, 19);
            hsNotifListener.Checked = site.Permissions.startNotifOnBoot;
            //
            // lbNotifListener
            //
            lbNotifListener.AutoSize = true;
            lbNotifListener.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbNotifListener.Location = new System.Drawing.Point(hsNotifListener.Location.X + hsNotifListener.Width, hsNotifListener.Location.Y - 2);
            lbNotifListener.Name = "lbNotifListener";
            lbNotifListener.Text = "Run notification listener at background";
            //
            // lbNotifListenerInfo
            //
            lbNotifListenerInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbNotifListenerInfo.Location = new System.Drawing.Point(lbNotifListener.Location.X, lbNotifListener.Location.Y + lbNotifListener.Height + 5);
            lbNotifListenerInfo.Name = "lbNotifListenerInfo";
            lbNotifListenerInfo.AutoSize = true;
            lbNotifListenerInfo.Text = "Allows Yorot to run a background service for retrieving new notifications for this site.";
            //
            // hsRunOnStartup
            //
            hsRunOnStartup.Location = new System.Drawing.Point(hsEnabled.Location.X, lbNotifListenerInfo.Location.Y + lbNotifListenerInfo.Height + 20);
            hsRunOnStartup.Name = "hsRunOnStartup";
            hsRunOnStartup.Size = new System.Drawing.Size(50, 19);
            hsRunOnStartup.Checked = site.Permissions.allowCam.Allowance == YorotPermissionMode.Allow;
            //
            // lbAllowCamera
            //
            lbAllowCamera.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAllowCamera.Location = new System.Drawing.Point(hsRunOnStartup.Location.X + hsRunOnStartup.Width, hsRunOnStartup.Location.Y - 2);
            lbAllowCamera.Name = "lbAllowCamera";
            lbAllowCamera.AutoSize = true;
            lbAllowCamera.Text = "Allow Camera Access";
            //
            // lbAllowCamInfo
            //
            lbAllowCamInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbAllowCamInfo.Location = new System.Drawing.Point(lbAllowCamera.Location.X, lbAllowCamera.Location.Y + lbAllowCamera.Height + 5);
            lbAllowCamInfo.Name = "lbAllowCamInfo";
            lbAllowCamInfo.AutoSize = true;
            lbAllowCamInfo.Text = "Allows this site to access cameras.";
            //
            // hsRunOnIncognito
            //
            hsRunOnIncognito.Location = new System.Drawing.Point(hsEnabled.Location.X, lbAllowCamInfo.Location.Y + lbAllowCamInfo.Height + 20);
            hsRunOnIncognito.Name = "hsRunOnIncognito";
            hsRunOnIncognito.Size = new System.Drawing.Size(50, 19);
            hsRunOnIncognito.Checked = site.Permissions.allowMic.Allowance == YorotPermissionMode.Allow;
            //
            // lbAllowMic
            //
            lbAllowMic.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAllowMic.Location = new System.Drawing.Point(hsRunOnIncognito.Location.X + hsRunOnIncognito.Width, hsRunOnIncognito.Location.Y - 2);
            lbAllowMic.Name = "lbAllowMic";
            lbAllowMic.AutoSize = true;
            lbAllowMic.Text = "Allow Microphone Access";
            //
            // lbAllowMicInfo
            //
            lbAllowMicInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbAllowMicInfo.Location = new System.Drawing.Point(lbAllowMic.Location.X, lbAllowMic.Location.Y + lbAllowMic.Height + 5);
            lbAllowMicInfo.Name = "lbAllowMicInfo";
            lbAllowMicInfo.AutoSize = true;
            lbAllowMicInfo.Text = "Allows this site to access microphones.";
            //
            // hsAllowWE
            //
            hsAllowWE.Location = new System.Drawing.Point(hsEnabled.Location.X, lbAllowMicInfo.Location.Y + lbAllowMicInfo.Height + 20);
            hsAllowWE.Name = "hsRunOnIncognito";
            hsAllowWE.Size = new System.Drawing.Size(50, 19);
            hsAllowWE.Checked = site.Permissions.allowWE.Allowance == YorotPermissionMode.Allow;
            //
            // lbAllowWE
            //
            lbAllowWE.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAllowWE.Location = new System.Drawing.Point(hsAllowWE.Location.X + hsAllowWE.Width, hsAllowWE.Location.Y - 2);
            lbAllowWE.Name = "lbAllowWE";
            lbAllowWE.AutoSize = true;
            lbAllowWE.Text = "Allow Web Engines";
            //
            // lbAllowWEInfo
            //
            lbAllowWEInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbAllowWEInfo.Location = new System.Drawing.Point(lbAllowWE.Location.X, lbAllowWE.Location.Y + lbAllowWE.Height + 5);
            lbAllowWEInfo.Name = "lbAllowWEInfo";
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
            pContainer.Controls.Add(lbAllowMic);
            pContainer.Controls.Add(lbAllowCamera);
            pContainer.Controls.Add(lbPrior);
            pContainer.Controls.Add(lbEnabled);
            pContainer.Controls.Add(lbEnabledInfo);
            pContainer.Controls.Add(lbAllowMicInfo);
            pContainer.Controls.Add(lbNotif);
            pContainer.Controls.Add(lbAllowCamInfo);
            pContainer.Controls.Add(lbNotifListenerInfo);
            pContainer.Controls.Add(lbPriorInfo);
            pContainer.Controls.Add(lbNotifInfo);
            pContainer.Controls.Add(lbAppOrigin);
            pContainer.Controls.Add(lbOrigin);
            pContainer.Controls.Add(lbSizeOnDisk);
            pContainer.Controls.Add(btUninst);
            pContainer.Controls.Add(btReset);
            pContainer.Controls.Add(pbSiteIcon);
            pContainer.Controls.Add(lbAppCName);
            pContainer.Controls.Add(lbVer);
            pContainer.Controls.Add(lbAppAuthor);
            pContainer.Controls.Add(lbAppName);

            System.Collections.IList list = pContainer.Controls;
            for (int i = 0; i < list.Count; i++)
            {
                ((Control)list[i]).Tag = site;
            }
        }
        private void GenerateWETab(YorotWebEngine we)
        {
            System.Windows.Forms.PictureBox pbAppIcon = new System.Windows.Forms.PictureBox();
            System.Windows.Forms.Label lbAppCName = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbVer = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAppAuthor = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbAppName = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTButton btReset = new HTAlt.WinForms.HTButton();
            HTAlt.WinForms.HTButton btUninst = new HTAlt.WinForms.HTButton();
            System.Windows.Forms.Label lbSizeOnDisk = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbEnabled = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbWEEnabledInfo = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsEnabled = new HTAlt.WinForms.HTSwitch();
            System.Windows.Forms.Label lbWEDesc = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbWEDescInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbWEAllowIncognitoInfo = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lbWEAllowIncognito = new System.Windows.Forms.Label();
            HTAlt.WinForms.HTSwitch hsWEAllowIncognito = new HTAlt.WinForms.HTSwitch();
            lbContainerTitle.Text = we.Name;
            //
            // pbAppIcon
            //
            pbAppIcon.Location = new System.Drawing.Point(18, 18);
            pbAppIcon.Name = "pbAppIcon";
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
            lbAppName.Name = "lbAppName";
            lbAppName.Text = we.Name;
            //
            // lbVer
            //
            lbVer.AutoSize = true;
            lbVer.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbVer.Location = new System.Drawing.Point(lbAppName.Location.X, lbAppName.Location.Y + lbAppName.Height + 2);
            lbVer.Name = "lbVer";
            lbVer.Text = Version.Replace("[V]", "" + we.Version);
            lbVer.Tag = we;
            //
            // lbAppCName
            //
            lbAppCName.AutoSize = true;
            lbAppCName.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAppCName.Location = new System.Drawing.Point(lbAppName.Location.X, lbVer.Location.Y + lbVer.Height + 2);
            lbAppCName.Name = "lbAppCName";
            lbAppCName.Text = we.CodeName;
            //
            // lbAppAuthor
            //
            lbAppAuthor.AutoSize = true;
            lbAppAuthor.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbAppAuthor.Location = new System.Drawing.Point(95, lbAppCName.Location.Y + lbAppCName.Height + 2);
            lbAppAuthor.Name = "lbAppAuthor";
            lbAppAuthor.Text = we.Author;
            //
            // btReset
            //
            btReset.AutoColor = true;
            btReset.ButtonImage = null;
            btReset.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            btReset.ClickColor = System.Drawing.Color.FromArgb(180, 180, 180);
            btReset.DrawImage = false;
            btReset.HoverColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btReset.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            btReset.Location = new System.Drawing.Point(18, lbAppAuthor.Location.Y + lbAppAuthor.Height + 5);
            btReset.Name = "btReset";
            btReset.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            btReset.NormalColor = System.Drawing.Color.FromArgb(220, 220, 220);
            btReset.Size = new System.Drawing.Size(pContainer.Width - 36, 23);
            btReset.Text = Reset;
            btReset.Tag = we;
            //
            // btUninst
            //
            btUninst.AutoColor = true;
            btUninst.ButtonImage = null;
            btUninst.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            btUninst.ClickColor = System.Drawing.Color.FromArgb(180, 180, 180);
            btUninst.DrawImage = false;
            btUninst.HoverColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btUninst.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            btUninst.Location = new System.Drawing.Point(18, btReset.Location.Y + btReset.Height + 5);
            btUninst.Name = "btUninst";
            btUninst.NormalColor = System.Drawing.Color.FromArgb(220, 220, 220);
            btUninst.Size = new System.Drawing.Size(pContainer.Width - 36, 23);
            btUninst.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            btUninst.Tag = we;
            btUninst.Text = Uninstall;
            //
            // lbSizeOnDisk
            //
            lbSizeOnDisk.AutoSize = true;
            lbSizeOnDisk.Font = new System.Drawing.Font("Ubuntu", 12F);
            lbSizeOnDisk.Location = new System.Drawing.Point(18, btUninst.Location.Y + btReset.Height + 5);
            lbSizeOnDisk.Name = "lbSizeOnDisk";
            lbSizeOnDisk.Text = SizeOnDisk.Replace("[S]", we.GetWESizeInfo(SizeInfoBytes));
            lbSizeOnDisk.Tag = we;
            //
            // lbWEDesc
            //
            lbWEDesc.Font = new System.Drawing.Font("Ubuntu", 12F);
            lbWEDesc.Location = new System.Drawing.Point(18, lbSizeOnDisk.Location.Y + lbSizeOnDisk.Height + 5);
            lbWEDesc.Name = "lbWEDesc";
            lbWEDesc.AutoSize = true;
            lbWEDesc.Text = WEDesc;
            //
            // lbWEDescInfo
            //
            lbWEDescInfo.Font = new System.Drawing.Font("Ubuntu", 12F);
            lbWEDescInfo.Location = new System.Drawing.Point(lbWEDesc.Location.X + lbWEDesc.Width, lbWEDesc.Location.Y);
            lbWEDescInfo.Name = "lbWEDescInfo";
            lbWEDescInfo.Size = new System.Drawing.Size(pContainer.Width - (36 + lbWEDesc.Width), lbWEDesc.Height * 4);
            lbWEDescInfo.Text = we.Desc;
            //
            // hsEnabled
            //
            hsEnabled.Location = new System.Drawing.Point(18, lbWEDescInfo.Location.Y + lbWEDescInfo.Height + 20);
            hsEnabled.Name = "hsEnabled";
            hsEnabled.Size = new System.Drawing.Size(50, 19);
            hsEnabled.Checked = we.isEnabled;
            //
            // lbEnabled
            //
            lbEnabled.AutoSize = true;
            lbEnabled.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbEnabled.Location = new System.Drawing.Point(hsEnabled.Location.X + hsEnabled.Width + 5, hsEnabled.Location.Y - 2);
            lbEnabled.Name = "lbEnabled";
            lbEnabled.Text = AppEnabled;
            lbEnabled.Tag = we;
            //
            // lbWEEnabledInfo
            //
            lbWEEnabledInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbWEEnabledInfo.Location = new System.Drawing.Point(lbEnabled.Location.X, lbEnabled.Location.Y + lbEnabled.Height + 5);
            lbWEEnabledInfo.Name = "lbWEEnabledInfo";
            lbWEEnabledInfo.AutoSize = true;
            lbWEEnabledInfo.Text = WEEnabledInfo;
            //
            // hsRunOnIncognito
            //
            hsWEAllowIncognito.Location = new System.Drawing.Point(hsEnabled.Location.X, lbWEEnabledInfo.Location.Y + lbWEEnabledInfo.Height + 20);
            hsWEAllowIncognito.Name = "hsWEAllowIncognito";
            hsWEAllowIncognito.Size = new System.Drawing.Size(50, 19);
            //
            // lbAllowIncognito
            //
            lbWEAllowIncognito.Font = new System.Drawing.Font("Ubuntu", 12.5F);
            lbWEAllowIncognito.Location = new System.Drawing.Point(hsWEAllowIncognito.Location.X + hsWEAllowIncognito.Width, hsWEAllowIncognito.Location.Y - 2);
            lbWEAllowIncognito.Name = "lbWEAllowIncognito";
            lbWEAllowIncognito.AutoSize = true;
            lbWEAllowIncognito.Text = RunInc;
            //
            // lbIncInfo
            //
            lbWEAllowIncognitoInfo.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbWEAllowIncognitoInfo.Location = new System.Drawing.Point(lbWEAllowIncognito.Location.X, lbWEAllowIncognito.Location.Y + lbWEAllowIncognito.Height + 5);
            lbWEAllowIncognitoInfo.Name = "lbWEAllowIncognitoInfo";
            lbWEAllowIncognitoInfo.AutoSize = true;
            lbWEAllowIncognitoInfo.Text = WERunIncInfo;

            btContainerBack.Tag = (sbyte)5;
            //
            // tpAppPage
            //
            pContainer.Controls.Add(hsWEAllowIncognito);
            pContainer.Controls.Add(hsEnabled);
            pContainer.Controls.Add(lbWEAllowIncognito);
            pContainer.Controls.Add(lbEnabled);
            pContainer.Controls.Add(lbWEEnabledInfo);
            pContainer.Controls.Add(lbWEAllowIncognitoInfo);
            pContainer.Controls.Add(lbWEDescInfo);
            pContainer.Controls.Add(lbWEDesc);
            pContainer.Controls.Add(lbSizeOnDisk);
            pContainer.Controls.Add(btUninst);
            pContainer.Controls.Add(btReset);
            pContainer.Controls.Add(pbAppIcon);
            pContainer.Controls.Add(lbAppCName);
            pContainer.Controls.Add(lbVer);
            pContainer.Controls.Add(lbAppAuthor);
            pContainer.Controls.Add(lbAppName);

            System.Collections.IList list = pContainer.Controls;
            for (int i = 0; i < list.Count; i++)
            {
                ((Control)list[i]).Tag = we;
            }
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

        public string SizeOnDisk = "Size on disk: [S]";
        public string WEDesc = "Description:";
        public string AppEnabled = "Enabled";
        public string WEEnabledInfo = "Determines if this web engine can be loaded or not.";
        public string RunInc = "Allow on Incognito mode";
        public string WERunIncInfo = "Allows this web engine to load on Incognito mode.";
        public string Uninstall = "Uninstall";
        public string Remove = "Remove";
        public string CannotUninstallApp = "Cannot uninstall system apps";
        public string CannotUninstallExt = "Cannot uninstall system extensions";
        public string Reset = "Reset";
        public string CannotResetApp = "Cannot reset system apps";
        public string CannotResetExt = "Cannot reset system extensions";
        public string ResetToDefault = "Reset all to default";
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

        string OriginOther = "Other";
        string OriginYopad = "Yopad";
        string OriginStore = "Store";
        string OriginEmbedded = "Embedded";
        string OriginUnknown = "Unknown";

        private string ExtRunIncInfo = "Determines if this extension can be loaded in Incognito mode.";
        private string AppRunIncInfo = "Determines if this application can be loaded in Incognito mode.";
        private string Notif = "Notifications";
        private string SiteNotifInfo = "Determines if this site can send notifications.";
        private string AppNotifInfo = "Determines if this app can send notifications.";
        private string ExtNotifInfo = "Determines if this extension can send notifications.";
        private string Prior = "Prioritize";
        private string Origin = "Origin:";
        private string SitePriorInfo = "Determines the priority of this site.";
        private string AppPriorInfo = "Determines the priority of this app.";
        private string ShowMenu = "Show menu options";
        private string ShowMenuInfo = "Allows this extension to show up in menus such as the right-click menu.";
        private string NotifListener = "Run Notification Listener";
        private string SiteNotifListenerInfo = "Determines if Yorot should run a background service to receive notifications from this site.";
        private string AppNotifListenerInfo = "Determines if Yorot should run a background service to receive notifications from this app.";
        private string RunStart = "Run on Startup";
        private string ExtRunStartInfo = "Determines if Yorot should run this extension from startup.";
        private string AppRunStartInfo = "Determines if Yorot should run this app from startup.";
        private string AllowCamera = "Allow Camera Access";
        private string AllowCamInfo = "Determines if this site can access camera.";
        private string AllowMic = "Allow Microphone Access";
        private string AllowMicInfo = "Determines if this site can access microphone.";
        private string AllowWE = "Allow Web Engine Access";
        private string AllowWEInfo = "Determines if this site can access web engines.";

        private string CannotDisableApp = "System apps cannot be disabled.";
        private string EnableInfoApp = "Determines if this app can be loaded or not.";
        private string EnableInfoExt = "Determines if this extension can be loaded or not.";
        private string CannotDisableExt = "System extensions cannot be disabled.";

        private string Yes = "Yes";
        private string No = "No";

        private string AppResetMessage = "Do you really want to reset this app?" + Environment.NewLine + "(All data will be lost on this app).";
        private string AppUninstMessage = "Do you really want to uninstall this app?" + Environment.NewLine + "(All data will be lost on this app).";

        #region Month and Day
        public string Sunday = "Sunday";
        public string Monday = "Monday";
        public string Tuesday = "Tuesday";
        public string Wednesday = "Wednesday";
        public string Thursday = "Thursday";
        public string Friday = "Friday";
        public string Saturday = "Saturday";
        public string Month1 = "January";
        public string Month2 = "February";
        public string Month3 = "March";
        public string Month4 = "April";
        public string Month5 = "May";
        public string Month6 = "June";
        public string Month7 = "July";
        public string Month8 = "August";
        public string Month9 = "September";
        public string Month10 = "October";
        public string Month11 = "November";
        public string Month12 = "December";
        public string[] monthNames() => new string[] { Month1, Month2, Month3, Month4, Month5, Month6, Month7,Month8, Month9, Month10, Month11, Month12 };
        public string[] dayNames() => new string[] { Sunday, Monday, Tuesday, Wednesday, Thursday, Friday,Saturday };
        #endregion Month and Day

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
                Sunday = l.GetItemText("Win32.DateTime.Sunday");
                Monday = l.GetItemText("Win32.DateTime.Monday");
                Tuesday = l.GetItemText("Win32.DateTime.Tuesday");
                Wednesday = l.GetItemText("Win32.DateTime.Wednesday");
                Thursday = l.GetItemText("Win32.DateTime.Thursday");
                Friday = l.GetItemText("Win32.DateTime.Friday");
                Saturday = l.GetItemText("Win32.DateTime.Saturday");
                Month1 = l.GetItemText("Win32.DateTime.Month1");
                Month2 = l.GetItemText("Win32.DateTime.Month2");
                Month3 = l.GetItemText("Win32.DateTime.Month3");
                Month4 = l.GetItemText("Win32.DateTime.Month4");
                Month5 = l.GetItemText("Win32.DateTime.Month5");
                Month6 = l.GetItemText("Win32.DateTime.Month6");
                Month7 = l.GetItemText("Win32.DateTime.Month7");
                Month8 = l.GetItemText("Win32.DateTime.Month8");
                Month9 = l.GetItemText("Win32.DateTime.Month9");
                Month10 = l.GetItemText("Win32.DateTime.Month10");
                Month11 = l.GetItemText("Win32.DateTime.Month11");
                Month12 = l.GetItemText("Win32.DateTime.Month12");
                Yes = l.GetItemText("Win32.DialogBox.Yes");
                No = l.GetItemText("Win32.DialogBox.No");
                AppUninstMessage = l.GetItemText("Win32.Settings.AppUninstMessage").Replace("[NEWLINE]", Environment.NewLine);
                AppResetMessage = l.GetItemText("Win32.Settings.AppResetMessage").Replace("[NEWLINE]", Environment.NewLine);
                llChangeUserName.Text = l.GetItemText("Win32.Settings.Change");
                SizeOnDisk = l.GetItemText("Win32.Settings.SizeOnDisk");
                WEDesc = l.GetItemText("Win32.Settings.WEDesc");
                AppEnabled = l.GetItemText("Win32.Settings.AppEnabled");
                WEEnabledInfo = l.GetItemText("Win32.Settings.WEEnabledInfo");
                RunInc = l.GetItemText("Win32.Settings.RunInc");
                WERunIncInfo = l.GetItemText("Win32.Settings.WERunIncInfo");
                Uninstall = l.GetItemText("Win32.Settings.Uninstall");
                Remove = l.GetItemText("Win32.Settings.Remove");
                CannotUninstallApp = l.GetItemText("Win32.Settings.CannotUninstallApp");
                CannotUninstallExt = l.GetItemText("Win32.Settings.CannotUninstallExt");
                Reset = l.GetItemText("Win32.Settings.Reset");
                CannotDisableApp = l.GetItemText("Win32.Settings.CannotDisableApp");
                EnableInfoApp = l.GetItemText("Win32.Settings.EnableInfoApp");
                EnableInfoExt = l.GetItemText("Win32.Settings.EnableInfoExt");
                CannotDisableExt = l.GetItemText("Win32.Settings.CannotDisableExt");
                CannotResetApp = l.GetItemText("Win32.Settings.CannotResetApp");
                CannotResetExt = l.GetItemText("Win32.Settings.CannotResetExt ");
                ResetToDefault = l.GetItemText("Win32.Settings.ResetToDefault");
                Version = l.GetItemText("Win32.Settings.Version");
                ExtRunIncInfo = l.GetItemText("Win32.Settings.ExtRunIncInfo");
                AppRunIncInfo = l.GetItemText("Win32.Settings.AppRunIncInfo");
                Notif = l.GetItemText("Win32.Settings.Notif");
                SiteNotifInfo = l.GetItemText("Win32.Settings.SiteNotifInfo");
                OriginOther = l.GetItemText("Win32.Settings.OriginOther");
                OriginYopad = l.GetItemText("Win32.Settings.OriginYopad");
                OriginStore = l.GetItemText("Win32.Settings.OriginStore");
                OriginEmbedded = l.GetItemText("Win32.Settings.OriginEmbedded");
                OriginUnknown = l.GetItemText("Win32.Settings.OriginUnknown");
                AppNotifInfo = l.GetItemText("Win32.Settings.AppNotifInfo");
                ExtNotifInfo = l.GetItemText("Win32.Settings.ExtNotifInfo");
                Prior = l.GetItemText("Win32.Settings.Prior");
                Origin = l.GetItemText("Win32.Settings.Origin");
                SitePriorInfo = l.GetItemText("Win32.Settings.SitePriorInfo");
                AppPriorInfo = l.GetItemText("Win32.Settings.AppPriorInfo");
                NotifListener = l.GetItemText("Win32.Settings.NotifListener");
                SiteNotifListenerInfo = l.GetItemText("Win32.Settings.SiteNotifListenerInfo");
                AppNotifListenerInfo = l.GetItemText("Win32.Settings.AppNotifListenerInfo");
                RunStart = l.GetItemText("Win32.Settings.RunStart");
                ExtRunStartInfo = l.GetItemText("Win32.Settings.ExtRunStartInfo");
                AppRunStartInfo = l.GetItemText("Win32.Settings.AppRunStartInfo");
                AllowCamera = l.GetItemText("Win32.Settings.AllowCamera");
                AllowCamInfo = l.GetItemText("Win32.Settings.AllowCamInfo");
                AllowMic = l.GetItemText("Win32.Settings.AllowMic");
                AllowMicInfo = l.GetItemText("Win32.Settings.AllowMicInfo");
                AllowWE = l.GetItemText("Win32.Settings.AllowWE");
                AllowWEInfo = l.GetItemText("Win32.Settings.AllowWEInfo");
                ShowMenu = l.GetItemText("Win32.Settings.ShowMenu");
                ShowMenuInfo = l.GetItemText("Win32.Settings.ShowMenuInfo");
                SearchApp = l.GetItemText("Win32.Settings.AppsSearch");
                SearchExt = l.GetItemText("Win32.Settings.ExtSearch");
                SearchWE = l.GetItemText("Win32.Settings.WESearch");
                SearchSite = l.GetItemText("Win32.Settings.SiteSearch");
                SizeInfoBytes = l.GetItemText("Win32.Settings.Bytes");
                RemoveSelected = l.GetItemText("Win32.Settings.RemSelected");
                btContainerBack.Text = l.GetItemText("Win32.Settings.Back");
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
                for (int i = 0; i < logOIFLile.Count; i++)
                {
                    logOIFLile[i].Text = OpenInFiles;
                }
                for (int i = 0; i < logInfoLabels.Count; i++)
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
                // TODO:
                /*


            lbLogDescEx.Name = "lbLogDescEx";
lbLogDescEx.Tag = logInfo; (int[])
lbLogDescEx.Text = LogInfo.Replace("[W]", "" + logInfo[0]).Replace("[E]", "" + logInfo[1]).Replace("[C]", "" + logInfo[2]).Replace("[I]", "" + logInfo[3]).Replace("[NEWLINE]",Environment.NewLine);

btLogOpenEx.Name = "btLogOpenEx";
btLogOpenEx.Tag = logLoc; (string)
btLogOpenEx.Text = OpenInFiles;

lbHistoryExDate.Name = "lbHistoryExDate";
lbHistoryExDate.Tag = site.Date; (DateTime)
lbHistoryExDate.Text = YorotGlobal.Main.CurrentSettings.DateFormat.GetLongName(site.Date, monthNames(), dayNames());







 */

                if (btContainerBack.Tag != null)
                {
                    System.Collections.IList list = pContainer.Controls;
                    for (int i = 0; i < list.Count; i++)
                    {
                        Control c = (Control)list[i];
                        switch (c)
                        {
                            case Label _:
                                Label label = c as Label;
                                switch(label.Name)
                                {
                                    case "lbVer":
                                        switch(label.Tag)
                                        {
                                            case YorotWebEngine _:
                                                label.Text = Version.Replace("[V]", "" + (label.Tag as YorotWebEngine).Version);
                                                break;
                                            case YorotExtension _:
                                                label.Text = Version.Replace("[V]", "" + (label.Tag as YorotExtension).Version);
                                                break;
                                            case YorotApp _:
                                                label.Text = Version.Replace("[V]", (label.Tag as YorotApp).Version + " [" + (label.Tag as YorotApp).VersionNo + "]");
                                                break;
                                        }
                                        break;
                                    case "lbSizeOnDisk":
                                        switch (label.Tag)
                                        {
                                            case YorotWebEngine _:
                                                label.Text = SizeOnDisk.Replace("[S]", (label.Tag as YorotWebEngine).GetWESizeInfo(SizeInfoBytes));
                                                break;
                                            case YorotExtension _:
                                                label.Text = SizeOnDisk.Replace("[S]", (label.Tag as YorotExtension).GetSizeOnDisk(SizeInfoBytes));
                                                break;
                                            case YorotApp _:
                                                label.Text = SizeOnDisk.Replace("[S]", (label.Tag as YorotApp).GetAppSizeInfo(SizeInfoBytes));
                                                break;
                                        }
                                        break;
                                    case "lbEnabledInfo":
                                        switch (label.Tag)
                                        {
                                            case YorotWebEngine _:
                                                label.Text = WEEnabledInfo;
                                                break;
                                            case YorotExtension _:
                                                label.Text = (label.Tag as YorotExtension).isSystemExt ? CannotDisableExt : EnableInfoExt;
                                                break;
                                            case YorotApp _:
                                                label.Text = (label.Tag as YorotApp).isSystemApp ? CannotDisableApp : EnableInfoApp;
                                                break;
                                        }
                                        break;
                                    case "lbOrigin":
                                        label.Text = Origin;
                                        break;
                                    case "lbOriginInfo":
                                        label.Text = GetAppOrigin(label.Tag as YorotApp);
                                        break;
                                    case "lbWEDesc":
                                        label.Text = WEDesc;
                                        break;
                                    case "lbEnabled":
                                        label.Text = AppEnabled;
                                        break;

                                    case "lbAllowIncognito":
                                        label.Text = RunInc;
                                        break;
                                    case "lbIncInfo":
                                        switch (label.Tag)
                                        {
                                            case YorotWebEngine _:
                                                label.Text = WERunIncInfo;
                                                break;
                                            case YorotExtension _:
                                                label.Text = ExtRunIncInfo;
                                                break;
                                            case YorotApp _:
                                                label.Text = AppRunIncInfo;
                                                break;
                                        }
                                        break;
                                    case "lbNotif":
                                        label.Text = Notif;
                                        break;
                                    case "lbNotifInfo":
                                        switch(label.Tag)
                                        {
                                            case YorotSite _:
                                                label.Text = SiteNotifInfo;
                                                break;
                                            case YorotApp _:
                                                label.Text = AppNotifInfo;
                                                break;
                                            case YorotExtension _:
                                                label.Text = ExtNotifInfo;
                                                break;
                                        }
                                        break;
                                    case "lbPrior":
                                        label.Text = Prior;
                                        break;
                                    case "lbPriorInfo":
                                        switch (label.Tag)
                                        {
                                            case YorotSite _:
                                                label.Text = SitePriorInfo;
                                                break;
                                            case YorotApp _:
                                                label.Text = AppPriorInfo;
                                                break;
                                        }
                                        break;
                                    case "lbNotifListener":
                                        label.Text = NotifListener;
                                        break;

                                    case "lbNotifListenerInfo":
                                        switch(label.Tag)
                                        {
                                            case YorotSite _:
                                                label.Text = SiteNotifListenerInfo;
                                                break;
                                            case YorotApp _:
                                                label.Text = AppNotifListenerInfo;
                                                break;
                                        }
                                        break;

                                    case "lbShowMenu":
                                        label.Text = ShowMenu;
                                        break;

                                    case "lbShowMenuInfo":
                                        label.Text = ShowMenuInfo;
                                        break;

                                    case "lbRunStart":
                                        label.Text = RunStart;
                                        break;

                                    case "lbRunStartInfo":
                                        label.Text = label.Tag is YorotExtension ? ExtRunStartInfo : AppRunStartInfo;
                                        break;

                                    case "lbAllowCamera":
                                        label.Text = AllowCamera;
                                        break;

                                    case "lbAllowCamInfo":
                                        label.Text = AllowCamInfo;
                                        break;

                                    case "lbAllowMic":
                                        label.Text = AllowMic;
                                        break;

                                    case "lbALlowMicInfo":
                                        label.Text = AllowMicInfo;
                                        break;

                                    case "lbAllowWE":
                                        label.Text = AllowWE;
                                        break;

                                    case "lbAllowWEInfo":
                                        label.Text = AllowWEInfo;
                                        break;

                                }
                                break;
                            case HTButton _:
                                HTButton htbutton = c as HTButton;
                                switch (htbutton.Name)
                                {
                                    case "btReset":
                                        switch (htbutton.Tag)
                                        {
                                            case YorotApp _:
                                                htbutton.Text = ((YorotApp)htbutton.Tag).isSystemApp ? CannotResetApp : Reset;
                                                break;
                                            case YorotWebEngine _:
                                                htbutton.Text = Reset;
                                                break;
                                            case YorotSite _:
                                                htbutton.Text = ResetToDefault;
                                                break;
                                            case YorotExtension _:
                                                htbutton.Text = ((YorotExtension)htbutton.Tag).isSystemExt ? CannotResetExt : Reset;
                                                break;
                                        }
                                        break;
                                    case "btUninst":
                                        switch (htbutton.Tag)
                                        {
                                            case YorotApp _:
                                                htbutton.Text = ((YorotApp)htbutton.Tag).isSystemApp ? CannotUninstallApp : Uninstall;
                                                break;
                                            case YorotWebEngine _:
                                                htbutton.Text = Uninstall;
                                                break;
                                            case YorotSite _:
                                                htbutton.Text = Remove;
                                                break;
                                            case YorotExtension _:
                                                htbutton.Text = ((YorotExtension)htbutton.Tag).isSystemExt ? CannotUninstallExt : Uninstall;
                                                break;
                                        }
                                        break;
                                }
                                break;
                        }
                    }
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

        private void genHistoryEntry(YorotSite site)
        {
            System.Windows.Forms.Panel pHistoryEx = new System.Windows.Forms.Panel();
            System.Windows.Forms.TextBox lbHistoryExUrl = new System.Windows.Forms.TextBox();
            System.Windows.Forms.PictureBox pbHistoryExIcon = new System.Windows.Forms.PictureBox();
            System.Windows.Forms.Label lbHistoryExClose = new System.Windows.Forms.Label();
            System.Windows.Forms.TextBox lbHistoryExTitle = new System.Windows.Forms.TextBox();
            System.Windows.Forms.TextBox lbHistoryExDate = new System.Windows.Forms.TextBox();

            pHistoryEx.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(pbHistoryExIcon)).BeginInit();

            // 
            // lbHistoryExClose
            // 
            lbHistoryExClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            lbHistoryExClose.AutoSize = true;
            lbHistoryExClose.Font = new System.Drawing.Font("Ubuntu", 12F);
            lbHistoryExClose.Location = new System.Drawing.Point(pHistory.Width - 25, 5);
            lbHistoryExClose.Name = "lbHistoryExClose";
            lbHistoryExClose.Text = "X";

            // 
            // pbHistoryExIcon
            // 
            pbHistoryExIcon.Location = new System.Drawing.Point(15, lbHistoryExClose.Location.Y + lbHistoryExClose.Height + 5);
            pbHistoryExIcon.Name = "pbHistoryExIcon";
            pbHistoryExIcon.Size = new System.Drawing.Size(64,64);
            pbHistoryExIcon.SizeMode = PictureBoxSizeMode.Zoom;
            Icon siteIcon = Yorot.Tools.GetSiteIcon(site, YorotGlobal.Main);
            pbHistoryExIcon.Image = siteIcon != null ? siteIcon.ToBitmap() : Properties.Resources.Yorot; //TODO: Change Yorot logo to default No-Icon logo.

            // 
            // lbHistoryExTitle
            // 
            lbHistoryExTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            lbHistoryExTitle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lbHistoryExTitle.Font = new System.Drawing.Font("Ubuntu", 12F);
            lbHistoryExTitle.Location = new System.Drawing.Point(pbHistoryExIcon.Location.X + pbHistoryExIcon.Width + 5, lbHistoryExClose.Location.Y + lbHistoryExClose.Height + 5);
            lbHistoryExTitle.Name = "lbHistoryExTitle";
            lbHistoryExTitle.ReadOnly = true;
            lbHistoryExTitle.Size = new System.Drawing.Size(pHistory.Width - (pbHistoryExIcon.Location.X + pbHistoryExIcon.Width + 15), 19);
            lbHistoryExTitle.Text = site.Name;

            // 
            // lbHistoryExDate
            // 
            lbHistoryExDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            lbHistoryExDate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lbHistoryExDate.Font = new System.Drawing.Font("Ubuntu", 11F);
            lbHistoryExDate.Location = new System.Drawing.Point(lbHistoryExTitle.Location.X, lbHistoryExTitle.Location.Y + lbHistoryExTitle.Height + 5);
            lbHistoryExDate.Name = "lbHistoryExDate";
            lbHistoryExDate.ReadOnly = true;
            lbHistoryExDate.Size = new System.Drawing.Size(lbHistoryExTitle.Width, 17);
            lbHistoryExDate.Tag = site.Date;
            lbHistoryExDate.Text = YorotGlobal.Main.CurrentSettings.DateFormat.GetLongName(site.Date, monthNames(), dayNames());

            // 
            // lbHistoryExUrl
            // 
            lbHistoryExUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            lbHistoryExUrl.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lbHistoryExUrl.Font = new System.Drawing.Font("Ubuntu", 10F);
            lbHistoryExUrl.Location = new System.Drawing.Point(lbHistoryExTitle.Location.X, lbHistoryExDate.Location.Y + lbHistoryExDate.Height + 5);
            lbHistoryExUrl.Name = "lbHistoryExUrl";
            lbHistoryExUrl.ReadOnly = true;
            lbHistoryExUrl.Size = new System.Drawing.Size(lbHistoryExTitle.Width, 16);
            lbHistoryExUrl.Text = site.Url;
            
            // 
            // pHistoryEx
            // 
            pHistoryEx.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pHistoryEx.Controls.Add(lbHistoryExDate);
            pHistoryEx.Controls.Add(lbHistoryExUrl);
            pHistoryEx.Controls.Add(pbHistoryExIcon);
            pHistoryEx.Controls.Add(lbHistoryExClose);
            pHistoryEx.Controls.Add(lbHistoryExTitle);
            pHistoryEx.Dock = System.Windows.Forms.DockStyle.Top;
            pHistoryEx.Location = new System.Drawing.Point(0, 0);
            pHistoryEx.Margin = new System.Windows.Forms.Padding(10);
            pHistoryEx.Name = "pHistoryEx";
            pHistoryEx.Height = 105;
            pHistory.Controls.Add(pHistoryEx);
            pHistoryEx.ResumeLayout(false);
            pHistoryEx.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(pbHistoryExIcon)).EndInit();
        }

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
            string logFile = HTAlt.Tools.ReadFile(logLoc, System.Text.Encoding.UTF8);
            string[] lines = logFile.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            int w = 0;
            int e = 0;
            int c = 0;
            int ı = 0;
            for(int i = 0; i < lines.Length;i++)
            {
                if (lines[i].ToLowerEnglish().StartsWith(" [w]"))
                {
                    w++;
                }else if (lines[i].ToLowerEnglish().StartsWith(" [e]"))
                {
                    e++;
                }
                else if (lines[i].ToLowerEnglish().StartsWith(" [c]"))
                {
                    c++; //lmao
                }
                else if (lines[i].ToLowerEnglish().StartsWith(" [i]"))
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
            lbLogDescEx.Tag = logInfo;
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
            btLogOpenEx.Click += new EventHandler((sender, e) => 
            {
                YorotGlobal.Main.MainForm.LaunchApp(YorotGlobal.Main.AppMan.FindByAppCN("com.haltroy.fileman"), new string[] { logLoc });
            });
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