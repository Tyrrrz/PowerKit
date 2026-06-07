using System;
using System.Globalization;

namespace PowerKit.Extensions;

/// <summary>
/// Extensions for <see cref="ulong" />.
/// </summary>
public static class UInt64Extensions
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
        /// Parses the string as a <see cref="ulong" /> using the specified format provider,
        /// returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static ulong? ParseOrNull(string? str, IFormatProvider? formatProvider) =>
            ulong.ParseOrNull(str, NumberStyles.Integer, formatProvider);

        /// <summary>
        /// Parses the string as a <see cref="ulong" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static ulong? ParseOrNull(string? str) =>
            ulong.ParseOrNull(str, CultureInfo.CurrentCulture);

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
        /// Parses the string as a <see cref="ulong" /> using the specified format provider,
        /// returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static ulong ParseOrDefault(
            string? str,
            IFormatProvider? formatProvider,
            ulong defaultValue = default
        ) => ulong.ParseOrNull(str, formatProvider) ?? defaultValue;

        /// <summary>
        /// Parses the string as a <see cref="ulong" />, returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static ulong ParseOrDefault(string? str, ulong defaultValue = default) =>
            ulong.ParseOrDefault(str, CultureInfo.CurrentCulture, defaultValue);
    }
}
