using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

namespace Yorot
{
    /// <summary>
    /// Yorot App Manager
    /// </summary>
    public class AppManager
    {
        public AppManager(string configFile)
        {
            if (!string.IsNullOrWhiteSpace(configFile)) 
            {
                if (System.IO.File.Exists(configFile))
                {
                    try
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(HTAlt.Tools.ReadFile(configFile, System.Text.Encoding.Unicode));
                        XmlNode rootNode = Yorot.Tools.FindRoot(doc.DocumentElement);
                        List<string> appliedSettings = new List<string>();
                        for (int i = 0;i < rootNode.ChildNodes.Count;i++)
                        {
                            var node = rootNode.ChildNodes[i];
                            switch(node.Name)
                            {
                                case "Apps":
                                    if (appliedSettings.FindAll(it => it == node.Name).Count > 0)
                                    {
                                        Output.WriteLine("[AppMan] Threw away \"" + node.OuterXml + "\". Configurtion already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    for(int ı = 0; ı < node.ChildNodes.Count;ı++)
                                    {
                                        var subnode = node.ChildNodes[ı];
                                        if (subnode.Name == "App" && subnode.Attributes["CodeName"] != null)
                                        {
                                            if (Apps.FindAll(it => it.AppCodeName == subnode.Attributes["CodeName"].Value).Count > 0) 
                                            {
                                                Output.WriteLine("[AppMan] Threw away \"" + subnode.OuterXml + "\". App already installed.", LogLevel.Warning);
                                            }
                                            else
                                            {
                                                YorotApp app = new YorotApp(subnode.Attributes["CodeName"].Value,this);
                                                if (subnode.Attributes["Pinned"] != null)
                                                {
                                                    app.isPinned = subnode.Attributes["Pinned"].Value == "true";
                                                }
                                                if (subnode.Attributes["Origin"] != null && subnode.Attributes["OriginInfo"] != null)
                                                {
                                                    app.AppOrigin = (YorotAppOrigin)(int.Parse(subnode.Attributes["Origin"].Value));
                                                    app.AppOriginInfo = subnode.Attributes["OriginInfo"].Value.Replace("[NEWLINE]",Environment.NewLine);
                                                }
                                                else
                                                {
                                                    app.AppOrigin = YorotAppOrigin.Unknown;
                                                    app.AppOriginInfo = "Xml node responsible for this app did not include \"Origin\" and \"OriginInfo\" attributes in config file \"" + configFile + "\".";
                                                }
                                                Apps.Add(app);
                                            }
                                        }else
                                        {
                                            if (!subnode.OuterXml.StartsWith("<!--"))
                                            {
                                                Output.WriteLine("[AppMan] Threw away \"" + subnode.OuterXml + "\". Invalid format.", LogLevel.Warning);
                                            }
                                        }
                                    }
                                    break;
                                default:
                                    if (!node.OuterXml.StartsWith("<!--"))
                                    {
                                        Output.WriteLine("[AppMan] Threw away \"" + node.OuterXml + "\". Invalid configurtion.", LogLevel.Warning);
                                    }
                                    break;
                            }
                        }
                    }catch (XmlException)
                    {
                        Output.WriteLine("[AppMan] Loaded defaults because configuration file has error(s).", LogLevel.Warning);
                    }
                    catch(Exception ex)
                    {
                        Output.WriteLine("[AppMan] Loaded defaults because of this error: " + Environment.NewLine + ex.ToString(), LogLevel.Warning);
                    }
                }else
                {
                    Output.WriteLine("[AppMan] Loaded defaults, cannot access configuration file.", LogLevel.Warning);
                }
            }else
            {
                Output.WriteLine("[AppMan] Loaded defaults because configuration file path was empty.", LogLevel.Warning);
            }
            
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
            ClaimMan();
            UpdateCount++;
        }
        private void ClaimMan()
        {
            for(int i =0;i < Apps.Count;i++)
            {
                Apps[i].Manager = this;
            }
        }
        /// <summary>
        /// Retrieves configuration file content
        /// </summary>
        /// <returns></returns>
        public string ToXml()
        {
            string x = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" + Environment.NewLine +
                "<root>" + Environment.NewLine +
                "<!-- Yorot Apps Config File" + Environment.NewLine + Environment.NewLine +
                "This file is used to configure applications." + Environment.NewLine +
                "Editing this file might cause problems with apps." + Environment.NewLine +
                "-->" + Environment.NewLine +
                "<Apps>" + Environment.NewLine;
            for (int i = 0; i < Apps.Count; i++)
            {
                var app = Apps[i];
                if (!app.isSystemApp)
                {
                    x += "<App CodeName=\"" + app.AppCodeName + "\" Origin=\"" + app.AppOrigin.ToString() + "\" OriginInfo=\"" + app.AppOriginInfo.Replace(Environment.NewLine,"[NEWLINE]") + "\" />" + Environment.NewLine;
                }
            }
            return Yorot.Tools.PrintXML(x + "</Apps>" + Environment.NewLine + "</root>");
        }
        /// <summary>
        /// Saves configuration.
        /// </summary>
        public void Save()
        {
            HTAlt.Tools.WriteFile(Settings.UserApp, ToXml(), System.Text.Encoding.Unicode);
        }
         /// <summary>
        /// Main settings for Yorot.
        /// </summary>
        public Settings Settings { get; set; }
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
            Version = "1",
            VersionNo = 1,
            MultipleSession = true,
            AppOrigin = YorotAppOrigin.Embedded,
            Author = "Haltroy",
            AppOriginInfo = "frmMain*.cs",
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
            Version = "1",
            VersionNo = 1,
            MultipleSession = true,
            AppOrigin = YorotAppOrigin.Embedded,
            Author = "Haltroy",
            AppOriginInfo = "SystemApps/settings*.cs",
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
            Version = "1",
            VersionNo = 1,
            MultipleSession = true,
            AppOrigin = YorotAppOrigin.Embedded,
            Author = "Haltroy",
            AppOriginInfo = "SystemApps/store*.cs",
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
            Version = "1",
            VersionNo = 1,
            MultipleSession = true,
            AppOrigin = YorotAppOrigin.Embedded,
            Author = "Haltroy",
            AppOriginInfo = "SystemApps/calc*.cs",
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
            Version = "1",
            VersionNo = 1,
            MultipleSession = true,
            AppOrigin = YorotAppOrigin.Embedded,
            Author = "Haltroy",
            AppOriginInfo = "SystemApps/calendar*.cs",
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
            Version = "1",
            VersionNo = 1,
            MultipleSession = true,
            AppOrigin = YorotAppOrigin.Embedded,
            Author = "Haltroy",
            AppOriginInfo = "SystemApps/notepad*.cs",
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
            Version = "1",
            VersionNo = 1,
            MultipleSession = true,
            AppOrigin = YorotAppOrigin.Embedded,
            Author = "Haltroy",
            AppOriginInfo = "SystemApps/console*.cs",
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
            Version = "1",
            VersionNo = 1,
            MultipleSession = true,
            AppOrigin = YorotAppOrigin.Embedded,
            Author = "Haltroy",
            AppOriginInfo = "SystemApps/colman*.cs",
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
            Version = "1",
            VersionNo = 1,
            MultipleSession = true,
            AppOrigin = YorotAppOrigin.Embedded,
            Author = "Haltroy",
            AppOriginInfo = "SystemApps/fileman*.cs",
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
            Version = "1",
            VersionNo = 1,
            MultipleSession = true,
            AppOrigin = YorotAppOrigin.Embedded,
            Author = "Haltroy",
            AppOriginInfo = "SystemApps/yopad*.cs",
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
            Version = "1",
            VersionNo = 1,
            MultipleSession = true,
            AppOrigin = YorotAppOrigin.Embedded,
            Author = "Haltroy",
            AppOriginInfo = "SystemApps/spacepass*.cs",
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
        public YorotApp(string appCodeName,AppManager manager)
        {
            AppCodeName = appCodeName;
            Manager = manager;
            string configFile = Manager.Settings.UserApps + appCodeName + "\\app.ycf";
            if (!string.IsNullOrWhiteSpace(configFile))
            {
                if (System.IO.File.Exists(configFile))
                {
                    try
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(HTAlt.Tools.ReadFile(configFile, System.Text.Encoding.Unicode));
                        XmlNode rootNode = Yorot.Tools.FindRoot(doc.DocumentElement);
                        List<string> appliedSettings = new List<string>();
                        for (int i = 0; i < rootNode.ChildNodes.Count;i++)
                        {
                            var node = rootNode.ChildNodes[i];
                            switch(node.Name) {
                                case "AppIcon": 
                                    if (appliedSettings.FindAll(it => it == node.Name).Count > 0)
                                    {
                                        Output.WriteLine("[YorotApp] Threw away \"" + node.OuterXml + "\", configuration already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    break;
                                case "Author":
                                    if (appliedSettings.FindAll(it => it == node.Name).Count > 0)
                                    {
                                        Output.WriteLine("[YorotApp] Threw away \"" + node.OuterXml + "\", configuration already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    break;
                                case "Version":
                                    if (appliedSettings.FindAll(it => it == node.Name).Count > 0)
                                    {
                                        Output.WriteLine("[YorotApp] Threw away \"" + node.OuterXml + "\", configuration already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    break;
                                case "VersionNo":
                                    if (appliedSettings.FindAll(it => it == node.Name).Count > 0)
                                    {
                                        Output.WriteLine("[YorotApp] Threw away \"" + node.OuterXml + "\", configuration already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    break;
                                case "HTUPDATE":
                                    if (appliedSettings.FindAll(it => it == node.Name).Count > 0)
                                    {
                                        Output.WriteLine("[YorotApp] Threw away \"" + node.OuterXml + "\", configuration already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    break;
                                case "StartFile":
                                    if (appliedSettings.FindAll(it => it == node.Name).Count > 0)
                                    {
                                        Output.WriteLine("[YorotApp] Threw away \"" + node.OuterXml + "\", configuration already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    break;
                                case "AppCodeName":
                                    if (appliedSettings.FindAll(it => it == node.Name).Count > 0)
                                    {
                                        Output.WriteLine("[YorotApp] Threw away \"" + node.OuterXml + "\", configuration already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    break;
                                case "isLocal":
                                    if (appliedSettings.FindAll(it => it == node.Name).Count > 0)
                                    {
                                        Output.WriteLine("[YorotApp] Threw away \"" + node.OuterXml + "\", configuration already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    break;
                                case "AppName":
                                    if (appliedSettings.FindAll(it => it == node.Name).Count > 0)
                                    {
                                        Output.WriteLine("[YorotApp] Threw away \"" + node.OuterXml + "\", configuration already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    break;
                                case "MultipleSession":
                                    if (appliedSettings.FindAll(it => it == node.Name).Count > 0)
                                    {
                                        Output.WriteLine("[YorotApp] Threw away \"" + node.OuterXml + "\", configuration already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    break;
                                default:
                                    Output.WriteLine("[YorotApp] Threw away \"" + node.OuterXml + "\", Invalid configuration.", LogLevel.Warning);
                                    break;
                            }
                        }
                    }catch (XmlException xex)
                    {
                        Error = xex;
                    }catch(Exception ex)
                    {
                        Error = ex;
                    }
                }else
                {
                    Error = new ArgumentNullException("Configuration file does not exists.");
                    Output.WriteLine("[YorotApp] Cannot load app \"" + appCodeName + "\", configuration file does not exists.",LogLevel.Error);
                }
            }else
            {
                Error = new ArgumentNullException("Cone name string was empty.");
                Output.WriteLine("[YorotApp] Cannot load app, codename was empty.", LogLevel.Error);
            }
            // Todo: Loaad config
        }
        /// <summary>
        /// Determines if this application had error(s) while loading.
        /// </summary>
        public Exception Error { get; set; }
        /// <summary>
        /// Creates new <see cref="YorotApp"/>.
        /// </summary>
        public YorotApp() { }
        /// <summary>
        /// gets the main manager of this app.
        /// </summary>
        public AppManager Manager { get; set; }
        /// <summary>
        /// Icon location of app.
        /// </summary>
        public string AppIcon { get; set; }
        
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
        /// URL of HTUPDATE file for this app.
        /// </summary>
        public string HTUPDATE { get; set; }
        /// <summary>
        /// Name of file (or URL) when loaded while starting app.
        /// </summary>
        public string StartFile { get; set; }
        /// <summary>
        /// Codename of app.
        /// </summary>
        public string AppCodeName { get; set; }
        /// <summary>
        /// <see cref="true"/> if app is locally saved, otherwise <see cref="false"/>.
        /// </summary>
        public bool isLocal { get; set; }
        /// <summary>
        /// Display name of application.
        /// </summary>
        public string AppName { get; set; }
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
            return Layouts.FindAll(i => i.hasSessions).Count > 0;
        }
        /// <summary>
        /// Returns app size in bytes.
        /// </summary>+
        public long AppSize
        {
            get
            {
                return Yorot.Tools.DirSize(new System.IO.DirectoryInfo(Manager.Settings.UserApps + AppCodeName));
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
        /// <see cref="true"/> if this app is pinned, otherwise <seealso cref="false"/>.
        /// </summary>
        public bool isPinned { get; set; } = false;
        
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
    }
    public abstract class YorotAppLayout
    {
        /// <summary>
        /// Ownner of this app layout.
        /// </summary>
        public YorotApp Parent { get; set; }
        /// <summary>
        /// <see cref="true"/> to ask this layout to reload app, otherwise <see cref="false"/>
        /// </summary>
        public bool waitLayoutRestart { get; set; } = false;
        public bool hasSessions { get; set; } = false;
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
        /// <summary>
        /// No information given about this app origin.
        /// </summary>
        Unknown,
    }
}
