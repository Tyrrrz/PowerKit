using System;
using System.Globalization;

namespace PowerKit.Extensions;

internal static class TimeSpanExtensions
{
    extension(TimeSpan)
    {
        /// <summary>
        /// Parses the string as a <see cref="TimeSpan" /> using the specified format provider,
        /// returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static TimeSpan? ParseOrNull(string? str, IFormatProvider? formatProvider) =>
            TimeSpan.TryParse(str, formatProvider, out var result) ? result : null;

        /// <summary>
        /// Parses the string as a <see cref="TimeSpan" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static TimeSpan? ParseOrNull(string? str) =>
            TimeSpan.ParseOrNull(str, CultureInfo.InvariantCulture);
    }
}
