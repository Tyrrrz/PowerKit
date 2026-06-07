#if !NETFRAMEWORK || NET45_OR_GREATER
using System;
using System.Threading;

namespace PowerKit;

/// <summary>
/// An observer that synchronizes access to the underlying observer.
/// </summary>
public class SynchronizedObserver<T>(IObserver<T> observer) : IObserver<T>
{
    private readonly Lock _lock = new();

    /// <inheritdoc />
    public void OnCompleted()
    {
        lock (_lock)
        {
            observer.OnCompleted();
        }
    }

    /// <inheritdoc />
    public void OnError(Exception error)
    {
        lock (_lock)
        {
            observer.OnError(error);
        }
    }

    /// <inheritdoc />
    public void OnNext(T value)
    {
        lock (_lock)
        {
            observer.OnNext(value);
        }
    }
}
#endif
