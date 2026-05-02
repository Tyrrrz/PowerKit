using System;
using System.Text;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class StringExtensionsTests
{
    [Fact]
    public void Replace_Test()
    {
        // Act & assert
        "a1b2c3".Replace(ch => char.IsDigit(ch) ? '*' : ch).Should().Be("a*b*c*");
        "hello".Replace(ch => ch).Should().Be("hello");
        "".Replace(ch => ch).Should().Be("");
    }

    [Fact]
    public void ReplaceWhiteSpace_Test()
    {
        // Act & assert
        "hello world".ReplaceWhiteSpace('_').Should().Be("hello_world");
        "hello\tworld\nfoo".ReplaceWhiteSpace('-').Should().Be("hello-world-foo");
        "hello\u00A0world".ReplaceWhiteSpace(' ').Should().Be("hello world");
        "helloworld".ReplaceWhiteSpace('_').Should().Be("helloworld");
        "".ReplaceWhiteSpace('_').Should().Be("");
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
    public void ToSecureString_Test()
    {
        // Act & assert
        using (var s1 = "hello".ToSecureString())
        {
            s1.Length.Should().Be(5);
            s1.IsReadOnly().Should().BeTrue();
        }

        using (var s2 = "".ToSecureString())
        {
            s2.Length.Should().Be(0);
            s2.IsReadOnly().Should().BeTrue();
        }

        using (var s3 = "abc".ToSecureString())
        {
            s3.Length.Should().Be(3);
            s3.IsReadOnly().Should().BeTrue();
        }
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
    public void Truncate_Test()
    {
        // Act & assert
        "hi".Truncate(10).Should().Be("hi");
        "hello".Truncate(5).Should().Be("hello");
        "hello".Truncate(3).Should().Be("hel");
    }

    [Fact]
    public void TruncateBytes_Test()
    {
        // Act & assert

        // ASCII-only (1 byte per char with UTF-8)
        "hello".TruncateBytes(10, Encoding.UTF8).Should().Be("hello");
        "hello".TruncateBytes(3, Encoding.UTF8).Should().Be("hel");
        "hello".TruncateBytes(0, Encoding.UTF8).Should().Be("");

        // Multi-byte characters (é = 2 bytes in UTF-8)
        "héllo".TruncateBytes(4, Encoding.UTF8).Should().Be("hél");
        "héllo".TruncateBytes(3, Encoding.UTF8).Should().Be("hé");
        "héllo".TruncateBytes(2, Encoding.UTF8).Should().Be("h");
        "héllo".TruncateBytes(1, Encoding.UTF8).Should().Be("h");

        // Supplementary character (𝄞 = 4 bytes in UTF-8, encoded as a surrogate pair in C#)
        "a𝄞b".TruncateBytes(5, Encoding.UTF8).Should().Be("a𝄞");
        "a𝄞b".TruncateBytes(4, Encoding.UTF8).Should().Be("a");
        "a𝄞b".TruncateBytes(1, Encoding.UTF8).Should().Be("a");

        // Default encoding is UTF-8
        "héllo".TruncateBytes(4).Should().Be("hél");

        // Non-UTF-8 encoding
        "hello".TruncateBytes(3, Encoding.ASCII).Should().Be("hel");

        // Negative byte count is invalid
        ((Action)(() => "hello".TruncateBytes(-1)))
            .Should()
            .Throw<ArgumentOutOfRangeException>();
    }
}
