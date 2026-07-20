using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PowerKit.Extensions;

file sealed class MemoryBackedStream(Stream source) : Stream
{
    private MemoryStream? _buffer;

    private MemoryStream EnsureBuffer()
    {
        if (_buffer is not null)
            return _buffer;

        _buffer = new MemoryStream();

        if (source.CanRead)
        {
            if (source.CanSeek)
            {
                // Load the entire stream into memory and restore the original position.
                var savedPosition = source.Position;
                source.Seek(0, SeekOrigin.Begin);
                source.CopyTo(_buffer);
                _buffer.Position = Math.Min(savedPosition, _buffer.Length);
            }
            else
            {
                // Non-seekable: load from the current position and start reading at 0.
                source.CopyTo(_buffer);
                _buffer.Position = 0;
            }
        }

        return _buffer;
    }

    public override bool CanRead => source.CanRead;
    public override bool CanSeek => true;
    public override bool CanWrite => source.CanWrite;

    public override long Length => EnsureBuffer().Length;

    public override long Position
    {
        get => EnsureBuffer().Position;
        set => EnsureBuffer().Position = value;
    }

    // Flush is a no-op because the buffer is an in-memory MemoryStream, which
    // never needs flushing. The write-back to the underlying stream happens on Dispose.
    public override void Flush() { }

    public override int Read(byte[] buffer, int offset, int count)
    {
        if (!source.CanRead)
            throw new NotSupportedException("Stream does not support reading.");

        return EnsureBuffer().Read(buffer, offset, count);
    }

    public override long Seek(long offset, SeekOrigin origin) =>
        EnsureBuffer().Seek(offset, origin);

    public override void SetLength(long value) => EnsureBuffer().SetLength(value);

    public override void Write(byte[] buffer, int offset, int count)
    {
        if (!source.CanWrite)
            throw new NotSupportedException("Stream does not support writing.");

        EnsureBuffer().Write(buffer, offset, count);
    }

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

/// <summary>
/// Extensions for <see cref="Stream" />.
/// </summary>
public static class StreamExtensions
{
    extension(Stream source)
    {
        /// <summary>
        /// Returns a <see cref="Stream" /> backed by a <see cref="MemoryStream" />.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The first read or write lazily loads the underlying stream into memory.
        /// Subsequent reads and writes operate directly against the in-memory buffer,
        /// making the returned stream always seekable.
        /// </para>
        /// <para>
        /// On readable and seekable streams, the entire content is loaded from the
        /// beginning and the original position is restored before the operation continues.
        /// On non-seekable readable streams, content is loaded from the current position.
        /// </para>
        /// <para>
        /// Writes go to the in-memory buffer. When the wrapper is disposed, the buffer
        /// is written back to the underlying stream. On readable and seekable streams
        /// the underlying stream is seeked to the beginning before the write-back.
        /// </para>
        /// <para>
        /// If the stream is already a <see cref="MemoryStream" />, it is returned as-is.
        /// </para>
        /// </remarks>
        public Stream ToMemoryStream()
        {
            if (source is MemoryStream)
                return source;

            return new MemoryBackedStream(source);
        }

#if NET40_OR_GREATER || NETSTANDARD || NET
        /// <summary>
        /// Copies the contents of the stream to the destination stream, optionally flushing after each write.
        /// </summary>
        public async Task CopyToAsync(
            Stream destination,
            bool autoFlush,
            CancellationToken cancellationToken = default
        )
        {
            using var buffer = MemoryPool<byte>.Shared.Rent(81920);

            while (
                await source.ReadAsync(buffer.Memory, cancellationToken).ConfigureAwait(false)
                    is > 0
                        and var bytesRead
            )
            {
                await destination
                    .WriteAsync(buffer.Memory[..bytesRead], cancellationToken)
                    .ConfigureAwait(false);

                if (autoFlush)
                {
                    await destination.FlushAsync(cancellationToken).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Copies the contents of the stream to the destination stream, reporting progress
        /// as a ratio of bytes read to <paramref name="sourceLength" />.
        /// </summary>
        public async ValueTask CopyToAsync(
            Stream destination,
            long sourceLength,
            IProgress<double>? progress,
            CancellationToken cancellationToken = default
        )
        {
            using var buffer = MemoryPool<byte>.Shared.Rent(81920);

            var totalBytesRead = 0L;

            while (
                await source.ReadAsync(buffer.Memory, cancellationToken).ConfigureAwait(false)
                    is > 0
                        and var bytesRead
            )
            {
                await destination
                    .WriteAsync(buffer.Memory[..bytesRead], cancellationToken)
                    .ConfigureAwait(false);

                totalBytesRead += bytesRead;

                if (progress is not null && sourceLength > 0)
                {
                    progress.Report(1.0 * totalBytesRead / sourceLength);
                }
            }
        }

        /// <summary>
        /// Copies the contents of the stream to the destination stream, reporting progress
        /// based on the source stream's length when available.
        /// </summary>
        public async ValueTask CopyToAsync(
            Stream destination,
            IProgress<double>? progress = null,
            CancellationToken cancellationToken = default
        ) =>
            await source
                .CopyToAsync(
                    destination,
                    source.CanSeek ? source.Length : -1,
                    progress,
                    cancellationToken
                )
                .ConfigureAwait(false);
#endif
    }
}
