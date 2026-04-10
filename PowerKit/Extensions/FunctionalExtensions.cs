using System;
using System.Collections.Generic;

namespace PowerKit.Extensions;

internal static class FunctionalExtensions
{
    extension<TIn>(TIn input)
    {
        public TOut Pipe<TOut>(Func<TIn, TOut> transform) => transform(input);
    }

    extension<T>(T value)
        where T : struct
    {
        public T? NullIf(Func<T, bool> predicate) => !predicate(value) ? value : null;

        public T? NullIfDefault() =>
            value.NullIf(v => EqualityComparer<T>.Default.Equals(v, default));
    }

    extension(string value)
    {
        public string? NullIfEmpty() => !string.IsNullOrEmpty(value) ? value : null;

        public string? NullIfWhiteSpace() => !string.IsNullOrWhiteSpace(value) ? value : null;
    }
}
