#if NET40_OR_GREATER || NETSTANDARD || NET
#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace PowerKit;

/// <summary>
/// Multiplexes multiple <see cref="IProgress{T}" /> reporters into a single output reporter,
/// combining weighted progress values from multiple sources.
/// </summary>
#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class ProgressMuxer(IProgress<double> output)
{
    private readonly Lock _lock = new();
    private readonly Dictionary<int, double> _splitTotals = new();

    private int _splitCount;

    /// <summary>
    /// Creates a new progress input with the specified weight.
    /// Progress reported to this input is multiplied by <paramref name="weight" />
    /// and combined with all other inputs before being forwarded to the output.
    /// </summary>
    public IProgress<double> CreateInput(double weight = 1)
    {
        var index = Interlocked.Increment(ref _splitCount) - 1;
        return new DelegateProgress<double>(p =>
        {
            using (_lock.EnterScope())
            {
                _splitTotals[index] = weight * p;
                output.Report(_splitTotals.Values.Sum());
            }
        });
    }
}

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
file sealed class DelegateProgress<T>(Action<T> report) : IProgress<T>
{
    public void Report(T value) => report(value);
}
#endif
