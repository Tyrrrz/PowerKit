using System;
using System.Collections.Generic;

namespace PowerKit.Extensions;

internal static class ExceptionExtensions
{
    extension(Exception exception)
    {
        /// <summary>
        /// Returns a flat list containing the exception itself and all of its
        /// nested inner exceptions, recursively unwrapping <see cref="AggregateException" /> instances.
        /// </summary>
        public List<Exception> GetSelfAndDescendants()
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

