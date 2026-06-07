#if !NETFRAMEWORK || NET45_OR_GREATER
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
    ) => await content.CopyToAsync(stream).ConfigureAwait(false);

    protected override bool TryComputeLength(out long length)
    {
        length = content.Headers.ContentLength ?? 0;
        return content.Headers.ContentLength.HasValue;
    }
}

/// <summary>
/// Extensions for <see cref="HttpRequestMessage" />.
/// </summary>
public static class HttpRequestMessageExtensions
{
    extension(HttpRequestMessage request)
    {
        /// <summary>
        /// Creates a clone of the HTTP request message.
        /// </summary>
        public HttpRequestMessage Clone()
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Version = request.Version,
#if NET5_0_OR_GREATER
                VersionPolicy = request.VersionPolicy,
#endif
                Content = request.Content?.Pipe(c => new NonDisposableHttpContent(c)),
            };

#if NET5_0_OR_GREATER
            foreach (var option in request.Options)
            {
                clone.Options.Set(new HttpRequestOptionsKey<object?>(option.Key), option.Value);
            }
#else
            foreach (var property in request.Properties)
            {
                clone.Properties[property.Key] = property.Value;
            }
#endif

            foreach (var (key, value) in request.Headers)
                clone.Headers.TryAddWithoutValidation(key, value);

            if (request.Content is not null && clone.Content is not null)
            {
                foreach (var (key, value) in request.Content.Headers)
                    clone.Content.Headers.TryAddWithoutValidation(key, value);
            }

            return clone;
        }
    }
}
#endif
