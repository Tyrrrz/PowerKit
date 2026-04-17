#nullable enable
using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace PowerKit.Extensions;

internal interface ISpanOwner<T> : IDisposable
{
    Span<T> Span { get; }
}

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
file sealed class ArrayPoolSpanOwner<T>(ArrayPool<T> pool, T[] buffer, int minimumLength)
    : ISpanOwner<T>
{
    private int _disposed;

    public Span<T> Span
    {
        get
        {
            ObjectDisposedException.ThrowIf(_disposed != 0, this);
            return new Span<T>(buffer, 0, minimumLength);
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

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class ArrayPoolExtensions
{
    extension<T>(ArrayPool<T> pool)
    {
        /// <summary>
        /// Rents a buffer of at least <paramref name="minimumLength" /> elements from the pool
        /// and wraps it in an <see cref="ISpanOwner{T}" /> that returns the buffer to the pool
        /// when disposed.
        /// </summary>
        public ISpanOwner<T> RentOwner(int minimumLength = 1) =>
            new ArrayPoolSpanOwner<T>(pool, pool.Rent(minimumLength), minimumLength);
    }
}
