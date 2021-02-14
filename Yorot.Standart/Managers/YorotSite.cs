using System;

namespace Yorot
{
    /// <summary>
    /// Site class used by History and Download managers.
    /// </summary>
    public class YorotSite 
    {
        /// <summary>
        /// Name of the site.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// URI of the site.
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Location on drive of site (probably Download manager site).
        /// </summary>
        public string FilePath { get; set;}
        /// <summary>
        /// Status of the site (probably Download manager site).
        /// </summary>
        public YorotSiteStatus Status { get; set;}
        /// <summary>
        /// Error code of site (probably Download manager site).
        /// </summary>
        public string ErrorCode { get; set;}
        /// <summary>
        /// Date of the site.
        /// </summary>
        public DateTime Date { get; set; }
    }
    /// <summary>
    /// Enum used by YorotSites in Download manager.
    /// </summary>
    public enum YorotSiteStatus
    {
        Finished,
        Error,
        Cancelled,
    }
}
