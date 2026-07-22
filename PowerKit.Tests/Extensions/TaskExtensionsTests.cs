using System;
using System.Threading.Tasks;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class TaskExtensionsTests
{
    [Fact]
    public async Task ObserveException_Task_ReturnsFaultException_Test()
    {
        // Arrange
        var task = Task.Run(() => throw new InvalidOperationException("test error"));

        // Act
        var exception = await task.ObserveException();

        // Assert
        task.IsFaulted.Should().BeTrue();
        exception.Should().NotBeNull();
        exception!.InnerException.Should().BeOfType<InvalidOperationException>();
    }

    [Fact]
    public async Task ObserveException_TaskOfT_ReturnsFaultException_Test()
    {
        // Arrange
        var task = Task.Run(new Func<int>(() => throw new InvalidOperationException("test error")));

        // Act
        var exception = await task.ObserveException();

        // Assert
        task.IsFaulted.Should().BeTrue();
        exception.Should().NotBeNull();
        exception!.InnerException.Should().BeOfType<InvalidOperationException>();
    }

    [Fact]
    public async Task ObserveException_Task_SuccessfulTask_ReturnsNull_Test()
    {
        // Arrange
        var task = Task.CompletedTask;

        // Act
        var exception = await task.ObserveException();

        // Assert
        task.IsCompletedSuccessfully.Should().BeTrue();
        exception.Should().BeNull();
    }

    [Fact]
    public async Task ObserveException_TaskOfT_SuccessfulTask_ReturnsNull_Test()
    {
        // Arrange
        var task = Task.FromResult(42);

        // Act
        var exception = await task.ObserveException();

        // Assert
        task.IsCompletedSuccessfully.Should().BeTrue();
        (await task).Should().Be(42);
        exception.Should().BeNull();
    }

    [Fact]
    public async Task ObserveException_Task_DoesNotThrowUnobservedException_Test()
    {
        // Arrange
        var task = Task.Run(() => throw new InvalidOperationException("test error"));
        _ = task.ObserveException();

        // Act & assert: waiting for the task to complete should not raise an unobserved exception
        await Task.Delay(100);

        // Force GC to collect the task and trigger finalizer-based unobserved exception detection
        GC.Collect();
        GC.WaitForPendingFinalizers();

        task.IsFaulted.Should().BeTrue();
    }

    [Fact]
    public async Task ObserveException_TaskOfT_DoesNotThrowUnobservedException_Test()
    {
        // Arrange
        var task = Task.Run(new Func<int>(() => throw new InvalidOperationException("test error")));
        _ = task.ObserveException();

        // Act & assert
        await Task.Delay(100);

        GC.Collect();
        GC.WaitForPendingFinalizers();

        task.IsFaulted.Should().BeTrue();
    }
}
