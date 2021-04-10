using HTAlt;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Yorot
{
    /// <summary>
    /// Yorot Downloads manager. NOTE: This download manager does not includes engine-specific downloads queue (ex. CefSharp's DownloadItem) and also works similar to History manager meaning that you have to add items manually by code.
    /// </summary>
    public class DownloadManager : YorotManager
    {
        public DownloadManager(string configFile,YorotMain main) : base(configFile,main)
        {}
        /// <summary>
        /// A List of previous downloads.
        /// </summary>
        public List<YorotSite> Downloads { get; set; } = new List<YorotSite>();
        /// <summary>
        /// If <see cref="true"/>, opens files after they were downloaded.
        /// </summary>
        public bool OpenFilesAfterDownload { get; set; } = false;
        /// <summary>
        /// Auto-downloads to a directory.
        /// </summary>
        public bool AutoDownload { get; set; } = true;
        /// <summary>
        /// Location of auto-downloads. 
        /// </summary>
        public string DownloadFolder { get; set; } = "";
        /// <summary>
        /// Gives current configuration to XML format.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public override string ToXml()
        {
            string x = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" + Environment.NewLine +
           "<root>" + Environment.NewLine +
           "<!-- Yorot Downloads Config File" + Environment.NewLine + Environment.NewLine +
            "This file is used to save browser downloads history." + Environment.NewLine +
           "Editing this file might cause problems with Yorot." + Environment.NewLine +
           "-->" + Environment.NewLine +
           "<Downloads>" + Environment.NewLine;
            for (int i = 0; i < Downloads.Count; i++)
            {
                var site = Downloads[i];
                x += "<Download Name=\"" + site.Name.ToXML() + "\" Url=\"" + site.Url.ToXML() + "\" Location=\"" + site.FilePath + "\" Date=\"" + site.Date.ToString("dd-MM-yyyy HH-mm-ss") + "\" Status=\"" + (int)site.Status + "\" " + (site.ErrorCode != null ? "ErrorCode=\"" + site.ErrorCode.ToString().ToXML() + "\" " : "") + "/>" + Environment.NewLine;
            }
            return (x + "</Downloads>" + Environment.NewLine + "</root>").BeautifyXML();
        }

        public override void ExtractXml(XmlNode rootNode)
        {
            List<string> appliedSettings = new List<string>();
            for (int ı = 0; ı < rootNode.ChildNodes.Count; ı++)
            {
                var node = rootNode.ChildNodes[ı];
                switch (node.Name)
                {
                    case "Downloads":
                        if (appliedSettings.FindAll(it => it == node.Name).Count > 0)
                        {
                            Output.WriteLine("[DownloadManager] Threw away \"" + node.OuterXml + "\", configuration already applied.", LogLevel.Warning);
                            break;
                        }
                        appliedSettings.Add(node.Name);
                        for (int i = 0; i < node.ChildNodes.Count; i++)
                        {
                            var subnode = node.ChildNodes[i];
                            if (subnode.Name == "Download" && subnode.Attributes["Url"] != null && subnode.Attributes["Name"] != null && subnode.Attributes["Status"] != null)
                            {
                                YorotSite download = new YorotSite();
                                download.Name = subnode.Attributes["Name"].Value.InnerXmlToString();
                                download.Url = subnode.Attributes["Url"].Value.InnerXmlToString();
                                download.Status = (YorotSiteStatus)int.Parse(subnode.Attributes["Status"].Value);
                                if (subnode.Attributes["Error"] != null) { download.ErrorCode = subnode.Attributes["Error"].Value.InnerXmlToString(); }
                                if (subnode.Attributes["Location"] != null) { download.FilePath = subnode.Attributes["Location"].Value.InnerXmlToString(); }
                                if (subnode.Attributes["Date"] != null) { download.Date = DateTime.ParseExact(subnode.Attributes["Date"].Value.InnerXmlToString(), "dd-MM-yyyy HH-mm-ss", null); }
                                Downloads.Add(download);
                            }
                        }
                        break;
                    default:
                        if (!node.OuterXml.StartsWith("<!--"))
                        {
                            Output.WriteLine("[DownloadManager] Threw away \"" + node.OuterXml + "\", unsupported.", LogLevel.Warning);
                        }
                        break;
                }
            }
        }
    }
}
