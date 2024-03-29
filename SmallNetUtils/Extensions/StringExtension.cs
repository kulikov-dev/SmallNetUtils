﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SmallNetUtils.Utils;

namespace SmallNetUtils.Extensions
{
    /// <summary>
    /// String extension
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Common ToString method with possibility to change empty value to default text
        /// </summary>
        /// <param name="input"> Input string </param>
        /// <param name="defaultValue"> Default value </param>
        /// <returns> To string </returns>
        public static string ToString(this string input, string defaultValue)
        {
            return ConvertUtil.ToString(input, defaultValue);
        }

        /// <summary>
        /// Change first char of input text to Upper
        /// </summary>
        /// <param name="text"> Input text </param>
        /// <returns> Input text with first Upper char </returns>
        public static string FirstCharToUpper(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            return text[0].ToString().ToUpper() + text.Substring(1).ToLower();
        }

        /// <summary>
        /// Wrap string with HTML tag
        /// </summary>
        /// <param name="input"> Input string </param>
        /// <param name="htmlTag"> Tag </param>
        /// <returns> Wrapped string </returns>
        public static string AddHtmlTag(this string input, string htmlTag)
        {
            return $"<{htmlTag}>{input}</{htmlTag}>";
        }

        /// <summary>
        /// Remove all HTML tags from string
        /// </summary>
        /// <param name="input"> Input string </param>
        /// <returns> String without HTML tags </returns>
        public static string RemoveHtmlTags(this string input)
        {
            return Regex.Replace(input, "<.*?>", string.Empty);
        }

        /// <summary>
        /// Convert input text to multiline length with lines of specific row length. Preserve words
        /// </summary>
        /// <param name="input"> Input text </param>
        /// <param name="lineLength"> Line length </param>
        /// <returns> Multiline </returns>
        public static string CreateMultiLineByLength(this string input, int lineLength)
        {
            if (lineLength <= 0)
            {
                throw new ArgumentException("Wrong line length.");
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            var result = new StringBuilder(input.Length);
            var line = new StringBuilder();

            var stack = new Stack<string>(input.Split(' '));

            while (stack.Count > 0)
            {
                var word = stack.Pop();

                if (word.Length > lineLength)
                {
                    var head = word.Substring(0, lineLength);
                    var tail = word.Substring(lineLength);

                    word = head;

                    stack.Push(tail);
                }

                if (line.Length + word.Length > lineLength)
                {
                    result.AppendLine(line.ToString());
                    line.Clear();
                }

                line.Append(word + " ");
            }

            result.Append(line);
            return result.ToString();
        }

        /// <summary>
        /// Convert input text to multiline separated by delimiters.
        /// </summary>
        /// <param name="input"> Input text </param>
        /// <param name="delimiters"> Delimiters </param>
        /// <returns> Multiline </returns>
        public static string CreateMultiLineByDelimiters(this string input, IEnumerable<string> delimiters = null)
        {
            if (delimiters == null || !delimiters.Any())
            {
                return input;
            }

            var escapedDelimeters = delimiters.Select(Regex.Escape);
            var regexWithDelimeters = string.Join("|", escapedDelimeters);
            var splitterRegex = $"(?<={regexWithDelimeters})";
            var lines = Regex.Split(input, splitterRegex).Where(line => !string.IsNullOrWhiteSpace(line));

            return string.Join(Environment.NewLine, lines.ToArray());
        }

        /// <summary>
        /// Allow to append new line to a StringBuilder with delimeter (like spaces)
        /// </summary>
        /// <param name="stringBuilder"> StringBuilder </param>
        /// <param name="input"> Input string </param>
        /// <param name="delimiter"> Delimeter between strings </param>
        public static void Append(this StringBuilder stringBuilder, string input, string delimiter)
        {
            if (stringBuilder.Length != 0)
            {
                stringBuilder.Append(delimiter);
            }

            stringBuilder.Append(input);
        }

        /// <summary>
        /// Convert string from Base64
        /// </summary>
        /// <param name="stringBase64"> String in Base64 </param>
        /// <returns> String in UTF8 </returns>
        public static string ConvertFromBase64(string stringBase64)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(stringBase64));
        }
    }
}