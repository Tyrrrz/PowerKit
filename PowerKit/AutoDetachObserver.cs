#if !NETFRAMEWORK || NET45_OR_GREATER
using System;
using System.Threading;

namespace PowerKit;

/// <summary>
/// An observer that automatically disposes the upstream source subscription when a terminal
/// event (<see cref="OnError" /> or <see cref="OnCompleted" />) is received, when any
/// observer callback throws, or when the subscription is disposed externally.
/// All observer methods are no-ops once the subscription has been stopped.
/// </summary>
internal class AutoDetachObserver<T>(IObserver<T> observer) : IObserver<T>, IDisposable
{
    private readonly Lock _lock = new();
    private IDisposable? _disposable;
    private bool _isUnsubscribedOrAbandoned;

    internal void SetSubscription(IDisposable disposable)
    {
        IDisposable? toDispose;
        lock (_lock)
        {
            if (_isUnsubscribedOrAbandoned)
            {
                toDispose = disposable;
            }
            else
            {
                _disposable = disposable;
                toDispose = null;
            }
        }

        toDispose?.Dispose();
    }

    private void DisposeSource()
    {
        IDisposable? disposable;
        lock (_lock)
        {
            disposable = _disposable;
            _disposable = null;
            _isUnsubscribedOrAbandoned = true;
        }

        disposable?.Dispose();
    }

    /// <inheritdoc />
    public void OnNext(T value)
    {
        lock (_lock)
        {
            if (_isUnsubscribedOrAbandoned)
                return;
        }

        try
        {
            observer.OnNext(value);
        }
        catch
        {
            DisposeSource();
            throw;
        }
    }

    /// <inheritdoc />
    public void OnError(Exception error)
    {
        lock (_lock)
        {
            if (_isUnsubscribedOrAbandoned)
                return;
        }

        try
        {
            observer.OnError(error);
        }
        finally
        {
            DisposeSource();
        }
    }

    /// <inheritdoc />
    public void OnCompleted()
    {
        lock (_lock)
        {
            if (_isUnsubscribedOrAbandoned)
                return;
        }

        try
        {
            observer.OnCompleted();
        }
        finally
        {
            DisposeSource();
        }
    }

    /// <inheritdoc />
    public void Dispose() => DisposeSource();
}
#endif
