using FluentAssertions;
using PowerKit.Extensions;

namespace PowerKit.Tests;

public class TimeSpanExtensionsTests
{
    [Fact]
    public void Clamp_Test()
    {
        // Arrange
        var value = TimeSpan.FromSeconds(5);

        // Act
        var result = value.Clamp(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10));

        // Assert
        result.Should().Be(value);
    }

    [Fact]
    public void Clamp_BelowMin_Test()
    {
        // Arrange
        var min = TimeSpan.FromSeconds(3);

        // Act
        var result = TimeSpan.FromSeconds(1).Clamp(min, TimeSpan.FromSeconds(10));

        // Assert
        result.Should().Be(min);
    }

    [Fact]
    public void Clamp_AboveMax_Test()
    {
        // Arrange
        var max = TimeSpan.FromSeconds(7);

        // Act
        var result = TimeSpan.FromSeconds(20).Clamp(TimeSpan.FromSeconds(1), max);

        // Assert
        result.Should().Be(max);
    }

    [Fact]
    public void Clamp_EqualToMin_Test()
    {
        // Arrange
        var min = TimeSpan.FromSeconds(3);

        // Act & assert
        min.Clamp(min, TimeSpan.FromSeconds(10)).Should().Be(min);
    }

    [Fact]
    public void Clamp_EqualToMax_Test()
    {
        // Arrange
        var max = TimeSpan.FromSeconds(10);

        // Act & assert
        max.Clamp(TimeSpan.FromSeconds(1), max).Should().Be(max);
    }
}
