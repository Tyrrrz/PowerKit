#nullable enable
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
}
