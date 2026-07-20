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

    [Fact]
    public void WriteInt16BigEndian_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteInt16BigEndian(0x0102);

        // Assert
        stream.ToArray().Should().Equal(0x01, 0x02);
    }

    [Fact]
    public void WriteInt16LittleEndian_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteInt16LittleEndian(0x0102);

        // Assert
        stream.ToArray().Should().Equal(0x02, 0x01);
    }

    [Fact]
    public void WriteUInt16BigEndian_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteUInt16BigEndian((ushort)0xFFFE);

        // Assert
        stream.ToArray().Should().Equal(0xFF, 0xFE);
    }

    [Fact]
    public void WriteUInt16LittleEndian_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteUInt16LittleEndian((ushort)0xFFFE);

        // Assert
        stream.ToArray().Should().Equal(0xFE, 0xFF);
    }

    [Fact]
    public void WriteInt32BigEndian_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteInt32BigEndian(0x01020304);

        // Assert
        stream.ToArray().Should().Equal(0x01, 0x02, 0x03, 0x04);
    }

    [Fact]
    public void WriteInt32LittleEndian_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteInt32LittleEndian(0x01020304);

        // Assert
        stream.ToArray().Should().Equal(0x04, 0x03, 0x02, 0x01);
    }

    [Fact]
    public void WriteUInt32BigEndian_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteUInt32BigEndian(0xFFFEFDFCu);

        // Assert
        stream.ToArray().Should().Equal(0xFF, 0xFE, 0xFD, 0xFC);
    }

    [Fact]
    public void WriteUInt32LittleEndian_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteUInt32LittleEndian(0xFFFEFDFCu);

        // Assert
        stream.ToArray().Should().Equal(0xFC, 0xFD, 0xFE, 0xFF);
    }

    [Fact]
    public void WriteInt64BigEndian_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteInt64BigEndian(0x0102030405060708L);

        // Assert
        stream.ToArray().Should().Equal(0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08);
    }

    [Fact]
    public void WriteInt64LittleEndian_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteInt64LittleEndian(0x0102030405060708L);

        // Assert
        stream.ToArray().Should().Equal(0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01);
    }

    [Fact]
    public void WriteUInt64BigEndian_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteUInt64BigEndian(0xFFFEFDFCFBFAF9F8uL);

        // Assert
        stream.ToArray().Should().Equal(0xFF, 0xFE, 0xFD, 0xFC, 0xFB, 0xFA, 0xF9, 0xF8);
    }

    [Fact]
    public void WriteUInt64LittleEndian_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteUInt64LittleEndian(0xFFFEFDFCFBFAF9F8uL);

        // Assert
        stream.ToArray().Should().Equal(0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF);
    }

    [Fact]
    public void WriteSingleBigEndian_Test()
    {
        // Arrange — IEEE 754 big-endian bytes for 1.0f: 0x3F800000
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteSingleBigEndian(1.0f);

        // Assert
        stream.ToArray().Should().Equal(0x3F, 0x80, 0x00, 0x00);
    }

    [Fact]
    public void WriteSingleLittleEndian_Test()
    {
        // Arrange — IEEE 754 little-endian bytes for 1.0f: 0x3F800000 reversed
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteSingleLittleEndian(1.0f);

        // Assert
        stream.ToArray().Should().Equal(0x00, 0x00, 0x80, 0x3F);
    }

    [Fact]
    public void WriteDoubleBigEndian_Test()
    {
        // Arrange — IEEE 754 big-endian bytes for 1.0d: 0x3FF0000000000000
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteDoubleBigEndian(1.0d);

        // Assert
        stream.ToArray().Should().Equal(0x3F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00);
    }

    [Fact]
    public void WriteDoubleLittleEndian_Test()
    {
        // Arrange — IEEE 754 little-endian bytes for 1.0d: 0x3FF0000000000000 reversed
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteDoubleLittleEndian(1.0d);

        // Assert
        stream.ToArray().Should().Equal(0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF0, 0x3F);
    }
}
