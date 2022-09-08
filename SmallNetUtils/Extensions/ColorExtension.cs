using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;

namespace SmallNetUtils.Extensions
{
    /// <summary>
    /// Extensions to work with colors
    /// </summary>
    public static class ColorExtension
    {
        /// <summary>
        /// LRU last random colors
        /// </summary>
        private static readonly List<Color> LastRandomColors = new List<Color>();

        /// <summary>
        /// Randomize for color
        /// </summary>
        private static readonly Random Random = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Steps to beatify colors generation
        /// </summary>
        private static readonly int[] ColorsStep = { 32, 64, 96, 128, 160, 192, 224, 255 };

        /// <summary>
        /// Do color inversion
        /// </summary>
        /// <param name="color"> Color </param>
        /// <returns> Inverted color </returns>
        public static Color Invert(this Color color)
        {
            return Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B);
        }

        /// <summary>
        /// Tints the color by the given percent.
        /// </summary>
        /// <param name="color">The color being tinted.</param>
        /// <param name="percent">The percent to tint. Ex: 0.1 will make the color 10% lighter.</param>
        /// <returns> The new tinted color. </returns>
        public static Color Lighten(this Color color, float percent)
        {
            var lighting = color.GetBrightness();

            lighting += lighting * percent;
            if (lighting > 1.0)
            {
                lighting = 1;
            }
            else if (lighting <= 0)
            {
                lighting = 0.1f;
            }

            return FromHsl(color.A, color.GetHue(), color.GetSaturation(), lighting);
        }

        /// <summary>
        /// Tints the color by the given percent.
        /// </summary>
        /// <param name="color">The color being tinted.</param>
        /// <param name="percent">The percent to tint. Ex: 0.1 will make the color 10% darker.</param>
        /// <returns> The new tinted color. </returns>
        public static Color Darken(this Color color, float percent)
        {
            var lighting = color.GetBrightness();

            lighting -= lighting * percent;
            if (lighting > 1.0)
            {
                lighting = 1;
            }
            else if (lighting <= 0)
            {
                lighting = 0;
            }

            return FromHsl(color.A, color.GetHue(), color.GetSaturation(), lighting);
        }

        /// <summary>
        /// Get random color
        /// </summary>
        /// <returns> Random color</returns>
        public static Color GetRandomColor()
        {
            Color randomColor;

            do
            {
                var r = ColorsStep[Random.Next(ColorsStep.Length)];
                var g = ColorsStep[Random.Next(ColorsStep.Length)];
                var b = ColorsStep[Random.Next(ColorsStep.Length)];
                randomColor = Color.FromArgb(r, g, b);
            }
            while (LastRandomColors.Contains(randomColor));
            LastRandomColors.Add(randomColor);
            if (LastRandomColors.Count > 100)
            {
                LastRandomColors.RemoveAt(0);
            }

            return randomColor;
        }

        /// <summary>
        /// Generate color based on a string hash
        /// </summary>
        /// <param name="input"> Input string </param>
        /// <returns> Color based on a hash </returns>
        public static Color GenerateColorFromString(string input)
        {
            byte[] hash;

            using (var sha = SHA1.Create())
            {
                hash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
            }

            var hashColor = Color.FromArgb(BitConverter.ToInt32(hash, 0));
            var alpha = hashColor.A;
            var r = (hashColor.R + alpha) % 256 / 32 * 32;
            var g = (hashColor.G + alpha) % 256 / 32 * 32;
            var b = (hashColor.B + alpha) % 256 / 32 * 32;

            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// Converts the HSL values to a Color.
        /// </summary>
        /// <param name="alpha">The alpha.</param>
        /// <param name="hue">The hue.</param>
        /// <param name="saturation">The saturation.</param>
        /// <param name="lighting">The lighting.</param>
        /// <returns> Changed color </returns>
        private static Color FromHsl(int alpha, float hue, float saturation, float lighting)
        {
            if (saturation == 0)
            {
                return Color.FromArgb(alpha, Convert.ToInt32(lighting * 255), Convert.ToInt32(lighting * 255), Convert.ToInt32(lighting * 255));
            }

            float fMax, fMid, fMin;

            if (lighting > 0.5)
            {
                fMax = lighting - lighting * saturation + saturation;
                fMin = lighting + lighting * saturation - saturation;
            }
            else
            {
                fMax = lighting + lighting * saturation;
                fMin = lighting - lighting * saturation;
            }

            var iSextant = (int)Math.Floor(hue / 60f);

            if (hue >= 300f)
            {
                hue -= 360f;
            }

            hue /= 60f;
            hue -= 2f * (float)Math.Floor((iSextant + 1f) % 6f / 2f);
            if (iSextant % 2 == 0)
            {
                fMid = hue * (fMax - fMin) + fMin;
            }
            else
            {
                fMid = fMin - hue * (fMax - fMin);
            }

            var iMax = Convert.ToInt32(fMax * 255);
            var iMid = Convert.ToInt32(fMid * 255);
            var iMin = Convert.ToInt32(fMin * 255);

            switch (iSextant)
            {
                case 1:
                    return Color.FromArgb(alpha, iMid, iMax, iMin);
                case 2:
                    return Color.FromArgb(alpha, iMin, iMax, iMid);
                case 3:
                    return Color.FromArgb(alpha, iMin, iMid, iMax);
                case 4:
                    return Color.FromArgb(alpha, iMid, iMin, iMax);
                case 5:
                    return Color.FromArgb(alpha, iMax, iMin, iMid);
                default:
                    return Color.FromArgb(alpha, iMax, iMid, iMin);
            }
        }
    }
}