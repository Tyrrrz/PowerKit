using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class UShortExtensionsTests
{
    [Fact]
    public void ParseOrNull_Test()
    {
        // Act & assert
        ushort.ParseOrNull("42").Should().Be(42);
        ushort.ParseOrNull("65535").Should().Be(65535);
        ushort.ParseOrNull("abc").Should().BeNull();
        ushort.ParseOrNull(null).Should().BeNull();
    }

    [Fact]
    public void ParseOrDefault_Test()
    {
        // Act & assert
        ushort.ParseOrDefault("42").Should().Be(42);
        ushort.ParseOrDefault("65535").Should().Be(65535);
        ushort.ParseOrDefault("abc").Should().Be(0);
        ushort.ParseOrDefault("abc", 7).Should().Be(7);
        ushort.ParseOrDefault(null).Should().Be(0);
    }
}
