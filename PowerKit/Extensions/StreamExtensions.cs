using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PowerKit.Extensions;

internal static class StreamExtensions
{
    extension(Stream source)
    {
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

            while (true)
            {
                var bytesRead = await source
                    .ReadAsync(buffer.Memory, cancellationToken)
                    .ConfigureAwait(false);

                if (bytesRead <= 0)
                {
                    break;
                }

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
        /// as a ratio of bytes read to <paramref name="contentLength"/>.
        /// </summary>
        public async ValueTask CopyToAsync(
            Stream destination,
            long contentLength,
            IProgress<double>? progress,
            CancellationToken cancellationToken = default
        )
        {
            using var buffer = MemoryPool<byte>.Shared.Rent(81920);

            var totalBytesRead = 0L;

            while (true)
            {
                var bytesRead = await source
                    .ReadAsync(buffer.Memory, cancellationToken)
                    .ConfigureAwait(false);

                if (bytesRead <= 0)
                {
                    break;
                }

                await destination
                    .WriteAsync(buffer.Memory[..bytesRead], cancellationToken)
                    .ConfigureAwait(false);

                totalBytesRead += bytesRead;

                if (progress is not null && contentLength > 0)
                {
                    progress.Report(1.0 * totalBytesRead / contentLength);
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
        )
        {
            var contentLength = source.CanSeek ? source.Length : -1;
            await source
                .CopyToAsync(destination, contentLength, progress, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
