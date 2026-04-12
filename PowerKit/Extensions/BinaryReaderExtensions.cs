using System.IO;
using System.Text;

namespace PowerKit.Extensions;

internal static class BinaryReaderExtensions
{
    extension(BinaryReader reader)
    {
        /// <summary>
        /// Skips zero bytes in the stream, up to the specified maximum length.
        /// </summary>
        public void SkipZeroes(long? maxSkipLength = null)
        {
            var endPosition = maxSkipLength is not null
                ? reader.BaseStream.Position + maxSkipLength.Value
                : reader.BaseStream.Length;

            while (reader.BaseStream.Position < endPosition)
            {
                if (reader.ReadByte() != 0)
                {
                    // Go back to non-zero byte
                    reader.BaseStream.Seek(-1, SeekOrigin.Current);
                    return;
                }
            }
        }

        /// <summary>
        /// Reads a null-terminated string from the binary reader.
        /// </summary>
        public string ReadNullTerminatedString()
        {
            var buffer = new StringBuilder();

            while (true)
            {
                var ch = reader.ReadChar();
                if (ch == '\0')
                {
                    break;
                }

                buffer.Append(ch);
            }

            return buffer.ToString();
        }
    }
}
