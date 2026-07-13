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
    /// Compresses data from the source stream and writes it to the destination stream.
    /// </summary>
    public static void Compress(Stream source, Stream destination)
    {
        using (var deflateStream = new DeflateStream(destination, CompressionMode.Compress, true))
        {
            var buffer = new byte[4096];
            int read;
            while ((read = source.Read(buffer, 0, buffer.Length)) > 0)
                deflateStream.Write(buffer, 0, read);
        }
    }

    /// <summary>
    /// Decompresses data from the source stream and writes it to the destination stream.
    /// </summary>
    public static void Decompress(Stream source, Stream destination)
    {
        using (var deflateStream = new DeflateStream(source, CompressionMode.Decompress, true))
        {
            var buffer = new byte[4096];
            int read;
            while ((read = deflateStream.Read(buffer, 0, buffer.Length)) > 0)
                destination.Write(buffer, 0, read);
        }
    }

    /// <summary>
    /// Compresses the specified data using the Deflate algorithm.
    /// </summary>
    public static byte[] Compress(byte[] data)
    {
        using var input = new MemoryStream(data);
        using var output = new MemoryStream();
        Compress(input, output);
        return output.ToArray();
    }

    /// <summary>
    /// Decompresses the specified data using the Deflate algorithm.
    /// </summary>
    public static byte[] Decompress(byte[] data)
    {
        using var input = new MemoryStream(data);
        using var output = new MemoryStream();
        Decompress(input, output);
        return output.ToArray();
    }

#if !NETFRAMEWORK || NET45_OR_GREATER
    /// <summary>
    /// Compresses the specified data using the Deflate algorithm.
    /// </summary>
    public static byte[] Compress(ReadOnlySpan<byte> data) => Compress(data.ToArray());

    /// <summary>
    /// Decompresses the specified data using the Deflate algorithm.
    /// </summary>
    public static byte[] Decompress(ReadOnlySpan<byte> data) => Decompress(data.ToArray());
#endif
}
