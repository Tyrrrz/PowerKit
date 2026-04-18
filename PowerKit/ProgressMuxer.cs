#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
    private readonly List<double> _splitWeights = new();
    private readonly List<double> _splitValues = new();
    private long _version;

    /// <summary>
    /// Creates a new progress input with the specified weight.
    /// Progress reported to this input is combined with all other inputs as a normalized
    /// weighted average before being forwarded to the output.
    /// </summary>
    public IProgress<double> CreateInput(double weight = 1)
    {
        if (double.IsNaN(weight) || double.IsInfinity(weight) || weight < 0)
            throw new ArgumentOutOfRangeException(nameof(weight));

        var index = 0;
        using (_lock.EnterScope())
        {
            index = _splitWeights.Count;
            _splitWeights.Add(weight);
            _splitValues.Add(0);
        }

        return new DelegateProgress<double>(p =>
        {
            var value = 0.0;
            var version = 0L;
            using (_lock.EnterScope())
            {
                _splitValues[index] = p;

                var weightedSum = 0.0;
                var weightedMax = 0.0;

                for (var i = 0; i < _splitWeights.Count; i++)
                {
                    weightedSum += _splitWeights[i] * _splitValues[i];
                    weightedMax += _splitWeights[i];
                }

                value = weightedMax > 0 ? weightedSum / weightedMax : 0;
                version = Interlocked.Increment(ref _version);
            }

            if (Interlocked.Read(ref _version) == version)
                output.Report(value);
        });
    }
}
