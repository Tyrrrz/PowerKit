using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class Int32ExtensionsTests
{
    [Fact]
    public void ParseOrNull_Test()
    {
        // Act & assert
        int.ParseOrNull("42").Should().Be(42);
        int.ParseOrNull("-7").Should().Be(-7);
        int.ParseOrNull("abc").Should().BeNull();
        int.ParseOrNull(null).Should().BeNull();
    }

    [Fact]
    public void ParseOrDefault_Test()
    {
        // Act & assert
        int.ParseOrDefault("42").Should().Be(42);
        int.ParseOrDefault("-7").Should().Be(-7);
        int.ParseOrDefault("abc").Should().Be(0);
        int.ParseOrDefault("abc", -1).Should().Be(-1);
        int.ParseOrDefault(null).Should().Be(0);
    }
}
