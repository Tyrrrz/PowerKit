using System;
using FluentAssertions;
using PowerKit;
using Xunit;

namespace PowerKit.Tests;

public class Crc32Tests
{
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
        var hash = Crc32.Hash(Array.Empty<byte>());

        // Assert
        hash.Should().Be(0u);
    }
}
