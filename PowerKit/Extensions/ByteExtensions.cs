#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class ByteExtensions
{
    extension(byte)
    {
        /// <summary>
        /// Parses the string as a <see cref="byte" /> using the specified styles and format provider,
        /// returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static byte? ParseOrNull(
            string? str,
            NumberStyles styles,
            IFormatProvider? formatProvider
        ) => byte.TryParse(str, styles, formatProvider, out var result) ? result : null;

        /// <summary>
        /// Parses the string as a <see cref="byte" /> using the specified format provider,
        /// returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static byte? ParseOrNull(string? str, IFormatProvider? formatProvider) =>
            byte.ParseOrNull(str, NumberStyles.Integer, formatProvider);

        /// <summary>
        /// Parses the string as a <see cref="byte" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static byte? ParseOrNull(string? str) =>
            byte.ParseOrNull(str, CultureInfo.CurrentCulture);

        /// <summary>
        /// Parses the string as a <see cref="byte" /> using the specified styles and format provider,
        /// returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static byte ParseOrDefault(
            string? str,
            NumberStyles styles,
            IFormatProvider? formatProvider,
            byte defaultValue = default
        ) => byte.ParseOrNull(str, styles, formatProvider) ?? defaultValue;

        /// <summary>
        /// Parses the string as a <see cref="byte" /> using the specified format provider,
        /// returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static byte ParseOrDefault(
            string? str,
            IFormatProvider? formatProvider,
            byte defaultValue = default
        ) => byte.ParseOrNull(str, formatProvider) ?? defaultValue;

        /// <summary>
        /// Parses the string as a <see cref="byte" />, returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static byte ParseOrDefault(string? str, byte defaultValue = default) =>
            byte.ParseOrDefault(str, CultureInfo.CurrentCulture, defaultValue);
    }
}
