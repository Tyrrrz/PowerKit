#if !NETFRAMEWORK || NET45_OR_GREATER
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using PowerKit.Extensions;

namespace PowerKit;

/// <summary>
/// Like <see cref="DelegatingHandler" />, but wraps an <see cref="HttpClient" /> instead of an
/// <see cref="HttpMessageHandler" />. Used to extend an externally provided <see cref="HttpClient" />
/// with additional behavior.
/// </summary>
public class ClientDelegatingHandler(HttpClient http, bool disposeClient = false)
    : HttpMessageHandler
{
    /// <inheritdoc />
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

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing && disposeClient)
            http.Dispose();

        base.Dispose(disposing);
    }
}
#endif
