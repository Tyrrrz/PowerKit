using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PowerKit.Tests;

public class ThrottleLockTests
{
    [Fact]
    public async Task WaitAsync_Test()
    {
        // Arrange
        using var throttle = new ThrottleLock(TimeSpan.FromMilliseconds(50));

        // Act
        var stopwatch = Stopwatch.StartNew();
        await throttle.WaitAsync();
        await throttle.WaitAsync();
        await throttle.WaitAsync();

        // Assert
        stopwatch.Elapsed.Should().BeGreaterThanOrEqualTo(2 * TimeSpan.FromMilliseconds(50));
    }

    [Fact]
    public async Task WaitAsync_Cancellation_Test()
    {
        // Arrange
        using var throttle = new ThrottleLock(TimeSpan.FromSeconds(10));

        // Act & assert
        var act = async () => await throttle.WaitAsync(new CancellationToken(true));
        await act.Should().ThrowAsync<OperationCanceledException>();
    }
}
