using HTAlt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Yorot
{
    public class Settings
    {
        // TODO: Add XML desc to empty ones
        public Settings(string appPath)
        {
            if (string.IsNullOrWhiteSpace(appPath)) { throw new ArgumentNullException("\"aappPath\" cannot be empty."); };
            if (!Directory.Exists(appPath)) { Directory.CreateDirectory(appPath); }
            if (!appPath.HasWriteAccess()) { throw new FileLoadException("Cannot access to path \"" + appPath + "\"."); }
            
            // Set paths.
            AppPath = appPath;
            UserLoc = AppPath + @"usr\";
            CacheLoc = UserLoc + @"cache\";
            LangLoc = UserLoc + @"lang\";
            ExtLoc = UserLoc + @"ext\";
            EngineLoc = UserLoc + @"engines\";
            ThemesLoc = UserLoc + @"themes\";
            UserSettings = UserLoc + @"usr.ycf";
            UserHistory = UserLoc + @"history.ycf";
            UserFavorites = UserLoc + @"favorites.ycf";
            UserDownloads = UserLoc + @"downloads.ycf";
            UserTheme = UserLoc + @"tman.ycf";
            UserExt = UserLoc + @"extman.ycf";
            UserApp = UserLoc + @"yam.ycf";
            UserApps = UserLoc + @"apps\";
            UserProfiles = UserLoc + @"profiles\";
            UserProfile = UserLoc + @"profiles.ycf";

            // Detect if Yorot Users folder is moved. If then, move info to new app path.
            if (File.Exists(appPath + @"\yorot.moved"))
            {
                var MOVED = HTAlt.Tools.ReadFile(appPath + @"\yorot.moved",Encoding.Unicode);
                if (!string.IsNullOrWhiteSpace(MOVED))
                {
                    if (MOVED.HasWriteAccess())
                    {
                        AppPath = MOVED;
                        UserLoc = MOVED + @"usr\";
                        CacheLoc = UserLoc + @"\cache\";
                        ThemesLoc = UserLoc + @"\themes\";
                        UserSettings = UserLoc + @"usr.ycf";
                        UserHistory = UserLoc + @"history.ycf";
                        UserFavorites = UserLoc + @"favorites.ycf";
                        UserDownloads = UserLoc + @"downloads.ycf";
                        UserTheme = UserLoc + @"tman.ycf";
                        UserExt = UserLoc + @"extman.ycf";
                        UserApp = UserLoc + @"yam.ycf";
                        UserApps = UserLoc + @"apps\";
                        LangLoc = UserLoc + @"lang\";
                        ExtLoc = UserLoc + @"ext\";
                        EngineLoc = UserLoc + @"engines\";
                        UserProfile = UserLoc + @"profiles.ycf";
                        UserProfiles = UserLoc + @"profiles\";
                    }else
                    {
                        Output.WriteLine("[Settings] Ignoring yorot.moved file. Cannot use path given in this file.", LogLevel.Warning);
                    }
                }else
                {
                    Output.WriteLine("[Settings] Ignoring yorot.moved file. File does not contains suitable path data.", LogLevel.Warning);
                }
            }
            LoadDefaults();
            LoadFromFile(UserSettings);
        }
        public void LoadFromFile(string fileLocation)
        {
            if (string.IsNullOrWhiteSpace(fileLocation))
            {
                Output.WriteLine("[Settings] Loaded default settings because file location is in invalid format.", LogLevel.Warning);
            }
            else
            {
                if (!File.Exists(fileLocation))
                {
                    Output.WriteLine("[Settings] Loaded default settings because file is empty.", LogLevel.Warning);
                }
                else
                {
                    try
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(HTAlt.Tools.ReadFile(UserSettings,Encoding.Unicode));
                        XmlNode root = Yorot.Tools.FindRoot(doc);
                        List<string> appliedSettings = new List<string>();
                        for(int i = 0; i< root.ChildNodes.Count; i++)
                        {
                            var node = root.ChildNodes[i];
                            switch(node.Name)
                            {
                                case "HomePage":
                                    if (appliedSettings.Contains(node.Name))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    HomePage = node.InnerXml.InnerXmlToString();
                                    break;
                                case "SearchEngine":
                                    if (appliedSettings.Contains(node.Name))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    if (node.Attributes["Name"] != null && node.Attributes["Url"] != null)
                                    {
                                        SearchEngine = new YorotSearchEngine(node.Attributes["Name"].Value, node.Attributes["Url"].Value);
                                    }
                                    else
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Invalid Search Engine format.",LogLevel.Warning);
                                    }

                                    break;
                                case "SearchEngines":
                                    if (appliedSettings.Contains(node.Name))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    for (int ı = 0; ı < node.ChildNodes.Count;ı++)
                                    {
                                        var subnode = node.ChildNodes[ı];
                                        if (subnode.Name == "Engine")
                                        {
                                            if (subnode.Attributes["Name"] != null && subnode.Attributes["Url"] != null)
                                            {
                                                if (!SearchEngineExists(subnode.Attributes["Name"].Value, subnode.Attributes["Url"].Value))
                                                {
                                                    SearchEngines.Add(new YorotSearchEngine(subnode.Attributes["Name"].Value, subnode.Attributes["Url"].Value));
                                                }else
                                                {
                                                    if (!subnode.OuterXml.StartsWith("<!--"))
                                                    {
                                                        Output.WriteLine("[Search Engine] Threw away \"" + subnode.OuterXml + "\". Search Engine already exists.", LogLevel.Warning);
                                                    }
                                                }
                                            }else
                                            {
                                                if (!subnode.OuterXml.StartsWith("<!--"))
                                                {
                                                    Output.WriteLine("[Search Engine] Threw away \"" + subnode.OuterXml + "\". Invalid format.", LogLevel.Warning);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (!subnode.OuterXml.StartsWith("<!--"))
                                            {
                                                Output.WriteLine("[Search Engine] Threw away \"" + subnode.OuterXml + "\". Invalid format.", LogLevel.Warning);
                                            }
                                        }
                                    }
                                    break;
                                case "WebEngines":
                                    if (appliedSettings.Contains(node.Name))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    for (int ı = 0; ı < node.ChildNodes.Count;ı++)
                                    {
                                        var subnode = node.ChildNodes[ı];
                                        if (subnode.Name == "Engine")
                                        {
                                            if (subnode.Attributes["Name"] != null && subnode.Attributes["Url"] != null)
                                            {
                                                if (!SearchEngineExists(subnode.Attributes["Name"].Value, subnode.Attributes["Url"].Value))
                                                {
                                                    WebEngines.Add(new YorotWebEngine(EngineLoc + subnode.Attributes["Name"].Value + @"\engine.ycf"));
                                                }
                                                else
                                                {
                                                    Output.WriteLine("[Web Engine] Threw away \"" + subnode.OuterXml + "\". Web Engine already exists.", LogLevel.Warning);
                                                }
                                            }
                                            else
                                            {
                                                if (!subnode.OuterXml.StartsWith("<!--"))
                                                {
                                                    Output.WriteLine("[Web Engine] Threw away \"" + subnode.OuterXml + "\". Invalid format.", LogLevel.Warning);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (!subnode.OuterXml.StartsWith("<!--"))
                                            {
                                                Output.WriteLine("[Web Engine] Threw away \"" + subnode.OuterXml + "\". Invalid format.", LogLevel.Warning);
                                            }
                                        }
                                    }
                                    break;
                                case "Lang":
                                    if (appliedSettings.Contains(node.Name))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    LangManager.LoadFromFile(node.InnerXml.InnerXmlToString().GetPath(AppPath));
                                    break;
                                case "RestoreOldSessions":
                                    if (appliedSettings.Contains(node.Name))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    RestoreOldSessions = node.InnerXml.InnerXmlToString() == "true";
                                    break;
                                case "RememberLastProxy":
                                    if (appliedSettings.Contains(node.Name))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    RememberLastProxy = node.InnerXml.InnerXmlToString() == "true";
                                    break;
                                case "DoNotTrack":
                                    if (appliedSettings.Contains(node.Name))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    DoNotTrack = node.InnerXml.InnerXmlToString() == "true";
                                    break;
                                case "ShowFavorites":
                                    if (appliedSettings.Contains(node.Name))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    FavManager.ShowFavorites = node.InnerXml.InnerXmlToString() == "true";
                                    break;
                                case "StartWithFullScreen":
                                    if (appliedSettings.Contains(node.Name))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    StartWithFullScreen = node.InnerXml.InnerXmlToString() == "true";
                                    break;
                                case "OpenFilesAfterDownload":
                                    if (appliedSettings.Contains(node.Name))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    DownloadManager.OpenFilesAfterDownload = node.InnerXml.InnerXmlToString() == "true";
                                    break;
                                case "AutoDownload":
                                    if (appliedSettings.Contains(node.Name))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    DownloadManager.AutoDownload = node.InnerXml.InnerXmlToString() == "true";
                                    break;
                                case "DownloadFolder":
                                    if (appliedSettings.Contains(node.Name))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    DownloadManager.DownloadFolder = node.InnerXml.InnerXmlToString();
                                    break;
                                case "AlwaysCheckDefaultBrowser":
                                    if (appliedSettings.Contains(node.Name))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    AlwaysCheckDefaultBrowser = node.InnerXml.InnerXmlToString() == "true";
                                    break;
                                case "StartOnBoot":
                                    if (appliedSettings.Contains(node.Name))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    StartOnBoot = node.InnerXml.InnerXmlToString() == "true";
                                    break;
                                case "StartInSystemTray":
                                    if (appliedSettings.Contains(node.Name))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    StartInSystemTray = node.InnerXml.InnerXmlToString() == "true";
                                    break;
                                case "NotifPlaySound":
                                    if (appliedSettings.Contains(node.Name))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    NotifPlaySound = node.InnerXml.InnerXmlToString() == "true";
                                    break;
                                case "NotifUseDefault":
                                    if (appliedSettings.Contains(node.Name))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    NotifUseDefault = node.InnerXml.InnerXmlToString() == "true";
                                    break;
                                case "NotifSoundLoc":
                                    if (appliedSettings.Contains(node.Name))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    NotifSoundLoc = node.InnerXml.InnerXmlToString();
                                    break;
                                case "NotifSilent":
                                    if (appliedSettings.Contains(node.Name))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    NotifSilent = node.InnerXml.InnerXmlToString() == "true";
                                    break;
                                default:
                                    if (!node.OuterXml.StartsWith("<!--"))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Unsupported.", LogLevel.Warning);
                                    }
                                    break;
                            }
                        }
                    }
                    catch (XmlException)
                    {
                        Output.WriteLine("[Settings] Loaded default settings because file is in invalid format.", LogLevel.Warning);
                    }
                    catch (Exception ex)
                    {
                        Output.WriteLine("[Settings] Loaded default settings because of this exception:" + Environment.NewLine + ex.ToString(), LogLevel.Warning);
                    }
                }
            }
        }

        public string ToXml()
        {
            string x = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" + Environment.NewLine +
                "<root>" + Environment.NewLine +
                "<!-- Yorot User File" + Environment.NewLine + Environment.NewLine +
                "This file is used by Yorot to get user information." + Environment.NewLine +
                "Editing this file might cause problems within Yorot and" + Environment.NewLine +
                "other apps and extensions." + Environment.NewLine +
                "-->" + Environment.NewLine;
            x += "<HomePage>" + HomePage.ToXML() + "</HomePage>" + Environment.NewLine;
            x += "<SearchEngine Name=\"" + SearchEngine.Name.ToXML() + "\" Url=\"" + SearchEngine.Url.ToXML() + "\" />" + Environment.NewLine;
            if (SearchEngines.Count > 0)
            {
                x += "<SearchEngines>" + Environment.NewLine;
                for (int i = 0; i < SearchEngines.Count; i++)
                {
                    if (!SearchEngines[i].comesWithYorot)
                    {
                        x += "<Engine Name=\"" + SearchEngines[i].Name.ToXML() + "\" Url=\"" + SearchEngines[i].Url.ToXML() + "\" />" + Environment.NewLine;
                    }
                }
                x += "</SearchEngines>" + Environment.NewLine;
            }
            if (WebEngines.Count > 0)
            {
                x += "<WebEngines>" + Environment.NewLine;
                for (int i = 0; i < WebEngines.Count; i++)
                {
                    x += "<Engine Name=\"" + WebEngines[i].CodeName.ToXML() + "\" />" + Environment.NewLine;
                }
                x += "</WebEngines>" + Environment.NewLine;
            }
            x += "<RestoreOldSessions>" + (RestoreOldSessions ? "true" : "false") + "</RestoreOldSessions>" + Environment.NewLine;
            x += "<RememberLastProxy>" + (RememberLastProxy ? "true" : "false") + "</RememberLastProxy>" + Environment.NewLine;
            x += "<Lang>" + LangManager.LoadedLangFile.ToXML().ShortenPath(AppPath) + "</Lang>" + Environment.NewLine;
            x += "<DoNotTrack>" + (DoNotTrack ? "true" : "false") + "</DoNotTrack>" + Environment.NewLine;
            x += "<ShowFavorites>" + (FavManager.ShowFavorites ? "true" : "false") + "</ShowFavorites>" + Environment.NewLine;
            x += "<StartWithFullScreen>" + (StartWithFullScreen ? "true" : "false") + "</StartWithFullScreen>" + Environment.NewLine;
            x += "<OpenFilesAfterDownload>" + (DownloadManager.OpenFilesAfterDownload ? "true" : "false") + "</OpenFilesAfterDownload>" + Environment.NewLine;
            x += "<AutoDownload>" + (DownloadManager.AutoDownload ? "true" : "false") + "</AutoDownload>" + Environment.NewLine;
            x += "<AlwaysCheckDefaultBrowser>" + (AlwaysCheckDefaultBrowser ? "true" : "false") + "</AlwaysCheckDefaultBrowser>" + Environment.NewLine;
            x += "<StartOnBoot>" + (StartOnBoot ? "true" : "false") + "</StartOnBoot>" + Environment.NewLine;
            x += "<StartInSystemTray>" + (StartInSystemTray ? "true" : "false") + "</StartInSystemTray>" + Environment.NewLine;
            x += "<NotifPlaySound>" + (NotifPlaySound ? "true" : "false") + "</NotifPlaySound>" + Environment.NewLine;
            x += "<NotifUseDefault>" + (NotifUseDefault ? "true" : "false") + "</NotifUseDefault>" + Environment.NewLine;
            x += "<NotifSilent>" + (NotifSilent ? "true" : "false") + "</NotifSilent>" + Environment.NewLine;
            x += "<DownloadFolder>" + DownloadManager.DownloadFolder.ToXML() + "</DownloadFolder>" + Environment.NewLine;
            x += "<NotifSoundLoc>" + NotifSoundLoc.ToXML() + "</NotifSoundLoc>" + Environment.NewLine;
            return (x + Environment.NewLine + "</root>").BeautifyXML();
        }

        public void LoadDefaults()
        {

            if (!Directory.Exists(UserLoc))
            {
                Directory.CreateDirectory(UserLoc);
            }
            if (!Directory.Exists(CacheLoc))
            {
                Directory.CreateDirectory(CacheLoc);
            }
            if (!Directory.Exists(UserApps))
            {
                Directory.CreateDirectory(UserApps);
            }
            if (!Directory.Exists(ThemesLoc))
            {
                Directory.CreateDirectory(ThemesLoc);
            }
            if (!Directory.Exists(LangLoc))
            {
                Directory.CreateDirectory(LangLoc);
            }
            if (!Directory.Exists(ExtLoc))
            {
                Directory.CreateDirectory(ExtLoc);
            }
            if (!Directory.Exists(EngineLoc))
            {
                Directory.CreateDirectory(EngineLoc);
            }
            AppMan = new AppManager(UserApp) { Settings = this};
            ThemeMan = new ThemeManager(UserTheme) { Settings = this };
            DownloadManager = new DownloadManager(UserDownloads) { Settings = this };
            HistoryManager = new HistoryManager(UserHistory) { Settings = this };
            FavManager = new FavMan() { Settings = this };
            LangManager = new LangManager() { Settings = this };
            ProfileManager = new ProfileManager() { Settings = this };
            Extensions = new ExtensionManager(UserExt) { Settings = this };
            HomePage = "yorot://newtab";
            // BEGIN: Search Engines
            SearchEngines.Clear();
            SearchEngine = new YorotSearchEngine("Google", "https://www.google.com/search?q=") { comesWithYorot = true };
            SearchEngines.Add(SearchEngine);
            SearchEngines.Add(new YorotSearchEngine("Yandex", "https://yandex.com.tr/search/?lr=103873&text=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("Bing", "https://www.bing.com/search?q=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("Yaani", "https://www.yaani.com/#q=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("DuckDuckGo", "https://duckduckgo.com/?q=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("Baidu", "https://www.baidu.com/s?&wd=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("WolframAlpha", "https://www.wolframalpha.com/input/?i=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("AOL", "https://search.aol.com/aol/search?&q=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("Yahoo", "https://search.yahoo.com/search?p=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("Ask", "https://www.ask.com/web?q=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("Archive.org", "https://web.archive.org/web/*/") { comesWithYorot = true });
            // END: Search Engines
            RestoreOldSessions = false;
            RememberLastProxy = false;
            DoNotTrack = true;
            FavManager.ShowFavorites = true;
            StartWithFullScreen = false;
            DownloadManager.OpenFilesAfterDownload = false;
            DownloadManager.AutoDownload = true;
            DownloadManager.DownloadFolder = UserLoc + @"Downloads\";
            AlwaysCheckDefaultBrowser = true;
            StartOnBoot = false;
            StartInSystemTray = false;
            NotifPlaySound = true;
            NotifSilent = false;
            NotifUseDefault = true;
            NotifSoundLoc =  @"RES\n.ogg";
        }

        public void Save()
        {
            ThemeMan.Save();
            AppMan.Save();
            HistoryManager.Save();
            FavManager.Save();
            ProfileManager.Save();
            Extensions.Save();
            DownloadManager.Save();
            HTAlt.Tools.WriteFile(UserSettings, ToXml(), Encoding.Unicode);
        }

        public AppManager AppMan { get; set; }
        public ThemeManager ThemeMan { get; set; }
        public DownloadManager DownloadManager { get; set; }
        public HistoryManager HistoryManager { get; set; }
        public FavMan FavManager { get; set; }
        public LangManager LangManager { get; set; }
        public ProfileManager ProfileManager { get; set; }
        public ExtensionManager Extensions { get; set; }
        public bool SearchEngineExists(string name, string url)
        {
            return SearchEngines.FindAll(i => i.Name == name && i.Url == url).Count > 0;
        }
        public string HomePage { get; set; } = "";
        public YorotSearchEngine SearchEngine { get; set; } = null;
        public List<YorotSearchEngine> SearchEngines { get; set; } = new List<YorotSearchEngine>();
        public List<YorotWebEngine> WebEngines { get; set; } = new List<YorotWebEngine>();
        public bool RestoreOldSessions { get; set; } = false;
        public bool RememberLastProxy { get; set; } = false;
        public bool DoNotTrack { get; set; } = true;
        public bool StartWithFullScreen { get; set; } = false;
        public bool AlwaysCheckDefaultBrowser { get; set; } = true;
        public bool StartOnBoot { get; set; } = false;
        public bool StartInSystemTray { get; set; } = true;
        public bool NotifPlaySound { get; set; } = true;
        public bool NotifUseDefault { get; set; } = true;
        public string NotifSoundLoc { get; set; } = "";
        public bool NotifSilent { get; set; } = false;
        /// <summary>
        /// Location of application files.
        /// </summary>
        public string AppPath { get; set; }
        /// <summary>
        /// User Files location.
        /// </summary>
        public string UserLoc { get; set; }
        /// <summary>
        /// User Cache location.
        /// </summary>
        public  string CacheLoc { get; set; }
        /// <summary>
        /// User Language folder
        /// </summary>
        public string LangLoc { get; set; }
        /// <summary>
        /// User Extensions folder
        /// </summary>
        public string ExtLoc { get; set; }
        /// <summary>
        /// User Web Engines folder
        /// </summary>
        public string EngineLoc { get; set; }
        /// <summary>
        /// User Themes location.
        /// </summary>
        public string ThemesLoc { get; set; }
        /// <summary>
        /// User settings location.
        /// </summary>
        public string UserSettings { get; set; }
        /// <summary>
        /// History Manager configuration file location.
        /// </summary>
        public string UserHistory { get; set; }
        /// <summary>
        /// Favorites Manager configuration file location.
        /// </summary>
        public string UserFavorites { get; set; }
        /// <summary>
        /// Downloads Manager configuration file location.
        /// </summary>
        public string UserDownloads { get; set; }
        /// <summary>
        /// Themes Manager configuration file location.
        /// </summary>
        public string UserTheme { get; set; }
        /// <summary>
        /// Extension Manager configuration file location.
        /// </summary>
        public string UserExt { get; set; }
        /// <summary>
        /// Yorot App Manager configuration file location.
        /// </summary>
        public string UserApp { get; set; }
        /// <summary>
        /// Yorot App Manager Application storage.
        /// </summary>
        public string UserApps { get; set; }
        /// <summary>
        /// User profiles folder.
        /// </summary>
        public string UserProfiles { get; set; }
        /// <summary>
        /// User profiles configuration file.
        /// </summary>
        public string UserProfile { get; set; }

    }
    public class YorotSearchEngine
    {
        public YorotSearchEngine(string name,string url)
        {
            Name = name;
            Url = url;
        }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool comesWithYorot { get; set; } = false;
    }
    /// <summary>
    /// Yorot Web Engine
    /// </summary>
    public class YorotWebEngine
    {
        /// <summary>
        /// Creates a new <see cref="YorotWebEngine"/>.
        /// </summary>
        /// <param name="configFile">Location of configuration file for this engine.</param>
        public YorotWebEngine(string configFile)
        {
            // TODO:
        }
        /// <summary>
        /// HTUPDATE address of this engine.
        /// </summary>
        public string HTUPDATE { get; set; }
        /// <summary>
        /// Location of thi engine in locaal drive.
        /// </summary>
        public string EngineLoc { get; set; }
        /// <summary>
        /// Current version of this engine.
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// Name of this engine.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Codename of the engine.
        /// </summary>
        public string CodeName { get; set; }
        /// <summary>
        /// Description of this engine.
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// Location of this engine's logo on local drive.
        /// </summary>
        public string IconLoc { get; set; }
        /// <summary>
        /// Gets the engine's logo.
        /// </summary>
        public System.Drawing.Image Icon => HTAlt.Tools.ReadFile(IconLoc, System.Drawing.Imaging.ImageFormat.Png);
    }
}
