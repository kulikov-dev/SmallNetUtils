using System;
using System.Collections.Generic;
using System.Linq;
using SmallNetUtils.Data;

namespace SmallNetUtils.Utils
{
    /// <summary>
    /// Utils to work with DateTime and DateInterval
    /// </summary>
    public static class DateTimeUtil
    {
        /// <summary>
        /// Get smaller date
        /// </summary>
        /// <param name="date1"> Date 1 </param>
        /// <param name="date2"> Date 2 </param>
        /// <returns> Minimum date </returns>
        public static DateTime Min(DateTime date1, DateTime date2)
        {
            return date1 < date2 ? date1 : date2;
        }

        /// <summary>
        /// Get bigger date
        /// </summary>
        /// <param name="date1"> Date 1 </param>
        /// <param name="date2"> Date 2 </param>
        /// <returns> Maximum date </returns>
        public static DateTime Max(DateTime date1, DateTime date2)
        {
            return date1 > date2 ? date1 : date2;
        }

        /// <summary>
        /// Get difference between two dates in months
        /// </summary>
        /// <param name="date1"> Date 1 </param>
        /// <param name="date2"> Date 2 </param>
        /// <returns> Difference in months between two dates </returns>
        public static int MonthsBetween(DateTime date1, DateTime date2)
        {
            return date1.Year * 12 + date1.Month - (date2.Year * 12 + date2.Month);
        }

        /// <summary>
        /// Concat two lists of DateTime
        /// </summary>
        /// <param name="item1"> First list </param>
        /// <param name="item2"> Second list </param>
        /// <returns> Merged list </returns>
        public static DateTime[] Concat(IEnumerable<DateTime> item1, IEnumerable<DateTime> item2)
        {
            return item1.Concat(item2).Distinct().OrderBy(date => date).ToArray();
        }

        /// <summary>
        /// Convert list of DateInterval to list of DateTime
        /// </summary>
        /// <param name="dateIntervals"> List of DateInterval </param>
        /// <returns> List of DateTime </returns>
        public static IEnumerable<DateTime> ConvertDateIntervalsToDateTime(IEnumerable<DateInterval> dateIntervals)
        {
            var enumerable = dateIntervals as DateInterval[] ?? dateIntervals.ToArray();
            var begins = new HashSet<DateTime>();
            var ends = new HashSet<DateTime>();

            foreach (var interval in enumerable)
            {
                if (interval.IsEmpty)
                {
                    continue;
                }

                begins.Add(interval.Begin);
                ends.Add(interval.End);
            }

            return Concat(begins, ends);
        }

        /// <summary>
        /// Convert list of DateTime to list of DateInterval
        /// </summary>
        /// <param name="dates"> List of DateTime </param>
        /// <returns> List of DateInterval </returns>
        public static DateInterval[] ConvertDateTimeToDateIntervals(DateTime[] dates)
        {
            if (dates.Length < 2)
            {
                return Array.Empty<DateInterval>();
            }

            var result = new DateInterval[dates.Length - 1];

            for (var i = 0; i < dates.Length - 1; i++)
            {
                result[i] = new DateInterval(dates[i], dates[i + 1]);
            }

            return result;
        }

        /// <summary>
        /// Merge consecutive dates into one DateInterval
        /// </summary>
        /// <param name="dateIntervals"> List of DateInterval </param>
        /// <returns> List of DateInterval with merged consecutive dates </returns>
        public static List<DateInterval> Merge(List<DateInterval> dateIntervals)
        {
            if (dateIntervals.Count < 2)
            {
                return dateIntervals;
            }

            var sortedIntervals = dateIntervals.OrderBy(interval => interval.Begin).ToList();
            var result = new List<DateInterval>();
            var tempInterval = new DateInterval();

            for (var i = 0; i < sortedIntervals.Count - 1; ++i)
            {
                var currentInterval = sortedIntervals[i];
                var nextInterval = sortedIntervals[i + 1];

                if (tempInterval.IsEmpty)
                {
                    tempInterval = currentInterval;
                }

                if (currentInterval.End != nextInterval.Begin)
                {
                    result.Add(tempInterval);

                    tempInterval = new DateInterval();
                }
                else
                {
                    tempInterval = new DateInterval(tempInterval.Begin, nextInterval.End);
                }
            }

            result.Add(!tempInterval.IsEmpty ? tempInterval : sortedIntervals.Last());

            return result;
        }
    }
}