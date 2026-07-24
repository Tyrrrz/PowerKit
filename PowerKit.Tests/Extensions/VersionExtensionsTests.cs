using System;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class VersionExtensionsTests
{
    [Fact]
    public void ToSemanticString_Test()
    {
        new Version(1, 2).ToSemanticString().Should().Be("1.2.0");
        new Version(1, 2, 3).ToSemanticString().Should().Be("1.2.3");
        new Version(1, 2, 3, 4).ToSemanticString().Should().Be("1.2.3.4");
        new Version(1, 2, 3, 0).ToSemanticString().Should().Be("1.2.3");
    }
}
