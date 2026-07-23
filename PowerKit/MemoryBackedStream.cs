using System;
using System.IO;

namespace PowerKit;

/// <summary>
/// A <see cref="Stream" /> wrapper that lazily backs any stream with an in-memory <see cref="MemoryStream" />.
/// </summary>
/// <remarks>
/// <para>
/// The first read, write, seek, or access to <see cref="Length" /> or <see cref="Position" />
/// lazily loads the underlying stream into memory. Subsequent operations work directly against
/// the in-memory buffer, making the stream always seekable regardless of whether the underlying
/// stream supports seeking.
/// </para>
/// <para>
/// Content is loaded from the source stream's current position when the buffer is first
/// initialized. The buffer position always starts at 0.
/// </para>
/// <para>
/// Writes go to the in-memory buffer. When the wrapper is disposed, the buffer is written back
/// to the underlying stream at its current position. If the underlying stream is seekable and
/// contains more data after the write-back range, that trailing data is not removed.
/// </para>
/// </remarks>
public sealed class MemoryBackedStream(Stream source) : Stream
{
    private MemoryStream? _buffer;

    private MemoryStream EnsureBuffer()
    {
        if (_buffer is not null)
            return _buffer;

        _buffer = new MemoryStream();

        if (source.CanRead)
        {
            source.CopyTo(_buffer);
            _buffer.Position = 0;
        }

        return _buffer;
    }

    /// <inheritdoc />
    public override bool CanRead => source.CanRead;

    /// <inheritdoc />
    public override bool CanSeek => true;

    /// <inheritdoc />
    public override bool CanWrite => source.CanWrite;

    /// <inheritdoc />
    public override long Length => EnsureBuffer().Length;

    /// <inheritdoc />
    public override long Position
    {
        get => EnsureBuffer().Position;
        set => EnsureBuffer().Position = value;
    }

    /// <inheritdoc />
    public override void Flush() => _buffer?.Flush();

    /// <inheritdoc />
    public override int Read(byte[] buffer, int offset, int count)
    {
        if (!source.CanRead)
            throw new NotSupportedException("Stream does not support reading.");

        return EnsureBuffer().Read(buffer, offset, count);
    }

    /// <inheritdoc />
    public override long Seek(long offset, SeekOrigin origin) =>
        EnsureBuffer().Seek(offset, origin);

    /// <inheritdoc />
    public override void SetLength(long value) => EnsureBuffer().SetLength(value);

    /// <inheritdoc />
    public override void Write(byte[] buffer, int offset, int count)
    {
        if (!source.CanWrite)
            throw new NotSupportedException("Stream does not support writing.");

        EnsureBuffer().Write(buffer, offset, count);
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing && _buffer is not null && source.CanWrite)
        {
            _buffer.Position = 0;
            _buffer.CopyTo(source);
        }

        base.Dispose(disposing);
    }
}
