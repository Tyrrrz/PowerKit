using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
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
        using var access1 = await semaphore.AcquireAsync();
        var access2Task = semaphore.AcquireAsync();

        // Assert
        access2Task.IsCompleted.Should().BeFalse();
        access1.Dispose();
        using var access2 = await access2Task;
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
    public async Task AcquireAsync_Resized_Test()
    {
        // Arrange
        using var semaphore = new ResizableSemaphore { MaxCount = 1 };
        using var _ = await semaphore.AcquireAsync();

        // Act
        var accessTask = semaphore.AcquireAsync();
        semaphore.MaxCount = 2;

        // Assert
        using var access = await accessTask;
    }
}
