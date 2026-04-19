using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using PowerKit;
using Xunit;

namespace PowerKit.Tests;

public class ThrottleLockTests
{
    [Fact]
    public async Task WaitAsync_Test()
    {
        var interval = TimeSpan.FromMilliseconds(200);
        using var throttle = new ThrottleLock(interval);
        var stopwatch = Stopwatch.StartNew();

        // First call should not be throttled
        await throttle.WaitAsync();
        stopwatch.Elapsed.Should().BeLessThan(interval);

        // Subsequent call should be throttled for the remainder of the interval
        sw.Restart();
        await throttle.WaitAsync();
        stopwatch.Elapsed.Should().BeGreaterThanOrEqualTo(interval - TimeSpan.FromMilliseconds(50));

        // After the interval elapses, the next call should not be throttled
        await Task.Delay(interval + TimeSpan.FromMilliseconds(50));
        stopwatch.Restart();
        await throttle.WaitAsync();
        stopwatch.Elapsed.Should().BeLessThan(interval);
    }

    [Fact]
    public async Task WaitAsync_Cancellation_Test()
    {
        // Arrange
        using var throttle = new ThrottleLock(TimeSpan.FromSeconds(10));
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & assert
        var act = async () => await throttle.WaitAsync(cts.Token);
        await act.Should().ThrowAsync<OperationCanceledException>();
    }
}
