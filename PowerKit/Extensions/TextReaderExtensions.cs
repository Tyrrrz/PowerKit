#nullable enable
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class TextReaderExtensions
{
    extension(TextReader reader)
    {
#if NET40_OR_GREATER || NETSTANDARD || NET
        /// <summary>
        /// Reads all lines from the text reader as an async sequence.
        /// </summary>
        public async IAsyncEnumerable<string> ReadLinesAsync(
            [EnumeratorCancellation] CancellationToken cancellationToken = default
        )
        {
            while (await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false) is { } line)
            {
                yield return line;
            }
        }
#endif
    }
}
