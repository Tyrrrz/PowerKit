#if NET40_OR_GREATER || NETSTANDARD || NET
#nullable enable
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
        /// Attempts to get the values of the header with the specified name.
        /// Returns an empty array if the header is not present.
        /// </summary>
        public IReadOnlyList<string> TryGetValues(string name) =>
            headers.TryGetValues(name, out var values) ? values.ToArray() : [];

        /// <summary>
        /// Attempts to get the value of the header with the specified name.
        /// Returns an empty string if the header is not present.
        /// </summary>
        public string? TryGetValue(string name) => string.Join(", ", headers.TryGetValues(name));
    }
}
#endif
