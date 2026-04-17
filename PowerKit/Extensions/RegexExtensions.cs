#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class RegexExtensions
{
    extension(Regex)
    {
        /// <summary>
        /// Creates a <see cref="Regex" /> from a wildcard pattern, where <c>?</c> matches any single
        /// character and <c>*</c> matches any sequence of characters (including an empty sequence).
        /// </summary>
        public static Regex FromWildcardPattern(
            string pattern,
            RegexOptions options = RegexOptions.None
        )
        {
            var regexPattern =
                "^" + Regex.Escape(pattern).Replace("\\?", ".").Replace("\\*", ".*") + "$";

            return new Regex(regexPattern, options);
        }
    }
}
