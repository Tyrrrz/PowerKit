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
        /// Parses the string as a <see cref="double" /> using the specified format provider,
        /// returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static double? ParseOrNull(string? str, IFormatProvider? formatProvider) =>
            double.ParseOrNull(
                str,
                NumberStyles.Float | NumberStyles.AllowThousands,
                formatProvider
            );

        /// <summary>
        /// Parses the string as a <see cref="double" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static double? ParseOrNull(string? str) =>
            double.ParseOrNull(str, CultureInfo.CurrentCulture);

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
        /// Parses the string as a <see cref="double" /> using the specified format provider,
        /// returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static double ParseOrDefault(
            string? str,
            IFormatProvider? formatProvider,
            double defaultValue = default
        ) => double.ParseOrNull(str, formatProvider) ?? defaultValue;

        /// <summary>
        /// Parses the string as a <see cref="double" />, returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static double ParseOrDefault(string? str, double defaultValue = default) =>
            double.ParseOrDefault(str, CultureInfo.CurrentCulture, defaultValue);
    }

    extension(double value)
    {
        /// <summary>
        /// Wraps the value into the half-open range [<paramref name="min" />, <paramref name="max" />).
        /// A value equal to <paramref name="max" /> wraps to <paramref name="min" />.
        /// </summary>
        /// <param name="min">The inclusive lower bound of the range.</param>
        /// <param name="max">The exclusive upper bound of the range.</param>
        /// <remarks>
        /// This method requires <paramref name="max" /> to be greater than <paramref name="min" />.
        /// If <paramref name="max" /> is less than or equal to <paramref name="min" />, the behavior is invalid.
        /// </remarks>
        public double Wrap(double min, double max) =>
            value < min ? max - (min - value) % (max - min) : min + (value - min) % (max - min);
    }
}
