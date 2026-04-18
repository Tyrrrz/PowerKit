#if NET40_OR_GREATER || NETSTANDARD || NET
#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class HttpContentExtensions
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
    }
}
#endif
