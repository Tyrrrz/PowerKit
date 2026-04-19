#if NET40_OR_GREATER || NETSTANDARD || NET
#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace PowerKit;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
file class ResizableSemaphoreAccess(ResizableSemaphore semaphore) : IDisposable
{
    private bool _isDisposed;

    public void Dispose()
    {
        if (!_isDisposed)
        {
            semaphore.Release();
        }

        _isDisposed = true;
    }
}

/// <summary>
/// Like a regular semaphore, but the max count can be changed at any point.
/// </summary>
#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class ResizableSemaphore : IDisposable
{
    private readonly Lock _lock = new();
    private readonly Queue<TaskCompletionSource> _waiters = new();
    private readonly CancellationTokenSource _cts = new();

    private bool _isDisposed;
    private int _maxCount = int.MaxValue;
    private int _count;

    /// <summary>
    /// Gets or sets the maximum number of concurrent accesses.
    /// Defaults to <see cref="int.MaxValue" />.
    /// </summary>
    public int MaxCount
    {
        get
        {
            using (_lock.EnterScope())
            {
                return _maxCount;
            }
        }
        set
        {
            using (_lock.EnterScope())
            {
                _maxCount = value;
                Refresh();
            }
        }
    }

    // Must be called while holding the lock.
    private void Refresh()
    {
        // Provide access to pending waiters, as long as max count allows.
        while (_count < _maxCount && _waiters.TryDequeue(out var waiter))
        {
            // Don't increment the count if the waiter has already been
            // completed before (most likely by getting canceled).
            if (waiter!.TrySetResult())
                _count++;
        }
    }

    /// <summary>
    /// Acquires access to the semaphore, waiting asynchronously if the max count has been reached.
    /// Dispose the returned handle to release access.
    /// </summary>
    public async Task<IDisposable> AcquireAsync(CancellationToken cancellationToken = default)
    {
        if (_isDisposed)
            throw new ObjectDisposedException(GetType().Name);

        var waiter = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        using var ctsRegistration = _cts.Token.Register(() => waiter.TrySetCanceled(_cts.Token));
        using var ctRegistration = cancellationToken.Register(() =>
            waiter.TrySetCanceled(cancellationToken)
        );

        using (_lock.EnterScope())
        {
            _waiters.Enqueue(waiter);
            Refresh();
        }

        await waiter.Task.ConfigureAwait(false);

        return new ResizableSemaphoreAccess(this);
    }

    internal void Release()
    {
        using (_lock.EnterScope())
        {
            _count--;
            Refresh();
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (!_isDisposed)
        {
            _cts.Cancel();
            _cts.Dispose();
        }

        _isDisposed = true;
    }
}
#endif
