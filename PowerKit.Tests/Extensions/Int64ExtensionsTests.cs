using System.Globalization;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class Int64ExtensionsTests
{
    [Fact]
    public void ParseOrNull_Test()
    {
        // Act & assert
        long.ParseOrNull("9876543210", CultureInfo.InvariantCulture).Should().Be(9876543210L);
        long.ParseOrNull("-1", CultureInfo.InvariantCulture).Should().Be(-1L);
        long.ParseOrNull("abc").Should().BeNull();
        long.ParseOrNull(null).Should().BeNull();
    }

    [Fact]
    public void ParseOrDefault_Test()
    {
        // Act & assert
        long.ParseOrDefault("9876543210", CultureInfo.InvariantCulture).Should().Be(9876543210L);
        long.ParseOrDefault("-1", CultureInfo.InvariantCulture).Should().Be(-1L);
        long.ParseOrDefault("abc").Should().Be(0L);
        long.ParseOrDefault("abc", -1L).Should().Be(-1L);
        long.ParseOrDefault(null).Should().Be(0L);
    }
}
