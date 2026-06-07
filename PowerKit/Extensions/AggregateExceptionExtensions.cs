#if !NETFRAMEWORK || NET45_OR_GREATER
using System;

namespace PowerKit.Extensions;

/// <summary>
/// Extensions for <see cref="AggregateException" />.
/// </summary>
public static class AggregateExceptionExtensions
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
#endif
