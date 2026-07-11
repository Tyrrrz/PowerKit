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

        for (uint i = 0; i < 256; i++)
        {
            var entry = i;
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

#if !NETFRAMEWORK || NET45_OR_GREATER
    /// <summary>
    /// Computes the CRC-32 checksum of the specified data.
    /// </summary>
    public static uint Hash(ReadOnlySpan<byte> data)
    {
        var crc = 0xFFFFFFFFu;

        for (var i = 0; i < data.Length; i++)
            crc = (crc >> 8) ^ Table[(crc ^ data[i]) & 0xFF];

        return crc ^ 0xFFFFFFFFu;
    }

    /// <summary>
    /// Computes the CRC-32 checksum of the specified data.
    /// </summary>
    public static uint Hash(byte[] data) => Hash((ReadOnlySpan<byte>)data);
#else
    /// <summary>
    /// Computes the CRC-32 checksum of the specified data.
    /// </summary>
    public static uint Hash(byte[] data)
    {
        var crc = 0xFFFFFFFFu;

        for (var i = 0; i < data.Length; i++)
            crc = (crc >> 8) ^ Table[(crc ^ data[i]) & 0xFF];

        return crc ^ 0xFFFFFFFFu;
    }
#endif
}
