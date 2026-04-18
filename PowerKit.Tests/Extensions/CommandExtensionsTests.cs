using System;
using System.Windows.Input;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class CommandExtensionsTests
{
    [Fact]
    public void ExecuteIfCan_CanExecute_Test()
    {
        // Arrange
        var command = new FakeCommand(canExecute: true);

        // Act
        command.ExecuteIfCan();

        // Assert
        command.ExecuteCount.Should().Be(1);
    }

    [Fact]
    public void ExecuteIfCan_CannotExecute_Test()
    {
        // Arrange
        var command = new FakeCommand(canExecute: false);

        // Act
        command.ExecuteIfCan();

        // Assert
        command.ExecuteCount.Should().Be(0);
    }
}

file class FakeCommand(bool canExecute) : ICommand
{
    public int ExecuteCount { get; private set; }

    public event EventHandler? CanExecuteChanged
    {
        add { }
        remove { }
    }

    public bool CanExecute(object? parameter) => canExecute;

    public void Execute(object? parameter) => ExecuteCount++;
}
