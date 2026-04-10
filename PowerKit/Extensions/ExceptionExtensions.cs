using System;
using System.Collections.Generic;

namespace PowerKit.Extensions;

internal static class ExceptionExtensions
{
    extension(Exception exception)
    {
        public IReadOnlyList<Exception> GetSelfAndDescendants()
        {
            static void PopulateDescendants(Exception ex, ICollection<Exception> result)
            {
                if (ex is AggregateException aggregateException)
                {
                    foreach (var innerException in aggregateException.InnerExceptions)
                    {
                        result.Add(innerException);
                        PopulateDescendants(innerException, result);
                    }
                }
                else if (ex.InnerException is not null)
                {
                    result.Add(ex.InnerException);
                    PopulateDescendants(ex.InnerException, result);
                }
            }

            var result = new List<Exception> { exception };
            PopulateDescendants(exception, result);
            return result;
        }
    }
}
