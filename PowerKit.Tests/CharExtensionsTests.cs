using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class CharExtensionsTests
{
    [Fact]
    public void Repeat_Test()
    {
        // Act & assert
        'a'.Repeat(3).Should().Be("aaa");
        'x'.Repeat(1).Should().Be("x");
        'z'.Repeat(0).Should().Be("");
    }

    [Fact]
    public void AsString_Test()
    {
        // Act & assert
        'a'.AsString().Should().Be("a");
        ' '.AsString().Should().Be(" ");
    }
}
