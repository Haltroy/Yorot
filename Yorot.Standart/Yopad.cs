using HTAlt;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Yorot
{
    /// <summary>
    /// <c>Yo</c>rot <c>Pa</c>ckage <c>D</c>istribution service.
    /// </summary>
    public class Yopad : YorotManager
    {
        /// <summary>
        /// Creates a new Yopad Service.
        /// </summary>
        /// <param name="main"><see cref="YorotMain"/></param>
        public Yopad(YorotMain main) : base(main.YopadConfig, main) { Repositories.Add(new YopadRepository() { Name = "Yopad Official Repository", ExpireTime = 300, CodeName = "com.haltroy.yopad", isEnabled = true, Description = "The official Yopad repository.", Yopad = this, Url = "https://raw.githubusercontent.com/Haltroy/Yopad/main/index.yrf" }); }

        /// <summary>
        /// A list of HTUPDATEs of installed add-ons.
        /// </summary>
        public List<HTUPDATE> HTUPDATEs { get; set; } = new List<HTUPDATE>();

        /// <summary>
        /// A list of Yopad repositories.
        /// </summary>
        public List<YopadRepository> Repositories { get; set; } = new List<YopadRepository>();

        /// <summary>
        /// Registers HTUPDATE.
        /// </summary>
        /// <param name="url">URL of HTUPDATE.</param>
        /// <param name="self">Add-on</param>
        public void RegisterHTU(string url, object self)
        {
            string name = string.Empty;
            string temp = Main.TempFolder;
            string work = string.Empty;
            int version = 1;
            switch (self)
            {
                case YorotApp _:
                    var app = self as YorotApp;
                    name = app.AppCodeName;
                    temp += name + "\\";
                    work = Main.AppsFolder + name;
                    version = app.VersionNo;
                    break;

                case YorotExtension _:
                    var ext = self as YorotExtension;
                    name = ext.CodeName;
                    temp += name + "\\";
                    work = Main.ExtFolder + name;
                    version = ext.Version;
                    break;

                case YorotLanguage _:
                    var lang = self as YorotLanguage;
                    name = lang.CodeName;
                    temp = name + "\\";
                    work = Main.LangFolder + name;
                    version = lang.Version;
                    break;

                case YorotTheme _:
                    var theme = self as YorotTheme;
                    name = theme.CodeName;
                    temp += name + "\\";
                    work = Main.ThemesFolder + name;
                    version = theme.Version;
                    break;

                case YorotWebEngine _:
                    var we = self as YorotWebEngine;
                    name = we.CodeName;
                    temp = name + "\\";
                    work = Main.WEFolder + name;
                    version = we.Version;
                    break;

                case ExpPack _:
                    var ep = self as ExpPack;
                    name = ep.CodeName;
                    temp += name + "\\";
                    work = Main.EPFolder + name;
                    version = ep.Version;
                    break;
            }
            var htu = new HTUPDATE(name, url, work, temp, version, "noarch");
            htu.DoTaskAsAsync = false;
            htu.OnLogEntry += YopadLog;
            HTUPDATEs.Add(htu);
        }

        /// <summary>
        /// Refreshes repositories without freezing app.
        /// </summary>
        /// <param name="force"><c>true</c> to refresh even not-expired repositories, otherwise <c>false</c>.</param>
        public async void RefreshReposAsync(bool force = false)
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                RefreshRepos(force);
            });
        }

        /// <summary>
        /// Refreshes repositories (freezes app).
        /// </summary>
        /// <param name="force"><c>true</c> to refresh even not-expired repositories, otherwise <c>false</c>.</param>
        public void RefreshRepos(bool force = false)
        {
            for (int i = 0; i < Repositories.Count; i++)
            {
                var repo = Repositories[i];
                try
                {
                    if ((force || repo.ExpireDate.AddSeconds(repo.ExpireTime).HasExpired()) && repo.isEnabled)
                    {
                        YopadLog(this, new OnLogEntryEventArgs((force ? "Force check on repository: " : "Check on repository: ") + repo.CodeName, LogEventType.Info));
                        var e = new YopadProgressEventArgs() { Total = 100, Received = 0 };
                        YopadProgress(repo, e);
                        System.Net.WebClient wc = new System.Net.WebClient();
                        string repoXml = wc.DownloadString(repo.Url);
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(repoXml);
                        XmlNode rootNode = doc.FindRoot();
                        List<string> applied = new List<string>();
                        for (int _i = 0; _i < rootNode.ChildNodes.Count; _i++)
                        {
                            var node = rootNode.ChildNodes[_i];
                            switch (node.Name.ToLowerEnglish())
                            {
                                case "name":
                                    if (applied.Contains(node.Name.ToLowerEnglish()))
                                    {
                                        YopadLog(this, new OnLogEntryEventArgs("[" + repo.CodeName + "] Threw out \"" + node.OuterXml + "\", configuration already applied.", LogEventType.Warning));
                                        break;
                                    }
                                    applied.Add(node.Name.ToLowerEnglish());
                                    repo.Name = node.InnerXml.XmlToString();
                                    break;

                                case "codename":
                                    if (applied.Contains(node.Name.ToLowerEnglish()))
                                    {
                                        YopadLog(this, new OnLogEntryEventArgs("[" + repo.CodeName + "] Threw out \"" + node.OuterXml + "\", configuration already applied.", LogEventType.Warning));
                                        break;
                                    }
                                    applied.Add(node.Name.ToLowerEnglish());
                                    repo.CodeName = node.InnerXml.XmlToString();
                                    break;

                                case "description":
                                    if (applied.Contains(node.Name.ToLowerEnglish()))
                                    {
                                        YopadLog(this, new OnLogEntryEventArgs("[" + repo.CodeName + "] Threw out \"" + node.OuterXml + "\", configuration already applied.", LogEventType.Warning));
                                        break;
                                    }
                                    applied.Add(node.Name.ToLowerEnglish());
                                    repo.Description = node.InnerXml.XmlToString();
                                    break;

                                case "expire":
                                    if (applied.Contains(node.Name.ToLowerEnglish()))
                                    {
                                        YopadLog(this, new OnLogEntryEventArgs("[" + repo.CodeName + "] Threw out \"" + node.OuterXml + "\", configuration already applied.", LogEventType.Warning));
                                        break;
                                    }
                                    applied.Add(node.Name.ToLowerEnglish());
                                    repo.ExpireTime = int.Parse(node.InnerXml.XmlToString());
                                    break;

                                case "ref":
                                    if (node.Attributes["Name"] != null && node.Attributes["Url"] != null)
                                    {
                                        string refname = node.Attributes["Name"].Value.XmlToString();
                                        string refurl = node.Attributes["Url"].Value.XmlToString();
                                        if (!string.IsNullOrWhiteSpace(refname) && !string.IsNullOrWhiteSpace(refurl))
                                        {
                                            if (repo.Addons.FindAll(it => it.Name == refname).Count > 0)
                                            {
                                                var _ref = repo.Addons.FindAll(it => it.Name == refname)[0];
                                                string refdesc = string.Empty;
                                                if (node.Attributes["Description"] != null) { refdesc = node.Attributes["Description"].Value.XmlToString(); }
                                                _ref.Name = refname;
                                                _ref.Url = refurl;
                                                _ref.Description = refdesc;
                                                // TODO: Add categories and refresh list
                                            }
                                            else
                                            {
                                                string refdesc = string.Empty;
                                                if (node.Attributes["Description"] != null) { refdesc = node.Attributes["Description"].Value.XmlToString(); }
                                                var _ref = new YopadAddonList(repo, refname, refdesc, refurl);
                                                repo.Addons.Add(_ref);
                                                // TODO: Add categories and refresh list
                                            }
                                        }
                                        else
                                        {
                                            YopadLog(this, new OnLogEntryEventArgs("[" + repo.CodeName + "] Threw out \"" + node.OuterXml + "\", important XMl node attribute(s) missing values.", LogEventType.Warning));
                                        }
                                    }
                                    else
                                    {
                                        YopadLog(this, new OnLogEntryEventArgs("[" + repo.CodeName + "] Threw out \"" + node.OuterXml + "\", missing XMl node attribute(s).", LogEventType.Warning));
                                    }
                                    break;

                                default:
                                    if (!node.IsComment())
                                    {
                                        YopadLog(this, new OnLogEntryEventArgs("[" + repo.CodeName + "] Threw out \"" + node.OuterXml + "\", unsupported.", LogEventType.Warning));
                                    }
                                    break;
                            }
                        }
                        e.Received = 100;
                        YopadProgress(repo, e);
                    }
                }
                catch (Exception ex)
                {
                    YopadLog(this, new OnLogEntryEventArgs("Error while refreshing repository \"" + repo.CodeName + "\". Exception caught: " + ex.ToString(), LogEventType.Error));
                }
            }
        }

        /// <summary>
        /// Updates current add-ons (without freezing).
        /// </summary>
        /// <param name="force"><c>true</c> to refresh even not-expired repositories, otherwise <c>false</c>.</param>
        public async void UpdateAsync(bool force = false)
        {
            await System.Threading.Tasks.Task.Run(() => { Update(force); });
        }

        /// <summary>
        /// Updates current add-ons (might freeze).
        /// </summary>
        /// <param name="force"><c>true</c> to refresh even not-expired repositories, otherwise <c>false</c>.</param>
        public void Update(bool force = false)
        {
            RefreshRepos(force);
            // TODO: Update Current Add-ons
        }

        /// <summary>
        /// Installs an add-on (might freeze).
        /// </summary>
        /// <param name="addon"><see cref="YopadAddon"/></param>
        public void Install(YopadAddon addon)
        {
            // TODO
        }

        /// <summary>
        /// Installs an add-on (without freezing).
        /// </summary>
        /// <param name="addon"><see cref="YopadAddon"/></param>
        public async void InstallAsync(YopadAddon addon)
        {
            await System.Threading.Tasks.Task.Run(() => { Install(addon); });
        }

        /// <summary>
        /// Event handler for Yopad progress changes.
        /// </summary>
        /// <param name="sender"><see cref="object"/></param>
        /// <param name="e"><see cref="YopadProgressEventArgs"/></param>
        public delegate void YopadProgressEventHandler(object sender, YopadProgressEventArgs e);

        /// <summary>
        /// Event raised on Yopad progress change.
        /// </summary>
        public event YopadProgressEventHandler YopadProgress;

        /// <summary>
        /// Event raised when Yopad log.
        /// </summary>
        public event HTUPDATE.OnLogEntryDelegate YopadLog;

        public override string ToXml()
        {
            string x = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" + Environment.NewLine +
                "<root>" + Environment.NewLine +
                "<!-- Yopad Config File" + Environment.NewLine + Environment.NewLine +
                "This file is used to configure Yorot Package Distribution Service." + Environment.NewLine +
                "Editing this file might cause problems with add-on installation." + Environment.NewLine +
                "-->" + Environment.NewLine +
                "<Repos>" + Environment.NewLine;
            for (int i = 0; i < Repositories.Count; i++)
            {
                if (Repositories[i].CodeName.ToLowerEnglish() != "com.haltroy.yopad")
                {
                    x += "<Repo CodeName=\"" + Repositories[i].CodeName.ToXML() + "\" Url=\"" + Repositories[i].Url.ToXML() + "\" Name=\"" + Repositories[i].Name.ToXML() + "\" Expire=\"" + Repositories[i].ExpireTime + "\" />" + Environment.NewLine;
                }
            }
            return (x + "</Repos>" + Environment.NewLine + "</root>").BeautifyXML();
        }

        public override void ExtractXml(XmlNode rootNode)
        {
            List<string> appliedSettings = new List<string>();
            for (int i = 0; i < rootNode.ChildNodes.Count; i++)
            {
                XmlNode node = rootNode.ChildNodes[i];
                switch (node.Name.ToLowerEnglish())
                {
                    case "repos":
                        if (appliedSettings.FindAll(it => it == node.Name).Count > 0)
                        {
                            Output.WriteLine("[Yopad] Threw away \"" + node.OuterXml + "\". Configurtion already applied.", LogLevel.Warning);
                            break;
                        }
                        appliedSettings.Add(node.Name);
                        for (int ı = 0; ı < node.ChildNodes.Count; ı++)
                        {
                            XmlNode subnode = node.ChildNodes[ı];
                            if (subnode.Name.ToLowerEnglish() == "lang")
                            {
                                if (subnode.Attributes["CodeName"] != null && subnode.Attributes["Name"] != null && subnode.Attributes["URL"] != null && subnode.Attributes["Expire"] != null)
                                {
                                    string cn = subnode.Attributes["CodeName"].Value.XmlToString();
                                    string n = subnode.Attributes["Name"].Value.XmlToString();
                                    string url = subnode.Attributes["URL"].Value.XmlToString();
                                    int exp = int.Parse(subnode.Attributes["Expire"].Value.XmlToString());
                                    Repositories.Add(new YopadRepository() { CodeName = cn, Name = n, Url = url, ExpireTime = exp });
                                }
                                else
                                {
                                    Output.WriteLine("[Yopad] Threw away \"" + subnode.OuterXml + "\". missing required atrributes.", LogLevel.Warning);
                                }
                            }
                            else
                            {
                                if (!subnode.IsComment())
                                {
                                    Output.WriteLine("[Yopad] Threw away \"" + subnode.OuterXml + "\". unsupported.", LogLevel.Warning);
                                }
                            }
                        }
                        break;

                    default:
                        if (!node.IsComment())
                        {
                            Output.WriteLine("[Yopad] Threw away \"" + node.OuterXml + "\". Invalid configurtion.", LogLevel.Warning);
                        }
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Ypad Progress Change Event arguments.
    /// </summary>
    public class YopadProgressEventArgs : EventArgs
    {
        /// <summary>
        /// Total bytes.
        /// </summary>
        public long Total { get; set; }

        /// <summary>
        /// Received bytes.
        /// </summary>
        public long Received { get; set; }

        /// <summary>
        /// Bytes to receive.
        /// </summary>
        public long Left => Total - Received;

        /// <summary>
        /// Percentage in 0.x form.
        /// </summary>
        public double Percentage => Received / Total;

        /// <summary>
        /// Percentage in % form.
        /// </summary>
        public long Percentage100 => (long)(Percentage * 100);
    }

    /// <summary>
    /// Yopad repository.
    /// </summary>
    public class YopadRepository
    {
        /// <summary>
        /// Determine if this repository is enabled ro not.
        /// </summary>
        public bool isEnabled { get; set; }

        /// <summary>
        /// URL of the repository.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Service attached to this repository.
        /// </summary>
        public Yopad Yopad { get; set; }

        /// <summary>
        /// Determines which date YOpad should refresh it.
        /// </summary>
        public System.DateTime ExpireDate { get; set; }

        /// <summary>
        /// Name of the repository.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Codename of the repository.
        /// </summary>
        public string CodeName { get; set; }

        /// <summary>
        /// Description of the repository.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Times between refreshes in second.
        /// </summary>
        public int ExpireTime { get; set; }

        /// <summary>
        /// A list of Add-ons in this repository.
        /// </summary>
        public List<YopadAddonList> Addons { get; set; } = new List<YopadAddonList>();
    }

    /// <summary>
    /// A list of Yopad Add-ons.
    /// </summary>
    public class YopadAddonList
    {
        public YopadAddonList(YopadRepository repository, string name, string description, string url)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Url = url ?? throw new ArgumentNullException(nameof(url));
            Categories.Add(new YopadCategory(this, "Uncategorised", "uncategorised", "Add-ons with no categories."));
        }

        /// <summary>
        /// Repository of this list.
        /// </summary>
        public YopadRepository Repository { get; set; }

        /// <summary>
        /// Attached service of this list.
        /// </summary>
        public Yopad Yopad => Repository.Yopad;

        /// <summary>
        /// Name of the Add-on list.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the Add-on list.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// URL of the Add-on list.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Categories of the Add-on list.
        /// </summary>
        public List<YopadCategory> Categories { get; set; } = new List<YopadCategory>();

        /// <summary>
        /// Gets <see cref="YopadCategory"/> by <seealso cref="YopadCategory.CodeName"/>.
        /// </summary>
        /// <param name="cn"><seealso cref="YopadCategory.CodeName"/></param>
        /// <returns><see cref="YopadCategory"/></returns>
        public YopadCategory GetCatByCN(string cn) => Categories.FindAll(it => it.CodeName.ToLowerEnglish() == cn.ToLowerEnglish()).Count > 0 ? Categories.FindAll(it => it.CodeName.ToLowerEnglish() == cn.ToLowerEnglish())[0] : null;
    }

    /// <summary>
    /// Yopad Add-on Category.
    /// </summary>
    public class YopadCategory
    {
        public YopadCategory(YopadAddonList addonList, string name, string codeName, string description)
        {
            AddonList = addonList;
            Name = name;
            CodeName = codeName;
            Description = description;
        }

        /// <summary>
        /// The list of this category.
        /// </summary>
        public YopadAddonList AddonList { get; set; }

        /// <summary>
        /// The repository of this category.
        /// </summary>
        public YopadRepository Repository => AddonList.Repository;

        /// <summary>
        /// Service of this category.
        /// </summary>
        public Yopad Yopad => Repository.Yopad;

        /// <summary>
        /// Name of this category.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Code name of the category.
        /// </summary>
        public string CodeName { get; set; }

        /// <summary>
        /// Description of the category.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Add-ons inside this category.
        /// </summary>
        public List<YopadAddon> Addons { get; set; } = new List<YopadAddon>();
    }

    /// <summary>
    /// Yopad Add-on.
    /// </summary>
    public class YopadAddon
    {
        public YopadAddon(YopadCategory category, string name, string codeName, string description, string hTU_Url, string[] screenshotUrl)
        {
            Category = category ?? throw new ArgumentNullException(nameof(category));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            CodeName = codeName ?? throw new ArgumentNullException(nameof(codeName));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            HTU_Url = hTU_Url ?? throw new ArgumentNullException(nameof(hTU_Url));
            ScreenshotUrl = screenshotUrl ?? throw new ArgumentNullException(nameof(screenshotUrl));
        }

        /// <summary>
        /// Category of this add-on.
        /// </summary>
        public YopadCategory Category { get; set; }

        /// <summary>
        /// Type of this add-on.
        /// </summary>
        public YopadAddonList AddonList => Category.AddonList;

        /// <summary>
        /// Repository of this add-on.
        /// </summary>
        public YopadRepository Repository => AddonList.Repository;

        /// <summary>
        /// Service of this add-on.
        /// </summary>
        public Yopad Yopad => Repository.Yopad;

        /// <summary>
        /// Name of the add-on.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Code name of the add-on.
        /// </summary>
        public string CodeName { get; set; }

        /// <summary>
        /// Description of the add-on.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// HTUPDATE URL of the add-on.
        /// </summary>
        public string HTU_Url { get; set; }

        /// <summary>
        /// A string array containing all screenshots' URLs.
        /// </summary>
        public string[] ScreenshotUrl { get; set; }

        /// <summary>
        /// An array containing all screenshots.
        /// </summary>
        public System.Drawing.Image[] Screenshots { get; set; }

        /// <summary>
        /// The Add-on that associated with this Yopad add-on.
        /// </summary>
        public object AssocAddon { get; set; }

        /// <summary>
        /// Determines if this Add-on is already installer or not.
        /// </summary>
        public bool isInstalled { get; set; }
    }
}