#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class UInt32Extensions
{
    extension(uint)
    {
        /// <summary>
        /// Parses the string as a <see cref="uint" /> using the specified styles and format provider,
        /// returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static uint? ParseOrNull(
            string? str,
            NumberStyles styles,
            IFormatProvider? formatProvider
        ) => uint.TryParse(str, styles, formatProvider, out var result) ? result : null;

        /// <summary>
        /// Parses the string as a <see cref="uint" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static uint? ParseOrNull(string? str) =>
            uint.ParseOrNull(str, NumberStyles.Integer, CultureInfo.CurrentCulture);

        /// <summary>
        /// Parses the string as a <see cref="uint" /> using the specified styles and format provider,
        /// returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static uint ParseOrDefault(
            string? str,
            NumberStyles styles,
            IFormatProvider? formatProvider,
            uint defaultValue = default
        ) => uint.ParseOrNull(str, styles, formatProvider) ?? defaultValue;

        /// <summary>
        /// Parses the string as a <see cref="uint" />, returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static uint ParseOrDefault(string? str, uint defaultValue = default) =>
            uint.ParseOrNull(str) ?? defaultValue;
    }
}
