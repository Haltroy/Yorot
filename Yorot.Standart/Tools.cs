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
        /// <returnsa <see cref="XmlNode"/> which represents as the root node.></returns>
        public static System.Xml.XmlNode FindRoot(System.Xml.XmlNode doc)
        {
            System.Xml.XmlNode found = null;
            if (doc.Name.ToLower() == "root")
            {
                found = doc;
            }
            else
            {
                for (int i = 0; i < doc.ChildNodes.Count; i++)
                {
                    var node = doc.ChildNodes[i];
                    if (node.Name.ToLower() == "root")
                    {
                        found = node;
                    }
                }
            }
            return found;
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
        /// <param name="appPath">Application path used by Yorot.</param>
        /// <returns><see cref="string"/></returns>
        public static string ShortenPath(this string path,string appPath)
        {
            var UserLoc = appPath + @"usr\";
            var LogPath = appPath + @"log\";
            var CacheLoc = UserLoc + @"\cache\";
            var ThemesLoc = UserLoc + @"\themes\";
            var UserApps = UserLoc + @"apps\";
            var LangLoc = UserLoc + @"lang\";
            var ExtLoc = UserLoc + @"ext\";
            var EngineLoc = UserLoc + @"engines\";
            var UserProfiles = UserLoc + @"profiles\";
            return path.Replace(UserProfiles, "[USERPROF]")
                .Replace(EngineLoc, "[WEBENG]")
                .Replace(ExtLoc, "[USEREXT]")
                .Replace(LangLoc, "[USERLANG]")
                .Replace(UserApps, "[USERAPPS]")
                .Replace(ThemesLoc, "[USERTHEME]")
                .Replace(CacheLoc, "[USERCACHE]")
                .Replace(LogPath, "[LOGS]")
                .Replace(UserLoc, "[USR]")
                .Replace(appPath,"[APPPATH]");
        }
        /// <summary>
        /// Unshortens the path.
        /// </summary>
        /// <param name="path">Path</param>
        /// <param name="appPath">Application path used by Yorot.</param>
        /// <returns><see cref="string"/></returns>
        public static string GetPath(this string path, string appPath)
        {
            var UserLoc = appPath + @"usr\";
            var LogPath = appPath + @"log\";
            var CacheLoc = UserLoc + @"\cache\";
            var ThemesLoc = UserLoc + @"\themes\";
            var UserApps = UserLoc + @"apps\";
            var LangLoc = UserLoc + @"lang\";
            var ExtLoc = UserLoc + @"ext\";
            var EngineLoc = UserLoc + @"engines\";
            var UserProfiles = UserLoc + @"profiles\";
            return path.Replace("[USERPROF]", UserProfiles)
                .Replace("[WEBENG]", EngineLoc)
                .Replace("[USEREXT]", ExtLoc)
                .Replace("[USERLANG]", LangLoc)
                .Replace("[USERAPPS]", UserApps)
                .Replace("[USERTHEME]", ThemesLoc)
                .Replace("[USERCACHE]", CacheLoc)
                .Replace("[LOGS]", LogPath)
                .Replace("[USR]", UserLoc)
                .Replace("[APPPATH]", appPath);
        }
    }
}
