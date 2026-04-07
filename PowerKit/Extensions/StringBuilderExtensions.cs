using System.Text;

namespace PowerKit.Extensions;

internal static class StringBuilderExtensions
{
    extension(StringBuilder builder)
    {
        public StringBuilder AppendIfNotEmpty(char value) =>
            builder.Length > 0 ? builder.Append(value) : builder;
    }
}
