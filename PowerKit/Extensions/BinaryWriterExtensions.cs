using System;
using System.Buffers.Binary;
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

        /// <summary>
        /// Writes a 2-byte signed integer to the stream using big-endian byte order.
        /// </summary>
        public void WriteBigEndian(short value)
        {
            var buffer = new byte[sizeof(short)];
            BinaryPrimitives.WriteInt16BigEndian(buffer, value);
            writer.Write(buffer);
        }

        /// <summary>
        /// Writes a 2-byte signed integer to the stream using little-endian byte order.
        /// </summary>
        public void WriteLittleEndian(short value)
        {
            var buffer = new byte[sizeof(short)];
            BinaryPrimitives.WriteInt16LittleEndian(buffer, value);
            writer.Write(buffer);
        }

        /// <summary>
        /// Writes a 2-byte unsigned integer to the stream using big-endian byte order.
        /// </summary>
        public void WriteBigEndian(ushort value)
        {
            var buffer = new byte[sizeof(ushort)];
            BinaryPrimitives.WriteUInt16BigEndian(buffer, value);
            writer.Write(buffer);
        }

        /// <summary>
        /// Writes a 2-byte unsigned integer to the stream using little-endian byte order.
        /// </summary>
        public void WriteLittleEndian(ushort value)
        {
            var buffer = new byte[sizeof(ushort)];
            BinaryPrimitives.WriteUInt16LittleEndian(buffer, value);
            writer.Write(buffer);
        }

        /// <summary>
        /// Writes a 4-byte signed integer to the stream using big-endian byte order.
        /// </summary>
        public void WriteBigEndian(int value)
        {
            var buffer = new byte[sizeof(int)];
            BinaryPrimitives.WriteInt32BigEndian(buffer, value);
            writer.Write(buffer);
        }

        /// <summary>
        /// Writes a 4-byte signed integer to the stream using little-endian byte order.
        /// </summary>
        public void WriteLittleEndian(int value)
        {
            var buffer = new byte[sizeof(int)];
            BinaryPrimitives.WriteInt32LittleEndian(buffer, value);
            writer.Write(buffer);
        }

        /// <summary>
        /// Writes a 4-byte unsigned integer to the stream using big-endian byte order.
        /// </summary>
        public void WriteBigEndian(uint value)
        {
            var buffer = new byte[sizeof(uint)];
            BinaryPrimitives.WriteUInt32BigEndian(buffer, value);
            writer.Write(buffer);
        }

        /// <summary>
        /// Writes a 4-byte unsigned integer to the stream using little-endian byte order.
        /// </summary>
        public void WriteLittleEndian(uint value)
        {
            var buffer = new byte[sizeof(uint)];
            BinaryPrimitives.WriteUInt32LittleEndian(buffer, value);
            writer.Write(buffer);
        }

        /// <summary>
        /// Writes an 8-byte signed integer to the stream using big-endian byte order.
        /// </summary>
        public void WriteBigEndian(long value)
        {
            var buffer = new byte[sizeof(long)];
            BinaryPrimitives.WriteInt64BigEndian(buffer, value);
            writer.Write(buffer);
        }

        /// <summary>
        /// Writes an 8-byte signed integer to the stream using little-endian byte order.
        /// </summary>
        public void WriteLittleEndian(long value)
        {
            var buffer = new byte[sizeof(long)];
            BinaryPrimitives.WriteInt64LittleEndian(buffer, value);
            writer.Write(buffer);
        }

        /// <summary>
        /// Writes an 8-byte unsigned integer to the stream using big-endian byte order.
        /// </summary>
        public void WriteBigEndian(ulong value)
        {
            var buffer = new byte[sizeof(ulong)];
            BinaryPrimitives.WriteUInt64BigEndian(buffer, value);
            writer.Write(buffer);
        }

        /// <summary>
        /// Writes an 8-byte unsigned integer to the stream using little-endian byte order.
        /// </summary>
        public void WriteLittleEndian(ulong value)
        {
            var buffer = new byte[sizeof(ulong)];
            BinaryPrimitives.WriteUInt64LittleEndian(buffer, value);
            writer.Write(buffer);
        }

        /// <summary>
        /// Writes a 4-byte floating-point value to the stream using big-endian byte order.
        /// </summary>
        public void WriteBigEndian(float value)
        {
            var buffer = new byte[sizeof(float)];
            BinaryPrimitives.WriteSingleBigEndian(buffer, value);
            writer.Write(buffer);
        }

        /// <summary>
        /// Writes a 4-byte floating-point value to the stream using little-endian byte order.
        /// </summary>
        public void WriteLittleEndian(float value)
        {
            var buffer = new byte[sizeof(float)];
            BinaryPrimitives.WriteSingleLittleEndian(buffer, value);
            writer.Write(buffer);
        }

        /// <summary>
        /// Writes an 8-byte floating-point value to the stream using big-endian byte order.
        /// </summary>
        public void WriteBigEndian(double value)
        {
            var buffer = new byte[sizeof(double)];
            BinaryPrimitives.WriteDoubleBigEndian(buffer, value);
            writer.Write(buffer);
        }

        /// <summary>
        /// Writes an 8-byte floating-point value to the stream using little-endian byte order.
        /// </summary>
        public void WriteLittleEndian(double value)
        {
            var buffer = new byte[sizeof(double)];
            BinaryPrimitives.WriteDoubleLittleEndian(buffer, value);
            writer.Write(buffer);
        }
    }
}
