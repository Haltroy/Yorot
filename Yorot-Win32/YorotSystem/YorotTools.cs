using System;
using System.Linq;
using System.Xml;

namespace Yorot
{
    public static class YorotTools
    {
        /// <summary>
        /// Detects if user can access <paramref name="dir"/> by try{} method.
        /// </summary>
        /// <param name="dir">Directory</param>
        /// <returns><see cref="true"/> if can access to folder, <seealso cref="false"/> if user has no access to <paramref name="dir"/> and throws <see cref="Exception"/> on other scenarios.</returns>
        public static bool HasWriteAccess(string dir)
        {
            try
            {
                var random = HTAlt.Tools.GenerateRandomText(17);
                HTAlt.Tools.WriteFile(dir + "\\YOROT.TEST",random, System.Text.Encoding.Unicode);
                string file = HTAlt.Tools.ReadFile(dir + "\\YOROT.TEST", System.Text.Encoding.Unicode);
                if (file == random)
                {
                    return true;
                }
                else
                {
                    throw new Exception("Test file \"" + dir + "\\YOROT.TEST" + "\" was altered.");
                }
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Gets directory size.
        /// Thanks to hao & Alexandre Pepin from StackOverflow
        /// https://stackoverflow.com/a/468131
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static long DirSize(System.IO.DirectoryInfo d)
        {
            long size = 0;
            // Add file sizes.
            System.IO.FileInfo[] fis = d.GetFiles();
            foreach (System.IO.FileInfo fi in fis)
            {
                size += fi.Length;
            }
            // Add subdirectory sizes.
            System.IO.DirectoryInfo[] dis = d.GetDirectories();
            foreach (System.IO.DirectoryInfo di in dis)
            {
                size += DirSize(di);
            }
            return size;
        }
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
        /// Converts <paramref name="img"/> to an <see cref="Icon"/>
        /// Thanks to Hans Passant from StackOverflow.
        /// https://stackoverflow.com/a/21389253
        /// </summary>
        /// <param name="img">Convertion <see cref="Image"/></param>
        /// <returns><seealso cref="Icon"/></returns>
        public static System.Drawing.Icon IconFromImage(System.Drawing.Image img)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(ms);
            // Header
            bw.Write((short)0);   // 0 : reserved
            bw.Write((short)1);   // 2 : 1=ico, 2=cur
            bw.Write((short)1);   // 4 : number of images
                                  // Image directory
            int w = img.Width;
            if (w >= 256)
            {
                w = 0;
            }

            bw.Write((byte)w);    // 0 : width of image
            int h = img.Height;
            if (h >= 256)
            {
                h = 0;
            }

            bw.Write((byte)h);    // 1 : height of image
            bw.Write((byte)0);    // 2 : number of colors in palette
            bw.Write((byte)0);    // 3 : reserved
            bw.Write((short)0);   // 4 : number of color planes
            bw.Write((short)0);   // 6 : bits per pixel
            long sizeHere = ms.Position;
            bw.Write(0);     // 8 : image size
            int start = (int)ms.Position + 4;
            bw.Write(start);      // 12: offset of image data
                                  // Image data
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            int imageSize = (int)ms.Position - start;
            ms.Seek(sizeHere, System.IO.SeekOrigin.Begin);
            bw.Write(imageSize);
            ms.Seek(0, System.IO.SeekOrigin.Begin);

            // And load it
            return new System.Drawing.Icon(ms);
        }
        /// <summary>
        /// Trims all non-numeric chars (except ",","." and "-")
        /// </summary>
        /// <param name="input">String</param>
        /// <returns></returns>
        public static string TrimToNumbers(this string input)
        {
            return new string(input.Where(c => (char.IsDigit(c) || c == ',' || c == '.' || c == '-')).ToArray());
        }
        /// <summary>
        /// Prettifies XML code.
        /// Thanks to S M Kamran & Bakudan from StackOverflow
        /// https://stackoverflow.com/a/1123731
        /// </summary>
        /// <param name="xml">XML code</param>
        /// <returns>Prettified <paramref name="xml"/></returns>
        public static string PrintXML(string xml)
        {
            string result = "";

            System.IO.MemoryStream mStream = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(mStream, System.Text.Encoding.Unicode);
            System.Xml.XmlDocument document = new System.Xml.XmlDocument();

            // Load the XmlDocument with the XML.
            document.LoadXml(xml);

            writer.Formatting = System.Xml.Formatting.Indented;

            // Write the XML into a formatting XmlTextWriter
            document.WriteContentTo(writer);
            writer.Flush();
            mStream.Flush();

            // Have to rewind the MemoryStream in order to read
            // its contents.
            mStream.Position = 0;

            // Read MemoryStream contents into a StreamReader.
            System.IO.StreamReader sReader = new System.IO.StreamReader(mStream);

            // Extract the text from the StreamReader.
            string formattedXml = sReader.ReadToEnd();

            result = formattedXml;

            mStream.Close();
            writer.Close();

            return result;
        }
        /// <summary>
        /// Finds the root node of <paramref name="doc"/>.
        /// </summary>
        /// <param name="doc">the <see cref="XmlNode"/> (probably <seealso cref="XmlDocument.DocumentElement"/>) to search on.</param>
        /// <returnsa <see cref="XmlNode"/> which represents as the root node.></returns>
        public static XmlNode FindRoot(XmlNode doc)
        {
            XmlNode found = null;
            for (int i = 0; i < doc.ChildNodes.Count;i++)
            {
                var node = doc.ChildNodes[i];
                if (node.Name.ToLower() == "root")
                {
                    found = node;
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
            return innerxml.Replace("&amp;","&").Replace("&quot;","\"").Replace("&apos;","'").Replace("&lt;","<").Replace("&gt;",">");
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
    }
}
