using HTAlt;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;

namespace Yorot
{
    /// <summary>
    /// Yorot Theme Manager
    /// </summary>
    public class ThemeManager : YorotManager
    {
        public ThemeManager(YorotMain main) : base(main.ThemeConfig, main)
        {
            Themes.Add(DefaultThemes.YorotLight.CarbonCopy());
            Themes.Add(DefaultThemes.YorotDeepBlue.CarbonCopy());
            Themes.Add(DefaultThemes.YorotStone.CarbonCopy());
            Themes.Add(DefaultThemes.YorotShadow.CarbonCopy());
            Themes.Add(DefaultThemes.YorotRazor.CarbonCopy());
            Themes.Add(DefaultThemes.YorotDark.CarbonCopy());
            ClaimMan();
        }

        /// <summary>
        /// Claims management for every theme.
        /// </summary>
        private void ClaimMan()
        {
            for (int i = 0; i < Themes.Count; i++)
            {
                Themes[i].Manager = this;
            }
        }

        /// <summary>
        /// A list of loaded themes.
        /// </summary>
        public List<YorotTheme> Themes { get; set; } = new List<YorotTheme>();

        ///
        public override string ToXml()
        {
            string x = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" + Environment.NewLine +
                "<root>" + Environment.NewLine +
                "<!-- Yorot Theme Config File" + Environment.NewLine + Environment.NewLine +
                "This file is used to configure themes." + Environment.NewLine +
                "Editing this file might cause problems with themes." + Environment.NewLine +
                "-->" + Environment.NewLine +
                "<Themes>" + Environment.NewLine;
            for (int i = 0; i < Themes.Count; i++)
            {
                YorotTheme theme = Themes[i];
                if (!theme.isDefaultTheme)
                {
                    x += "<Theme>" + theme.Config.ShortenPath(Main) + "</Theme>" + Environment.NewLine;
                }
            }
            return (x + "</Themes>" + Environment.NewLine + "</root>").BeautifyXML();
        }

        public override void ExtractXml(XmlNode rootNode)
        {
            List<string> acceptedSetting = new List<string>();
            for (int i = 0; i < rootNode.ChildNodes.Count; i++)
            {
                XmlNode node = rootNode.ChildNodes[i];
                switch (node.Name)
                {
                    case "Themes":
                        if (acceptedSetting.Contains(node.Name))
                        {
                            Output.WriteLine("[ThemeMan] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                            break;
                        }
                        acceptedSetting.Add(node.Name);
                        for (int ı = 0; ı < node.ChildNodes.Count; ı++)
                        {
                            XmlNode subnode = node.ChildNodes[ı];
                            switch (subnode.Name)
                            {
                                case "Theme":
                                    Themes.Add(new YorotTheme(subnode.InnerXml.XmlToString().ShortenPath(Main)));
                                    break;

                                default:
                                    if (!subnode.IsComment())
                                    {
                                        Output.WriteLine("[ThemeMan] Threw away \"" + subnode.OuterXml + "\". unsupported.", LogLevel.Warning);
                                    }
                                    break;
                            }
                        }
                        break;

                    default:
                        if (!node.IsComment())
                        {
                            Output.WriteLine("[ThemeMan] Threw away \"" + node.OuterXml + "\", unsupported.", LogLevel.Warning);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Finds and returns a <see cref="bool"/> if a theme exists in the drive.
        /// </summary>
        /// <param name="value"><see cref="YorotTheme.CodeName"/> of theme.</param>
        /// <returns><see cref="bool"/></returns>
        public bool ThemeExists(string value)
        {
            return Themes.FindAll(i => i.CodeName == value).Count > 0;
        }

        /// <summary>
        /// Enables a theöe.
        /// </summary>
        /// <param name="value"><see cref="YorotTheme.CodeName"/> of theme.</param>
        public void Enable(string value)
        {
            List<YorotTheme> l = Themes.FindAll(i => i.CodeName == value);
            if (l.Count > 0)
            {
                l[0].Enabled = true;
            }
            else
            {
                throw new ArgumentException("Cannot find theme with codename \"" + value + "\".");
            }
        }

        /// <summary>
        /// Gets theme from code name.
        /// </summary>
        /// <param name="codeName">Code name of the theme.</param>
        /// <returns><see cref="YorotTheme"/></returns>
        public YorotTheme GetThemeByCN(string codeName)
        {
            List<YorotTheme> l = Themes.FindAll(i => i.CodeName == codeName);
            if (l.Count > 0)
            {
                return l[0];
            }
            else
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Default themes that comes with Yorot and all other flavors.
    /// </summary>
    public static class DefaultThemes
    {
        /// <summary>
        /// Light theme
        /// </summary>
        public static YorotTheme YorotLight => new YorotTheme()
        {
            Name = "Yorot Light",
            Author = "Haltroy",
            CodeName = "com.haltroy.yorotlight",
            isDefaultTheme = true,
            Version = 1,
            ThumbLoc = @"YorotLight.png",
            BackColor = Color.FromArgb(255, 255, 255, 255),
            ForeColor = Color.FromArgb(255, 0, 0, 0),
            OverlayColor = Color.FromArgb(255, 64, 128, 255),
            ArtColor = Color.FromArgb(255, 235, 235, 235),
            Enabled = true,
        };

        /// <summary>
        /// Light gray theme, looks like it's made in early 90's
        /// </summary>
        public static YorotTheme YorotStone => new YorotTheme()
        {
            Name = "Yorot Stone",
            Author = "Haltroy",
            CodeName = "com.haltroy.yorotstone",
            isDefaultTheme = true,
            Version = 1,
            ThumbLoc = @"YorotStone.png",
            BackColor = Color.FromArgb(255, 155, 155, 155),
            ForeColor = Color.FromArgb(255, 0, 0, 0),
            OverlayColor = Color.FromArgb(255, 64, 128, 255),
            ArtColor = Color.FromArgb(255, 0, 0, 255),
            Enabled = true,
        };

        /// <summary>
        /// Razor theme
        /// </summary>
        public static YorotTheme YorotRazor => new YorotTheme()
        {
            Name = "Yorot Razor",
            Author = "Haltroy",
            CodeName = "com.haltroy.yorotrazor",
            isDefaultTheme = true,
            Version = 1,
            ThumbLoc = @"YorotRazor.png",
            BackColor = Color.FromArgb(255, 255, 255, 255),
            ForeColor = Color.FromArgb(255, 0, 0, 0),
            OverlayColor = Color.FromArgb(255, 64, 32, 64),
            ArtColor = Color.FromArgb(255, 64, 32, 16),
            Enabled = true,
        };

        /// <summary>
        /// The "Dark mode"
        /// </summary>
        public static YorotTheme YorotDark => new YorotTheme()
        {
            Name = "Yorot Dark",
            Author = "Haltroy",
            CodeName = "com.haltroy.yorotdark",
            isDefaultTheme = true,
            Version = 1,
            ThumbLoc = @"YorotDark.png",
            BackColor = Color.FromArgb(255, 0, 0, 0),
            ForeColor = Color.FromArgb(255, 195, 195, 195),
            OverlayColor = Color.FromArgb(255, 64, 128, 255),
            ArtColor = Color.FromArgb(255, 64, 64, 64),
            Enabled = true,
        };

        /// <summary>
        /// A little brighter than <see cref="YorotDark"/>
        /// </summary>
        public static YorotTheme YorotShadow => new YorotTheme()
        {
            Name = "Yorot Shadow",
            Author = "Haltroy",
            CodeName = "com.haltroy.yorotshadow",
            isDefaultTheme = true,
            Version = 1,
            ThumbLoc = @"YorotShadow.png",
            BackColor = Color.FromArgb(255, 23, 32, 32),
            ForeColor = Color.FromArgb(255, 195, 195, 195),
            OverlayColor = Color.FromArgb(255, 64, 128, 255),
            ArtColor = Color.FromArgb(255, 64, 64, 64),
            Enabled = true,
        };

        /// <summary>
        /// Theme used in Haltroy's website
        /// </summary>
        public static YorotTheme YorotDeepBlue => new YorotTheme()
        {
            Name = "Yorot Deep Blue",
            Author = "Haltroy",
            CodeName = "com.haltroy.yorotdeepblue",
            isDefaultTheme = true,
            Version = 1,
            ThumbLoc = @"YorotDeepBlue.png",
            BackColor = Color.FromArgb(255, 8, 0, 64),
            ForeColor = Color.FromArgb(255, 0, 255, 196),
            OverlayColor = Color.FromArgb(255, 64, 128, 255),
            ArtColor = Color.FromArgb(255, 16, 8, 82),
            Enabled = true,
        };
    }

    /// <summary>
    /// Yorot Theme
    /// </summary>
    public class YorotTheme
    {
        /// <summary>
        /// Creates a new Yorot theme. This constructor does not initializes the theme.
        /// </summary>
        public YorotTheme() { }

        /// <summary>
        /// Creates a new Yorot theme and initializes it.
        /// </summary>
        /// <param name="fileLoc">Location of the theme file on disk.</param>
        public YorotTheme(string fileLoc)
        {
            if (!string.IsNullOrWhiteSpace(fileLoc))
            {
                if (File.Exists(fileLoc))
                {
                    Config = fileLoc;
                    string xml = HTAlt.Tools.ReadFile(fileLoc, Encoding.Unicode);
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);
                    XmlNode rootNode = HTAlt.Tools.FindRoot(doc);
                    List<string> applied = new List<string>();
                    for (int i = 0; i < rootNode.ChildNodes.Count; i++)
                    {
                        XmlNode node = rootNode.ChildNodes[i];
                        string nodeName = node.Name.ToLowerEnglish();
                        if (applied.Contains(nodeName))
                        {
                            Output.WriteLine("[Theme:\"" + Config + "\"] Threw away \"" + node.OuterXml + "\", configuration already applied.", LogLevel.Warning);
                            break;
                        }
                        applied.Add(nodeName);
                        switch (nodeName)
                        {
                            case "name":
                                Name = node.InnerXml.XmlToString();
                                break;

                            case "author":
                                Author = node.InnerXml.XmlToString();
                                break;

                            case "codename":
                                CodeName = node.InnerXml.XmlToString();
                                break;

                            case "htupdate":
                                HTUPDATE = node.InnerXml.XmlToString();
                                Manager.Main.Yopad.RegisterHTU(HTUPDATE, this);
                                break;

                            case "version":
                                Version = int.Parse(node.InnerXml.XmlToString());
                                break;

                            case "thumbnail":
                                ThumbLoc = node.InnerXml.XmlToString();
                                break;

                            case "backcolor":
                                BackColor = node.InnerXml.XmlToString().HexToColor();
                                break;

                            case "forecolor":
                                ForeColor = node.InnerXml.XmlToString().ToLowerEnglish() == "auto" ? HTAlt.Tools.AutoWhiteBlack(BackColor) : node.InnerXml.XmlToString().HexToColor();
                                break;

                            case "overlaycolor":
                                OverlayColor = node.InnerXml.XmlToString().HexToColor();
                                break;

                            case "artcolor":
                                ArtColor = node.InnerXml.XmlToString().HexToColor();
                                break;

                            default:
                                if (!node.IsComment())
                                {
                                    Output.WriteLine("[Theme:\"" + Config + "\"] Threw away \"" + node.OuterXml + "\", unsupported.", LogLevel.Warning);
                                }
                                break;
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("File \"" + fileLoc + "\" on argument \"fileLoc\" does not exists.");
                }
            }
            else
            {
                throw new ArgumentNullException("fileLoc");
            }
        }

        /// <summary>
        /// Creates a carbon-copy of this theme.
        /// </summary>
        /// <returns><see cref="YorotTheme"/></returns>
        public YorotTheme CarbonCopy()
        {
            return new YorotTheme()
            {
                Name = Name,
                Author = Author,
                CodeName = CodeName,
                HTUPDATE = HTUPDATE,
                Version = Version,
                ThumbLoc = ThumbLoc,
                isDefaultTheme = isDefaultTheme,
                BackColor = BackColor,
                ForeColor = ForeColor,
                OverlayColor = OverlayColor,
                ArtColor = ArtColor
            };
        }

        /// <summary>
        /// Manager associated with this theme
        /// </summary>
        public ThemeManager Manager { get; set; }

        /// <summary>
        /// Name of the theme.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Author of the theme.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// CodeName of the theme.
        /// </summary>
        public string CodeName { get; set; }

        /// <summary>
        /// HTUPDATE of the theme.
        /// </summary>
        public string HTUPDATE { get; set; }

        /// <summary>
        /// Version of the theme.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Thumbnail location of the theme.
        /// </summary>
        public string ThumbLoc { get; set; }

        /// <summary>
        /// Location of the theme file on disk.
        /// </summary>
        public string Config { get; set; }

        /// <summary>
        /// Determines if this theme comes pre-installed with Yorot.
        /// </summary>
        public bool isDefaultTheme { get; set; } = false;

        /// <summary>
        /// Background Color
        /// </summary>
        public System.Drawing.Color BackColor { get; set; }

        /// <summary>
        /// A little brighter/darker <see cref="BackColor"/>.
        /// </summary>
        public Color BackColor2 => BackColor.ShiftBrightness(20, false);

        /// <summary>
        /// Brighter/darker <see cref="BackColor"/>.
        /// </summary>
        public Color BackColor3 => BackColor.ShiftBrightness(40, false);

        /// <summary>
        /// More brighter/darker <see cref="BackColor"/>.
        /// </summary>
        public Color BackColor4 => BackColor.ShiftBrightness(60, false);

        /// <summary>
        /// The foreground color, determines the text and button images colors.
        /// </summary>
        public System.Drawing.Color ForeColor { get; set; }

        /// <summary>
        /// The overlay color, determines the edges etc.
        /// </summary>
        public System.Drawing.Color OverlayColor { get; set; }

        /// <summary>
        /// A little brighter/darker <see cref="OverlayColor"/>.
        /// </summary>
        public System.Drawing.Color OverlayColor2 => OverlayColor.ShiftBrightness(20, false);

        /// <summary>
        /// Brighter/darker <see cref="OverlayColor"/>.
        /// </summary>
        public System.Drawing.Color OverlayColor3 => OverlayColor.ShiftBrightness(40, false);

        /// <summary>
        /// More brighter/darker <see cref="OverlayColor"/>.
        /// </summary>
        public System.Drawing.Color OverlayColor4 => OverlayColor.ShiftBrightness(60, false);

        /// <summary>
        /// Artiliary color, similar to <see cref="OverlayColor"/>
        /// </summary>
        public System.Drawing.Color ArtColor { get; set; }

        /// <summary>
        /// A little brighter/darker <see cref="ArtColor"/>.
        /// </summary>
        public Color ArtColor2 => ArtColor.ShiftBrightness(20, false);

        /// <summary>
        /// Brighter/darker <see cref="ArtColor"/>.
        /// </summary>
        public Color ArtColor3 => ArtColor.ShiftBrightness(40, false);

        /// <summary>
        /// More brighter/darker <see cref="ArtColor"/>.
        /// </summary>
        public Color ArtColor4 => ArtColor.ShiftBrightness(60, false);

        /// <summary>
        /// Determines if this theme is enabled or disabled by user or the system.
        /// </summary>
        public bool Enabled { get; set; } = false;
    }
}