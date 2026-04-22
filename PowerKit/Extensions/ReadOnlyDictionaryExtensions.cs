#nullable enable
#if NET40_OR_GREATER || NETSTANDARD || NET
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class ReadOnlyDictionaryExtensions
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
