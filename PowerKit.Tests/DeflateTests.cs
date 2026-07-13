using System;
using System.IO;
using FluentAssertions;
using PowerKit;
using Xunit;

namespace PowerKit.Tests;

public class DeflateTests
{
    [Fact]
    public void Compress_Decompress_Stream_Test()
    {
        // Arrange
        var data = "Hello world!"u8.ToArray();
        using var input = new MemoryStream(data);
        using var compressed = new MemoryStream();
        using var decompressed = new MemoryStream();

        // Act
        Deflate.Compress(input, compressed);
        compressed.Position = 0;
        Deflate.Decompress(compressed, decompressed);

        // Assert
        decompressed.ToArray().Should().Equal(data);
    }

    [Fact]
    public void Compress_Decompress_ByteArray_Test()
    {
        // Arrange
        var data = "Hello world!"u8.ToArray();

        // Act
        var compressed = Deflate.Compress(data);
        var decompressed = Deflate.Decompress(compressed);

        // Assert
        decompressed.Should().Equal(data);
    }

    [Fact]
    public void Compress_Decompress_Span_Test()
    {
        // Arrange
        var data = "Hello world!"u8.ToArray();

        // Act
        var compressed = Deflate.Compress((ReadOnlySpan<byte>)data);
        var decompressed = Deflate.Decompress((ReadOnlySpan<byte>)compressed);

        // Assert
        decompressed.Should().Equal(data);
    }

    [Fact]
    public void Compress_Decompress_EmptyData_Test()
    {
        // Arrange
        var data = Array.Empty<byte>();

        // Act
        var compressed = Deflate.Compress(data);
        var decompressed = Deflate.Decompress(compressed);

        // Assert
        decompressed.Should().BeEmpty();
    }

    [Fact]
    public void Compress_ProducesCompressedOutput_Test()
    {
        // Arrange
        var data = new byte[1000];
        for (var i = 0; i < data.Length; i++)
            data[i] = (byte)(i % 4);

        // Act
        var compressed = Deflate.Compress(data);

        // Assert
        compressed.Should().HaveCountLessThan(data.Length);
    }
}
