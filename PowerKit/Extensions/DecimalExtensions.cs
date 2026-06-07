using System;
using System.Globalization;

namespace PowerKit.Extensions;

/// <summary>
/// Extensions for <see cref="decimal" />.
/// </summary>
public static class DecimalExtensions
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
        /// Parses the string as a <see cref="decimal" /> using the specified format provider,
        /// returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static decimal? ParseOrNull(string? str, IFormatProvider? formatProvider) =>
            decimal.ParseOrNull(str, NumberStyles.Number, formatProvider);

        /// <summary>
        /// Parses the string as a <see cref="decimal" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static decimal? ParseOrNull(string? str) =>
            decimal.ParseOrNull(str, CultureInfo.CurrentCulture);

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
        /// Parses the string as a <see cref="decimal" /> using the specified format provider,
        /// returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static decimal ParseOrDefault(
            string? str,
            IFormatProvider? formatProvider,
            decimal defaultValue = default
        ) => decimal.ParseOrNull(str, formatProvider) ?? defaultValue;

        /// <summary>
        /// Parses the string as a <see cref="decimal" />, returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static decimal ParseOrDefault(string? str, decimal defaultValue = default) =>
            decimal.ParseOrDefault(str, CultureInfo.CurrentCulture, defaultValue);
    }
}
