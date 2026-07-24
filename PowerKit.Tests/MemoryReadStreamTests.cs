using System.IO;
using FluentAssertions;
using PowerKit.Tests.Utils;
using Xunit;

namespace PowerKit.Tests;

public class MemoryReadStreamTests
{
    [Fact]
    public void MemoryReadStream_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var source = new MemoryStream(data);
        using var nonSeekable = new NonSeekableStream(source);
        using var seekable = new MemoryReadStream(nonSeekable);

        // Act
        var partial = new byte[2];
        seekable.ReadExactly(partial);

        seekable.Seek(0, SeekOrigin.Begin);

        var full = new byte[data.Length];
        seekable.ReadExactly(full);

        // Assert
        seekable.CanSeek.Should().BeTrue();
        full.Should().Equal(data);
    }
}
