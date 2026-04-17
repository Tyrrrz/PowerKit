#nullable enable
using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace PowerKit.Extensions;

internal interface IArrayOwner<T> : IDisposable
{
    T[] Array { get; }
    int Length { get; }
}

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
file sealed class ArrayPoolArrayOwner<T>(ArrayPool<T> pool, T[] buffer, int minimumLength)
    : IArrayOwner<T>
{
    private int _disposed;

    public T[] Array
    {
        get
        {
            ObjectDisposedException.ThrowIf(_disposed != 0, this);
            return buffer;
        }
    }

    public int Length => minimumLength;

    public void Dispose()
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0)
        {
            return;
        }

        pool.Return(buffer);
    }
}

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class ArrayPoolExtensions
{
    extension<T>(ArrayPool<T> pool)
    {
        /// <summary>
        /// Rents a buffer of at least <paramref name="minimumLength" /> elements from the pool
        /// and wraps it in an <see cref="IArrayOwner{T}" /> that returns the buffer to the pool
        /// when disposed.
        /// </summary>
        public IArrayOwner<T> RentOwner(int minimumLength = 1) =>
            new ArrayPoolArrayOwner<T>(pool, pool.Rent(minimumLength), minimumLength);
    }
}
