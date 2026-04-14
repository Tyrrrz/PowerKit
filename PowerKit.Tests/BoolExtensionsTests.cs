#nullable enable
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
}
