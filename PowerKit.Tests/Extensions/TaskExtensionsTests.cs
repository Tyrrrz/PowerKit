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
    public async Task ObserveException_Task_DoesNotRaiseUnobservedTaskException_Test()
    {
        // Arrange
        var unobservedRaised = false;
        EventHandler<UnobservedTaskExceptionEventArgs> handler = (_, e) =>
        {
            if (e.Exception.InnerException is InvalidOperationException { Message: "test error" })
                unobservedRaised = true;

            e.SetObserved();
        };
        TaskScheduler.UnobservedTaskException += handler;

        try
        {
            // Act: create the task in a separate scope so it can be collected
            CreateFaultedTask();

            await Task.Delay(50);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // Assert
            unobservedRaised.Should().BeFalse();
        }
        finally
        {
            TaskScheduler.UnobservedTaskException -= handler;
        }

        static void CreateFaultedTask()
        {
            Task.Run(() => throw new InvalidOperationException("test error")).ObserveException();
        }
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
