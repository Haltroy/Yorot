using HTAlt;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Yorot
{
    /// <summary>
    /// <see cref="ExpPack"/> Manager.
    /// </summary>
    public class EPManager : YorotManager
    {
        public EPManager(YorotMain main) : base(main.EPMConfig, main)
        {
            ExpPacks.Add(DefaultExpPacks.Haltroy(this));
        }

        public override void ExtractXml(XmlNode rootNode)
        {
            for (int i = 0; i < rootNode.ChildNodes.Count; i++)
            {
                XmlNode node = rootNode.ChildNodes[i];
                switch (node.Name.ToLowerEnglish())
                {
                    case "exppack":
                        if (node.Attributes["ID"] != null)
                        {
                            ExpPacks.Add(new ExpPack(Main.EPFolder + node.Attributes["ID"].Value.XmlToString() + ".yep", this));
                        }
                        else
                        {
                            Output.WriteLine("[ExpPackManager] Threw away \"" + node.OuterXml + "\", missing attribute.");
                        }
                        break;

                    default:
                        if (!node.IsComment())
                        {
                            Output.WriteLine("[ExpPackManager] Threw away \"" + node.OuterXml + "\", unsupported.");
                        }
                        break;
                }
            }
        }

        public override string ToXml()
        {
            string x = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" + Environment.NewLine +
                       "<root>" + Environment.NewLine +
                       "<!-- Yorot ExpPacks Manager Config File" + Environment.NewLine + Environment.NewLine +
                       "This file is used to save experience packs." + Environment.NewLine +
                       "Editing this file might cause problems with experience packs." + Environment.NewLine +
                       "-->" + Environment.NewLine;
            for (int i = 0; i < ExpPacks.Count; i++)
            {
                ExpPack exp = ExpPacks[i];
                x += "<ExpPack ID=\"" + exp.CodeName.ToXML() + "\" />" + Environment.NewLine;
            }
            return (x + "</root>").BeautifyXML();
        }

        public List<ExpPack> ExpPacks { get; set; } = new List<ExpPack>();
    }

    public class DefaultExpPacks
    {
        public static ExpPack Haltroy(EPManager manager)
        {
            ExpPack pack = new ExpPack(manager) { Author = "Haltroy", Name = "Haltroy Experience Pack", CodeName = "com.haltroy.exppack" };
            pack.SearchBar.Add(new ExpPack.SearchExp(@"^haltroy\:\/\/", "https://haltroy.com/go.php?u=½U½", new int[] { -2 }));
            return pack;
        }
    }

    /// <summary>
    /// Yorot Experience Pack class.
    /// </summary>
    public class ExpPack
    {
        public ExpPack(EPManager manager)
        {
            if (manager is null) { throw new ArgumentNullException(nameof(manager)); }
            Manager = manager;
        }

        public ExpPack(string fileLoc, EPManager manager) : this(manager)
        {
            if (string.IsNullOrWhiteSpace(fileLoc)) { throw new ArgumentNullException(nameof(fileLoc)); }
            if (!System.IO.File.Exists(fileLoc)) { throw new System.IO.FileNotFoundException("File not found.", fileLoc); }
            try
            {
                string xml = HTAlt.Tools.ReadFile(fileLoc, Encoding.Unicode);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlNode rootNode = HTAlt.Tools.FindRoot(doc);
                List<string> applied = new List<string>();
                for (int i = 0; i < rootNode.ChildNodes.Count; i++)
                {
                    XmlNode node = rootNode.ChildNodes[i];
                    string s = node.Name.ToLowerEnglish();
                    if (applied.Contains(s))
                    {
                        Output.WriteLine("[ExpPack] Threw away \"" + node.OuterXml + "\", config already applied.", LogLevel.Warning);
                    }
                    applied.Add(s);
                    switch (s)
                    {
                        case "name":
                            Name = node.InnerXml.XmlToString();
                            break;

                        case "codename":
                            CodeName = node.InnerXml.XmlToString();
                            break;

                        case "author":
                            Author = node.InnerXml.XmlToString();
                            break;

                        case "version":
                            Version = int.Parse(node.InnerXml.XmlToString());
                            break;

                        case "htupdate":
                            HTU_Url = node.InnerXml.XmlToString();
                            Manager.Main.Yopad.RegisterHTU(HTU_Url, this);
                            break;

                        case "searchbar":
                            for (int _i = 0; _i < node.ChildNodes.Count; _i++)
                            {
                                XmlNode subnode = node.ChildNodes[_i];
                                if (subnode.Name.ToLowerEnglish() == "exp")
                                {
                                    if (subnode.Attributes["Condition"] != null && subnode.Attributes["Redirect"] != null && subnode.Attributes["Groups"] != null)
                                    {
                                        SearchBar.Add(new SearchExp(subnode.Attributes["Condition"].Value.XmlToString(), subnode.Attributes["Redirect"].Value.XmlToString(), Array.ConvertAll(subnode.Attributes["Groups"].Value.XmlToString().Split(';'), str => int.Parse(str))));
                                    }
                                    else
                                    {
                                        Output.WriteLine("[ExpPack] Threw away \"" + node.OuterXml + "\", missing attribute.", LogLevel.Warning);
                                    }
                                }
                                else
                                {
                                    Output.WriteLine("[ExpPack] Threw away \"" + subnode.OuterXml + "\", unsupported.", LogLevel.Warning);
                                }
                            }
                            break;

                        default:
                            if (!node.IsComment()) { Output.WriteLine("[ExpPack] Threw away \"" + node.OuterXml + "\", unsupported.", LogLevel.Warning); }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Output.WriteLine("[ExpPack] Exception cught while loading: " + ex.ToString(), LogLevel.Error);
            }
        }

        /// <summary>
        /// Name of the Experience pack
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Code name of the Experience pack
        /// </summary>
        public string CodeName { get; set; }

        /// <summary>
        /// Author of the experience pack
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Version of the experience pack
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// HTUPDATE URL of this experience pack
        /// </summary>
        public string HTU_Url { get; set; }

        /// <summary>
        /// Manager of this experience pack.
        /// </summary>
        public EPManager Manager { get; set; }

        /// <summary>
        /// Search Bar experiences
        /// </summary>
        public List<SearchExp> SearchBar { get; set; } = new List<SearchExp>();

        #region Experiences

        /// <summary>
        /// Search Experience
        /// </summary>
        public class SearchExp
        {
            /// <summary>
            /// Creates a new Search Experience
            /// </summary>
            /// <param name="condition">Condition</param>
            /// <param name="redirect">Redirect URL</param>
            /// <param name="redirGroups">Redirect Regex Groups</param>
            public SearchExp(string condition, string redirect, int[] redirGroups)
            {
                Condition = condition;
                Redirect = redirect;
                RedirectGroups = redirGroups;
            }

            /// <summary>
            /// The Regex condition of this experience.
            /// </summary>
            public string Condition { get; set; }

            /// <summary>
            /// The URl that will be redirected to when condition is clear.
            /// </summary>
            public string Redirect { get; set; }

            /// <summary>
            /// A list of group IDs that are detected with Regex and going to be inserted to <see cref="Redirect"/>. Yorot always prefers the first entry.
            /// <para></para>
            /// -3 => Insert all text.
            /// <para></para>
            /// -2 => Insert everything except the detected text.
            /// <para></para>
            /// -1 => Insert the detected text.
            /// <para></para>
            /// Every number equal or bigger than 0 => Insert the group with the same ID number.
            /// </summary>
            public int[] RedirectGroups { get; set; }

            /// <summary>
            /// Detects if the condition is clear.
            /// </summary>
            /// <param name="address">URL</param>
            /// <returns><see cref="bool"/></returns>
            public bool ConditionOK(string address)
            {
                return new Regex(Condition, RegexOptions.Compiled | RegexOptions.Singleline).IsMatch(address);
            }

            /// <summary>
            /// Does the redirect.
            /// </summary>
            /// <param name="address">URL</param>
            /// <returns><see cref="string"/></returns>
            public string DoRedirect(string address)
            {
                string x = string.Empty;
                Regex reg = new Regex(Condition, RegexOptions.Compiled | RegexOptions.Singleline);
                Match m = reg.Match(address);
                if (!m.Success) { return string.Empty; }
                for (int i = 0; i < RedirectGroups.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(x))
                    {
                        if (m.Groups.Count >= i || i > -4)
                        {
                            switch (i)
                            {
                                case -3:
                                    x = address;
                                    break;

                                case -2:
                                    x = address.Replace(m.Value, "");
                                    break;

                                case -1:
                                    x = m.Value;
                                    break;

                                default:
                                    if (!string.IsNullOrWhiteSpace(m.Groups[i].Value))
                                    {
                                        x = m.Groups[i].Value;
                                    }
                                    break;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                return Redirect.Replace("%U%", x);
            }
        }

        #endregion Experiences
    }
}