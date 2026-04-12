using System;
using System.Globalization;

namespace PowerKit.Extensions;

internal static class LongParsingExtensions
{
    extension(long)
    {
        /// <summary>
        /// Parses the string as a <see cref="long" /> using the specified styles and format provider,
        /// returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static long? ParseOrNull(
            string? str,
            NumberStyles styles,
            IFormatProvider? formatProvider
        ) => long.TryParse(str, styles, formatProvider, out var result) ? result : null;

        /// <summary>
        /// Parses the string as a <see cref="long" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static long? ParseOrNull(string? str) =>
            long.ParseOrNull(str, NumberStyles.Integer, CultureInfo.InvariantCulture);
    }
}
