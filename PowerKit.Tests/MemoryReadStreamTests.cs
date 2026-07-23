using System;
using System.IO;
using FluentAssertions;
using Xunit;

namespace PowerKit.Tests;

public class MemoryReadStreamTests
{
    private sealed class NonSeekableStream(Stream inner) : Stream
    {
        public override bool CanRead => inner.CanRead;
        public override bool CanSeek => false;
        public override bool CanWrite => inner.CanWrite;
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
            inner.Write(buffer, offset, count);

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                inner.Dispose();
            base.Dispose(disposing);
        }
    }

    [Fact]
    public void MemoryReadStream_MakesUnseekableStreamSeekable_Test()
    {
        // Arrange — wrap a MemoryStream in a non-seekable facade
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var source = new NonSeekableStream(new MemoryStream(data));
        source.CanSeek.Should().BeFalse();

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
