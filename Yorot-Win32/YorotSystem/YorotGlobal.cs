using System;
using System.Drawing;
using System.Windows.Forms;

namespace Yorot
{
    /// <summary>
    /// Yorot Global Static Variables.
    /// </summary>
    public static class YorotGlobal
    {
        /// <summary>
        /// Application location.
        /// </summary>
        public static string YorotAppPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Yorot\\";
        /// <summary>
        /// Application Settings.
        /// </summary>
        public static Settings Settings = null;
        /// <summary>
        /// Wolfhook management.
        /// </summary>
        public static Wolfhook Wolfhook = null;
        /// <summary>
        /// <c>true</c> if this session is a PreOut, otherwise <c>false</c>.
        /// </summary>
        public static bool isPreOut = false;
        /// <summary>
        /// Version of Yorot.
        /// </summary>
        public static string Version = isPreOut ? "indev1" : Application.ProductVersion.ToString();
        /// <summary>
        /// Version Number of this Yorot version.
        /// </summary>
        public static int VersionNo = 0;
        /// <summary>
        /// Codename of current Yorot version.
        /// </summary>
        public static string CodeName = "Hamantha";
        /// <summary>
        /// Version Control (HTUPDATE) URL.
        /// </summary>
        public static string HTULoc = "[HTUPDATE LOCATION HERE]"; // TODO
        /// <summary>
        /// User Files location.
        /// </summary>
        public static string UserLoc = YorotAppPath + "\\usr\\";
        /// <summary>
        /// User Cache location.
        /// </summary>
        public static string CacheLoc = UserLoc + "\\c\\";
        /// <summary>
        /// User settings location.
        /// </summary>
        public static string UserSettings = UserLoc + "usr.knf";
        /// <summary>
        /// History Manager configuration file location.
        /// </summary>
        public static string UserHistory = UserLoc + "hman.knf";
        /// <summary>
        /// Favorites Manager configuration file location.
        /// </summary>
        public static string UserFavorites = UserLoc + "fman.knf";
        /// <summary>
        /// Downloads Manager configuration file location.
        /// </summary>
        public static string UserDownloads = UserLoc + "dman.knf";
        /// <summary>
        /// Themes Manager configuration file location.
        /// </summary>
        public static string UserTheme = UserLoc + "tman.knf";
        /// <summary>
        /// Extension Manager configuration file location.
        /// </summary>
        public static string UserExt = UserLoc + "extman.knf";
        /// <summary>
        /// Yorot App Manager configuration file location.
        /// </summary>
        public static string UserApp = UserLoc + "yam.knf";
        /// <summary>
        /// Yorot App Manager Application storage.
        /// </summary>
        public static string UserApps = UserLoc + "yam\\";
        /// <summary>
        /// Generates <see cref="Image"/> from <paramref name="baseIcon"/>.
        /// </summary>
        /// <param name="baseIcon"></param>
        /// <returns></returns>
        public static Image GenerateAppIcon(Image baseIcon, Color? BackColor = null, int squareSize = 64)
        {
            if (BackColor == null)
            {
                BackColor = Color.FromArgb(255, 128, 128, 128);
            }
            int sqHalfSize = squareSize / 2;
            int sqQuartSize = sqHalfSize / 2;
            Bitmap bm = new Bitmap(64, 64);
            using (Graphics g = Graphics.FromImage(bm))
            {
                g.FillRectangle(new SolidBrush(BackColor.Value), 0, 0, squareSize, squareSize);
                Image iconimg = HTAlt.Tools.ResizeImage(baseIcon, sqHalfSize,sqHalfSize);
                g.DrawImage(iconimg, new Rectangle(sqQuartSize, sqQuartSize, sqHalfSize, sqHalfSize));
            }
            return bm;
        }
    }
}
