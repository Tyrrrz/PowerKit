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
    public void WriteBigEndian_Int16_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteBigEndian((short)0x0102);

        // Assert
        stream.ToArray().Should().Equal(0x01, 0x02);
    }

    [Fact]
    public void WriteLittleEndian_Int16_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteLittleEndian((short)0x0102);

        // Assert
        stream.ToArray().Should().Equal(0x02, 0x01);
    }

    [Fact]
    public void WriteBigEndian_UInt16_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteBigEndian((ushort)0xFFFE);

        // Assert
        stream.ToArray().Should().Equal(0xFF, 0xFE);
    }

    [Fact]
    public void WriteLittleEndian_UInt16_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteLittleEndian((ushort)0xFFFE);

        // Assert
        stream.ToArray().Should().Equal(0xFE, 0xFF);
    }

    [Fact]
    public void WriteBigEndian_Int32_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteBigEndian(0x01020304);

        // Assert
        stream.ToArray().Should().Equal(0x01, 0x02, 0x03, 0x04);
    }

    [Fact]
    public void WriteLittleEndian_Int32_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteLittleEndian(0x01020304);

        // Assert
        stream.ToArray().Should().Equal(0x04, 0x03, 0x02, 0x01);
    }

    [Fact]
    public void WriteBigEndian_UInt32_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteBigEndian(0xFFFEFDFCu);

        // Assert
        stream.ToArray().Should().Equal(0xFF, 0xFE, 0xFD, 0xFC);
    }

    [Fact]
    public void WriteLittleEndian_UInt32_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteLittleEndian(0xFFFEFDFCu);

        // Assert
        stream.ToArray().Should().Equal(0xFC, 0xFD, 0xFE, 0xFF);
    }

    [Fact]
    public void WriteBigEndian_Int64_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteBigEndian(0x0102030405060708L);

        // Assert
        stream.ToArray().Should().Equal(0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08);
    }

    [Fact]
    public void WriteLittleEndian_Int64_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteLittleEndian(0x0102030405060708L);

        // Assert
        stream.ToArray().Should().Equal(0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01);
    }

    [Fact]
    public void WriteBigEndian_UInt64_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteBigEndian(0xFFFEFDFCFBFAF9F8uL);

        // Assert
        stream.ToArray().Should().Equal(0xFF, 0xFE, 0xFD, 0xFC, 0xFB, 0xFA, 0xF9, 0xF8);
    }

    [Fact]
    public void WriteLittleEndian_UInt64_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteLittleEndian(0xFFFEFDFCFBFAF9F8uL);

        // Assert
        stream.ToArray().Should().Equal(0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF);
    }

    [Fact]
    public void WriteBigEndian_Single_Test()
    {
        // Arrange — IEEE 754 big-endian bytes for 1.0f: 0x3F800000
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteBigEndian(1.0f);

        // Assert
        stream.ToArray().Should().Equal(0x3F, 0x80, 0x00, 0x00);
    }

    [Fact]
    public void WriteLittleEndian_Single_Test()
    {
        // Arrange — IEEE 754 little-endian bytes for 1.0f: 0x3F800000 reversed
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteLittleEndian(1.0f);

        // Assert
        stream.ToArray().Should().Equal(0x00, 0x00, 0x80, 0x3F);
    }

    [Fact]
    public void WriteBigEndian_Double_Test()
    {
        // Arrange — IEEE 754 big-endian bytes for 1.0d: 0x3FF0000000000000
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteBigEndian(1.0d);

        // Assert
        stream.ToArray().Should().Equal(0x3F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00);
    }

    [Fact]
    public void WriteLittleEndian_Double_Test()
    {
        // Arrange — IEEE 754 little-endian bytes for 1.0d: 0x3FF0000000000000 reversed
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Act
        writer.WriteLittleEndian(1.0d);

        // Assert
        stream.ToArray().Should().Equal(0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF0, 0x3F);
    }
}
