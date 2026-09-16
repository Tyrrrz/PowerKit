using System;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class RandomExtensionsTests
{
    [Fact]
    public void NextBoolean_Test()
    {
        // Arrange
        var random = new Random(1234);

        // Act
        var trueCount = 0;
        const int iterations = 10000;
        for (var i = 0; i < iterations; i++)
        {
            if (random.NextBoolean())
                trueCount++;
        }

        // Assert
        ((double)trueCount / iterations)
            .Should()
            .BeApproximately(0.5, 0.05);
    }

    [Fact]
    public void NextBoolean_AlwaysTrue_Test()
    {
        // Arrange
        var random = new Random(1234);

        // Act & assert
        for (var i = 0; i < 100; i++)
            random.NextBoolean(1).Should().BeTrue();
    }

    [Fact]
    public void NextBoolean_AlwaysFalse_Test()
    {
        // Arrange
        var random = new Random(1234);

        // Act & assert
        for (var i = 0; i < 100; i++)
            random.NextBoolean(0).Should().BeFalse();
    }

    [Fact]
    public void NextBoolean_InvalidProbability_Test()
    {
        // Arrange
        var random = new Random(1234);

        // Act & assert
        random.Invoking(r => r.NextBoolean(-0.1)).Should().Throw<ArgumentOutOfRangeException>();
        random.Invoking(r => r.NextBoolean(1.1)).Should().Throw<ArgumentOutOfRangeException>();
    }
}
