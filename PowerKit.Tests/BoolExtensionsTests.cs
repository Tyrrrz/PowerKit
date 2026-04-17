using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class BoolExtensionsTests
{
    [Fact]
    public void ParseOrNull_Test()
    {
        // Act & assert
        bool.ParseOrNull("true").Should().BeTrue();
        bool.ParseOrNull("false").Should().BeFalse();
        bool.ParseOrNull("yes").Should().BeNull();
        bool.ParseOrNull(null).Should().BeNull();
    }

    [Fact]
    public void ParseOrDefault_Test()
    {
        // Act & assert
        bool.ParseOrDefault("true").Should().BeTrue();
        bool.ParseOrDefault("false").Should().BeFalse();
        bool.ParseOrDefault("yes").Should().BeFalse();
        bool.ParseOrDefault("yes", true).Should().BeTrue();
        bool.ParseOrDefault(null).Should().BeFalse();
    }
}
