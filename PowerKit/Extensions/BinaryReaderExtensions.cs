using System.IO;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class BinaryReaderExtensions
{
    extension(BinaryReader reader)
    {
        /// <summary>
        /// Gets a value indicating whether the reader has reached the end of the stream.
        /// </summary>
        public bool IsEndOfStream => reader.BaseStream.CanSeek
            ? reader.BaseStream.Position >= reader.BaseStream.Length
            : reader.PeekChar() == -1;

        /// <summary>
        /// Skips bytes until the current position is aligned to the specified byte boundary.
        /// </summary>
        public void SkipPadding(int boundaryBytes = 4)
        {
            while (!reader.IsEndOfStream && reader.BaseStream.Position % boundaryBytes != 0)
            {
                _ = reader.ReadByte();
            }
        }

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
