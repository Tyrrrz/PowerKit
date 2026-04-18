using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class UInt64ExtensionsTests
{
    [Fact]
    public void ParseOrNull_Test()
    {
        // Act & assert
        ulong.ParseOrNull("42").Should().Be(42UL);
        ulong.ParseOrNull("18446744073709551615").Should().Be(ulong.MaxValue);
        ulong.ParseOrNull("abc").Should().BeNull();
        ulong.ParseOrNull(null).Should().BeNull();
    }

    [Fact]
    public void ParseOrDefault_Test()
    {
        // Act & assert
        ulong.ParseOrDefault("42").Should().Be(42UL);
        ulong.ParseOrDefault("18446744073709551615").Should().Be(ulong.MaxValue);
        ulong.ParseOrDefault("abc").Should().Be(0UL);
        ulong.ParseOrDefault("abc", 7UL).Should().Be(7UL);
        ulong.ParseOrDefault(null).Should().Be(0UL);
    }
}
