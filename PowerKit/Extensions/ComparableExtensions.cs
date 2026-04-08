using System;

namespace PowerKit.Extensions;

internal static class ComparableExtensions
{
    extension<T>(T value) where T : IComparable<T>
    {
        public T Clamp(T min, T max)
        {
            if (value.CompareTo(min) < 0)
                return min;

            if (value.CompareTo(max) > 0)
                return max;

            return value;
        }
    }
}
