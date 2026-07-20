using System;
using System.Buffers.Binary;
using System.IO;
using System.Text;

namespace PowerKit.Extensions;

/// <summary>
/// Extensions for <see cref="BinaryReader" />.
/// </summary>
public static class BinaryReaderExtensions
{
    extension(BinaryReader reader)
    {
        /// <summary>
        /// Gets a value indicating whether the reader has reached the end of the stream.
        /// </summary>
        public bool IsEndOfStream =>
            reader.BaseStream.CanSeek
                ? reader.BaseStream.Position >= reader.BaseStream.Length
                : reader.PeekChar() == -1;

        /// <summary>
        /// Reads a null-terminated string from the binary reader.
        /// </summary>
        public string ReadNullTerminatedString()
        {
            var buffer = new StringBuilder();

            while (reader.ReadChar() is not '\0' and var ch)
            {
                buffer.Append(ch);
            }

            return buffer.ToString();
        }

        /// <summary>
        /// Skips bytes until the current position is aligned to the specified byte boundary.
        /// </summary>
        public void SkipPadding(int boundaryBytes = 4)
        {
            if (boundaryBytes <= 0)
                throw new ArgumentOutOfRangeException(nameof(boundaryBytes));

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
        /// Reads a 2-byte signed integer from the stream using big-endian byte order.
        /// </summary>
        public short ReadInt16BigEndian() =>
            BinaryPrimitives.ReadInt16BigEndian(reader.ReadBytes(sizeof(short)));

        /// <summary>
        /// Reads a 2-byte signed integer from the stream using little-endian byte order.
        /// </summary>
        public short ReadInt16LittleEndian() =>
            BinaryPrimitives.ReadInt16LittleEndian(reader.ReadBytes(sizeof(short)));

        /// <summary>
        /// Reads a 2-byte unsigned integer from the stream using big-endian byte order.
        /// </summary>
        public ushort ReadUInt16BigEndian() =>
            BinaryPrimitives.ReadUInt16BigEndian(reader.ReadBytes(sizeof(ushort)));

        /// <summary>
        /// Reads a 2-byte unsigned integer from the stream using little-endian byte order.
        /// </summary>
        public ushort ReadUInt16LittleEndian() =>
            BinaryPrimitives.ReadUInt16LittleEndian(reader.ReadBytes(sizeof(ushort)));

        /// <summary>
        /// Reads a 4-byte signed integer from the stream using big-endian byte order.
        /// </summary>
        public int ReadInt32BigEndian() =>
            BinaryPrimitives.ReadInt32BigEndian(reader.ReadBytes(sizeof(int)));

        /// <summary>
        /// Reads a 4-byte signed integer from the stream using little-endian byte order.
        /// </summary>
        public int ReadInt32LittleEndian() =>
            BinaryPrimitives.ReadInt32LittleEndian(reader.ReadBytes(sizeof(int)));

        /// <summary>
        /// Reads a 4-byte unsigned integer from the stream using big-endian byte order.
        /// </summary>
        public uint ReadUInt32BigEndian() =>
            BinaryPrimitives.ReadUInt32BigEndian(reader.ReadBytes(sizeof(uint)));

        /// <summary>
        /// Reads a 4-byte unsigned integer from the stream using little-endian byte order.
        /// </summary>
        public uint ReadUInt32LittleEndian() =>
            BinaryPrimitives.ReadUInt32LittleEndian(reader.ReadBytes(sizeof(uint)));

        /// <summary>
        /// Reads an 8-byte signed integer from the stream using big-endian byte order.
        /// </summary>
        public long ReadInt64BigEndian() =>
            BinaryPrimitives.ReadInt64BigEndian(reader.ReadBytes(sizeof(long)));

        /// <summary>
        /// Reads an 8-byte signed integer from the stream using little-endian byte order.
        /// </summary>
        public long ReadInt64LittleEndian() =>
            BinaryPrimitives.ReadInt64LittleEndian(reader.ReadBytes(sizeof(long)));

        /// <summary>
        /// Reads an 8-byte unsigned integer from the stream using big-endian byte order.
        /// </summary>
        public ulong ReadUInt64BigEndian() =>
            BinaryPrimitives.ReadUInt64BigEndian(reader.ReadBytes(sizeof(ulong)));

        /// <summary>
        /// Reads an 8-byte unsigned integer from the stream using little-endian byte order.
        /// </summary>
        public ulong ReadUInt64LittleEndian() =>
            BinaryPrimitives.ReadUInt64LittleEndian(reader.ReadBytes(sizeof(ulong)));

#if NET5_0_OR_GREATER || !FEATURE_MEMORY
        /// <summary>
        /// Reads a 4-byte floating-point value from the stream using big-endian byte order.
        /// </summary>
        public float ReadSingleBigEndian() =>
            BinaryPrimitives.ReadSingleBigEndian(reader.ReadBytes(sizeof(float)));

        /// <summary>
        /// Reads a 4-byte floating-point value from the stream using little-endian byte order.
        /// </summary>
        public float ReadSingleLittleEndian() =>
            BinaryPrimitives.ReadSingleLittleEndian(reader.ReadBytes(sizeof(float)));

        /// <summary>
        /// Reads an 8-byte floating-point value from the stream using big-endian byte order.
        /// </summary>
        public double ReadDoubleBigEndian() =>
            BinaryPrimitives.ReadDoubleBigEndian(reader.ReadBytes(sizeof(double)));

        /// <summary>
        /// Reads an 8-byte floating-point value from the stream using little-endian byte order.
        /// </summary>
        public double ReadDoubleLittleEndian() =>
            BinaryPrimitives.ReadDoubleLittleEndian(reader.ReadBytes(sizeof(double)));
#endif
    }
}
