using IWshRuntimeLibrary;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace YorotInstaller
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            HTAltTools.CreateLangs();
            string prgFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Haltroy\\";
            string appPath = prgFiles + "YorotInstaller.exe";
            if (!args.Contains("--skip-folder-check"))
            {
                if (Application.ExecutablePath != appPath)
                {
                    if (!Directory.Exists(prgFiles))
                    {
                        Directory.CreateDirectory(prgFiles);
                    }
                    if (System.IO.File.Exists(appPath) && Application.ExecutablePath != appPath)
                    {
                        System.IO.File.Delete(appPath);
                    }
                    System.IO.File.Copy(Application.ExecutablePath, appPath);
                    Process.Start(new ProcessStartInfo(appPath) { UseShellExecute = true, Verb = "runas", Arguments = string.Join(" ", args) });
                    return;
                }
            }
            HTAltTools.appShortcut(appPath, Environment.GetFolderPath(Environment.SpecialFolder.Programs) + "\\Yorot Installer");
            HTAltTools.appShortcut(appPath, Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms) + "\\Yorot Installer");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
            Settings settings = new Settings();
            Application.Run(new frmMain(settings));
            settings.Save();
        }

    }
    /// <summary>
    /// HTAlt.Standart.Tools Jr. Plus
    /// </summary>
    public static class HTAltTools
    {
        public static void appShortcut(string app, string shortcutPath, string args = "")
        {
            if (!shortcutPath.ToLower().EndsWith(".lnk")) { shortcutPath += ".lnk"; }
            if (!System.IO.File.Exists(shortcutPath))
            {
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
                shortcut.TargetPath = app;
                shortcut.Arguments = args;
                shortcut.Save();
            }
        }
        /// <summary>
        /// Creates and writes a file without locking it.
        /// </summary>
        /// <param name="fileLocation">Location of the file.</param>
        /// <param name="input">Text to write on.</param>
        /// <param name="encode">Rules to follow while writing.</param>
        /// <returns><c>true</c> if successfully writes to file, otherwise throws an exception.</returns>
        public static bool WriteFile(string fileLocation, string input, Encoding encode)
        {
            if (!Directory.Exists(new FileInfo(fileLocation).DirectoryName)) { Directory.CreateDirectory(new FileInfo(fileLocation).DirectoryName); }
            if (System.IO.File.Exists(fileLocation))
            {
                System.IO.File.Delete(fileLocation);
            }
            System.IO.File.Create(fileLocation).Dispose();
            FileStream writer = new FileStream(fileLocation, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
            writer.Write(encode.GetBytes(input), 0, encode.GetBytes(input).Length);
            writer.Close();
            return true;
        }

        /// <summary>
        /// Creates and writes a file without locking it.
        /// </summary>
        /// <param name="fileLocation">Location of the file.</param>
        /// <param name="input">Text to write on.</param>
        /// <param name="encode">Rules to follow while writing.</param>
        /// <returns><c>true</c> if successfully writes to file, otherwise throws an exception.</returns>
        public static bool WriteFile(string fileLocation, byte[] input)
        {
            if (!Directory.Exists(new FileInfo(fileLocation).DirectoryName)) { Directory.CreateDirectory(new FileInfo(fileLocation).DirectoryName); }
            if (System.IO.File.Exists(fileLocation))
            {
                System.IO.File.Delete(fileLocation);
            }
            System.IO.File.Create(fileLocation).Dispose();
            FileStream writer = new FileStream(fileLocation, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
            writer.Write(input, 0, input.Length);
            writer.Close();
            return true;
        }

        /// <summary>
        /// Reads a file without locking it.
        /// </summary>
        /// <param name="fileLocation">Location of the file.</param>
        /// <param name="encode">Rules for reading the file.</param>
        /// <returns>Text inside the file.</returns>
        public static string ReadFile(string fileLocation, Encoding encode)
        {
            FileStream fs = new FileStream(fileLocation, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader sr = new StreamReader(fs, encode);
            string result = sr.ReadToEnd();
            sr.Close();
            return result;
        }
        public static void CreateLangs()
        {
            WriteFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Yorot\\Installer\\English.language", Properties.Resources.English);
            WriteFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Yorot\\Installer\\Turkish.language", Properties.Resources.Turkish);
        }
    }

    public class Settings
    {
        public static string FileLocation => WorkFolder + "installer.settings";
        public static string WorkFolder => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Yorot\\Installer\\";

        public Settings()
        {
            if (System.IO.File.Exists(FileLocation))
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(HTAltTools.ReadFile(FileLocation, Encoding.Unicode));
                    XmlNode firstNode = doc.FirstChild.Name == "InstallerSettings" ? doc.FirstChild : doc.FirstChild.NextSibling;
                    int loadedSettings = 0;
                    foreach (XmlNode node in firstNode.ChildNodes)
                    {
                        if (node.Name == "LangFile")
                        {
                            loadedSettings++;
                            LanguageFile = string.IsNullOrWhiteSpace(node.InnerXml) ? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Yorot\\Installer\\English.language" : node.InnerXml.Replace("[APPDATA]", WorkFolder);
                        }
                        else if (node.Name == "DarkMode")
                        {
                            loadedSettings++;
                            isDarkMode = string.IsNullOrWhiteSpace(node.InnerXml) ? false : (node.InnerXml == "1");
                        }
                    }
                    LoadedDefaults = false;
                    Console.WriteLine(" [Settings] Successfully loaded " + loadedSettings + " setting(s).");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" [Settings] Loaded Defaults: Error: " + ex.ToString());
                    isDarkMode = false;
                    LanguageFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Yorot\\Installer\\English.language";
                    LoadedDefaults = true;
                }
            }
            else
            {
                Console.WriteLine(" [Settings] Loaded Defaults: File not found (\"" + FileLocation + "\").");
                isDarkMode = false;
                LanguageFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Yorot\\Installer\\English.language";
                LoadedDefaults = true;
            }
            LoadLang(LanguageFile);
        }

        public string XmlOut()
        {
            return "<?xml version=\"1.0\" encoding=\"utf-16\"?>" + Environment.NewLine
                + "<InstallerSettings>" + Environment.NewLine
                + "  <LangFile>" + (string.IsNullOrWhiteSpace(LanguageFile) ? "" : LanguageFile.Replace(WorkFolder, "[APPDATA]")) + "</LangFile>" + Environment.NewLine
                + "  <DarkMode>" + (isDarkMode ? "1" : "0") + "</DarkMode>" + Environment.NewLine
                + "</InstallerSettings>";
        }

        public void LoadLang(string LangFile)
        {
            if (string.IsNullOrWhiteSpace(LangFile)) { LangFile = LanguageFile; }
            if (!System.IO.File.Exists(LangFile)) { LangFile = LanguageFile; }
            Translations.Clear();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(HTAltTools.ReadFile(LangFile, Encoding.Unicode));
            XmlNode firstNode = doc.FirstChild.Name != "Language" ? doc.FirstChild.NextSibling : doc.FirstChild;
            for (int i = 0; i < firstNode.ChildNodes.Count; i++)
            {
                XmlNode node = firstNode.ChildNodes[i];
                if (node.Name == "Translate")
                {
                    if (node.Attributes["ID"] != null && node.Attributes["Text"] != null)
                    {
                        Translations.Add(new Translation(node.Attributes["ID"].Value, node.Attributes["Text"].Value));
                    }
                }
            }
        }

        public Color BackColor => isDarkMode ? Color.FromArgb(255, 0, 0, 0) : Color.FromArgb(255, 255, 255, 255);

        public Color BackColor1 => isDarkMode ? Color.FromArgb(255, 20, 20, 20) : Color.FromArgb(255, 235, 235, 235);

        public Color BackColor2 => isDarkMode ? Color.FromArgb(255, 40, 40, 40) : Color.FromArgb(255, 215, 215, 215);

        public Color BackColor3 => isDarkMode ? Color.FromArgb(255, 60, 60, 60) : Color.FromArgb(255, 195, 195, 195);

        public Color BackColor4 => isDarkMode ? Color.FromArgb(255, 80, 80, 80) : Color.FromArgb(255, 175, 175, 175);

        public Color BackColor5 => isDarkMode ? Color.FromArgb(255, 100, 100, 100) : Color.FromArgb(255, 155, 155, 155);

        public Color BackColor6 => isDarkMode ? Color.FromArgb(255, 120, 120, 120) : Color.FromArgb(255, 135, 135, 135);

        public Color MidColor => Color.FromArgb(255, 128, 128, 128);

        public Color ForeColor => isDarkMode ? Color.FromArgb(255, 255, 255, 255) : Color.FromArgb(255, 0, 0, 0);

        public void Save()
        {
            HTAltTools.WriteFile(FileLocation, XmlOut(), Encoding.Unicode);
        }
        public string LanguageFile { get; set; }
        public static string InstallPath => Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Haltroy\\Yorot\\";
        public bool isDarkMode { get; set; }
        public bool LoadedDefaults = true;
        public List<Translation> Translations { get; set; } = new List<Translation>();
        public string GetItemText(string ID)
        {
            List<Translation> foundItems = new List<Translation>();
            for (int i = 0; i < Translations.Count; i++)
            {
                if (Translations[i].ID == ID) { foundItems.Add(Translations[i].CarbonCopy()); }
            }
            if (foundItems.Count > 0) { return foundItems[new Random().Next(0, foundItems.Count - 1)].Text.Replace("[NEWLINE]", Environment.NewLine); } else { return "[MI]" + ID; }
        }
    }

    public class Translation
    {
        public Translation(string id, string text)
        {
            ID = id;
            Text = text;
        }
        public Translation CarbonCopy()
        {
            return new Translation(ID, Text);
        }
        public void CarbonCopyTo(Translation copyTo)
        {
            copyTo.ID = ID;
            copyTo.Text = Text;
        }

        public override bool Equals(object obj)
        {
            return obj is Translation translation &&
                   ID == translation.ID;
        }

        public override int GetHashCode()
        {
            int hashCode = -2039955522;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ID);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Text);
            return hashCode;
        }

        public string ID { get; set; }
        public string Text { get; set; }
    }

    public class VersionManager
    {
        public string LatesVersion { get; set; }
        public int LatestVersionNumber { get; set; }
        public int LatestUpdateMinVer { get; set; }
        public string LatesPreOut { get; set; }
        public int PreOutVerNumber { get; set; }
        public int PreOutMinVer { get; set; }
        public static int InstallerVer => 1;
        public int LatestInstallerVer { get; set; }
        public string InstallerLoc { get; set; }

        public YorotVersion GetVersionFromVersionName(string name)
        {
            List<YorotVersion> foundItems = new List<YorotVersion>();
            for (int i = 0; i < Versions.Count; i++)
            {
                YorotVersion ver = Versions[i];
                if (ver.VersionText == name)
                {
                    foundItems.Add(ver);
                }
            }
            if (foundItems.Count > 0) { return foundItems[0]; } else { return null; }
        }

        public YorotVersion GetVersionFromVersionNo(int verno)
        {
            List<YorotVersion> foundItems = new List<YorotVersion>();
            for (int i = 0; i < Versions.Count; i++)
            {
                YorotVersion ver = Versions[i];
                if (ver.VersionNo == verno)
                {
                    foundItems.Add(ver);
                }
            }
            if (foundItems.Count > 0) { return foundItems[0]; } else { return null; }
        }

        public List<YorotVersion> Versions { get; set; } = new List<YorotVersion>();

        public List<YorotVersion> PreOutVersions => Versions.FindAll(i => i.isPreOut);
        public enum UpdateType
        {
            /// <summary>
            /// Application is up to date.
            /// </summary>
            UpToDate,
            /// <summary>
            /// Application requires minimal update.
            /// </summary>
            Upgrade,
            /// <summary>
            /// Application requires complete reinstallation
            /// </summary>
            FullUpgrade,
            /// <summary>
            /// Application requşres update in PreOut mode.
            /// </summary>
            PreOutUpdate,
            /// <summary>
            /// A full release is available even for latest PreOut version.
            /// </summary>
            PreOutFull,
        }


    }
    public class YorotVersion
    {
        public YorotVersion(string _text, int _no, string _zipPath, string _flags, string scheme)
        {
            if (!string.IsNullOrWhiteSpace(_flags))
            {
                Flags = _flags.Split(';');
            }
            VersionText = _text;
            VersionNo = _no;
            ZipPath = _zipPath;
            switch (scheme)
            {
                default:
                case "Standart":
                    Reg = RegType.Standart;
                    break;
                case "StandartWithProtocol":
                    Reg = RegType.StandartWithProtocol;
                    break;
                case "StandartWithCommandProtocol":
                    Reg = RegType.StandartWithCommandProtocol;
                    break;
            }
        }
        public YorotVersion(string _text, int _no)
        {
            VersionText = _text;
            VersionNo = _no;
        }
        public YorotVersion(string _text, int _no, string _flags)
        {
            if (!string.IsNullOrWhiteSpace(_flags))
            {
                Flags = _flags.Split(';');
            }
            VersionText = _text;
            VersionNo = _no;
        }
        public bool isOnlyx86 => Flags.Contains("onlyx86");
        public bool isOnlyx64 => Flags.Contains("onlyx64");
        public bool isPreOut => Flags.Contains("preout");
        public bool isMissing => Flags.Contains("missing");
        /// <summary>
        /// .Net Framework 4.8
        /// </summary>
        public bool RequiresNet48 => Flags.Contains("reqnet48");
        /// <summary>
        /// .Net Framework 4.6.1
        /// </summary>
        public bool RequiresNet461 => Flags.Contains("reqnet461");
        /// <summary>
        /// .Net Framework 4.5.2
        /// </summary>
        public bool RequiresNet452 => Flags.Contains("reqnet452");
        /// <summary>
        /// Visual C++ 2015
        /// </summary>
        public bool RequiresVisualC2015 => Flags.Contains("reqvc2015");
        public string[] Flags { get; set; } = new string[] { };
        public string VersionText { get; set; } = "";
        public int VersionNo { get; set; } = 0;
        public string ZipPath { get; set; } = "";
        public RegType Reg { get; set; } = RegType.Standart;

        public override string ToString()
        {
            return VersionText + " (" + VersionNo + ")";
        }
    }
    public static class PreResqs
    {
        public static bool is64BitMachine => Environment.Is64BitOperatingSystem || Environment.Is64BitProcess;
        public static bool isInstalled(PreResq resq)
        {
            string key = Registry.LocalMachine.OpenSubKey(resq.RegistryKey).GetValue(resq.RegistryValue).ToString();
            switch (resq.CheckType)
            {
                case RegistryCheckType.Equals:
                    return key == resq.ValueDeğeri;
                case RegistryCheckType.Different:
                    return key != resq.ValueDeğeri;
                case RegistryCheckType.DoesNotContain:
                    return !key.Contains(resq.ValueDeğeri);
                case RegistryCheckType.Contains:
                    return key.Contains(resq.ValueDeğeri);
                case RegistryCheckType.GreaterThan:
                    return Convert.ToInt32(key) > Convert.ToInt32(resq.ValueDeğeri);
                case RegistryCheckType.GreaterOrEqual:
                    return Convert.ToInt32(key) >= Convert.ToInt32(resq.ValueDeğeri);
                case RegistryCheckType.LessThan:
                    return Convert.ToInt32(key) < Convert.ToInt32(resq.ValueDeğeri);
                case RegistryCheckType.LessOrEqual:
                    return Convert.ToInt32(key) <= Convert.ToInt32(resq.ValueDeğeri);
                default:
                    return key == resq.ValueDeğeri;
            }
        }

        public static string GetNTVersion
        {
            get
            {
                int major = Environment.OSVersion.Version.Major; // 10 - 6 - 6 - 6
                int minor = Environment.OSVersion.Version.Minor; // 0  - 3 - 2 - 1
                int win10 = 0; // 2009 - 2004 - 1909 - 1903
                int sp = 0; // 1 - 2
                if (major == 10)
                {
                    string key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion").GetValue("ReleaseId").ToString();
                    if (!string.IsNullOrWhiteSpace(key))
                    {
                        win10 = Convert.ToInt32(key);
                    }
                }
                if (!string.IsNullOrWhiteSpace(Environment.OSVersion.ServicePack))
                {
                    sp = Convert.ToInt32(new Regex(@"\-?\d+").Match(Environment.OSVersion.ServicePack).Value);
                }
                return major + "." + minor + (win10 != 0 ? "." + win10 : "") + (sp != 0 ? "sp" + sp : "");
            }
        }

        public static PreResq NetFramework48 => new PreResq()
        {
            FileName = "net48.exe",
            Url = "https://download.visualstudio.microsoft.com/download/pr/7afca223-55d2-470a-8edc-6a1739ae3252/abd170b4b0ec15ad0222a809b761a036/ndp48-x86-x64-allos-enu.exe",
            Name = ".Net Framework 4.8",
            SilentArgs = "/q /norestart",
            RegistryKey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full",
            RegistryValue = "Release",
            ValueDeğeri = "528048",
            CheckType = RegistryCheckType.GreaterOrEqual
        };

        public static PreResq NetFramework452 => new PreResq()
        {
            FileName = "net452.exe",
            Url = "http://download.microsoft.com/download/E/2/1/E21644B5-2DF2-47C2-91BD-63C560427900/NDP452-KB2901907-x86-x64-AllOS-ENU.exe",
            Name = ".Net Framework 4.5.2",
            SilentArgs = "/q /norestart",
            RegistryKey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full",
            RegistryValue = "Release",
            ValueDeğeri = "379892",
            CheckType = RegistryCheckType.GreaterOrEqual
        };

        public static PreResq NetFramework461 => new PreResq()
        {
            FileName = "net461.exe",
            Url = "http://download.microsoft.com/download/E/4/1/E4173890-A24A-4936-9FC9-AF930FE3FA40/NDP461-KB3102436-x86-x64-AllOS-ENU.exe",
            Name = ".Net Framework 4.6.1",
            SilentArgs = "/q /norestart",
            RegistryKey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full",
            RegistryValue = "Release",
            ValueDeğeri = "394270",
            CheckType = RegistryCheckType.GreaterOrEqual
        };

        public static PreResq VisualC2015x86 => new PreResq()
        {
            FileName = "vc2015-x86.exe",
            Url = "https://download.visualstudio.microsoft.com/download/pr/d60aa805-26e9-47df-b4e3-cd6fcc392333/A06AAC66734A618AB33C1522920654DDFC44FC13CAFAA0F0AB85B199C3D51DC0/VC_redist.x86.exe",
            Name = "Visual C++ 2015 (32-bit)",
            SilentArgs = "",
            RegistryKey = @"SOFTWARE\Microsoft\DevDiv\VC\Servicing\14.0\RuntimeMinimum",
            RegistryValue = "Version",
            ValueDeğeri = "14.26.28720",
            CheckType = RegistryCheckType.Equals
        };

        public static PreResq VisualC2015x64 => new PreResq()
        {
            FileName = "vc2015-x64.exe",
            Url = "https://download.visualstudio.microsoft.com/download/pr/d60aa805-26e9-47df-b4e3-cd6fcc392333/7D7105C52FCD6766BEEE1AE162AA81E278686122C1E44890712326634D0B055E/VC_redist.x64.exe",
            Name = "Visual C++ 2015 (64-bit)",
            SilentArgs = "",
            RegistryKey = @"SOFTWARE\Microsoft\DevDiv\VC\Servicing\14.0\RuntimeMinimum",
            RegistryValue = "Version",
            ValueDeğeri = "14.26.28720",
            CheckType = RegistryCheckType.Equals
        };

        public static bool SystemSupportsNet452 => (GetNTVersion == "6.0sp2") // Vista SP2
                    || (GetNTVersion == "6.1sp1") // 7 SP1
                    || (GetNTVersion == "6.2") // 8
                    || (GetNTVersion == "6.3") // 8.1
                    || (GetNTVersion.StartsWith("10.0")); // All Windows 10 versions 

        public static bool SystemSupportsNet461 => (GetNTVersion == "6.1sp1") // 7 SP1
                    || (GetNTVersion == "6.2") // 8
                    || (GetNTVersion == "6.3") // 8.1
                    || (GetNTVersion.StartsWith("10.0")); // All Windows 10 versions 
        public static bool SystemSupportsVisualC2015x86 =>
                // Visual C++ 2015 & .Net Framework 4.5 supports same operating system reqs 
                // and since this application can run, we don't have to check for versions like in the other ones.
                true;
        public static bool SystemSupportsNet48 => (GetNTVersion == "6.1sp1") // 7 SP1
                     || (GetNTVersion == "6.2") // 8
                     || (GetNTVersion == "6.3") // 8.1
                     || (GetNTVersion == "10.0.1607") // 10 anniversary
                     || (GetNTVersion == "10.0.1703") // 10 creators
                     || (GetNTVersion == "10.0.1709") // 10 fall creators
                     || (GetNTVersion == "10.0.1803") // 10 april 2018
                     || (GetNTVersion == "10.0.1809") // 10 october 2018
                     || (GetNTVersion == "10.0.1903") // 10 may 2019
                     || (GetNTVersion == "10.0.1909") // 10 october 2019
                     || (GetNTVersion == "10.0.2004") // 10 may 2020
                     || (GetNTVersion == "10.0.2009"); // 10 october 2020
        public static bool SystemSupportsVisualC2015x64 =>
                // if visualc++2015 x86 is supported, then only thing we need to check is if it runs on x64.
                SystemSupportsVisualC2015x86 && is64BitMachine;
        public class PreResq
        {
            public string Name { get; set; }
            public string Url { get; set; }
            public string FileName { get; set; }
            public string SilentArgs { get; set; }
            public string RegistryKey { get; set; }
            public string RegistryValue { get; set; }

            /// <summary>
            /// value's value
            /// yeah
            /// weededit
            /// </summary>
            public string ValueDeğeri { get; set; }
            public RegistryCheckType CheckType { get; set; }
        }
        public enum RegistryCheckType
        {
            /// <summary>
            /// ==
            /// </summary>
            Equals,
            /// <summary>
            /// >
            /// </summary>
            GreaterThan,
            /// <summary>
            /// <
            /// </summary>
            LessThan,
            /// <summary>
            /// !=
            /// </summary>
            Different,
            /// <summary>
            /// Contains()
            /// </summary>
            Contains,
            /// <summary>
            /// !Contains()
            /// </summary>
            DoesNotContain,
            /// <summary>
            /// >=
            /// </summary>
            GreaterOrEqual,
            /// <summary>
            /// <=
            /// </summary>
            LessOrEqual
        }
    }
}
