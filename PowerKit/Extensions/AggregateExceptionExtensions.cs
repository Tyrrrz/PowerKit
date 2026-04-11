using System;
using System.Linq;

namespace PowerKit.Extensions;

internal static class AggregateExceptionExtensions
{
    extension(AggregateException exception)
    {
        public Exception? TryGetSingle()
        {
            var exceptions = exception.Flatten().InnerExceptions;
            return exceptions.Count == 1 ? exceptions[0] : null;
        }
    }
}
