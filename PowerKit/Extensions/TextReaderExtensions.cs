using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace PowerKit.Extensions;

internal static class TextReaderExtensions
{
    extension(TextReader reader)
    {
        public async IAsyncEnumerable<string> ReadLinesAsync(
            [EnumeratorCancellation] CancellationToken cancellationToken = default
        )
        {
            while (
                await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false) is { } line
            )
            {
                yield return line;
            }
        }
    }
}
