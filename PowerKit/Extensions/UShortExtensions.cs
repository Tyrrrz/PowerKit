#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class UShortExtensions
{
    extension(ushort)
    {
        /// <summary>
        /// Parses the string as a <see cref="ushort" /> using the specified styles and format provider,
        /// returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static ushort? ParseOrNull(
            string? str,
            NumberStyles styles,
            IFormatProvider? formatProvider
        ) => ushort.TryParse(str, styles, formatProvider, out var result) ? result : null;

        /// <summary>
        /// Parses the string as a <see cref="ushort" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static ushort? ParseOrNull(string? str) =>
            ushort.ParseOrNull(str, NumberStyles.Integer, CultureInfo.CurrentCulture);

        /// <summary>
        /// Parses the string as a <see cref="ushort" /> using the specified styles and format provider,
        /// returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static ushort ParseOrDefault(
            string? str,
            NumberStyles styles,
            IFormatProvider? formatProvider,
            ushort defaultValue = default
        ) => ushort.ParseOrNull(str, styles, formatProvider) ?? defaultValue;

        /// <summary>
        /// Parses the string as a <see cref="ushort" />, returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static ushort ParseOrDefault(string? str, ushort defaultValue = default) =>
            ushort.ParseOrNull(str) ?? defaultValue;
    }
}
