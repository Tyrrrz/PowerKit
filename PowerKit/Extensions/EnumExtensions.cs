#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class EnumExtensions
{
    extension(Enum)
    {
        /// <summary>
        /// Parses the string as an enum value of type <typeparamref name="TEnum" />,
        /// returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static TEnum? ParseOrNull<TEnum>(string? str, bool ignoreCase)
            where TEnum : struct, Enum =>
            Enum.TryParse<TEnum>(str, ignoreCase, out var result) ? result : null;

        /// <summary>
        /// Parses the string as an enum value of type <typeparamref name="TEnum" />,
        /// returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static TEnum? ParseOrNull<TEnum>(string? str)
            where TEnum : struct, Enum => Enum.ParseOrNull<TEnum>(str, false);

        /// <summary>
        /// Parses the string as an enum value of type <typeparamref name="TEnum" />,
        /// returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static TEnum ParseOrDefault<TEnum>(
            string? str,
            bool ignoreCase,
            TEnum defaultValue = default
        )
            where TEnum : struct, Enum => Enum.ParseOrNull<TEnum>(str, ignoreCase) ?? defaultValue;

        /// <summary>
        /// Parses the string as an enum value of type <typeparamref name="TEnum" />,
        /// returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static TEnum ParseOrDefault<TEnum>(string? str, TEnum defaultValue = default)
            where TEnum : struct, Enum => Enum.ParseOrDefault(str, false, defaultValue);
    }
}
