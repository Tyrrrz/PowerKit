#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
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

    extension(IDictionary dictionary)
    {
        /// <summary>
        /// Converts a non-generic dictionary to a typed <see cref="Dictionary{TKey, TValue}"/> using the specified comparer.
        /// </summary>
        public Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(IEqualityComparer<TKey> comparer)
            where TKey : notnull =>
            dictionary
                .Cast<DictionaryEntry>()
                .ToDictionary(entry => (TKey)entry.Key, entry => (TValue)entry.Value!, comparer);
    }
}
