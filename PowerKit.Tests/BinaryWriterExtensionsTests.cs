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
        stream.ToArray().Should().Equal((byte)'h', (byte)'e', (byte)'l', (byte)'l', (byte)'o', 0);
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
