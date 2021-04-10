using HTAlt;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        public static string YorotAppPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.yorot\\";
        /// <summary>
        /// Yorot Main
        /// </summary>
        public static YorotMain Main;
        /// <summary>
        /// Yorot Special class for this type of executable (In this case, Win32).
        /// </summary>
        public static YorotSpecial Y1 = null;
        /// <summary>
        /// <c>true</c> if this session is a PreOut, otherwise <c>false</c>.
        /// </summary>
        public static bool isPreOut = true;
        /// <summary>
        /// Version of Yorot.
        /// </summary>
        public static string Version = isPreOut ? "indev1" : Application.ProductVersion.ToString();
        /// <summary>
        /// Version Number of this Yorot version.
        /// </summary>
        public static int VersionNo = 1;
        /// <summary>
        /// Codename of current Yorot version.
        /// </summary>
        public static string CodeName = "Hamantha"; // "HAMANTHA" "FLOWER BAZAAR" "Tropic" "Emerald Fastline" 
        /// <summary>
        /// Placeholder text used by default apps. 
        /// </summary>
        public static string DefaultaAppOriginPlaceHolder = "8 February 2021 00:19:00 GMT+3:00" + Environment.NewLine + "https://github.com/Haltroy/Yorot" + Environment.NewLine + "Yorot C# Embedded Code" + Environment.NewLine + "(<Source>)"; // LONG-TERM TODO: Change date on releases.
        /// <summary>
        /// Version Control (HTUPDATE) URL.
        /// </summary>
        public static string HTULoc = "https://raw.githubusercontent.com/Haltroy/Yorot/main/Yorot.htupdate";
    }
    public class YorotSpecial
    {
        public List<frmMain> MainForms { get; set; } = new List<frmMain>();
        public frmMain MainForm { get => MainForms[0]; }
    }
}
