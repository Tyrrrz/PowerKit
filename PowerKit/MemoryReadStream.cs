using System.IO;

namespace PowerKit;

/// <summary>
/// A <see cref="Stream" /> wrapper that lazily loads a readable stream into an in-memory buffer,
/// making it fully seekable regardless of whether the underlying stream supports seeking.
/// </summary>
/// <remarks>
/// <para>
/// The underlying stream is read into memory on the first access to
/// <see cref="Read" />, <see cref="Seek" />, <see cref="Length" />, or <see cref="Position" />.
/// Subsequent operations work directly against the in-memory buffer.
/// </para>
/// <para>
/// Content is buffered from the source stream's current position when the buffer is first
/// initialized. The buffer position always starts at 0.
/// </para>
/// </remarks>
public sealed class MemoryReadStream(Stream source) : Stream
{
    private MemoryStream? _buffer;

    private MemoryStream EnsureBuffer()
    {
        if (_buffer is not null)
            return _buffer;

        var capacity = source.CanSeek ? (int)(source.Length - source.Position) : 0;
        _buffer = new MemoryStream(capacity);
        source.CopyTo(_buffer);
        _buffer.Position = 0;

        return _buffer;
    }

    /// <inheritdoc />
    public override bool CanRead => true;

    /// <inheritdoc />
    public override bool CanSeek => true;

    /// <inheritdoc />
    public override bool CanWrite => false;

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
    public override int Read(byte[] buffer, int offset, int count) =>
        EnsureBuffer().Read(buffer, offset, count);

    /// <inheritdoc />
    public override long Seek(long offset, SeekOrigin origin) =>
        EnsureBuffer().Seek(offset, origin);

    /// <inheritdoc />
    public override void SetLength(long value) =>
        throw new System.NotSupportedException("Stream does not support writing.");

    /// <inheritdoc />
    public override void Write(byte[] buffer, int offset, int count) =>
        throw new System.NotSupportedException("Stream does not support writing.");
}
