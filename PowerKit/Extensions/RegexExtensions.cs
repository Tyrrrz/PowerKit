using System.Text.RegularExpressions;

namespace PowerKit.Extensions;

/// <summary>
/// Extensions for <see cref="Regex" />.
/// </summary>
public static class RegexExtensions
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
        ) =>
            new(
                "^" + Regex.Escape(pattern).Replace("\\?", ".").Replace("\\*", ".*") + "$",
                options
            );
    }
}
