using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Gress;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class StreamExtensionsTests
{
    [Fact]
    public void ToMemoryStream_ReadableFile_ReadsCorrectly_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, data);
        using var source = File.OpenRead(tempFile.Path);

        // Act
        using var result = source.ToMemoryStream();

        // Assert
        result.CanRead.Should().BeTrue();
        var buffer = new byte[data.Length];
        result.ReadExactly(buffer);
        buffer.Should().Equal(data);
    }

    [Fact]
    public void ToMemoryStream_ReadableFile_IsSeekable_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, data);
        using var source = File.OpenRead(tempFile.Path);

        // Act
        using var result = source.ToMemoryStream();

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
    public void ToMemoryStream_MemoryStream_ReturnsNoOp_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var source = new MemoryStream(data);

        // Act
        var result = source.ToMemoryStream();

        // Assert — MemoryStream source is returned as-is
        result.Should().BeSameAs(source);
    }

    [Fact]
    public void ToMemoryStream_WritableFile_WriteBackOnDispose_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var tempFile = TempFile.Create();

        // Act — writes go to the in-memory buffer; dispose flushes them to the file
        using (var source = File.OpenWrite(tempFile.Path))
        using (var wrapper = source.ToMemoryStream())
        {
            wrapper.Write(data, 0, data.Length);
        }

        // Assert
        File.ReadAllBytes(tempFile.Path).Should().Equal(data);
    }

    [Fact]
    public void ToMemoryStream_ReadWriteFile_LoadsFullStreamAndWritesBack_Test()
    {
        // Arrange
        var initial = new byte[] { 1, 2, 3, 4, 5 };
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, initial);

        // Act
        using (var source = File.Open(tempFile.Path, FileMode.Open, FileAccess.ReadWrite))
        {
            using (var wrapper = source.ToMemoryStream())
            {
                // Full stream is loaded from position 0
                wrapper.Length.Should().Be(5);

                // Overwrite the first three bytes
                wrapper.Position = 0;
                wrapper.Write(new byte[] { 10, 20, 30 }, 0, 3);
            } // dispose writes the entire buffer back to source from position 0
        } // source FileStream is flushed and closed

        // Assert
        File.ReadAllBytes(tempFile.Path).Should().Equal(new byte[] { 10, 20, 30, 4, 5 });
    }

    [Fact]
    public async Task CopyToAsync_AutoFlush_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var source = new MemoryStream(data);
        using var destination = new MemoryStream();

        // Act
        await source.CopyToAsync(destination, true);

        // Assert
        destination.ToArray().Should().Equal(data);
    }

    [Fact]
    public async Task CopyToAsync_Progress_Test()
    {
        // Arrange
        var data = new byte[1024];
        using var source = new MemoryStream(data);
        using var destination = new MemoryStream();

        var progress = new ProgressCollector<double>();

        // Act
        await source.CopyToAsync(destination, progress);

        // Assert
        var reports = progress.GetValues().ToArray();
        reports.Should().NotBeEmpty();
        reports.Should().AllSatisfy(v => v.Should().BeInRange(0.0, 1.0));
        reports[^1].Should().BeApproximately(1.0, 1e-5);
    }
}
