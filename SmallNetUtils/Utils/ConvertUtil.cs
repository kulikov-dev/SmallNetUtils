using System.Globalization;

namespace SmallNetUtils.Utils
{
    /// <summary>
    /// Some useful methods to extend System.Convert library
    /// </summary>
    public static class ConvertUtil
    {
        /// <summary>
        /// Cached NumberFormatInfo
        /// </summary>
        private static readonly NumberFormatInfo CachedProvider;

        /// <summary>
        /// Trim symbols with processing specific Unicode bad symbols
        /// </summary>
        private static readonly char[] TrimSymbols = { ' ', '\t', '\uFEFF', '\u200B' };

        /// <summary>
        /// Parameter-less constructor
        /// </summary>
        static ConvertUtil()
        {
            CachedProvider = NumberFormatInfo.CurrentInfo;
        }

        /// <summary>
        /// Convert an object to string
        /// </summary>
        /// <param name="objectValue"> Object </param>
        /// <param name="defaultStringValue"> Default objectValue for a null object </param>
        /// <returns> String representation of an object </returns>
        public static string ToString(object? objectValue, string defaultStringValue = "")
        {
            return (objectValue is null or DBNull ? defaultStringValue : objectValue.ToString()) ?? string.Empty;
        }

        /// <summary>
        /// Convert an object to boolean
        /// </summary>
        /// <param name="objectValue"> Object </param>
        /// <returns> Boolean value </returns>
        /// <remarks> Support different representation of 'true' value like 1, YES, Y </remarks>
        public static bool ToBool(object? objectValue)
        {
            return objectValue switch
            {
                null => false,
                bool b => b,
                DBNull => false,
                _ => ToBool(objectValue.ToString())
            };
        }

        /// <summary>
        /// Convert a string to boolean
        /// </summary>
        /// <param name="stringValue"> String value </param>
        /// <returns> Boolean value </returns>
        /// <remarks> Support different representation of 'true' value like 1, YES, Y. Process unicode BOM symbols </remarks>
        public static bool ToBool(string? stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return false;
            }

            var clearValue = stringValue.Trim(TrimSymbols).ToUpperInvariant();

            return clearValue == bool.TrueString || clearValue is "1" or "YES" or "Y" or "T";
        }

        /// <summary>
        /// Convert a string value to DateTime
        /// </summary>
        /// <param name="stringValue"> String value </param>
        /// <returns> DateTime </returns>
        /// <remarks> Support both default string representation of DateTime and OLE automation date (Excel). Process unicode BOM symbols </remarks>
        public static DateTime ToDateTime(string? stringValue)
        {
            return ToDateTime(stringValue, DateTime.MinValue);
        }

        /// <summary>
        /// Convert an object to DateTime
        /// </summary>
        /// <param name="objectValue"> Object </param>
        /// <param name="outValue"> DateTime </param>
        /// <returns> Flag of successful parsing </returns>
        /// <remarks> Support both default string representation of DateTime and OLE automation date (Excel). Process unicode BOM symbols, DBull values </remarks>
        public static bool ToDateTime(object? objectValue, out DateTime outValue)
        {
            return ToDateTime(objectValue, DateTime.MinValue, out outValue);
        }

        /// <summary>
        /// Convert an object to DateTime
        /// </summary>
        /// <param name="objectValue"> Object </param>
        /// <param name="defaultValue"> Default DateTime value </param>
        /// <returns> DateTime </returns>
        /// <remarks> Support both default string representation of DateTime and OLE automation date (Excel). Process unicode BOM symbols, DBull values </remarks>
        public static DateTime ToDateTime(object? objectValue, DateTime defaultValue)
        {
            ToDateTime(objectValue, defaultValue, out var temp);
            return temp;
        }

        /// <summary>
        /// Convert an object to DateTime
        /// </summary>
        /// <param name="objectValue"> Object </param>
        /// <param name="defaultValue"> Default DateTime value </param>
        /// <param name="outValue"> DateTime </param>
        /// <returns> Flag of successful parsing </returns>
        /// <remarks> Support both default string representation of DateTime and OLE automation date (Excel). Process unicode BOM symbols, DBull values </remarks>
        public static bool ToDateTime(object? objectValue, DateTime defaultValue, out DateTime outValue)
        {
            switch (objectValue)
            {
                case DateTime dateTime:

                    outValue = dateTime;

                    return true;
                case DBNull:

                    outValue = defaultValue;

                    return false;
                default:

                    outValue = defaultValue;

                    return objectValue != null && ToDateTime(objectValue.ToString(), out outValue);
            }
        }

        /// <summary>
        /// Convert a string value to DateTime
        /// </summary>
        /// <param name="stringValue"> String value </param>
        /// <param name="defaultValue"> Default DateTime value </param>
        /// <returns> DateTime </returns>
        /// <remarks> Support both default string representation of DateTime and OLE automation date (Excel). Process unicode BOM symbols </remarks>
        public static DateTime ToDateTime(string? stringValue, DateTime defaultValue)
        {
            ToDateTime(stringValue, defaultValue, out var temp);
            return temp;
        }

        /// <summary>
        /// Convert a string value to DateTime
        /// </summary>
        /// <param name="stringValue"> String value </param>
        /// <param name="outValue"> DateTime </param>
        /// <returns> Flag of successful parsing </returns>
        /// <remarks> Support both default string representation of DateTime and OLE automation date (Excel). Process unicode BOM symbols </remarks>
        public static bool ToDateTime(string? stringValue, out DateTime outValue)
        {
            return ToDateTime(stringValue, DateTime.MinValue, out outValue);
        }

        /// <summary>
        /// Convert a string value to DateTime
        /// </summary>
        /// <param name="stringValue"> String value </param>
        /// <param name="defaultValue"> Default DateTime value </param>
        /// <param name="outValue"> DateTime </param>
        /// <returns> Flag of successful parsing </returns>
        /// <remarks> Support both default string representation of DateTime and OLE automation date (Excel). Process unicode BOM symbols </remarks>
        public static bool ToDateTime(string? stringValue, DateTime defaultValue, out DateTime outValue)
        {
            outValue = defaultValue;

            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return false;
            }

            var newSeparator = CachedProvider.NumberDecimalSeparator;
            var oldSeparator = newSeparator == "," ? "." : ",";
            stringValue = stringValue.Replace(oldSeparator, newSeparator).Trim(TrimSymbols);

            if (DateTime.TryParse(stringValue, CachedProvider, DateTimeStyles.None, out var result))
            {
                outValue = result;

                return true;
            }

            var oleDate = ToDouble(stringValue);
            outValue = double.IsNaN(oleDate) ? defaultValue : DateTime.FromOADate(oleDate);

            return !double.IsNaN(oleDate);
        }

        /// <summary>
        /// Convert string value to double
        /// </summary>
        /// <param name="stringValue"> String value </param>
        /// <returns> Double </returns>
        /// <remarks> Parsing with supports of CurrentFormatInfo. Process unicode BOM symbols </remarks>
        public static double ToDouble(string? stringValue)
        {
            return ToDouble(stringValue, double.NaN);
        }

        /// <summary>
        /// Convert object value to double
        /// </summary>
        /// <param name="objectValue"> Object value </param>
        /// <returns> Double value </returns>
        /// <remarks> Parsing with supports of CurrentFormatInfo. Process unicode BOM symbols, DBull values </remarks>
        public static double ToDouble(object? objectValue)
        {
            return ToDouble(objectValue, double.NaN);
        }

        /// <summary>
        /// Convert object value to double
        /// </summary>
        /// <param name="objectValue"> Object value </param>
        /// <param name="defaultValue"> Default double value </param>
        /// <returns> Double value </returns>
        /// <remarks> Parsing with supports of CurrentFormatInfo. Process unicode BOM symbols, DBull values </remarks>
        public static double ToDouble(object? objectValue, double defaultValue)
        {
            ToDouble(objectValue, defaultValue, out var temp);
            return temp;
        }

        /// <summary>
        /// Convert object value to double
        /// </summary>
        /// <param name="objectValue"> Object value </param>
        /// <param name="outValue"> Double </param>
        /// <returns> Flag of successful parsing </returns>
        /// <remarks> Parsing with supports of CurrentFormatInfo. Process unicode BOM symbols, DBull values </remarks>
        public static bool ToDouble(object? objectValue, out double outValue)
        {
            return ToDouble(objectValue, double.NaN, out outValue);
        }

        /// <summary>
        /// Convert object value to double
        /// </summary>
        /// <param name="objectValue"> Object value </param>
        /// <param name="defaultValue"> Default double value </param>
        /// <param name="outValue"> Double </param>
        /// <returns> Flag of successful parsing </returns>
        /// <remarks> Parsing with supports of CurrentFormatInfo. Process unicode BOM symbols, DBull values </remarks>
        public static bool ToDouble(object? objectValue, double defaultValue, out double outValue)
        {
            outValue = defaultValue;

            switch (objectValue)
            {
                case double doubleValue:

                    outValue = doubleValue;

                    return true;

                case DBNull:
                    return false;

                default:
                    return objectValue != null && ToDouble(objectValue.ToString(), defaultValue, out outValue);
            }
        }

        /// <summary>
        /// Convert string value to double
        /// </summary>
        /// <param name="stringValue"> String value </param>
        /// <param name="defaultValue"> Default double value </param>
        /// <returns> Double </returns>
        /// <remarks> Parsing with supports of CurrentFormatInfo. Process unicode BOM symbols </remarks>
        public static double ToDouble(string? stringValue, double defaultValue)
        {
            ToDouble(stringValue, defaultValue, out var temp);
            return temp;
        }

        /// <summary>
        /// Convert string value to double
        /// </summary>
        /// <param name="stringValue"> String value </param>
        /// <param name="outValue"> Double </param>
        /// <returns> Flag of successful parsing </returns>
        /// <remarks> Parsing with supports of CurrentFormatInfo. Process unicode BOM symbols </remarks>
        public static bool ToDouble(string? stringValue, out double outValue)
        {
            return ToDouble(stringValue, double.NaN, out outValue);
        }

        /// <summary>
        /// Convert string value to double
        /// </summary>
        /// <param name="stringValue"> String value </param>
        /// <param name="defaultValue"> Default double value </param>
        /// <param name="outValue"> Double </param>
        /// <returns> Flag of successful parsing </returns>
        /// <remarks> Parsing with supports of CurrentFormatInfo. Process unicode BOM symbols </remarks>
        public static bool ToDouble(string? stringValue, double defaultValue, out double outValue)
        {
            outValue = defaultValue;

            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return false;
            }

            var newSeparator = CachedProvider.NumberDecimalSeparator;
            var oldSeparator = newSeparator == "," ? "." : ",";
            stringValue = stringValue.Replace(oldSeparator, newSeparator).Trim(TrimSymbols);

            if (!double.TryParse(stringValue, NumberStyles.Any, CachedProvider, out var result))
            {
                return false;
            }

            outValue = result;

            return true;
        }

        /// <summary>
        /// Convert string string to string of double in specific format
        /// </summary>
        /// <param name="stringValue"> String value </param>
        /// <param name="defaultValue"> Default string value </param>
        /// <param name="format"> String number format </param>
        /// <returns> String of double </returns>
        /// <remarks> Parsing and printing with supports of CurrentFormatInfo. Process unicode BOM symbols </remarks>
        public static string ToDoubleString(string stringValue, string defaultValue, string format = "")
        {
            return ToDoubleString((object)stringValue, defaultValue, format);
        }

        /// <summary>
        /// Convert object value to string of double in specific format
        /// </summary>
        /// <param name="objectValue"> Object value </param>
        /// <param name="defaultValue"> Default string value </param>
        /// <param name="format"> String number format </param>
        /// <returns> String of double </returns>
        /// <remarks> Parsing and printing with supports of CurrentFormatInfo. Process unicode BOM symbols, DBNull values </remarks>
        public static string ToDoubleString(object objectValue, string defaultValue, string format = "")
        {
            var result = ToDouble(objectValue, out var temp);

            return result ? temp.ToString(format, CachedProvider) : defaultValue;
        }

        /// <summary>
        /// Convert string value to int
        /// </summary>
        /// <param name="stringValue"> String value </param>
        /// <param name="defaultValue"> Default int value </param>
        /// <returns> Int </returns>
        public static int ToInt(string? stringValue, int defaultValue = int.MinValue)
        {
            ToInt(stringValue, defaultValue, out var temp);
            return temp;
        }

        /// <summary>
        /// Convert object value to int
        /// </summary>
        /// <param name="objectValue"> Object value </param>
        /// <param name="defaultValue"> Default int value </param>
        /// <returns> Int </returns>
        public static int ToInt(object? objectValue, int defaultValue = int.MinValue)
        {
            ToInt(objectValue, defaultValue, out var temp);
            return temp;
        }

        /// <summary>
        /// Convert object value to int
        /// </summary>
        /// <param name="objectValue"> Object value </param>
        /// <param name="outValue"> Int </param>
        /// <returns> Flag of successful parsing </returns>
        public static bool ToInt(object? objectValue, out int outValue)
        {
            return ToInt(objectValue, int.MinValue, out outValue);
        }

        /// <summary>
        /// Convert object value to int
        /// </summary>
        /// <param name="objectValue"> Object value </param>
        /// <param name="defaultValue"> Default int value </param>
        /// <param name="outValue"> Int </param>
        /// <returns> Flag of successful parsing </returns>
        public static bool ToInt(object? objectValue, int defaultValue, out int outValue)
        {
            outValue = defaultValue;

            switch (objectValue)
            {
                case int intValue:

                    outValue = intValue;

                    return true;

                case DBNull:
                    return false;

                default:
                    return objectValue != null && ToInt(objectValue.ToString(), defaultValue, out outValue);
            }
        }

        /// <summary>
        /// Convert string value to int
        /// </summary>
        /// <param name="stringValue"> String value </param>
        /// <param name="defaultValue"> Default int value </param>
        /// <param name="outValue"> Int </param>
        /// <returns> Flag of successful parsing </returns>
        public static bool ToInt(string? stringValue, int defaultValue, out int outValue)
        {
            outValue = defaultValue;

            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return false;
            }

            if (!int.TryParse(stringValue, NumberStyles.Any, CachedProvider, out var result))
            {
                return false;
            }

            outValue = result;

            return true;
        }
    }
}