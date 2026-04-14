using System;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;

namespace PowerKit.Extensions;

#if POWERKIT_EXCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
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
