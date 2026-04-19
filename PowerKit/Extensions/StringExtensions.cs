#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security;
using System.Text;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class StringExtensions
{
    extension(string str)
    {
        /// <summary>
        /// Replaces each character using the replacement produced by the provided selector.
        /// </summary>
        public string Replace(Func<char, char> getReplacement) =>
            string.Create(
                str.Length,
                (str, getReplacement),
                static (chars, source) =>
                {
                    foreach (var (i, ch) in source.str.Index())
                    {
                        chars[i] = source.getReplacement(ch);
                    }
                }
            );

        /// <summary>
        /// Replaces each whitespace character using the replacement produced by the provided selector.
        /// </summary>
        public string ReplaceWhiteSpace(Func<char, char> getReplacement) =>
            str.Replace(ch => char.IsWhiteSpace(ch) ? getReplacement(ch) : ch);

        /// <summary>
        /// Replaces each whitespace character with the specified replacement character.
        /// </summary>
        public string ReplaceWhiteSpace(char replacement) =>
            str.ReplaceWhiteSpace(_ => replacement);

        /// <summary>
        /// Returns the string with the characters in reverse order.
        /// </summary>
        public string Reverse()
        {
            if (str.Length <= 1)
            {
                return str;
            }

            return string.Create(
                str.Length,
                str,
                static (chars, source) =>
                {
                    for (var i = 0; i < source.Length; i++)
                    {
                        chars[i] = source[source.Length - 1 - i];
                    }
                }
            );
        }

        /// <summary>
        /// Inserts the specified separator before each uppercase letter, splitting PascalCase words.
        /// </summary>
        public string SeparateWords(char separator)
        {
            var builder = new StringBuilder(str.Length * 2);

            foreach (var ch in str)
            {
                if (char.IsUpper(ch) && builder.Length > 0)
                {
                    builder.Append(separator);
                }

                builder.Append(ch);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Returns the substring after the first occurrence of the specified substring.
        /// If the substring is not found, returns an empty string.
        /// </summary>
        public string SubstringAfter(
            string sub,
            StringComparison comparison = StringComparison.Ordinal
        ) =>
            str.IndexOf(sub, comparison) switch
            {
                >= 0 and var index => str[(index + sub.Length)..],
                _ => "",
            };

        /// <summary>
        /// Returns the substring after the last occurrence of the specified substring.
        /// If the substring is not found, returns an empty string.
        /// </summary>
        public string SubstringAfterLast(
            string sub,
            StringComparison comparison = StringComparison.Ordinal
        ) =>
            str.LastIndexOf(sub, comparison) switch
            {
                >= 0 and var index => str[(index + sub.Length)..],
                _ => "",
            };

        /// <summary>
        /// Returns the substring before the first occurrence of the specified substring.
        /// If the substring is not found, returns the original string.
        /// </summary>
        public string SubstringUntil(
            string sub,
            StringComparison comparison = StringComparison.Ordinal
        ) =>
            str.IndexOf(sub, comparison) switch
            {
                >= 0 and var index => str[..index],
                _ => str,
            };

        /// <summary>
        /// Returns the substring before the last occurrence of the specified substring.
        /// If the substring is not found, returns the original string.
        /// </summary>
        public string SubstringUntilLast(
            string sub,
            StringComparison comparison = StringComparison.Ordinal
        ) =>
            str.LastIndexOf(sub, comparison) switch
            {
                >= 0 and var index => str[..index],
                _ => str,
            };

        /// <summary>
        /// Converts the PascalCase string to kebab-case (e.g., "FooBar" → "foo-bar").
        /// </summary>
        public string ToKebabCase() => str.SeparateWords('-').ToLowerInvariant();

        /// <summary>
        /// Converts the string to a <see cref="SecureString"/>.
        /// </summary>
        public SecureString ToSecureString()
        {
            var secure = new SecureString();

            foreach (var ch in str)
            {
                secure.AppendChar(ch);
            }

            secure.MakeReadOnly();
            return secure;
        }

        /// <summary>
        /// Converts the PascalCase string to snake_case (e.g., "FooBar" → "foo_bar").
        /// </summary>
        public string ToSnakeCase() => str.SeparateWords('_').ToLowerInvariant();

        /// <summary>
        /// Truncates the string to the specified maximum number of characters.
        /// </summary>
        public string Truncate(int charCount) => str.Length > charCount ? str[..charCount] : str;
    }
}
