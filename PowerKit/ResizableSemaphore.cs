#if NET40_OR_GREATER || NETSTANDARD || NET
#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace PowerKit;

/// <summary>
/// Semaphore whose maximum concurrency count can be adjusted at run time.
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
    private int _count;

    /// <summary>
    /// Gets or sets the maximum number of concurrent accesses.
    /// Defaults to <see cref="int.MaxValue" />.
    /// </summary>
    public int MaxCount
    {
        get => field;
        set
        {
            using (_lock.EnterScope())
                field = value;

            Refresh();
        }
    } = int.MaxValue;

    private void Refresh()
    {
        using (_lock.EnterScope())
        {
            // Provide access to pending waiters, as long as max count allows
            while (_count < MaxCount && _waiters.TryDequeue(out var waiter))
            {
                // Don't increment the count if the waiter has already been
                // completed before (most likely by getting canceled).
                if (waiter!.TrySetResult())
                    _count++;
            }
        }
    }

    private void Release()
    {
        using (_lock.EnterScope())
            _count--;

        Refresh();
    }

    /// <summary>
    /// Acquires access to the semaphore, waiting asynchronously if the maximum concurrency count
    /// has been reached. Dispose the returned handle to release access.
    /// </summary>
    public async Task<IDisposable> AcquireAsync(CancellationToken cancellationToken = default)
    {
        var waiter = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        using (_cts.Token.Register(() => waiter.TrySetCanceled(_cts.Token)))
        using (cancellationToken.Register(() =>
                waiter.TrySetCanceled(cancellationToken)
            ))
        using (_lock.EnterScope())
        {
            ObjectDisposedException.ThrowIf(_isDisposed, this);
            _waiters.Enqueue(waiter);
        }

        Refresh();
        await waiter.Task.ConfigureAwait(false);

        return Disposable.Create(Release);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        using (_lock.EnterScope())
        {
            if (_isDisposed)
                return;

            _isDisposed = true;
            _cts.Cancel();
        }

        _cts.Dispose();
    }
}
#endif
