using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class LongExtensionsTests
{
    [Fact]
    public void ParseOrNull_Test()
    {
        // Act & assert
        long.ParseOrNull("9876543210").Should().Be(9876543210L);
        long.ParseOrNull("-1").Should().Be(-1L);
        long.ParseOrNull("abc").Should().BeNull();
        long.ParseOrNull(null).Should().BeNull();
    }

    [Fact]
    public void ParseOrDefault_Test()
    {
        // Act & assert
        long.ParseOrDefault("9876543210").Should().Be(9876543210L);
        long.ParseOrDefault("-1").Should().Be(-1L);
        long.ParseOrDefault("abc").Should().Be(0L);
        long.ParseOrDefault("abc", -1L).Should().Be(-1L);
        long.ParseOrDefault(null).Should().Be(0L);
    }
}
