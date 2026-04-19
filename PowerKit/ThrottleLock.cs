#if NET40_OR_GREATER || NETSTANDARD || NET
#nullable enable
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace PowerKit;

/// <summary>
/// Represents a lock that enforces a minimum interval between consecutive acquisitions,
/// ensuring that operations do not proceed faster than the specified rate.
/// </summary>
#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class ThrottleLock(TimeSpan interval) : IDisposable
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private long? _lastTimestamp;

    /// <summary>
    /// Asynchronously waits until the throttle interval has elapsed since the last acquisition,
    /// then records the current instant as the new last-request time.
    /// </summary>
    public async Task WaitAsync(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            if (_lastTimestamp is { } last)
            {
                var remaining = interval - Stopwatch.GetElapsedTime(last);
                if (remaining > TimeSpan.Zero)
                    await Task.Delay(remaining, cancellationToken).ConfigureAwait(false);
            }

            _lastTimestamp = Stopwatch.GetTimestamp();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <inheritdoc />
    public void Dispose() => _semaphore.Dispose();
}
#endif
