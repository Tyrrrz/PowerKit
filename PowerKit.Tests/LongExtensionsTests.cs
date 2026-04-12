using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class LongExtensionsTests
{
    [Fact]
    public void ParseOrNull_Test()
    {
        // Act & assert
        long.ParseOrNull("9876543210").Should().Be(9876543210L);
        long.ParseOrNull("-1").Should().Be(-1L);
        long.ParseOrNull("abc").Should().BeNull();
        long.ParseOrNull(null).Should().BeNull();
    }
}
