using System;
using System.Collections.Generic;
using System.Text;

namespace Yorot
{
    public class ProfileManager
    {
        public string ToXml()
        {
            //TODO
            throw new NotImplementedException();
        }
        public List<YorotProfile> Profiles { get; set; } = new List<YorotProfile>();
        public void Save()
        {
            //TODO
        }
        public YorotProfile Current { get => Profiles[0]; set => Profiles[0] = value; }
        public YorotMain Main { get; set; }
    }
    /// <summary>
    /// Class for handling Yorot profiles.
    /// </summary>
    public class YorotProfile
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public string Path { get; set; }
        public Settings Settings { get; set; }
        public System.Drawing.Image Picture { get => HTAlt.Tools.ReadFile(Path + "picture.png", System.Drawing.Imaging.ImageFormat.Png); set => HTAlt.Tools.WriteFile(Path + "picture.png", value, System.Drawing.Imaging.ImageFormat.Png); }
        public ProfileManager Manager { get; set; }
        /// <summary>
        /// User Cache location.
        /// </summary>
        public string CacheLoc { get; set; }
        /// <summary>
        /// User settings location.
        /// </summary>
        public string UserSettings { get; set; }
        /// <summary>
        /// History Manager configuration file location.
        /// </summary>
        public string UserHistory { get; set; }
        /// <summary>
        /// Favorites Manager configuration file location.
        /// </summary>
        public string UserFavorites { get; set; }
        /// <summary>
        /// Downloads Manager configuration file location.
        /// </summary>
        public string UserDownloads { get; set; }
    }
}
