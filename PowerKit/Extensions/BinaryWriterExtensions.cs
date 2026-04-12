using System.IO;
using System.Text;

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
            writer.Write(Encoding.UTF8.GetBytes(value));
            writer.Write((byte)0);
        }
    }
}
