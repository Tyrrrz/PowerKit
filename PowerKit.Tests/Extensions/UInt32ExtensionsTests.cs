using System.Globalization;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class UInt32ExtensionsTests
{
    [Fact]
    public void ParseOrNull_Test()
    {
        // Act & assert
        uint.ParseOrNull("42", CultureInfo.InvariantCulture).Should().Be(42U);
        uint.ParseOrNull("4294967295", CultureInfo.InvariantCulture).Should().Be(uint.MaxValue);
        uint.ParseOrNull("abc").Should().BeNull();
        uint.ParseOrNull(null).Should().BeNull();
    }

    [Fact]
    public void ParseOrDefault_Test()
    {
        // Act & assert
        uint.ParseOrDefault("42", CultureInfo.InvariantCulture).Should().Be(42U);
        uint.ParseOrDefault("4294967295", CultureInfo.InvariantCulture).Should().Be(uint.MaxValue);
        uint.ParseOrDefault("abc").Should().Be(0U);
        uint.ParseOrDefault("abc", 7U).Should().Be(7U);
        uint.ParseOrDefault(null).Should().Be(0U);
    }
}
