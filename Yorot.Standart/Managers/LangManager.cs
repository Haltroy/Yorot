using HTAlt;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Yorot
{
    /// <summary>
    /// Yorot Language manager.
    /// </summary>
    public class LangManager
    {
        /// <summary>
        /// Creates a new Language manager.
        /// </summary>
        public LangManager()
        {
            AddDefaultVars();
        }
        /// <summary>
        /// Adds default variables to system.
        /// </summary>
        private void AddDefaultVars()
        {
            // LONG-TERM TODO: ADD MORE VARIABLES
            LangVars.Add(new YorotLangVar("NEWLINE", Environment.NewLine));
        }
        /// <summary>
        /// Yorot Settings used in this manager.
        /// </summary>
        public Settings Settings { get; set; }
        /// <summary>
        /// Location of loaded language file in drive.
        /// </summary>
        public string LoadedLangFile { get; set; }
        /// <summary>
        /// Loaded Language Variables.
        /// </summary>
        public List<YorotLangVar> LangVars { get; set; } = new List<YorotLangVar>();
        /// <summary>
        /// Loaded Language Items.
        /// </summary>
        public List<YorotLangItem> LangItems { get; set; } = new List<YorotLangItem>();
        /// <summary>
        /// Name of loaded language.
        /// </summary>
        public string LoadedLangName { get; set; }
        /// <summary>
        /// Autho information about loaded langauge.
        /// </summary>
        public string LoadedLangAuthor { get; set; }
        /// <summary>
        /// Yorot version that loaded language is made for.
        /// </summary>
        public int LoadedLangCompatibleVer { get; set; }
        /// <summary>
        /// Version of loaded language file.
        /// </summary>
        public int LoadedLangVersion { get; set; }
        /// <summary>
        /// Fşnds Language Item from ID.
        /// </summary>
        /// <param name="ID">ID of translation.</param>
        /// <returns><see cref="string"/></returns>
        public string GetItemText(string ID)
        {
            YorotLangItem item = LangItems.Find(i => i.ID.Trim() == ID.Trim());
            if (item == null)
            {
                Output.WriteLine("[Language] Missing Item [ID=\"" + ID + "\" LangFile=\"" + LoadedLangFile + "\"]",LogLevel.Warning);
                return "[MI] " + ID;
            }
            else
            {
                string itemText = item.Text;
                for (int i = 0; i < LangVars.Count; i++)
                {
                    YorotLangVar globalVar = LangVars[i];
                    itemText = RuleifyString(itemText, globalVar);
                }
                return itemText;
            }
        }
        /// <summary>
        /// Determines if group #YOROT-ROOT is loaded.
        /// </summary>
        public bool LoadedRoot { get; set; }
        /// <summary>
        /// Loads language from file.
        /// </summary>
        /// <param name="fileLoc">Location of language file.</param>
        /// <param name="ignoreVersionError"><see cref="true"/> to ignore version checking.</param>
        public void LoadFromFile(string fileLoc)
        {
            LangVars.Clear();
            AddDefaultVars();
            LoadedRoot = false;
            if (!string.IsNullOrWhiteSpace(fileLoc))
            {
                if (!System.IO.File.Exists(fileLoc)) 
                {
                    try
                    {
                        XmlDocument document = new XmlDocument();
                        document.LoadXml(HTAlt.Tools.ReadFile(fileLoc, System.Text.Encoding.Unicode));
                        XmlNode rootNode = Yorot.Tools.FindRoot(document.DocumentElement);
                        RecursiveAdd(rootNode);
                    }
                    catch (XmlException)
                    {
                        Output.WriteLine("[LangManager] Cannot load language, config file contains XML error(s).", LogLevel.Warning);
                    }
                    catch (Exception ex)
                    {
                        Output.WriteLine("[LangManager] Error while loading language configuration: " + ex.ToString(), LogLevel.Warning);
                    }
                }
                else
                {
                    Output.WriteLine("[LangManager] Cannot load language, config file does not exists.", LogLevel.Warning);
                }
            }
            else
            {
                Output.WriteLine("[LangManager] Cannot load language, config file string was empty.", LogLevel.Warning);
            }
        }

        /// <summary>
        /// Recursively adds items to system.
        /// </summary>
        /// <param name="rootNode">Root Node</param>
        private void RecursiveAdd(XmlNode rootNode)
        {
            for(int i =0;i < rootNode.ChildNodes.Count;i++)
            {
                var node = rootNode.ChildNodes[i];
                switch (node.Name)
                {
                    case "Var":
                        {
                            if (node.Attributes["ID"] != null && node.Attributes["Text"] != null)
                            {
                                if (LangVars.FindAll(it => it.Name == node.Attributes["ID"].Value && it.Text == node.Attributes["Text"].Value).Count > 0) 
                                {
                                    Output.WriteLine("[LangManager] Threw away \"" + node.OuterXml + "\", Language Variable already exists.", LogLevel.Warning);
                                }
                                else
                                {
                                    LangVars.Add(new YorotLangVar(node.Attributes["ID"].Value.InnerXmlToString(), node.Attributes["Text"].Value.InnerXmlToString()));
                                }
                            }
                            else
                            {
                                Output.WriteLine("[LangManager] Threw away \"" + node.OuterXml + "\", invalid format for Yorot Language Variable.", LogLevel.Warning);
                            }
                            break;
                        }
                    case "Translate":
                        {
                            string id = node.Attributes["ID"] != null ? node.Attributes["ID"].Value.InnerXmlToString() : HTAlt.Tools.GenerateRandomText(12);
                            string text = node.Attributes["Text"] != null ? node.Attributes["Text"].Value.InnerXmlToString() : id;
                            if (!string.IsNullOrWhiteSpace(id) && !string.IsNullOrWhiteSpace(text))
                            {
                                if (LangItems.FindAll(it => it.ID == id && it.Text == text).Count > 0) 
                                {
                                    Output.WriteLine("[LangManager] Threw away \"" + node.OuterXml + "\", Language Item already exists.", LogLevel.Warning);
                                }
                                else
                                {
                                    LangItems.Add(new YorotLangItem() { ID = id, Text = text });
                                }
                            }
                            else
                            {
                                Output.WriteLine("[LangManager] Threw away \"" + node.OuterXml + "\", invalid format for Yorot Language Item.", LogLevel.Warning);
                            }
                            break;
                        }
                    case "Group":
                        {
                            if (node.Attributes["Name"] != null)
                            {
                                if (node.Attributes["Name"].Value == "#YOROT-ROOT")
                                {
                                    if (!LoadedRoot)
                                    {
                                        LoadedRoot = true;
                                        for (int ı = 0; ı < node.ChildNodes.Count; ı++)
                                        {
                                            var subnode = node.ChildNodes[ı];
                                            List<string> appliedSettings = new List<string>();
                                            switch (subnode.Name)
                                            {
                                                case "Name":
                                                    if (appliedSettings.FindAll(it => it == subnode.Name).Count > 0)
                                                    {
                                                        Output.WriteLine("[LangManager] Threw away \"" + node.OuterXml + "\", #YOROT-ROOT configuration already loaded.", LogLevel.Warning);
                                                        break;
                                                    }
                                                    appliedSettings.Add(subnode.Name);
                                                    LoadedLangName = subnode.InnerXml.InnerXmlToString();
                                                    break;
                                                case "Author":
                                                    if (appliedSettings.FindAll(it => it == subnode.Name).Count > 0)
                                                    {
                                                        Output.WriteLine("[LangManager] Threw away \"" + node.OuterXml + "\", #YOROT-ROOT configuration already loaded.", LogLevel.Warning);
                                                        break;
                                                    }
                                                    appliedSettings.Add(subnode.Name);
                                                    LoadedLangAuthor = subnode.InnerXml.InnerXmlToString();
                                                    break;
                                                case "CompatibleVersion":
                                                    if (appliedSettings.FindAll(it => it == subnode.Name).Count > 0)
                                                    {
                                                        Output.WriteLine("[LangManager] Threw away \"" + node.OuterXml + "\", #YOROT-ROOT configuration already loaded.", LogLevel.Warning);
                                                        break;
                                                    }
                                                    appliedSettings.Add(subnode.Name);
                                                    LoadedLangCompatibleVer = int.Parse(subnode.InnerXml.InnerXmlToString());
                                                    break;
                                                case "Version":
                                                    if (appliedSettings.FindAll(it => it == subnode.Name).Count > 0)
                                                    {
                                                        Output.WriteLine("[LangManager] Threw away \"" + node.OuterXml + "\", #YOROT-ROOT configuration already loaded.", LogLevel.Warning);
                                                        break;
                                                    }
                                                    appliedSettings.Add(subnode.Name);
                                                    LoadedLangVersion = int.Parse(subnode.InnerXml.InnerXmlToString());
                                                    break;
                                                default:
                                                    if (!subnode.OuterXml.StartsWith("<!--"))
                                                    {
                                                        Output.WriteLine("[LangManager] Threw away \"" + node.OuterXml + "\", invalid format for #YOROT-ROOT.", LogLevel.Warning);
                                                    }
                                                    break;
                                            }
                                        }
                                    }else
                                    {
                                        Output.WriteLine("[LangManager] Threw away \"" + node.OuterXml + "\", #YOROT-ROOT already loaded.", LogLevel.Warning);
                                    }
                                }
                            }
                            RecursiveAdd(node);
                            break;
                        }
                    default:
                        Output.WriteLine("[LangManager] Threw away \"" + node.OuterXml + "\", unsupported.", LogLevel.Warning);
                        break;
                }
            }
        }

        /// <summary>
        /// Used for dynamically inputting <paramref name="langVar"/> to <seealso cref="YorotLangItem"/>.
        /// </summary>
        /// <param name="main"><see cref="YorotLangItem"/></param>
        /// <param name="langVar"><see cref="YorotLangVar"/></param>
        /// <returns><see cref="string"/></returns>
        private static string RuleifyString(string main, YorotLangVar langVar)
        {
            string ignored = "§IGNORED_" + HTAlt.Tools.GenerateRandomText(17) + "§";
            return main
                .Replace("![" + langVar.Name.ToUpper() + "]", ignored)
                .Replace("[" + langVar.Name.ToUpper() + "]", string.IsNullOrEmpty(langVar.Text) ? "" : langVar.Text)
                .Replace(ignored, "[" + langVar.Name.ToUpper() + "]");
        }
    }
    /// <summary>
    /// Item used in translation of Yorot.
    /// </summary>
    public class YorotLangItem
    {
        /// <summary>
        /// Name/CodeName of item.
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// Text to display.
        /// </summary>
        public string Text { get; set; }
    }

    /// <summary>
    /// Variable used in <see cref="YorotLangItem"/>.
    /// </summary>
    public class YorotLangVar
    {
        /// <summary>
        /// Creates a new <see cref="YorotLangVar"/>.
        /// </summary>
        /// <param name="name">Name of <see cref="YorotLangVar"/>.</param>
        /// <param name="defaultVal">Default value of <see cref="YorotLangVar"/>.</param>
        public YorotLangVar(string name, string defaultVal = "")
        {
            Text = defaultVal;
            Name = name;
        }
        /// <summary>
        /// nbame of <see cref="LanguageGlobalVar"/>.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Text or value of <see cref="LanguageGlobalVar"/>.
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Condition of <see cref="LanguageGlobalVar"/>. Used by <see cref="Condition"/>.
        /// </summary>
        public string ConditionString { get; set; }
    }
}
