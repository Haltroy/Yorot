using HTAlt;
using System;
using System.Text;

namespace Yorot
{
    /// <summary>
    /// Main class for Yorot.
    /// <para></para>Suggestion: Create a new instance of this class in your project's one of the public static classes for easy access and avoid duplicates. See Yorot-Win32 project for simple implementation.
    /// </summary>
    public abstract class YorotMain
    {
        /// <summary>
        /// Creates a new Yorot Main.
        /// </summary>
        /// <param name="appPath">Path of the application, mostly &quot;.yorot&quot; folder</param>
        /// <param name="codename">Codename of the Yorot or the flavor.</param>
        /// <param name="isIncognito"><see cref="true"/> to start Yorot in incognito mode, otherwise <seealso cref="false"/>.</param>
        /// <param name="version">Version of the Yorot or the flavor.</param>
        /// <param name="name">Name of the Yorot or the flavor.</param>
        /// <param name="verno">Version number of the Yorot or the flavor.</param>
        public YorotMain(string appPath, string name, string codename, string version, int verno, string branch, bool isIncognito = false)
        {
            if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentException("\"name\" caanot be empty."); }
            Name = name;
            if (string.IsNullOrWhiteSpace(codename)) { throw new ArgumentException("\"codename\" caanot be empty."); }
            CodeName = codename;
            if (string.IsNullOrWhiteSpace(version)) { throw new ArgumentException("\"version\" caanot be empty."); }
            VersionText = version;
            if (string.IsNullOrWhiteSpace(branch)) { throw new ArgumentException("\"branch\" caanot be empty."); }
            YorotBranch = branch;
            if (verno <= 0) { throw new ArgumentException("\"verno\" must be bigger than zero."); }
            Version = verno;
            if (string.IsNullOrWhiteSpace(appPath)) { throw new ArgumentNullException("\"appPath\" cannot be empty."); };
            if (!System.IO.Directory.Exists(appPath)) { System.IO.Directory.CreateDirectory(appPath); }
            if (!appPath.HasWriteAccess()) { throw new System.IO.FileLoadException("Cannot access to path \"" + appPath + "\"."); }
            Incognito = isIncognito;
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
                string MOVED = HTAlt.Tools.ReadFile(appPath + @"\yorot.moved", Encoding.Unicode);
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
                        if (!System.IO.Directory.Exists(LogFolder)) { System.IO.Directory.CreateDirectory(LogFolder); }
                        if (!System.IO.Directory.Exists(Wolfhook.WhFolder)) { System.IO.Directory.CreateDirectory(Wolfhook.WhFolder); }
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
            BeforeInit();
            AppMan = new AppManager(this);
            ThemeMan = new ThemeManager(this);
            LangMan = new YorotLangManager(this);
            Extensions = new ExtensionManager(this);
            WebEngineMan = new YorotWEManager(this);
            Profiles = new ProfileManager(this);
            AfterInit();
        }
        /// <summary>
        /// Current Bracnh of Yorot. (ex. Yorot-Win32, Yorot-Avalonia)
        /// </summary>
        public string YorotBranch { get; set; }

        /// <summary>
        /// Event raised before launching all managers.
        /// </summary>
        public abstract void BeforeInit();

        /// <summary>
        /// Event raised after launching all managers.
        /// </summary>
        public abstract void AfterInit();

        /// <summary>
        /// Gets the current theme applied by user.
        /// </summary>
        public YorotTheme CurrentTheme => Profiles.Current.Settings.CurrentTheme;

        /// <summary>
        /// Gets the current settings applied by user.
        /// </summary>
        public Settings CurrentSettings => Profiles.Current.Settings;

        /// <summary>
        /// Gets the current language applied by user.
        /// </summary>
        public YorotLanguage CurrentLanguage => Profiles.Current.Settings.CurrentLanguage;

        /// <summary>
        /// Determines if this session is Incognito mode.
        /// </summary>
        public bool Incognito { get; set; }

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
        /// Wolfhook Content Delivery System
        /// </summary>
        public Wolfhook Wolfhook { get; set; } = new Wolfhook();

        /// <summary>
        /// Saves configuration and shuts down.
        /// </summary>
        public void Shutdown()
        {
            if (!Incognito)
            {
                Profiles.Current.Settings.Save();
                Profiles.Save();
                AppMan.Save();
                ThemeMan.Save();
                LangMan.Save();
                Extensions.Save();
                WebEngineMan.Save();
                Wolfhook.StopSearch();
            }
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

        /// <summary>
        /// Name of your application. Ex.: Yorot
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Codename of your application. Ex.: indev1
        /// </summary>
        public string CodeName { get; set; }

        /// <summary>
        /// Version text of your application. Ex.: 1.0.0.0
        /// </summary>
        public string VersionText { get; set; }

        /// <summary>
        /// Version number of your application. Ex.: 1
        /// </summary>
        public int Version { get; set; }
    }
}