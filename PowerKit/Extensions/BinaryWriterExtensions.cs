#nullable enable
using System.IO;
using System.Diagnostics.CodeAnalysis;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class BinaryWriterExtensions
{
    extension(BinaryWriter writer)
    {
        /// <summary>
        /// Writes a null-terminated string to the binary writer.
        /// </summary>
        public void WriteNullTerminatedString(string value)
        {
            foreach (var ch in value)
            {
                writer.Write(ch);
            }

            writer.Write('\0');
        }
    }
}
