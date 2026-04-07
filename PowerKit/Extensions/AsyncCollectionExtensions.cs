using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace PowerKit.Extensions;

internal static class AsyncCollectionExtensions
{
    extension<T>(IAsyncEnumerable<T> source)
    {
        public async IAsyncEnumerable<T> TakeAsync(
            int count,
            [EnumeratorCancellation] CancellationToken cancellationToken = default
        )
        {
            var currentCount = 0;

            await foreach (var item in source.WithCancellation(cancellationToken))
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
            await foreach (var item in source.WithCancellation(cancellationToken))
                foreach (var result in transform(item))
                    yield return result;
        }

        public async ValueTask<List<T>> ToListAsync(
            CancellationToken cancellationToken = default
        )
        {
            var list = new List<T>();

            await foreach (var item in source.WithCancellation(cancellationToken))
                list.Add(item);

            return list;
        }

        public ValueTaskAwaiter<List<T>> GetAwaiter() => source.ToListAsync().GetAwaiter();
    }
}
