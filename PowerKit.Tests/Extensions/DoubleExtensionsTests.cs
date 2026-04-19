using System.Globalization;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

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

    [Fact]
    public void Wrap_Test()
    {
        // Act & assert
        5.0.Wrap(0.0, 10.0).Should().Be(5.0);
        13.0.Wrap(0.0, 10.0).Should().Be(3.0);
        (-3.0).Wrap(0.0, 10.0).Should().Be(7.0);
        0.0.Wrap(0.0, 10.0).Should().Be(0.0);
        10.0.Wrap(0.0, 10.0).Should().Be(0.0);
        23.0.Wrap(0.0, 10.0).Should().Be(3.0);
    }
}
