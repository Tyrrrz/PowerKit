using System;
using System.Buffers;

namespace PowerKit.Extensions;

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

file sealed class ArrayPoolMemoryOwner<T>(ArrayPool<T> pool, T[] buffer, int minimumLength)
    : IMemoryOwner<T>
{
    public Memory<T> Memory { get; } = buffer.AsMemory(0, minimumLength);

    public void Dispose() => pool.Return(buffer);
}
