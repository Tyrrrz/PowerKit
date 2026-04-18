#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class Int16Extensions
{
    extension(short)
    {
        /// <summary>
        /// Parses the string as a <see cref="short" /> using the specified styles and format provider,
        /// returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static short? ParseOrNull(
            string? str,
            NumberStyles styles,
            IFormatProvider? formatProvider
        ) => short.TryParse(str, styles, formatProvider, out var result) ? result : null;

        /// <summary>
        /// Parses the string as a <see cref="short" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static short? ParseOrNull(string? str) =>
            short.ParseOrNull(str, NumberStyles.Integer, CultureInfo.CurrentCulture);

        /// <summary>
        /// Parses the string as a <see cref="short" /> using the specified styles and format provider,
        /// returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static short ParseOrDefault(
            string? str,
            NumberStyles styles,
            IFormatProvider? formatProvider,
            short defaultValue = default
        ) => short.ParseOrNull(str, styles, formatProvider) ?? defaultValue;

        /// <summary>
        /// Parses the string as a <see cref="short" />, returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static short ParseOrDefault(string? str, short defaultValue = default) =>
            short.ParseOrNull(str) ?? defaultValue;
    }
}
