using System;
using System.IO;
using System.Text;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class BinaryReaderExtensionsTests
{
    [Fact]
    public void SkipZeroes_NonSeekableStream_Test()
    {
        // Arrange
        var data = new byte[] { 0, 0, 1, 2 };
        using var stream = new NonSeekableStream(data);
        using var reader = new BinaryReader(stream);

        // Act & assert
        var act = () => reader.SkipZeroes();
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void SkipZeroes_Test()
    {
        // Arrange
        var data = new byte[] { 0, 0, 0, 1, 2, 3 };
        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream);

        // Act
        reader.SkipZeroes();

        // Assert
        stream.Position.Should().Be(3);
        reader.ReadByte().Should().Be(1);
    }

    [Fact]
    public void SkipZeroes_WithMaxLength_Test()
    {
        // Arrange
        var data = new byte[] { 0, 0, 0, 0, 1, 2, 3 };
        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream);

        // Act
        reader.SkipZeroes(maxSkipLength: 2);

        // Assert
        stream.Position.Should().Be(2);
        reader.ReadByte().Should().Be(0);
    }

    [Fact]
    public void SkipZeroes_AllZeroes_Test()
    {
        // Arrange
        var data = new byte[] { 0, 0, 0 };
        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream);

        // Act
        reader.SkipZeroes();

        // Assert
        stream.Position.Should().Be(3);
    }

    [Fact]
    public void ReadNullTerminatedString_Test()
    {
        // Arrange
        var data = Encoding.ASCII.GetBytes("hello\0");
        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream, Encoding.ASCII);

        // Act
        var result = reader.ReadNullTerminatedString();

        // Assert
        result.Should().Be("hello");
    }

    [Fact]
    public void ReadNullTerminatedString_Empty_Test()
    {
        // Arrange
        var data = new byte[] { 0 };
        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream, Encoding.ASCII);

        // Act
        var result = reader.ReadNullTerminatedString();

        // Assert
        result.Should().Be("");
    }
}

file class NonSeekableStream(byte[] data) : MemoryStream(data)
{
    public override bool CanSeek => false;
}
