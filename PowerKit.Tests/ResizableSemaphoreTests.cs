using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using PowerKit;
using Xunit;

namespace PowerKit.Tests;

public class ResizableSemaphoreTests
{
    [Fact]
    public async Task AcquireAsync_Test()
    {
        // Arrange
        using var semaphore = new ResizableSemaphore { MaxCount = 1 };

        // Blocks when max count is reached
        var access1 = await semaphore.AcquireAsync();
        var acquireTask = semaphore.AcquireAsync();
        acquireTask.IsCompleted.Should().BeFalse();

        // Releasing unblocks the next waiter
        access1.Dispose();
        using var access2 = await acquireTask;

        // Double-dispose is idempotent
        access1.Dispose();
    }

    [Fact]
    public async Task AcquireAsync_CancellationToken_Test()
    {
        // Arrange
        using var semaphore = new ResizableSemaphore { MaxCount = 1 };
        using var access = await semaphore.AcquireAsync();
        using var cts = new CancellationTokenSource();

        // Act
        var acquireTask = semaphore.AcquireAsync(cts.Token);
        cts.Cancel();

        // Assert
        await acquireTask.Awaiting(t => t).Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task MaxCount_Test()
    {
        // Arrange
        using var semaphore = new ResizableSemaphore { MaxCount = 1 };
        using var access = await semaphore.AcquireAsync();

        // Act: increasing MaxCount unblocks pending waiters
        var acquireTask = semaphore.AcquireAsync();
        semaphore.MaxCount = 2;

        // Assert
        using var access2 = await acquireTask;
    }

    [Fact]
    public async Task Dispose_Test()
    {
        // Arrange
        using var semaphore = new ResizableSemaphore { MaxCount = 1 };
        using var access = await semaphore.AcquireAsync();

        // Act: dispose cancels pending waiters
        var acquireTask = semaphore.AcquireAsync();
        semaphore.Dispose();

        await acquireTask.Awaiting(t => t).Should().ThrowAsync<OperationCanceledException>();

        // Acquire after dispose throws immediately
        await semaphore
            .Awaiting(s => s.AcquireAsync())
            .Should()
            .ThrowAsync<ObjectDisposedException>();
    }
}
