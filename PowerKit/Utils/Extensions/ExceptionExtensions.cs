using System;
using System.Collections.Generic;
using System.Linq;

namespace PowerKit.Utils.Extensions;

internal static class ExceptionExtensions
{
    extension(AggregateException exception)
    {
        public Exception? TryGetSingle()
        {
            var exceptions = exception.Flatten().InnerExceptions;
            return exceptions.Count == 1 ? exceptions.Single() : null;
        }
    }

    extension(Exception exception)
    {
        private void PopulateDescendants(ICollection<Exception> result)
        {
            if (exception is AggregateException aggregateException)
            {
                foreach (var innerException in aggregateException.InnerExceptions)
                {
                    result.Add(innerException);
                    innerException.PopulateDescendants(result);
                }
            }
            else if (exception.InnerException is not null)
            {
                result.Add(exception.InnerException);
                exception.InnerException.PopulateDescendants(result);
            }
        }

        public IReadOnlyList<Exception> GetSelfAndDescendants()
        {
            var result = new List<Exception> { exception };
            exception.PopulateDescendants(result);
            return result;
        }
    }
}
