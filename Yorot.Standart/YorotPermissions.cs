using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Yorot
{
    // TODO
    public class YorotPermission
    {
        public string ID { get; set; }
        public object Requestor { get; set; }
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
        /// Permission denied for one time only. Will reset to <see cref="None"/> on close.
        /// </summary>
        DenyOneTime,
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
        public YorotPermissionControl(string configFile,YorotMain main) : base(configFile, main)
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
