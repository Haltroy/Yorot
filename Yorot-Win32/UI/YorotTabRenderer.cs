/*

Copyright © 2020 Eren "Haltroy" Kanat

Use of this source code is governed by an MIT License that can be found in github.com/Haltroy/Yorot/blob/master/LICENSE

*/

using EasyTabs;
using HTAlt;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Win32Interop.Enums;

namespace Yorot
{
    public class YorotTabRenderer : BaseTabRenderer
    {
        private readonly WindowsSizingBoxes _windowsSizingBoxes = null;
        private readonly Font _captionFont = null;
        public Color BackColor = Color.White;
        public Color OverlayBackColor = Color.White;
        public Color OverlayColor = Color.FromArgb(255, 71, 191, 255);

        public YorotTabRenderer(TitleBarTabs parentWindow) : base(parentWindow)
        {
            // Initialize the various images to use during rendering
            /*
            _activeLeftSideImage = Properties.Resources.Left;
            _activeRightSideImage = Properties.Resources.Right;
            _activeCenterImage = Properties.Resources.Center;
            _inactiveLeftSideImage = Properties.Resources.InactiveLeft;
            _inactiveRightSideImage = Properties.Resources.InactiveRight;
            _inactiveCenterImage = Properties.Resources.InactiveCenter;
            _closeButtonImage = Properties.Resources.Close;
            _closeButtonHoverImage = Properties.Resources.CloseHover;
            _background = null;
            _addButtonImage = new Bitmap(Properties.Resources.Add);
            _addButtonHoverImage = new Bitmap(Properties.Resources.AddHover);
            */

            // Set the various positioning properties
            CloseButtonMarginTop = 9;
            CloseButtonMarginLeft = 2;
            CloseButtonMarginRight = 4;
            AddButtonMarginTop = 3;
            AddButtonMarginLeft = 2;
            CaptionMarginTop = 9;
            IconMarginLeft = 9;
            IconMarginTop = 9;
            IconMarginRight = 5;
            AddButtonMarginRight = 45;

            _windowsSizingBoxes = new YorotWSB(parentWindow);
            _captionFont = new Font("Ubuntu", 9);

            if (_captionFont.Name != "Ubuntu")
            {
                _captionFont = new Font(SystemFonts.CaptionFont.Name, 9);
            }
        }

        public void ApplyColors(Color bColor, Color fColor, Color oColor, Color obColor)
        {
            BackColor = bColor;
            ForeColor = fColor;
            OverlayColor = oColor;
            OverlayBackColor = obColor;
        }

        public void Redraw()
        {
            _parentWindow.RedrawTabs();
        }

        public override Font CaptionFont => _captionFont;

        public override int TabHeight => _parentWindow.WindowState == FormWindowState.Maximized ? base.TabHeight : base.TabHeight + TopPadding;

        public override int TopPadding => _parentWindow.WindowState == FormWindowState.Maximized ? 0 : 8;

        /// <summary>Since Chrome tabs overlap, we set this property to the amount that they overlap by.</summary>
        public override int OverlapWidth => 14;

        public override bool RendersEntireTitleBar => IsWindows10;

        public override bool IsOverSizingBox(Point cursor)
        {
            return _windowsSizingBoxes.Contains(cursor);
        }

        public override HT NonClientHitTest(Message message, Point cursor)
        {
            HT result = _windowsSizingBoxes.NonClientHitTest(cursor);
            return result == HT.HTNOWHERE ? HT.HTCAPTION : result;
        }

        public override void Render(List<TitleBarTab> tabs, Graphics graphicsContext, Point offset, Point cursor, bool forceRedraw = false)
        {
            if (_suspendRendering || tabs == null || tabs.Count == 0)
            {
                return;
            }

            Point screenCoordinates = _parentWindow.PointToScreen(_parentWindow.ClientRectangle.Location);

            // Calculate the maximum tab area, excluding the add button and any minimize/maximize/close buttons in the window
            _maxTabArea.Location = new Point(SystemInformation.BorderSize.Width + offset.X + screenCoordinates.X, offset.Y + screenCoordinates.Y);
            _maxTabArea.Width = GetMaxTabAreaWidth(tabs, offset);
            _maxTabArea.Height = TabHeight;

            // Get the width of the content area for each tab by taking the parent window's client width, subtracting the left and right border widths and the
            // add button area (if applicable) and then dividing by the number of tabs
            int tabContentWidth = Math.Min(_activeCenterImage.Width, Convert.ToInt32(Math.Floor(Convert.ToDouble(_maxTabArea.Width / tabs.Count))));

            // Determine if we need to redraw the TabImage properties for each tab by seeing if the content width that we calculated above is equal to content
            // width we had in the previous rendering pass
            bool redraw = tabContentWidth != _tabContentWidth || forceRedraw;

            if (redraw)
            {
                _tabContentWidth = tabContentWidth;
            }

            int i = tabs.Count - 1;
            List<Tuple<TitleBarTab, int, Rectangle>> activeTabs = new List<Tuple<TitleBarTab, int, Rectangle>>();

            // Render the background
            graphicsContext.FillRectangle(new SolidBrush(BackColor), offset.X, offset.Y, _parentWindow.Width, TabHeight);

            int selectedIndex = tabs.FindIndex(t => t.Active);
            Image tabCenterImage = null;

            if (selectedIndex != -1)
            {
                TitleBarTab selectedTab = tabs[selectedIndex];

                Image tabLeftImage = GetTabLeftImage(selectedTab);
                Image tabRightImage = GetTabRightImage(selectedTab);
                tabCenterImage = GetTabCenterImage(selectedTab);

                Rectangle tabArea = new Rectangle(
                    SystemInformation.BorderSize.Width + offset.X +
                    selectedIndex * (tabContentWidth + tabLeftImage.Width + tabRightImage.Width - OverlapWidth),
                    offset.Y + (TabHeight - tabCenterImage.Height), tabContentWidth + tabLeftImage.Width + tabRightImage.Width,
                    tabCenterImage.Height);

                if (IsTabRepositioning && _tabClickOffset != null)
                {
                    // Make sure that the user doesn't move the tab past the beginning of the list or the outside of the window
                    tabArea.X = cursor.X - _tabClickOffset.Value;
                    tabArea.X = Math.Max(SystemInformation.BorderSize.Width + offset.X, tabArea.X);
                    tabArea.X =
                        Math.Min(
                            SystemInformation.BorderSize.Width + (_parentWindow.WindowState == FormWindowState.Maximized
                                ? _parentWindow.ClientRectangle.Width - (_parentWindow.ControlBox
                                    ? SystemInformation.CaptionButtonSize.Width
                                    : 0) -
                                  (_parentWindow.MinimizeBox
                                      ? SystemInformation.CaptionButtonSize.Width
                                      : 0) -
                                  (_parentWindow.MaximizeBox
                                      ? SystemInformation.CaptionButtonSize.Width
                                      : 0)
                                : _parentWindow.ClientRectangle.Width) - tabArea.Width, tabArea.X);

                    int dropIndex = 0;

                    // Figure out which slot the active tab is being "dropped" over
                    if (tabArea.X - SystemInformation.BorderSize.Width - offset.X - TabRepositionDragDistance > 0)
                    {
                        dropIndex =
                            Math.Min(
                                Convert.ToInt32(
                                    Math.Round(
                                        Convert.ToDouble(tabArea.X - SystemInformation.BorderSize.Width - offset.X - TabRepositionDragDistance) /
                                        Convert.ToDouble(tabArea.Width - OverlapWidth))), tabs.Count - 1);
                    }

                    // If the tab has been moved over another slot, move the tab object in the window's tab list
                    if (dropIndex != selectedIndex)
                    {
                        TitleBarTab tab = tabs[selectedIndex];

                        _parentWindow.Tabs.SuppressEvents();
                        _parentWindow.Tabs.Remove(tab);
                        _parentWindow.Tabs.Insert(dropIndex, tab);
                        _parentWindow.Tabs.ResumeEvents();
                    }
                }

                activeTabs.Add(new Tuple<TitleBarTab, int, Rectangle>(tabs[selectedIndex], selectedIndex, tabArea));
            }

            // Loop through the tabs in reverse order since we need the ones farthest on the left to overlap those to their right
            foreach (TitleBarTab tab in ((IEnumerable<TitleBarTab>)tabs).Reverse())
            {
                Image tabLeftImage = GetTabLeftImage(tab);
                tabCenterImage = GetTabCenterImage(tab);
                Image tabRightImage = GetTabRightImage(tab);

                Rectangle tabArea =
                    new Rectangle(
                        SystemInformation.BorderSize.Width + offset.X +
                        (i * (tabContentWidth + tabLeftImage.Width + tabRightImage.Width - OverlapWidth)),
                        offset.Y + (TabHeight - tabCenterImage.Height), tabContentWidth + tabLeftImage.Width + tabRightImage.Width,
                        tabCenterImage.Height);

                // If we need to redraw the tab image, null out the property so that it will be recreated in the call to Render() below
                if (redraw)
                {
                    tab.TabImage = null;
                }

                // In this first pass, we only render the inactive tabs since we need the active tabs to show up on top of everything else
                if (!tab.Active)
                {
                    Render(graphicsContext, tab, i, tabArea, cursor, tabLeftImage, tabCenterImage, tabRightImage);
                }

                i--;
            }

            // In the second pass, render all of the active tabs identified in the previous pass
            foreach (Tuple<TitleBarTab, int, Rectangle> tab in activeTabs)
            {
                Image tabLeftImage = GetTabLeftImage(tab.Item1);
                tabCenterImage = GetTabCenterImage(tab.Item1);
                Image tabRightImage = GetTabRightImage(tab.Item1);

                Render(graphicsContext, tab.Item1, tab.Item2, tab.Item3, cursor, tabLeftImage, tabCenterImage, tabRightImage);
            }

            _previousTabCount = tabs.Count;

            // Render the add tab button to the screen
            if (ShowAddButton && !IsTabRepositioning)
            {
                _addButtonArea =
                    new Rectangle(
                        (_previousTabCount *
                         (tabContentWidth + _activeLeftSideImage.Width + _activeRightSideImage.Width - OverlapWidth)) +
                        _activeRightSideImage.Width + AddButtonMarginLeft + offset.X,
                        AddButtonMarginTop + offset.Y + (TabHeight - tabCenterImage.Height), _addButtonImage.Width, _addButtonImage.Height);

                bool cursorOverAddButton = IsOverAddButton(cursor);
                float _plusOffset = 8F;

                // Draw Circle behind plus
                graphicsContext.FillEllipse(new SolidBrush(Tools.ShiftBrightness(BackColor, cursorOverAddButton ? 60 : 40, false)), _addButtonArea);

                // Draw horizontal line of plus
                graphicsContext.DrawLine(new Pen(ForeColor, 1F),
                    _addButtonArea.X + ((_addButtonArea.Width / 2)),
                    _addButtonArea.Y + (_addButtonArea.Height - _plusOffset),
                    _addButtonArea.X + ((_addButtonArea.Width / 2)),
                    _addButtonArea.Y + _plusOffset);

                // Draw vertical line of plus
                graphicsContext.DrawLine(new Pen(ForeColor, 1F),
                    _addButtonArea.X + _plusOffset,
                    _addButtonArea.Y + ((_addButtonArea.Height / 2)),
                    _addButtonArea.X + (_addButtonArea.Width - _plusOffset),
                    _addButtonArea.Y + ((_addButtonArea.Height / 2)));

                // OLD: Draws image

                //graphicsContext.DrawImage(
                //					cursorOverAddButton
                //						? _addButtonHoverImage
                //						: _addButtonImage, _addButtonArea, 0, 0, cursorOverAddButton
                //							? _addButtonHoverImage.Width
                //							: _addButtonImage.Width,
                //					cursorOverAddButton
                //						? _addButtonHoverImage.Height
                //						: _addButtonImage.Height, GraphicsUnit.Pixel);
            }

            if (IsWindows10)
            {
                _windowsSizingBoxes.Render(graphicsContext, cursor);
            }
        }

        private static GraphicsPath GetLeftTabPath(int x, int y, int w, int h, int rx, int ry)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(x, y, rx + rx, ry + ry, 180, 90);
            path.AddLine(x + rx, y, x + w, y);
            path.AddLine(x + w, y, x + w, y + h);
            path.AddLine(x + w, y + h, x, y + h);
            path.CloseFigure();
            return path;
        }

        private static GraphicsPath GetRightTabPath(int x, int y, int w, int h, int rx, int ry)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddLine(x, y, x + w - rx, y);
            path.AddArc(x + w - 2 * rx, y, 2 * rx, 2 * ry, 270, 90);
            path.AddLine(x + w, y + ry, x + w, y + h);
            path.AddLine(x + w - rx, y + h, x, y + h);
            path.CloseFigure();
            return path;
        }

        private static GraphicsPath GetXPath(int x, int y, int w, int h)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddLine(x, y, x + w, y + h);
            path.AddLine(x, y, x + (w / 2), y + (h / 2)); //Line here so it won't connect their ends.
            path.AddLine(x, y + h, x + w, y);
            return path;
        }

        protected override void Render(Graphics graphicsContext, TitleBarTab tab, int index, Rectangle area, Point cursor, Image tabLeftImage, Image tabCenterImage, Image tabRightImage)
        {
            if (!IsWindows10 && !tab.Active && index == _parentWindow.Tabs.Count - 1)
            {
                //  tabRightImage = Properties.Resources.InactiveRightNoDivider;
            }

            if (_suspendRendering)
            {
                return;
            }

            // If we need to redraw the tab image
            if (tab.TabImage == null)
            {
                frmTab cefform = tab.Content as frmTab;
                Color tabColor = cefform.AutoTabColor ? (tab.Active ? Tools.ShiftBrightness(BackColor, 40, false) : Tools.ShiftBrightness(BackColor, 20, false)) : cefform.TabColor;
                // We render the tab to an internal property so that we don't necessarily have to redraw it in every rendering pass, only if its width or
                // status have changed
                tab.TabImage = new Bitmap(area.Width <= 0 ? 1 : area.Width, tabCenterImage.Height <= 0 ? 1 : tabCenterImage.Height);

                using (Graphics tabGraphicsContext = Graphics.FromImage(tab.TabImage))
                {
                    // Create brush for filling rectangles, ellipses etc.
                    Brush tabBrush = new SolidBrush(tabColor);
                    int curve = 5;

                    // Draw the left portion of the tab
                    Rectangle tabLeft = new Rectangle(0, 0, tabLeftImage.Width, tabLeftImage.Height);
                    tabGraphicsContext.FillPath(tabBrush, GetLeftTabPath(tabLeft.X, tabLeft.Y, tabLeft.Width, tabLeft.Height, curve, curve));

                    // OLD: Draw the left portion of the tab
                    //tabGraphicsContext.DrawImage(tabLImage, tabLeft, 0, 0, tabLeftImage.Width, tabLeftImage.Height, GraphicsUnit.Pixel);

                    // Draw the center portion of the tab
                    Rectangle tabCenter = new Rectangle(tabLeftImage.Width, 0, _tabContentWidth, tabCenterImage.Height);
                    tabGraphicsContext.FillRectangle(tabBrush, tabCenter);

                    // OLD: Draw the center portion of the tab
                    //tabGraphicsContext.DrawImage(tabCenterImage, new Rectangle(tabLeftImage.Width, 0, _tabContentWidth, tabCenterImage.Height), 0, 0, _tabContentWidth, tabCenterImage.Height,GraphicsUnit.Pixel);

                    // Draw the right portion of the tab
                    Rectangle tabRight = new Rectangle(tabLeftImage.Width + _tabContentWidth, 0, tabRightImage.Width, tabRightImage.Height);
                    tabGraphicsContext.FillPath(tabBrush, GetRightTabPath(tabRight.X, tabRight.Y, tabRight.Width, tabRight.Height, curve, curve));

                    // OLD: Draw the right portion of the tab
                    //tabGraphicsContext.DrawImage(tabRightImage, tabRight, 0, 0, tabRightImage.Width, tabRightImage.Height, GraphicsUnit.Pixel);

                    // Draw the close button
                    if (tab.ShowCloseButton)
                    {
                        Image closeButtonImage = IsOverCloseButton(tab, cursor)
                            ? _closeButtonHoverImage
                            : _closeButtonImage;

                        tab.CloseButtonArea = new Rectangle(
                            area.Width - tabRightImage.Width - CloseButtonMarginRight - closeButtonImage.Width, CloseButtonMarginTop, closeButtonImage.Width,
                            closeButtonImage.Height);

                        // Draw Circle behind plus
                        tabGraphicsContext.FillEllipse(new SolidBrush(Tools.ShiftBrightness(tabColor, IsOverCloseButton(tab, cursor) ? 60 : 40, false)), tab.CloseButtonArea);
                        int _closeOffsetSize = 10;
                        int _closeOffsetLoc = 5;

                        tabGraphicsContext.DrawPath(new Pen(ForeColor, 1F), GetXPath(tab.CloseButtonArea.X + _closeOffsetLoc, tab.CloseButtonArea.Y + _closeOffsetLoc, tab.CloseButtonArea.Width - _closeOffsetSize, tab.CloseButtonArea.Height - _closeOffsetSize));

                        // OLD: Draws image

                        //tabGraphicsContext.DrawImage(
                        //    closeButtonImage, tab.CloseButtonArea, 0, 0,
                        //    closeButtonImage.Width, closeButtonImage.Height,
                        //    GraphicsUnit.Pixel);
                    }
                }

                tab.Area = area;
            }

            // Render the tab's saved image to the screen
            graphicsContext.DrawImage(
                tab.TabImage, area, 0, 0, tab.TabImage.Width, tab.TabImage.Height,
                GraphicsUnit.Pixel);

            // Render the icon for the tab's content, if it exists and there's room for it in the tab's content area
            if (tab.Content.ShowIcon && _tabContentWidth > 16 + IconMarginLeft + (tab.ShowCloseButton
                ? CloseButtonMarginLeft +
                  tab.CloseButtonArea.Width +
                  CloseButtonMarginRight
                : 0))
            {
                graphicsContext.DrawIcon(
                    new Icon(tab.Content.Icon, 16, 16),
                    new Rectangle(area.X + OverlapWidth + IconMarginLeft, IconMarginTop + area.Y, 16, 16));
            }

            // Render the caption for the tab's content if there's room for it in the tab's content area
            if (_tabContentWidth > (tab.Content.ShowIcon
                ? 16 + IconMarginLeft + IconMarginRight
                : 0) + CaptionMarginLeft + CaptionMarginRight + (tab.ShowCloseButton
                    ? CloseButtonMarginLeft +
                      tab.CloseButtonArea.Width +
                      CloseButtonMarginRight
                    : 0))
            {
                graphicsContext.DrawString(
                    tab.Caption, CaptionFont, new SolidBrush(ForeColor),
                    new Rectangle(
                        area.X + OverlapWidth + CaptionMarginLeft + (tab.Content.ShowIcon
                            ? IconMarginLeft +
                              16 +
                              IconMarginRight
                            : 0),
                        CaptionMarginTop + area.Y,
                        _tabContentWidth - (tab.Content.ShowIcon
                            ? IconMarginLeft + 16 + IconMarginRight
                            : 0) - (tab.ShowCloseButton
                                ? _closeButtonImage.Width +
                                  CloseButtonMarginRight +
                                  CloseButtonMarginLeft
                                : 0), tab.TabImage.Height),
                    new StringFormat(StringFormatFlags.NoWrap)
                    {
                        Trimming = StringTrimming.EllipsisCharacter
                    });
            }
        }

        protected override int GetMaxTabAreaWidth(List<TitleBarTab> tabs, Point offset)
        {
            return _parentWindow.ClientRectangle.Width - offset.X -
                        (ShowAddButton
                            ? _addButtonImage.Width + AddButtonMarginLeft + AddButtonMarginRight
                            : 0) -
                        (tabs.Count * OverlapWidth) -
                        _windowsSizingBoxes.Width;
        }
    }

    public class YorotWSB : WindowsSizingBoxes
    {
        public YorotWSB(TitleBarTabs parentWindow) : base(parentWindow)
        {
        }

        public override void Render(Graphics graphicsContext, Point cursor)
        {
            int right = _parentWindow.ClientRectangle.Width;
            YorotTabRenderer tRender = _parentWindow.TabRenderer as YorotTabRenderer;
            Color inactiveColor = tRender.BackColor;
            Color activeColor = tRender.OverlayColor;
            Color fColor1 = Tools.AutoWhiteBlack(inactiveColor);
            Color fColor2 = Tools.AutoWhiteBlack(activeColor);

            _minimizeButtonArea.X = right - 135;
            _maximizeRestoreButtonArea.X = right - 90;
            _closeButtonArea.X = right - 45;

            // Minimize
            bool minimizeButtonHighlighted = _minimizeButtonArea.Contains(cursor);
            graphicsContext.FillRectangle(new SolidBrush(minimizeButtonHighlighted ? activeColor : inactiveColor), _minimizeButtonArea);
            graphicsContext.DrawLine(new Pen(minimizeButtonHighlighted ? fColor2 : fColor1, 1F), new Point(_minimizeButtonArea.Location.X + ((_minimizeButtonArea.Width / 2) - 5), _minimizeButtonArea.Location.Y + (_minimizeButtonArea.Height / 2)), new Point(_minimizeButtonArea.Location.X + (_minimizeButtonArea.Width / 2) + 5, _minimizeButtonArea.Location.Y + (_minimizeButtonArea.Height / 2)));
            //graphicsContext.DrawImage(_minimizeImage, _minimizeButtonArea.X + 17, _minimizeButtonArea.Y + 9);

            // Maximize
            bool maximizeButtonHighligted = _maximizeRestoreButtonArea.Contains(cursor);
            bool isMaximized = _parentWindow.WindowState == FormWindowState.Maximized;
            graphicsContext.FillRectangle(new SolidBrush(maximizeButtonHighligted ? activeColor : inactiveColor), _maximizeRestoreButtonArea);
            graphicsContext.DrawRectangle(new Pen(maximizeButtonHighligted ? fColor2 : fColor1, 1F), _maximizeRestoreButtonArea.X + ((_maximizeRestoreButtonArea.Width / 5) * 2), _maximizeRestoreButtonArea.Y + ((_maximizeRestoreButtonArea.Height / 5) * 2), _maximizeRestoreButtonArea.Width / 5, _maximizeRestoreButtonArea.Width / 5);
            //graphicsContext.DrawImage(_parentWindow.WindowState == FormWindowState.Maximized ? _restoreImage : _maximizeImage, _maximizeRestoreButtonArea.X + 17, _maximizeRestoreButtonArea.Y + 9);

            // Close
            bool closeButtonHighlighted = _closeButtonArea.Contains(cursor);
            graphicsContext.FillRectangle(closeButtonHighlighted ? new SolidBrush(activeColor) : new SolidBrush(inactiveColor), _closeButtonArea);
            graphicsContext.DrawPath(new Pen(closeButtonHighlighted ? fColor2 : fColor1, 1F), GetXPath(_closeButtonArea.X + 17, _closeButtonArea.Y + 9, 10, 10));
        }

        private static GraphicsPath GetXPath(int x, int y, int w, int h)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddLine(x, y, x + w, y + h);
            path.AddLine(x, y, x + (w / 2), y + (h / 2)); //Line here so it won't connect their ends.
            path.AddLine(x, y + h, x + w, y);
            return path;
        }
    }

    public enum TabColors
    {
        BackColor,
        ForeColor,
        OverlayColor,
        OverlayBackColor
    }
}