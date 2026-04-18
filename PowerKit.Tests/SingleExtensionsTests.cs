using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class SingleExtensionsTests
{
    [Fact]
    public void ParseOrNull_Test()
    {
        // Act & assert
        float.ParseOrNull("3").Should().Be(3f);
        float.ParseOrNull("-1").Should().Be(-1f);
        float.ParseOrNull("abc").Should().BeNull();
        float.ParseOrNull(null).Should().BeNull();
    }

    [Fact]
    public void ParseOrDefault_Test()
    {
        // Act & assert
        float.ParseOrDefault("3").Should().Be(3f);
        float.ParseOrDefault("-1").Should().Be(-1f);
        float.ParseOrDefault("abc").Should().Be(0.0f);
        float.ParseOrDefault("abc", -1.0f).Should().Be(-1.0f);
        float.ParseOrDefault(null).Should().Be(0.0f);
    }
}
