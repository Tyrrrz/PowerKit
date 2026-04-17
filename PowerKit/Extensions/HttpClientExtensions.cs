#if NET40_OR_GREATER || NETSTANDARD || NET
#nullable enable
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
        /// Downloads the content at the specified URL to a local file.
        /// </summary>
        public async Task DownloadAsync(
            string url,
            string filePath,
            CancellationToken cancellationToken = default
        )
        {
            using var response = await http.GetAsync(
                    url,
                    HttpCompletionOption.ResponseHeadersRead,
                    cancellationToken
                )
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            using var source = await response
                .Content.ReadAsStreamAsync(cancellationToken)
                .ConfigureAwait(false);

            using var destination = File.Create(filePath, 81920, FileOptions.Asynchronous);

            await source.CopyToAsync(destination, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a HEAD request to the specified URI and returns the response.
        /// </summary>
        public async ValueTask<HttpResponseMessage> HeadAsync(
            string requestUri,
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
    }
}
#endif
