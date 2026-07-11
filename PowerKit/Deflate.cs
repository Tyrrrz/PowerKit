using System.IO;
using System.IO.Compression;
#if !NETFRAMEWORK || NET45_OR_GREATER
using System;
#endif

namespace PowerKit;

/// <summary>
/// Provides methods for compressing and decompressing data using the Deflate algorithm.
/// </summary>
public static class Deflate
{
    /// <summary>
    /// Compresses the specified data using the Deflate algorithm.
    /// </summary>
    public static byte[] Compress(byte[] data)
    {
        using var output = new MemoryStream();

        using (var stream = new DeflateStream(output, CompressionMode.Compress, true))
            stream.Write(data, 0, data.Length);

        return output.ToArray();
    }

    /// <summary>
    /// Decompresses the specified data using the Deflate algorithm.
    /// </summary>
    public static byte[] Decompress(byte[] data)
    {
        using var input = new MemoryStream(data);
        using var output = new MemoryStream();

        using (var stream = new DeflateStream(input, CompressionMode.Decompress, true))
        {
            var buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                output.Write(buffer, 0, bytesRead);
        }

        return output.ToArray();
    }

#if !NETFRAMEWORK || NET45_OR_GREATER
    /// <summary>
    /// Compresses the specified data using the Deflate algorithm.
    /// </summary>
    public static byte[] Compress(ReadOnlySpan<byte> data)
    {
        using var output = new MemoryStream();

        using (var stream = new DeflateStream(output, CompressionMode.Compress, true))
        {
#if NET6_0_OR_GREATER
            stream.Write(data);
#else
            stream.Write(data.ToArray(), 0, data.Length);
#endif
        }

        return output.ToArray();
    }

    /// <summary>
    /// Decompresses the specified data using the Deflate algorithm.
    /// </summary>
    public static byte[] Decompress(ReadOnlySpan<byte> data) => Decompress(data.ToArray());
#endif
}
