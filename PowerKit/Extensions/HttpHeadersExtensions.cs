#if !NETFRAMEWORK || NET45_OR_GREATER
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace PowerKit.Extensions;

/// <summary>
/// Extensions for <see cref="HttpHeaders" />.
/// </summary>
public static class HttpHeadersExtensions
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
        /// Returns null if the header is not present.
        /// </summary>
        public string? TryGetValue(string name) =>
            headers.TryGetValues(name, out var values) ? string.Join(", ", values) : null;
    }
}
#endif
