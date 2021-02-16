using System.Collections.Generic;

namespace Yorot
{
    /// <summary>
    /// Yorot Extension Manager
    /// </summary>
    public class ExtensionManager
    {
        /// <summary>
        /// Creates a new Extension manager.
        /// </summary>
        /// <param name="configFile">Location of the configuration file in drive.</param>
        public ExtensionManager(string configFile)
        {

        }
        /// <summary>
        /// Settings used by this manager.
        /// </summary>
        public Settings Settings { get; set; }
        public List<YorotExtension> Extensions { get; set; } = new List<YorotExtension>();
        /// <summary>
        /// Prints the current configuration as XML.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public string ToXml()
        {

        }
        /// <summary>
        /// Saves 
        /// </summary>
        public void Save()
        {
            HTAlt.Tools.WriteFile(Settings.UserExt, ToXml(), System.Text.Encoding.Unicode);
        }
    }
    /// <summary>
    /// Yorot Extension.
    /// </summary>
    public class YorotExtension
    {
        /// <summary>
        /// Creates a new yorot Extension.
        /// </summary>
        /// <param name="manifestFile">Location of the manifest file for this extension on drive.</param>
        public YorotExtension(string manifestFile)
        {
            // TODO
        }
        /// <summary>
        /// Associated extension manager for this extension.
        /// </summary>
        public ExtensionManager Manager { get; set; }
        /// <summary>
        /// Location fo the manifest file for this extension on drive.
        /// </summary>
        public string ManifestFile { get; set; }
        /// <summary>
        /// Code Name of the extension.
        /// </summary>
        public string CodeName { get; set; }
        /// <summary>
        /// Display name of the extension.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Author of the extension.
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// Version of the extension.
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// Display Icon of the extension.
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// Sİze of the Pop-up menu used in this extension.
        /// </summary>
        public System.Drawing.Size Size { get; set; }
        /// <summary>
        /// Document to load as pop-up menu.
        /// </summary>
        public string Popup { get; set; }
        /// <summary>
        /// Script to run on Yorot startup.
        /// </summary>
        public string Startup { get; set; }
        /// <summary>
        /// Script to run on background for allowed pages.
        /// </summary>
        public string Background { get; set; }
        /// <summary>
        /// List of locations to files that used in this extension.
        /// </summary>
        public List<string> Files { get; set; }
        /// <summary>
        /// Settings for this extension.
        /// </summary>
        public YorotExtensionSettings Settings { get; set; }
        /// <summary>
        /// List of pages that user allowed this extension to run.
        /// </summary>
        public List<string> PageList { get; set; } = new List<string>();
    }
    public class YorotExtensionRCOption
    {
        /// <summary>
        /// Text to display in option
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Location of the script to run when user clicks on drive.
        /// </summary>
        public string Script { get; set; }
        /// <summary>
        /// Icon to display near text.
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// Determines when to display this item.
        /// </summary>
        public RightClickOptionStyle Option { get; set; }
    }
    /// <summary>
    /// Right-Click options styles for extensions.
    /// </summary>
    public enum RightClickOptionStyle
    {
        /// <summary>
        /// Shows option when nothing is selected
        /// </summary>
        None,
        /// <summary>
        /// Shows option when user hovering or selected a link.
        /// </summary>
        Link,
        /// <summary>
        /// Shows option if user hovering or selected an image.
        /// </summary>
        Image,
        /// <summary>
        /// Shows option if user hovered or selected a text.
        /// </summary>
        Text,
        /// <summary>
        /// Shows option if user hovered or selected an edit text box.
        /// </summary>
        Edit,
        /// <summary>
        /// Always shows option on anything.
        /// </summary>
        Always
    }
    /// <summary>
    /// Settings for Yorot Extensions.
    /// </summary>
    public class YorotExtensionSettings
    {
        /// <summary>
        /// Load extension on Yorot start.
        /// </summary>
        public bool autoLoad { get; set; } = false;
        /// <summary>
        /// If set to <see cref="true"/>, shows pop-up menu when user clicks on extension icon.
        /// </summary>
        public bool showPopupMenu { get; set; } = false;
        /// <summary>
        /// <see cref="true"/> if Extension has Proxy manipulations.
        /// </summary>
        public bool hasProxy { get; set; } = false;
        /// <summary>
        /// <see cref="true"/> if this extension can be auto-updated.
        /// </summary>
        public bool useHaltroyUpdater { get; set; } = false;
    }
}
