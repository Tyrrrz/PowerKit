using System.Diagnostics.CodeAnalysis;

namespace PowerKit.Extensions;

#if POWERKIT_EXCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class BoolExtensions
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
