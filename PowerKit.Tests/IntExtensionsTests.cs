#nullable enable
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class IntExtensionsTests
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
}
