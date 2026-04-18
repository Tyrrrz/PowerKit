using System;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class DateTimeExtensionsTests
{
    [Fact]
    public void ParseOrNull_Test()
    {
        // Act & assert
        DateTime.ParseOrNull("2024-06-15").Should().Be(new DateTime(2024, 6, 15));
        DateTime.ParseOrNull("not a date").Should().BeNull();
        DateTime.ParseOrNull(null).Should().BeNull();
    }

    [Fact]
    public void ParseOrDefault_Test()
    {
        // Act & assert
        DateTime.ParseOrDefault("2024-06-15").Should().Be(new DateTime(2024, 6, 15));
        DateTime.ParseOrDefault("not a date").Should().Be(default(DateTime));
        DateTime
            .ParseOrDefault("not a date", new DateTime(2000, 1, 1))
            .Should()
            .Be(new DateTime(2000, 1, 1));
        DateTime.ParseOrDefault(null).Should().Be(default(DateTime));
    }
}
