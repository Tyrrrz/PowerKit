using System;
using System.Drawing;

namespace PowerKit.Extensions;

/// <summary>
/// Extensions for <see cref="Color" />.
/// </summary>
public static class ColorExtensions
{
    extension(Color)
    {
        /// <summary>
        /// Creates a new <see cref="Color" /> from the specified HSV (hue, saturation, value) components.
        /// </summary>
        /// <param name="hue">Hue, in degrees (0-360).</param>
        /// <param name="saturation">Saturation, in the range 0-1.</param>
        /// <param name="value">Value (brightness), in the range 0-1.</param>
        public static Color FromHsv(double hue, double saturation, double value) =>
            Color.FromAhsv(255, hue, saturation, value);

        /// <summary>
        /// Creates a new <see cref="Color" /> from the specified alpha and HSV (hue, saturation, value) components.
        /// </summary>
        /// <param name="alpha">Alpha component, in the range 0-255.</param>
        /// <param name="hue">Hue, in degrees (0-360).</param>
        /// <param name="saturation">Saturation, in the range 0-1.</param>
        /// <param name="value">Value (brightness), in the range 0-1.</param>
        public static Color FromAhsv(byte alpha, double hue, double saturation, double value)
        {
            hue %= 360;
            if (hue < 0)
                hue += 360;

            saturation = Math.Min(Math.Max(saturation, 0), 1);
            value = Math.Min(Math.Max(value, 0), 1);

            var c = value * saturation;
            var x = c * (1 - Math.Abs(hue / 60 % 2 - 1));
            var m = value - c;

            var (r, g, b) = hue switch
            {
                < 60 => (c, x, 0.0),
                < 120 => (x, c, 0.0),
                < 180 => (0.0, c, x),
                < 240 => (0.0, x, c),
                < 300 => (x, 0.0, c),
                _ => (c, 0.0, x),
            };

            return Color.FromArgb(
                alpha,
                (int)Math.Round((r + m) * 255),
                (int)Math.Round((g + m) * 255),
                (int)Math.Round((b + m) * 255)
            );
        }
    }

    extension(Color color)
    {
        /// <summary>
        /// Returns the hexadecimal representation of the color (e.g., <c>#RRGGBB</c>).
        /// </summary>
        public string ToHexString() => $"#{color.R:X2}{color.G:X2}{color.B:X2}";

        /// <summary>
        /// Returns the RGB value of the color as a 32-bit integer (without the alpha channel).
        /// </summary>
        public int ToRgb() => color.ToArgb() & 0xffffff;

        /// <summary>
        /// Returns a new <see cref="Color" /> with the specified alpha value.
        /// </summary>
        public Color WithAlpha(int alpha) => Color.FromArgb(alpha, color);

        /// <summary>
        /// Returns a new <see cref="Color" /> with its alpha component set to 255 (fully opaque).
        /// </summary>
        public Color WithFullAlpha() => color.WithAlpha(255);
    }
}
