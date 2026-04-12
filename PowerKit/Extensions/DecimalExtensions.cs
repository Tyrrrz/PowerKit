using System;
using System.Globalization;

namespace PowerKit.Extensions;

internal static class DecimalExtensions
{
    extension(decimal)
    {
        /// <summary>
        /// Parses the string as a <see cref="decimal" /> using the specified styles and format provider,
        /// returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static decimal? ParseOrNull(
            string? str,
            NumberStyles styles,
            IFormatProvider? formatProvider
        ) => decimal.TryParse(str, styles, formatProvider, out var result) ? result : null;

        /// <summary>
        /// Parses the string as a <see cref="decimal" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static decimal? ParseOrNull(string? str) =>
            decimal.ParseOrNull(str, NumberStyles.Number, CultureInfo.InvariantCulture);
    }
}
