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
        /// Parses the string as a <see cref="float" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static float? ParseOrNull(string? str) =>
            float.ParseOrNull(
                str,
                NumberStyles.Float | NumberStyles.AllowThousands,
                CultureInfo.CurrentCulture
            );

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
        /// Parses the string as a <see cref="float" />, returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static float ParseOrDefault(string? str, float defaultValue = default) =>
            float.ParseOrNull(str) ?? defaultValue;
    }
}
