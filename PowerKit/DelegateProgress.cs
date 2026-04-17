#if NET40_OR_GREATER || NETSTANDARD || NET
#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;

namespace PowerKit;

/// <summary>
/// Provides a lightweight <see cref="IProgress{T}" /> implementation that delegates
/// progress reporting to an action.
/// </summary>
#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class DelegateProgress<T>(Action<T> report) : IProgress<T>
{
    /// <inheritdoc />
    public void Report(T value) => report(value);
}
#endif
