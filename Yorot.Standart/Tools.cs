using System;
using System.Drawing;
using System.IO;

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
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.FillRectangle(new System.Drawing.SolidBrush(BackColor.Value), 0, 0, squareSize, squareSize);
                System.Drawing.Image iconimg = HTAlt.Tools.ResizeImage(baseIcon, sqHalfSize, sqHalfSize);
                g.DrawImage(iconimg, new System.Drawing.Rectangle(sqQuartSize, sqQuartSize, sqHalfSize, sqHalfSize));
            }
            return bm;
        }

        /// <summary>
        /// Determines if we passed a date.
        /// </summary>
        /// <param name="date"><see cref="DateTime"/></param>
        /// <returns><see cref="bool"/></returns>
        public static bool HasExpired(this DateTime date) => DateTime.Now.CompareTo(date.Add(new TimeSpan(2, 0, 0))) > 0;

        /// <summary>
        /// Turns <see cref="HTAlt.LogEventType"/> to <seealso cref="HTAlt.LogLevel"/>.
        /// </summary>
        /// <param name="e"><see cref="HTAlt.LogEventType"/></param>
        /// <returns><see cref="HTAlt.LogLevel"/></returns>
        public static HTAlt.LogLevel LogeEventToLogLevel(this HTAlt.LogEventType e)
        {
            switch (e)
            {
                default:
                case HTAlt.LogEventType.Nothing:
                    return HTAlt.LogLevel.None;

                case HTAlt.LogEventType.Info:
                    return HTAlt.LogLevel.Info;

                case HTAlt.LogEventType.Warning:
                    return HTAlt.LogLevel.Warning;

                case HTAlt.LogEventType.Error:
                    return HTAlt.LogLevel.Error;
            }
        }

        /// <summary>
        /// Uploads a file to server
        /// </summary>
        /// <param name="url">Address of the server.</param>
        /// <param name="filePath">Path of the file that is going to be sent.</param>
        /// <param name="username">FTP Username</param>
        /// <param name="password">FTP Password</param>
        public static void UploadFileToFtp(string url, string filePath, string username, string password)
        {
            string fileName = System.IO.Path.GetFileName(filePath);
            System.Net.FtpWebRequest request = (System.Net.FtpWebRequest)System.Net.WebRequest.Create(url + fileName);

            request.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new System.Net.NetworkCredential(username, password);
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = false;

            using (System.IO.FileStream fileStream = System.IO.File.OpenRead(filePath))
            {
                using (System.IO.Stream requestStream = request.GetRequestStream())
                {
                    fileStream.CopyTo(requestStream);
                    requestStream.Close();
                }
            }

            System.Net.FtpWebResponse response = (System.Net.FtpWebResponse)request.GetResponse();
            Console.WriteLine("Upload done: {0}", response.StatusDescription);
            response.Close();
        }

        /// <summary>
        /// Shortens the path.
        /// </summary>
        /// <param name="path">Path</param>
        /// <param name="main"><see cref="YorotMain"/></param>
        /// <returns><see cref="string"/></returns>
        public static string ShortenPath(this string path, YorotMain main)
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
                .Replace(main.AppPath, "[APPPATH]");
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
                .Replace("[LOGS]", main.LogFolder)
                .Replace("[USER]", main.Profiles.Current.Path)
                .Replace("[APPPATH]", main.AppPath);
        }

        /// <summary>
        /// Gets the <paramref name="site"/> icon. Can return null.
        /// </summary>
        /// <param name="site"><see cref="YorotSite"/></param>
        /// <param name="main"><see cref="YorotMain"/></param>
        /// <returns><see cref="Image"/></returns>
        public static Icon GetSiteIcon(YorotSite site, YorotMain main)
        {
            return File.Exists(main.AppPath + "\\favicons\\" + HTAlt.Tools.GetBaseURL(site.Url) + ".ico")
? new Icon(main.AppPath + "\\favicons\\" + HTAlt.Tools.GetBaseURL(site.Url) + ".ico")
: null;
        }

        /// <summary>
        /// Sets the <paramref name="site"/> image.
        /// </summary>
        /// <param name="site"><see cref="YorotSite"/></param>
        /// <param name="image">Favicon</param>
        /// <param name="main"><see cref="YorotMain"/></param>
        public static void SetSiteIcon(YorotSite site, Icon image, YorotMain main)
        {
            using (FileStream fs = new FileStream(main.AppPath + "\\favicons\\" + HTAlt.Tools.GetBaseURL(site.Url) + ".ico", FileMode.Create))
            {
                image.Save(fs);
            }
        }
    }
}