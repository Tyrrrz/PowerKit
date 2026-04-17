#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class UriExtensions
{
    extension(Uri uri)
    {
        /// <summary>
        /// Gets the scheme and host components of the URI (e.g. "https://example.com").
        /// </summary>
        public string Domain => uri.Scheme + Uri.SchemeDelimiter + uri.Host;
    }
}
