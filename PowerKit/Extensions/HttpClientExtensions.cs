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
internal static class HttpClientExtensions
{
    extension(HttpClient http)
    {
        /// <summary>
        /// Downloads the content at the specified URI to a local file.
        /// </summary>
        public async Task DownloadAsync(
            Uri requestUri,
            string filePath,
            IProgress<double>? progress = null,
            CancellationToken cancellationToken = default
        )
        {
            using var response = await http.GetAsync(
                    requestUri,
                    HttpCompletionOption.ResponseHeadersRead,
                    cancellationToken
                )
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            using var destination = File.Create(filePath, 81920, FileOptions.Asynchronous);

            await response
                .Content.CopyToStreamAsync(destination, progress, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Downloads the content at the specified URI to a local file.
        /// </summary>
        public async Task DownloadAsync(
            string requestUri,
            string filePath,
            IProgress<double>? progress = null,
            CancellationToken cancellationToken = default
        ) =>
            await http.DownloadAsync(
                    new Uri(requestUri, UriKind.RelativeOrAbsolute),
                    filePath,
                    progress,
                    cancellationToken
                )
                .ConfigureAwait(false);

        /// <summary>
        /// Sends a HEAD request to the specified URI and returns the response.
        /// </summary>
        public async Task<HttpResponseMessage> HeadAsync(
            Uri requestUri,
            CancellationToken cancellationToken = default
        )
        {
            using var request = new HttpRequestMessage(HttpMethod.Head, requestUri);
            return await http.SendAsync(
                    request,
                    HttpCompletionOption.ResponseHeadersRead,
                    cancellationToken
                )
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a HEAD request to the specified URI and returns the response.
        /// </summary>
        public async Task<HttpResponseMessage> HeadAsync(
            string requestUri,
            CancellationToken cancellationToken = default
        ) =>
            await http.HeadAsync(new Uri(requestUri, UriKind.RelativeOrAbsolute), cancellationToken)
                .ConfigureAwait(false);
    }
}
#endif
