using System;

namespace PowerKit.Extensions;

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
