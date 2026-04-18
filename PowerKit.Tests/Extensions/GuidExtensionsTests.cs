using System;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class GuidExtensionsTests
{
    [Fact]
    public void ParseOrNull_Test()
    {
        // Act & assert
        Guid.ParseOrNull("12345678-1234-1234-1234-123456789abc")
            .Should()
            .Be(new Guid("12345678-1234-1234-1234-123456789abc"));
        Guid.ParseOrNull("not-a-guid").Should().BeNull();
        Guid.ParseOrNull(null).Should().BeNull();
    }

    [Fact]
    public void ParseOrDefault_Test()
    {
        // Act & assert
        Guid.ParseOrDefault("12345678-1234-1234-1234-123456789abc")
            .Should()
            .Be(new Guid("12345678-1234-1234-1234-123456789abc"));
        Guid.ParseOrDefault("not-a-guid").Should().Be(Guid.Empty);
        Guid.ParseOrDefault("not-a-guid", new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"))
            .Should()
            .Be(new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"));
        Guid.ParseOrDefault(null).Should().Be(Guid.Empty);
    }
}
