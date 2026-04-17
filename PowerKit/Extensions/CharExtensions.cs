#nullable enable
using System.Diagnostics.CodeAnalysis;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class CharExtensions
{
    extension(char c)
    {
        /// <summary>
        /// Returns a string that contains the character repeated the specified number of times.
        /// </summary>
        public string Repeat(int count) => new(c, count);
    }
}
