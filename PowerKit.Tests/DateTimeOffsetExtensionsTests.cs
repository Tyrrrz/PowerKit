using System;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

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
}
