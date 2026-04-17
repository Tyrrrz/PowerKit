#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;

namespace PowerKit;

#if NET40_OR_GREATER || NETSTANDARD || NET
/// <summary>
/// An observer that synchronizes access to the underlying observer.
/// </summary>
#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class SynchronizedObserver<T>(IObserver<T> observer) : IObserver<T>
{
    private readonly object _lock = new();

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
