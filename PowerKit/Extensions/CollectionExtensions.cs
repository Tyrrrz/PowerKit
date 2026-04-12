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
        public void RemoveAll(Func<T, bool> predicate)
        {
            foreach (var item in source.ToArray())
            {
                if (predicate(item))
                {
                    source.Remove(item);
                }
            }
        }
    }
}
