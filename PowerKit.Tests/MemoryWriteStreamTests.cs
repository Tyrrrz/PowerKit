using System.IO;
using FluentAssertions;
using Xunit;

namespace PowerKit.Tests;

public class MemoryWriteStreamTests
{
    [Fact]
    public void MemoryWriteStream_MakesUnseekableStreamSeekable_Test()
    {
        // Arrange — write-only file stream is not seekable after the initial position
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var tempFile = TempFile.Create();

        // Act — wrapper buffers writes in memory; Flush() commits them to the source
        using (var source = File.OpenWrite(tempFile.Path))
        using (var wrapper = new MemoryWriteStream(source))
        {
            wrapper.CanSeek.Should().BeTrue();

            // Write all bytes then seek back and overwrite the first two
            wrapper.Write(data, 0, data.Length);
            wrapper.Seek(0, SeekOrigin.Begin);
            wrapper.Write(new byte[] { 10, 20 }, 0, 2);

            wrapper.Flush();
        }

        // Assert — overwritten prefix is reflected in the file
        File.ReadAllBytes(tempFile.Path).Should().Equal(new byte[] { 10, 20, 3, 4, 5 });
    }
}
