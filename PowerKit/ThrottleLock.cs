#if !NETFRAMEWORK || NET45_OR_GREATER
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace PowerKit;

/// <summary>
/// Represents a lock that enforces a minimum interval between consecutive acquisitions,
/// ensuring that operations do not proceed faster than the specified rate.
/// </summary>
public class ThrottleLock(TimeSpan interval) : IDisposable
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private long _lastTimestamp = Stopwatch.GetTimestamp();

    /// <summary>
    /// Asynchronously waits until the throttle interval has elapsed since the last acquisition,
    /// then records the current instant as the new last-request time.
    /// </summary>
    public async Task WaitAsync(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            var remaining = interval - Stopwatch.GetElapsedTime(_lastTimestamp);
            if (remaining > TimeSpan.Zero)
                await Task.Delay(remaining, cancellationToken).ConfigureAwait(false);

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
