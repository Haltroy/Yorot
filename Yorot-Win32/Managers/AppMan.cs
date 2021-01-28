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
            if (!string.IsNullOrWhiteSpace(configFile)) { }
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
            UpdateCount++;
        }
        /// <summary>
        /// Serves information of update count. Used in App menu refreshment
        /// </summary>
        public int UpdateCount { get; set; } = 0;
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
            Version = YorotGlobal.Version,
            VersionNo = YorotGlobal.VersionNo,
            MultipleSession = true,
            AppOrigin = YorotAppOrigin.Embedded,
            Author = "Haltroy",
            AppOriginInfo = YorotGlobal.DefaultaAppOriginPlaceHolder + "frmMain*.cs",
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
            Version = YorotGlobal.Version,
            VersionNo = YorotGlobal.VersionNo,
            MultipleSession = true,
            AppOrigin = YorotAppOrigin.Embedded,
            Author = "Haltroy",
            AppOriginInfo = YorotGlobal.DefaultaAppOriginPlaceHolder + "SystemApps/settings*.cs",
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
            Version = YorotGlobal.Version,
            VersionNo = YorotGlobal.VersionNo,
            MultipleSession = true,
            AppOrigin = YorotAppOrigin.Embedded,
            Author = "Haltroy",
            AppOriginInfo = YorotGlobal.DefaultaAppOriginPlaceHolder + "SystemApps/store*.cs",
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
            Version = YorotGlobal.Version,
            VersionNo = YorotGlobal.VersionNo,
            MultipleSession = true,
            AppOrigin = YorotAppOrigin.Embedded,
            Author = "Haltroy",
            AppOriginInfo = YorotGlobal.DefaultaAppOriginPlaceHolder + "SystemApps/calc*.cs",
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
            Version = YorotGlobal.Version,
            VersionNo = YorotGlobal.VersionNo,
            MultipleSession = true,
            AppOrigin = YorotAppOrigin.Embedded,
            Author = "Haltroy",
            AppOriginInfo = YorotGlobal.DefaultaAppOriginPlaceHolder + "SystemApps/calendar*.cs",
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
            Version = YorotGlobal.Version,
            VersionNo = YorotGlobal.VersionNo,
            MultipleSession = true,
            AppOrigin = YorotAppOrigin.Embedded,
            Author = "Haltroy",
            AppOriginInfo = YorotGlobal.DefaultaAppOriginPlaceHolder + "SystemApps/notepad*.cs",
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
            Version = YorotGlobal.Version,
            VersionNo = YorotGlobal.VersionNo,
            MultipleSession = true,
            AppOrigin = YorotAppOrigin.Embedded,
            Author = "Haltroy",
            AppOriginInfo = YorotGlobal.DefaultaAppOriginPlaceHolder + "SystemApps/console*.cs",
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
            Version = YorotGlobal.Version,
            VersionNo = YorotGlobal.VersionNo,
            MultipleSession = true,
            AppOrigin = YorotAppOrigin.Embedded,
            Author = "Haltroy",
            AppOriginInfo = YorotGlobal.DefaultaAppOriginPlaceHolder + "SystemApps/colman*.cs",
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
            Version = YorotGlobal.Version,
            VersionNo = YorotGlobal.VersionNo,
            MultipleSession = true,
            AppOrigin = YorotAppOrigin.Embedded,
            Author = "Haltroy",
            AppOriginInfo = YorotGlobal.DefaultaAppOriginPlaceHolder + "SystemApps/fileman*.cs",
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
            Version = YorotGlobal.Version,
            VersionNo = YorotGlobal.VersionNo,
            MultipleSession = true,
            AppOrigin = YorotAppOrigin.Embedded,
            Author = "Haltroy",
            AppOriginInfo = YorotGlobal.DefaultaAppOriginPlaceHolder + "SystemApps/yopad*.cs",
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
            Version = YorotGlobal.Version,
            VersionNo = YorotGlobal.VersionNo,
            MultipleSession = true,
            AppOrigin = YorotAppOrigin.Embedded,
            Author = "Haltroy",
            AppOriginInfo = YorotGlobal.DefaultaAppOriginPlaceHolder + "SystemApps/spacepass*.cs",
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
                return HTAlt.Tools.ReadFile(YorotGlobal.Settings.UserApps + AppCodeName + "\\" + AppIcon, System.Drawing.Imaging.ImageFormat.Png);
            }
        }
        /// <summary>
        /// Creator of this app.
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// Origin of aapp.
        /// </summary>
        public YorotAppOrigin AppOrigin { get; set; }
        /// <summary>
        /// Information about origin of this app.
        /// </summary>
        public string AppOriginInfo { get; set; }
        /// <summary>
        /// Display version of this app.
        /// </summary>
        public string Version { get; set; } = "0";
        /// <summary>
        /// Actual version of this app. Used by HTUPDATE.
        /// </summary>
        public int VersionNo { get; set; } = 0;
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
            return Layouts.FindAll(i => i.AssocForm != null).Count > 0;
        }
        /// <summary>
        /// Returns app size in bytes.
        /// </summary>
        public float AppSize
        {
            get
            {
                //TODO: Add App Sie detection.
                throw new NotImplementedException("TODO");
            }
        }
        public string GetAppSizeInfo(string bytes)
        {
            var size = AppSize;
            if (size > 1099511627776F) //TiB 
            {
                return (size / 1099511627776F) + " TiB (" + size + " " + bytes + ")"; 
            }
            else if (size > 1073741824F) //GiB 
            {
                return (size / 1073741824F) + " GiB (" + size + " " + bytes + ")";
            }
            else if (size > 1048576F) //MiB 
            {
                return (size / 1048576F) + " MiB (" + size + " " + bytes + ")";
            }
            else if (size > 1024F) // KiB
            {
                return (size / 1024F) + " KiB (" + size + " " + bytes + ")";
            }else
            {
                return size + " " + bytes;
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
        /// <see cref="true"/> if this app is pinned, otherwise <seealso cref="false"/>.
        /// </summary>
        public bool isPinned { get; set; } = false;
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
        /// List of layouts (sessions) for this app.
        /// </summary>
        public List<YorotAppLayout> Layouts { get; set; } = new List<YorotAppLayout>();
        /// <summary>
        /// Display name of application.
        /// </summary>
        public string AppName { get; set; }
    }
    public class YorotAppLayout
    {
        /// <summary>
        /// Ownner of this app layout.
        /// </summary>
        public YorotApp Parent { get; set; }
        /// <summary>
        /// Gets associated <see cref="UI.frmApp"/> of this <see cref="YorotApp"/>. <see cref="null"/> if no forms are associated.
        /// </summary>
        public UI.frmApp AssocForm { get; set; }
        /// <summary>
        /// Gets associated <see cref="System.Windows.Forms.TabPage"/> of this <see cref="YorotApp"/>. <see cref="null"/> if no tabs are associated.
        /// </summary>
        public System.Windows.Forms.TabPage AssocTab { get; set; }
        /// <summary>
        /// Gets associated <see cref="YAMItem"/> of this <see cref="YorotApp"/>. <see cref="null"/> if no YAMIs are associated.
        /// </summary>
        public YAMItem AssocItem { get; set; }
        /// <summary>
        /// <see cref="true"/> to ask this layout to reload app, otherwise <see cref="false"/>
        /// </summary>
        public bool waitLayoutRestart { get; set; } = false;
    }
    public enum YorotAppOrigin
    {
        /// <summary>
        /// App instalked from other software outside Yorot.
        /// </summary>
        Other,
        /// <summary>
        /// App installed using Yopad.
        /// </summary>
        Yopad,
        /// <summary>
        /// App downloaded and instaalled from store.
        /// </summary>
        Store,
        /// <summary>
        /// App embedded to Yorot.
        /// </summary>
        Embedded,
    }
}
