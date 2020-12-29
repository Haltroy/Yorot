using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Korot_Win32
{
    /// <summary>
    /// Korot App Manager (KAM)
    /// yes that's intentional
    /// </summary>
    public class AppMan
    {
        public AppMan(string configFile)
        {
            Apps.Add(DefaultApps.AppMaker);
            Apps.Add(DefaultApps.Calculator);
            Apps.Add(DefaultApps.Collections);
            Apps.Add(DefaultApps.Console);
            Apps.Add(DefaultApps.DumbBattlePassThing);
            Apps.Add(DefaultApps.ExtMaker);
            Apps.Add(DefaultApps.FileExplorer);
            Apps.Add(DefaultApps.Kopad);
            Apps.Add(DefaultApps.LangMaker);
            Apps.Add(DefaultApps.Notepad);
            Apps.Add(DefaultApps.Settings);
            Apps.Add(DefaultApps.Store);
            Apps.Add(DefaultApps.ThemeMaker);
            Apps.Add(DefaultApps.WebBrowser);
        }
        /// <summary>
        /// A <see cref="List{T}"/> of <see cref="KorotApp"/>(s).
        /// </summary>
        public List<KorotApp> Apps { get; set; } = new List<KorotApp>();
    }
    /// <summary>
    /// This class contains default <see cref="KorotApp"/>s.
    /// </summary>
    public static class DefaultApps
    {
        public static bool isSystemApp(string codeName)
        {
                                                 
            string[] defaultApps = new string[] { "com.haltroy.korot",
                                                  "com.haltroy.settings",
                                                  "com.haltroy.store",
                                                  "com.haltroy.calendar",
                                                  "com.haltroy.calc",
                                                  "com.haltroy.notepad",
                                                  "com.haltroy.console",
                                                  "com.haltroy.colman",
                                                  "com.haltroy.fileman",
                                                  "com.haltroy.mkth",
                                                  "com.haltroy.mkext",
                                                  "com.haltroy.mkapp",
                                                  "com.haltroy.mklng",
                                                  "com.haltroy.packdist",
                                                  "com.haltroy.spacepass"};
            for(int i = 0; i < defaultApps.Length;i++)
            {
                if (string.Equals(defaultApps[i],codeName))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Korot
        /// </summary>
        public static KorotApp WebBrowser
        {
            get
            {
                return new KorotApp()
                {
                    AppName = "Korot",
                    AppCodeName = "com.haltroy.korot",
                    AppIcon = "§RES/Korot.png",
                    isLocal = true,
                    HTUPDATE = null,
                    StartFile = null,
                };
            }
        }
        /// <summary>
        /// Korot Settings
        /// </summary>
        public static KorotApp Settings
        {
            get
            {
                return new KorotApp()
                {
                    AppName = "Settings",
                    AppCodeName = "com.haltroy.settings",
                    AppIcon = "§RES/settings.png",
                    isLocal = true,
                    HTUPDATE = null,
                    StartFile = null,
                };
            }
        }
        /// <summary>
        /// Haltroy Web Store
        /// </summary>
        public static KorotApp Store
        {
            get
            {
                return new KorotApp()
                {
                    AppName = "Store",
                    AppCodeName = "com.haltroy.store",
                    AppIcon = "§RES/store.png",
                    isLocal = true,
                    HTUPDATE = null,
                    StartFile = null,
                };
            }
        }
        /// <summary>
        /// Calculator
        /// </summary>
        public static KorotApp Calculator
        {
            get
            {
                return new KorotApp()
                {
                    AppName = "Calculator",
                    AppCodeName = "com.haltroy.calc",
                    AppIcon = "§RES/calc.png",
                    isLocal = true,
                    HTUPDATE = null,
                    StartFile = null,
                };
            }
        }
        /// <summary>
        /// Calendar
        /// </summary>
        public static KorotApp Calendar
        {
            get
            {
                return new KorotApp()
                {
                    AppName = "Calendar",
                    AppCodeName = "com.haltroy.calendar",
                    AppIcon = "§RES/calendar.png",
                    isLocal = true,
                    HTUPDATE = null,
                    StartFile = null,
                };
            }
        }
        /// <summary>
        /// Text altering program.
        /// </summary>
        public static KorotApp Notepad
        {
            get
            {
                return new KorotApp()
                {
                    AppName = "Notepad",
                    AppCodeName = "com.haltroy.notepad",
                    AppIcon = "§RES/notepad.png",
                    isLocal = true,
                    HTUPDATE = null,
                    StartFile = null,
                };
            }
        }
        /// <summary>
        /// Console
        /// </summary>
        public static KorotApp Console
        {
            get
            {
                return new KorotApp()
                {
                    AppName = "Console",
                    AppCodeName = "com.haltroy.console",
                    AppIcon = "§RES/console.png",
                    isLocal = true,
                    HTUPDATE = null,
                    StartFile = null,
                };
            }
        }
        /// <summary>
        /// Collection management application.
        /// </summary>
        public static KorotApp Collections
        {
            get
            {
                return new KorotApp()
                {
                    AppName = "Collections",
                    AppCodeName = "com.haltroy.colman",
                    AppIcon = "§RES/colman.png",
                    isLocal = true,
                    HTUPDATE = null,
                    StartFile = null,
                };
            }
        }
        /// <summary>
        /// File exploration app.
        /// </summary>
        public static KorotApp FileExplorer
        {
            get
            {
                return new KorotApp()
                {
                    AppName = "Files",
                    AppCodeName = "com.haltroy.fileman",
                    AppIcon = "§RES/fileman.png",
                    isLocal = true,
                    HTUPDATE = null,
                    StartFile = null,
                };
            }
        }
        /// <summary>
        /// App that used to make themes.
        /// </summary>
        public static KorotApp ThemeMaker
        {
            get
            {
                return new KorotApp()
                {
                    AppName = "Theme Maker",
                    AppCodeName = "com.haltroy.mkth",
                    AppIcon = "§RES/mkth.png",
                    isLocal = true,
                    HTUPDATE = null,
                    StartFile = null,
                };
            }
        }
        /// <summary>
        /// APp that used to make extensions.
        /// </summary>
        public static KorotApp ExtMaker
        {
            get
            {
                return new KorotApp()
                {
                    AppName = "Extension Maker",
                    AppCodeName = "com.haltroy.mkext",
                    AppIcon = "§RES/mkext.png",
                    isLocal = true,
                    HTUPDATE = null,
                    StartFile = null,
                };
            }
        }
        /// <summary>
        /// App used to make app files.
        /// </summary>
        public static KorotApp AppMaker
        {
            get
            {
                return new KorotApp()
                {
                    AppName = "App Maker",
                    AppCodeName = "com.haltroy.mkapp",
                    AppIcon = "§RES/mkapp.png",
                    isLocal = true,
                    HTUPDATE = null,
                    StartFile = null,
                };
            }
        }
        /// <summary>
        /// App used to make language files.
        /// </summary>
        public static KorotApp LangMaker
        {
            get
            {
                return new KorotApp()
                {
                    AppName = "Language Maker",
                    AppCodeName = "com.haltroy.mklng",
                    AppIcon = "§RES/mklng.png",
                    isLocal = true,
                    HTUPDATE = null,
                    StartFile = null,
                };
            }
        }
        /// <summary>
        /// Korot Package Distrubiton system.
        /// </summary>
        public static KorotApp Kopad
        {
            get
            {
                return new KorotApp()
                {
                    AppName = "Kopad",
                    AppCodeName = "com.haltroy.packdist",
                    AppIcon = "§RES/Kopad.png",
                    isLocal = true,
                    HTUPDATE = null,
                    StartFile = null,
                };
            }
        }
        /// <summary>
        /// App that handles Space Pass stuff.
        /// </summary>
        public static KorotApp DumbBattlePassThing //Suggested by Pikehan, the drifto master
        {
            get
            {
                return new KorotApp()
                {
                    AppName = "Space Pass",
                    AppCodeName = "com.haltroy.spacepass",
                    AppIcon = "§RES/spacepass.png",
                    isLocal = true,
                    HTUPDATE = null,
                    StartFile = null,
                };
            }
        }
    }
    /// <summary>
    /// A Korot App.
    /// </summary>
    public class KorotApp
    {
        /// <summary>
        /// Creates new <see cref="KorotApp"/>.
        /// </summary>
        /// <param name="xmlNode"><see cref="XmlNode"/> that contains details of <see cref="KorotApp"/>.</param>
        public KorotApp(XmlNode xmlNode)
        {
            if (xmlNode == null)
            {
                throw new ArgumentNullException("\"xmlNode\" was null.");
            }else
            {

            }
        }
        /// <summary>
        /// Creates new <see cref="KorotApp"/>.
        /// </summary>
        public KorotApp() {   }
        /// <summary>
        /// Icon location of app.
        /// </summary>
        public string AppIcon { get; set; }
        public Image GetAppIcon() 
        {
            if (AppIcon.ToLowerInvariant().StartsWith("§RES\\") || AppIcon.ToLowerInvariant().StartsWith("§RES/"))
            {
                string appIconName = AppIcon.Substring(4);
                switch(appIconName.ToLowerInvariant())
                {
                    default:
                    case "Korot.png":
                        return Properties.Resources.Korot;
                    case "settings.png":
                        return Properties.Resources.Settings;
                    case "store.png":
                        return Properties.Resources.store;
                    case "calc.png": 
                        return Properties.Resources.calc;
                    case "calendar.png":
                        return Properties.Resources.calendar;
                    case "notepad.png": 
                        return Properties.Resources.notepad;
                    case "console.png": 
                        return Properties.Resources.console;
                    case "colman.png": 
                        return Properties.Resources.colman;
                    case "fileman.png": 
                        return Properties.Resources.fileman;
                    case "mkth.png": 
                        return Properties.Resources.kopad;
                    case "mkext.png": 
                        return Properties.Resources.kopad;
                    case "mkapp.png": 
                        return Properties.Resources.kopad;
                    case "mklng.png": 
                        return Properties.Resources.kopad;
                    case "Kopad.png": 
                        return Properties.Resources.kopad;
                    case "spacepass.png": 
                        return Properties.Resources.spacepass;
                }
            }else
            {
                return HTAlt.Tools.ReadFile(KorotGlobal.UserApps + AppCodeName + "\\" + AppIcon, System.Drawing.Imaging.ImageFormat.Png);
            }
        }
        /// <summary>
        /// Codename of app.
        /// </summary>
        public string AppCodeName { get; set; }
        /// <summary>
        /// <see cref="true"/> if app is locally saved, otherwise <see cref="false"/>.
        /// </summary>
        public bool isLocal { get; set; }
        /// <summary>
        /// URL of HTUPDATE file for this app.
        /// </summary>
        public string HTUPDATE { get; set; }
        /// <summary>
        /// Name of file (or URL) when loaded while starting app.
        /// </summary>
        public string StartFile { get; set; }
        /// <summary>
        /// Display name of application.
        /// </summary>
        public string AppName { get; set; }
    }
}
