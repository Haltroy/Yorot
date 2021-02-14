using HTAlt;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Yorot
{
    /// <summary>
    /// History management for Yorot. NOTE: You (dev) have to add sites to this manager manually. Check out other Yorot projects inside of solution for simple implementations.
    /// </summary>
    public class HistoryManager
    {
        /// <summary>
        /// Creates a new History manager.
        /// </summary>
        /// <param name="configFile">Location of configuration file on drive.</param>
        public HistoryManager(string configFile)
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
                        for(int i = 0;i < rootNode.ChildNodes.Count;i++)
                        {
                            var node = rootNode.ChildNodes[i];
                            switch(node.Name)
                            {
                                case "History":
                                    if (appliedSettings.Contains(node.Name))
                                    {
                                        Output.WriteLine("[HistoryMan] Threw away \"" + node.OuterXml + "\", configuration already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    appliedSettings.Add(node.Name);
                                    for (int ı = 0; ı < node.ChildNodes.Count;ı++)
                                    {
                                        var subnode = node.ChildNodes[ı];
                                        if (subnode.Name == "Site")
                                        {
                                            if (subnode.Attributes["Name"] != null && subnode.Attributes["Url"] != null && subnode.Attributes["Date"] != null)
                                            {
                                                Sites.Add(new YorotSite()
                                                {
                                                    Name = subnode.Attributes["Name"].Value.InnerXmlToString(),
                                                    Url = subnode.Attributes["Url"].Value.InnerXmlToString(),
                                                    Date = DateTime.ParseExact(subnode.Attributes["Date"].Value.InnerXmlToString(), "dd-MM-yyyy HH-mm-ss", null),
                                                });
                                            }else
                                            {
                                                Output.WriteLine("[HistoryMan] Threw away \"" + node.OuterXml + "\", invalid site configuration.", LogLevel.Warning);
                                            }
                                        }else
                                        {
                                            Output.WriteLine("[HistoryMan] Threw away \"" + subnode.OuterXml + "\", unsupported.", LogLevel.Warning);
                                        }
                                    }
                                    break;
                                default:
                                    if (!node.OuterXml.StartsWith("<!--"))
                                    {
                                        Output.WriteLine("[HistoryMan] Threw away \"" + node.OuterXml + "\", invalid configuration.", LogLevel.Warning);
                                    }
                                    break;
                            }
                        }
                    }
                    catch (XmlException)
                    {
                        Output.WriteLine("[HistoryMan] Loaded default configuration, configuration file has XML errors.", LogLevel.Warning);
                    }
                    catch (Exception ex)
                    {
                        Output.WriteLine("[HistoryMan] Loaded default configuration, Exception caught: " + ex.ToString(), LogLevel.Warning);
                    }
                }
                else
                {
                    Output.WriteLine("[HistoryMan] Loaded default configuration, configuration file \"" + configFile +  "\" not found.", LogLevel.Warning);
                }
            }else
            {
                Output.WriteLine("[HistoryMan] Loaded default configuration, parameter \"configFile\" was empty.", LogLevel.Warning);
            }
        }
        /// <summary>
        /// Yorot Settings used in this manager.
        /// </summary>
        public Settings Settings { get; set; }
        /// <summary>
        /// YorotSites of this manager.
        /// </summary>
        public List<YorotSite> Sites { get; set; } = new List<YorotSite>();
        /// <summary>
        /// Exports current status to  XML format. Used by Save() command.
        /// </summary>
        public string ToXml()
        {
            string x = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" + Environment.NewLine +
                "<root>" + Environment.NewLine +
                "<!-- Yorot History Config File" + Environment.NewLine + Environment.NewLine +
                "This file is used to save browser history." + Environment.NewLine +
                "Editing this file might cause problems with Yorot." + Environment.NewLine +
                "-->" + Environment.NewLine +
                "<History>" + Environment.NewLine;
            for (int i = 0; i < Sites.Count; i++)
            {
                var site = Sites[i];
                x += "<Site Name=\"" + site.Name.ToXML() + "\" Url=\"" + site.Url.ToXML() + "\" Date=\"" + site.Date.ToString("dd-MM-yyyy HH-mm-ss") + "\" />" + Environment.NewLine;
            }
            return (x + "</History>" + Environment.NewLine + "</root>").BeautifyXML();
        }
        /// <summary>
        /// Saves history to drive.
        /// </summary>
        public void Save()
        {
            HTAlt.Tools.WriteFile(Settings.UserHistory, ToXml(), Encoding.Unicode);
        }
    }
}
