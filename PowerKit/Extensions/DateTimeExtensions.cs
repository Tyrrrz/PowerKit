using System;
using System.Globalization;

namespace PowerKit.Extensions;

internal static class DateTimeExtensions
{
    extension(DateTime)
    {
        /// <summary>
        /// Parses the string as a <see cref="DateTime" /> using the specified format provider and styles,
        /// returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static DateTime? ParseOrNull(
            string? str,
            IFormatProvider? formatProvider,
            DateTimeStyles styles
        ) => DateTime.TryParse(str, formatProvider, styles, out var result) ? result : null;

        /// <summary>
        /// Parses the string as a <see cref="DateTime" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static DateTime? ParseOrNull(string? str) =>
            DateTime.ParseOrNull(str, CultureInfo.InvariantCulture, DateTimeStyles.None);
    }
}
