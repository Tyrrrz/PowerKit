#nullable enable
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
        double.ParseOrNull("3.14").Should().Be(3.14);
        double.ParseOrNull("-1.5").Should().Be(-1.5);
        double.ParseOrNull("abc").Should().BeNull();
        double.ParseOrNull(null).Should().BeNull();
    }
}
