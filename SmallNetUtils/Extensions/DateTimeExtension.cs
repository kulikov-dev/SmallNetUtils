using System;
using SmallNetUtils.Data;
using SmallNetUtils.Enums;

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
            return date.IsMonthStart() && (date.Month == 1 || date.Month == 4 || date.Month == 7 || date.Month == 10);
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
            switch (date.Month)
            {
                case 1:
                case 2:
                case 3:
                    return 1;
                case 4:
                case 5:
                case 6:
                    return 2;
                case 7:
                case 8:
                case 9:
                    return 3;
                default:
                    return 4;
            }
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

            if (!isArabic)
            {
                return quarter.ToString();
            }

            switch (quarter)
            {
                case 1:
                    return "I";
                case 2:
                    return "II";
                case 3:
                    return "III";
                default:
                    return "IV";
            }
        }

        /// <summary>
        /// Check if date is start of DateInterval type
        /// </summary>
        /// <param name="date"> Date </param>
        /// <param name="dateType"> Interval type </param>
        /// <returns> Flag if start </returns>
        public static bool IsStartOfIntervalType(this DateTime date, DateIntervalType dateType)
        {
            switch (dateType)
            {
                case DateIntervalType.Day:
                    return date.IsMidnight();
                case DateIntervalType.Month:
                    return date.IsMonthStart();
                case DateIntervalType.Quarter:
                    return date.IsQuarterStart();
                case DateIntervalType.Year:
                    return date.IsYearStart();
                default:
                    throw new NotSupportedException("This DateInterval not supported.");
            }
        }

        /// <summary>
        /// Addition amount to a date by DateInterval type
        /// </summary>
        /// <param name="date"> Date </param>
        /// <param name="dateType"> DateInterval type </param>
        /// <param name="amount"> Amount to add </param>
        /// <returns> Date by adding amount </returns>
        public static DateTime AddByType(this DateTime date, DateIntervalType dateType, int amount = 1)
        {
            switch (dateType)
            {
                case DateIntervalType.Day:
                    return date.AddDays(amount);
                case DateIntervalType.Month:
                    return date.AddMonths(amount);
                case DateIntervalType.Quarter:
                    return date.AddMonths(3 * amount);
                case DateIntervalType.Year:
                    return date.AddYears(amount);
                default:
                    throw new NotSupportedException("This DateInterval not supported.");
            }
        }

        /// <summary>
        /// Subtracting amount to a date by DateInterval type
        /// </summary>
        /// <param name="date"> Date </param>
        /// <param name="dateType"> DateInterval type </param>
        /// <param name="amount"> Amount to subtract </param>
        /// <returns> Date by subtracting amount </returns>
        public static DateTime SubtractByType(this DateTime date, DateIntervalType dateType, int amount = 1)
        {
            return date.AddByType(dateType, -amount);
        }

        /// <summary>
        /// Ceil date to DateInterval date
        /// </summary>
        /// <param name="date"> Date </param>
        /// <param name="dateType"> DateInterval type </param>
        /// <returns> Ceil date </returns>
        public static DateTime Ceil(this DateTime date, DateIntervalType dateType)
        {
            return date.IsStartOfIntervalType(dateType) ? date : date.AddByType(dateType).Floor(dateType);
        }

        /// <summary>
        /// Floor date to DateInterval date
        /// </summary>
        /// <param name="date"> Date </param>
        /// <param name="dateType"> DateInterval type </param>
        /// <returns> Floor date </returns>
        public static DateTime Floor(this DateTime date, DateIntervalType dateType)
        {
            switch (dateType)
            {
                case DateIntervalType.Month:
                    return new DateTime(date.Year, date.Month, 1);
                case DateIntervalType.Quarter:
                    switch (date.Month)
                    {
                        case 1:
                        case 2:
                        case 3:
                            return new DateTime(date.Year, 1, 1);
                        case 4:
                        case 5:
                        case 6:
                            return new DateTime(date.Year, 4, 1);
                        case 7:
                        case 8:
                        case 9:
                            return new DateTime(date.Year, 7, 1);
                        default:
                            return new DateTime(date.Year, 10, 1);
                    }
                    case DateIntervalType.Year:
                        return new DateTime(date.Year, 1, 1);
                default:
                    throw new NotSupportedException("This DateIntervalType not supported.");
            }
        }

        /// <summary>
        /// Convert DateTime to DateInterval according to DateInterval type
        /// </summary>
        /// <param name="date"> Date </param>
        /// <param name="dateType"> DateInterval type </param>
        /// <returns> DateInterval </returns>
        public static DateInterval GetDateInterval(this DateTime date, DateIntervalType dateType)
        {
            if (!date.IsStartOfIntervalType(dateType))
            {
                return new DateInterval(date.Floor(dateType), date.Ceil(dateType));
            }

            return new DateInterval(date, date.AddByType(dateType));
        }
    }
}