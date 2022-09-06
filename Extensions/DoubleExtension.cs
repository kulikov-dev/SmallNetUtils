using System.Globalization;

namespace SmallNetUtils.Extensions
{
    /// <summary>
    /// Extensions for Double
    /// </summary>
    public static class DoubleExtension
    {
        /// <summary>
        /// Validate value (not nan or infinity), if not valid - return default value
        /// </summary>
        /// <param name="value"> Double </param>
        /// <param name="defaultValue"> Default value if has no data </param>
        /// <returns> Valid double </returns>
        public static double Validate(this double value, double defaultValue = double.NaN)
        {
            return value.HasActualValue() ? value : defaultValue;
        }

        /// <summary>
        /// Validate value (not nan or infinity), if not valid - return string default value
        /// </summary>
        /// <param name="value"> Double </param>
        /// <param name="defaultValue"> Default string if has no data </param>
        /// <returns> Valid data </returns>
        /// <remarks> Useful to work with Excel/Word OLE </remarks>
        public static object Validate(this double value, string defaultValue)
        {
            return value.HasActualValue() ? value : defaultValue;
        }

        /// <summary>
        /// Get string representation of double
        /// </summary>
        /// <param name="value"> Double </param>
        /// <param name="provider"> FormatInfo provider </param>
        /// <param name="defaultValue"> Default string if has no data </param>
        /// <returns> String representation </returns>
        public static string ToString(this double value, NumberFormatInfo provider, string defaultValue)
        {
            return value.ToString(defaultValue, string.Empty, provider);
        }

        /// <summary>
        /// Get string representation of double
        /// </summary>
        /// <param name="value"> Double </param>
        /// <param name="defaultValue"> Default string if has no data </param>
        /// <param name="format"> Number format </param>
        /// <returns> String representation </returns>
        public static string ToString(this double value, string defaultValue, string format = "")
        {
            return value.ToString(defaultValue, format, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Get string representation of double
        /// </summary>
        /// <param name="value"> Double </param>
        /// <param name="defaultValue"> Default string if has no data </param>
        /// <param name="format"> Number format </param>
        /// <param name="provider"> FormatInfo provider </param>
        /// <returns> String representation </returns>
        public static string ToString(this double value, string defaultValue, string format, NumberFormatInfo provider)
        {
            return value.HasActualValue() ? value.ToString(format, provider) : defaultValue;
        }

        /// <summary>
        /// Equal two numbers with epsilon
        /// </summary>
        /// <param name="item1"> Item 1 </param>
        /// <param name="item2"> Item 2</param>
        /// <param name="eps"> Epsilon </param>
        /// <returns> Flag if equal </returns>
        /// <remarks> Floating-point comparison </remarks>
        public static bool EqualsEpsilon(this double item1, double item2, double eps = double.Epsilon)
        {
            return Math.Abs(item1 - item2) < eps;
        }

        /// <summary>
        /// Equal two numbers with epsilon. Accept double.NaN
        /// </summary>
        /// <param name="item1"> Item 1 </param>
        /// <param name="item2"> Item 2</param>
        /// <param name="eps"> Epsilon </param>
        /// <returns> Flag if equal </returns>
        /// <remarks> Floating-point comparison </remarks>
        public static bool EqualsEpsilonNaN(this double item1, double item2, double eps = double.Epsilon)
        {
            if (double.IsNaN(item1) && double.IsNaN(item2))
            {
                return true;
            }

            return EqualsEpsilon(item1, item2, eps);
        }

        /// <summary>
        /// Equal two numbers with epsilon. Accept double.NaN, infinities
        /// </summary>
        /// <param name="item1"> Item 1 </param>
        /// <param name="item2"> Item 2</param>
        /// <param name="eps"> Epsilon </param>
        /// <returns> Flag if equal </returns>
        /// <remarks> Floating-point comparison </remarks>
        public static bool EqualsEpsilonNanInf(this double item1, double item2, double eps = double.Epsilon)
        {
            if (double.IsNegativeInfinity(item1) && double.IsNegativeInfinity(item2))
            {
                return true;
            }

            if (double.IsPositiveInfinity(item1) && double.IsPositiveInfinity(item2))
            {
                return true;
            }

            return EqualsEpsilonNaN(item1, item2, eps);
        }

        /// <summary>
        /// Round number up to significant digits
        /// </summary>
        /// <param name="value"> Double </param>
        /// <param name="digits"> Number of significant digits </param>
        /// <returns> Rounded double </returns>
        public static double RoundToSignificant(this double value, int digits)
        {
            if (value == 0)
            {
                return 0;
            }

            var scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(value))) + 1);

            return scale * Math.Round(value / scale, digits);
        }

        /// <summary>
        /// Check if value has data (not nan or infinity)
        /// </summary>
        /// <param name="value"> Value </param>
        /// <returns> Flag if has data </returns>
        private static bool HasActualValue(this double value)
        {
            return !double.IsNaN(value) && !double.IsInfinity(value);
        }
    }
}