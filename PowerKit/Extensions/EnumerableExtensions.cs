using System.Collections.Generic;
using System.Linq;

namespace PowerKit.Extensions;

internal static class EnumerableExtensions
{
    extension<T>(T obj)
    {
        /// <summary>
        /// Wraps the object in an enumerable containing a single element.
        /// </summary>
        public IEnumerable<T> ToSingletonEnumerable()
        {
            yield return obj;
        }
    }

    extension<T>(IEnumerable<T?> source)
        where T : class
    {
        /// <summary>
        /// Filters out <see langword="null" /> elements from the sequence.
        /// </summary>
        public IEnumerable<T> WhereNotNull()
        {
            foreach (var item in source)
            {
                if (item is not null)
                {
                    yield return item;
                }
            }
        }
    }

    extension<T>(IEnumerable<T?> source)
        where T : struct
    {
        /// <summary>
        /// Filters out <see langword="null" /> elements from the sequence of nullable value types.
        /// </summary>
        public IEnumerable<T> WhereNotNull()
        {
            foreach (var item in source)
            {
                if (item is not null)
                {
                    yield return item.Value;
                }
            }
        }
    }

    extension(IEnumerable<string?> source)
    {
        /// <summary>
        /// Filters out <see langword="null" /> and empty strings from the sequence.
        /// </summary>
        public IEnumerable<string> WhereNotNullOrEmpty()
        {
            foreach (var item in source)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    yield return item!;
                }
            }
        }

        /// <summary>
        /// Filters out <see langword="null" />, empty, and whitespace-only strings from the sequence.
        /// </summary>
        public IEnumerable<string> WhereNotNullOrWhiteSpace()
        {
            foreach (var item in source)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    yield return item!;
                }
            }
        }
    }

    extension<T>(IEnumerable<T> source)
        where T : struct
    {
        /// <summary>
        /// Returns the first element of the sequence, or <see langword="null" /> if the sequence is empty.
        /// </summary>
        public T? FirstOrNull()
        {
            foreach (var item in source)
            {
                return item;
            }

            return null;
        }

        /// <summary>
        /// Returns the last element of the sequence, or <see langword="null" /> if the sequence is empty.
        /// </summary>
        public T? LastOrNull()
        {
#if NET40_OR_GREATER || NETSTANDARD || NET
            if (source is IReadOnlyList<T> list)
            {
                return list.Count > 0 ? list[list.Count - 1] : null;
            }
#endif

            var last = default(T?);

            foreach (var item in source)
            {
                last = item;
            }

            return last;
        }

        /// <summary>
        /// Returns the element at the specified index, or <see langword="null" /> if the index is out of range.
        /// </summary>
        public T? ElementAtOrNull(int index)
        {
#if NET40_OR_GREATER || NETSTANDARD || NET
            var list = source as IReadOnlyList<T> ?? source.ToArray();
#else
            var list = source as IList<T> ?? source.ToArray();
#endif
            return index >= 0 && index < list.Count ? list[index] : null;
        }
    }
}
