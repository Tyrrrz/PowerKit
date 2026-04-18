using System.IO;
using System.Text;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class BinaryReaderExtensionsTests
{
    [Fact]
    public void IsEndOfStream_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3 };
        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream);

        // Act & Assert
        reader.IsEndOfStream.Should().BeFalse();
        stream.Position = stream.Length;
        reader.IsEndOfStream.Should().BeTrue();
    }

    [Fact]
    public void IsEndOfStream_Empty_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var reader = new BinaryReader(stream);

        // Act & Assert
        reader.IsEndOfStream.Should().BeTrue();
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
        result.Should().BeEmpty();
    }

    [Fact]
    public void SkipPadding_Test()
    {
        // Arrange - 1 byte of data, then 3 padding bytes, then 1 more byte
        var data = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x02 };
        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream);

        reader.ReadByte(); // advance to position 1

        // Act
        reader.SkipPadding(4);

        // Assert
        stream.Position.Should().Be(4);
    }

    [Fact]
    public void SkipPadding_AlreadyAligned_Test()
    {
        // Arrange
        var data = new byte[] { 0x01, 0x02, 0x03, 0x04 };
        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream);

        // Act (position 0 is already aligned to 4 bytes)
        reader.SkipPadding(4);

        // Assert
        stream.Position.Should().Be(0);
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
        reader.SkipZeroes(2);

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
}
