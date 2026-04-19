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
    public async Task WaitAsync_FirstCall_DoesNotDelay_Test()
    {
        // Arrange
        using var throttle = new ThrottleLock(TimeSpan.FromSeconds(1));
        var sw = Stopwatch.StartNew();

        // Act
        await throttle.WaitAsync();

        // Assert: first call should not be delayed
        sw.Elapsed.Should().BeLessThan(TimeSpan.FromMilliseconds(500));
    }

    [Fact]
    public async Task WaitAsync_SubsequentCall_IsThrottled_Test()
    {
        // Arrange
        var interval = TimeSpan.FromMilliseconds(200);
        using var throttle = new ThrottleLock(interval);
        await throttle.WaitAsync();

        var sw = Stopwatch.StartNew();

        // Act
        await throttle.WaitAsync();

        // Assert: second call must have waited at least the interval
        sw.Elapsed.Should().BeGreaterThanOrEqualTo(interval - TimeSpan.FromMilliseconds(50));
    }

    [Fact]
    public async Task WaitAsync_AfterIntervalElapsed_DoesNotDelay_Test()
    {
        // Arrange
        var interval = TimeSpan.FromMilliseconds(100);
        using var throttle = new ThrottleLock(interval);
        await throttle.WaitAsync();

        // Wait longer than the interval so the next call should not be throttled
        await Task.Delay(interval + TimeSpan.FromMilliseconds(100));

        var sw = Stopwatch.StartNew();

        // Act
        await throttle.WaitAsync();

        // Assert: call after interval elapsed should not be significantly delayed
        sw.Elapsed.Should().BeLessThan(TimeSpan.FromMilliseconds(100));
    }

    [Fact]
    public async Task WaitAsync_Cancelled_ThrowsOperationCancelledException_Test()
    {
        // Arrange
        using var throttle = new ThrottleLock(TimeSpan.FromSeconds(10));
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & assert
        var act = async () => await throttle.WaitAsync(cts.Token);
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public void Dispose_Test()
    {
        // Arrange
        var throttle = new ThrottleLock(TimeSpan.FromSeconds(1));

        // Act & assert
        var act = throttle.Dispose;
        act.Should().NotThrow();
    }
}
