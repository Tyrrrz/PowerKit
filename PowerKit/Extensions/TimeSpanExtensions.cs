#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

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

        /// <summary>
        /// Parses the string as a <see cref="TimeSpan" /> using the specified format provider,
        /// returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static TimeSpan ParseOrDefault(
            string? str,
            IFormatProvider? formatProvider,
            TimeSpan defaultValue = default
        ) => TimeSpan.ParseOrNull(str, formatProvider) ?? defaultValue;

        /// <summary>
        /// Parses the string as a <see cref="TimeSpan" />, returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static TimeSpan ParseOrDefault(string? str, TimeSpan defaultValue = default) =>
            TimeSpan.ParseOrNull(str) ?? defaultValue;
    }
}
