using HTAlt;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Yorot
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            bool _i = args.Contains("-i") || args.Contains("--incognito");
            bool exists = System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1;
            YorotGlobal.Main = new YorotMain(YorotGlobal.YorotAppPath, "Yorot", YorotGlobal.CodeName, YorotGlobal.Version, YorotGlobal.VersionNo);
            YorotGlobal.Main.Wolfhook = new Wolfhook() { WhFolder = YorotGlobal.YorotAppPath + "\\wolfhook\\", };
            YorotGlobal.Y1 = new YorotSpecial();
            Output.LogDirPath = YorotGlobal.Main.AppPath + "\\logs\\";
            if (exists && !_i)
            {
                Output.WriteLine("<Yorot.Program> App already running. Passing arguments...", LogLevel.Warning);
                YorotGlobal.Main.Wolfhook.SendWolf(string.Join("§", args));
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain(true));
            }
        }
    }
}
