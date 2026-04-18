using System.Globalization;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class SByteExtensionsTests
{
    [Fact]
    public void ParseOrNull_Test()
    {
        // Act & assert
        sbyte.ParseOrNull("42", CultureInfo.InvariantCulture).Should().Be(42);
        sbyte.ParseOrNull("-7", CultureInfo.InvariantCulture).Should().Be(-7);
        sbyte.ParseOrNull("abc").Should().BeNull();
        sbyte.ParseOrNull(null).Should().BeNull();
    }

    [Fact]
    public void ParseOrDefault_Test()
    {
        // Act & assert
        sbyte.ParseOrDefault("42", CultureInfo.InvariantCulture).Should().Be(42);
        sbyte.ParseOrDefault("-7", CultureInfo.InvariantCulture).Should().Be(-7);
        sbyte.ParseOrDefault("abc").Should().Be(0);
        sbyte.ParseOrDefault("abc", -1).Should().Be(-1);
        sbyte.ParseOrDefault(null).Should().Be(0);
    }
}
