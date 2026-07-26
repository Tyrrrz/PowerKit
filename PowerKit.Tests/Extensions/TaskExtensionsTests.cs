using System;
using System.Threading.Tasks;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class TaskExtensionsTests
{
    [Fact]
    public async Task Catch_ReturnsFaultException_Test()
    {
        // Arrange
        var task = Task.Run(() => throw new InvalidOperationException("test error"));

        // Act
        var exception = await task.Catch();

        // Assert
        task.IsFaulted.Should().BeTrue();
        exception.Should().NotBeNull();
        exception!.InnerException.Should().BeOfType<InvalidOperationException>();
    }

    [Fact]
    public async Task Catch_SuccessfulTask_ReturnsNull_Test()
    {
        // Arrange
        var task = Task.CompletedTask;

        // Act
        var exception = await task.Catch();

        // Assert
        task.IsCompletedSuccessfully.Should().BeTrue();
        exception.Should().BeNull();
    }

    [Fact]
    public async Task Catch_DoesNotRaiseUnobservedTaskException_Test()
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
            // Act
            _ = Task.Run(() => throw new InvalidOperationException("test error")).Catch();

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
    }
}
