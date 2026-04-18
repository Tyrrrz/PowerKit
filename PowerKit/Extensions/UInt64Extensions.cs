#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class UInt64Extensions
{
    extension(ulong)
    {
        /// <summary>
        /// Parses the string as a <see cref="ulong" /> using the specified styles and format provider,
        /// returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static ulong? ParseOrNull(
            string? str,
            NumberStyles styles,
            IFormatProvider? formatProvider
        ) => ulong.TryParse(str, styles, formatProvider, out var result) ? result : null;

        /// <summary>
        /// Parses the string as a <see cref="ulong" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static ulong? ParseOrNull(string? str) =>
            ulong.ParseOrNull(str, NumberStyles.Integer, CultureInfo.CurrentCulture);

        /// <summary>
        /// Parses the string as a <see cref="ulong" /> using the specified styles and format provider,
        /// returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static ulong ParseOrDefault(
            string? str,
            NumberStyles styles,
            IFormatProvider? formatProvider,
            ulong defaultValue = default
        ) => ulong.ParseOrNull(str, styles, formatProvider) ?? defaultValue;

        /// <summary>
        /// Parses the string as a <see cref="ulong" />, returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static ulong ParseOrDefault(string? str, ulong defaultValue = default) =>
            ulong.ParseOrNull(str) ?? defaultValue;
    }
}
