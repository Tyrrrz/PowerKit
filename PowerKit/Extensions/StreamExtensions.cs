using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PowerKit.Extensions;

/// <summary>
/// Extensions for <see cref="Stream" />.
/// </summary>
public static class StreamExtensions
{
    extension(Stream source)
    {
        /// <summary>
        /// Copies the contents of the stream into a new <see cref="MemoryStream" />.
        /// If the stream is already a <see cref="MemoryStream" />, it is returned as-is.
        /// </summary>
        public MemoryStream ToMemoryStream()
        {
            if (source is MemoryStream asMemoryStream)
                return asMemoryStream;

            var memoryStream = new MemoryStream();
            source.CopyTo(memoryStream);
            memoryStream.Position = 0;

            return memoryStream;
        }

#if NET40_OR_GREATER || NETSTANDARD || NET
        /// <summary>
        /// Copies the contents of the stream into a new <see cref="MemoryStream" /> asynchronously.
        /// If the stream is already a <see cref="MemoryStream" />, it is returned as-is.
        /// </summary>
        public async Task<MemoryStream> ToMemoryStreamAsync(
            CancellationToken cancellationToken = default
        )
        {
            if (source is MemoryStream asMemoryStream)
                return asMemoryStream;

            var memoryStream = new MemoryStream();
            await source.CopyToAsync(memoryStream, cancellationToken).ConfigureAwait(false);
            memoryStream.Position = 0;

            return memoryStream;
        }

#endif
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
