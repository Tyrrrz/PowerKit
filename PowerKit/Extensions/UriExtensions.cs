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

        /// <summary>
        /// Tries to get the file name from the last segment of the URI's path.
        /// Returns <see langword="null" /> if the URI has no file name segment.
        /// </summary>
        public string? TryGetFileName()
        {
            var segments = uri.Segments;
            var lastSegment = segments[segments.Length - 1];
            if (lastSegment == "/")
                return null;

            return Uri.UnescapeDataString(lastSegment);
        }
    }
}
