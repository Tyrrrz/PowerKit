using System.Text;

namespace PowerKit.Extensions;

internal static class StringBuilderExtensions
{
    extension(StringBuilder builder)
    {
        public StringBuilder AppendIfNotEmpty(char value) =>
            builder.Length > 0 ? builder.Append(value) : builder;

        public StringBuilder Trim()
        {
            while (builder.Length > 0 && char.IsWhiteSpace(builder[builder.Length - 1]))
            {
                builder.Remove(builder.Length - 1, 1);
            }

            while (builder.Length > 0 && char.IsWhiteSpace(builder[0]))
            {
                builder.Remove(0, 1);
            }

            return builder;
        }
    }
}
