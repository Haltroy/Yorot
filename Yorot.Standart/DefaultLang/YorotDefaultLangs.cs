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
        /// Gets the default configuration from <paramref name="codeName"/>.
        /// </summary>
        /// <param name="codeName">Code Name of the language.</param>
        /// <returns><see cref="string"/></returns>
        public static string GetDefaultLang(string codeName)
        {
            switch(codeName.ToLowerEnglish())
            {
                default:
                    Output.WriteLine("[DefaultLangs] Cannot find language \"" + codeName + "\". Loaded com.haltroy.english-us.", LogLevel.Warning);
                    return English_US;
                case "com.haltroy.english":
                case "com.haltroy.english-us":
                    return English_US;
                case "com.haltroy.english-gb":
                    return English_GB;
                case "com.haltroy.turkish":
                    return Turkish;
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
                    return English_US;
            }
        }
        public static string[] DefaultLangList => new string[] 
        {   
            "com.haltroy.english" , 
            "com.haltroy.english-us",
            "com.haltroy.english-gb",
            "com.haltroy.turkish",
            "com.haltroy.japanese",
            "com.haltroy.chinese-s",
            "com.haltroy.chinese-t",
            "com.haltroy.french",
            "com.haltroy.german",
            "com.haltroy.itallian",
            "com.haltroy.russian",
            "com.haltroy.ukranian",
            "com.haltroy.arabic",
            "com.haltroy.persian",
            "com.haltroy.spanish",
            "com.haltroy.portuguese",
            "com.haltroy.greek",
            "com.haltroy.latin",
            "com.haltroy.swedish",
            "com.haltroy.norwegian",
            "com.haltroy.danish",
            "com.haltroy.punjabi",
            "com.haltroy.romanian",
            "com.haltroy.serbian",
            "com.haltroy.hungarian",
            "com.haltroy.dutch",
            "com.haltroy.georgian",
            "com.haltroy.hebrew"
        };
        /// <summary>
        /// English (United States) language configuration.
        /// </summary>
        public static string English_US => Properties.Resources.English_US;
        /// <summary>
        /// English (Great Britain) language configuration.
        /// </summary>
        public static string English_GB => Properties.Resources.English_GB;
        /// <summary>
        /// Turkish language configuration.
        /// </summary>
        public static string Turkish => Properties.Resources.Turkish;
    }
}
