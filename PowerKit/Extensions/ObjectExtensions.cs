using System;

namespace PowerKit.Extensions;

internal static class ObjectExtensions
{
    extension<T>(T value)
        where T : class
    {
        /// <summary>
        /// Returns <see langword="null" /> if the value matches the specified predicate; otherwise, returns the value.
        /// </summary>
        public T? NullIf(Func<T, bool> predicate) => !predicate(value) ? value : null;
    }
}
