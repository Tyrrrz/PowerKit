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
        var interval = TimeSpan.FromMilliseconds(50);
        using var throttle = new ThrottleLock(interval);
        var stopwatch = Stopwatch.StartNew();

        await throttle.WaitAsync();
        await throttle.WaitAsync();
        await throttle.WaitAsync();

        stopwatch
            .Elapsed.Should()
            .BeGreaterThanOrEqualTo(interval * 3 - TimeSpan.FromMilliseconds(50));
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
