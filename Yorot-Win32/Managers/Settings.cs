namespace Yorot
{
    public class Settings
    {
        public Settings()
        {

        }

        public AppMan AppMan { get; set; } = new AppMan(null);
        public frmMain MainForm { get; set; }

        public string Homepage {get;set;}
        public int PanelMiidSize {get;set;}
        public int PanelState {get;set;}
        public System.Drawing.Color BackColor { get; set; }
        public System.Drawing.Color ForeColor { get; set; }
        public System.Drawing.Color OverlayColor { get; set; }
        public System.Drawing.Color TabBackColor { get; set; }
        public System.Drawing.Color TabForeColor { get; set; }
        public System.Drawing.Color TabbarBackColor { get; set; }
        public System.Drawing.Color TabbarOverlayColor { get; set; }
        public System.Drawing.Color AppsOverlayColor { get; set; }
        public System.Drawing.Color AppsBackColor { get; set; }
        public System.Drawing.Color AppsForeColor { get; set; }
        public System.Drawing.Color AppListBackColor { get; set; }
        public System.Drawing.Color AppListForeColor { get; set; }
        public System.Drawing.Color AppListOverlayColor { get; set; }
        /// <summary>
        /// User Files location.
        /// </summary>
        public string UserLoc = YorotGlobal.YorotAppPath + "\\usr\\";
        /// <summary>
        /// User Cache location.
        /// </summary>
        public  string CacheLoc = YorotGlobal.YorotAppPath + "\\usr\\c\\";
        /// <summary>
        /// User settings location.
        /// </summary>
        public string UserSettings = YorotGlobal.YorotAppPath + "\\usr\\usr.knf";
        /// <summary>
        /// History Manager configuration file location.
        /// </summary>
        public string UserHistory = YorotGlobal.YorotAppPath + "\\usr\\hman.knf";
        /// <summary>
        /// Favorites Manager configuration file location.
        /// </summary>
        public string UserFavorites = YorotGlobal.YorotAppPath + "\\usr\\fman.knf";
        /// <summary>
        /// Downloads Manager configuration file location.
        /// </summary>
        public string UserDownloads = YorotGlobal.YorotAppPath + "\\usr\\dman.knf";
        /// <summary>
        /// Themes Manager configuration file location.
        /// </summary>
        public string UserTheme = YorotGlobal.YorotAppPath + "\\usr\\tman.knf";
        /// <summary>
        /// Extension Manager configuration file location.
        /// </summary>
        public string UserExt = YorotGlobal.YorotAppPath + "\\usr\\extman.knf";
        /// <summary>
        /// Yorot App Manager configuration file location.
        /// </summary>
        public string UserApp = YorotGlobal.YorotAppPath + "\\usr\\yam.knf";
        /// <summary>
        /// Yorot App Manager Application storage.
        /// </summary>
        public string UserApps = YorotGlobal.YorotAppPath + "\\usr\\yam\\";

    }
}
