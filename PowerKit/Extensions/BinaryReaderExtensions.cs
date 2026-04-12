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
            var skipped = 0L;
            while (maxSkipLength is null || skipped < maxSkipLength)
            {
                if (reader.PeekChar() != 0)
                {
                    break;
                }

                reader.ReadByte();
                skipped++;
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
