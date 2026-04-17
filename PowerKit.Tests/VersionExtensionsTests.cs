using System;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class VersionExtensionsTests
{
    [Fact]
    public void ToSemanticString_WithoutRevision_Test()
    {
        new Version(1, 2, 3).ToSemanticString().Should().Be("1.2.3");
    }

    [Fact]
    public void ToSemanticString_WithRevision_Test()
    {
        new Version(1, 2, 3, 4).ToSemanticString().Should().Be("1.2.3.4");
    }

    [Fact]
    public void ToSemanticString_WithZeroRevision_Test()
    {
        new Version(1, 2, 3, 0).ToSemanticString().Should().Be("1.2.3");
    }
}
