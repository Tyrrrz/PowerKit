#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace PowerKit.Extensions;

internal static class CollectionExtensions
{
    extension<T>(ICollection<T> source)
    {
        /// <summary>
        /// Removes all elements from the collection that match the specified predicate.
        /// </summary>
        public int RemoveAll(Func<T, bool> predicate)
        {
            var removedCount = 0;

            foreach (var item in source.ToArray())
            {
                if (predicate(item) && source.Remove(item))
                {
                    removedCount++;
                }
            }

            return removedCount;
        }
    }
}
