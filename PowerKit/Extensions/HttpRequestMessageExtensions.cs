#if NET40_OR_GREATER || NETSTANDARD || NET
#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PowerKit.Extensions;

file class NonDisposableHttpContent(HttpContent content) : HttpContent
{
    protected override async Task SerializeToStreamAsync(
        Stream stream,
        TransportContext? context
    ) => await content.CopyToAsync(stream);

    protected override bool TryComputeLength(out long length)
    {
        length = 0;
        return false;
    }
}

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class HttpRequestMessageExtensions
{
    extension(HttpRequestMessage request)
    {
        /// <summary>
        /// Creates a clone of the HTTP request message.
        /// </summary>
        public HttpRequestMessage Clone()
        {
            var clonedRequest = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Version = request.Version,
#if NET5_0_OR_GREATER
                VersionPolicy = request.VersionPolicy,
#endif
                // Don't dispose the original request's content
                Content = request.Content is not null
                    ? new NonDisposableHttpContent(request.Content)
                    : null,
            };

#if NET5_0_OR_GREATER
            foreach (var option in request.Options)
                clonedRequest.Options.Set(new HttpRequestOptionsKey<object?>(option.Key), option.Value);
#else
            foreach (var property in request.Properties)
                clonedRequest.Properties[property.Key] = property.Value;
#endif
            foreach (var (key, value) in request.Headers)
                clonedRequest.Headers.TryAddWithoutValidation(key, value);

            if (request.Content is not null && clonedRequest.Content is not null)
            {
                foreach (var (key, value) in request.Content.Headers)
                    clonedRequest.Content.Headers.TryAddWithoutValidation(key, value);
            }

            return clonedRequest;
        }
    }
}
#endif
