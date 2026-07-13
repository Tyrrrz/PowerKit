using System;
using System.IO;
using FluentAssertions;
using Xunit;

namespace PowerKit.Tests;

public class Crc32Tests
{
    [Fact]
    public void Hash_Stream_Test()
    {
        // Arrange
        using var stream = new MemoryStream("123456789"u8.ToArray());

        // Act
        var hash = Crc32.Hash(stream);

        // Assert
        hash.Should().Be(0xCBF43926u);
    }

    [Fact]
    public void Hash_ByteArray_Test()
    {
        // Arrange
        var data = "123456789"u8.ToArray();

        // Act
        var hash = Crc32.Hash(data);

        // Assert
        hash.Should().Be(0xCBF43926u);
    }

    [Fact]
    public void Hash_Span_Test()
    {
        // Arrange
        var data = "123456789"u8.ToArray();

        // Act
        var hash = Crc32.Hash((ReadOnlySpan<byte>)data);

        // Assert
        hash.Should().Be(0xCBF43926u);
    }

    [Fact]
    public void Hash_EmptyData_Test()
    {
        // Act
        var hash = Crc32.Hash([]);

        // Assert
        hash.Should().Be(0u);
    }
}
