using System.Globalization;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class ByteExtensionsTests
{
    [Fact]
    public void ParseOrNull_Test()
    {
        // Act & assert
        byte.ParseOrNull("42", CultureInfo.InvariantCulture).Should().Be(42);
        byte.ParseOrNull("255", CultureInfo.InvariantCulture).Should().Be(255);
        byte.ParseOrNull("abc").Should().BeNull();
        byte.ParseOrNull(null).Should().BeNull();
    }

    [Fact]
    public void ParseOrDefault_Test()
    {
        // Act & assert
        byte.ParseOrDefault("42", CultureInfo.InvariantCulture).Should().Be(42);
        byte.ParseOrDefault("255", CultureInfo.InvariantCulture).Should().Be(255);
        byte.ParseOrDefault("abc").Should().Be(0);
        byte.ParseOrDefault("abc", 7).Should().Be(7);
        byte.ParseOrDefault(null).Should().Be(0);
    }
}
