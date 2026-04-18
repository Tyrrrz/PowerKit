using System;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class DateTimeOffsetExtensionsTests
{
    [Fact]
    public void ParseOrNull_Test()
    {
        // Act & assert
        DateTimeOffset
            .ParseOrNull("2024-06-15T00:00:00+00:00")
            .Should()
            .Be(new DateTimeOffset(2024, 6, 15, 0, 0, 0, TimeSpan.Zero));
        DateTimeOffset.ParseOrNull("not a date").Should().BeNull();
        DateTimeOffset.ParseOrNull(null).Should().BeNull();
    }

    [Fact]
    public void ParseOrDefault_Test()
    {
        // Act & assert
        DateTimeOffset
            .ParseOrDefault("2024-06-15T00:00:00+00:00")
            .Should()
            .Be(new DateTimeOffset(2024, 6, 15, 0, 0, 0, TimeSpan.Zero));
        DateTimeOffset.ParseOrDefault("not a date").Should().Be(default(DateTimeOffset));
        DateTimeOffset
            .ParseOrDefault("not a date", new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero))
            .Should()
            .Be(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero));
        DateTimeOffset.ParseOrDefault(null).Should().Be(default(DateTimeOffset));
    }
}
