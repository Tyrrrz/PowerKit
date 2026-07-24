using System;
using System.IO;

namespace PowerKit;

/// <summary>
/// A <see cref="Stream" /> wrapper that buffers all writes in memory and flushes them to the
/// underlying stream on <see cref="Flush()" />.
/// </summary>
/// <remarks>
/// Writes go to an in-memory buffer and do not touch the underlying stream until
/// <see cref="Flush()" /> is called. This makes the wrapper always seekable and allows writes to
/// be reordered freely before the final flush.
/// </remarks>
public class MemoryWriteStream(Stream source) : Stream
{
    private readonly MemoryStream _buffer = new();
    private bool _isFlushed;

    /// <inheritdoc />
    public override bool CanRead => false;

    /// <inheritdoc />
    public override bool CanSeek => true;

    /// <inheritdoc />
    public override bool CanWrite => true;

    /// <inheritdoc />
    public override long Length => _buffer.Length;

    /// <inheritdoc />
    public override long Position
    {
        get => _buffer.Position;
        set => _buffer.Position = value;
    }

    private void Flush(bool throwIfAlreadyFlushed)
    {
        if (_isFlushed)
        {
            if (throwIfAlreadyFlushed)
                throw new InvalidOperationException(
                    $"{nameof(MemoryWriteStream)} has already been flushed."
                );

            return;
        }

        _buffer.Position = 0;
        _buffer.CopyTo(source);
        source.Flush();
        _isFlushed = true;
    }

    /// <inheritdoc />
    public override void Flush() => Flush(true);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        Flush(false);
        base.Dispose(disposing);
    }

    /// <inheritdoc />
    public override long Seek(long offset, SeekOrigin origin) => _buffer.Seek(offset, origin);

    /// <inheritdoc />
    public override void SetLength(long value) => _buffer.SetLength(value);

    /// <inheritdoc />
    public override int Read(byte[] buffer, int offset, int count) =>
        throw new NotSupportedException();

    /// <inheritdoc />
    public override void Write(byte[] buffer, int offset, int count)
    {
        if (_isFlushed)
            throw new InvalidOperationException(
                $"{nameof(MemoryWriteStream)} has already been flushed."
            );

        _buffer.Write(buffer, offset, count);
    }
}
