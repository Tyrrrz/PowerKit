#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class Int64Extensions
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
            long.ParseOrNull(str, NumberStyles.Integer, CultureInfo.CurrentCulture);

        /// <summary>
        /// Parses the string as a <see cref="long" /> using the specified styles and format provider,
        /// returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static long ParseOrDefault(
            string? str,
            NumberStyles styles,
            IFormatProvider? formatProvider,
            long defaultValue = default
        ) => long.ParseOrNull(str, styles, formatProvider) ?? defaultValue;

        /// <summary>
        /// Parses the string as a <see cref="long" />, returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static long ParseOrDefault(string? str, long defaultValue = default) =>
            long.ParseOrNull(str) ?? defaultValue;
    }
}
