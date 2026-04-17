using System;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class TimeSpanExtensionsTests
{
    [Fact]
    public void ParseOrNull_Test()
    {
        // Act & assert
        TimeSpan.ParseOrNull("1:30:00").Should().Be(new TimeSpan(1, 30, 0));
        TimeSpan.ParseOrNull("not a timespan").Should().BeNull();
        TimeSpan.ParseOrNull(null).Should().BeNull();
    }

    [Fact]
    public void ParseOrDefault_Test()
    {
        // Act & assert
        TimeSpan.ParseOrDefault("1:30:00").Should().Be(new TimeSpan(1, 30, 0));
        TimeSpan.ParseOrDefault("not a timespan").Should().Be(TimeSpan.Zero);
        TimeSpan
            .ParseOrDefault("not a timespan", TimeSpan.FromSeconds(5))
            .Should()
            .Be(TimeSpan.FromSeconds(5));
        TimeSpan.ParseOrDefault(null).Should().Be(TimeSpan.Zero);
    }
}
