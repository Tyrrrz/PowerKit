using System;
using System.IO;

namespace PowerKit.Extensions;

/// <summary>
/// Extensions for <see cref="BinaryWriter" />.
/// </summary>
public static class BinaryWriterExtensions
{
    extension(BinaryWriter writer)
    {
        /// <summary>
        /// Writes zero bytes until the current position is aligned to the specified byte boundary.
        /// </summary>
        public void SkipPadding(int boundaryBytes = 4)
        {
            if (boundaryBytes <= 0)
                throw new ArgumentOutOfRangeException(nameof(boundaryBytes));

            while (writer.BaseStream.Position % boundaryBytes != 0)
            {
                writer.Write((byte)0);
            }
        }

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
