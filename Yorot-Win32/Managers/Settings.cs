namespace Yorot
{
    public class Settings
    {
        public Settings()
        {

        }

        public AppMan AppMan { get; set; } = new AppMan(YorotGlobal.UserApp);
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

    }
}
