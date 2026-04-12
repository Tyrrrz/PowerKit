using System;
using System.Text;

namespace PowerKit.Extensions;

internal static class StringExtensions
{
    extension(string str)
    {
        public string SubstringUntil(
            string sub,
            StringComparison comparison = StringComparison.Ordinal
        ) =>
            str.IndexOf(sub, comparison) switch
            {
                >= 0 and var index => str[..index],
                _ => str,
            };

        public string SubstringAfter(
            string sub,
            StringComparison comparison = StringComparison.Ordinal
        ) =>
            str.IndexOf(sub, comparison) switch
            {
                >= 0 and var index => str[(index + sub.Length)..],
                _ => "",
            };

        public string Truncate(int charCount) => str.Length > charCount ? str[..charCount] : str;

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

        public string ToKebabCase() => str.SeparateWords('-').ToLowerInvariant();

        public string ToSnakeCase() => str.SeparateWords('_').ToLowerInvariant();
    }
}
