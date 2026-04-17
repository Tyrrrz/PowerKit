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
    public void SubstringUntilLast_Test()
    {
        // Act & assert
        "hello world foo".SubstringUntilLast(" ").Should().Be("hello world");
        "hello".SubstringUntilLast("x").Should().Be("hello");
        "a.b.c".SubstringUntilLast(".").Should().Be("a.b");
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
    public void SubstringAfterLast_Test()
    {
        // Act & assert
        "hello world foo".SubstringAfterLast(" ").Should().Be("foo");
        "hello".SubstringAfterLast("x").Should().Be("");
        "a.b.c".SubstringAfterLast(".").Should().Be("c");
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

    [Fact]
    public void ToKebabCase_Test()
    {
        // Act & assert
        "HelloWorld".ToKebabCase().Should().Be("hello-world");
        "FooBarBaz".ToKebabCase().Should().Be("foo-bar-baz");
        "Hello".ToKebabCase().Should().Be("hello");
        "hello".ToKebabCase().Should().Be("hello");
        "".ToKebabCase().Should().Be("");
    }

    [Fact]
    public void ToSnakeCase_Test()
    {
        // Act & assert
        "HelloWorld".ToSnakeCase().Should().Be("hello_world");
        "FooBarBaz".ToSnakeCase().Should().Be("foo_bar_baz");
        "Hello".ToSnakeCase().Should().Be("hello");
        "hello".ToSnakeCase().Should().Be("hello");
        "".ToSnakeCase().Should().Be("");
    }

    [Fact]
    public void ToSecureString_Test()
    {
        // Act & assert
        "hello".ToSecureString().Length.Should().Be(5);
        "".ToSecureString().Length.Should().Be(0);
        "abc".ToSecureString().Length.Should().Be(3);
    }

    [Fact]
    public void Reverse_Test()
    {
        // Act & assert
        "hello".Reverse().Should().Be("olleh");
        "abcde".Reverse().Should().Be("edcba");
        "a".Reverse().Should().Be("a");
        "".Reverse().Should().Be("");
    }
}
