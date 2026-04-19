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
    public async Task AcquireAsync_WithinMaxCount_Test()
    {
        // Arrange
        using var semaphore = new ResizableSemaphore { MaxCount = 2 };

        // Act
        using var access1 = await semaphore.AcquireAsync();
        using var access2 = await semaphore.AcquireAsync();

        // Assert
        access1.Should().NotBeNull();
        access2.Should().NotBeNull();
    }

    [Fact]
    public async Task AcquireAsync_BlocksWhenMaxCountReached_Test()
    {
        // Arrange
        using var semaphore = new ResizableSemaphore { MaxCount = 1 };
        using var access1 = await semaphore.AcquireAsync();

        // Act
        var acquireTask = semaphore.AcquireAsync();

        // Assert
        acquireTask.IsCompleted.Should().BeFalse();

        // Release and let the second acquire complete
        access1.Dispose();
        using var access2 = await acquireTask;
        access2.Should().NotBeNull();
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
    public async Task AcquireAsync_Dispose_CancelsWaiters_Test()
    {
        // Arrange
        using var semaphore = new ResizableSemaphore { MaxCount = 1 };
        using var access = await semaphore.AcquireAsync();

        // Act
        var acquireTask = semaphore.AcquireAsync();
        semaphore.Dispose();

        // Assert
        await acquireTask.Awaiting(t => t).Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task AcquireAsync_AfterDispose_Throws_Test()
    {
        // Arrange
        using var semaphore = new ResizableSemaphore();
        semaphore.Dispose();

        // Act & assert
        await semaphore
            .Awaiting(s => s.AcquireAsync())
            .Should()
            .ThrowAsync<ObjectDisposedException>();
    }

    [Fact]
    public async Task MaxCount_IncreasedUnblocksWaiters_Test()
    {
        // Arrange
        using var semaphore = new ResizableSemaphore { MaxCount = 1 };
        using var access1 = await semaphore.AcquireAsync();

        // Act
        var acquireTask = semaphore.AcquireAsync();
        semaphore.MaxCount = 2;

        // Assert
        using var access2 = await acquireTask;
        access2.Should().NotBeNull();
    }

    [Fact]
    public async Task Release_AllowsNextWaiter_Test()
    {
        // Arrange
        using var semaphore = new ResizableSemaphore { MaxCount = 1 };

        // Act
        var access1 = await semaphore.AcquireAsync();
        var acquireTask = semaphore.AcquireAsync();

        access1.Dispose();

        // Assert
        using var access2 = await acquireTask;
        access2.Should().NotBeNull();
    }

    [Fact]
    public async Task Release_DoubleDispose_OnlyReleasesOnce_Test()
    {
        // Arrange
        using var semaphore = new ResizableSemaphore { MaxCount = 1 };
        var access = await semaphore.AcquireAsync();

        // Act
        access.Dispose();
        access.Dispose();

        // Assert: should still be able to acquire once (count wasn't double-decremented)
        using var access2 = await semaphore.AcquireAsync();
        access2.Should().NotBeNull();
    }
}
