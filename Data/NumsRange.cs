using System.Globalization;
using SmallNetUtils.Extensions;

namespace SmallNetUtils.Data
{
    /// <summary>
    /// Class to store double range min-max
    /// </summary>
    [Serializable]
    public struct NumsRange : IComparable
    {
        /// <summary>
        /// Empty value
        /// </summary>
        public static readonly NumsRange Empty = new();

        /// <summary>
        /// Constructor
        /// </summary>
        public NumsRange()
        {
            Min = double.MinValue;
            Max = double.MaxValue;
        }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="min"> Min range value </param>
        /// <param name="max"> Max range value </param>
        public NumsRange(double min, double max)
        {
            Min = Math.Min(min, max);
            Max = Math.Max(min, max);
        }

        /// <summary>
        /// Min range value
        /// </summary>
        public double Min { get; set; }

        /// <summary>
        /// Max range value
        /// </summary>
        public double Max { get; set; }

        /// <summary>
        /// Flag if empty
        /// </summary>
        public bool IsEmpty => this == Empty;

        /// <summary>
        /// Range length
        /// </summary>
        public double Length => Max - Min;

        /// <summary>
        /// Overload ==
        /// </summary>
        /// <param name="item1"> First range </param>
        /// <param name="item2"> Second range </param>
        /// <returns> Flag if ranges are equal </returns>
        public static bool operator ==(NumsRange item1, NumsRange item2)
        {
            return item1.Min.EqualsEpsilon(item2.Min) && item1.Max.EqualsEpsilon(item2.Max);
        }

        /// <summary>
        /// Overload !=
        /// </summary>
        /// <param name="item1"> First range </param>
        /// <param name="item2"> Second range </param>
        /// <returns> Flag if ranges are not equal </returns>
        public static bool operator !=(NumsRange item1, NumsRange item2)
        {
            return !(item1 == item2);
        }

        /// <summary>
        /// Check if two ranges are equal
        /// </summary>
        /// <param name="other"> Second range </param>
        /// <returns> Flag if equals </returns>
        public bool Equals(NumsRange other)
        {
            return Min.Equals(other.Min) && Max.Equals(other.Max);
        }

        /// <summary>
        /// Check if two ranges are equal
        /// </summary>
        /// <param name="obj"> Second range </param>
        /// <returns> Flag if equals </returns>
        public override bool Equals(object? obj)
        {
            return obj is NumsRange other && Equals(other);
        }

        /// <summary>
        /// Get struct hashcode
        /// </summary>
        /// <returns> Hashcode </returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Min, Max);
        }

        /// <summary>
        /// Compare two ranges
        /// </summary>
        /// <param name="obj"> Second range </param>
        /// <returns> Compare result </returns>
        public int CompareTo(object? obj)
        {
            if (obj == null)
            {
                return 1;
            }

            var value = (NumsRange)obj;

            if (Max <= value.Min)
            {
                return -1;
            }

            return Min > value.Max ? 1 : 0;
        }

        /// <summary>
        /// Check if range contains number
        /// </summary>
        /// <param name="value"> Number </param>
        /// <returns> Flag if contains </returns>
        public bool Contains(double value)
        {
            if (double.IsNaN(value))
            {
                return false;
            }

            return Min <= value && value <= Max;
        }

        /// <summary>
        /// Get string representation of a struct
        /// </summary>
        /// <returns> String representation </returns>
        public override string ToString()
        {
            return ToString(2);
        }

        /// <summary>
        /// Get string representation of a struct
        /// </summary>
        /// <param name="format"> Display format </param>
        /// <returns> String representation </returns>
        public string ToString(string format)
        {
            return (Min == double.MinValue ? "<" : Min.ToString(format)) + " - " + (Max == double.MaxValue ? ">" : Max.ToString(format));
        }

        /// <summary>
        /// Get string representation of a struct
        /// </summary>
        /// <param name="precision"> Precision of numbers in a range </param>
        /// <returns> String representation </returns>
        public string ToString(int precision)
        {
            var min = Min == double.MinValue ? "<" : Math.Round(Min, precision >= 0 ? precision : 0, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture);
            var max = Max == double.MaxValue ? ">" : Math.Round(Max, precision >= 0 ? precision : 0, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture);

            return min + " - " + max;
        }
    }
}