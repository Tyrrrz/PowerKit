using FluentAssertions;
using PowerKit;
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
    }

    [Fact]
    public void TryOpen_ValueNotSet_Test()
    {
        // Arrange
        var cell = new Cell<int?>();

        // Act
        var result = cell.TryOpen(out var value);

        // Assert
        result.Should().BeFalse();
        value.Should().BeNull();
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
    }

    [Fact]
    public void OpenOrDefault_ValueNotSet_Test()
    {
        // Arrange
        var cell = new Cell<int?>();

        // Act & assert
        cell.OpenOrDefault().Should().BeNull();
        cell.OpenOrDefault(42).Should().Be(42);
    }

    [Fact]
    public void OpenOrDefault_ValueSet_Test()
    {
        // Arrange
        var cell = new Cell<int?>();

        // Act
        cell.Store(42);

        // Assert
        cell.OpenOrDefault().Should().Be(42);
        cell.OpenOrDefault(99).Should().Be(42);
    }
}
