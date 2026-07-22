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

    [Fact]
    public void ReadInt16BigEndian_Test()
    {
        // Arrange
        var data = new byte[] { 0x01, 0x02 };
        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream);

        // Act
        var result = reader.ReadInt16BigEndian();

        // Assert
        result.Should().Be(0x0102);
    }

    [Fact]
    public void ReadInt16LittleEndian_Test()
    {
        // Arrange
        var data = new byte[] { 0x02, 0x01 };
        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream);

        // Act
        var result = reader.ReadInt16LittleEndian();

        // Assert
        result.Should().Be(0x0102);
    }

    [Fact]
    public void ReadUInt16BigEndian_Test()
    {
        // Arrange
        var data = new byte[] { 0xFF, 0xFE };
        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream);

        // Act
        var result = reader.ReadUInt16BigEndian();

        // Assert
        result.Should().Be((ushort)0xFFFE);
    }

    [Fact]
    public void ReadUInt16LittleEndian_Test()
    {
        // Arrange
        var data = new byte[] { 0xFE, 0xFF };
        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream);

        // Act
        var result = reader.ReadUInt16LittleEndian();

        // Assert
        result.Should().Be((ushort)0xFFFE);
    }

    [Fact]
    public void ReadInt32BigEndian_Test()
    {
        // Arrange
        var data = new byte[] { 0x01, 0x02, 0x03, 0x04 };
        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream);

        // Act
        var result = reader.ReadInt32BigEndian();

        // Assert
        result.Should().Be(0x01020304);
    }

    [Fact]
    public void ReadInt32LittleEndian_Test()
    {
        // Arrange
        var data = new byte[] { 0x04, 0x03, 0x02, 0x01 };
        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream);

        // Act
        var result = reader.ReadInt32LittleEndian();

        // Assert
        result.Should().Be(0x01020304);
    }

    [Fact]
    public void ReadUInt32BigEndian_Test()
    {
        // Arrange
        var data = new byte[] { 0xFF, 0xFE, 0xFD, 0xFC };
        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream);

        // Act
        var result = reader.ReadUInt32BigEndian();

        // Assert
        result.Should().Be(0xFFFEFDFCu);
    }

    [Fact]
    public void ReadUInt32LittleEndian_Test()
    {
        // Arrange
        var data = new byte[] { 0xFC, 0xFD, 0xFE, 0xFF };
        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream);

        // Act
        var result = reader.ReadUInt32LittleEndian();

        // Assert
        result.Should().Be(0xFFFEFDFCu);
    }

    [Fact]
    public void ReadInt64BigEndian_Test()
    {
        // Arrange
        var data = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream);

        // Act
        var result = reader.ReadInt64BigEndian();

        // Assert
        result.Should().Be(0x0102030405060708L);
    }

    [Fact]
    public void ReadInt64LittleEndian_Test()
    {
        // Arrange
        var data = new byte[] { 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01 };
        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream);

        // Act
        var result = reader.ReadInt64LittleEndian();

        // Assert
        result.Should().Be(0x0102030405060708L);
    }

    [Fact]
    public void ReadUInt64BigEndian_Test()
    {
        // Arrange
        var data = new byte[] { 0xFF, 0xFE, 0xFD, 0xFC, 0xFB, 0xFA, 0xF9, 0xF8 };
        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream);

        // Act
        var result = reader.ReadUInt64BigEndian();

        // Assert
        result.Should().Be(0xFFFEFDFCFBFAF9F8uL);
    }

    [Fact]
    public void ReadUInt64LittleEndian_Test()
    {
        // Arrange
        var data = new byte[] { 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF };
        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream);

        // Act
        var result = reader.ReadUInt64LittleEndian();

        // Assert
        result.Should().Be(0xFFFEFDFCFBFAF9F8uL);
    }

    [Fact]
    public void ReadSingleBigEndian_Test()
    {
        // Arrange — IEEE 754 big-endian bytes for 1.0f: 0x3F800000
        var data = new byte[] { 0x3F, 0x80, 0x00, 0x00 };
        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream);

        // Act
        var result = reader.ReadSingleBigEndian();

        // Assert
        result.Should().Be(1.0f);
    }

    [Fact]
    public void ReadSingleLittleEndian_Test()
    {
        // Arrange — IEEE 754 little-endian bytes for 1.0f: 0x3F800000 reversed
        var data = new byte[] { 0x00, 0x00, 0x80, 0x3F };
        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream);

        // Act
        var result = reader.ReadSingleLittleEndian();

        // Assert
        result.Should().Be(1.0f);
    }

    [Fact]
    public void ReadDoubleBigEndian_Test()
    {
        // Arrange — IEEE 754 big-endian bytes for 1.0d: 0x3FF0000000000000
        var data = new byte[] { 0x3F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream);

        // Act
        var result = reader.ReadDoubleBigEndian();

        // Assert
        result.Should().Be(1.0d);
    }

    [Fact]
    public void ReadDoubleLittleEndian_Test()
    {
        // Arrange — IEEE 754 little-endian bytes for 1.0d: 0x3FF0000000000000 reversed
        var data = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF0, 0x3F };
        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream);

        // Act
        var result = reader.ReadDoubleLittleEndian();

        // Assert
        result.Should().Be(1.0d);
    }
}
