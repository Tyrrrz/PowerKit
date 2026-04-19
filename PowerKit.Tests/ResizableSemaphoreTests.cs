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

        // Act
        var access1 = await semaphore.AcquireAsync();
        var acquireTask = semaphore.AcquireAsync();

        // Assert
        acquireTask.IsCompleted.Should().BeFalse();
        access1.Dispose();
        using var access2 = await acquireTask;
    }

    [Fact]
    public async Task AcquireAsync_Cancellation_Test()
    {
        // Arrange
        using var semaphore = new ResizableSemaphore { MaxCount = 1 };
        using var _ = await semaphore.AcquireAsync();

        // Act & assert
        var act = async () => await semaphore.AcquireAsync(new CancellationToken(true));
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task MaxCount_Test()
    {
        // Arrange
        using var semaphore = new ResizableSemaphore { MaxCount = 1 };
        using var _ = await semaphore.AcquireAsync();

        // Act
        var acquireTask = semaphore.AcquireAsync();
        semaphore.MaxCount = 2;

        // Assert
        using var access = await acquireTask;
    }
}
