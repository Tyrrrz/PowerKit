#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class SingleExtensions
{
    extension(float value)
    {
        /// <summary>
        /// Wraps the value to the specified range, cycling it back around when it exceeds the bounds.
        /// </summary>
        public float Wrap(float min, float max) =>
            value < min ? max - (min - value) % (max - min) : min + (value - min) % (max - min);
    }

    extension(float)
    {
        /// <summary>
        /// Parses the string as a <see cref="float" /> using the specified styles and format provider,
        /// returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static float? ParseOrNull(
            string? str,
            NumberStyles styles,
            IFormatProvider? formatProvider
        ) => float.TryParse(str, styles, formatProvider, out var result) ? result : null;

        /// <summary>
        /// Parses the string as a <see cref="float" /> using the specified format provider,
        /// returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static float? ParseOrNull(string? str, IFormatProvider? formatProvider) =>
            float.ParseOrNull(
                str,
                NumberStyles.Float | NumberStyles.AllowThousands,
                formatProvider
            );

        /// <summary>
        /// Parses the string as a <see cref="float" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static float? ParseOrNull(string? str) =>
            float.ParseOrNull(str, CultureInfo.CurrentCulture);

        /// <summary>
        /// Parses the string as a <see cref="float" /> using the specified styles and format provider,
        /// returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static float ParseOrDefault(
            string? str,
            NumberStyles styles,
            IFormatProvider? formatProvider,
            float defaultValue = default
        ) => float.ParseOrNull(str, styles, formatProvider) ?? defaultValue;

        /// <summary>
        /// Parses the string as a <see cref="float" /> using the specified format provider,
        /// returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static float ParseOrDefault(
            string? str,
            IFormatProvider? formatProvider,
            float defaultValue = default
        ) => float.ParseOrNull(str, formatProvider) ?? defaultValue;

        /// <summary>
        /// Parses the string as a <see cref="float" />, returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static float ParseOrDefault(string? str, float defaultValue = default) =>
            float.ParseOrDefault(str, CultureInfo.CurrentCulture, defaultValue);
    }
}
