using System.Globalization;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class DoubleExtensionsTests
{
    [Fact]
    public void ParseOrNull_Test()
    {
        // Act & assert
        double.ParseOrNull("3.14", CultureInfo.InvariantCulture).Should().Be(3.14);
        double.ParseOrNull("-1.5", CultureInfo.InvariantCulture).Should().Be(-1.5);
        double.ParseOrNull("abc").Should().BeNull();
        double.ParseOrNull(null).Should().BeNull();
    }

    [Fact]
    public void ParseOrDefault_Test()
    {
        // Act & assert
        double.ParseOrDefault("3.14", CultureInfo.InvariantCulture).Should().Be(3.14);
        double.ParseOrDefault("-1.5", CultureInfo.InvariantCulture).Should().Be(-1.5);
        double.ParseOrDefault("abc").Should().Be(0.0);
        double.ParseOrDefault("abc", -1.0).Should().Be(-1.0);
        double.ParseOrDefault(null).Should().Be(0.0);
    }
}
