using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Yorot
{
    public class LangManager
    {
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
        public Settings Settings { get; set; }
        public string LoadedLangFile { get; set; }
        public List<YorotLangVar> LangVars { get; set; } = new List<YorotLangVar>();
        public List<YorotLangItem> LangItems { get; set; } = new List<YorotLangItem>();

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
        public void LoadFromFile(string fileLoc)
        {
            LangVars.Clear();
            AddDefaultVars();
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
                                LangVars.Add(new YorotLangVar(node.Attributes["ID"].Value, node.Attributes["Text"].Value));
                            }
                            else
                            {
                                Output.WriteLine("[LangManager] Threw away \"" + node.OuterXml + "\", Invalid format for YorotLangVar.", LogLevel.Warning);
                            }
                            break;
                        }
                    case "Translate":
                        {
                            string id = node.Attributes["ID"] != null ? node.Attributes["ID"].Value.Replace("&amp;", "&").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&apos;", "'").Replace("&quot;", "\"") : HTAlt.Tools.GenerateRandomText(12);
                            string text = node.Attributes["Text"] != null ? node.Attributes["Text"].Value.Replace("&amp;", "&").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&apos;", "'").Replace("&quot;", "\"") : id;
                            if (!string.IsNullOrWhiteSpace(id) && !string.IsNullOrWhiteSpace(text))
                            {
                                LangItems.Add(new YorotLangItem() { ID = id, Text = text });
                            }
                            else
                            {
                                Output.WriteLine("[LangManager] Threw away \"" + node.OuterXml + "\", Invalid format for YorotLangItem.", LogLevel.Warning);
                            }
                            break;
                        }
                    case "Group":
                        {
                            RecursiveAdd(node);
                            break;
                        }
                    default:
                        Output.WriteLine("[LangManager] Threw away \"" + node.OuterXml + "\", Unsupported.", LogLevel.Warning);
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

    public class YorotLangItem
    {
        public string ID { get; set; }
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
