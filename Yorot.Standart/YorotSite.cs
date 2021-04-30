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
        public string FilePath { get; set; }

        /// <summary>
        /// Status of the site (probably Download manager site).
        /// </summary>
        public YorotSiteStatus Status { get; set; }

        /// <summary>
        /// Error code of site (probably Download manager site).
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Date of the site.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Permissions of this site.
        /// </summary>
        public YorotSitePermissions Permissions { get; set; }
    }

    /// <summary>
    /// Permissions of a generic <see cref="YorotSite"/>.
    /// </summary>
    public class YorotSitePermissions
    {
        /// <summary>
        /// Creates a new <see cref="YorotSitePermissions"/> with default values.
        /// </summary>
        /// <param name="site"><see cref="YorotSite"/></param>
        public YorotSitePermissions(YorotSite site) : this(site, YorotPermissionMode.None, YorotPermissionMode.None, YorotPermissionMode.None, 0, false, YorotPermissionMode.None, YorotPermissionMode.None) { }

        /// <summary>
        /// Creates a new <see cref="YorotSitePermissions"/> with custom values.
        /// </summary>
        /// <param name="site"><see cref="YorotSite"/></param>
        /// <param name="allowMic">Determines if the microphone can be used by this site.</param>
        /// <param name="allowCam">Determines if the camera can be used by this site.</param>
        /// <param name="allowNotif">Determines if this site can send notifications.</param>
        /// <param name="notifPriority">Determines the priority of the notifications comşng from this website.
        /// <para></para>
        /// -1 = Prioritize others
        /// <para></para>
        /// 0 = Normal
        /// <para></para>
        /// 1 = Prioritize this</param>
        /// <param name="startNotifOnBoot">Dertermines if Yorot should start notification listener on start for this site.</param>
        /// <param name="allowWE">Determines if this website can use Web Engines.</param>
        /// <param name="allowYS">Determines if this site can access Yorot Special.</param>
        public YorotSitePermissions(YorotSite site, YorotPermissionMode allowMic, YorotPermissionMode allowCam, YorotPermissionMode allowNotif, int notifPriority, bool startNotifOnBoot, YorotPermissionMode allowWE, YorotPermissionMode allowYS)
        {
            Site = site;
            this.allowMic = new YorotPermission() { Allowance = allowMic, ID = "allowMic", Requestor = site };
            this.allowCam = new YorotPermission() { Allowance = allowCam, ID = "allowCam", Requestor = site };
            this.allowNotif = new YorotPermission() { Allowance = allowNotif, ID = "allowNotif", Requestor = site };
            this.notifPriority = notifPriority;
            this.startNotifOnBoot = startNotifOnBoot;
            this.allowWE = new YorotPermission() { Allowance = allowWE, ID = "allowWE", Requestor = site };
            this.allowYS = new YorotPermission() { Allowance = allowYS, ID = "allowYS", Requestor = site };
        }

        /// <summary>
        /// The site of these permissions.
        /// </summary>
        public YorotSite Site { get; set; }

        /// <summary>
        /// Determines if the microphone can be used by this site.
        /// </summary>
        public YorotPermission allowMic { get; set; }

        /// <summary>
        /// Determines if the camera can be used by this site.
        /// </summary>
        public YorotPermission allowCam { get; set; }

        /// <summary>
        /// Determines if this site can send notifications.
        /// </summary>
        public YorotPermission allowNotif { get; set; }

        /// <summary>
        /// Determines the priority of the notifications comşng from this website.
        /// <para></para>
        /// -1 = Prioritize others
        /// <para></para>
        /// 0 = Normal
        /// <para></para>
        /// 1 = Prioritize this
        /// </summary>
        public int notifPriority { get; set; } = 0;

        /// <summary>
        /// Dertermines if Yorot should start notification listener on start for this site.
        /// </summary>
        public bool startNotifOnBoot { get; set; } = false;

        /// <summary>
        /// Determines if this website can use Web Engines.
        /// </summary>
        public YorotPermission allowWE { get; set; }

        /// <summary>
        /// Determines if this site can access Yorot Special.
        /// </summary>
        public YorotPermission allowYS { get; set; }
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