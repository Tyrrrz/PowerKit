using System;
using System.Globalization;

namespace PowerKit.Extensions;

internal static class IntExtensions
{
    extension(int)
    {
        /// <summary>
        /// Parses the string as an <see cref="int" /> using the specified styles and format provider,
        /// returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static int? ParseOrNull(
            string? str,
            NumberStyles styles,
            IFormatProvider? formatProvider
        ) => int.TryParse(str, styles, formatProvider, out var result) ? result : null;

        /// <summary>
        /// Parses the string as an <see cref="int" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static int? ParseOrNull(string? str) =>
            int.ParseOrNull(str, NumberStyles.Integer, CultureInfo.CurrentCulture);
    }
}
