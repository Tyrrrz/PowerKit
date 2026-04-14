#nullable enable
using System.IO;
using System.Text;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class BinaryWriterExtensionsTests
{
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
