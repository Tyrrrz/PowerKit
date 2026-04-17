#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class DoubleExtensions
{
    extension(double)
    {
        /// <summary>
        /// Parses the string as a <see cref="double" /> using the specified styles and format provider,
        /// returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static double? ParseOrNull(
            string? str,
            NumberStyles styles,
            IFormatProvider? formatProvider
        ) => double.TryParse(str, styles, formatProvider, out var result) ? result : null;

        /// <summary>
        /// Parses the string as a <see cref="double" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static double? ParseOrNull(string? str) =>
            double.ParseOrNull(
                str,
                NumberStyles.Float | NumberStyles.AllowThousands,
                CultureInfo.CurrentCulture
            );

        /// <summary>
        /// Parses the string as a <see cref="double" /> using the specified styles and format provider,
        /// returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static double ParseOrDefault(
            string? str,
            NumberStyles styles,
            IFormatProvider? formatProvider,
            double defaultValue = default
        ) => double.ParseOrNull(str, styles, formatProvider) ?? defaultValue;

        /// <summary>
        /// Parses the string as a <see cref="double" />, returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static double ParseOrDefault(string? str, double defaultValue = default) =>
            double.ParseOrNull(str) ?? defaultValue;
    }
}
