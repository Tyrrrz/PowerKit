#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
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
            decimal.ParseOrNull(str, NumberStyles.Number, CultureInfo.CurrentCulture);

        /// <summary>
        /// Parses the string as a <see cref="decimal" /> using the specified styles and format provider,
        /// returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static decimal ParseOrDefault(
            string? str,
            NumberStyles styles,
            IFormatProvider? formatProvider,
            decimal defaultValue = default
        ) => decimal.ParseOrNull(str, styles, formatProvider) ?? defaultValue;

        /// <summary>
        /// Parses the string as a <see cref="decimal" />, returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static decimal ParseOrDefault(string? str, decimal defaultValue = default) =>
            decimal.ParseOrNull(str) ?? defaultValue;
    }
}
