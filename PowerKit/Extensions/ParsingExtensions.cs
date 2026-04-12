using System;
using System.Globalization;

namespace PowerKit.Extensions;

internal static class IntParsingExtensions
{
    extension(int)
    {
        /// <summary>
        /// Parses the string as an <see cref="int" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static int? ParseOrNull(string? str) =>
            int.TryParse(str, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result)
                ? result
                : null;
    }
}

internal static class LongParsingExtensions
{
    extension(long)
    {
        /// <summary>
        /// Parses the string as a <see cref="long" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static long? ParseOrNull(string? str) =>
            long.TryParse(str, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result)
                ? result
                : null;
    }
}

internal static class DoubleParsingExtensions
{
    extension(double)
    {
        /// <summary>
        /// Parses the string as a <see cref="double" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static double? ParseOrNull(string? str) =>
            double.TryParse(
                str,
                NumberStyles.Float | NumberStyles.AllowThousands,
                CultureInfo.InvariantCulture,
                out var result
            )
                ? result
                : null;
    }
}

internal static class DecimalParsingExtensions
{
    extension(decimal)
    {
        /// <summary>
        /// Parses the string as a <see cref="decimal" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static decimal? ParseOrNull(string? str) =>
            decimal.TryParse(
                str,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out var result
            )
                ? result
                : null;
    }
}

internal static class BoolParsingExtensions
{
    extension(bool)
    {
        /// <summary>
        /// Parses the string as a <see cref="bool" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static bool? ParseOrNull(string? str) =>
            bool.TryParse(str, out var result) ? result : null;
    }
}

internal static class DateTimeParsingExtensions
{
    extension(DateTime)
    {
        /// <summary>
        /// Parses the string as a <see cref="DateTime" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static DateTime? ParseOrNull(string? str) =>
            DateTime.TryParse(
                str,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var result
            )
                ? result
                : null;
    }
}

internal static class DateTimeOffsetParsingExtensions
{
    extension(DateTimeOffset)
    {
        /// <summary>
        /// Parses the string as a <see cref="DateTimeOffset" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static DateTimeOffset? ParseOrNull(string? str) =>
            DateTimeOffset.TryParse(
                str,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var result
            )
                ? result
                : null;
    }
}

internal static class TimeSpanParsingExtensions
{
    extension(TimeSpan)
    {
        /// <summary>
        /// Parses the string as a <see cref="TimeSpan" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static TimeSpan? ParseOrNull(string? str) =>
            TimeSpan.TryParse(str, CultureInfo.InvariantCulture, out var result) ? result : null;
    }
}

internal static class GuidParsingExtensions
{
    extension(Guid)
    {
        /// <summary>
        /// Parses the string as a <see cref="Guid" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static Guid? ParseOrNull(string? str) =>
            Guid.TryParse(str, out var result) ? result : null;
    }
}
