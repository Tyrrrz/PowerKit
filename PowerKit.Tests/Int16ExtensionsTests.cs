using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class Int16ExtensionsTests
{
    [Fact]
    public void ParseOrNull_Test()
    {
        // Act & assert
        short.ParseOrNull("42").Should().Be(42);
        short.ParseOrNull("-7").Should().Be(-7);
        short.ParseOrNull("abc").Should().BeNull();
        short.ParseOrNull(null).Should().BeNull();
    }

    [Fact]
    public void ParseOrDefault_Test()
    {
        // Act & assert
        short.ParseOrDefault("42").Should().Be(42);
        short.ParseOrDefault("-7").Should().Be(-7);
        short.ParseOrDefault("abc").Should().Be(0);
        short.ParseOrDefault("abc", -1).Should().Be(-1);
        short.ParseOrDefault(null).Should().Be(0);
    }
}
