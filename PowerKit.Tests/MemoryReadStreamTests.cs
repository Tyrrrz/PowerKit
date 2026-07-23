using System.IO;
using FluentAssertions;
using Xunit;

namespace PowerKit.Tests;

public class MemoryReadStreamTests
{
    [Fact]
    public void MemoryReadStream_ReadableFile_ReadsCorrectly_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, data);
        using var source = File.OpenRead(tempFile.Path);

        // Act
        using var result = new MemoryReadStream(source);

        // Assert
        var buffer = new byte[data.Length];
        result.ReadExactly(buffer);
        buffer.Should().Equal(data);
    }

    [Fact]
    public void MemoryReadStream_ReadableFile_BuffersFromCurrentPosition_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, data);
        using var source = File.OpenRead(tempFile.Path);

        // Advance the source to position 2 before wrapping
        source.Seek(2, SeekOrigin.Begin);

        // Act
        using var result = new MemoryReadStream(source);

        // Assert — buffer contains only the content from position 2 onwards,
        // starting at buffer position 0
        result.Position.Should().Be(0);
        result.Length.Should().Be(3);

        var tail = new byte[3];
        result.ReadExactly(tail);
        tail.Should().Equal(new byte[] { 3, 4, 5 });
    }

    [Fact]
    public void MemoryReadStream_ReadableFile_IsSeekable_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, data);
        using var source = File.OpenRead(tempFile.Path);

        // Act
        using var result = new MemoryReadStream(source);

        // Assert — wrapper is seekable even though the source may not be
        result.CanSeek.Should().BeTrue();
        result.CanRead.Should().BeTrue();
        result.CanWrite.Should().BeFalse();

        var partial = new byte[2];
        result.ReadExactly(partial);

        result.Seek(0, SeekOrigin.Begin);

        var full = new byte[data.Length];
        result.ReadExactly(full);
        full.Should().Equal(data);
    }
}
