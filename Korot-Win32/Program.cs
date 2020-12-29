using System;
using System.Linq;
using System.Windows.Forms;

namespace Korot_Win32
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var exists = System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1;
            KorotGlobal.Wolfhook = new Wolfhook();
            if (exists)
            {
                Output.WriteLine("<Korot.Program> App already running. Passing arguments..." , LogLevel.Warning);
                hook.SendWolf(string.Join("§", args));
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain());
            }
        }
    }
}
