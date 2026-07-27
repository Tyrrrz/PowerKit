#if !NETFRAMEWORK || NET45_OR_GREATER
using System;
using System.Threading;

namespace PowerKit;

/// <summary>
/// An observer that automatically disposes the upstream source subscription when a terminal
/// event (<see cref="OnError" /> or <see cref="OnCompleted" />) is received, or when any
/// observer callback throws.
/// </summary>
internal class AutoDetachObserver<T>(IObserver<T> observer) : IObserver<T>
{
    private readonly Lock _gate = new();
    private IDisposable? _disposable;
    private bool _disposeOnAssign;

    internal void SetDisposable(IDisposable disposable)
    {
        bool shouldDispose;
        lock (_gate)
        {
            shouldDispose = _disposeOnAssign;
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
            _disposeOnAssign = true;
        }

        disposable?.Dispose();
    }

    /// <inheritdoc />
    public void OnNext(T value)
    {
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
        try
        {
            observer.OnCompleted();
        }
        finally
        {
            DisposeSource();
        }
    }
}
#endif
