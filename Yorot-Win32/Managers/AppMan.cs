using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

namespace Yorot
{
    /// <summary>
    /// Yorot App Manager (KAM)
    /// yes that's intentional
    /// </summary>
    public class AppMan
    {
        public AppMan(string configFile)
        {
            Apps.Add(DefaultApps.Calculator.CreateCarbonCopy());
            Apps.Add(DefaultApps.Collections.CreateCarbonCopy());
            Apps.Add(DefaultApps.Console.CreateCarbonCopy());
            Apps.Add(DefaultApps.DumbBattlePassThing.CreateCarbonCopy());
            Apps.Add(DefaultApps.FileExplorer.CreateCarbonCopy());
            Apps.Add(DefaultApps.Yopad.CreateCarbonCopy());
            Apps.Add(DefaultApps.Notepad.CreateCarbonCopy());
            Apps.Add(DefaultApps.Settings.CreateCarbonCopy());
            Apps.Add(DefaultApps.Store.CreateCarbonCopy());
            Apps.Add(DefaultApps.WebBrowser.CreateCarbonCopy());
        }
        /// <summary>
        /// A <see cref="List{T}"/> of <see cref="YorotApp"/>(s).
        /// </summary>
        public List<YorotApp> Apps { get; set; } = new List<YorotApp>();
        /// <summary>
        /// Gets <see cref="YorotApp"/> by it's <see cref="YorotApp.AppCodeName"/>.
        /// </summary>
        /// <param name="appcn"><see cref="YorotApp.AppCodeName"/></param>
        /// <returns><see cref="YorotApp"/></returns>
        public YorotApp FindByAppCN(string appcn)
        {
            return Apps.Find(i => string.Equals(i.AppCodeName, appcn));
        }
        /// <summary>
        /// Gets a <see cref="List"/> of <see cref="YorotApp"/> by their <see cref="YorotApp.AppCodeName"/>.
        /// </summary>
        /// <param name="appcn"><see cref="YorotApp.AppCodeName"/></param>
        /// <returns>A <see cref="List"/> of <see cref="YorotApp"/>.</returns>
        public List<YorotApp> FindMultipleByAppCN(string appcn)
        {
            return Apps.FindAll(i => string.Equals(i.AppCodeName, appcn));
        }
    }
    /// <summary>
    /// This class contains default <see cref="YorotApp"/>s.
    /// </summary>
    public static class DefaultApps
    {
        /// <summary>
        /// Yorot
        /// </summary>
        public static YorotApp WebBrowser => new YorotApp()
        {
            AppName = "Yorot",
            AppCodeName = "com.haltroy.yorot",
            AppIcon = "yorot.png",
            isLocal = true,
            HTUPDATE = null,
            isSystemApp = true,
            StartFile = null,
        };
        /// <summary>
        /// Yorot Settings
        /// </summary>
        public static YorotApp Settings => new YorotApp()
        {
            AppName = "Settings",
            AppCodeName = "com.haltroy.settings",
            AppIcon = "settings.png",
            isLocal = true,
            HTUPDATE = null,
            isSystemApp = true,
            StartFile = null,
        };
        /// <summary>
        /// Haltroy Web Store
        /// </summary>
        public static YorotApp Store => new YorotApp()
        {
            AppName = "Store",
            AppCodeName = "com.haltroy.store",
            AppIcon = "store.png",
            isLocal = true,
            HTUPDATE = null,
            isSystemApp = true,
            StartFile = null,
        };
        /// <summary>
        /// Calculator
        /// </summary>
        public static YorotApp Calculator => new YorotApp()
        {
            AppName = "Calculator",
            AppCodeName = "com.haltroy.calc",
            AppIcon = "calc.png",
            isLocal = true,
            HTUPDATE = null,
            isSystemApp = true,
            StartFile = null,
        };
        /// <summary>
        /// Calendar
        /// </summary>
        public static YorotApp Calendar => new YorotApp()
        {
            AppName = "Calendar",
            AppCodeName = "com.haltroy.calendar",
            AppIcon = "calendar.png",
            isLocal = true,
            HTUPDATE = null,
            isSystemApp = true,
            StartFile = null,
        };
        /// <summary>
        /// Text altering program.
        /// </summary>
        public static YorotApp Notepad => new YorotApp()
        {
            AppName = "Notepad",
            AppCodeName = "com.haltroy.notepad",
            AppIcon = "notepad.png",
            isLocal = true,
            HTUPDATE = null,
            isSystemApp = true,
            StartFile = null,
        };
        /// <summary>
        /// Console
        /// </summary>
        public static YorotApp Console => new YorotApp()
        {
            AppName = "Console",
            AppCodeName = "com.haltroy.console",
            AppIcon = "console.png",
            isLocal = true,
            HTUPDATE = null,
            isSystemApp = true,
            StartFile = null,
        };
        /// <summary>
        /// Collection management application.
        /// </summary>
        public static YorotApp Collections => new YorotApp()
        {
            AppName = "Collections",
            AppCodeName = "com.haltroy.colman",
            AppIcon = "colman.png",
            isLocal = true,
            HTUPDATE = null,
            isSystemApp = true,
            StartFile = null,
        };
        /// <summary>
        /// File exploration app.
        /// </summary>
        public static YorotApp FileExplorer => new YorotApp()
        {
            AppName = "Files",
            AppCodeName = "com.haltroy.fileman",
            AppIcon = "fileman.png",
            isLocal = true,
            HTUPDATE = null,
            StartFile = null,
            isSystemApp = true,
        };
        /// <summary>
        /// Yorot Package Distrubiton system.
        /// </summary>
        public static YorotApp Yopad => new YorotApp()
        {
            AppName = "Yopad",
            AppCodeName = "com.haltroy.packdist",
            AppIcon = "yopad.png",
            isLocal = true,
            HTUPDATE = null,
            isSystemApp = true,
            StartFile = null,
        };
        /// <summary>
        /// App that handles Space Pass stuff.
        /// </summary>
        public static YorotApp DumbBattlePassThing  => new YorotApp() //Suggested by Pikehan, the drifto master
        {
            AppName = "Space Pass",
            AppCodeName = "com.haltroy.spacepass",
            AppIcon = "spacepass.png",
            isLocal = true,
            HTUPDATE = null,
            isSystemApp = true,
            StartFile = null,
        };
    }
    /// <summary>
    /// A Yorot App.
    /// </summary>
    public class YorotApp
    {
        /// <summary>
        /// Creates new <see cref="YorotApp"/>.
        /// </summary>
        /// <param name="xmlNode"><see cref="XmlNode"/> that contains details of <see cref="YorotApp"/>.</param>
        public YorotApp(XmlNode xmlNode)
        {
            if (xmlNode == null)
            {
                throw new ArgumentNullException("\"xmlNode\" was null.");
            }
            else
            {

            }
        }
        /// <summary>
        /// Creates new <see cref="YorotApp"/>.
        /// </summary>
        public YorotApp() { }
        /// <summary>
        /// Icon location of app.
        /// </summary>
        public string AppIcon { get; set; }
        public Image GetAppIcon()
        {
            if (isSystemApp)
            {
                switch (AppIcon)
                {
                    default:
                    case "yorot.png":
                        return Properties.Resources.Yorot;
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
                    case "yopad.png":
                        return Properties.Resources.yopad;
                    case "spacepass.png":
                        return Properties.Resources.spacepass;
                }
            }
            else
            {
                return HTAlt.Tools.ReadFile(YorotGlobal.UserApps + AppCodeName + "\\" + AppIcon, System.Drawing.Imaging.ImageFormat.Png);
            }
        }
        /// <summary>
        /// Determines if this application is duplicated to create another session.
        /// </summary>
        public bool isDuplicate {get;set;} = false;
        /// <summary>
        /// A <see cref="List{T}"/> of identical duplicate <see cref="YorotApp"/>s.
        /// </summary>
        public List<YorotApp> Duplicates { get; set; } = new List<YorotApp>();
        /// <summary>
        /// Determines if this application supports multiple sessions.
        /// </summary>
        public bool MultipleSession {get;set;} = false;
        /// <summary>
        /// Determines if this <see cref="YorotApp"/> is a system app.
        /// </summary>
        public bool isSystemApp { get; set; }
        /// <summary>
        /// Figures out this <see cref="YorotApp"/> has open session(s).
        /// </summary>
        /// <returns><see cref="true"/> if this <see cref="YorotApp"/> has open session(s), otherwise <see cref="false"/>.</returns>
        public bool hasSessions()
        {
            return AssocForm != null && Duplicates.FindAll(i => i.AssocForm != null).Count > 0;
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
        /// Creates a copy of this <see cref="YorotApp"/> excluding <see cref="YorotApp.AssocForm"/>,<see cref="YorotApp.AssocTab"/> and <see cref="YorotApp.AssocPB"/>.
        /// </summary>
        /// <returns><see cref="YorotApp"/></returns>
        public YorotApp CreateCarbonCopy()
        {
            return new YorotApp()
            {
                AppIcon = AppIcon,
                isSystemApp = isSystemApp,
                AppCodeName = AppCodeName,
                isLocal = isLocal,
                HTUPDATE = HTUPDATE,
                AppName = AppName,
            };
        }
        /// <summary>
        /// Display name of application.
        /// </summary>
        public string AppName { get; set; }
        /// <summary>
        /// Gets associated <see cref="UI.frmApp"/> of this <see cref="YorotApp"/>. <see cref="null"/> if no forms are associated.
        /// </summary>
        public UI.frmApp AssocForm { get; set; }
        /// <summary>
        /// Gets associated <see cref="System.Windows.Forms.TabPage"/> of this <see cref="YorotApp"/>. <see cref="null"/> if no tabs are associated.
        /// </summary>
        public System.Windows.Forms.TabPage AssocTab { get; set; }
        /// <summary>
        /// Gets associated <see cref="System.Windows.Forms.PictureBox"/> of this <see cref="YorotApp"/>. <see cref="null"/> if no PBs are associated.
        /// </summary>
        public System.Windows.Forms.PictureBox AssocPB { get; set; }
        /// <summary>
        /// Determines if <see cref="AssocForm"/> is suspended.
        /// </summary>
        public bool isLayoutSuspended { get; set; } = false;
    }
}
