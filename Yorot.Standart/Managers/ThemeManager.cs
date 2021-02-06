using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using HTAlt;

namespace Yorot
{
    // TODO: Add XML descriptions to every property.
    public class ThemeManager
    {
        public ThemeManager(string configFile)
        {
            Themes.Add(DefaultThemes.YorotLight.CarbonCopy());
            Themes.Add(DefaultThemes.YorotDeepBlue.CarbonCopy());
            Themes.Add(DefaultThemes.YorotStone.CarbonCopy());
            Themes.Add(DefaultThemes.YorotShadow.CarbonCopy());
            Themes.Add(DefaultThemes.YorotRazor.CarbonCopy());
            Themes.Add(DefaultThemes.YorotDark.CarbonCopy());
            if (string.IsNullOrWhiteSpace(configFile))
            {
                Output.WriteLine("[ThemeMan] Loaded defaults because config file location was empty.", LogLevel.Warning);
            }else
            {
                if(!File.Exists(configFile))
                {
                    Output.WriteLine("[ThemeMan] Loaded defaults beacuse config file does not exists.", LogLevel.Warning);
                }
                else
                {
                    try
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(HTAlt.Tools.ReadFile(configFile, Encoding.Unicode));
                        XmlNode rootnode = Yorot.Tools.FindRoot(doc);
                        List<string> acceptedSetting = new List<string>();
                        bool ayaj = false;
                        for (int i = 0; i < rootnode.ChildNodes.Count;i++)
                        {
                            var node = rootnode.ChildNodes[i];
                            switch(node.Name)
                            {
                                case "LoadedTheme":
                                    if (acceptedSetting.Contains(node.Name))
                                    {
                                        Output.WriteLine("[ThemeMan] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    acceptedSetting.Add(node.Name);
                                    if (node.InnerXml.InnerXmlToString().ToLower().StartsWith("com.haltroy."))
                                    {
                                        AppliedTheme = Themes.Find(t => t.CodeName == node.InnerXml.InnerXmlToString());
                                        ayaj = false;
                                    }
                                    else
                                    {
                                        if (Themes.FindAll(t => t.Config == node.InnerXml.InnerXmlToString().ShortenPath(Settings.AppPath)).Count > 0)
                                        {
                                            ayaj = false;
                                            AppliedTheme = Themes.Find(t => t.Config == node.InnerXml.InnerXmlToString().ShortenPath(Settings.AppPath));
                                        }
                                        else
                                        {
                                            AppliedTheme = new YorotTheme(node.InnerText.InnerXmlToString().ShortenPath(Settings.AppPath));
                                            ayaj = true;
                                        }
                                    }
                                    break;
                                case "Themes":
                                    if (acceptedSetting.Contains(node.Name))
                                    {
                                        Output.WriteLine("[ThemeMan] Threw away \"" + node.OuterXml + "\". Setting already applied.", LogLevel.Warning);
                                        break;
                                    }
                                    acceptedSetting.Add(node.Name);
                                    for(int ı = 0; ı < node.ChildNodes.Count;ı++)
                                    {
                                        var subnode = node.ChildNodes[ı];
                                        switch(subnode.Name)
                                        {
                                            case "Theme":
                                                if (AppliedTheme == null)
                                                {
                                                    ayaj = false;
                                                }
                                                if (ayaj) // Applied theme might be in 
                                                {
                                                    if (subnode.InnerXml.InnerXmlToString() == AppliedTheme.Config)
                                                    {
                                                        Themes.Add(AppliedTheme);
                                                    }else
                                                    {
                                                        Themes.Add(new YorotTheme(subnode.InnerXml.InnerXmlToString().ShortenPath(Settings.AppPath)));
                                                    }
                                                }else //Applied theme is already in, no need to worry about duplication.
                                                {
                                                    Themes.Add(new YorotTheme(subnode.InnerXml.InnerXmlToString().ShortenPath(Settings.AppPath)));
                                                }
                                                break;
                                            default:
                                                if (!subnode.OuterXml.StartsWith("<!--"))
                                                {
                                                    Output.WriteLine("[ThemeMan] Threw away \"" + subnode.OuterXml + "\". Invalid format.", LogLevel.Warning);
                                                }
                                                break;
                                        }
                                    }
                                    break;
                                default:
                                    if (!node.OuterXml.StartsWith("<!--")) 
                                    {
                                        Output.WriteLine("[ThemeMan] Threw away \"" + node.OuterXml + "\". Unsupported.", LogLevel.Warning);
                                    }
                                    break;
                            }
                        }
                    }
                    catch(XmlException)
                    {
                        Output.WriteLine("[ThemeMan] Loaded defaults beacuse config file is in invalid format or has XML errors.", LogLevel.Warning);
                    }
                    catch (Exception ex)
                    {
                        Output.WriteLine("[ThemeMan] Loaded defaults because of this exception:" + Environment.NewLine + ex.ToString(), LogLevel.Warning);
                    }
                }
            }
            if (AppliedTheme == null)
            {
                if (Themes.Count > 0)
                {
                    AppliedTheme = Themes[0];
                }else
                {
                    Themes.Add(DefaultThemes.YorotLight);
                    AppliedTheme = Themes[0];
                }
            }
            ClaimMan();
        }
        private void ClaimMan()
        {
            for(int i = 0;i < Themes.Count;i++)
            {
                Themes[i].Manager = this;
            }
        }
        public Settings Settings { get; set; }
        public YorotTheme AppliedTheme { get; set; }
        public List<YorotTheme> Themes { get; set; } = new List<YorotTheme>();

        public string ToXml()
        {
            string x = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" + Environment.NewLine + 
                "<root>" + Environment.NewLine +
                "<!-- Yorot Theme Config File" + Environment.NewLine + Environment.NewLine +
                "This file is used to configure themes." + Environment.NewLine +
                "Editing this file might cause problems with themes." + Environment.NewLine +
                "-->" + Environment.NewLine +
                "<LoadedTheme>" + (AppliedTheme.isDefaultTheme ? AppliedTheme.CodeName : AppliedTheme.Config.ToXML().ShortenPath(Settings.AppPath))+ "</LoadedTheme>" + Environment.NewLine +
                "<Themes>" + Environment.NewLine;
            for(int i = 0; i < Themes.Count;i++)
            {
                var theme = Themes[i];
                if (!theme.isDefaultTheme)
                {
                    x += "<Theme>" + theme.Config.ShortenPath(Settings.AppPath) + "</Theme>" + Environment.NewLine;
                }
            }
            return Yorot.Tools.PrintXML(x + "</Themes>" + Environment.NewLine + "</root>");
        }
        public void Save()
        {
            ToXml().WriteToFile(Settings.UserTheme,Encoding.Unicode);
        }
    }

    public static class DefaultThemes
    {
        public static YorotTheme YorotLight => new YorotTheme(null)
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
            ArtColor = Color.FromArgb(255, 235,235,235),
        };
        public static YorotTheme YorotStone => new YorotTheme(null)
        {
            Name = "Yorot Stone",
            Author = "Haltroy",
            CodeName = "com.haltroy.yorotstone",
            isDefaultTheme = true,
            Version = 1,
            ThumbLoc = @"YorotStone.png",
            BackColor = Color.FromArgb(255, 155,155,155),
            ForeColor = Color.FromArgb(255, 0, 0, 0),
            OverlayColor = Color.FromArgb(255, 64, 128, 255),
            ArtColor = Color.FromArgb(255, 0,0,255),
        };
        public static YorotTheme YorotRazor => new YorotTheme(null)
        {
            Name = "Yorot Razor",
            Author = "Haltroy",
            CodeName = "com.haltroy.yorotrazor",
            isDefaultTheme = true,
            Version = 1,
            ThumbLoc = @"YorotRazor.png",
            BackColor = Color.FromArgb(255,255,255,255),
            ForeColor = Color.FromArgb(255,0,0,0),
            OverlayColor = Color.FromArgb(255, 64, 32, 64),
            ArtColor = Color.FromArgb(255, 64, 32, 16),
        };
        public static YorotTheme YorotDark => new YorotTheme(null)
        {
            Name = "Yorot Dark",
            Author = "Haltroy",
            CodeName = "com.haltroy.yorotdark",
            isDefaultTheme = true,
            Version = 1,
            ThumbLoc = @"YorotDark.png",
            BackColor = Color.FromArgb(255, 0,0,0),
            ForeColor = Color.FromArgb(255, 195,195,195),
            OverlayColor = Color.FromArgb(255, 64, 128, 255),
            ArtColor = Color.FromArgb(255, 64,64,64),
        };
        public static YorotTheme YorotShadow => new YorotTheme(null)
        {
            Name = "Yorot Shadow",
            Author = "Haltroy",
            CodeName = "com.haltroy.yorotshadow",
            isDefaultTheme = true,
            Version = 1,
            ThumbLoc = @"YorotShadow.png",
            BackColor = Color.FromArgb(255, 23,32,32),
            ForeColor = Color.FromArgb(255, 195, 195, 195),
            OverlayColor = Color.FromArgb(255, 64, 128, 255),
            ArtColor = Color.FromArgb(255, 64, 64, 64),
        };
        public static YorotTheme YorotDeepBlue => new YorotTheme(null)
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
        };
    }

    public class YorotTheme
    {
        public YorotTheme(string fileLoc)
        {

        }
        public YorotTheme CarbonCopy()
        {
            return new YorotTheme(null)
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
        public ThemeManager Manager { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string CodeName { get; set; }
        public string HTUPDATE { get; set; }
        public int Version { get; set; }
        public string ThumbLoc { get; set; }

        public string Config { get; set; }
        public bool isDefaultTheme { get; set; } = false;
        public System.Drawing.Color BackColor { get; set; }
        public Color BackColor2 => BackColor.ShiftBrightness(20, false);
        public Color BackColor3 => BackColor.ShiftBrightness(40, false);
        public Color BackColor4 => BackColor.ShiftBrightness(60, false);

        public System.Drawing.Color ForeColor { get; set; }
        public System.Drawing.Color OverlayColor { get; set; }
        public System.Drawing.Color OverlayColor2 => OverlayColor.ShiftBrightness(20, false);
        public System.Drawing.Color OverlayColor3 => OverlayColor.ShiftBrightness(40, false);
        public System.Drawing.Color OverlayColor4 => OverlayColor.ShiftBrightness(60, false);
        public System.Drawing.Color ArtColor { get; set; }
        public Color ArtColor2 => ArtColor.ShiftBrightness(20, false);
        public Color ArtColor3 => ArtColor.ShiftBrightness(40, false);
        public Color ArtColor4 => ArtColor.ShiftBrightness(60, false);
    }
}
