#if !NETFRAMEWORK || NET45_OR_GREATER
using System;
using System.Threading;

namespace PowerKit;

file class AutoDetachObserver<T>(IObserver<T> observer) : IObserver<T>
{
    private readonly Lock _gate = new();
    private IDisposable? _disposable;
    private bool _disposeOnAssign;

    public void SetDisposable(IDisposable disposable)
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

file class Observable<T>(Func<IObserver<T>, IDisposable> subscribe) : IObservable<T>
{
    public IDisposable Subscribe(IObserver<T> observer) => subscribe(observer);
}

/// <summary>
/// Provides utility methods for creating <see cref="IObservable{T}" /> instances.
/// </summary>
public static class Observable
{
    /// <summary>
    /// Creates an observable that invokes the specified subscribe function when subscribed to.
    /// </summary>
    public static IObservable<T> Create<T>(Func<IObserver<T>, IDisposable> subscribe) =>
        new Observable<T>(observer =>
        {
            var autoDetach = new AutoDetachObserver<T>(observer);
            var disposable = subscribe(autoDetach);
            autoDetach.SetDisposable(disposable);
            return disposable;
        });

    /// <summary>
    /// Creates an observable that invokes the specified subscribe function when subscribed to,
    /// wrapping the observer in a <see cref="SynchronizedObserver{T}" /> to ensure thread safety.
    /// </summary>
    public static IObservable<T> CreateSynchronized<T>(Func<IObserver<T>, IDisposable> subscribe) =>
        Create<T>(observer => subscribe(new SynchronizedObserver<T>(observer)));
}
#endif
