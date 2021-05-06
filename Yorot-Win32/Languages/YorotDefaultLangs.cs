using HTAlt;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yorot
{
    /// <summary>
    /// Static class containing default language configurations.
    /// </summary>
    public static class YorotDefaultLangs
    {
        /// <summary>
        /// Generates the default languages.
        /// </summary>
        /// <param name="langLoc">Location of the language folder.</param>
        public static void GenLangs(string langLoc,bool force = false)
        {
            if (!langLoc.EndsWith("\\")) { langLoc += "\\"; }
            var d = YorotDefaultLanguages.DefaultLangList;
            for (int i = 0; i < d.Length; i++)
            {
                string l = d[i];
                string _l = langLoc + l + ".ylf";
                if (force || !System.IO.File.Exists(_l))
                {
                    HTAlt.Tools.WriteFile(_l, GetDefaultLang(l), Encoding.Unicode);
                }
            }
        }
        /// <summary>
        /// An array of languages that are default to this Yorot flavor.
        /// </summary>
        /// <summary>
        /// Gets the default configuration from <paramref name="codeName"/>.
        /// </summary>
        /// <param name="codeName">Code Name of the language.</param>
        /// <returns><see cref="string"/></returns>
        private static string GetDefaultLang(string codeName)
        {
            switch(codeName.ToLowerEnglish())
            {
                default:
                    Output.WriteLine("[DefaultLangs] Cannot find language \"" + codeName + "\". Loaded com.haltroy.english-us.", LogLevel.Warning);
                    return Properties.Resources.English_US;
                case "com.haltroy.english":
                case "com.haltroy.english-us":
                    return Properties.Resources.English_US;
                case "com.haltroy.english-gb":
                    return Properties.Resources.English_GB;
                case "com.haltroy.turkish":
                    return Properties.Resources.Turkish;
                case "com.haltroy.japanese":
                case "com.haltroy.chinese-s":
                case "com.haltroy.chinese-t":
                case "com.haltroy.french":
                case "com.haltroy.german":
                case "com.haltroy.itallian":
                case "com.haltroy.russian":
                case "com.haltroy.ukranian":
                case "com.haltroy.arabic":
                case "com.haltroy.persian":
                case "com.haltroy.spanish":
                case "com.haltroy.portuguese":
                case "com.haltroy.greek":
                case "com.haltroy.latin":
                case "com.haltroy.swedish":
                case "com.haltroy.norwegian":
                case "com.haltroy.danish":
                case "com.haltroy.punjabi":
                case "com.haltroy.romanian":
                case "com.haltroy.serbian":
                case "com.haltroy.hungarian":
                case "com.haltroy.dutch":
                case "com.haltroy.georgian":
                case "com.haltroy.hebrew":
                    Output.WriteLine("[DefaultLangs] Language \"" + codeName + "\" is not implemented yet! Loaded com.haltroy.english-us.",LogLevel.Warning);
                    return Properties.Resources.English_US;
            }
        }
    }
}
