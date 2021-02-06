using System;
using System.Collections.Generic;
using System.Text;

namespace Yorot
{
    public class ProfileManager
    {
        public Settings Settings { get; set; }
        public string ToXml()
        {

        }
        public void Save()
        {
            HTAlt.Tools.WriteFile(Settings.UserProfile, ToXml(), Encoding.Unicode);
        }
    }
}
