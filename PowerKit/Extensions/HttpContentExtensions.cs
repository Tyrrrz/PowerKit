#if !NETFRAMEWORK || NET45_OR_GREATER
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PowerKit.Extensions;

/// <summary>
/// Extensions for <see cref="HttpContent" />.
/// </summary>
public static class HttpContentExtensions
{
    extension(HttpContent content)
    {
        /// <summary>
        /// Copies the HTTP content to the specified stream, reporting progress
        /// as a ratio of bytes written to the content length when available.
        /// </summary>
        public async Task CopyToStreamAsync(
            Stream destination,
            IProgress<double>? progress = null,
            CancellationToken cancellationToken = default
        )
        {
            using var source = await content
                .ReadAsStreamAsync(cancellationToken)
                .ConfigureAwait(false);

            var contentLength = content.Headers.ContentLength ?? -1;
            await source
                .CopyToAsync(destination, contentLength, progress, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Reads the HTTP content body and deserializes it as a <see cref="JsonElement"/>.
        /// </summary>
        public async Task<JsonElement> ReadAsJsonAsync(
            CancellationToken cancellationToken = default
        )
        {
            using var stream = await content
                .ReadAsStreamAsync(cancellationToken)
                .ConfigureAwait(false);

            using var document = await JsonDocument
                .ParseAsync(stream, default, cancellationToken)
                .ConfigureAwait(false);

            return document.RootElement.Clone();
        }
    }
}
#endif
