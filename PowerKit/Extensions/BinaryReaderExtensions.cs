using System.IO;
using System.Text;

namespace PowerKit.Extensions;

internal static class BinaryReaderExtensions
{
    extension(BinaryReader reader)
    {
        /// <summary>
        /// Gets a value indicating whether the reader has reached the end of the stream.
        /// </summary>
        public bool IsEndOfStream => reader.BaseStream.Position >= reader.BaseStream.Length;

        /// <summary>
        /// Skips bytes until the current position is aligned to the specified bit boundary.
        /// </summary>
        public void SkipPadding(int boundaryBits = 32)
        {
            while (!reader.IsEndOfStream && reader.BaseStream.Position * 8 % boundaryBits != 0)
            {
                // Read a character so that it takes up either 1 or 2 bytes,
                // depending on the encoding of the stream.
                _ = reader.ReadChar();
            }
        }

        /// <summary>
        /// Skips zero bytes, stopping at the first non-zero byte or after reading
        /// <paramref name="maxSkipLength" /> bytes.
        /// </summary>
        public void SkipZeroes(long? maxSkipLength = null)
        {
            var endPosition = maxSkipLength is not null
                ? reader.BaseStream.Position + maxSkipLength
                : reader.BaseStream.Length;

            while (reader.BaseStream.Position < endPosition)
            {
                if (reader.ReadByte() != 0)
                {
                    // Go back to the non-zero byte
                    reader.BaseStream.Seek(-1, SeekOrigin.Current);
                    return;
                }
            }
        }

        /// <summary>
        /// Reads a null-terminated string from the stream.
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
