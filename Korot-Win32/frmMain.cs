using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HTAlt;

namespace Korot_Win32
{
    public partial class frmMain : Form
    {

        #region Constructor

        public frmMain()
        {
            if (KorotGlobal.Settings == null)
            {
                KorotGlobal.Settings = new Settings();
            }
            InitializeComponent();
            RefreshAppList(true);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void RefreshAppList(bool clearCurrent = false)
        {
            if (clearCurrent) { lvApps.Items.Clear(); }
            foreach (KorotApp kapp in KorotGlobal.Settings.AppMan.Apps)
            {
                ilAppMan.Images.Add(KorotGlobal.GenerateAppIcon(kapp.GetAppIcon(), "#808080".HexToColor()));
                ListViewItem item = new ListViewItem()
                {
                    Text = kapp.AppName,
                    ToolTipText = kapp.AppCodeName,
                    ImageIndex = ilAppMan.Images.Count - 1,
                    Tag = kapp,
                };
                lvApps.Items.Add(item);
            }

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
            AnimationContinue = true;
            Direction = animate;
            timer1.Start();
        }

        private AnimateDirection Direction = AnimateDirection.Nothing;
        private AnimateDirection PrevDirection = AnimateDirection.Left;
        private int AnimationSpeed = 60;
        private int panelMinSize = 60;
        private int panelMaxSize = 420;
        private bool AnimationContinue = false;
        private bool isAnimating => Direction != AnimateDirection.Nothing;

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (AnimationContinue)
            {
                case true:
                    if(pAppDrawer.Dock != DockStyle.Left)
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
                            switch(PrevDirection)
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
                            pAppDrawer.Width = pAppDrawer.Width < (Width - 15) ? pAppDrawer.Width + AnimationSpeed : (Width - 15);
                            AnimationContinue = pAppDrawer.Width < (Width - 15);
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


        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (lvApps.SelectedItems.Count > 0)
            {
                var appItem = lvApps.SelectedItems[0];
                var app = appItem.Tag as KorotApp;
                var appcn = appItem.ToolTipText;
                bool isSystemApp = DefaultApps.isSystemApp(appcn);
                if (isSystemApp)
                {

                }
            }
        }
        private void showApp(Form app)
        {
        }
    }
}
