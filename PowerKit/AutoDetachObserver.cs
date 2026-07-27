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
    private readonly Lock _gate = new();
    private IDisposable? _disposable;
    private bool _stopped;

    internal void SetDisposable(IDisposable disposable)
    {
        bool shouldDispose;
        lock (_gate)
        {
            shouldDispose = _stopped;
            if (!shouldDispose)
                _disposable = disposable;
        }

        if (shouldDispose)
            disposable.Dispose();
    }

    private void DisposeSource()
    {
        IDisposable? disposable;
        lock (_gate)
        {
            disposable = _disposable;
            _disposable = null;
            _stopped = true;
        }

        disposable?.Dispose();
    }

    /// <inheritdoc />
    public void OnNext(T value)
    {
        lock (_gate)
        {
            if (_stopped)
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
        lock (_gate)
        {
            if (_stopped)
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
        lock (_gate)
        {
            if (_stopped)
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
