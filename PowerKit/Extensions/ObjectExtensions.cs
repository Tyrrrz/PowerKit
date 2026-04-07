using System.Collections.Generic;

namespace PowerKit.Extensions;

internal static class ObjectExtensions
{
    extension<T>(T obj)
    {
        public IEnumerable<T> ToSingletonEnumerable()
        {
            yield return obj;
        }
    }
}
