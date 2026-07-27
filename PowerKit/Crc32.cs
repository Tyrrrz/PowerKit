using System.IO;
#if !NETFRAMEWORK || NET45_OR_GREATER
using System;
#endif

namespace PowerKit;

/// <summary>
/// Provides methods for computing CRC-32 checksums.
/// </summary>
public static class Crc32
{
    private static readonly uint[] Table = GenerateTable();

    private static uint[] GenerateTable()
    {
        var table = new uint[256];

        for (var i = 0; i < 256; i++)
        {
            var entry = (uint)i;
            for (var j = 0; j < 8; j++)
            {
                if ((entry & 1) != 0)
                    entry = (entry >> 1) ^ 0xEDB88320u;
                else
                    entry >>= 1;
            }

            table[i] = entry;
        }

        return table;
    }

    /// <summary>
    /// Computes the CRC-32 checksum of data read from the specified stream.
    /// </summary>
    public static uint Hash(Stream stream)
    {
        var crc = 0xFFFFFFFFu;

        using var buffer = SpanPool<byte>.Shared.Rent(4096);

        while (stream.Read(buffer.Span) is > 0 and var bytesRead)
        {
            for (var i = 0; i < bytesRead; i++)
                crc = (crc >> 8) ^ Table[(byte)(crc ^ buffer.Span[i])];
        }

        return crc ^ 0xFFFFFFFFu;
    }

    /// <summary>
    /// Computes the CRC-32 checksum of the specified data.
    /// </summary>
    public static uint Hash(byte[] data)
    {
        using var stream = new MemoryStream(data);
        return Hash(stream);
    }

#if !NETFRAMEWORK || NET45_OR_GREATER
    /// <inheritdoc cref="Hash(byte[])" />
    public static uint Hash(ReadOnlySpan<byte> data)
    {
        var crc = 0xFFFFFFFFu;

        for (var i = 0; i < data.Length; i++)
            crc = (crc >> 8) ^ Table[(byte)(crc ^ data[i])];

        return crc ^ 0xFFFFFFFFu;
    }
#endif
}
