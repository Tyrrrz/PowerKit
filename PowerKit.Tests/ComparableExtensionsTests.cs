using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class ComparableExtensionsTests
{
    [Fact]
    public void Clamp_Test()
    {
        // Act & assert
        5.Clamp(1, 10).Should().Be(5);
        1.Clamp(3, 10).Should().Be(3);
        20.Clamp(1, 7).Should().Be(7);
        3.14.Clamp(0.0, 3.0).Should().Be(3.0);
        "banana".Clamp("apple", "cherry").Should().Be("banana");
    }

    [Fact]
    public void Min_Test()
    {
        // Act & assert
        5.Min(3).Should().Be(3);
        2.Min(7).Should().Be(2);
        4.Min(4).Should().Be(4);
    }

    [Fact]
    public void Max_Test()
    {
        // Act & assert
        5.Max(3).Should().Be(5);
        2.Max(7).Should().Be(7);
        4.Max(4).Should().Be(4);
    }
}
