using System;
using System.Text;

namespace PowerKit;

/// <summary>
/// Helper methods for working with XML.
/// </summary>
public static class Xml
{
    private static bool IsValidXmlChar(char ch) =>
        ch == '\t'
        || ch == '\n'
        || ch == '\r'
        || (ch >= '\x20' && ch <= '\xD7FF')
        || (ch >= '\xE000' && ch <= '\xFFFD');

    /// <summary>
    /// Escapes invalid XML characters in the specified string, returning a valid XML string.
    /// Characters that cannot be represented in XML (such as most control characters) are removed.
    /// Special XML characters (<c>&amp;</c>, <c>&lt;</c>, <c>&gt;</c>, <c>&quot;</c>, <c>&apos;</c>)
    /// are replaced with their corresponding XML entities.
    /// </summary>
    public static string Escape(string str)
    {
        ArgumentNullException.ThrowIfNull(str);
        StringBuilder? builder = null;

        var i = 0;
        while (i < str.Length)
        {
            var ch = str[i];

            string? replacement;
            if (ch == '&')
                replacement = "&amp;";
            else if (ch == '<')
                replacement = "&lt;";
            else if (ch == '>')
                replacement = "&gt;";
            else if (ch == '"')
                replacement = "&quot;";
            else if (ch == '\'')
                replacement = "&apos;";
            else if (
                char.IsHighSurrogate(ch)
                && i + 1 < str.Length
                && char.IsLowSurrogate(str[i + 1])
            )
            {
                // Valid surrogate pair — represents a supplementary character (U+10000..U+10FFFF),
                // which is valid in XML. Append both chars and advance past the pair.
                builder?.Append(ch);
                builder?.Append(str[i + 1]);
                i += 2;
                continue;
            }
            else if (IsValidXmlChar(ch))
                replacement = null;
            else
            {
                // Truly invalid XML character — skip it.
                builder ??= new StringBuilder(str, 0, i, str.Length);
                i++;
                continue;
            }

            if (replacement is not null)
            {
                builder ??= new StringBuilder(str, 0, i, str.Length);
                builder.Append(replacement);
            }
            else
            {
                builder?.Append(ch);
            }

            i++;
        }

        return builder?.ToString() ?? str;
    }
}
