using System.Globalization;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class SingleExtensionsTests
{
    [Fact]
    public void ParseOrNull_Test()
    {
        // Act & assert
        float.ParseOrNull("3.14", CultureInfo.InvariantCulture)
            .Should()
            .BeApproximately(3.14f, 0.001f);
        float.ParseOrNull("-1.5", CultureInfo.InvariantCulture).Should().Be(-1.5f);
        float.ParseOrNull("abc").Should().BeNull();
        float.ParseOrNull(null).Should().BeNull();
    }

    [Fact]
    public void ParseOrDefault_Test()
    {
        // Act & assert
        float.ParseOrDefault("3.14", CultureInfo.InvariantCulture)
            .Should()
            .BeApproximately(3.14f, 0.001f);
        float.ParseOrDefault("-1.5", CultureInfo.InvariantCulture).Should().Be(-1.5f);
        float.ParseOrDefault("abc").Should().Be(0.0f);
        float.ParseOrDefault("abc", -1.0f).Should().Be(-1.0f);
        float.ParseOrDefault(null).Should().Be(0.0f);
    }

    [Fact]
    public void Wrap_Test()
    {
        // Act & assert
        5.0f.Wrap(0.0f, 10.0f).Should().Be(5.0f);
        13.0f.Wrap(0.0f, 10.0f).Should().Be(3.0f);
        (-3.0f).Wrap(0.0f, 10.0f).Should().Be(7.0f);
        (-10.0f).Wrap(0.0f, 10.0f).Should().Be(0.0f);
        0.0f.Wrap(0.0f, 10.0f).Should().Be(0.0f);
        10.0f.Wrap(0.0f, 10.0f).Should().Be(0.0f);
        23.0f.Wrap(0.0f, 10.0f).Should().Be(3.0f);
    }
}
