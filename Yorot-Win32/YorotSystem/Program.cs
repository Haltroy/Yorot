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
            bool exists = System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1;
            YorotGlobal.Wolfhook = new Wolfhook() { WhFolder = YorotGlobal.YorotAppPath + "\\wolfhook\\", };
            YorotGlobal.Y1 = new YorotSpecial();
            YorotGlobal.Settings = new Settings(YorotGlobal.YorotAppPath);
            Output.LogDirPath = YorotGlobal.Settings.AppPath + "\\logs\\";
            if (exists)
            {
                Output.WriteLine("<Yorot.Program> App already running. Passing arguments...", LogLevel.Warning);
                YorotGlobal.Wolfhook.SendWolf(string.Join("§", args));
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
