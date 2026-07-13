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
        using var deflate = new DeflateStream(destination, CompressionMode.Compress, true);
        using var buffer = SpanPool<byte>.Shared.Rent(4096);

        while (source.Read(buffer.Span) is > 0 and var bytesRead)
            deflate.Write(buffer.Span[..bytesRead]);
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

#if !NETFRAMEWORK || NET45_OR_GREATER
    /// <summary>
    /// Compresses the specified data using the Deflate algorithm.
    /// </summary>
    public static byte[] Compress(ReadOnlySpan<byte> data) => Compress(data.ToArray());
#endif

    /// <summary>
    /// Decompresses data from the source stream and writes it to the destination stream.
    /// </summary>
    public static void Decompress(Stream source, Stream destination)
    {
        using var deflate = new DeflateStream(source, CompressionMode.Decompress, true);
        using var buffer = SpanPool<byte>.Shared.Rent(4096);

        while (deflate.Read(buffer.Span) is > 0 and var bytesRead)
            destination.Write(buffer.Span[..bytesRead]);
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
    /// Decompresses the specified data using the Deflate algorithm.
    /// </summary>
    public static byte[] Decompress(ReadOnlySpan<byte> data) => Decompress(data.ToArray());
#endif
}
