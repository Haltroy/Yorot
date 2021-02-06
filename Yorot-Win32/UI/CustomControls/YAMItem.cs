using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HTAlt;

namespace Yorot
{
    public class YAMItem : Control
    {
        private Color overlayColor = System.Drawing.Color.FromArgb(255, 64, 128, 255);
        private YorotApp assocApp;
        private int focusedLayoutIndex = 0;
        public enum AppStatuses
        {
            Pinned,
            Normal,
            Multiple
        }

        public AppStatuses CurrentStatus => AssocApp.Layouts.Count > 0 ? (assocApp == null ? true : AssocApp.Layouts.Count > 1) ? AppStatuses.Multiple : AppStatuses.Normal : AppStatuses.Pinned;

        public System.Drawing.Color OverlayColor
        {
            get => overlayColor;
            set
            {
                overlayColor = value;
                Refresh();
            }
        }
        private System.Drawing.Color bc2 => BackColor.ShiftBrightness(20, false);
        private System.Drawing.Color oc2 => OverlayColor.ShiftBrightness(20, false);
        public bool HasFocus => assocApp == null ? true : (AssocApp.Layouts.FindAll(i => i is WinAppLayout && ((i as WinAppLayout).AssocForm.Focused || (i as WinAppLayout).AssocForm.ContainsFocus)).Count > 0);
        public YorotApp AssocApp
        {
            get => assocApp; set
            {
                assocApp = value;
                Refresh();
            }
        }
        public frmMain AssocFrmMain { get; set; }
        public int FocusedLayoutIndex { get => focusedLayoutIndex; set {
                focusedLayoutIndex = value;
                Refresh();
            } }
        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);

            // Draw background
            e.Graphics.FillRectangle(new System.Drawing.SolidBrush(HasFocus ? bc2 : BackColor), Bounds);

            // Draw Icon
            e.Graphics.DrawImage(HTAlt.Tools.ResizeImage(assocApp == null ? Properties.Resources.Yorot : YorotTools.GetAppIcon(assocApp), 32, 32), new Rectangle(5,5,32,32));

            if (assocApp != null)
            {
                // Draw session indicator
                if (assocApp.Layouts.Count > 0)
                {
                    // Fix our layout
                    if ((assocApp.Layouts.Count <= focusedLayoutIndex) || (focusedLayoutIndex < 0))
                    {
                        FocusedLayoutIndex = 0;
                    }
                    switch(CurrentStatus)
                    {
                        case AppStatuses.Pinned:
                            break;
                        case AppStatuses.Normal:
                            e.Graphics.FillRectangle(new System.Drawing.SolidBrush(HasFocus ? OverlayColor : oc2), new System.Drawing.Rectangle(0, 0, 4, Height));
                            break;
                        case AppStatuses.Multiple:
                            e.Graphics.FillRectangle(new System.Drawing.SolidBrush(OverlayColor), new System.Drawing.Rectangle(0, 0, 4, Height / 2));
                            e.Graphics.FillRectangle(new System.Drawing.SolidBrush(oc2), new System.Drawing.Rectangle(0, Height / 2, 4, Height / 2));
                            break;
                    }
                }
            }else
            {
                e.Graphics.FillRectangle(new System.Drawing.SolidBrush(OverlayColor), new System.Drawing.Rectangle(0, 0, 3, Height / 2));
                e.Graphics.FillRectangle(new System.Drawing.SolidBrush(OverlayColor.ShiftBrightness(20, false)), new System.Drawing.Rectangle(0, Height / 2, 4, Height / 2));
            }
        }
    }
    public class WinAppLayout : YorotAppLayout
    {
        /// <summary>
        /// Gets associated <see cref="UI.frmApp"/> of this <see cref="YorotApp"/>. <see cref="null"/> if no forms are associated.
        /// </summary>
        public UI.frmApp AssocForm { get; set; }
        /// <summary>
        /// Gets associated <see cref="System.Windows.Forms.TabPage"/> of this <see cref="YorotApp"/>. <see cref="null"/> if no tabs are associated.
        /// </summary>
        public System.Windows.Forms.TabPage AssocTab { get; set; }
        /// <summary>
        /// Gets associated <see cref="YAMItem"/> of this <see cref="YorotApp"/>. <see cref="null"/> if no YAMIs are associated.
        /// </summary>
        public YAMItem AssocItem { get; set; }
        public new bool hasSessions
        {
            get => AssocForm != null;
        }
    }
}
