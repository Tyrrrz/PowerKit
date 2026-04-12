using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace PowerKit.Extensions;

internal static class AsyncEnumerableExtensions
{
    extension<T>(IAsyncEnumerable<T> source)
    {
        /// <summary>
        /// Returns a specified number of elements from the start of the async sequence.
        /// </summary>
        public async IAsyncEnumerable<T> TakeAsync(
            int count,
            [EnumeratorCancellation] CancellationToken cancellationToken = default
        )
        {
            if (count <= 0)
                yield break;

            var currentCount = 0;

            await using var enumerator = source.GetAsyncEnumerator(cancellationToken);

            while (
                currentCount < count
                && await enumerator.MoveNextAsync().ConfigureAwait(false)
            )
            {
                yield return enumerator.Current;
                currentCount++;
            }
        }

        /// <summary>
        /// Projects each element of the async sequence to an <see cref="IEnumerable{T}"/>
        /// and flattens the resulting sequences into one async sequence.
        /// </summary>
        public async IAsyncEnumerable<TResult> SelectManyAsync<TResult>(
            Func<T, IEnumerable<TResult>> transform,
            [EnumeratorCancellation] CancellationToken cancellationToken = default
        )
        {
            await foreach (
                var item in source
                    .WithCancellation(cancellationToken)
                    .ConfigureAwait(false)
            )
            {
                foreach (var result in transform(item))
                {
                    yield return result;
                }
            }
        }

        /// <summary>
        /// Materializes the async sequence into a <see cref="List{T}"/>.
        /// </summary>
        public async ValueTask<List<T>> ToListAsync(
            CancellationToken cancellationToken = default
        )
        {
            var list = new List<T>();

            await foreach (
                var item in source
                    .WithCancellation(cancellationToken)
                    .ConfigureAwait(false)
            )
            {
                list.Add(item);
            }

            return list;
        }

        /// <summary>
        /// Enables directly awaiting the async sequence, materializing it into a <see cref="List{T}"/>.
        /// </summary>
        public ValueTaskAwaiter<List<T>> GetAwaiter() => source.ToListAsync().GetAwaiter();
    }
}
