using System;
using FluentAssertions;
using PowerKit.Extensions;

namespace PowerKit.Tests;

public class ComparableExtensionsTests
{
    [Fact]
    public void Clamp_Test()
    {
        // Arrange & act
        var result = 5.Clamp(1, 10);

        // Assert
        result.Should().Be(5);
    }

    [Fact]
    public void Clamp_BelowMin_Test()
    {
        // Act
        var result = 1.Clamp(3, 10);

        // Assert
        result.Should().Be(3);
    }

    [Fact]
    public void Clamp_AboveMax_Test()
    {
        // Act
        var result = 20.Clamp(1, 7);

        // Assert
        result.Should().Be(7);
    }

    [Fact]
    public void Clamp_EqualToMin_Test()
    {
        // Act & assert
        3.Clamp(3, 10).Should().Be(3);
    }

    [Fact]
    public void Clamp_EqualToMax_Test()
    {
        // Act & assert
        10.Clamp(1, 10).Should().Be(10);
    }

    [Fact]
    public void Clamp_WithDouble_Test()
    {
        // Act
        var result = 3.14.Clamp(0.0, 3.0);

        // Assert
        result.Should().Be(3.0);
    }

    [Fact]
    public void Clamp_WithString_Test()
    {
        // Act
        var result = "banana".Clamp("apple", "cherry");

        // Assert
        result.Should().Be("banana");
    }

    [Fact]
    public void Clamp_WithTimeSpan_Test()
    {
        // Arrange
        var value = TimeSpan.FromSeconds(5);

        // Act
        var result = value.Clamp(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10));

        // Assert
        result.Should().Be(value);
    }
}
