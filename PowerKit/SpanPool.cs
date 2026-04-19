#nullable enable
using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace PowerKit;

/// <summary>
/// Represents a rented buffer that is exposed as a <see cref="Span{T}" />.
/// </summary>
internal interface ISpanOwner<T> : IDisposable
{
    /// <summary>
    /// Gets the rented buffer as a span.
    /// </summary>
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

/// <summary>
/// Provides a pool of buffers that are exposed as <see cref="Span{T}" /> instances.
/// </summary>
#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class SpanPool<T>(ArrayPool<T> pool)
{
    /// <summary>
    /// Gets a shared <see cref="SpanPool{T}" /> instance backed by
    /// <see cref="ArrayPool{T}.Shared" />.
    /// </summary>
    public static SpanPool<T> Shared { get; } = new(ArrayPool<T>.Shared);

    /// <summary>
    /// Rents a buffer of at least <paramref name="minimumLength" /> elements from the pool
    /// and wraps it in an <see cref="ISpanOwner{T}" /> that returns the buffer to the pool
    /// when disposed.
    /// </summary>
    public ISpanOwner<T> Rent(int minimumLength = 1) =>
        new ArrayPoolSpanOwner<T>(pool, pool.Rent(minimumLength), minimumLength);
}
