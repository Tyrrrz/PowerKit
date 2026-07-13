using System.IO;
#if !NETFRAMEWORK || NET45_OR_GREATER
using System;
#endif

namespace PowerKit;

/// <summary>
/// Provides methods for computing Adler-32 checksums.
/// </summary>
public static class Adler32
{
    private const uint Modulus = 65521;

    /// <summary>
    /// Computes the Adler-32 checksum of data read from the specified stream.
    /// </summary>
    public static uint Hash(Stream stream)
    {
        uint a = 1,
            b = 0;

        var buffer = new byte[4096];
        int read;

        while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            for (var i = 0; i < read; i++)
            {
                a = (a + buffer[i]) % Modulus;
                b = (b + a) % Modulus;
            }
        }

        return (b << 16) | a;
    }

    /// <summary>
    /// Computes the Adler-32 checksum of the specified data.
    /// </summary>
    public static uint Hash(byte[] data)
    {
        using var stream = new MemoryStream(data);
        return Hash(stream);
    }

#if !NETFRAMEWORK || NET45_OR_GREATER
    /// <summary>
    /// Computes the Adler-32 checksum of the specified data.
    /// </summary>
    public static uint Hash(ReadOnlySpan<byte> data)
    {
        uint a = 1,
            b = 0;

        for (var i = 0; i < data.Length; i++)
        {
            a = (a + data[i]) % Modulus;
            b = (b + a) % Modulus;
        }

        return (b << 16) | a;
    }
#endif
}
