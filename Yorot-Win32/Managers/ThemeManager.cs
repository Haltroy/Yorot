using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HTAlt;

namespace Yorot
{
    public class ThemeManager
    {
        public ThemeManager(string configFile)
        {
            // TODO: Read Config Fİle
            Themes.Add(DefaultThemes.YorotLight.CarbonCopy());
            Themes.Add(DefaultThemes.YorotDeepBlue.CarbonCopy());
            Themes.Add(DefaultThemes.YorotStone.CarbonCopy());
            Themes.Add(DefaultThemes.YorotShadow.CarbonCopy());
            Themes.Add(DefaultThemes.YorotRazor.CarbonCopy());
            Themes.Add(DefaultThemes.YorotDark.CarbonCopy());
        }
        public YorotTheme AppliedTheme { get; set; }
        public List<YorotTheme> Themes { get; set; } = new List<YorotTheme>();
    }

    public static class DefaultThemes
    {
        public static YorotTheme YorotLight => new YorotTheme()
        {
            Name = "Yorot Light",
            Author = "Haltroy",
            CodeName = "com.haltroy.yorotlight",
            isDefaultTheme = true,
            Version = 1,
            ThumbLoc = @"§RES\YorotLight.png",
            BackColor = Color.FromArgb(255, 255, 255, 255),
            ForeColor = Color.FromArgb(255, 0, 0, 0),
            OverlayColor = Color.FromArgb(255, 64, 128, 255),
            ArtColor = Color.FromArgb(255, 235,235,235),
        };
        public static YorotTheme YorotStone => new YorotTheme()
        {
            Name = "Yorot Stone",
            Author = "Haltroy",
            CodeName = "com.haltroy.yorotstone",
            isDefaultTheme = true,
            Version = 1,
            ThumbLoc = @"§RES\YorotStone.png",
            BackColor = Color.FromArgb(255, 155,155,155),
            ForeColor = Color.FromArgb(255, 0, 0, 0),
            OverlayColor = Color.FromArgb(255, 64, 128, 255),
            ArtColor = Color.FromArgb(255, 0,0,255),
        };
        public static YorotTheme YorotRazor => new YorotTheme()
        {
            Name = "Yorot Razor",
            Author = "Haltroy",
            CodeName = "com.haltroy.yorotrazor",
            isDefaultTheme = true,
            Version = 1,
            ThumbLoc = @"§RES\YorotRazor.png",
            BackColor = Color.FromArgb(255,255,255,255),
            ForeColor = Color.FromArgb(255,0,0,0),
            OverlayColor = Color.FromArgb(255, 64, 32, 64),
            ArtColor = Color.FromArgb(255, 64, 32, 16),
        };
        public static YorotTheme YorotDark => new YorotTheme()
        {
            Name = "Yorot Dark",
            Author = "Haltroy",
            CodeName = "com.haltroy.yorotdark",
            isDefaultTheme = true,
            Version = 1,
            ThumbLoc = @"§RES\YorotDark.png",
            BackColor = Color.FromArgb(255, 0,0,0),
            ForeColor = Color.FromArgb(255, 195,195,195),
            OverlayColor = Color.FromArgb(255, 64, 128, 255),
            ArtColor = Color.FromArgb(255, 64,64,64),
        };
        public static YorotTheme YorotShadow => new YorotTheme()
        {
            Name = "Yorot Shadow",
            Author = "Haltroy",
            CodeName = "com.haltroy.yorotshadow",
            isDefaultTheme = true,
            Version = 1,
            ThumbLoc = @"§RES\YorotShadow.png",
            BackColor = Color.FromArgb(255, 23,32,32),
            ForeColor = Color.FromArgb(255, 195, 195, 195),
            OverlayColor = Color.FromArgb(255, 64, 128, 255),
            ArtColor = Color.FromArgb(255, 64, 64, 64),
        };
        public static YorotTheme YorotDeepBlue => new YorotTheme()
        {
            Name = "Yorot Deep Blue",
            Author = "Haltroy",
            CodeName = "com.haltroy.yorotdeepblue",
            isDefaultTheme = true,
            Version = 1,
            ThumbLoc = @"§RES\YorotDeepBlue.png",
            BackColor = Color.FromArgb(255, 8, 0, 64),
            ForeColor = Color.FromArgb(255, 0, 255, 196),
            OverlayColor = Color.FromArgb(255, 64, 128, 255),
            ArtColor = Color.FromArgb(255, 16, 8, 82),
        };
    }

    public class YorotTheme
    {
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
        public string Name { get; set; }
        public string Author { get; set; }
        public string CodeName { get; set; }
        public string HTUPDATE { get; set; }
        public int Version { get; set; }
        public string ThumbLoc { get; set; }
        public Image Thumbnail
        {
            get
            {
                // TODO
                return HTAlt.Tools.ReadFile(YorotGlobal.Settings.ThemesLoc + CodeName + @"\" + ThumbLoc, ImageFormat.Png);
            }
        }

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
