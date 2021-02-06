using System;
using System.Collections.Generic;
using System.Text;

namespace Yorot
{
    public class DownloadManager
    {
        public Settings Settings { get; set; }
        public bool OpenFilesAfterDownload { get; set; } = false;
        public bool AutoDownload { get; set; } = true;
        public string DownloadFolder { get; set; } = "";
        public string ToXml()
        {

        }
        public void Save()
        {
            HTAlt.Tools.WriteFile(Settings.UserDownloads, ToXml(), Encoding.Unicode);
        }
    }
}
