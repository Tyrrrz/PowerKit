using System;
using System.Threading.Tasks;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class TaskExtensionsTests
{
    [Fact]
    public async Task ObserveException_Task_DoesNotThrowUnobservedException_Test()
    {
        // Arrange
        var task = Task.Run(() => throw new InvalidOperationException("test error"));
        task.ObserveException();

        // Act & assert: waiting for the task to complete should not raise an unobserved exception
        await Task.Delay(100);

        // Force GC to collect the task and trigger finalizer-based unobserved exception detection
        GC.Collect();
        GC.WaitForPendingFinalizers();

        // The exception was observed, so no UnobservedTaskException should have been raised
        task.IsFaulted.Should().BeTrue();
    }

    [Fact]
    public async Task ObserveException_TaskOfT_DoesNotThrowUnobservedException_Test()
    {
        // Arrange
        var task = Task.Run(new Func<int>(() => throw new InvalidOperationException("test error")));
        task.ObserveException();

        // Act & assert
        await Task.Delay(100);

        GC.Collect();
        GC.WaitForPendingFinalizers();

        task.IsFaulted.Should().BeTrue();
    }

    [Fact]
    public async Task ObserveException_Task_SuccessfulTask_IsUnaffected_Test()
    {
        // Arrange
        var task = Task.CompletedTask;
        task.ObserveException();

        // Act & assert
        await Task.Delay(50);

        task.IsCompletedSuccessfully.Should().BeTrue();
    }

    [Fact]
    public async Task ObserveException_TaskOfT_SuccessfulTask_IsUnaffected_Test()
    {
        // Arrange
        var task = Task.FromResult(42);
        task.ObserveException();

        // Act & assert
        await Task.Delay(50);

        task.IsCompletedSuccessfully.Should().BeTrue();
        (await task).Should().Be(42);
    }
}
