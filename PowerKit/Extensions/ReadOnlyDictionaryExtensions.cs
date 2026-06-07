#if !NETFRAMEWORK || NET45_OR_GREATER
using System.Collections.Generic;

namespace PowerKit.Extensions;

/// <summary>
/// Extensions for <see cref="IReadOnlyDictionary{TKey, TValue}" />.
/// </summary>
public static class ReadOnlyDictionaryExtensions
{
    extension<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> dictionary)
        where TKey : notnull
        where TValue : struct
    {
        /// <summary>
        /// Returns the value associated with the specified key, or <see langword="null" /> if the key is not found.
        /// </summary>
        public TValue? GetValueOrNull(TKey key) =>
            dictionary.TryGetValue(key, out var value) ? value : null;
    }
}
#endif
