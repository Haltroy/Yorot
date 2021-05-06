using System;
using System.Xml;

namespace Yorot
{
    /// <summary>
    /// Yorot Permission item
    /// </summary>
    public class YorotPermission
    {
        /// <summary>
        /// Id or internal name of this permission item.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// The object that requested this permission.
        /// </summary>
        public object Requestor { get; set; }

        /// <summary>
        /// Value of this permission item.
        /// </summary>
        public YorotPermissionMode Allowance { get; set; } = YorotPermissionMode.None;
    }

    /// <summary>
    /// Types of permission allowance.
    /// </summary>
    public enum YorotPermissionMode
    {
        /// <summary>
        /// Nothing set.
        /// </summary>
        None,

        /// <summary>
        /// Permission denied forever.
        /// </summary>
        Deny,

        /// <summary>
        /// Permission allowed forever.
        /// </summary>
        Allow,

        /// <summary>
        /// Permission allowed for one time only. Will reset to <see cref="None"/> on close.
        /// </summary>
        AllowOneTime
    }

    public class YorotPermissionControl : YorotManager
    {
        public YorotPermissionControl(string configFile, YorotMain main) : base(configFile, main)
        {
        }

        public override void ExtractXml(XmlNode rootNode)
        {
            throw new NotImplementedException();
        }

        public override string ToXml()
        {
            throw new NotImplementedException();
        }
    }
}