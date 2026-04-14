#nullable enable
using System;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class DateTimeOffsetExtensions
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
        ) =>
            DateTimeOffset.TryParse(str, formatProvider, styles, out var result) ? result : null;

        /// <summary>
        /// Parses the string as a <see cref="DateTimeOffset" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static DateTimeOffset? ParseOrNull(string? str) =>
            DateTimeOffset.ParseOrNull(str, CultureInfo.CurrentCulture, DateTimeStyles.None);
    }
}
