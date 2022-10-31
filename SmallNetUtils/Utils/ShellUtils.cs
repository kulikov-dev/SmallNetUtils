namespace SmallNetUtils.Utils
{
    /// <summary>
    /// Shell utils
    /// </summary>
    public static class ShellUtils
    {
        /// <summary>
        /// Flag if user launcher app is a developer
        /// </summary>
        /// <returns> Flag if developer </returns>
        public static bool IsDeveloper()
        {
            #if (DEBUG)
            return true;
            #else
            return false;
            #endif
        }
    }
}
