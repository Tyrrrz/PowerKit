using System.Text;

namespace PowerKit.Extensions;

internal static class StringExtensions
{
    extension(string str)
    {
        public string? NullIfWhiteSpace() => !string.IsNullOrWhiteSpace(str) ? str : null;

        public string SubstringUntil(
            string sub,
            System.StringComparison comparison = System.StringComparison.Ordinal
        ) =>
            str.IndexOf(sub, comparison) switch
            {
                >= 0 and var index => str[..index],
                _ => str,
            };

        public string SubstringAfter(
            string sub,
            System.StringComparison comparison = System.StringComparison.Ordinal
        ) =>
            str.IndexOf(sub, comparison) switch
            {
                >= 0 and var index => str[(index + sub.Length)..],
                _ => "",
            };

        public string Truncate(int charCount) => str.Length > charCount ? str[..charCount] : str;

        public string ToSpaceSeparatedWords()
        {
            var builder = new StringBuilder(str.Length * 2);

            foreach (var c in str)
            {
                if (char.IsUpper(c) && builder.Length > 0)
                    builder.Append(' ');

                builder.Append(c);
            }

            return builder.ToString();
        }
    }

    extension(StringBuilder builder)
    {
        public StringBuilder AppendIfNotEmpty(char value) =>
            builder.Length > 0 ? builder.Append(value) : builder;
    }
}
