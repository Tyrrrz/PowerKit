#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class Int32Extensions
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
        /// Parses the string as an <see cref="int" /> using the specified format provider,
        /// returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static int? ParseOrNull(string? str, IFormatProvider? formatProvider) =>
            int.ParseOrNull(str, NumberStyles.Integer, formatProvider);

        /// <summary>
        /// Parses the string as an <see cref="int" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static int? ParseOrNull(string? str) =>
            int.ParseOrNull(str, CultureInfo.CurrentCulture);

        /// <summary>
        /// Parses the string as an <see cref="int" /> using the specified styles and format provider,
        /// returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static int ParseOrDefault(
            string? str,
            NumberStyles styles,
            IFormatProvider? formatProvider,
            int defaultValue = default
        ) => int.ParseOrNull(str, styles, formatProvider) ?? defaultValue;

        /// <summary>
        /// Parses the string as an <see cref="int" /> using the specified format provider,
        /// returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static int ParseOrDefault(
            string? str,
            IFormatProvider? formatProvider,
            int defaultValue = default
        ) => int.ParseOrNull(str, formatProvider) ?? defaultValue;

        /// <summary>
        /// Parses the string as an <see cref="int" />, returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static int ParseOrDefault(string? str, int defaultValue = default) =>
            int.ParseOrDefault(str, CultureInfo.CurrentCulture, defaultValue);
    }
}
