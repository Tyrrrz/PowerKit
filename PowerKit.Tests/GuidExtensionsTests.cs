#nullable enable
using System;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class GuidExtensionsTests
{
    [Fact]
    public void ParseOrNull_Test()
    {
        // Act & assert
        Guid.ParseOrNull("12345678-1234-1234-1234-123456789abc").Should().Be(
            new Guid("12345678-1234-1234-1234-123456789abc")
        );
        Guid.ParseOrNull("not-a-guid").Should().BeNull();
        Guid.ParseOrNull(null).Should().BeNull();
    }
}
