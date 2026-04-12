using System;

namespace PowerKit.Extensions;

internal static class AggregateExceptionExtensions
{
    extension(AggregateException exception)
    {
        public Exception? TryGetSingle() =>
            exception.Flatten().InnerExceptions is [var single] ? single : null;
    }
}
