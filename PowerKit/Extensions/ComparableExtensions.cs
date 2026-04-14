#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class ComparableExtensions
{
    extension<T>(T value) where T : IComparable<T>
    {
        /// <summary>
        /// Clamps the value to the specified range.
        /// </summary>
        public T Clamp(T min, T max)
        {
            if (value.CompareTo(min) < 0)
                return min;

            if (value.CompareTo(max) > 0)
                return max;

            return value;
        }

        /// <summary>
        /// Returns the smaller of the current value and the specified value.
        /// </summary>
        public T Min(T other) => value.CompareTo(other) <= 0 ? value : other;

        /// <summary>
        /// Returns the larger of the current value and the specified value.
        /// </summary>
        public T Max(T other) => value.CompareTo(other) >= 0 ? value : other;
    }
}
