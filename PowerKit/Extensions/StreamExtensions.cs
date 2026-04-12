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
