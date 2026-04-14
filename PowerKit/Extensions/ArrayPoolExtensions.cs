using System;
using System.Buffers;
using System.Threading;
using System.Diagnostics.CodeAnalysis;

namespace PowerKit.Extensions;

#if POWERKIT_EXCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class ArrayPoolExtensions
{
    extension<T>(ArrayPool<T> pool)
    {
        /// <summary>
        /// Rents a buffer of at least <paramref name="minimumLength" /> elements from the pool
        /// and wraps it in an <see cref="IMemoryOwner{T}" /> that returns the buffer to the pool
        /// when disposed.
        /// </summary>
        public IMemoryOwner<T> RentOwner(int minimumLength = 1) =>
            new ArrayPoolMemoryOwner<T>(pool, pool.Rent(minimumLength), minimumLength);
    }
}

#if POWERKIT_EXCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
file sealed class ArrayPoolMemoryOwner<T>(ArrayPool<T> pool, T[] buffer, int minimumLength)
    : IMemoryOwner<T>
{
    private int _disposed;

    public Memory<T> Memory
    {
        get
        {
            ObjectDisposedException.ThrowIf(_disposed != 0, this);
            return buffer.AsMemory(0, minimumLength);
        }
    }

    public void Dispose()
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0)
        {
            return;
        }

        pool.Return(buffer);
    }
}
