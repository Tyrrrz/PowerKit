using System;
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
    // A non-MemoryStream wrapper used to exercise the copy path
    private sealed class NonMemoryStream(Stream inner) : Stream
    {
        public override bool CanRead => inner.CanRead;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();
        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override void Flush() => inner.Flush();

        public override int Read(byte[] buffer, int offset, int count) =>
            inner.Read(buffer, offset, count);

        public override long Seek(long offset, SeekOrigin origin) =>
            throw new NotSupportedException();

        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count) =>
            throw new NotSupportedException();
    }

    [Fact]
    public void ToMemoryStream_RegularStream_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var inner = new MemoryStream(data);
        using var source = new NonMemoryStream(inner);

        // Act
        using var result = source.ToMemoryStream();

        // Assert
        result.Position.Should().Be(0);
        result.ToArray().Should().Equal(data);
    }

    [Fact]
    public void ToMemoryStream_AlreadyMemoryStream_ReturnsSameInstance_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var source = new MemoryStream(data);

        // Act
        var result = source.ToMemoryStream();

        // Assert
        result.Should().BeSameAs(source);
    }

    [Fact]
    public async Task ToMemoryStreamAsync_RegularStream_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var inner = new MemoryStream(data);
        using var source = new NonMemoryStream(inner);

        // Act
        using var result = await source.ToMemoryStreamAsync();

        // Assert
        result.Position.Should().Be(0);
        result.ToArray().Should().Equal(data);
    }

    [Fact]
    public async Task ToMemoryStreamAsync_AlreadyMemoryStream_ReturnsSameInstance_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var source = new MemoryStream(data);

        // Act
        var result = await source.ToMemoryStreamAsync();

        // Assert
        result.Should().BeSameAs(source);
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
