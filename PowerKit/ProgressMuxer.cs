#if NET40_OR_GREATER || NETSTANDARD || NET
#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace PowerKit;

/// <summary>
/// Multiplexes multiple <see cref="IProgress{T}" /> reporters into a single output reporter,
/// combining weighted progress values from multiple sources.
/// </summary>
#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class ProgressMuxer
{
    private readonly object _lock = new();
    private readonly IProgress<double> _output;
    private readonly Dictionary<int, double> _splitTotals;

    private int _splitCount;

    /// <summary>
    /// Initializes a new instance of <see cref="ProgressMuxer" /> that forwards combined
    /// progress to the specified output reporter.
    /// </summary>
    public ProgressMuxer(IProgress<double> output)
    {
        _output = output;
        _splitTotals = new Dictionary<int, double>();
    }

    /// <summary>
    /// Creates a new progress input with the specified weight.
    /// Progress reported to this input is multiplied by <paramref name="weight" />
    /// and combined with all other inputs before being forwarded to the output.
    /// </summary>
    public IProgress<double> CreateInput(double weight = 1)
    {
        var index = _splitCount++;
        return new DelegateProgress(p =>
        {
            lock (_lock)
            {
                _splitTotals[index] = weight * p;
                _output.Report(_splitTotals.Values.Sum());
            }
        });
    }
}

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
file sealed class DelegateProgress(Action<double> report) : IProgress<double>
{
    public void Report(double value) => report(value);
}
#endif
