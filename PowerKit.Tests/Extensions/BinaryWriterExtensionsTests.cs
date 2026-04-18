using System.IO;
using System.Text;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class BinaryWriterExtensionsTests
{
    [Fact]
    public void SkipPadding_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        writer.Write((byte)0x01); // advance to position 1

        // Act
        writer.SkipPadding(4);

        // Assert
        stream.Position.Should().Be(4);
        stream.ToArray().Should().Equal(0x01, 0x00, 0x00, 0x00);
    }

    [Fact]
    public void SkipPadding_AlreadyAligned_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act (position 0 is already aligned to 4 bytes)
        writer.SkipPadding(4);

        // Assert
        stream.Position.Should().Be(0);
        stream.ToArray().Should().BeEmpty();
    }

    [Fact]
    public void WriteNullTerminatedString_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.ASCII);

        // Act
        writer.WriteNullTerminatedString("hello");

        // Assert
        stream.ToArray().Should().Equal("hello\0"u8.ToArray());
    }

    [Fact]
    public void WriteNullTerminatedString_Empty_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.ASCII);

        // Act
        writer.WriteNullTerminatedString("");

        // Assert
        stream.ToArray().Should().Equal(0);
    }
}
