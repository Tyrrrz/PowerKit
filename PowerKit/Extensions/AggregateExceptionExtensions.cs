#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class AggregateExceptionExtensions
{
    extension(AggregateException exception)
    {
        /// <summary>
        /// Returns the single inner exception if the aggregate contains exactly one after flattening;
        /// otherwise, returns <see langword="null" />.
        /// </summary>
        public Exception? TryGetSingle() =>
            exception.Flatten().InnerExceptions is [var single] ? single : null;
    }
}
