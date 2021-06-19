namespace Yorot
{
    /// <summary>
    /// Yorot Permission item
    /// </summary>
    public class YorotPermission
    {
        private YorotPermissionMode allowance = YorotPermissionMode.None;

        /// <summary>
        /// Creates anew <see cref="YorotPermission"/>.
        /// </summary>
        /// <param name="id">ID of the permission.</param>
        /// <param name="requestor">The <see cref="object"/> that requested the permission.</param>
        /// <param name="main"><see cref="YorotMain"/></param>
        /// <param name="allowance">Permission mode.</param>
        public YorotPermission(string id, object requestor, YorotMain main, YorotPermissionMode allowance = YorotPermissionMode.None)
        {
            ID = id;
            Requestor = requestor;
            Main = main;
            Allowance = allowance;
        }

        /// <summary>
        /// Id or internal name of this permission item.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// The object that requested this permission.
        /// </summary>
        public object Requestor { get; set; }

        /// <summary>
        /// Main of this permmission.
        /// </summary>
        public YorotMain Main { get; set; }

        /// <summary>
        /// Value of this permission item.
        /// </summary>
        public YorotPermissionMode Allowance
        {
            get => allowance;
            set => allowance = Main.OnPermissionRequest(this, value);
        }
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
}