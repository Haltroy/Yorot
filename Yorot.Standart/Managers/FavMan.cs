using System;
using System.Collections.Generic;
using System.Text;

namespace Yorot
{
    public class FavMan
    {
        public Settings Settings { get; set; }
        public bool ShowFavorites { get; set; } = true;
        public string ToXml()
        {

        }
        public void Save()
        {
            HTAlt.Tools.WriteFile(Settings.UserFavorites, ToXml(), Encoding.Unicode);
        }
    }
}
