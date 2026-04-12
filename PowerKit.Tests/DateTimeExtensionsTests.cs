using System;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

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
}
