using System;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
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
            TimeSpan.ParseOrNull(str, CultureInfo.CurrentCulture);
    }
}
