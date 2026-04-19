#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class ColorExtensions
{
    extension(Color color)
    {
        /// <summary>
        /// Returns the hexadecimal representation of the color (e.g., <c>#RRGGBB</c>).
        /// </summary>
        public string ToHex() => $"#{color.R:X2}{color.G:X2}{color.B:X2}";

        /// <summary>
        /// Returns the RGB value of the color as a 32-bit integer (without the alpha channel).
        /// </summary>
        public int ToRgb() => color.ToArgb() & 0xffffff;

        /// <summary>
        /// Returns a new <see cref="Color" /> with the specified alpha value.
        /// </summary>
        public Color WithAlpha(int alpha) => Color.FromArgb(alpha, color);
    }
}
