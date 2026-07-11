using System;
using FluentAssertions;
using PowerKit;
using Xunit;

namespace PowerKit.Tests;

public class DeflateTests
{
    [Fact]
    public void Compress_Decompress_ByteArray_Test()
    {
        // Arrange
        var data = "hello world"u8.ToArray();

        // Act
        var compressed = Deflate.Compress(data);
        var decompressed = Deflate.Decompress(compressed);

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
