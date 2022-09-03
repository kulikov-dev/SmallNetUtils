using System.ComponentModel;

namespace SmallNetUtils.Extensions
{
    /// <summary>
    /// Extension for Enums
    /// </summary>
    /// <remarks> 
    /// Allows to get caption/enum value by Description Attribute
    /// </remarks>
    public static class EnumExtension
    {
        /// <summary>
        /// Get an enum caption by DescriptionAttribute
        /// </summary>
        /// <typeparam name="T"> Description attribute </typeparam>
        /// <param name="value"> Enum value </param>
        /// <returns> A caption </returns>
        public static string GetCaption<T>(this Enum value)
            where T : DescriptionAttribute
        {
            var attribute = value.GetAttribute<T>();

            return attribute == null ? value.ToString() : attribute.Description;
        }

        /// <summary>
        /// Get an enum caption by DescriptionAttribute
        /// </summary>
        /// <param name="value"> Enum value </param>
        /// <returns> Caption </returns>
        public static string GetCaption(this Enum value)
        {
            return GetCaption<DescriptionAttribute>(value);
        }

        /// <summary>
        /// Get an enum value by the caption
        /// </summary>
        /// <typeparam name="T"> Description attribute </typeparam>
        /// <param name="type"> Enum type </param>
        /// <param name="caption"> Enum value caption </param>
        /// <param name="defaultValue"> Default value </param>
        /// <returns> An enum value </returns>
        /// <exception cref="InvalidEnumArgumentException"> Exception on not found enum by caption </exception>
        public static object FromCaption<T>(Type type, string caption, object? defaultValue = null)
            where T : DescriptionAttribute
        {
            if (TryGetCaption<T>(type, caption, out var value))
            {
                return value;
            }

            if (defaultValue == null)
            {
                throw new InvalidEnumArgumentException($"Can't find a caption '{caption}' for the enum '{type}'.");
            }

            return defaultValue;
        }

        /// <summary>
        /// Get an enum value by the caption
        /// </summary>
        /// <typeparam name="T"> Enum type </typeparam>
        /// <param name="caption"> Enum value caption </param>
        /// <returns> An enum value </returns>
        public static T FromCaption<T>(string caption)
            where T : struct, IConvertible
        {
            return (T)FromCaption<DescriptionAttribute>(typeof(T), caption);
        }

        /// <summary>
        /// Get an enum value by the caption
        /// </summary>
        /// <typeparam name="T"> Enum type </typeparam>
        /// <param name="caption"> Enum value caption </param>
        /// <param name="defaultValue"> Default value </param>
        /// <returns> An enum value </returns>
        public static T FromCaption<T>(string caption, T defaultValue)
            where T : struct, IConvertible
        {
            return (T)FromCaption<DescriptionAttribute>(typeof(T), caption, defaultValue);
        }

        /// <summary>
        /// Get all captions for the enum values
        /// </summary>
        /// <typeparam name="T"> Description attribute </typeparam>
        /// <param name="type"> Enum type </param>
        /// <returns> List of enum captions </returns>
        public static List<string> GetAllCaptions<T>(Type type)
            where T : DescriptionAttribute
        {
            var result = new List<string>();

            foreach (var value in Enum.GetValues(type))
            {
                result.Add(((Enum)value).GetCaption<T>());
            }

            return result;
        }

        /// <summary>
        /// Get all captions for the enum values
        /// </summary>
        /// <typeparam name="T"> Enum type </typeparam>
        /// <returns> List of enum captions </returns>
        public static List<string> GetAllCaptions<T>()
            where T : struct, IConvertible
        {
            return GetAllCaptions<DescriptionAttribute>(typeof(T));
        }

        /// <summary>
        /// Get enum attribute
        /// </summary>
        /// <typeparam name="T"> Attribute type </typeparam>
        /// <param name="value"> Enum value </param>
        /// <returns> Attribute </returns>
        private static T GetAttribute<T>(this Enum value)
            where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);

            return (T)attributes[0];
        }

        /// <summary>
        /// Get an enum value by the caption
        /// </summary>
        /// <typeparam name="T"> Description attribute </typeparam>
        /// <param name="type"> Enum type </param>
        /// <param name="caption"> Enum value caption </param>
        /// <param name="value"> Enum value </param>
        /// <returns> Flag if success </returns>
        private static bool TryGetCaption<T>(Type type, string caption, out object value)
            where T : DescriptionAttribute
        {
            var clearCaption = string.IsNullOrWhiteSpace(caption) ? string.Empty : caption.Trim();
            value = null;

            foreach (Enum enumValue in Enum.GetValues(type))
            {
                var enumCaption = enumValue.GetCaption<T>();

                if (clearCaption.Equals(enumCaption, StringComparison.OrdinalIgnoreCase))
                {
                    value = enumValue;

                    return true;
                }
            }

            return false;
        }
    }
}