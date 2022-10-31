using System.ComponentModel;

namespace SmallNetUtils.Utils
{
    /// <summary>
    /// Utils to work with program forms
    /// </summary>
    public static class FormsUtil
    {
        /// <summary>
        /// Check if a form opened in Designer mode
        /// </summary>
        /// <remarks> To prevent subscribing for events in Designer mode (for example) use this flag </remarks>
        public static bool IsDesignMode
        {
            get
            {
                return LicenseManager.UsageMode == LicenseUsageMode.Designtime;
            }
        }
    }
}
