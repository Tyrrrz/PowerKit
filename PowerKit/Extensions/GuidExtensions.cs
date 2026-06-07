using System;

namespace PowerKit.Extensions;

/// <summary>
/// Extensions for <see cref="Guid" />.
/// </summary>
public static class GuidExtensions
{
    extension(Guid)
    {
        /// <summary>
        /// Parses the string as a <see cref="Guid" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static Guid? ParseOrNull(string? str) =>
            Guid.TryParse(str, out var result) ? result : null;

        /// <summary>
        /// Parses the string as a <see cref="Guid" />, returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static Guid ParseOrDefault(string? str, Guid defaultValue = default) =>
            Guid.ParseOrNull(str) ?? defaultValue;
    }
}
