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
        var a = 1u;
        var b = 0u;

        using var buffer = SpanPool<byte>.Shared.Rent(4096);

        while (stream.Read(buffer.Span) is > 0 and var bytesRead)
        {
            for (var i = 0; i < bytesRead; i++)
            {
                a = (a + buffer.Span[i]) % Modulus;
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
    /// <inheritdoc cref="Hash(byte[])" />
    public static uint Hash(ReadOnlySpan<byte> data)
    {
        var a = 1u;
        var b = 0u;

        for (var i = 0; i < data.Length; i++)
        {
            a = (a + data[i]) % Modulus;
            b = (b + a) % Modulus;
        }

        return (b << 16) | a;
    }
#endif
}
