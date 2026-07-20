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
    // Read-only, non-seekable, non-MemoryStream wrapper
    private sealed class ReadOnlyStream(byte[] data) : Stream
    {
        private readonly MemoryStream _inner = new(data);

        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();
        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count) =>
            _inner.Read(buffer, offset, count);

        public override void Flush() { }

        public override long Seek(long offset, SeekOrigin origin) =>
            throw new NotSupportedException();

        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count) =>
            throw new NotSupportedException();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _inner.Dispose();
            base.Dispose(disposing);
        }
    }

    // Write-only, non-seekable, non-MemoryStream wrapper
    private sealed class WriteOnlyStream : Stream
    {
        private readonly MemoryStream _inner = new();

        public override bool CanRead => false;
        public override bool CanSeek => false;
        public override bool CanWrite => true;
        public override long Length => throw new NotSupportedException();
        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count) =>
            throw new NotSupportedException();

        public override void Flush() { }

        public override long Seek(long offset, SeekOrigin origin) =>
            throw new NotSupportedException();

        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count) =>
            _inner.Write(buffer, offset, count);

        public byte[] GetAll() => _inner.ToArray();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _inner.Dispose();
            base.Dispose(disposing);
        }
    }

    // Readable, writable, seekable, non-MemoryStream wrapper
    private sealed class ReadWriteStream : Stream
    {
        private readonly MemoryStream _inner;

        public ReadWriteStream(byte[] data)
        {
            _inner = new MemoryStream();
            _inner.Write(data, 0, data.Length);
            _inner.Position = 0;
        }

        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => true;
        public override long Length => _inner.Length;
        public override long Position
        {
            get => _inner.Position;
            set => _inner.Position = value;
        }

        public override int Read(byte[] buffer, int offset, int count) =>
            _inner.Read(buffer, offset, count);

        public override void Flush() { }

        public override long Seek(long offset, SeekOrigin origin) => _inner.Seek(offset, origin);

        public override void SetLength(long value) => _inner.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count) =>
            _inner.Write(buffer, offset, count);

        public byte[] GetAll()
        {
            _inner.Position = 0;
            return _inner.ToArray();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _inner.Dispose();
            base.Dispose(disposing);
        }
    }

    [Fact]
    public void ToMemoryStream_ReadOnlyStream_ReadsCorrectly_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var source = new ReadOnlyStream(data);

        // Act
        using var result = source.ToMemoryStream();

        // Assert — first read triggers buffering; all bytes are present at position 0
        result.CanRead.Should().BeTrue();
        var buffer = new byte[data.Length];
        result.ReadExactly(buffer);
        buffer.Should().Equal(data);
    }

    [Fact]
    public void ToMemoryStream_ReadOnlyStream_IsSeekable_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var source = new ReadOnlyStream(data);

        // Act
        using var result = source.ToMemoryStream();

        // Assert — wrapper is seekable even though source is not
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
    public void ToMemoryStream_WriteOnlyStream_WriteBackOnDispose_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var target = new WriteOnlyStream();

        // Act — writes go to the in-memory buffer; dispose flushes them to target
        using (var wrapper = target.ToMemoryStream())
        {
            wrapper.Write(data, 0, data.Length);
        }

        // Assert
        target.GetAll().Should().Equal(data);
    }

    [Fact]
    public void ToMemoryStream_ReadWriteStream_LoadsFullStreamAndWritesBack_Test()
    {
        // Arrange — source is positioned in the middle
        var initial = new byte[] { 1, 2, 3, 4, 5 };
        using var source = new ReadWriteStream(initial);
        source.Position = 3;

        // Act
        using (var wrapper = source.ToMemoryStream())
        {
            // Full stream is loaded from position 0 and original position is restored
            wrapper.Length.Should().Be(5);
            wrapper.Position.Should().Be(3);

            // Overwrite the first three bytes
            wrapper.Position = 0;
            wrapper.Write(new byte[] { 10, 20, 30 }, 0, 3);
        } // dispose writes the entire buffer back to source from position 0

        // Assert
        source.GetAll().Should().Equal(new byte[] { 10, 20, 30, 4, 5 });
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
