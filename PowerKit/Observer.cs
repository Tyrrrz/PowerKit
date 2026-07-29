#if !NETFRAMEWORK || NET45_OR_GREATER
using System;

namespace PowerKit;

file class DelegateObserver<T>(Action<T>? onNext, Action<Exception>? onError, Action? onCompleted)
    : IObserver<T>
{
    public void OnNext(T value) => onNext?.Invoke(value);

    public void OnError(Exception error) => onError?.Invoke(error);

    public void OnCompleted() => onCompleted?.Invoke();
}

/// <summary>
/// Provides utility methods for creating <see cref="IObserver{T}" /> instances.
/// </summary>
public static class Observer
{
    /// <summary>
    /// Creates an observer from the specified delegate callbacks.
    /// </summary>
    /// <remarks>
    /// Any callback left as <see langword="null" /> is treated as a no-op.
    /// </remarks>
    public static IObserver<T> Create<T>(
        Action<T>? onNext = null,
        Action<Exception>? onError = null,
        Action? onCompleted = null
    ) => new DelegateObserver<T>(onNext, onError, onCompleted);
}
#endif
