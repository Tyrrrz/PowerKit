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
    private static byte[] ReadExactBytes(BinaryReader reader, int count)
    {
        var bytes = reader.ReadBytes(count);
        if (bytes.Length != count)
            throw new EndOfStreamException();

        return bytes;
    }

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
            BinaryPrimitives.ReadInt16BigEndian(ReadExactBytes(reader, sizeof(short)));

        /// <summary>
        /// Reads a 2-byte signed integer from the stream using little-endian byte order.
        /// </summary>
        public short ReadInt16LittleEndian() =>
            BinaryPrimitives.ReadInt16LittleEndian(ReadExactBytes(reader, sizeof(short)));

        /// <summary>
        /// Reads a 2-byte unsigned integer from the stream using big-endian byte order.
        /// </summary>
        public ushort ReadUInt16BigEndian() =>
            BinaryPrimitives.ReadUInt16BigEndian(ReadExactBytes(reader, sizeof(ushort)));

        /// <summary>
        /// Reads a 2-byte unsigned integer from the stream using little-endian byte order.
        /// </summary>
        public ushort ReadUInt16LittleEndian() =>
            BinaryPrimitives.ReadUInt16LittleEndian(ReadExactBytes(reader, sizeof(ushort)));

        /// <summary>
        /// Reads a 4-byte signed integer from the stream using big-endian byte order.
        /// </summary>
        public int ReadInt32BigEndian() =>
            BinaryPrimitives.ReadInt32BigEndian(ReadExactBytes(reader, sizeof(int)));

        /// <summary>
        /// Reads a 4-byte signed integer from the stream using little-endian byte order.
        /// </summary>
        public int ReadInt32LittleEndian() =>
            BinaryPrimitives.ReadInt32LittleEndian(ReadExactBytes(reader, sizeof(int)));

        /// <summary>
        /// Reads a 4-byte unsigned integer from the stream using big-endian byte order.
        /// </summary>
        public uint ReadUInt32BigEndian() =>
            BinaryPrimitives.ReadUInt32BigEndian(ReadExactBytes(reader, sizeof(uint)));

        /// <summary>
        /// Reads a 4-byte unsigned integer from the stream using little-endian byte order.
        /// </summary>
        public uint ReadUInt32LittleEndian() =>
            BinaryPrimitives.ReadUInt32LittleEndian(ReadExactBytes(reader, sizeof(uint)));

        /// <summary>
        /// Reads an 8-byte signed integer from the stream using big-endian byte order.
        /// </summary>
        public long ReadInt64BigEndian() =>
            BinaryPrimitives.ReadInt64BigEndian(ReadExactBytes(reader, sizeof(long)));

        /// <summary>
        /// Reads an 8-byte signed integer from the stream using little-endian byte order.
        /// </summary>
        public long ReadInt64LittleEndian() =>
            BinaryPrimitives.ReadInt64LittleEndian(ReadExactBytes(reader, sizeof(long)));

        /// <summary>
        /// Reads an 8-byte unsigned integer from the stream using big-endian byte order.
        /// </summary>
        public ulong ReadUInt64BigEndian() =>
            BinaryPrimitives.ReadUInt64BigEndian(ReadExactBytes(reader, sizeof(ulong)));

        /// <summary>
        /// Reads an 8-byte unsigned integer from the stream using little-endian byte order.
        /// </summary>
        public ulong ReadUInt64LittleEndian() =>
            BinaryPrimitives.ReadUInt64LittleEndian(ReadExactBytes(reader, sizeof(ulong)));

        /// <summary>
        /// Reads a 4-byte floating-point value from the stream using big-endian byte order.
        /// </summary>
        public float ReadSingleBigEndian() =>
            BinaryPrimitives.ReadSingleBigEndian(ReadExactBytes(reader, sizeof(float)));

        /// <summary>
        /// Reads a 4-byte floating-point value from the stream using little-endian byte order.
        /// </summary>
        public float ReadSingleLittleEndian() =>
            BinaryPrimitives.ReadSingleLittleEndian(ReadExactBytes(reader, sizeof(float)));

        /// <summary>
        /// Reads an 8-byte floating-point value from the stream using big-endian byte order.
        /// </summary>
        public double ReadDoubleBigEndian() =>
            BinaryPrimitives.ReadDoubleBigEndian(ReadExactBytes(reader, sizeof(double)));

        /// <summary>
        /// Reads an 8-byte floating-point value from the stream using little-endian byte order.
        /// </summary>
        public double ReadDoubleLittleEndian() =>
            BinaryPrimitives.ReadDoubleLittleEndian(ReadExactBytes(reader, sizeof(double)));
    }
}
