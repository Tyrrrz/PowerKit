using System;
using System.IO;
using FluentAssertions;
using PowerKit.Tests.Utils;
using Xunit;

namespace PowerKit.Tests;

public class MemoryWriteStreamTests
{
    [Fact]
    public void MemoryWriteStream_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var destination = new MemoryStream();
        using var nonSeekable = new NonSeekableStream(destination);
        using var seekable = new MemoryWriteStream(nonSeekable);

        // Act
        seekable.Write([6, 6, 6, 6, 6]);

        seekable.Seek(0, SeekOrigin.Begin);

        seekable.Write(data);

        seekable.Flush();

        // Assert
        seekable.CanSeek.Should().BeTrue();
        destination.ToArray().Should().Equal(data);
    }

    [Fact]
    public void MemoryWriteStream_FlushTwice_Throws()
    {
        // Arrange
        using var destination = new MemoryStream();
        using var seekable = new MemoryWriteStream(destination);

        seekable.Write([1, 2, 3]);
        seekable.Flush();

        // Act & Assert
        seekable.Invoking(s => s.Flush()).Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void MemoryWriteStream_WriteAfterFlush_Throws()
    {
        // Arrange
        using var destination = new MemoryStream();
        using var seekable = new MemoryWriteStream(destination);

        seekable.Write([1, 2, 3]);
        seekable.Flush();

        // Act & Assert
        seekable.Invoking(s => s.Write([4, 5, 6])).Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void MemoryWriteStream_FlushThenDispose_DoesNotThrow()
    {
        // Arrange
        using var destination = new MemoryStream();
        var seekable = new MemoryWriteStream(destination);

        seekable.Write([1, 2, 3]);
        seekable.Flush();

        // Act & Assert
        seekable.Invoking(s => s.Dispose()).Should().NotThrow();
    }
}
