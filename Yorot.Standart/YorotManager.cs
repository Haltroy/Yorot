using HTAlt;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Yorot
{
    /// <summary>
    /// Base class for Yorot managers.
    /// <para></para>
    /// You only need to override <see cref="ToXml"/> and <seealso cref="ExtractXml(XmlNode)"/> on creating new manager.
    /// </summary>
    public abstract class YorotManager
    {
        /// <summary>
        /// Creates a new manager.
        /// </summary>
        /// <param name="configFile">Location of the configuration file of this manager on disk.</param>
        /// <param name="main"><see cref="YorotMain"/> of this manager.</param>
        public YorotManager(string configFile,YorotMain main)
        {
            Output.WriteLine("[" + this.GetType().Name + "] Starting Init...", LogLevel.Info);
            if (main is null) { throw new ArgumentNullException("main"); } Main = main;
            if (!string.IsNullOrWhiteSpace(configFile))
            {
                ConfigFile = configFile;
                if (System.IO.File.Exists(configFile))
                {
                    try
                    {
                        string xml = HTAlt.Tools.ReadFile(configFile, Encoding.Unicode);
                        if (!string.IsNullOrWhiteSpace(xml))
                        {
                            try
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(xml);
                                XmlNode rootNode = Yorot.Tools.FindRoot(doc);
                                ExtractXml(rootNode);
                            }
                            catch (XmlException xe)
                            {
                                Output.WriteLine("[" + this.GetType().Name + "] Loaded defaults, configuration file has XML error(s): " + xe.Message, LogLevel.Warning);
                            }
                            catch (Exception e)
                            {
                                Output.WriteLine("[" + this.GetType().Name + "] Loaded defaults, exception caught: " + e.Message, LogLevel.Warning);
                            }
                        }
                        else
                        {
                            Output.WriteLine("[" + this.GetType().Name + "] Loaded defaults, configuration file was empty.", LogLevel.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        Output.WriteLine("[" + this.GetType().Name + "] Loaded defaults, exception caught: " + ex.Message, LogLevel.Warning);
                    }
                }
                else
                {
                    Output.WriteLine("[" + this.GetType().Name + "] Loaded defaults, configuration file does not exists.", LogLevel.Warning);
                }
            }
            else
            {
                Output.WriteLine("[" + this.GetType().Name + "] Loaded defaults, configuration file location was empty.", LogLevel.Warning);
            }
            Output.WriteLine("[" + this.GetType().Name + "] Init done.", LogLevel.Info);
        }
        /// <summary>
        /// Location of the configuration file of this manager on disk.
        /// </summary>
        public string ConfigFile { get; set; }
        /// <summary>
        /// Retrieves current configuration as XML.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public abstract string ToXml();
        /// <summary>
        /// Same as <see cref="ToXml"/>
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public override string ToString()
        {
            return ToXml();
        }
        /// <summary>
        /// Saves current configuration to disk.
        /// </summary>
        public void Save() => HTAlt.Tools.WriteFile(ConfigFile, ToXml(), Encoding.Unicode);
        /// <summary>
        /// Extracts configuration from XML root node.
        /// <para></para>
        /// You can throw exceptions if something in configuration is wrong or just write a warning with <see cref="Output.WriteLine(string, LogLevel)"/>.
        /// </summary>
        /// <param name="rootNode">Root node</param>
        public abstract void ExtractXml(XmlNode rootNode);
        /// <summary>
        /// Gets <see cref="YorotMain"/> associated with this manager.
        /// </summary>
        public YorotMain Main { get; set; }
    }
}
