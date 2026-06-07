using System;
using System.Globalization;

namespace PowerKit.Extensions;

/// <summary>
/// Extensions for <see cref="DateTimeOffset" />.
/// </summary>
public static class DateTimeOffsetExtensions
{
    extension(DateTimeOffset)
    {
        /// <summary>
        /// Parses the string as a <see cref="DateTimeOffset" /> using the specified format provider and styles,
        /// returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static DateTimeOffset? ParseOrNull(
            string? str,
            IFormatProvider? formatProvider,
            DateTimeStyles styles
        ) => DateTimeOffset.TryParse(str, formatProvider, styles, out var result) ? result : null;

        /// <summary>
        /// Parses the string as a <see cref="DateTimeOffset" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static DateTimeOffset? ParseOrNull(string? str) =>
            DateTimeOffset.ParseOrNull(str, CultureInfo.CurrentCulture, DateTimeStyles.None);

        /// <summary>
        /// Parses the string as a <see cref="DateTimeOffset" /> using the specified format provider and styles,
        /// returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static DateTimeOffset ParseOrDefault(
            string? str,
            IFormatProvider? formatProvider,
            DateTimeStyles styles,
            DateTimeOffset defaultValue = default
        ) => DateTimeOffset.ParseOrNull(str, formatProvider, styles) ?? defaultValue;

        /// <summary>
        /// Parses the string as a <see cref="DateTimeOffset" />, returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static DateTimeOffset ParseOrDefault(
            string? str,
            DateTimeOffset defaultValue = default
        ) => DateTimeOffset.ParseOrNull(str) ?? defaultValue;
    }
}
