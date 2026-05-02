using FluentAssertions;
using Xunit;

namespace PowerKit.Tests;

public class CellTests
{
    [Fact]
    public void TryOpen_Test()
    {
        // Arrange
        var cell = new Cell<int?>();
        cell.Store(42);

        // Act
        var result = cell.TryOpen(out var value);

        // Assert
        result.Should().BeTrue();
        value.Should().Be(42);
        cell.IsEmpty.Should().BeFalse();
    }

    [Fact]
    public void TryOpen_Null_Test()
    {
        // Arrange
        var cell = new Cell<int?>();
        cell.Store(null);

        // Act
        var result = cell.TryOpen(out var value);

        // Assert
        result.Should().BeTrue();
        value.Should().BeNull();
        cell.IsEmpty.Should().BeFalse();
    }

    [Fact]
    public void TryOpen_Unset_Test()
    {
        // Arrange
        var cell = new Cell<int?>();

        // Act
        var result = cell.TryOpen(out var value);

        // Assert
        result.Should().BeFalse();
        value.Should().BeNull();
        cell.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void TryOpen_Cleared_Test()
    {
        // Arrange
        var cell = new Cell<int?>();
        cell.Store(42);
        cell.Clear();

        // Act
        var result = cell.TryOpen(out var value);

        // Assert
        result.Should().BeFalse();
        value.Should().BeNull();
        cell.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void OpenOrDefault_Test()
    {
        // Arrange
        var cell = new Cell<int?>();
        cell.Store(42);

        // Act
        var value = cell.OpenOrDefault();
        var valueOrFallback = cell.OpenOrDefault(99);

        // Assert
        value.Should().Be(42);
        valueOrFallback.Should().Be(42);
    }

    [Fact]
    public void OpenOrDefault_Null_Test()
    {
        // Arrange
        var cell = new Cell<int?>();
        cell.Store(null);

        // Act
        var value = cell.OpenOrDefault();
        var valueOrFallback = cell.OpenOrDefault(99);

        // Act & assert
        value.Should().BeNull();
        valueOrFallback.Should().BeNull();
    }

    [Fact]
    public void OpenOrDefault_Unset_Test()
    {
        // Arrange
        var cell = new Cell<int?>();

        // Act
        var value = cell.OpenOrDefault();
        var valueOrFallback = cell.OpenOrDefault(99);

        // Act & assert
        value.Should().BeNull();
        valueOrFallback.Should().Be(99);
    }

    [Fact]
    public void OpenOrDefault_Cleared_Test()
    {
        // Arrange
        var cell = new Cell<int?>();
        cell.Store(42);
        cell.Clear();

        // Act
        var value = cell.OpenOrDefault();
        var valueOrFallback = cell.OpenOrDefault(99);

        // Assert
        value.Should().BeNull();
        valueOrFallback.Should().Be(99);
        cell.IsEmpty.Should().BeTrue();
    }
}
