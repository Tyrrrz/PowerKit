using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class StringExtensionsTests
{
    [Fact]
    public void SubstringUntil_Test()
    {
        // Act & assert
        "hello world".SubstringUntil(" ").Should().Be("hello");
        "hello".SubstringUntil("x").Should().Be("hello");
        "xhello".SubstringUntil("x").Should().Be("");
    }

    [Fact]
    public void SubstringAfter_Test()
    {
        // Act & assert
        "hello world".SubstringAfter(" ").Should().Be("world");
        "hello".SubstringAfter("x").Should().Be("");
        "hellox".SubstringAfter("x").Should().Be("");
    }

    [Fact]
    public void Truncate_Test()
    {
        // Act & assert
        "hi".Truncate(10).Should().Be("hi");
        "hello".Truncate(5).Should().Be("hello");
        "hello".Truncate(3).Should().Be("hel");
    }

    [Fact]
    public void SeparateWords_Test()
    {
        // Act & assert
        "HelloWorld".SeparateWords(' ').Should().Be("Hello World");
        "Hello".SeparateWords(' ').Should().Be("Hello");
        "hello".SeparateWords(' ').Should().Be("hello");
        "".SeparateWords(' ').Should().Be("");
        "FooBarBaz".SeparateWords(' ').Should().Be("Foo Bar Baz");
        "FooBarBaz".SeparateWords('-').Should().Be("Foo-Bar-Baz");
    }
}
