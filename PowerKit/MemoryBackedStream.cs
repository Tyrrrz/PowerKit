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
/// On readable and seekable streams, the entire content is loaded from the beginning and
/// the buffer position is set to match the source's original position. On non-seekable
/// readable streams, content is loaded from the current position and the buffer starts at 0.
/// </para>
/// <para>
/// Writes go to the in-memory buffer. When the wrapper is disposed, the buffer is written back
/// to the underlying stream. On readable and seekable streams the underlying stream is truncated
/// to the buffer length and seeked to the beginning before the write-back, ensuring the original
/// content is completely replaced.
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
            var initialPosition = source.CanSeek ? source.Position : 0L;

            if (source.CanSeek)
                source.Seek(0, SeekOrigin.Begin);

            source.CopyTo(_buffer);
            _buffer.Position = initialPosition;
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

            // If the full stream was loaded (readable + seekable), truncate the source
            // to the buffer's length and seek to the beginning so the write-back
            // completely replaces the original content without leaving trailing data.
            if (source.CanRead && source.CanSeek)
            {
                source.SetLength(_buffer.Length);
                source.Seek(0, SeekOrigin.Begin);
            }

            _buffer.CopyTo(source);
        }

        base.Dispose(disposing);
    }
}
