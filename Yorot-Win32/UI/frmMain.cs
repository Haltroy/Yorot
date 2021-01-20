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
            if (!isCarbonCopy) { RefreshAppList(true); YorotGlobal.Settings.MainForm = this; }  else
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


        private void RefreshAppList(bool clearCurrent = false)
        {
            if (clearCurrent) { lvApps.Items.Clear(); }
            ListViewItem yorotApp = null;
            foreach (YorotApp kapp in YorotGlobal.Settings.AppMan.Apps)
            {
                ilAppMan.Images.Add(YorotGlobal.GenerateAppIcon(kapp.GetAppIcon(), "#808080".HexToColor()));
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
        }

        #endregion Constructor

        #region Animator

        private bool RightMostClosing = false;

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
            /// Expand a little
            /// </summary>
            Right,
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
            Direction = animate;
            timer1.Start();
        }

        private AnimateDirection Direction = AnimateDirection.Nothing;
        private AnimateDirection PrevDirection = AnimateDirection.Left;
        private readonly int AnimationSpeed = 60;
        private readonly int panelMinSize = 60;
        private readonly int panelMaxSize = 420;
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
                                case AnimateDirection.Right:
                                    pAppDrawer.Width = pAppDrawer.Width > panelMinSize ? pAppDrawer.Width - AnimationSpeed : panelMinSize;
                                    AnimationContinue = pAppDrawer.Width > panelMinSize;
                                    break;
                                case AnimateDirection.LeftFullScreen:
                                    pAppDrawer.Width = pAppDrawer.Width < panelMinSize ? pAppDrawer.Width + AnimationSpeed : panelMinSize;
                                    AnimationContinue = pAppDrawer.Width < panelMinSize;
                                    break;
                                case AnimateDirection.Left:
                                    pAppDrawer.Width = panelMinSize;
                                    AnimationContinue = false;
                                    break;
                            }
                            break;
                        case AnimateDirection.LeftFullScreen:
                            pAppDrawer.Width = pAppDrawer.Width > 10 ? pAppDrawer.Width - AnimationSpeed : pAppDrawer.Width;
                            AnimationContinue = pAppDrawer.Width > 10;
                            break;
                        case AnimateDirection.Right:
                            switch (PrevDirection)
                            {
                                case AnimateDirection.LeftFullScreen:
                                case AnimateDirection.Left:
                                    pAppDrawer.Width = pAppDrawer.Width < panelMaxSize ? pAppDrawer.Width + AnimationSpeed : panelMaxSize;
                                    AnimationContinue = pAppDrawer.Width < panelMaxSize;
                                    break;
                                case AnimateDirection.RightMost:
                                    pAppDrawer.Width = pAppDrawer.Width > panelMaxSize ? pAppDrawer.Width - AnimationSpeed : panelMaxSize;
                                    AnimationContinue = pAppDrawer.Width > panelMaxSize;
                                    break;
                                case AnimateDirection.Right:
                                    pAppDrawer.Width = panelMaxSize;
                                    AnimationContinue = false;
                                    break;
                            }
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
                            pAppDrawer.Width = 10;
                            break;
                        case AnimateDirection.Right:
                            pAppDrawer.Width = panelMaxSize;
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
        private void pbYorot_Click(object sender, EventArgs e)
        {
            
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
                        RightMostClosing = false;
                        AnimateTo(AnimateDirection.Right);
                        break;
                    case AnimateDirection.Right:
                        AnimateTo(RightMostClosing ? AnimateDirection.Left : AnimateDirection.RightMost);
                        RightMostClosing = false;
                        break;
                    case AnimateDirection.RightMost:
                        RightMostClosing = true;
                        AnimateTo(AnimateDirection.Right);
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
                    AnimateTo(PrevDirection == AnimateDirection.RightMost ? AnimateDirection.Right : AnimateDirection.RightMost);
                }
                else if (pAppDrawer.Width < panelMaxSize)
                {
                    AnimateTo(PrevDirection == AnimateDirection.Right ? AnimateDirection.Left : AnimateDirection.Right);
                }
                else if (pAppDrawer.Width == panelMaxSize)
                {
                    AnimateTo(PrevDirection == AnimateDirection.RightMost ? AnimateDirection.Left : AnimateDirection.RightMost);
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
                } 
                else
                {
                    if (app.AssocTab == null)
                    {
                        UI.frmApp fapp /* pls dont laught at this we are not 4th graders */ = new UI.frmApp(app) { assocForm = this, TopLevel = false, Visible = true, Dock = DockStyle.Fill, FormBorderStyle = FormBorderStyle.None };
                        app.AssocForm = fapp;
                        showApp(fapp);
                    }
                    else
                    {
                        if (app.AssocForm.freeMode) 
                        {
                            app.AssocForm.Show();
                            app.AssocForm.BringToFront();
                        }
                        else
                        {
                            TabControl tabc = app.AssocTab.Parent as TabControl;
                            if (tabc.InvokeRequired)
                            {
                                Invoke(new Action(() => { allowSwitch = true; tabc.SelectedTab = app.AssocTab; }));
                            }
                            else
                            {
                                allowSwitch = true;
                                tabc.SelectedTab = app.AssocTab;
                            }
                        }
                    }
                }
            }
        }
        private void showApp(UI.frmApp app)
        {
            TabPage tp = new TabPage() { Text = app.assocApp.AppName };
            app.assocApp.AssocTab = tp;
            tp.Controls.Add(app);
            tcAppMan.TabPages.Add(tp);
            allowSwitch = true;
            tcAppMan.SelectedTab = tp;
            if (app.assocApp.AppCodeName != "com.haltroy.settings")
            {
                PictureBox pbIcon = new PictureBox() { SizeMode = PictureBoxSizeMode.Zoom, Image = app.assocApp.GetAppIcon(), Size = new Size(32, 32), Margin = new Padding(5), Visible = true, Tag = app };
                pbIcon.MouseClick += pbIcon_MouseClick;
                app.assocApp.AssocPB = pbIcon;
                flpFavApps.Controls.Add(pbIcon);
            }
        }
        private void pbIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (pAppDrawer.Width < panelMaxSize)
                {
                    AnimateTo(AnimateDirection.Right);
                }
                PictureBox pbIcon = sender as PictureBox;
                var fapp = pbIcon.Tag as UI.frmApp;
                if (fapp.freeMode)
                {
                    fapp.Show();
                    fapp.BringToFront();
                }
                else
                {
                    allowSwitch = true;
                    tcAppMan.SelectedTab = fapp.assocApp.AssocTab;
                }
            }else if (e.Button == MouseButtons.Right)
            {
                PictureBox pbIcon = sender as PictureBox;
                var fapp = pbIcon.Tag as UI.frmApp;
                var app = fapp.assocApp;
                rcSender = app;
                rcType = 2;
                cmsApp.Show(MousePosition);
            }
        }

        public void pbSettings_Click(object sender, EventArgs e)
        {
           
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
                    if (rcSender is YorotApp)
                    {
                        var app = rcSender as YorotApp;
                        var hasSession = app.hasSessions();
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
                    openANewSessionToolStripMenuItem.Visible = true;
                    closeAllSessionsToolStripMenuItem.Visible = YorotGlobal.Settings.AppMan.FindByAppCN(DefaultApps.Settings.AppCodeName).hasSessions();
                    closeAllSessionsToolStripMenuItem.Enabled = closeAllSessionsToolStripMenuItem.Visible;
                    tsAppSep1.Visible = true;
                    pinToAppBarToolStripMenuItem.Visible = false;
                    appSettingsToolStripMenuItem.Visible = true;
                    tsAppSep2.Visible = true;
                    reloadToolStripMenuItem.Visible = false;
                    settingsToolStripMenuItem.Visible = true;
                    openANewSessionToolStripMenuItem.Enabled = true;
                    tsAppSep1.Enabled = true;
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
                    YorotGlobal.Settings.MainForm.Invoke(new Action(() => { YorotGlobal.Settings.MainForm.pbSettings_Click(this, e); YorotGlobal.Settings.MainForm.BringToFront(); }));
                }
                else
                {
                    if (pAppDrawer.Width < panelMaxSize)
                    {
                        AnimateTo(AnimateDirection.Right);
                    }
                    var settingApp = YorotGlobal.Settings.AppMan.FindByAppCN("com.haltroy.settings");
                    if (settingApp.AssocTab == null)
                    {
                        UI.frmApp fapp /* pls dont laught at this we are not 4th graders */ = new UI.frmApp(settingApp) { assocForm = this, TopLevel = false, Visible = true, Dock = DockStyle.Fill, FormBorderStyle = FormBorderStyle.None };
                        settingApp.AssocForm = fapp;
                        showApp(fapp);
                    }
                    else
                    {
                        TabControl tabc = settingApp.AssocTab.Parent as TabControl;
                        if (tabc.InvokeRequired)
                        {
                            Invoke(new Action(() => tabc.SelectedTab = settingApp.AssocTab));
                        }
                        else
                        {
                            tabc.SelectedTab = settingApp.AssocTab;
                        }
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
                    if (pAppDrawer.Width == panelMaxSize)
                    {
                        AnimateTo(PrevDirection == AnimateDirection.Right ? AnimateDirection.Left : AnimateDirection.RightMost);
                    }
                    else
                    {
                        AnimateTo(PrevDirection == AnimateDirection.Right ? AnimateDirection.Left : AnimateDirection.Right);
                    }
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
                var app = rcType == 1 ? (rcSender as ListViewItem).Tag as YorotApp : rcSender as YorotApp;
            }
        }
    }
}
