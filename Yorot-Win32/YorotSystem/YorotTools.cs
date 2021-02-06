using System;
using System.Linq;
using System.Xml;

namespace Yorot
{
    public static class YorotTools
    {
        public static string FromThemeFolder(this string x)
        {
            return x.Replace("[THEMES]", YorotGlobal.Settings.ThemesLoc);
        }
        public static string ToThemeFolder(this string x)
        {
            return x.Replace(YorotGlobal.Settings.ThemesLoc, "[THEMES]");
        }
        public static System.Drawing.Image GetAppIcon(YorotApp app)
        {
            if (app.isSystemApp)
            {
                switch (app.AppIcon)
                {
                    default:
                    case "yorot.png":
                        return Properties.Resources.Yorot;
                    case "settings.png":
                        return Properties.Resources.Settings;
                    case "store.png":
                        return Properties.Resources.store;
                    case "calc.png":
                        return Properties.Resources.calc;
                    case "calendar.png":
                        return Properties.Resources.calendar;
                    case "notepad.png":
                        return Properties.Resources.notepad;
                    case "console.png":
                        return Properties.Resources.console;
                    case "colman.png":
                        return Properties.Resources.colman;
                    case "fileman.png":
                        return Properties.Resources.fileman;
                    case "yopad.png":
                        return Properties.Resources.yopad;
                    case "spacepass.png":
                        return Properties.Resources.spacepass;
                }
            }
            else
            {
                return HTAlt.Tools.ReadFile(YorotGlobal.Settings.UserApps + app.AppCodeName + "\\" + app.AppIcon, System.Drawing.Imaging.ImageFormat.Png);
            }
        }
        public static System.Drawing.Image ThemeThumbnail(YorotTheme theme)
        {

            if (theme.isDefaultTheme)
            {
                switch (theme.ThumbLoc)
                {
                    default: case "YorotLight.png": return Properties.Resources.YorotLight;
                    case "YorotStone.png": return Properties.Resources.YorotStone;
                    case "YorotRazor.png": return Properties.Resources.YorotRazor;
                    case "YorotDark.png": return Properties.Resources.YorotDark;
                    case "YorotShadow.png": return Properties.Resources.YorotShadow;
                    case "YorotDeepBlue.png": return Properties.Resources.YorotDeepBlue;
                }
            }
            else
            {
                return HTAlt.Tools.ReadFile(YorotGlobal.Settings.ThemesLoc + theme.CodeName + @"\" + theme.ThumbLoc, System.Drawing.Imaging.ImageFormat.Png);
            }
        }
    }
}
