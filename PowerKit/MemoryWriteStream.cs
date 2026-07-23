using System.IO;

namespace PowerKit;

/// <summary>
/// A <see cref="Stream" /> wrapper that buffers all writes in memory and flushes them to the
/// underlying stream on <see cref="Dispose" />.
/// </summary>
/// <remarks>
/// <para>
/// Writes go to an in-memory buffer and do not touch the underlying stream until the wrapper is
/// disposed. This makes the wrapper always seekable and allows writes to be reordered freely
/// before the final flush.
/// </para>
/// <para>
/// On disposal, the entire in-memory buffer is written to the underlying stream starting at its
/// current position.
/// </para>
/// </remarks>
public sealed class MemoryWriteStream(Stream source) : Stream
{
    private readonly MemoryStream _buffer = new();

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

    /// <inheritdoc />
    public override void Flush() => _buffer.Flush();

    /// <inheritdoc />
    public override int Read(byte[] buffer, int offset, int count) =>
        throw new System.NotSupportedException("Stream does not support reading.");

    /// <inheritdoc />
    public override long Seek(long offset, SeekOrigin origin) => _buffer.Seek(offset, origin);

    /// <inheritdoc />
    public override void SetLength(long value) => _buffer.SetLength(value);

    /// <inheritdoc />
    public override void Write(byte[] buffer, int offset, int count) =>
        _buffer.Write(buffer, offset, count);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _buffer.Position = 0;
            _buffer.CopyTo(source);
            source.Flush();
        }

        base.Dispose(disposing);
    }
}
