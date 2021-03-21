using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yorot
{
    /// <summary>
    /// Public tools that are used by Yorot.
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// Generates <see cref="Image"/> from <paramref name="baseIcon"/>.
        /// </summary>
        /// <param name="baseIcon"></param>
        /// <returns></returns>
        public static System.Drawing.Image GenerateAppIcon(System.Drawing.Image baseIcon, System.Drawing.Color? BackColor = null, int squareSize = 64)
        {
            if (BackColor == null)
            {
                BackColor = System.Drawing.Color.FromArgb(255, 128, 128, 128);
            }
            int sqHalfSize = squareSize / 2;
            int sqQuartSize = sqHalfSize / 2;
            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(64, 64);
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bm))
            {
                g.FillRectangle(new System.Drawing.SolidBrush(BackColor.Value), 0, 0, squareSize, squareSize);
                System.Drawing.Image iconimg = HTAlt.Tools.ResizeImage(baseIcon, sqHalfSize, sqHalfSize);
                g.DrawImage(iconimg, new System.Drawing.Rectangle(sqQuartSize, sqQuartSize, sqHalfSize, sqHalfSize));
            }
            return bm;
        }
        /// <summary>
        /// Finds the root node of <paramref name="doc"/>.
        /// </summary>
        /// <param name="doc">the <see cref="XmlNode"/> (probably <seealso cref="XmlDocument.DocumentElement"/>) to search on.</param>
        /// <returns>a <see cref="System.Xml.XmlNode"/> which represents as the root node.</returns>
        public static System.Xml.XmlNode FindRoot(System.Xml.XmlNode doc)
        {
            System.Xml.XmlNode found = null;
            if (doc.Name.ToLowerEnglish() == "root")
            {
                found = doc;
            }
            else
            {
                for (int i = 0; i < doc.ChildNodes.Count; i++)
                {
                    var node = doc.ChildNodes[i];
                    if (node.Name.ToLowerEnglish() == "root")
                    {
                        found = node;
                    }
                }
            }
            return found;
        }
        /// <summary>
        /// Turns all characters to lowercase, using en-US culture information to avoid language-specific ToLower() errors such as:
        /// <para>Turkish: I -&gt; ı İ -&gt; i</para>
        /// <para>English I &lt;-&gt; i</para>
        /// </summary>
        /// <param name="s"><see cref="string"/></param>
        /// <returns><see cref="string"/></returns>
        public static string ToLowerEnglish(this string s)
        {
            // USE TRANSLATOR
            // MS ToLower() fonksiyonunu açıklarken biraz ters örnek kullanmış, bu da hafif kafa karışıklığı oluşturmuş olabilir.
            // Gene de Türkçe'yi örnek olarak göstermeleri iyi olmuş. Malum İngilizce'de "İ" (Büyük i) ve "ı" (Küçük ı) yok.
            // Daha da kafa karışsın diye üstüne "İ" yi Unicode ile yazmışlar, ilk bakışta görmek biraz zor.
            // Bu da küçük bir "rant" olsun. Hem ToLowerInvariant() bir boka yaramıyor.
            return s.ToLower(new System.Globalization.CultureInfo("en-US", false));
        }
        /// <summary>
        /// Finds the root node of <paramref name="doc"/>.
        /// </summary>
        /// <param name="doc">The XML document.</param>
        /// <returns>a <see cref="System.Xml.XmlNode"/> which represents as the root node.</returns>
        public static System.Xml.XmlNode FindRoot(System.Xml.XmlDocument doc)
        {
            return FindRoot(doc.DocumentElement);
        }
        /// <summary>
        /// Converts <see cref="XmlNode.InnerXml"/> to formatted <seealso cref="string"/>.
        /// </summary>
        /// <param name="innerxml">Inenr XML</param>
        /// <returns>Formatted <paramref name="s"/>.</returns>
        public static string InnerXmlToString(this string innerxml)
        {
            return innerxml.Replace("&amp;", "&").Replace("&quot;", "\"").Replace("&apos;", "'").Replace("&lt;", "<").Replace("&gt;", ">");
        }
        /// <summary>
        /// Converts <paramref name="s"/> to <see cref="System.Xml"/> supported format.
        /// </summary>
        /// <param name="s">string</param>
        /// <returns>Formatted <paramref name="s"/>.</returns>
        public static string ToXML(this string s)
        {
            return s.Replace("&", "&amp;").Replace("\"", "&quot;").Replace("'", "&apos;").Replace("<", "&lt;").Replace(">", "&gt;");
        }
        /// <summary>
        /// Shortens the path.
        /// </summary>
        /// <param name="path">Path</param>
        /// <param name="main"><see cref="YorotMain"/></param>
        /// <returns><see cref="string"/></returns>
        public static string ShortenPath(this string path,YorotMain main)
        {
            return path.Replace(main.ProfilesFolder, "[PROFILES]")
                .Replace(main.WEFolder, "[WEBENG]")
                .Replace(main.ExtFolder, "[USEREXT]")
                .Replace(main.LangFolder, "[USERLANG]")
                .Replace(main.AppsFolder, "[USERAPPS]")
                .Replace(main.ThemesFolder, "[USERTHEME]")
                .Replace(main.Profiles.Current.CacheLoc, "[USERCACHE]")
                .Replace(main.LogFolder, "[LOGS]")
                .Replace(main.Profiles.Current.Path, "[USER]")
                .Replace(main.AppPath,"[APPPATH]");
        }
        /// <summary>
        /// Unshortens the path.
        /// </summary>
        /// <param name="path">Path</param>
        /// <param name="main"><see cref="YorotMain"/></param>
        /// <returns><see cref="string"/></returns>
        public static string GetPath(this string path, YorotMain main)
        {
            return path.Replace("[PROFILES]", main.ProfilesFolder)
                .Replace("[WEBENG]", main.WEFolder)
                .Replace("[USEREXT]", main.ExtFolder)
                .Replace("[USERLANG]", main.LangFolder)
                .Replace("[USERAPPS]", main.AppsFolder)
                .Replace("[USERTHEME]", main.ThemesFolder)
                .Replace("[USERCACHE]", main.Profiles.Current.CacheLoc)
                .Replace("[LOGS]",main.LogFolder)
                .Replace("[USER]",main.Profiles.Current.Path)
                .Replace("[APPPATH]",main.AppPath);
        }
    }
}
