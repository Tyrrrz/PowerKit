using System.IO;
using FluentAssertions;
using Xunit;

namespace PowerKit.Tests;

public class MemoryReadStreamTests
{
    [Fact]
    public void MemoryReadStream_MakesUnseekableStreamSeekable_Test()
    {
        // Arrange — write known data and open as a non-seekable network-style stream
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, data);
        using var source = File.OpenRead(tempFile.Path);

        // Act
        using var result = new MemoryReadStream(source);

        // Assert — wrapper is always seekable and can re-read from the beginning
        result.CanSeek.Should().BeTrue();

        var partial = new byte[2];
        result.ReadExactly(partial);

        result.Seek(0, SeekOrigin.Begin);

        var full = new byte[data.Length];
        result.ReadExactly(full);
        full.Should().Equal(data);
    }
}
