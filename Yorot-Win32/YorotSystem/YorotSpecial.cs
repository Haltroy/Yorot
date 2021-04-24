using HTAlt;
using System.Collections.Generic;

namespace Yorot
{
    public class YorotSpecial : YorotMain
    {
        public YorotSpecial(string appPath, string name, string codename, string version, int verno, bool isIncognito = false) : base(appPath, name, codename, version, verno, isIncognito)
        {

        }
        public override void BeforeInit()
        {
            Output.LogDirPath = AppPath + "\\logs\\";
            YorotDefaultLangs.GenLangs(LangFolder);
        }
        public override void AfterInit()
        {
            
        }
        public List<frmMain> MainForms { get; set; } = new List<frmMain>();
        public frmMain MainForm { get => MainForms[0]; }
    }
}
