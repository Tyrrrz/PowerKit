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

    /// <summary>
    /// Creates a new progress input with the specified weight.
    /// Progress reported to this input is combined with all other inputs as a normalized
    /// weighted average before being forwarded to the output.
    /// </summary>
    public IProgress<double> CreateInput(double weight = 1)
    {
        int index;

        using (_lock.EnterScope())
        {
            index = _splitWeights.Count;
            _splitWeights.Add(weight);
            _splitValues.Add(0);
        }

        return new DelegateProgress<double>(p =>
        {
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

                output.Report(weightedMax > 0 ? weightedSum / weightedMax : 0);
            }
        });
    }
}
