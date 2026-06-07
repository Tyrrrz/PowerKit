using System;
using System.Collections.Generic;
using System.Linq;

namespace PowerKit.Extensions;

/// <summary>
/// Extensions for <see cref="ICollection{T}" />.
/// </summary>
public static class CollectionExtensions
{
    extension<T>(ICollection<T> source)
    {
        /// <summary>
        /// Adds all elements from the specified sequence to the collection.
        /// </summary>
        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items.ToArray())
                source.Add(item);
        }

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

        /// <summary>
        /// Removes all elements in the specified sequence from the collection.
        /// </summary>
        public void RemoveRange(IEnumerable<T> items)
        {
            foreach (var item in items.ToArray())
                source.Remove(item);
        }
    }
}
