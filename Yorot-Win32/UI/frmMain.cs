using HTAlt;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Yorot
{
    public partial class frmMain : Form
    {

        #region Constructor

        bool isCarbonCopy = true;
        bool loadedAppList = false;

        public frmMain() : this(false) { }
        public frmMain(bool isMainSession)
        {
            InitializeComponent();
            pAppDrawer.Width = panelMinSize;
            isCarbonCopy = !isMainSession;
            YorotGlobal.Main.MainForms.Add(this);
            LoadTheme(true);
            loadPinnedApps(true);
            if (!isCarbonCopy) { RefreshAppList(true); loadedAppList = true; }
            else
            {
                loadedAppList = false;
                label2.Visible = true;
                label2.Enabled = true;
                htButton1.Visible = true;
                htButton1.Enabled = true;
            }
            LoadLang(true);
        }
        private List<YorotApp> loadedApps;
        private void loadPinnedApps(bool force = false)
        {
            var l = YorotGlobal.Main.AppMan.Apps.FindAll(it => it.isPinned);
            List<YorotApp> _n;
            if (force || loadedApps != l)
            {
                if (loadedApps is null)
                {
                    _n = l;
                }
                else
                {
                    var newApps = l.Except(loadedApps);
                    _n = newApps.ToList();
                }
            }else
            {
                _n = null;
            }
            if (_n != null)
            {
                loadedApps = l;
                for (int i = 0; i < _n.Count; i++)
                {
                    var a = _n[i];
                    YAMItem item = new YAMItem()
                    {
                        Size = new Size(38, 38),
                        Margin = new Padding(3),
                        Visible = true,
                        AssocApp = a,
                        AssocFrmMain = this,
                    };
                    item.MouseClick += pbIcon_MouseClick;
                    flpFavApps.Controls.Add(item);
                }
            }
        }
        private string UnpinAppText = "Unpin this app";
        private string PinAppText = "Pin this app";
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
        private void frmMain_Load(object sender, EventArgs e)
        {
            
        }
        int appListUpdate = 0;
        private string loadedTheme = string.Empty;
        private string loadedLang = string.Empty;
        private void tmrAppSync_Tick(object sender, EventArgs e)
        {
            if (appListUpdate != YorotGlobal.Main.AppMan.UpdateCount)
            {
                RefreshAppList(false);
            }
            // These both if() statements do diffrenet jobs, so that SideCreekGiant cards won't work on here.
            if (isFullScreen && PrevDirection != AnimateDirection.LeftFullScreen && !AnimationContinue)
            {
                AnimateTo(AnimateDirection.LeftFullScreen);
            }
            LoadTheme();
            loadPinnedApps();
            
        }
        private void LoadLang(bool force = false)
        {
            var l = YorotGlobal.Main.CurrentLanguage;
            if (force || loadedLang != l.CodeName)
            {
                loadedLang = l.CodeName;
                label2.Text = l.GetItemText("Win32.MainForm.NotLoadedApps");
                htButton1.Text = l.GetItemText("Win32.MainForm.LoadAppButton");
                PinAppText = l.GetItemText("Win32.MainForm.PinApp");
                UnpinAppText = l.GetItemText("Win32.MainForm.UnpinApp");
                openANewSessionToolStripMenuItem.Text = l.GetItemText("Win32.MainForm.OpenNewSession");
                closeAllSessionsToolStripMenuItem.Text = l.GetItemText("Win32.MainForm.CloseAllSessions");
                appSettingsToolStripMenuItem.Text = l.GetItemText("Win32.MainForm.AppSettings");
                reloadToolStripMenuItem.Text = l.GetItemText("Win32.MainForm.AppListReload");
                settingsToolStripMenuItem.Text = l.GetItemText("Win32.MainForm.ListSettings");
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
            }
        }
        private void LoadTheme(bool force = false)
        {
            var theme = YorotGlobal.Main.CurrentTheme;
            var themeid = theme.BackColor.ToHex() + "-" + theme.ForeColor.ToHex() + "-" + theme.OverlayColor.ToHex() + "-" + theme.ArtColor.ToHex();
            if (force || loadedTheme != themeid)
            {
                loadedTheme = themeid;
                BackColor = theme.BackColor;
                ForeColor = theme.ForeColor;
                label2.BackColor = theme.BackColor2;
                label2.ForeColor = theme.ForeColor;
                htButton1.BackColor = theme.BackColor3;
                htButton1.ForeColor = theme.ForeColor;
                pAppGrid.BackColor = theme.BackColor2;
                pAppGrid.ForeColor = theme.ForeColor;
                lvApps.BackColor = theme.BackColor2;
                lvApps.ForeColor = theme.ForeColor;
                label1.BackColor = theme.OverlayColor;
                for (int i = 0; i < flpFavApps.Controls.Count; i++)
                {
                    var c = flpFavApps.Controls[i] as YAMItem;
                    c.BackColor = theme.BackColor2;
                    c.ForeColor = theme.ForeColor;
                    c.OverlayColor = theme.OverlayColor;
                }
                for (int i = 0; i < tcAppMan.TabPages.Count;i++)
                {
                    var t = tcAppMan.TabPages[i];
                    t.BackColor = theme.BackColor;
                    t.ForeColor = theme.ForeColor;
                }
                cmsApp.BackColor = theme.BackColor3;
                cmsApp.ForeColor = theme.ForeColor;
            }
        }
        public void RefreshAppList(bool clearCurrent = false)
        {
            appListUpdate = YorotGlobal.Main.AppMan.UpdateCount;
            if (clearCurrent)
            {
                lvApps.Items.Clear();
                foreach (YorotApp kapp in YorotGlobal.Main.AppMan.Apps)
                {
                    ilAppMan.Images.Add(Tools.GenerateAppIcon(YorotTools.GetAppIcon(kapp), "#808080".HexToColor()));
                    ListViewItem item = new ListViewItem()
                    {
                        Text = kapp.AppName,
                        ToolTipText = kapp.AppCodeName,
                        ImageIndex = ilAppMan.Images.Count - 1,
                        Tag = kapp,
                    };
                    switch (kapp.AppCodeName.ToLowerEnglish())
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
                lvApps.Items.Insert(0, yorot_app);
            }else
            {
                foreach(YorotApp yapp in YorotGlobal.Main.AppMan.Apps)
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
                        ilAppMan.Images.Add(Yorot.Tools.GenerateAppIcon(YorotTools.GetAppIcon(yapp), "#808080".HexToColor()));
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

        internal void openDrawer()
        {
            if (pAppDrawer.Width < panelMaxSize)
            {
                AnimateTo(AnimateDirection.RightMost);
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

        bool allowSwitch = false;
        private void tcAppMan_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (allowSwitch) { allowSwitch = false; } else { e.Cancel = true; }
        }
        List<TabPage> tabHistory = new List<TabPage>();
        public void switchTab(TabPage tp)
        {
            if (tcAppMan.SelectedTab == tp) return;
            if (tcAppMan.SelectedTab != null) tabHistory.Add(tcAppMan.SelectedTab);
            allowSwitch = true;
            if (!tcAppMan.TabPages.Contains(tp)) tcAppMan.TabPages.Add(tp);
            tcAppMan.SelectedTab = tp;
        }
        public void switchTabGoBack()
        {
            allowSwitch = true;
            tcAppMan.SelectedTab = tabHistory[tabHistory.Count - 1];
            tabHistory.RemoveAt(tabHistory.Count - 1);
        }
        public void LaunchApp(YorotApp app, string[] args = null)
        {
            if (app.Layouts.Count > 0)
            {
                var layout = app.Layouts[0] as WinAppLayout;
                if (layout.AssocForm.freeMode)
                {
                    layout.AssocForm.Show();
                    layout.AssocForm.BringToFront();
                }
                else
                {
                    TabControl tabc = layout.AssocTab.Parent as TabControl;
                    var frm = tabc.FindForm() as frmMain;
                    if (frm.InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            frm.switchTab(layout.AssocTab);
                        }));
                    }
                    else
                    {
                        frm.switchTab(layout.AssocTab);
                    }
                }
            }
            else
            {
                showApp(app,args);
            }
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
                    LaunchApp(app);
                }
                foreach(Control x in flpFavApps.Controls)
                {
                    x.Refresh();
                }
            }
        }
        private void showApp(YorotApp app, string[] args = null, YAMItem sender = null)
        {
            TabPage tp = new TabPage() { Text = app.AppName };
            WinAppLayout layout = new WinAppLayout()
            {
                AssocTab = tp,
            };
            if (app.AppCodeName == "com.haltroy.settings")
            {
                layout.Args = string.IsNullOrWhiteSpace(settingsArgs) ? layout.Args : settingsArgs.Split(' ');
                settingsArgs = string.Empty;
            }else
            {
                layout.Args = args;
            }
            UI.frmApp fapp = new UI.frmApp(app,layout) { assocLayout = layout, assocForm = this, TopLevel = false, Visible = true, Dock = DockStyle.Fill, FormBorderStyle = FormBorderStyle.None };
            layout.AssocForm = fapp;
            if (sender == null)
            {
                fapp.assocApp.Layouts.Add(layout);
                fapp.assocLayout = layout;
                tp.Controls.Add(fapp);
                switchTab(tp);
                if (fapp.assocApp.AppCodeName != "com.haltroy.settings")
                {
                    YAMItem pbIcon = new YAMItem() { Size = new Size(38, 38), Margin = new Padding(3), Visible = true, AssocApp = fapp.assocApp, AssocFrmMain = this };
                    pbIcon.MouseClick += pbIcon_MouseClick;
                    layout.AssocItem = pbIcon;
                    flpFavApps.Controls.Add(pbIcon); pbIcon.Refresh();
                }
            }else
            {
                fapp.assocApp.Layouts.Add(layout);
                fapp.assocLayout = layout;
                tp.Controls.Add(fapp);
                switchTab(tp);
                layout.AssocItem = sender;
                if (fapp.assocLayout.AssocItem.InvokeRequired) 
                { fapp.assocLayout.AssocItem.Invoke(new Action(() => fapp.assocLayout.AssocItem.Refresh())); } else { fapp.assocLayout.AssocItem.Refresh(); }
            }
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
                switchTab(fapp.assocLayout.AssocTab);
            }
        }
        private void pbIcon_MouseClick(object sender, MouseEventArgs e)
        {
            YAMItem pbIcon = sender as YAMItem;
            if (e.Button == MouseButtons.Left)
            {
                if (pbIcon.CurrentStatus == YAMItem.AppStatuses.Pinned) // Launch app
                {
                    showApp(pbIcon.AssocApp,null, pbIcon);
                }
                else
                {
                    var layout = pbIcon.AssocApp.Layouts[pbIcon.FocusedLayoutIndex] as WinAppLayout;
                    if (layout.AssocForm.ContainsFocus || layout.AssocForm.Focused)
                    {
                        // shift between sessions
                        pbIcon.FocusedLayoutIndex = pbIcon.FocusedLayoutIndex < pbIcon.AssocApp.Layouts.Count - 1 ? pbIcon.FocusedLayoutIndex + 1 : 0;

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
            loadedAppList = true;
            RefreshAppList(true);
            LoadLang(true);
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
                                pinToAppBarToolStripMenuItem.Text = app.isPinned ? UnpinAppText : PinAppText;
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
                        pinToAppBarToolStripMenuItem.Text = PinAppText;
                        tsAppSep2.Visible = true;
                        pinToAppBarToolStripMenuItem.Enabled = true;
                        appSettingsToolStripMenuItem.Enabled = true;
                        tsAppSep2.Enabled = true;
                    }
                    break;
                case 2:
                    openANewSessionToolStripMenuItem.Visible = true;
                    pinToAppBarToolStripMenuItem.Text = PinAppText;
                    if (rcSender is YAMItem)
                    {
                        var hasSession = (rcSender as YAMItem).CurrentStatus != YAMItem.AppStatuses.Pinned;
                        closeAllSessionsToolStripMenuItem.Visible = hasSession;
                        closeAllSessionsToolStripMenuItem.Enabled = hasSession;
                        pinToAppBarToolStripMenuItem.Text = (rcSender as YAMItem).AssocApp.isPinned ? UnpinAppText : PinAppText;
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
                    var hasSessions = YorotGlobal.Main.AppMan.FindByAppCN(DefaultApps.Settings.AppCodeName).hasSessions();
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
                    YorotGlobal.Main.MainForm.Invoke(new Action(() => { YorotGlobal.Main.MainForm.settingsArgs = settingsArgs; YorotGlobal.Main.MainForm.pbSettings_MouseClick(this, e); YorotGlobal.Main.MainForm.BringToFront(); }));
                }
                else
                {
                    if (pAppDrawer.Width < panelMaxSize)
                    {
                        AnimateTo(AnimateDirection.RightMost);
                    }
                    var settingApp = YorotGlobal.Main.AppMan.FindByAppCN("com.haltroy.settings");
                    if (settingApp.Layouts.Count > 0)
                    {
                        var layout = settingApp.Layouts[0] as WinAppLayout;
                        TabControl tabc = layout.AssocTab.Parent as TabControl;
                        frmMain form = tabc.Parent.Parent as frmMain;
                        if (form.InvokeRequired)
                        {
                            form.Invoke(new Action(() => form.switchTab(layout.AssocTab)));
                        }
                        else
                        {
                            form.switchTab(layout.AssocTab);
                        }
                    }
                    else
                    {
                        showApp(settingApp);
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
                    switchTab(tabPage1);
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
                    showApp((rcSender as ListViewItem).Tag as YorotApp);
                }else
                {
                    showApp((rcSender as YAMItem).AssocApp,null, (rcSender as YAMItem));
                }
            }
        }

        private void closeAllSessionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rcType == 3)
            {
                foreach (YorotApp app in YorotGlobal.Main.AppMan.Apps)
                {
                    YorotAppLayout[] layArray = app.Layouts.ToArray();
                    foreach (YorotAppLayout layout in layArray)
                    {
                        var wl = layout as WinAppLayout;
                        if (wl.AssocForm.InvokeRequired) { wl.AssocForm.Invoke(new Action(() => wl.AssocForm.Close())); } else { wl.AssocForm.Close(); }
                    }
                }
            }
            else if (rcType == 4)
            {
                var settingApp = YorotGlobal.Main.AppMan.FindByAppCN("com.haltroy.settings");
                if (settingApp.Layouts.Count > 0)
                {
                    var layout = settingApp.Layouts[0] as WinAppLayout;
                    if(layout.AssocForm.InvokeRequired) { layout.AssocForm.Invoke(new Action(()=> layout.AssocForm.Close())); } else { layout.AssocForm.Close(); }
                }
            }
            else
            {
                var app = rcType == 1 ? (rcSender as ListViewItem).Tag as YorotApp : (rcSender as YAMItem).AssocApp;
                YorotAppLayout[] layArray = app.Layouts.ToArray();
                foreach (YorotAppLayout layout in layArray)
                {
                    var wl = layout as WinAppLayout;
                    if (wl.AssocForm.InvokeRequired) { wl.AssocForm.Invoke(new Action(() => wl.AssocForm.Close())); } else { wl.AssocForm.Close(); }
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
                    flpFavApps.Controls.Remove(cntrl);
                    loadedApps.Remove(app);
                }else
                {
                    loadedApps.Add(app);
                    YAMItem item = new YAMItem()
                    {
                        Size = new Size(38, 38),
                        Margin = new Padding(3),
                        Visible = true,
                        AssocApp = app,
                        AssocFrmMain = this,
                    };
                    item.MouseClick += pbIcon_MouseClick;
                    flpFavApps.Controls.Add(item);
                    flpFavApps.Refresh();
                }
            }
        }

        private void appSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var app = rcType == 4 ? YorotGlobal.Main.AppMan.FindByAppCN("com.haltroy.settings") : (rcType == 1 ? (rcSender as ListViewItem).Tag as YorotApp : (rcSender as YAMItem).AssocApp);
            settingsArgs = "Apps:" + app.AppCodeName;
            pbSettings_MouseClick(settingsToolStripMenuItem, new MouseEventArgs(MouseButtons.Left, 1, MousePosition.X, MousePosition.Y, 0));
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshAppList(false);
        }
        private string settingsArgs = string.Empty;
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rcType == 0)
            {
                settingsArgs = "Apps:Menu";
                pbSettings_MouseClick(settingsToolStripMenuItem, new MouseEventArgs(MouseButtons.Left, 1, MousePosition.X, MousePosition.Y, 0));
            }
            else
            {
                settingsArgs = "";
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
            if (YorotGlobal.Main.MainForms.Count == 1)
            {
                YorotGlobal.Main.Shutdown();
            }
        }

        
    }
}
