using System.IO;
using FluentAssertions;
using Xunit;

namespace PowerKit.Tests;

public class MemoryBackedStreamTests
{
    [Fact]
    public void MemoryBackedStream_ReadableFile_ReadsCorrectly_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, data);
        using var source = File.OpenRead(tempFile.Path);

        // Act
        using var result = new MemoryBackedStream(source);

        // Assert
        result.CanRead.Should().BeTrue();
        var buffer = new byte[data.Length];
        result.ReadExactly(buffer);
        buffer.Should().Equal(data);
    }

    [Fact]
    public void MemoryBackedStream_ReadableFile_BuffersFromCurrentPosition_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, data);
        using var source = File.OpenRead(tempFile.Path);

        // Advance the source to position 2 before wrapping
        source.Seek(2, SeekOrigin.Begin);

        // Act
        using var result = new MemoryBackedStream(source);

        // Assert — buffer contains only the content from position 2 onwards,
        // starting at buffer position 0
        result.Position.Should().Be(0);
        result.Length.Should().Be(3);

        var tail = new byte[3];
        result.ReadExactly(tail);
        tail.Should().Equal(new byte[] { 3, 4, 5 });
    }

    [Fact]
    public void MemoryBackedStream_ReadableFile_IsSeekable_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, data);
        using var source = File.OpenRead(tempFile.Path);

        // Act
        using var result = new MemoryBackedStream(source);

        // Assert — wrapper is seekable
        result.CanSeek.Should().BeTrue();

        var partial = new byte[2];
        result.ReadExactly(partial);

        result.Seek(0, SeekOrigin.Begin);

        var full = new byte[data.Length];
        result.ReadExactly(full);
        full.Should().Equal(data);
    }

    [Fact]
    public void MemoryBackedStream_WritableFile_WriteBackAtCurrentPosition_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();

        // Act — open write-only, advance source to position 2, then wrap and write
        using (var source = File.OpenWrite(tempFile.Path))
        {
            // Write 2 placeholder bytes to advance source to position 2
            source.Write(new byte[] { 1, 2 }, 0, 2);

            using var wrapper = new MemoryBackedStream(source);
            wrapper.Write(new byte[] { 10, 20, 30 }, 0, 3);
            // dispose writes the 3-byte buffer at source's current position (2)
        }

        // Assert — placeholder bytes at 0..1, then buffer content at 2..4
        File.ReadAllBytes(tempFile.Path).Should().Equal(new byte[] { 1, 2, 10, 20, 30 });
    }

    [Fact]
    public void MemoryBackedStream_WritableFile_WriteBackOnDispose_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var tempFile = TempFile.Create();

        // Act — writes go to the in-memory buffer; dispose flushes them to the file
        using (var source = File.OpenWrite(tempFile.Path))
        using (var wrapper = new MemoryBackedStream(source))
        {
            wrapper.Write(data, 0, data.Length);
        }

        // Assert
        File.ReadAllBytes(tempFile.Path).Should().Equal(data);
    }
}
