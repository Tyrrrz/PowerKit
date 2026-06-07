using System;
using System.Globalization;

namespace PowerKit.Extensions;

/// <summary>
/// Extensions for <see cref="long" />.
/// </summary>
public static class Int64Extensions
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
        /// Parses the string as a <see cref="long" /> using the specified format provider,
        /// returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static long? ParseOrNull(string? str, IFormatProvider? formatProvider) =>
            long.ParseOrNull(str, NumberStyles.Integer, formatProvider);

        /// <summary>
        /// Parses the string as a <see cref="long" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static long? ParseOrNull(string? str) =>
            long.ParseOrNull(str, CultureInfo.CurrentCulture);

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
        /// Parses the string as a <see cref="long" /> using the specified format provider,
        /// returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static long ParseOrDefault(
            string? str,
            IFormatProvider? formatProvider,
            long defaultValue = default
        ) => long.ParseOrNull(str, formatProvider) ?? defaultValue;

        /// <summary>
        /// Parses the string as a <see cref="long" />, returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static long ParseOrDefault(string? str, long defaultValue = default) =>
            long.ParseOrDefault(str, CultureInfo.CurrentCulture, defaultValue);
    }
}
