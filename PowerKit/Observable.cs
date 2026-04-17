#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;

namespace PowerKit;

#if NET40_OR_GREATER || NETSTANDARD || NET
/// <summary>
/// Represents an observable sequence of values.
/// </summary>
#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class Observable<T>(Func<IObserver<T>, IDisposable> subscribe) : IObservable<T>
{
    /// <inheritdoc />
    public IDisposable Subscribe(IObserver<T> observer) => subscribe(observer);
}

/// <summary>
/// Provides utility methods for creating <see cref="IObservable{T}" /> instances.
/// </summary>
#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class Observable
{
    /// <summary>
    /// Creates an observable that invokes the specified subscribe function when subscribed to.
    /// </summary>
    public static IObservable<T> Create<T>(Func<IObserver<T>, IDisposable> subscribe) =>
        new Observable<T>(subscribe);

    /// <summary>
    /// Creates an observable that invokes the specified subscribe function when subscribed to,
    /// wrapping the observer in a <see cref="SynchronizedObserver{T}" /> to ensure thread safety.
    /// </summary>
    public static IObservable<T> CreateSynchronized<T>(Func<IObserver<T>, IDisposable> subscribe) =>
        Create<T>(observer => subscribe(new SynchronizedObserver<T>(observer)));
}
#endif
