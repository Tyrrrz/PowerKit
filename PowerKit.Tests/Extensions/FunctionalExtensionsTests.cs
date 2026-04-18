using System;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class FunctionalExtensionsTests
{
    [Fact]
    public void Pipe_Test()
    {
        // Act & assert
        5.Pipe(x => x * 2).Should().Be(10);
        "hello".Pipe(s => s.ToUpper()).Pipe(s => s + "!").Should().Be("HELLO!");
    }

    [Fact]
    public void NullIf_ReferenceType_Test()
    {
        // Act & assert
        "hello".NullIf(v => v == "hello").Should().BeNull();
        "world".NullIf(v => v == "hello").Should().Be("world");
    }

    [Fact]
    public void NullIf_Test()
    {
        // Act & assert
        0.NullIf(v => v == 0).Should().BeNull();
        5.NullIf(v => v == 0).Should().Be(5);
    }

    [Fact]
    public void NullIfDefault_Test()
    {
        // Act & assert
        0.NullIfDefault().Should().BeNull();
        42.NullIfDefault().Should().Be(42);
        Guid.Empty.NullIfDefault().Should().BeNull();
        Guid.NewGuid().NullIfDefault().Should().NotBeNull();
    }

    [Fact]
    public void NullIfEmpty_Test()
    {
        // Act & assert
        "hello".NullIfEmpty().Should().Be("hello");
        "".NullIfEmpty().Should().BeNull();
        "   ".NullIfEmpty().Should().Be("   ");
    }

    [Fact]
    public void NullIfWhiteSpace_Test()
    {
        // Act & assert
        "hello".NullIfWhiteSpace().Should().Be("hello");
        "   ".NullIfWhiteSpace().Should().BeNull();
        "".NullIfWhiteSpace().Should().BeNull();
    }
}
