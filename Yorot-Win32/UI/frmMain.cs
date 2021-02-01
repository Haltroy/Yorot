using HTAlt;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Yorot
{
    public partial class frmMain : Form
    {

        #region Constructor

        bool isCarbonCopy = true;

        public frmMain() : this(false) { }
        public frmMain(bool isMainSession)
        {
            if (YorotGlobal.Settings == null)
            {
                YorotGlobal.Settings = new Settings();
            }
            InitializeComponent();
            pAppDrawer.Width = panelMinSize;
            isCarbonCopy = !isMainSession;
            YorotGlobal.Y1.MainForms.Add(this);
            if (!isCarbonCopy) { RefreshAppList(true); }  else
            {
                label2.Visible = true;
                label2.Enabled = true;
                htButton1.Visible = true;
                htButton1.Enabled = true;
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            
        }
        int appListUpdate = 0;
        private void tmrAppSync_Tick(object sender, EventArgs e)
        {
            if (appListUpdate != YorotGlobal.Settings.AppMan.UpdateCount)
            {
                RefreshAppList(false);
            }
            // These both if() statements do diffrenet jobs, so that SideCreekGiant cards won't work on here.
            if (isFullScreen && PrevDirection != AnimateDirection.LeftFullScreen && !AnimationContinue)
            {
                AnimateTo(AnimateDirection.LeftFullScreen);
            }
        }
        private void RefreshAppList(bool clearCurrent = false)
        {
            appListUpdate = YorotGlobal.Settings.AppMan.UpdateCount;
            if (clearCurrent)
            {
                lvApps.Items.Clear();

                ListViewItem yorotApp = null;
                foreach (YorotApp kapp in YorotGlobal.Settings.AppMan.Apps)
                {
                    ilAppMan.Images.Add(YorotTools.GenerateAppIcon(kapp.GetAppIcon(), "#808080".HexToColor()));
                    ListViewItem item = new ListViewItem()
                    {
                        Text = kapp.AppName,
                        ToolTipText = kapp.AppCodeName,
                        ImageIndex = ilAppMan.Images.Count - 1,
                        Tag = kapp,
                    };
                    if (kapp.AppCodeName == "com.haltroy.yorot") { yorotApp = item; }
                    else
                    {
                        lvApps.Items.Add(item);
                    }
                }
                lvApps.Items.Insert(0, yorotApp);
            }else
            {
                foreach(YorotApp yapp in YorotGlobal.Settings.AppMan.Apps)
                {
                    bool found = false;
                    for(int i =0;i < lvApps.Items.Count;i++)
                    {
                        if (lvApps.Items[i].ToolTipText == yapp.AppCodeName)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        ilAppMan.Images.Add(YorotTools.GenerateAppIcon(yapp.GetAppIcon(), "#808080".HexToColor()));
                        ListViewItem item = new ListViewItem()
                        {
                            Text = yapp.AppName,
                            ToolTipText = yapp.AppCodeName,
                            ImageIndex = ilAppMan.Images.Count - 1,
                            Tag = yapp,
                        };
                        lvApps.Items.Add(item);
                    }
                }
            }
        }

        #endregion Constructor

        #region Animator
        /// <summary>
        /// Animation directions.
        /// </summary>
        private enum AnimateDirection
        {
            /// <summary>
            /// No animation.
            /// </summary>
            Nothing,
            /// <summary>
            /// Back to normal
            /// </summary>
            Left,
            /// <summary>
            /// Expand to current screen
            /// </summary>
            RightMost,
            /// <summary>
            /// Hide completely
            /// </summary>
            LeftFullScreen,
        }

        private void AnimateTo(AnimateDirection animate)
        {
            if (animate == AnimateDirection.Nothing) { return; }
            AnimationContinue = true;
            panelMaxSize = Width - 220;
            Direction = animate;
            timer1.Start();
        }

        private AnimateDirection Direction = AnimateDirection.Nothing;
        private AnimateDirection PrevDirection = AnimateDirection.Left;
        private readonly int AnimationSpeed = 60;
        private readonly int panelMinSize = 60;
        private int panelMaxSize = 220;
        // ş
        private int panelMinDeadZWord = 220;
        // ş
        private bool AnimationContinue = false;
        private bool isAnimating => Direction != AnimateDirection.Nothing;

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (AnimationContinue)
            {
                case true:
                    if (pAppDrawer.Dock != DockStyle.Left)
                    {
                        pAppDrawer.Dock = DockStyle.Left;
                    }
                    switch (Direction)
                    {
                        case AnimateDirection.Left:
                            switch (PrevDirection)
                            {
                                case AnimateDirection.RightMost:
                                    pAppDrawer.Width = pAppDrawer.Width > panelMinSize ? pAppDrawer.Width - AnimationSpeed : panelMinSize;
                                    AnimationContinue = pAppDrawer.Width > panelMinSize;
                                    break;
                                case AnimateDirection.LeftFullScreen:
                                    pAppDrawer.Width = pAppDrawer.Width < panelMinSize ? pAppDrawer.Width + (pAppDrawer.Width < panelMinDeadZWord ? (AnimationSpeed / 6) : AnimationSpeed) : panelMinSize;
                                    AnimationContinue = pAppDrawer.Width < panelMinSize;
                                    break;
                                case AnimateDirection.Left:
                                    pAppDrawer.Width = panelMinSize;
                                    AnimationContinue = false;
                                    break;
                            }
                            break;
                        case AnimateDirection.LeftFullScreen:
                            pAppDrawer.Width = pAppDrawer.Width > label1.Width ? pAppDrawer.Width - (pAppDrawer.Width < panelMinDeadZWord ? (AnimationSpeed/6) : AnimationSpeed) : pAppDrawer.Width;
                            AnimationContinue = pAppDrawer.Width > label1.Width;
                            break;
                        case AnimateDirection.RightMost:
                            pAppDrawer.Width = pAppDrawer.Width < (Width - 10) ? pAppDrawer.Width + AnimationSpeed : (Width - 10);
                            AnimationContinue = pAppDrawer.Width < (Width - 10);
                            break;
                    }
                    break;
                case false:
                    switch (Direction)
                    {
                        case AnimateDirection.Nothing:
                        default:
                        case AnimateDirection.Left:
                            pAppDrawer.Width = panelMinSize;
                            break;
                        case AnimateDirection.LeftFullScreen:
                            pAppDrawer.Width = label1.Width;
                            break;
                        case AnimateDirection.RightMost:
                            pAppDrawer.Width = Width - 15;
                            break;
                    }
                    pAppDrawer.Anchor = Direction == AnimateDirection.RightMost
                        ? AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
                        : AnchorStyles.Top | AnchorStyles.Left;
                    pAppDrawer.Dock = Direction == AnimateDirection.RightMost ? DockStyle.Fill : DockStyle.Left;
                    PrevDirection = Direction;
                    Direction = AnimateDirection.Nothing;
                    timer1.Stop();
                    break;
            }
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            if (pAppDrawer.Dock != DockStyle.Left)
            {
                pAppDrawer.Dock = DockStyle.Left;
            }
            if (e.Clicks > 2)
            {
                switch (PrevDirection)
                {
                    default:
                    case AnimateDirection.Left:
                    case AnimateDirection.Nothing:
                    case AnimateDirection.LeftFullScreen:
                        AnimateTo(AnimateDirection.RightMost);
                        break;
                    case AnimateDirection.RightMost:
                        AnimateTo(AnimateDirection.Left);
                        break;
                }
            }
            else
            {
                allowResize = true;
            }
        }

        private void label1_MouseUp(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                if (pAppDrawer.Width > panelMaxSize)
                {
                    AnimateTo(AnimateDirection.RightMost);
                }
                else if (pAppDrawer.Width < panelMinDeadZWord)
                {
                    PrevDirection = AnimateDirection.RightMost;
                    AnimateTo(AnimateDirection.Left);
                }
            }
            allowResize = false;
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            int w = label1.Left + e.X;
            pAppDrawer.Width = allowResize ? (w > 0 ? (w <= (Width - 15) ? w : (Width - 15)) : panelMinSize) : pAppDrawer.Width;
        }

        private bool allowResize = false;

        private void Form1_MouseLeave(object sender, EventArgs e)
        {
            allowResize = false;
        }

        #endregion Animator

        public void loadMainTab()
        {
            if(tcAppMan.InvokeRequired)
            {
                tcAppMan.Invoke(new Action(() => { allowSwitch = true; tcAppMan.SelectedTab = tabPage1; }));
            }else
            {
                allowSwitch = true; 
                tcAppMan.SelectedTab = tabPage1;
            }
        }

        public void loadSpecificTab(TabPage tp)
        {
            if (tcAppMan.InvokeRequired)
            {
                tcAppMan.Invoke(new Action(() => { allowSwitch = true; tcAppMan.SelectedTab = tp; }));
            }
            else
            {
                allowSwitch = true;
                tcAppMan.SelectedTab = tp;
            }
        }

        bool allowSwitch = false;
        private void tcAppMan_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (allowSwitch) { allowSwitch = false; } else { e.Cancel = true; }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (lvApps.SelectedItems.Count > 0)
            {
                ListViewItem appItem = lvApps.SelectedItems[0];
                YorotApp app = appItem.Tag as YorotApp;
                string appcn = appItem.ToolTipText;
                if (appcn == "com.haltroy.yorot") 
                {
                    AnimateTo(AnimateDirection.Left);
                }else if (appcn == "com.haltroy.settings")
                {
                    pbSettings_MouseClick(sender, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
                }
                else
                {
                    if (app.Layouts.Count > 0)
                    {
                        var layout = app.Layouts[0];
                        if (layout.AssocForm.freeMode)
                        {
                            layout.AssocForm.Show();
                            layout.AssocForm.BringToFront();
                        }
                        else
                        {
                            TabControl tabc = layout.AssocTab.Parent as TabControl;
                            if (tabc.InvokeRequired)
                            {
                                Invoke(new Action(() => { allowSwitch = true; tabc.SelectedTab = layout.AssocTab; }));
                            }
                            else
                            {
                                allowSwitch = true;
                                tabc.SelectedTab = layout.AssocTab;
                            }
                        }
                    }
                    else
                    {
                        UI.frmApp fapp /* pls dont laught at this we are not 4th graders */ = new UI.frmApp(app) { assocForm = this, TopLevel = false, Visible = true, Dock = DockStyle.Fill, FormBorderStyle = FormBorderStyle.None };
                        showApp(fapp);
                    }
                }
                foreach(Control x in flpFavApps.Controls)
                {
                    x.Refresh();
                }
            }
        }
        private void showApp(UI.frmApp app,YAMItem sender = null)
        {
            if (sender == null)
            {
                TabPage tp = new TabPage() { Text = app.assocApp.AppName };
                YorotAppLayout layout = new YorotAppLayout()
                {
                    AssocForm = app,
                    AssocTab = tp,
                };
                app.assocApp.Layouts.Add(layout);
                app.assocLayout = layout;
                tp.Controls.Add(app);
                tcAppMan.TabPages.Add(tp);
                allowSwitch = true;
                tcAppMan.SelectedTab = tp;
                if (app.assocApp.AppCodeName != "com.haltroy.settings")
                {
                    YAMItem pbIcon = new YAMItem() { Size = new Size(38, 38), Margin = new Padding(3), Visible = true, AssocApp = app.assocApp, AssocFrmMain = this };
                    pbIcon.MouseClick += pbIcon_MouseClick;
                    layout.AssocItem = pbIcon;
                    flpFavApps.Controls.Add(pbIcon); pbIcon.Refresh();
                }
            }else
            {
                TabPage tp = new TabPage() { Text = app.assocApp.AppName };
                YorotAppLayout layout = new YorotAppLayout()
                {
                    AssocForm = app,
                    AssocTab = tp,
                };
                app.assocApp.Layouts.Add(layout);
                app.assocLayout = layout;
                tp.Controls.Add(app);
                tcAppMan.TabPages.Add(tp);
                allowSwitch = true;
                tcAppMan.SelectedTab = tp;
                layout.AssocItem = sender;
                if (app.assocLayout.AssocItem.InvokeRequired) { app.assocLayout.AssocItem.Invoke(new Action(() => app.assocLayout.AssocItem.Refresh())); } else { app.assocLayout.AssocItem.Refresh(); }
            }
        }
        private void pbIcon_MouseClick(object sender, MouseEventArgs e)
        {
            YAMItem pbIcon = sender as YAMItem;
            if (e.Button == MouseButtons.Left)
            {
                if (pbIcon.CurrentStatus == YAMItem.AppStatuses.Pinned) // Launch app
                {
                    UI.frmApp fapp  = new UI.frmApp(pbIcon.AssocApp) { assocForm = this, TopLevel = false, Visible = true, Dock = DockStyle.Fill, FormBorderStyle = FormBorderStyle.None };
                    showApp(fapp,pbIcon);
                }
                else
                {
                    if (pbIcon.AssocApp.Layouts[pbIcon.FocusedLayoutIndex].AssocForm.ContainsFocus || pbIcon.AssocApp.Layouts[pbIcon.FocusedLayoutIndex].AssocForm.Focused)
                    {
                        // shift between sessions
                        pbIcon.FocusedLayoutIndex = pbIcon.FocusedLayoutIndex < pbIcon.AssocApp.Layouts.Count - 1 ? pbIcon.FocusedLayoutIndex + 1 : 0;

                    }
                    var fapp = pbIcon.AssocApp.Layouts[pbIcon.FocusedLayoutIndex].AssocForm;
                    if (fapp.freeMode)
                    {
                        fapp.Show();
                        fapp.BringToFront();
                    }
                    else
                    {
                        if (pAppDrawer.Width < panelMaxSize)
                        {
                            AnimateTo(AnimateDirection.RightMost);
                        }
                        allowSwitch = true;
                        tcAppMan.SelectedTab = fapp.assocLayout.AssocTab;
                    }
                }

            }
            else if (e.Button == MouseButtons.Right)
            {
                rcSender = pbIcon;
                rcType = 2;
                cmsApp.Show(MousePosition);
            }
        }
        /// <summary>
        /// -1= None 0= lvApps 1= lvApps App 2= App 3= Yorot 4= Settings
        /// </summary>
        int rcType = -1;
        /// <summary>
        /// Sender of RC request
        /// </summary>
        object rcSender;

        private void htButton1_Click(object sender, EventArgs e)
        {
            label2.Visible = false;
            label2.Enabled = false;
            htButton1.Visible = false;
            htButton1.Enabled = false;
            RefreshAppList(true);
        }

        private void lvApps_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (lvApps.SelectedItems.Count != 0) // App right-click
                {
                    rcType = 1;
                    rcSender = lvApps.SelectedItems[0];
                }
                else // Normnal right-click
                {
                    rcType = 0;
                    rcSender = lvApps;
                }
                cmsApp.Show(MousePosition);
            }
        }

        private void cmsApp_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            switch(rcType)
            {
                case -1:
                    e.Cancel = true;
                    break;
                case 0:
                    openANewSessionToolStripMenuItem.Visible = false;
                    closeAllSessionsToolStripMenuItem.Visible = false;
                    tsAppSep1.Visible = false;
                    pinToAppBarToolStripMenuItem.Visible = false;
                    appSettingsToolStripMenuItem.Visible = false;
                    tsAppSep2.Visible = false;
                    reloadToolStripMenuItem.Visible = true;
                    settingsToolStripMenuItem.Visible = true;
                    openANewSessionToolStripMenuItem.Enabled = false;
                    closeAllSessionsToolStripMenuItem.Enabled = false;
                    tsAppSep1.Enabled = false;
                    pinToAppBarToolStripMenuItem.Enabled = false;
                    appSettingsToolStripMenuItem.Enabled = false;
                    tsAppSep2.Enabled = false;
                    reloadToolStripMenuItem.Enabled = true;
                    settingsToolStripMenuItem.Enabled = true;
                    break;
                case 1:
                    openANewSessionToolStripMenuItem.Visible = true;
                    tsAppSep1.Visible = true;
                    reloadToolStripMenuItem.Visible = true;
                    settingsToolStripMenuItem.Visible = true;
                    openANewSessionToolStripMenuItem.Enabled = true;
                    tsAppSep1.Enabled = true;
                    reloadToolStripMenuItem.Enabled = true;
                    settingsToolStripMenuItem.Enabled = true;
                    if (rcSender is ListViewItem)
                    {
                        var cntrl = rcSender as ListViewItem;
                        var app = cntrl.Tag as YorotApp;
                        if (app.AppCodeName == DefaultApps.WebBrowser.AppCodeName)
                        {
                            closeAllSessionsToolStripMenuItem.Enabled = true;
                            closeAllSessionsToolStripMenuItem.Visible = true;
                            appSettingsToolStripMenuItem.Visible = false;
                            pinToAppBarToolStripMenuItem.Visible = false;
                            tsAppSep2.Visible = false;
                            appSettingsToolStripMenuItem.Enabled = false;
                            pinToAppBarToolStripMenuItem.Enabled = false;
                            tsAppSep2.Enabled = false;
                        }
                        else
                        {
                            var hasSession = app.hasSessions();
                            closeAllSessionsToolStripMenuItem.Enabled = hasSession;
                            closeAllSessionsToolStripMenuItem.Visible = hasSession;
                            appSettingsToolStripMenuItem.Visible = true;
                            tsAppSep2.Visible = true;
                            if (app.AppCodeName == DefaultApps.Settings.AppCodeName)
                            {
                                pinToAppBarToolStripMenuItem.Visible = false;
                                pinToAppBarToolStripMenuItem.Enabled = false;
                            }
                            else
                            {
                                pinToAppBarToolStripMenuItem.Visible = true;
                                pinToAppBarToolStripMenuItem.Enabled = true;
                            }
                            appSettingsToolStripMenuItem.Visible = true;
                            tsAppSep2.Visible = true;
                            appSettingsToolStripMenuItem.Enabled = true;
                            tsAppSep2.Enabled = true;
                        }
                    }else
                    {
                        closeAllSessionsToolStripMenuItem.Enabled = false;
                        closeAllSessionsToolStripMenuItem.Visible = false;
                        pinToAppBarToolStripMenuItem.Visible = true;
                        appSettingsToolStripMenuItem.Visible = true;
                        tsAppSep2.Visible = true;
                        pinToAppBarToolStripMenuItem.Enabled = true;
                        appSettingsToolStripMenuItem.Enabled = true;
                        tsAppSep2.Enabled = true;
                    }
                    break;
                case 2:
                    openANewSessionToolStripMenuItem.Visible = true;
                    if (rcSender is YAMItem)
                    {
                        var hasSession = (rcSender as YAMItem).CurrentStatus != YAMItem.AppStatuses.Pinned;
                        closeAllSessionsToolStripMenuItem.Visible = hasSession;
                        closeAllSessionsToolStripMenuItem.Enabled = hasSession;
                    }
                    else
                    {
                        closeAllSessionsToolStripMenuItem.Enabled = false;
                        closeAllSessionsToolStripMenuItem.Visible = false;
                    }
                    tsAppSep1.Visible = true;
                    pinToAppBarToolStripMenuItem.Visible = true;
                    appSettingsToolStripMenuItem.Visible = true;
                    tsAppSep2.Visible = true;
                    reloadToolStripMenuItem.Visible = true;
                    settingsToolStripMenuItem.Visible = true;
                    openANewSessionToolStripMenuItem.Enabled = true;
                    tsAppSep1.Enabled = true;
                    pinToAppBarToolStripMenuItem.Enabled = true;
                    appSettingsToolStripMenuItem.Enabled = true;
                    tsAppSep2.Enabled = true;
                    reloadToolStripMenuItem.Enabled = true;
                    settingsToolStripMenuItem.Enabled = true;
                    break;
                case 3:
                    openANewSessionToolStripMenuItem.Visible = true;
                    closeAllSessionsToolStripMenuItem.Visible = true;
                    closeAllSessionsToolStripMenuItem.Enabled = true;
                    tsAppSep1.Visible = true;
                    pinToAppBarToolStripMenuItem.Visible = false;
                    appSettingsToolStripMenuItem.Visible = false;
                    tsAppSep2.Visible = false;
                    reloadToolStripMenuItem.Visible = false;
                    settingsToolStripMenuItem.Visible = true;
                    openANewSessionToolStripMenuItem.Enabled = true;
                    tsAppSep1.Enabled = true;
                    pinToAppBarToolStripMenuItem.Enabled = false;
                    appSettingsToolStripMenuItem.Enabled = false;
                    tsAppSep2.Enabled = false;
                    reloadToolStripMenuItem.Enabled = false;
                    settingsToolStripMenuItem.Enabled = true;
                    break;
                case 4:
                    openANewSessionToolStripMenuItem.Visible = false;
                    var hasSessions = YorotGlobal.Settings.AppMan.FindByAppCN(DefaultApps.Settings.AppCodeName).hasSessions();
                    closeAllSessionsToolStripMenuItem.Visible = hasSessions;
                    closeAllSessionsToolStripMenuItem.Enabled = hasSessions;
                    tsAppSep1.Visible = false;
                    pinToAppBarToolStripMenuItem.Visible = false;
                    appSettingsToolStripMenuItem.Visible = true;
                    tsAppSep2.Visible = true;
                    reloadToolStripMenuItem.Visible = false;
                    settingsToolStripMenuItem.Visible = true;
                    openANewSessionToolStripMenuItem.Enabled = false;
                    tsAppSep1.Enabled = false;
                    pinToAppBarToolStripMenuItem.Enabled = false;
                    appSettingsToolStripMenuItem.Enabled = true;
                    tsAppSep2.Enabled = true;
                    reloadToolStripMenuItem.Enabled = false;
                    settingsToolStripMenuItem.Enabled = true;
                    break;
            }
        }

        private void pbSettings_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (isCarbonCopy)
                {
                    YorotGlobal.Y1.MainForm.Invoke(new Action(() => { YorotGlobal.Y1.MainForm.pbSettings_MouseClick(this, e); YorotGlobal.Y1.MainForm.BringToFront(); }));
                }
                else
                {
                    if (pAppDrawer.Width < panelMaxSize)
                    {
                        AnimateTo(AnimateDirection.RightMost);
                    }
                    var settingApp = YorotGlobal.Settings.AppMan.FindByAppCN("com.haltroy.settings");
                    if (settingApp.Layouts.Count > 0)
                    {
                        var layout = settingApp.Layouts[0];
                        TabControl tabc = layout.AssocTab.Parent as TabControl;
                        if (tabc.InvokeRequired)
                        {
                            Invoke(new Action(() => tabc.SelectedTab = layout.AssocTab));
                        }
                        else
                        {
                            tabc.SelectedTab = layout.AssocTab;
                        }
                    }
                    else
                    {
                        UI.frmApp fapp /* pls dont laught at this we are not 4th graders */ = new UI.frmApp(settingApp) { assocForm = this, TopLevel = false, Visible = true, Dock = DockStyle.Fill, FormBorderStyle = FormBorderStyle.None };
                        showApp(fapp);
                    }
                }
            }else if (e.Button == MouseButtons.Right)
            {
                rcType = 4;
                rcSender = null;
                cmsApp.Show(MousePosition);
            }
        }

        private void pbYorot_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (tcAppMan.SelectedTab == tabPage1)
                {
                    AnimateTo(PrevDirection == AnimateDirection.Left ? AnimateDirection.RightMost : AnimateDirection.Left);
                }
                else
                {
                    allowSwitch = true;
                    tcAppMan.SelectedTab = tabPage1;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                rcType = 3;
                rcSender = null;
                cmsApp.Show(MousePosition);
            }
        }

        private void openANewSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rcType == 3)
            {
                frmMain main = new frmMain();
                main.Show();
            }
            else
            {
                if (rcType == 1) {
                    var app =  (rcSender as ListViewItem).Tag as YorotApp;
                    UI.frmApp fapp = new UI.frmApp(app) { assocForm = this, TopLevel = false, Visible = true, Dock = DockStyle.Fill, FormBorderStyle = FormBorderStyle.None };
                    showApp(fapp);
                }else
                {
                    var app = (rcSender as YAMItem).AssocApp;
                    UI.frmApp fapp = new UI.frmApp(app) { assocForm = this, TopLevel = false, Visible = true, Dock = DockStyle.Fill, FormBorderStyle = FormBorderStyle.None };
                    showApp(fapp, (rcSender as YAMItem));
                }
            }
        }

        private void closeAllSessionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rcType == 3)
            {
                foreach (YorotApp app in YorotGlobal.Settings.AppMan.Apps)
                {
                    YorotAppLayout[] layArray = app.Layouts.ToArray();
                    foreach (YorotAppLayout layout in layArray)
                    {
                        if (layout.AssocForm.InvokeRequired) { layout.AssocForm.Invoke(new Action(() => layout.AssocForm.Close())); } else { layout.AssocForm.Close(); }
                    }
                }
            }
            else if (rcType == 4)
            {
                var settingApp = YorotGlobal.Settings.AppMan.FindByAppCN("com.haltroy.settings");
                if (settingApp.Layouts.Count > 0)
                {
                    var layout = settingApp.Layouts[0];
                    if(layout.AssocForm.InvokeRequired) { layout.AssocForm.Invoke(new Action(()=> layout.AssocForm.Close())); } else { layout.AssocForm.Close(); }
                }
            }
            else
            {
                var app = rcType == 1 ? (rcSender as ListViewItem).Tag as YorotApp : (rcSender as YAMItem).AssocApp;
                YorotAppLayout[] layArray = app.Layouts.ToArray();
                foreach (YorotAppLayout layout in layArray)
                {
                    if (layout.AssocForm.InvokeRequired) { layout.AssocForm.Invoke(new Action(() => layout.AssocForm.Close())); } else { layout.AssocForm.Close(); }
                }
            }
        }

        private void pinToAppBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var app = rcType == 1 ? (rcSender as ListViewItem).Tag as YorotApp : (rcSender as YAMItem).AssocApp;
            app.isPinned = !app.isPinned;
            if (rcSender is YAMItem) 
            {
                var cntrl = (rcSender as YAMItem);
                if (!app.isPinned && cntrl.CurrentStatus == YAMItem.AppStatuses.Pinned)
                {
                    // TODO: Remove cntrl
                }
            }
        }

        private void appSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var app = rcType == 4 ? YorotGlobal.Settings.AppMan.FindByAppCN("com.haltroy.settings") : (rcType == 1 ? (rcSender as ListViewItem).Tag as YorotApp : (rcSender as YAMItem).AssocApp);
            // TODO: Open settings for this app.
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshAppList(false);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rcType == 0)
            {
                // TODO: Open Apps Menu Settings
            }
            else
            {
                pbSettings_MouseClick(settingsToolStripMenuItem, new MouseEventArgs(MouseButtons.Left, 1, MousePosition.X, MousePosition.Y, 0));
            }
        }

        private void frmMain_Resize(object sender, EventArgs e) => panelMaxSize = Width - 220;
        public bool isFullScreen { get; set; } = false;
        private FormWindowState prevState { get; set; } = FormWindowState.Normal;
        private AnimateDirection FSprev { get; set; } = AnimateDirection.Nothing;
        public void FullScreen(bool fs = false)
        {
            if (fs) // Switch to Full Screen
            {
                prevState = WindowState;
                Rectangle max = Screen.FromControl(this).Bounds;
                MaximizedBounds = max;
                MaximumSize = new Size(max.Width, max.Height);
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Normal;
                WindowState = FormWindowState.Maximized;
                AnimateTo(AnimateDirection.LeftFullScreen);
            }else // Switch to normal
            {
                MaximizedBounds = Screen.FromControl(this).WorkingArea;
                WindowState = FormWindowState.Normal;
                FormBorderStyle = FormBorderStyle.Sizable;
                WindowState = prevState;
                AnimateTo(FSprev == AnimateDirection.Nothing ? AnimateDirection.Left : FSprev);
            }
            isFullScreen = fs;
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F10) { FullScreen(!isFullScreen); }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (YorotGlobal.Y1.MainForms.Count == 1)
            {
                YorotGlobal.Settings.Save();
            }
        }
    }
}
