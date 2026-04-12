using System.Text;

namespace PowerKit.Extensions;

internal static class StringBuilderExtensions
{
    extension(StringBuilder builder)
    {
        /// <summary>
        /// Appends the specified character only if the builder is not empty.
        /// </summary>
        public StringBuilder AppendIfNotEmpty(char value) =>
            builder.Length > 0 ? builder.Append(value) : builder;

        /// <summary>
        /// Removes leading and trailing whitespace characters from the builder.
        /// </summary>
        public StringBuilder Trim()
        {
            var start = 0;
            while (start < builder.Length && char.IsWhiteSpace(builder[start]))
            {
                start++;
            }

            var end = builder.Length - 1;
            while (end >= start && char.IsWhiteSpace(builder[end]))
            {
                end--;
            }

            if (end < builder.Length - 1)
            {
                builder.Remove(end + 1, builder.Length - end - 1);
            }

            if (start > 0)
            {
                builder.Remove(0, start);
            }

            return builder;
        }
    }
}
