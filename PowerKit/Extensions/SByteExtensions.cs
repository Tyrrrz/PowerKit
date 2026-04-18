#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class SByteExtensions
{
    extension(sbyte)
    {
        /// <summary>
        /// Parses the string as an <see cref="sbyte" /> using the specified styles and format provider,
        /// returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static sbyte? ParseOrNull(
            string? str,
            NumberStyles styles,
            IFormatProvider? formatProvider
        ) => sbyte.TryParse(str, styles, formatProvider, out var result) ? result : null;

        /// <summary>
        /// Parses the string as an <see cref="sbyte" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static sbyte? ParseOrNull(string? str) =>
            sbyte.ParseOrNull(str, NumberStyles.Integer, CultureInfo.CurrentCulture);

        /// <summary>
        /// Parses the string as an <see cref="sbyte" /> using the specified styles and format provider,
        /// returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static sbyte ParseOrDefault(
            string? str,
            NumberStyles styles,
            IFormatProvider? formatProvider,
            sbyte defaultValue = default
        ) => sbyte.ParseOrNull(str, styles, formatProvider) ?? defaultValue;

        /// <summary>
        /// Parses the string as an <see cref="sbyte" />, returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static sbyte ParseOrDefault(string? str, sbyte defaultValue = default) =>
            sbyte.ParseOrNull(str) ?? defaultValue;
    }
}
