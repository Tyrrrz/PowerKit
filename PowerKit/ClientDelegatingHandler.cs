#if NET40_OR_GREATER || NETSTANDARD || NET
#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using PowerKit.Extensions;

namespace PowerKit;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
/// <summary>
/// Like <see cref="DelegatingHandler" />, but wraps an <see cref="HttpClient" /> instead of an
/// <see cref="HttpMessageHandler" />. Used to extend an externally provided <see cref="HttpClient" />
/// with additional behavior.
/// </summary>
internal class ClientDelegatingHandler(HttpClient http, bool disposeClient = false)
    : HttpMessageHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        // Clone the request to reset its completion status, which is required
        // in order to pass the request from one HttpClient to another.
        using var clonedRequest = request.Clone();

        return await http.SendAsync(
                clonedRequest,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken
            )
            .ConfigureAwait(false);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && disposeClient)
            http.Dispose();

        base.Dispose(disposing);
    }
}
#endif
