using System;
using System.Diagnostics.CodeAnalysis;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class GuidExtensions
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
