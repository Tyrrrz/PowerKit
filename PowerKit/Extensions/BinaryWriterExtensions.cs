using System.IO;

namespace PowerKit.Extensions;

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
