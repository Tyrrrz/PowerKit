#if NET40_OR_GREATER || NETSTANDARD || NET
#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class HttpHeadersExtensions
{
    extension(HttpHeaders headers)
    {
        /// <summary>
        /// Attempts to get the value of the header with the specified name.
        /// Returns <see langword="null" /> if the header is not present.
        /// </summary>
        public string? TryGetValue(string name) =>
            headers.TryGetValues(name, out var values) ? string.Concat(values) : null;
    }
}
#endif
