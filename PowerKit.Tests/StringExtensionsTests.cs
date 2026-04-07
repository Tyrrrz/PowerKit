using FluentAssertions;
using PowerKit.Extensions;

namespace PowerKit.Tests;

public class StringExtensionsTests
{
    [Fact]
    public void NullIfWhiteSpace_Test()
    {
        // Act & assert
        "hello".NullIfWhiteSpace().Should().Be("hello");
    }

    [Fact]
    public void NullIfWhiteSpace_Whitespace_Test()
    {
        // Act & assert
        "   ".NullIfWhiteSpace().Should().BeNull();
    }

    [Fact]
    public void NullIfWhiteSpace_Empty_Test()
    {
        // Act & assert
        "".NullIfWhiteSpace().Should().BeNull();
    }

    [Fact]
    public void SubstringUntil_Test()
    {
        // Act & assert
        "hello world".SubstringUntil(" ").Should().Be("hello");
    }

    [Fact]
    public void SubstringUntil_NotFound_Test()
    {
        // Act & assert
        "hello".SubstringUntil("x").Should().Be("hello");
    }

    [Fact]
    public void SubstringUntil_AtStart_Test()
    {
        // Act & assert
        "xhello".SubstringUntil("x").Should().Be("");
    }

    [Fact]
    public void SubstringAfter_Test()
    {
        // Act & assert
        "hello world".SubstringAfter(" ").Should().Be("world");
    }

    [Fact]
    public void SubstringAfter_NotFound_Test()
    {
        // Act & assert
        "hello".SubstringAfter("x").Should().Be("");
    }

    [Fact]
    public void SubstringAfter_AtEnd_Test()
    {
        // Act & assert
        "hellox".SubstringAfter("x").Should().Be("");
    }

    [Fact]
    public void Truncate_ShorterThanLimit_Test()
    {
        // Act & assert
        "hi".Truncate(10).Should().Be("hi");
    }

    [Fact]
    public void Truncate_ExactlyAtLimit_Test()
    {
        // Act & assert
        "hello".Truncate(5).Should().Be("hello");
    }

    [Fact]
    public void Truncate_LongerThanLimit_Test()
    {
        // Act & assert
        "hello".Truncate(3).Should().Be("hel");
    }

    [Fact]
    public void ToSpaceSeparatedWords_Test()
    {
        // Act & assert
        "HelloWorld".ToSpaceSeparatedWords().Should().Be("Hello World");
    }

    [Fact]
    public void ToSpaceSeparatedWords_SingleWord_Test()
    {
        // Act & assert
        "Hello".ToSpaceSeparatedWords().Should().Be("Hello");
    }

    [Fact]
    public void ToSpaceSeparatedWords_AllLowercase_Test()
    {
        // Act & assert
        "hello".ToSpaceSeparatedWords().Should().Be("hello");
    }

    [Fact]
    public void ToSpaceSeparatedWords_Empty_Test()
    {
        // Act & assert
        "".ToSpaceSeparatedWords().Should().Be("");
    }

    [Fact]
    public void ToSpaceSeparatedWords_Multiple_Test()
    {
        // Act & assert
        "FooBarBaz".ToSpaceSeparatedWords().Should().Be("Foo Bar Baz");
    }
}
