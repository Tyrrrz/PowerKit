using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace PowerKit.Extensions;

internal static class AsyncEnumerableExtensions
{
    extension<T>(IAsyncEnumerable<T> source)
    {
        public async IAsyncEnumerable<T> TakeAsync(
            int count,
            [EnumeratorCancellation] CancellationToken cancellationToken = default
        )
        {
            if (count <= 0)
                yield break;

            var currentCount = 0;

            await foreach (
                var item in source
                    .WithCancellation(cancellationToken)
                    .ConfigureAwait(false)
            )
            {
                if (currentCount >= count)
                    yield break;

                yield return item;
                currentCount++;
            }
        }

        public async IAsyncEnumerable<TResult> SelectManyAsync<TResult>(
            System.Func<T, IEnumerable<TResult>> transform,
            [EnumeratorCancellation] CancellationToken cancellationToken = default
        )
        {
            await foreach (
                var item in source
                    .WithCancellation(cancellationToken)
                    .ConfigureAwait(false)
            )
                foreach (var result in transform(item))
                    yield return result;
        }

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
                list.Add(item);

            return list;
        }

        public ValueTaskAwaiter<List<T>> GetAwaiter() => source.ToListAsync().GetAwaiter();
    }
}
