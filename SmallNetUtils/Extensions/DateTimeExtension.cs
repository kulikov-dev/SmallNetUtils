using SmallNetUtils.Data;

namespace SmallNetUtils.Extensions
{
    /// <summary>
    /// Extension to work with DateTime
    /// </summary>
    public static class DateTimeExtension
    {
        /// <summary>
        /// Check if date is midnight
        /// </summary>
        /// <param name="date"> Date </param>
        /// <returns> Flag if midnight</returns>
        public static bool IsMidnight(this DateTime date)
        {
            return date.Hour == 0 && date.Minute == 0 && date.Second == 0;
        }

        /// <summary>
        /// Check if date is month start
        /// </summary>
        /// <param name="date"> Date </param>
        /// <returns> Flag if month start </returns>
        /// <remarks> Including time start at 00:00:00 </remarks>
        public static bool IsMonthStart(this DateTime date)
        {
            return date.IsMidnight() && date.Day == 1;
        }

        /// <summary>
        /// Check if date is quarter start
        /// </summary>
        /// <param name="date"> Date </param>
        /// <returns> Flag if quarter start </returns>
        /// <remarks> Including time start at 00:00:00 </remarks>
        public static bool IsQuarterStart(this DateTime date)
        {
            return date.IsMonthStart() && date.Month is 1 or 4 or 7 or 10;
        }

        /// <summary>
        /// Check if date is year start
        /// </summary>
        /// <param name="date"> Date </param>
        /// <returns> Flag if year start </returns>
        /// <remarks> Including time start at 00:00:00 </remarks>
        public static bool IsYearStart(this DateTime date)
        {
            return date.IsMonthStart() && date.Month == 1;
        }

        /// <summary>
        /// Get date quarter
        /// </summary>
        /// <param name="date"> Date </param>
        /// <returns> Quarter number </returns>
        public static int GetQuarter(this DateTime date)
        {
            return date.Month switch
            {
                1 or 2 or 3 => 1,
                4 or 5 or 6 => 2,
                7 or 8 or 9 => 3,
                _ => 4,
            };
        }

        /// <summary>
        /// Get date quarter
        /// </summary>
        /// <param name="date"> Date </param>
        /// <param name="isArabic"> Flag for an arabic quarter format </param>
        /// <returns></returns>
        public static string GetQuarter(this DateTime date, bool isArabic)
        {
            var quarter = date.GetQuarter();

            if (isArabic)
            {
                return quarter switch
                {
                    1 => "I",
                    2 => "II",
                    3 => "III",
                    _ => "IV"
                };
            }

            return quarter.ToString();
        }

        /// <summary>
        /// Check if date is start of DateInterval type
        /// </summary>
        /// <param name="date"> Date </param>
        /// <param name="dateType"> Interval type </param>
        /// <returns> Flag if start </returns>
        public static bool IsStartOfIntervalType(this DateTime date, Microsoft.VisualBasic.DateInterval dateType)
        {
            return dateType switch
            {
                Microsoft.VisualBasic.DateInterval.Day => date.IsMidnight(),
                Microsoft.VisualBasic.DateInterval.Month => date.Day == 1,
                Microsoft.VisualBasic.DateInterval.Quarter => date.IsStartOfIntervalType(Microsoft.VisualBasic.DateInterval.Month) && date.Month is 1 or 4 or 7 or 10,
                Microsoft.VisualBasic.DateInterval.Year => date.IsStartOfIntervalType(Microsoft.VisualBasic.DateInterval.Month) && date.Month == 1,
                _ => throw new NotSupportedException("This DateInterval not supported.")
            };
        }

        /// <summary>
        /// Addition amount to a date by DateInterval type
        /// </summary>
        /// <param name="date"> Date </param>
        /// <param name="dateType"> DateInterval type </param>
        /// <param name="amount"> Amount to add </param>
        /// <returns> Date by adding amount </returns>
        public static DateTime AddByType(this DateTime date, Microsoft.VisualBasic.DateInterval dateType, int amount = 1)
        {
            return dateType switch
            {
                Microsoft.VisualBasic.DateInterval.Day => date.AddDays(amount),
                Microsoft.VisualBasic.DateInterval.Month => date.AddMonths(amount),
                Microsoft.VisualBasic.DateInterval.Quarter => date.AddMonths(3 * amount),
                Microsoft.VisualBasic.DateInterval.Year => date.AddYears(amount),
                _ => throw new NotSupportedException("This DateInterval not supported.")
            };
        }

        /// <summary>
        /// Substracting amount to a date by DateInterval type
        /// </summary>
        /// <param name="date"> Date </param>
        /// <param name="dateType"> DateInterval type </param>
        /// <param name="amount"> Amount to substract </param>
        /// <returns> Date by subtracting amount </returns>
        public static DateTime SubtactByType(this DateTime date, Microsoft.VisualBasic.DateInterval dateType, int amount = 1)
        {
            return dateType switch
            {
                Microsoft.VisualBasic.DateInterval.Day => date.AddDays(-amount),
                Microsoft.VisualBasic.DateInterval.Month => date.AddMonths(-amount),
                Microsoft.VisualBasic.DateInterval.Quarter => date.AddMonths(-3 * amount),
                Microsoft.VisualBasic.DateInterval.Year => date.AddYears(-amount),
                _ => throw new NotSupportedException("This DateInterval not supported.")
            };
        }

        /// <summary>
        /// Ceil date by DateInterval date
        /// </summary>
        /// <param name="date"> Date </param>
        /// <param name="dateType"> DateInterval type </param>
        /// <returns> Ceil date </returns>
        public static DateTime Ceil(this DateTime date, Microsoft.VisualBasic.DateInterval dateType)
        {
            return date.IsStartOfIntervalType(dateType) ? date : date.AddByType(dateType).Floor(dateType);
        }

        /// <summary>
        /// Floor date by DateInterval date
        /// </summary>
        /// <param name="date"> Date </param>
        /// <param name="dateType"> DateInterval type </param>
        /// <returns> Floor date </returns>
        public static DateTime Floor(this DateTime date, Microsoft.VisualBasic.DateInterval dateType)
        {
            return dateType switch
            {
                Microsoft.VisualBasic.DateInterval.Month => new DateTime(date.Year, date.Month, 1),
                Microsoft.VisualBasic.DateInterval.Quarter => date.Month switch
                {
                    1 or 2 or 3 => new DateTime(date.Year, 1, 1),
                    4 or 5 or 6 => new DateTime(date.Year, 4, 1),
                    7 or 8 or 9 => new DateTime(date.Year, 7, 1),
                    _ => new DateTime(date.Year, 10, 1),
                },
                Microsoft.VisualBasic.DateInterval.Year => new DateTime(date.Year, 1, 1),
                _ => throw new NotSupportedException("This DateInterval not supported.")
            };
        }

        /// <summary>
        /// Convert DateTime to DateInterval according to DateInterval type
        /// </summary>
        /// <param name="date"> Date </param>
        /// <param name="dateType"> DateInterval type </param>
        /// <returns> DateInterval </returns>
        public static DateInterval GetDateInterval(this DateTime date, Microsoft.VisualBasic.DateInterval dateType)
        {
            if (!date.IsStartOfIntervalType(dateType))
            {
                return new DateInterval(date.Floor(dateType), date.Ceil(dateType));
            }

            return new DateInterval(date, date.AddByType(dateType));
        }
    }
}