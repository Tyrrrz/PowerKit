#nullable enable
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class DecimalExtensionsTests
{
    [Fact]
    public void ParseOrNull_Test()
    {
        // Act & assert
        decimal.ParseOrNull("3.14").Should().Be(3.14m);
        decimal.ParseOrNull("-1.5").Should().Be(-1.5m);
        decimal.ParseOrNull("abc").Should().BeNull();
        decimal.ParseOrNull(null).Should().BeNull();
    }
}
