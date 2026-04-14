#nullable enable
using System;
using System.Collections.Generic;

namespace PowerKit.Extensions;

internal static class FunctionalExtensions
{
    extension<TIn>(TIn input)
    {
        /// <summary>
        /// Passes the value through the specified transform function and returns the result.
        /// </summary>
        public TOut Pipe<TOut>(Func<TIn, TOut> transform) => transform(input);
    }

    extension<T>(T value)
        where T : struct
    {
        /// <summary>
        /// Returns <see langword="null" /> if the value matches the specified predicate; otherwise, returns the value.
        /// </summary>
        public T? NullIf(Func<T, bool> predicate) => !predicate(value) ? value : null;

        /// <summary>
        /// Returns <see langword="null" /> if the value equals the default value for its type; otherwise, returns the value.
        /// </summary>
        public T? NullIfDefault() =>
            value.NullIf(v => EqualityComparer<T>.Default.Equals(v, default));
    }

    extension(string value)
    {
        /// <summary>
        /// Returns <see langword="null" /> if the string is <see langword="null" /> or empty; otherwise, returns the string.
        /// </summary>
        public string? NullIfEmpty() => !string.IsNullOrEmpty(value) ? value : null;

        /// <summary>
        /// Returns <see langword="null" /> if the string is <see langword="null" />, empty, or consists only of whitespace;
        /// otherwise, returns the string.
        /// </summary>
        public string? NullIfWhiteSpace() => !string.IsNullOrWhiteSpace(value) ? value : null;
    }
}

// Separate class because C# (CS0111) does not allow two generic methods with identical
// parameter types that differ only by constraint (class vs struct) in the same class.
internal static class ReferenceTypeFunctionalExtensions
{
    extension<T>(T value)
        where T : class
    {
        /// <summary>
        /// Returns <see langword="null" /> if the value is <see langword="null" /> or matches the specified predicate;
        /// otherwise, returns the value.
        /// </summary>
        public T? NullIf(Func<T, bool> predicate) => value?.Pipe(v => !predicate(v) ? v : null);
    }
}
