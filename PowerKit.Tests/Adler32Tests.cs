using System;
using FluentAssertions;
using PowerKit;
using Xunit;

namespace PowerKit.Tests;

public class Adler32Tests
{
    [Fact]
    public void Hash_ByteArray_Test()
    {
        // Arrange
        var data = "Wikipedia"u8.ToArray();

        // Act
        var hash = Adler32.Hash(data);

        // Assert
        hash.Should().Be(0x11E60398u);
    }

    [Fact]
    public void Hash_Span_Test()
    {
        // Arrange
        var data = "Wikipedia"u8.ToArray();

        // Act
        var hash = Adler32.Hash((ReadOnlySpan<byte>)data);

        // Assert
        hash.Should().Be(0x11E60398u);
    }

    [Fact]
    public void Hash_EmptyData_Test()
    {
        // Act
        var hash = Adler32.Hash(Array.Empty<byte>());

        // Assert
        hash.Should().Be(1u);
    }
}
