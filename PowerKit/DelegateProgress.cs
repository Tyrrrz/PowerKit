using System;

namespace PowerKit;

/// <summary>
/// Provides a lightweight <see cref="IProgress{T}" /> implementation that delegates
/// progress reporting to an action.
/// </summary>
public class DelegateProgress<T>(Action<T> report) : IProgress<T>
{
    /// <inheritdoc />
    public void Report(T value) => report(value);
}
