using HTAlt;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Yorot
{
    /// <summary>
    /// Yorot Extension Manager
    /// </summary>
    public class ExtensionManager
    {
        /// <summary>
        /// Creates a new Extension manager.
        /// </summary>
        /// <param name="configFile">Location of the configuration file in drive.</param>
        public ExtensionManager(string configFile)
        {
            if(!string.IsNullOrWhiteSpace(configFile))
            {
                if (System.IO.File.Exists(configFile))
                {
                    try
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(HTAlt.Tools.ReadFile(configFile, System.Text.Encoding.Unicode));
                        XmlNode rootNode = Yorot.Tools.FindRoot(doc.DocumentElement);
                        for(int i = 0; i < rootNode.ChildNodes.Count;i++)
                        {
                            var node = rootNode.ChildNodes[i];
                            if (node.Name == "Extension")
                            {
                                if (node.Attributes["CodeName"] != null)
                                {
                                    try
                                    {
                                        YorotExtension ext = new YorotExtension(node.Attributes["CodeName"].Value.InnerXmlToString(), this);
                                        if (node.Attributes["isPinned"] != null)
                                        {
                                            ext.isPinned = node.Attributes["isPinned"].Value.InnerXmlToString() == "true";
                                        }
                                        Extensions.Add(ext);
                                    }catch (Exception e)
                                    {
                                        Output.WriteLine("[ExtMan] Threw away \"" + node.OuterXml + "\", exception caught: " + e.ToString(), LogLevel.Warning);
                                    }
                                }
                                else
                                {
                                    Output.WriteLine("[ExtMan] Threw away \"" + node.OuterXml + "\", configuration does not includes \"CodeName\" attribute.", LogLevel.Warning);
                                }
                            }else
                            {
                                if (!node.OuterXml.StartsWith("<!--"))
                                {
                                    Output.WriteLine("[ExtMan] Threw away \"" + node.OuterXml + "\", unsupported.", LogLevel.Warning);
                                }
                            }
                        }
                    }catch (XmlException)
                    {
                        Output.WriteLine("[ExtMan] Loaded default configurations, configuration file has XML error(s).", LogLevel.Warning);
                    }
                    catch (Exception ex)
                    {
                        Output.WriteLine("[ExtMan] Loaded default configurations, exception caught: " + ex.ToString(), LogLevel.Warning);
                    }
                }else
                {
                    Output.WriteLine("[ExtMan] Loaded default configurations, configuration file does not exists.", LogLevel.Warning);
                }
            }else
            {
                Output.WriteLine("[ExtMan] Loaded default configurations, configuration file path was empty.",LogLevel.Warning);
            }
        }
        /// <summary>
        /// Settings used by this manager.
        /// </summary>
        public Settings Settings { get; set; }
        public List<YorotExtension> Extensions { get; set; } = new List<YorotExtension>();
        /// <summary>
        /// Prints the current configuration as XML.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public string ToXml()
        {
            string x = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" + Environment.NewLine +
                "<root>" + Environment.NewLine +
                "<!-- Yorot Extensions Config File" + Environment.NewLine + Environment.NewLine +
                "This file is used to save browser extensions." + Environment.NewLine +
                "Editing this file might cause problems with Yorot and/or extensions." + Environment.NewLine +
                "-->" + Environment.NewLine;
            for (int i = 0; i < Extensions.Count; i++)
            {
                var ext = Extensions[i];
                x += "<Extension CodeName=\"" + ext.CodeName.ToXML() + "\" isPinned=\"" + (ext.isPinned ? "true" : "false") + "\" />" + Environment.NewLine;
            }
            return (x + "</root>").BeautifyXML();
        }
        /// <summary>
        /// Saves 
        /// </summary>
        public void Save()
        {
            HTAlt.Tools.WriteFile(Settings.UserExt, ToXml(), System.Text.Encoding.Unicode);
        }
    }
    /// <summary>
    /// Yorot Extension.
    /// </summary>
    public class YorotExtension
    {
        /// <summary>
        /// Creates a new yorot Extension.
        /// </summary>
        /// <param name="manifestFile">Location of the manifest file for this extension on drive.</param>
        public YorotExtension(string codeName,ExtensionManager extman)
        {
            if (extman == null)
            {
                throw new ArgumentNullException("Extension Manager cannot be null.");
            }
            if (!string.IsNullOrWhiteSpace(codeName))
            {
                CodeName = codeName;
                ManifestFile = extman.Settings.ExtLoc + codeName + "\\ext.yem";
                if (System.IO.File.Exists(ManifestFile))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(HTAlt.Tools.ReadFile(ManifestFile, System.Text.Encoding.Unicode));
                    List<string> appliedConfig = new List<string>();
                    XmlNode rootNode = Yorot.Tools.FindRoot(doc.DocumentElement);
                    for(int i =0; i < rootNode.ChildNodes.Count;i++)
                    {
                        var node = rootNode.ChildNodes[i];
                        // TODO: Use this info
                        // To avoid language alphabets not suiting English lowercase one such as  
                        // Turkish I -> ı i -> İ  
                        // English I <-> i
                        // always use an English culture info (like here, I used English (United States) culture)
                        // also MS sucks at showing this example so don't bother looking for ToLower() func. documentation.
                        string nodeName = node.Name.ToLower(new System.Globalization.CultureInfo("en-US", false));
                        switch (nodeName) 
                        {
                            case "name":
                                if(appliedConfig.FindAll(it => it == nodeName).Count > 0)
                                {
                                    Output.WriteLine("[Extension:\"" + CodeName + "\"] Threw away \"" + node.OuterXml + "\", configuration already applied.");
                                    break;
                                }
                                appliedConfig.Add(nodeName);
                                Name = node.InnerXml.InnerXmlToString();
                                break;
                            case "author":
                                if (appliedConfig.FindAll(it => it == nodeName).Count > 0)
                                {
                                    Output.WriteLine("[Extension:\"" + CodeName + "\"] Threw away \"" + node.OuterXml + "\", configuration already applied.");
                                    break;
                                }
                                appliedConfig.Add(nodeName);
                                Author = node.InnerXml.InnerXmlToString();
                                break;
                            case "version":
                                if (appliedConfig.FindAll(it => it == nodeName).Count > 0)
                                {
                                    Output.WriteLine("[Extension:\"" + CodeName + "\"] Threw away \"" + node.OuterXml + "\", configuration already applied.");
                                    break;
                                }
                                appliedConfig.Add(nodeName);
                                Version = int.Parse(node.InnerXml.InnerXmlToString());
                                break;
                            case "icon":
                                if (appliedConfig.FindAll(it => it == nodeName).Count > 0)
                                {
                                    Output.WriteLine("[Extension:\"" + CodeName + "\"] Threw away \"" + node.OuterXml + "\", configuration already applied.");
                                    break;
                                }
                                appliedConfig.Add(nodeName);
                                Icon = node.InnerXml.InnerXmlToString();
                                break;
                            case "size":
                                if (appliedConfig.FindAll(it => it == nodeName).Count > 0)
                                {
                                    Output.WriteLine("[Extension:\"" + CodeName + "\"] Threw away \"" + node.OuterXml + "\", configuration already applied.");
                                    break;
                                }
                                appliedConfig.Add(nodeName);
                                string innertext = node.InnerXml.InnerXmlToString();
                                string w = innertext.Substring(0, innertext.IndexOf(';'));
                                string h = innertext.Substring(innertext.IndexOf(';'), innertext.Length - innertext.IndexOf(';'));
                                Size = new System.Drawing.Size(int.Parse(w), int.Parse(h));
                                break;
                            case "popup":
                                if (appliedConfig.FindAll(it => it == nodeName).Count > 0)
                                {
                                    Output.WriteLine("[Extension:\"" + CodeName + "\"] Threw away \"" + node.OuterXml + "\", configuration already applied.");
                                    break;
                                }
                                appliedConfig.Add(nodeName);
                                Popup = node.InnerXml.InnerXmlToString();
                                break;
                            case "startup":
                                if (appliedConfig.FindAll(it => it == nodeName).Count > 0)
                                {
                                    Output.WriteLine("[Extension:\"" + CodeName + "\"] Threw away \"" + node.OuterXml + "\", configuration already applied.");
                                    break;
                                }
                                appliedConfig.Add(nodeName);
                                Startup = node.InnerXml.InnerXmlToString();
                                break;
                            case "background":
                                if (appliedConfig.FindAll(it => it == nodeName).Count > 0)
                                {
                                    Output.WriteLine("[Extension:\"" + CodeName + "\"] Threw away \"" + node.OuterXml + "\", configuration already applied.");
                                    break;
                                }
                                appliedConfig.Add(nodeName);
                                Background = node.InnerXml.InnerXmlToString();
                                break;
                            case "files":
                                if (appliedConfig.FindAll(it => it == nodeName).Count > 0)
                                {
                                    Output.WriteLine("[Extension:\"" + CodeName + "\"] Threw away \"" + node.OuterXml + "\", configuration already applied.");
                                    break;
                                }
                                appliedConfig.Add(nodeName);
                                for(int ı = 0; ı < node.ChildNodes.Count;ı++)
                                {
                                    var subnode = node.ChildNodes[ı];
                                    if (subnode.Name.ToLower(new System.Globalization.CultureInfo("en-US",false)) == "file")
                                    {
                                        Files.Add(subnode.InnerXml.InnerXmlToString());
                                    }else
                                    {
                                        Output.WriteLine("[Extension:\"" + CodeName + "\"] Threw away \"" + subnode.OuterXml + "\", unsupported.");
                                    }
                                }
                                break;
                            case "settings":
                                if (appliedConfig.FindAll(it => it == nodeName).Count > 0)
                                {
                                    Output.WriteLine("[Extension:\"" + CodeName + "\"] Threw away \"" + node.OuterXml + "\", configuration already applied.");
                                    break;
                                }
                                appliedConfig.Add(nodeName);
                                for (int ı = 0; ı < node.ChildNodes.Count; ı++)
                                {
                                    var subnode = node.ChildNodes[ı];
                                    switch(subnode.Name)
                                    {
                                        case "autoLoad":
                                            if (appliedConfig.FindAll(it => it == subnode.Name).Count > 0)
                                            {
                                                Output.WriteLine("[Extension:\"" + CodeName + "\"] Threw away \"" + subnode.OuterXml + "\", configuration already applied.");
                                                break;
                                            }
                                            appliedConfig.Add(subnode.Name);
                                            Settings.autoLoad = subnode.InnerXml.InnerXmlToString() == "true";
                                            break;
                                        case "showPopupMenu": 
                                            if (appliedConfig.FindAll(it => it == subnode.Name).Count > 0)
                                            {
                                                Output.WriteLine("[Extension:\"" + CodeName + "\"] Threw away \"" + subnode.OuterXml + "\", configuration already applied.");
                                                break;
                                            }
                                            appliedConfig.Add(subnode.Name);
                                            Settings.showPopupMenu = subnode.InnerXml.InnerXmlToString() == "true";
                                            break;
                                        case "hasProxy": 
                                            if (appliedConfig.FindAll(it => it == subnode.Name).Count > 0)
                                            {
                                                Output.WriteLine("[Extension:\"" + CodeName + "\"] Threw away \"" + subnode.OuterXml + "\", configuration already applied.");
                                                break;
                                            }
                                            appliedConfig.Add(subnode.Name);
                                            Settings.hasProxy = subnode.InnerXml.InnerXmlToString() == "true";
                                            break;
                                        case "useHTUpdate": 
                                            if (appliedConfig.FindAll(it => it == subnode.Name).Count > 0)
                                            {
                                                Output.WriteLine("[Extension:\"" + CodeName + "\"] Threw away \"" + subnode.OuterXml + "\", configuration already applied.");
                                                break;
                                            }
                                            appliedConfig.Add(subnode.Name);
                                            Settings.useHTUpdate = subnode.InnerXml.InnerXmlToString() == "true";
                                            break;
                                        default:
                                            if(!subnode.OuterXml.StartsWith("<!--"))
                                            {
                                                Output.WriteLine("[Extension:\"" + CodeName + "\"] Threw away \"" + subnode.OuterXml + "\", unsupported.");
                                            }
                                            break;
                                    }
                                }
                                break;
                            case "pagelist":
                                if (appliedConfig.FindAll(it => it == nodeName).Count > 0)
                                {
                                    Output.WriteLine("[Extension:\"" + CodeName + "\"] Threw away \"" + node.OuterXml + "\", configuration already applied.");
                                    break;
                                }
                                appliedConfig.Add(nodeName);
                                for (int ı = 0; ı < node.ChildNodes.Count; ı++)
                                {
                                    var subnode = node.ChildNodes[ı];
                                    if (subnode.Name.ToLower(new System.Globalization.CultureInfo("en-US", false)) == "page")
                                    {
                                        PageList.Add(subnode.InnerXml.InnerXmlToString());
                                    }
                                    else
                                    {
                                        Output.WriteLine("[Extension:\"" + CodeName + "\"] Threw away \"" + subnode.OuterXml + "\", unsupported.");
                                    }
                                }
                                break;
                            case "rcoptions":
                                if (appliedConfig.FindAll(it => it == nodeName).Count > 0)
                                {
                                    Output.WriteLine("[Extension:\"" + CodeName + "\"] Threw away \"" + node.OuterXml + "\", configuration already applied.");
                                    break;
                                }
                                appliedConfig.Add(nodeName);
                                for (int ı = 0; ı < node.ChildNodes.Count; ı++)
                                {
                                    var subnode = node.ChildNodes[ı];
                                    if (subnode.Name.ToLower(new System.Globalization.CultureInfo("en-US", false)) == "rcoption")
                                    {
                                        // TODO 
                                    }
                                    else
                                    {
                                        Output.WriteLine("[Extension:\"" + CodeName + "\"] Threw away \"" + subnode.OuterXml + "\", unsupported.");
                                    }
                                }
                                break;
                            default:
                                if (!node.OuterXml.StartsWith("<!--"))
                                {
                                    Output.WriteLine("[Extension:\"" + CodeName + "\"] Threw away \"" + node.OuterXml + "\", unsupported.");
                                }
                                break;
                        }
                    }
                }else
                {
                    throw new Exception("Manifest file does not exists.");
                }
            }else
            {
                throw new Exception("Extension code name was empty.");
            }
        }
        /// <summary>
        /// Associated extension manager for this extension.
        /// </summary>
        public ExtensionManager Manager { get; set; }
        /// <summary>
        /// Determines if this extension is pinned to navigation bar.
        /// </summary>
        public bool isPinned { get; set; }
        /// <summary>
        /// Location fo the manifest file for this extension on drive.
        /// </summary>
        public string ManifestFile { get; set; }
        /// <summary>
        /// Code Name of the extension.
        /// </summary>
        public string CodeName { get; set; }
        /// <summary>
        /// Display name of the extension.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Author of the extension.
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// Version of the extension.
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// Display Icon of the extension.
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// Sİze of the Pop-up menu used in this extension.
        /// </summary>
        public System.Drawing.Size Size { get; set; }
        /// <summary>
        /// Document to load as pop-up menu.
        /// </summary>
        public string Popup { get; set; }
        /// <summary>
        /// Script to run on Yorot startup.
        /// </summary>
        public string Startup { get; set; }
        /// <summary>
        /// Script to run on background for allowed pages.
        /// </summary>
        public string Background { get; set; }
        /// <summary>
        /// List of locations to files that used in this extension.
        /// </summary>
        public List<string> Files { get; set; } = new List<string>();
        /// <summary>
        /// Settings for this extension.
        /// </summary>
        public YorotExtensionSettings Settings { get; set; } = new YorotExtensionSettings();
        /// <summary>
        /// List of pages that user allowed this extension to run.
        /// </summary>
        public List<string> PageList { get; set; } = new List<string>();
        /// <summary>
        /// Right-click options for this extension.
        /// </summary>
        public List<YorotExtensionRCOption> RCOptions { get; set; } = new List<YorotExtensionRCOption>();
    }
    /// <summary>
    /// Right-click options.
    /// </summary>
    public class YorotExtensionRCOption
    {
        /// <summary>
        /// Text to display in option
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Location of the script to run when user clicks on drive.
        /// </summary>
        public string Script { get; set; }
        /// <summary>
        /// Icon to display near text.
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// Determines when to display this item.
        /// </summary>
        public RightClickOptionStyle Option { get; set; }
    }
    /// <summary>
    /// Right-Click options styles for extensions.
    /// </summary>
    public enum RightClickOptionStyle
    {
        /// <summary>
        /// Shows option when nothing is selected
        /// </summary>
        None,
        /// <summary>
        /// Shows option when user hovering or selected a link.
        /// </summary>
        Link,
        /// <summary>
        /// Shows option if user hovering or selected an image.
        /// </summary>
        Image,
        /// <summary>
        /// Shows option if user hovered or selected a text.
        /// </summary>
        Text,
        /// <summary>
        /// Shows option if user hovered or selected an edit text box.
        /// </summary>
        Edit,
        /// <summary>
        /// Always shows option on anything.
        /// </summary>
        Always
    }
    /// <summary>
    /// Settings for Yorot Extensions.
    /// </summary>
    public class YorotExtensionSettings
    {
        /// <summary>
        /// Load extension on Yorot start.
        /// </summary>
        public bool autoLoad { get; set; } = false;
        /// <summary>
        /// If set to <see cref="true"/>, shows pop-up menu when user clicks on extension icon.
        /// </summary>
        public bool showPopupMenu { get; set; } = false;
        /// <summary>
        /// <see cref="true"/> if Extension has Proxy manipulations.
        /// </summary>
        public bool hasProxy { get; set; } = false;
        /// <summary>
        /// <see cref="true"/> if this extension can be auto-updated.
        /// </summary>
        public bool useHTUpdate { get; set; } = false;
    }
}
