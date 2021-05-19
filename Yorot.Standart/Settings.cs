using HTAlt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Yorot
{
    /// <summary>
    /// Yorot User Settings Class
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Creates a new Settings instance
        /// </summary>
        /// <param name="profile"><see cref="YorotProfile"/></param>
        public Settings(YorotProfile profile)
        {
            Profile = profile;
            switch (profile.Name)
            {
                case "root":
                    LoadDefaults(true);
                    break;

                default:
                    LoadDefaults(false);
                    LoadFromFile(profile.UserSettings);
                    break;
            }
        }

        /// <summary>
        /// Loads configuration.
        /// </summary>
        /// <param name="fileLocation">Location of the settings configuration file on drive.</param>
        private void LoadFromFile(string fileLocation)
        {
            if (string.IsNullOrWhiteSpace(fileLocation))
            {
                Output.WriteLine("[Settings] Loaded default settings because file location is in unsupported.", LogLevel.Warning);
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
                        doc.LoadXml(HTAlt.Tools.ReadFile(fileLocation, Encoding.Unicode));
                        XmlNode root = Yorot.Tools.FindRoot(doc);
                        List<string> appliedSettings = new List<string>();
                        for (int i = 0; i < root.ChildNodes.Count; i++)
                        {
                            XmlNode node = root.ChildNodes[i];
                            switch (node.Name.ToLowerEnglish())
                            {
                                case "homepage":
                                    if (appliedSettings.Contains(node.Name.ToLowerEnglish()))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name.ToLowerEnglish());
                                    HomePage = node.InnerXml.InnerXmlToString();
                                    break;

                                case "searchengines":
                                    if (appliedSettings.Contains(node.Name.ToLowerEnglish()))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name.ToLowerEnglish());
                                    for (int ı = 0; ı < node.ChildNodes.Count; ı++)
                                    {
                                        XmlNode subnode = node.ChildNodes[ı];
                                        if (subnode.Name.ToLowerEnglish() == "engine")
                                        {
                                            if (subnode.Attributes["Name"] != null && subnode.Attributes["Url"] != null)
                                            {
                                                if (!SearchEngineExists(subnode.Attributes["Name"].Value, subnode.Attributes["Url"].Value))
                                                {
                                                    SearchEngines.Add(new YorotSearchEngine(subnode.Attributes["Name"].Value, subnode.Attributes["Url"].Value));
                                                }
                                                else
                                                {
                                                    if (!subnode.OuterXml.StartsWith("<!--"))
                                                    {
                                                        Output.WriteLine("[SearchEngine] Threw away \"" + subnode.OuterXml + "\". Search Engine already exists.", LogLevel.Warning);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (!subnode.OuterXml.StartsWith("<!--"))
                                                {
                                                    Output.WriteLine("[SearchEngine] Threw away \"" + subnode.OuterXml + "\". unsupported.", LogLevel.Warning);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (!subnode.OuterXml.StartsWith("<!--"))
                                            {
                                                Output.WriteLine("[SearchEngine] Threw away \"" + subnode.OuterXml + "\". unsupported.", LogLevel.Warning);
                                            }
                                        }
                                    }
                                    if (node.Attributes["Selected"] != null)
                                    {
                                        SearchEngine = SearchEngines[int.Parse(node.Attributes["Selected"].Value.InnerXmlToString())];
                                    }
                                    else
                                    {
                                        SearchEngine = SearchEngines[0];
                                    }
                                    break;

                                case "webengines":
                                    if (appliedSettings.Contains(node.Name.ToLowerEnglish()))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name.ToLowerEnglish());
                                    for (int ı = 0; ı < node.ChildNodes.Count; ı++)
                                    {
                                        XmlNode subnode = node.ChildNodes[ı];
                                        if (subnode.Name.ToLowerEnglish() == "engine")
                                        {
                                            if (subnode.Attributes["Name"] != null)
                                            {
                                                if (Profile.Manager.Main.WebEngineMan.WEExists(subnode.Attributes["Name"].Value))
                                                {
                                                    Profile.Manager.Main.WebEngineMan.Enable(subnode.Attributes["Name"].Value);
                                                }
                                                else
                                                {
                                                    Output.WriteLine("[Web Engine] Threw away \"" + subnode.OuterXml + "\". Web Engine does not exists.", LogLevel.Warning);
                                                }
                                            }
                                            else
                                            {
                                                if (!subnode.OuterXml.StartsWith("<!--"))
                                                {
                                                    Output.WriteLine("[Web Engine] Threw away \"" + subnode.OuterXml + "\". unsupported.", LogLevel.Warning);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (!subnode.OuterXml.StartsWith("<!--"))
                                            {
                                                Output.WriteLine("[Web Engine] Threw away \"" + subnode.OuterXml + "\". unsupported.", LogLevel.Warning);
                                            }
                                        }
                                    }
                                    break;

                                case "extensions":
                                    if (appliedSettings.Contains(node.Name.ToLowerEnglish()))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name.ToLowerEnglish());
                                    for (int ı = 0; ı < node.ChildNodes.Count; ı++)
                                    {
                                        XmlNode subnode = node.ChildNodes[ı];
                                        if (subnode.Name.ToLowerEnglish() == "ext")
                                        {
                                            if (subnode.Attributes["Name"] != null)
                                            {
                                                string subnodeName = subnode.Attributes["Name"].Value.InnerXmlToString();
                                                if (Profile.Manager.Main.Extensions.ExtExists(subnodeName))
                                                {
                                                    Profile.Manager.Main.Extensions.Enable(subnodeName);
                                                    if (subnode.Attributes["allowInIncognito"] != null)
                                                    {
                                                        Profile.Manager.Main.Extensions.GetExtByCN(subnodeName).AllowInIncognito = subnode.Attributes["allowInIncognito"].Value == "true";
                                                    }
                                                    if (subnode.Attributes["isPinned"] != null)
                                                    {
                                                        Profile.Manager.Main.Extensions.GetExtByCN(subnodeName).isPinned = subnode.Attributes["isPinned"].Value == "true";
                                                    }
                                                }
                                                else
                                                {
                                                    Output.WriteLine("[Extensions] Threw away \"" + subnode.OuterXml + "\". Extension does not exists.", LogLevel.Warning);
                                                }
                                            }
                                            else
                                            {
                                                if (!subnode.OuterXml.StartsWith("<!--"))
                                                {
                                                    Output.WriteLine("[Extensions] Threw away \"" + subnode.OuterXml + "\". unsupported.", LogLevel.Warning);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (!subnode.OuterXml.StartsWith("<!--"))
                                            {
                                                Output.WriteLine("[Extensions] Threw away \"" + subnode.OuterXml + "\". unsupported.", LogLevel.Warning);
                                            }
                                        }
                                    }
                                    break;

                                case "themes":
                                    if (appliedSettings.Contains(node.Name.ToLowerEnglish()))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name.ToLowerEnglish());
                                    for (int ı = 0; ı < node.ChildNodes.Count; ı++)
                                    {
                                        XmlNode subnode = node.ChildNodes[ı];
                                        if (subnode.Name.ToLowerEnglish() == "theme")
                                        {
                                            if (subnode.Attributes["Name"] != null)
                                            {
                                                if (Profile.Manager.Main.ThemeMan.ThemeExists(subnode.Attributes["Name"].Value))
                                                {
                                                    Profile.Manager.Main.ThemeMan.Enable(subnode.Attributes["Name"].Value);
                                                }
                                                else
                                                {
                                                    Output.WriteLine("[Extensions] Threw away \"" + subnode.OuterXml + "\". Theme does not exists.", LogLevel.Warning);
                                                }
                                            }
                                            else
                                            {
                                                if (!subnode.OuterXml.StartsWith("<!--"))
                                                {
                                                    Output.WriteLine("[Extensions] Threw away \"" + subnode.OuterXml + "\". unsupported.", LogLevel.Warning);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (!subnode.OuterXml.StartsWith("<!--"))
                                            {
                                                Output.WriteLine("[Extensions] Threw away \"" + subnode.OuterXml + "\". unsupported.", LogLevel.Warning);
                                            }
                                        }
                                    }
                                    if (node.Attributes["Selected"] != null)
                                    {
                                        CurrentTheme = Profile.Manager.Main.ThemeMan.GetThemeByCN(node.Attributes["Selected"].Value.InnerXmlToString());
                                    }
                                    else
                                    {
                                        CurrentTheme = Profile.Manager.Main.ThemeMan.GetThemeByCN(DefaultThemes.YorotLight.CodeName);
                                    }
                                    break;

                                case "langs":
                                    if (appliedSettings.Contains(node.Name.ToLowerEnglish()))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name.ToLowerEnglish());
                                    for (int ı = 0; ı < node.ChildNodes.Count; ı++)
                                    {
                                        XmlNode subnode = node.ChildNodes[ı];
                                        if (subnode.Name.ToLowerEnglish() == "lang")
                                        {
                                            if (subnode.Attributes["Name"] != null)
                                            {
                                                if (Profile.Manager.Main.LangMan.LangExists(subnode.Attributes["Name"].Value))
                                                {
                                                    Profile.Manager.Main.LangMan.Enable(subnode.Attributes["Name"].Value);
                                                }
                                                else
                                                {
                                                    Output.WriteLine("[Extensions] Threw away \"" + subnode.OuterXml + "\". Language does not exists.", LogLevel.Warning);
                                                }
                                            }
                                            else
                                            {
                                                if (!subnode.OuterXml.StartsWith("<!--"))
                                                {
                                                    Output.WriteLine("[Extensions] Threw away \"" + subnode.OuterXml + "\". unsupported.", LogLevel.Warning);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (!subnode.OuterXml.StartsWith("<!--"))
                                            {
                                                Output.WriteLine("[Extensions] Threw away \"" + subnode.OuterXml + "\". unsupported.", LogLevel.Warning);
                                            }
                                        }
                                    }
                                    if (node.Attributes["Selected"] != null)
                                    {
                                        CurrentLanguage = Profile.Manager.Main.LangMan.GetLangByCN(node.Attributes["Selected"].Value.InnerXmlToString());
                                    }
                                    else
                                    {
                                        CurrentLanguage = Profile.Manager.Main.LangMan.GetLangByCN("com.haltroy.english-us");
                                    }
                                    break;

                                case "apps":
                                    if (appliedSettings.Contains(node.Name.ToLowerEnglish()))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name.ToLowerEnglish());
                                    for (int ı = 0; ı < node.ChildNodes.Count; ı++)
                                    {
                                        XmlNode subnode = node.ChildNodes[ı];
                                        if (subnode.Name.ToLowerEnglish() == "app")
                                        {
                                            if (subnode.Attributes["Name"] != null)
                                            {
                                                if (Profile.Manager.Main.AppMan.AppExists(subnode.Attributes["Name"].Value))
                                                {
                                                    Profile.Manager.Main.AppMan.Enable(subnode.Attributes["Name"].Value);
                                                    if (subnode.Attributes["isPinned"] != null)
                                                    {
                                                        Profile.Manager.Main.AppMan.SetPinStatus(subnode.Attributes["Name"].Value, subnode.Attributes["isPinned"].Value == "true");
                                                    }
                                                }
                                                else
                                                {
                                                    Output.WriteLine("[Apps] Threw away \"" + subnode.OuterXml + "\". App does not exists.", LogLevel.Warning);
                                                }
                                            }
                                            else
                                            {
                                                if (!subnode.OuterXml.StartsWith("<!--"))
                                                {
                                                    Output.WriteLine("[Apps] Threw away \"" + subnode.OuterXml + "\". unsupported.", LogLevel.Warning);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (!subnode.OuterXml.StartsWith("<!--"))
                                            {
                                                Output.WriteLine("[Apps] Threw away \"" + subnode.OuterXml + "\". unsupported.", LogLevel.Warning);
                                            }
                                        }
                                    }
                                    break;

                                case "restoreoldsessions":
                                    if (appliedSettings.Contains(node.Name.ToLowerEnglish()))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name.ToLowerEnglish());
                                    RestoreOldSessions = node.InnerXml.InnerXmlToString() == "true";
                                    break;

                                case "rememberlastproxy":
                                    if (appliedSettings.Contains(node.Name.ToLowerEnglish()))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name.ToLowerEnglish());
                                    RememberLastProxy = node.InnerXml.InnerXmlToString() == "true";
                                    break;

                                case "donottrack":
                                    if (appliedSettings.Contains(node.Name.ToLowerEnglish()))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name.ToLowerEnglish());
                                    DoNotTrack = node.InnerXml.InnerXmlToString() == "true";
                                    break;

                                case "showfavorites":
                                    if (appliedSettings.Contains(node.Name.ToLowerEnglish()))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name.ToLowerEnglish());
                                    FavManager.ShowFavorites = node.InnerXml.InnerXmlToString() == "true";
                                    break;

                                case "startwithfullscreen":
                                    if (appliedSettings.Contains(node.Name.ToLowerEnglish()))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name.ToLowerEnglish());
                                    StartWithFullScreen = node.InnerXml.InnerXmlToString() == "true";
                                    break;

                                case "openfilesafterdownload":
                                    if (appliedSettings.Contains(node.Name.ToLowerEnglish()))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name.ToLowerEnglish());
                                    DownloadManager.OpenFilesAfterDownload = node.InnerXml.InnerXmlToString() == "true";
                                    break;

                                case "autodownload":
                                    if (appliedSettings.Contains(node.Name.ToLowerEnglish()))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name.ToLowerEnglish());
                                    DownloadManager.AutoDownload = node.InnerXml.InnerXmlToString() == "true";
                                    break;

                                case "downloadfolder":
                                    if (appliedSettings.Contains(node.Name.ToLowerEnglish()))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name.ToLowerEnglish());
                                    DownloadManager.DownloadFolder = node.InnerXml.InnerXmlToString();
                                    break;

                                case "alwayscheckdefaultbrowser":
                                    if (appliedSettings.Contains(node.Name.ToLowerEnglish()))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name.ToLowerEnglish());
                                    AlwaysCheckDefaultBrowser = node.InnerXml.InnerXmlToString() == "true";
                                    break;

                                case "startonboot":
                                    if (appliedSettings.Contains(node.Name.ToLowerEnglish()))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name.ToLowerEnglish());
                                    StartOnBoot = node.InnerXml.InnerXmlToString() == "true";
                                    break;

                                case "startinsystemtray":
                                    if (appliedSettings.Contains(node.Name.ToLowerEnglish()))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name.ToLowerEnglish());
                                    StartInSystemTray = node.InnerXml.InnerXmlToString() == "true";
                                    break;

                                case "notifplaysound":
                                    if (appliedSettings.Contains(node.Name.ToLowerEnglish()))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name.ToLowerEnglish());
                                    NotifPlaySound = node.InnerXml.InnerXmlToString() == "true";
                                    break;

                                case "datetime":
                                    if (appliedSettings.Contains(node.Name.ToLowerEnglish()))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name.ToLowerEnglish());
                                    switch (node.InnerXml.InnerXmlToString())
                                    {
                                        default:
                                        case "DMY":
                                            DateFormat = YorotDateAndTime.DMY;
                                            break;

                                        case "YMD":
                                            DateFormat = YorotDateAndTime.YMD;
                                            break;

                                        case "MDY":
                                            DateFormat = YorotDateAndTime.MDY;
                                            break;
                                    }
                                    break;

                                case "notifusedefault":
                                    if (appliedSettings.Contains(node.Name.ToLowerEnglish()))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name.ToLowerEnglish());
                                    NotifUseDefault = node.InnerXml.InnerXmlToString() == "true";
                                    break;

                                case "notifsoundloc":
                                    if (appliedSettings.Contains(node.Name.ToLowerEnglish()))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name.ToLowerEnglish());
                                    NotifSoundLoc = node.InnerXml.InnerXmlToString();
                                    break;

                                case "notifsilent":
                                    if (appliedSettings.Contains(node.Name.ToLowerEnglish()))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name.ToLowerEnglish());
                                    NotifSilent = node.InnerXml.InnerXmlToString() == "true";
                                    break;

                                default:
                                    if (!node.OuterXml.StartsWith("<!--"))
                                    {
                                        Output.WriteLine("[Settings] Threw away \"" + node.OuterXml + "\", unsupported.", LogLevel.Warning);
                                    }
                                    break;
                            }
                        }
                    }
                    catch (XmlException)
                    {
                        Output.WriteLine("[Settings] Loaded default settings because file is in unsupported.", LogLevel.Warning);
                    }
                    catch (Exception ex)
                    {
                        Output.WriteLine("[Settings] Loaded default settings because of this exception:" + Environment.NewLine + ex.ToString(), LogLevel.Warning);
                    }
                }
            }
        }

        /// <summary>
        /// Current loaded language by user.
        /// </summary>
        public YorotLanguage CurrentLanguage { get; set; }

        /// <summary>
        /// Current theme loaded by user.
        /// </summary>
        public YorotTheme CurrentTheme { get; set; }

        /// <summary>
        /// Retrieves configuration in XML format.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public string ToXml()
        {
            List<YorotLanguage> langList = Profile.Manager.Main.LangMan.Languages.FindAll(it => it.Enabled);
            List<YorotExtension> extList = Profile.Manager.Main.Extensions.Extensions.FindAll(it => it.Enabled);
            List<YorotTheme> themeList = Profile.Manager.Main.ThemeMan.Themes.FindAll(it => it.Enabled);
            List<YorotApp> appList = Profile.Manager.Main.AppMan.Apps.FindAll(it => it.isEnabled);
            string x = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" + Environment.NewLine +
                "<root>" + Environment.NewLine +
                "<!-- Yorot User File" + Environment.NewLine + Environment.NewLine +
                "This file is used by Yorot to get user information." + Environment.NewLine +
                "Editing this file might cause problems within Yorot and" + Environment.NewLine +
                "other apps and extensions." + Environment.NewLine +
                "-->" + Environment.NewLine;
            x += "<HomePage>" + HomePage.ToXML() + "</HomePage>" + Environment.NewLine;
            x += "<SearchEngines Selected=\"" + SearchEngines.IndexOf(SearchEngine) + "\"" + (SearchEngines.Count > 0 ? "" : "/") + ">" + Environment.NewLine;
            if (SearchEngines.Count > 0)
            {
                for (int i = 0; i < SearchEngines.Count; i++)
                {
                    if (!SearchEngines[i].comesWithYorot)
                    {
                        x += "<Engine Name=\"" + SearchEngines[i].Name.ToXML() + "\" Url=\"" + SearchEngines[i].Url.ToXML() + "\" />" + Environment.NewLine;
                    }
                }
                x += "</SearchEngines>" + Environment.NewLine;
            }
            x += "<RestoreOldSessions>" + (RestoreOldSessions ? "true" : "false") + "</RestoreOldSessions>" + Environment.NewLine;
            x += "<RememberLastProxy>" + (RememberLastProxy ? "true" : "false") + "</RememberLastProxy>" + Environment.NewLine;
            x += "<Langs" + (CurrentLanguage != null ? " Selected=\"" + CurrentLanguage.CodeName + "\" " : "") + ">" + Environment.NewLine;
            foreach (YorotLanguage lang in Profile.Manager.Main.LangMan.Languages)
            {
                if (lang.Enabled)
                {
                    x += "<Lang Name=\"" + lang.CodeName + "\" />" + Environment.NewLine;
                }
            }
            x += "</Langs>" + Environment.NewLine + "<Themes Selected=\"" + CurrentTheme.CodeName + "\" >" + Environment.NewLine;
            foreach (YorotTheme theme in Profile.Manager.Main.ThemeMan.Themes)
            {
                if (theme.Enabled)
                {
                    x += "<Theme Name=\"" + theme.CodeName + "\" />" + Environment.NewLine;
                }
            }
            x += "</Themes>" + Environment.NewLine;
            List<YorotExtension> enabledExt = Profile.Manager.Main.Extensions.Extensions.FindAll(it => it.Enabled);
            if (enabledExt.Count > 0)
            {
                x += "<Extensions>" + Environment.NewLine;
                foreach (YorotExtension ext in enabledExt)
                {
                    x += "<Ext Name=\"" + ext.CodeName + "\" " + (ext.AllowInIncognito ? "allowInIncognito=\"true\" " : "") + (ext.isPinned ? "isPinned=\"true\" " : "") + "/>" + Environment.NewLine;
                }
                x += "</Extensions>" + Environment.NewLine;
            }
            List<YorotWebEngine> enabledWE = Profile.Manager.Main.WebEngineMan.Engines.FindAll(it => it.isEnabled);
            if (enabledWE.Count > 0)
            {
                x += "<WebEngines>" + Environment.NewLine;
                foreach (YorotWebEngine engine in enabledWE)
                {
                    x += "<Engine Name=\"" + engine.CodeName + "\" />" + Environment.NewLine;
                }
                x += "</WebEngines>" + Environment.NewLine;
            }
            List<YorotApp> enabledApps = Profile.Manager.Main.AppMan.Apps.FindAll(it => it.isEnabled);
            if (enabledApps.Count > 0)
            {
                x += "<Apps>" + Environment.NewLine;
                foreach (YorotApp app in enabledApps)
                {
                    x += "<App Name=\"" + app.AppCodeName + "\" " + (app.isPinned ? "isPinned=\"true\" " : "") + "/>" + Environment.NewLine;
                }
                x += "</Apps>" + Environment.NewLine;
            }
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
            x += "<DateTime>" + DateFormat.Name + "</DateTime>" + Environment.NewLine;
            return (x + Environment.NewLine + "</root>").BeautifyXML();
        }

        /// <summary>
        /// Loads default configurations.
        /// </summary>
        /// <param name="profileName">This arguyment is for detecting both incognito and root user.</param>
        public void LoadDefaults(bool root)
        {
            DownloadManager = new DownloadManager(Profile.UserDownloads, Profile.Manager.Main);
            HistoryManager = new HistoryManager(Profile.UserHistory, Profile.Manager.Main);
            FavManager = new FavMan(Profile.UserFavorites, Profile.Manager.Main);
            HomePage = "yorot://newtab";
            // BEGIN: Search Engines
            SearchEngines.Clear();
            SearchEngine = new YorotSearchEngine("Google", "https://www.google.com/search?q=") { comesWithYorot = true };
            SearchEngines.Add(SearchEngine);
            SearchEngines.Add(new YorotSearchEngine("Yandex", "https://yandex.com/search/?lr=103873&text=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("Yandex", "https://yandex.com/search/?lr=103873&text=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("Bing", "https://www.bing.com/search?q=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("Yaani", "https://www.yaani.com/#q=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("DuckDuckGo", "https://duckduckgo.com/?q=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("Baidu", "https://www.baidu.com/s?&wd=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("WolframAlpha", "https://www.wolframalpha.com/input/?i=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("AOL", "https://search.aol.com/aol/search?&q=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("Yahoo", "https://search.yahoo.com/search?p=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("Ask", "https://www.ask.com/web?q=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("Archive.org", "https://web.archive.org/web/*/") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("Wikipedia", "https://en.wikipedia.org/w/index.php?search=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("Mojeek", "https://www.mojeek.com/search?q=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("Qwant", "https://www.qwant.com/?q=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("Naver", "https://search.naver.com/search.naver?query=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("Sogou", "https://www.sogou.com/web?query=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("Gigablast", "https://www.gigablast.com/search?q=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("Lycos", "https://search17.lycos.com/web/?q=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("Teoma", "https://www.teoma.com/web?q=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("Ciao!", "https://www.ciao.co.uk/search?query=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("Ecosia", "https://www.ecosia.org/search?q=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("Webcrawler", "https://www.webcrawler.com/serp?q=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("Yahoo Japan", "https://search.yahoo.co.jp/search?p=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("business.com", "https://www.business.com/search/?q=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("Shodan", "https://www.shodan.io/search?query=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("Startpage", "https://www.startpage.com/do/dsearch?query=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("Wikipedia", "https://en.wikipedia.org/w/index.php?search=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("Kiddle", "https://www.kiddle.co/s.php?q=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("KidzSearch", "https://search.kidzsearch.com/kzsearch.php?q=") { comesWithYorot = true });
            SearchEngines.Add(new YorotSearchEngine("YouTube", "https://www.youtube.com/results?search_query=") { comesWithYorot = true });
            // END: Search Engines
            CurrentLanguage = Profile.Manager.Main.LangMan.Languages[0];
            CurrentTheme = Profile.Manager.Main.ThemeMan.Themes[0];
            RestoreOldSessions = false;
            RememberLastProxy = false;
            DoNotTrack = false;
            FavManager.ShowFavorites = !root;
            StartWithFullScreen = false;
            DownloadManager.OpenFilesAfterDownload = false;
            DownloadManager.AutoDownload = !root;
            DownloadManager.DownloadFolder = root ? Profile.Manager.Main.AppPath : Profile.Path + @"Downloads\";
            AlwaysCheckDefaultBrowser = true;
            StartOnBoot = false;
            StartInSystemTray = false;
            NotifPlaySound = true;
            NotifSilent = false;
            NotifUseDefault = true;
            NotifSoundLoc = @"RES\n.ogg";
        }

        /// <summary>
        /// Saves configuration to drive.
        /// </summary>
        public void Save()
        {
            if (Profile.Name != "root")
            {
                HistoryManager.Save();
                FavManager.Save();
                DownloadManager.Save();
                HTAlt.Tools.WriteFile(Profile.Manager.Main.Profiles.Current.UserSettings, ToXml(), Encoding.Unicode);
            }
        }

        /// <summary>
        /// The profile that this settings are associated with.
        /// </summary>
        public YorotProfile Profile { get; set; }

        /// <summary>
        /// user downloads manager
        /// </summary>
        public DownloadManager DownloadManager { get; set; }

        /// <summary>
        /// User history manager
        /// </summary>
        public HistoryManager HistoryManager { get; set; }

        /// <summary>
        /// User favorites manager
        /// </summary>
        public FavMan FavManager { get; set; }

        /// <summary>
        /// Determines if a search engine exists.
        /// </summary>
        /// <param name="name">Name of the engine</param>
        /// <param name="url">URI of the engine</param>
        /// <returns><see cref="bool"/></returns>
        public bool SearchEngineExists(string name, string url)
        {
            return SearchEngines.FindAll(i => i.Name == name && i.Url == url).Count > 0;
        }

        /// <summary>
        /// URI that loads when user clicks on the home button.
        /// </summary>
        public string HomePage { get; set; } = "";

        /// <summary>
        /// Current search engine selected by user.
        /// </summary>
        public YorotSearchEngine SearchEngine { get; set; } = null;

        /// <summary>
        /// A list of search engines.
        /// </summary>
        public List<YorotSearchEngine> SearchEngines { get; set; } = new List<YorotSearchEngine>();

        /// <summary>
        /// Determines the date and time format.
        /// </summary>
        public YorotDateAndTime DateFormat { get; set; } = YorotDateAndTime.DMY;

        /// <summary>
        /// Determines if old sessions should resotre on startup.
        /// </summary>
        public bool RestoreOldSessions { get; set; } = false;

        /// <summary>
        /// Determines to remeber last proxy on either Yorot or extension restart.
        /// </summary>
        public bool RememberLastProxy { get; set; } = false;

        /// <summary>
        /// Determines to sending DoNotTrack information to websites.
        /// </summary>
        public bool DoNotTrack { get; set; } = true;

        /// <summary>
        /// Determinbes if the app drawer should start full screen or not.
        /// </summary>
        public bool StartWithFullScreen { get; set; } = false;

        /// <summary>
        /// Determines to check if Yorot is the default on startup.
        /// </summary>
        public bool AlwaysCheckDefaultBrowser { get; set; } = true;

        /// <summary>
        /// Determines to start Yorot on operating system boot.
        /// </summary>
        public bool StartOnBoot { get; set; } = false;

        /// <summary>
        /// Determines to start and quickly hide Yorot to system tray on bootup start.
        /// </summary>
        public bool StartInSystemTray { get; set; } = true;

        /// <summary>
        /// Determines if notifications should play sound.
        /// </summary>
        public bool NotifPlaySound { get; set; } = true;

        /// <summary>
        /// Determines if notification sound should be the default one.
        /// </summary>
        public bool NotifUseDefault { get; set; } = true;

        /// <summary>
        /// Determines the location of notificatio sound on drive.
        /// </summary>
        public string NotifSoundLoc { get; set; } = "";

        /// <summary>
        /// Determines if the notification should play a sound.
        /// </summary>
        public bool NotifSilent { get; set; } = false;
    }

    /// <summary>
    /// Search engine class
    /// </summary>
    public class YorotSearchEngine
    {
        /// <summary>
        /// Creates a new search engine.
        /// </summary>
        /// <param name="name">Name of the engine.</param>
        /// <param name="url">URI of the engine.</param>
        public YorotSearchEngine(string name, string url)
        {
            Name = name;
            Url = url;
        }

        /// <summary>
        /// Name of the search engine.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// URI of the search engine.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Determines if this engine comes with Yorot.
        /// </summary>
        public bool comesWithYorot { get; set; } = false;

        /// <summary>
        /// Searches text with this engine.
        /// </summary>
        /// <param name="x"><see cref="string"/></param>
        /// <returns><see cref="string"/></returns>
        public string Search(string x)
        {
            return Url.Contains("%s%") ? Url.Replace("%s%", x) : Url + x;
        }
    }
}