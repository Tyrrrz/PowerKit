#if NET40_OR_GREATER || NETSTANDARD || NET
#nullable enable
using System.Collections.Generic;
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
        /// Gets the values of the header with the specified name.
        /// Returns an empty sequence if the header is not present.
        /// </summary>
        public IEnumerable<string> GetValuesOrEmpty(string name) =>
            headers.TryGetValues(name, out var values) ? values : [];

        /// <summary>
        /// Attempts to get the value of the header with the specified name.
        /// Returns an empty string if the header is not present.
        /// </summary>
        public string TryGetValue(string name) => string.Join(", ", headers.GetValuesOrEmpty(name));
    }
}
#endif
