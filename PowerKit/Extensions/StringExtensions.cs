using System;
using System.Linq;
using System.Security;
using System.Text;

namespace PowerKit.Extensions;

/// <summary>
/// Extensions for <see cref="string" />.
/// </summary>
public static class StringExtensions
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
        /// Removes the specified prefix from the beginning of the string, if present.
        /// If the string does not start with <paramref name="prefix" />, the original string is returned unchanged.
        /// </summary>
        public string TrimPrefix(
            string prefix,
            StringComparison comparison = StringComparison.Ordinal
        ) => str.StartsWith(prefix, comparison) ? str[prefix.Length..] : str;

        /// <summary>
        /// Removes the specified suffix from the end of the string, if present.
        /// If the string does not end with <paramref name="suffix" />, the original string is returned unchanged.
        /// </summary>
        public string TrimSuffix(
            string suffix,
            StringComparison comparison = StringComparison.Ordinal
        ) => str.EndsWith(suffix, comparison) ? str[..^suffix.Length] : str;

        /// <summary>
        /// Truncates the string to the specified maximum number of characters.
        /// </summary>
        public string Truncate(int charCount) => str.Length > charCount ? str[..charCount] : str;

        /// <summary>
        /// Truncates the string so that its encoded byte length does not exceed the specified maximum.
        /// Uses the provided encoding, or UTF-8 if <paramref name="encoding"/> is <c>null</c>.
        /// </summary>
        public string TruncateBytes(int byteCount, Encoding? encoding = null)
        {
            if (byteCount < 0)
                throw new ArgumentOutOfRangeException(nameof(byteCount));

            var actualEncoding = encoding ?? Encoding.UTF8;

            if (actualEncoding.GetByteCount(str) <= byteCount)
                return str;

            var chars = str.ToCharArray();
            var charLo = 0;
            var charHi = chars.Length;

            while (charLo < charHi)
            {
                var mid = charLo + (charHi - charLo + 1) / 2;

                // Use try/catch so that encodings with EncoderExceptionFallback don't
                // throw when a probe boundary happens to split a surrogate pair.
                var fits = false;
                try
                {
                    fits = actualEncoding.GetByteCount(chars, 0, mid) <= byteCount;
                }
                catch (EncoderFallbackException) { }

                if (fits)
                    charLo = mid;
                else
                    charHi = mid - 1;
            }

            // If the cut point landed right after a high surrogate (its paired low surrogate
            // was not included), step back to avoid returning a string with an unpaired surrogate.
            if (charLo > 0 && char.IsHighSurrogate(chars[charLo - 1]))
                charLo--;

            return str[..charLo];
        }
    }
}
