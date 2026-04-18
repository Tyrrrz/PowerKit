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
        var executed = false;
        var command = new FakeCommand(canExecute: _ => true, execute: _ => executed = true);

        // Act
        command.ExecuteIfCan();

        // Assert
        executed.Should().BeTrue();
    }

    [Fact]
    public void ExecuteIfCan_CannotExecute_Test()
    {
        // Arrange
        var executed = false;
        var command = new FakeCommand(canExecute: _ => false, execute: _ => executed = true);

        // Act
        command.ExecuteIfCan();

        // Assert
        executed.Should().BeFalse();
    }
}

file class FakeCommand(Func<object?, bool>? canExecute = null, Action<object?>? execute = null)
    : ICommand
{
    public event EventHandler? CanExecuteChanged
    {
        add { }
        remove { }
    }

    public bool CanExecute(object? parameter) => canExecute?.Invoke(parameter) ?? true;

    public void Execute(object? parameter) => execute?.Invoke(parameter);
}
