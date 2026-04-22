#nullable enable
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class DictionaryExtensions
{
    extension<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
        where TKey : notnull
        where TValue : struct
    {
        /// <summary>
        /// Returns the value associated with the specified key, or <see langword="null" /> if the key is not found.
        /// </summary>
        public TValue? GetValueOrNull(TKey key) =>
            dictionary.TryGetValue(key, out var value) ? value : null;
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

        /// <summary>
        /// Converts a non-generic dictionary to a typed <see cref="Dictionary{TKey, TValue}"/> using the default comparer.
        /// </summary>
        public Dictionary<TKey, TValue> ToDictionary<TKey, TValue>()
            where TKey : notnull =>
            dictionary.ToDictionary<TKey, TValue>(EqualityComparer<TKey>.Default);
    }
}
