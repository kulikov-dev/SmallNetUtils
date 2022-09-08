using System;
using System.Collections.Generic;
using SmallNetUtils.Data;
using SmallNetUtils.Extensions;

namespace SmallNetUtils.Utils
{
    /// <summary>
    /// Utils to work with NumsRange
    /// </summary>
    public static class NumsRangeUtil
    {
        public static int Accuracy = 3;

        /// <summary>
        /// Create list of NumsRange by specific creation type
        /// </summary>
        /// <param name="creationRule"> Rule for creation list </param>
        /// <param name="min"> Setting number 1 (depends on creation type) </param>
        /// <param name="max"> Setting number 2 (depends on creation type) </param>
        /// <param name="setting3"> Setting number 2 (depends on creation type) </param>
        /// <returns> List of NumsRange </returns>
        /// <exception cref="ArgumentOutOfRangeException"> Not supported creation type </exception>
        public static List<NumsRange> CreateRanges(RangesCreationRules creationRule, double min = double.NaN, double max = double.NaN, double setting3 = double.NaN)
        {
            switch (creationRule)
            {
                case RangesCreationRules.ByNumsRangesCount:
                    return CreateNumRangesByCount(min, max, (int)setting3);
                case RangesCreationRules.ByNumsRangeSize:
                    return CreateNumRangesBySize(min, max, setting3);
                default:
                    throw new ArgumentOutOfRangeException(nameof(creationRule), creationRule, null);
            }
        }

        /// <summary>
        /// Create list of NumsRange by a range size
        /// </summary>
        /// <param name="min"> Min value </param>
        /// <param name="max"> Max value </param>
        /// <param name="size"> NumsRange size </param>
        /// <returns> List of NumsRange </returns>
        public static List<NumsRange> CreateNumRangesBySize(double min, double max, double size)
        {
            var ranges = new List<NumsRange>();

            if (min == double.MinValue || max == double.MaxValue)
            {
                return ranges;
            }

            min = Math.Round(min, Accuracy);
            max = Math.Round(max, Accuracy);
            var curValue = min;

            min = Math.Min(min, max);
            max = Math.Max(curValue, max);
            curValue = min;

            if (min.EqualsEpsilon(max))
            {
                return ranges;
            }

            while (Math.Round(curValue, Accuracy) < max)
            {
                ranges.Add(new NumsRange(curValue, curValue + size > max ? max : curValue + size));
                curValue += size;
            }

            return ranges;
        }

        /// <summary>
        /// Create list of NumsRange by a ranges amount
        /// </summary>
        /// <param name="min"> Min value </param>
        /// <param name="max"> Max value </param>
        /// <param name="amount"> Ranges amount </param>
        /// <returns> List of NumsRange </returns>
        public static List<NumsRange> CreateNumRangesByCount(double min, double max, int amount)
        {
            var ranges = new List<NumsRange>(amount);

            if (min == double.MinValue || max == double.MaxValue)
            {
                return ranges;
            }

            if (min.EqualsEpsilonNaN(max))
            {
                return ranges;
            }

            var size = (max - min) / amount;

            return CreateNumRangesBySize(min, max, size);
        }

        /// <summary>
        /// Rules for list of NumsRange creation
        /// </summary>
        public enum RangesCreationRules
        {
            /// <summary>
            /// By intervals count
            /// </summary>
            ByNumsRangesCount,

            /// <summary>
            /// By NumsRange size
            /// </summary>
            ByNumsRangeSize
        }
    }
}