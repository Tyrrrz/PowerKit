using System.IO;
using FluentAssertions;
using Xunit;

namespace PowerKit.Tests;

public class MemoryWriteStreamTests
{
    [Fact]
    public void MemoryWriteStream_WritableFile_WriteBackOnDispose_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var tempFile = TempFile.Create();

        // Act — writes go to the in-memory buffer; dispose flushes them to the file
        using (var source = File.OpenWrite(tempFile.Path))
        using (var wrapper = new MemoryWriteStream(source))
        {
            wrapper.Write(data, 0, data.Length);
        }

        // Assert
        File.ReadAllBytes(tempFile.Path).Should().Equal(data);
    }

    [Fact]
    public void MemoryWriteStream_WritableFile_WriteBackAtCurrentPosition_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();

        // Act — open write-only, advance source to position 2, then wrap and write
        using (var source = File.OpenWrite(tempFile.Path))
        {
            // Write 2 placeholder bytes to advance source to position 2
            source.Write(new byte[] { 1, 2 }, 0, 2);

            using var wrapper = new MemoryWriteStream(source);
            wrapper.Write(new byte[] { 10, 20, 30 }, 0, 3);
            // dispose writes the 3-byte buffer at source's current position (2)
        }

        // Assert — placeholder bytes at 0..1, then buffer content at 2..4
        File.ReadAllBytes(tempFile.Path).Should().Equal(new byte[] { 1, 2, 10, 20, 30 });
    }

    [Fact]
    public void MemoryWriteStream_IsSeekable_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();
        using var source = File.OpenWrite(tempFile.Path);

        // Act
        using var wrapper = new MemoryWriteStream(source);

        // Assert
        wrapper.CanSeek.Should().BeTrue();
        wrapper.CanWrite.Should().BeTrue();
        wrapper.CanRead.Should().BeFalse();
    }
}
