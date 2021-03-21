using HTAlt;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yorot
{
    /// <summary>
    /// Main class for Yorot. 
    /// <para></para>Suggestion: Create a new instance of this class in your project's one of the public static classes for easy access and avoid duplicates. See Yorot-Win32 project for simple implementation.
    /// </summary>
    public class YorotMain
    {
        /// <summary>
        /// Creates a new Yorot Main.
        /// </summary>
        /// <param name="appPath">Path of the application, mostly &quot;.yorot&quot; folder</param>
        public YorotMain(string appPath)
        {
            if (string.IsNullOrWhiteSpace(appPath)) { throw new ArgumentNullException("\"appPath\" cannot be empty."); };
            if (!System.IO.Directory.Exists(appPath)) { System.IO.Directory.CreateDirectory(appPath); }
            if (!appPath.HasWriteAccess()) { throw new System.IO.FileLoadException("Cannot access to path \"" + appPath + "\"."); }

            // Set Application path

            AppPath = appPath;

            LangConfig = AppPath + @"lang.ycf";
            LangFolder = AppPath + @"lang\";
            ExtFolder = AppPath + @"ext\";
            ThemesFolder = AppPath + @"themes\";
            ThemeConfig = AppPath + @"themes.ycf";
            ExtConfig = AppPath + @"ext.ycf";
            AppsConfig = AppPath + @"apps.ycf";
            AppsFolder = AppPath + @"apps\";
            ProfilesFolder = AppPath + @"user\";
            ProfileConfig = AppPath + @"users.ycf";
            WEConfig = AppPath + @"weng.ycf";
            WEFolder = AppPath + @"weng\";
            LogFolder = AppPath + @"logs\";
            HTAlt.Output.LogDirPath = LogFolder;
            Wolfhook.WhFolder = AppPath + @"wh\";

            if (!System.IO.Directory.Exists(LogFolder)) { System.IO.Directory.CreateDirectory(LogFolder); }
            if (!System.IO.Directory.Exists(Wolfhook.WhFolder)) { System.IO.Directory.CreateDirectory(Wolfhook.WhFolder); }

            if (System.IO.File.Exists(appPath + @"\yorot.moved")) // Detect if Yorot Users folder is moved. If then, move info to new app path.
            {
                var MOVED = HTAlt.Tools.ReadFile(appPath + @"\yorot.moved", Encoding.Unicode);
                if (!string.IsNullOrWhiteSpace(MOVED))
                {
                    if (MOVED.HasWriteAccess())
                    {
                        AppPath = MOVED;

                        LangConfig = AppPath + @"lang.ycf";
                        LangFolder = AppPath + @"lang\";
                        ExtFolder = AppPath + @"ext\";
                        ThemesFolder = AppPath + @"themes\";
                        ThemeConfig = AppPath + @"themes.ycf";
                        ExtConfig = AppPath + @"ext.ycf";
                        AppsConfig = AppPath + @"apps.ycf";
                        AppsFolder = AppPath + @"apps\";
                        ProfilesFolder = AppPath + @"user\";
                        ProfileConfig = AppPath + @"users.ycf";
                        WEConfig = AppPath + @"weng.ycf";
                        WEFolder = AppPath + @"weng\";
                        LogFolder = AppPath + @"logs\";
                        Output.LogDirPath = LogFolder;
                        Wolfhook.WhFolder = AppPath + @"wh\";
                    }
                    else
                    {
                        Output.WriteLine("[YorotMain] Ignoring yorot.moved file. Cannot use path given in this file.", LogLevel.Warning);
                    }
                }
                else
                {
                    Output.WriteLine("[YorotMain] Ignoring yorot.moved file. File does not contains suitable path data.", LogLevel.Warning);
                }
            }

            if (!System.IO.Directory.Exists(LangFolder)) { System.IO.Directory.CreateDirectory(LangFolder); }
            if (!System.IO.Directory.Exists(ExtFolder)) { System.IO.Directory.CreateDirectory(ExtFolder); }
            if (!System.IO.Directory.Exists(ThemesFolder)) { System.IO.Directory.CreateDirectory(ThemesFolder); }
            if (!System.IO.Directory.Exists(AppsFolder)) { System.IO.Directory.CreateDirectory(AppsFolder); }
            if (!System.IO.Directory.Exists(ProfilesFolder)) { System.IO.Directory.CreateDirectory(ProfilesFolder); }
            if (!System.IO.Directory.Exists(WEFolder)) { System.IO.Directory.CreateDirectory(WEFolder); }
            AppMan = new AppManager(this);
            ThemeMan = new ThemeManager(this);
            LangMan = new YorotLangManager(this);
            Extensions = new ExtensionManager(this);
        }
        /// <summary>
        /// Profiles Manager
        /// </summary>
        public ProfileManager Profiles { get; set; }
        /// <summary>
        /// Application Manager
        /// </summary>
        public AppManager AppMan { get; set; }
        /// <summary>
        /// Theme Manager
        /// </summary>
        public ThemeManager ThemeMan { get; set; }
        /// <summary>
        /// Language Manager
        /// </summary>
        public YorotLangManager LangMan { get; set; }
        /// <summary>
        /// Extension Manager
        /// </summary>
        public ExtensionManager Extensions { get; set; }
        /// <summary>
        /// Web Engines Manager
        /// </summary>
        public YorotWEManager WebEngineMan { get; set; }
        /// <summary>
        /// Current User Settings
        /// </summary>
        public Settings Settings { get => Profiles.Current.Settings; set => Profiles.Current.Settings = value; }
        /// <summary>
        /// Wolfhook Content Delivery System
        /// </summary>
        public Wolfhook Wolfhook { get; set; } = new Wolfhook();
        /// <summary>
        /// Saves configuration and shuts down.
        /// </summary>
        public void Shutdown()
        {
            Settings.Save();
            Profiles.Save();
            AppMan.Save();
            ThemeMan.Save();
            LangMan.Save();
            Extensions.Save();
            WebEngineMan.Save();
            Wolfhook.StopSearch();
        }
        /// <summary>
        /// Location of application files.
        /// </summary>
        public string AppPath { get; set; }
        /// <summary>
        /// Language configuration file location.
        /// </summary>
        public string LangConfig { get; set; }
        /// <summary>
        /// Language folder
        /// </summary>
        public string LangFolder { get; set; }
        /// <summary>
        /// Extensions folder
        /// </summary>
        public string ExtFolder { get; set; }
        /// <summary>
        /// Themes location.
        /// </summary>
        public string ThemesFolder { get; set; }
        /// <summary>
        /// Themes Manager configuration file location.
        /// </summary>
        public string ThemeConfig { get; set; }
        /// <summary>
        /// Extension Manager configuration file location.
        /// </summary>
        public string ExtConfig { get; set; }
        /// <summary>
        /// Yorot App Manager configuration file location.
        /// </summary>
        public string AppsConfig { get; set; }
        /// <summary>
        /// Yorot App Manager Application storage.
        /// </summary>
        public string AppsFolder { get; set; }
        /// <summary>
        /// User profiles folder.
        /// </summary>
        public string ProfilesFolder { get; set; }
        /// <summary>
        /// User profiles configuration file.
        /// </summary>
        public string ProfileConfig { get; set; }
        /// <summary>
        /// The location of the Web Engines configuration file on drive.
        /// </summary>
        public string WEConfig { get; set; }
        /// <summary>
        /// The location of the Web Engines folder on drive.
        /// </summary>
        public string WEFolder { get; set; }
        /// <summary>
        /// Logs folder.
        /// </summary>
        public string LogFolder { get; set; }
    }
}
