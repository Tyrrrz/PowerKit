using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PowerKit;

/// <summary>
/// Provides utility methods for creating and composing <see cref="IDisposable" /> instances.
/// </summary>
#if POWERKIT_EXCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal partial class Disposable(Action dispose) : IDisposable
{
    /// <inheritdoc />
    public void Dispose() => dispose();
}

internal partial class Disposable
{
    /// <summary>
    /// Gets a disposable that performs no action when disposed.
    /// </summary>
    public static IDisposable Null { get; } = Create(() => { });

    /// <summary>
    /// Creates a disposable that invokes the specified action when disposed.
    /// </summary>
    public static IDisposable Create(Action dispose) => new Disposable(dispose);

    /// <summary>
    /// Creates a disposable that disposes all specified disposables when disposed,
    /// aggregating any exceptions thrown during disposal.
    /// </summary>
    public static IDisposable Merge(params IEnumerable<IDisposable> disposables) =>
        Create(() =>
        {
            List<Exception>? exceptions = null;

            foreach (var disposable in disposables)
            {
                try
                {
                    disposable.Dispose();
                }
                catch (Exception ex)
                {
                    (exceptions ??= []).Add(ex);
                }
            }

            if (exceptions?.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        });
}
