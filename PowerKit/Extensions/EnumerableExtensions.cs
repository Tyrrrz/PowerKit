using System.Collections.Generic;
using System.Linq;

namespace PowerKit.Extensions;

internal static class EnumerableExtensions
{
    extension<T>(IEnumerable<T?> source)
        where T : class
    {
        public IEnumerable<T> WhereNotNull()
        {
            foreach (var item in source)
            {
                if (item is not null)
                    yield return item;
            }
        }
    }

    extension<T>(IEnumerable<T?> source)
        where T : struct
    {
        public IEnumerable<T> WhereNotNull()
        {
            foreach (var item in source)
            {
                if (item is not null)
                    yield return item.Value;
            }
        }
    }

    extension(IEnumerable<string?> source)
    {
        public IEnumerable<string> WhereNotNullOrWhiteSpace()
        {
            foreach (var item in source)
            {
                if (!string.IsNullOrWhiteSpace(item))
                    yield return item!;
            }
        }
    }

    extension<T>(IEnumerable<T> source)
        where T : struct
    {
        public T? FirstOrNull()
        {
            foreach (var item in source)
                return item;

            return null;
        }

        public T? ElementAtOrNull(int index)
        {
            var list = source as IReadOnlyList<T> ?? source.ToArray();
            return index >= 0 && index < list.Count ? list[index] : null;
        }
    }
}
