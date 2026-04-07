using PowerKit.Extensions;

namespace PowerKit.Tests;

public class StringExtensionsTests
{
    [Fact]
    public void NullIfWhiteSpace_NonWhitespaceString_ReturnsSame()
    {
        Assert.Equal("hello", "hello".NullIfWhiteSpace());
    }

    [Fact]
    public void NullIfWhiteSpace_WhitespaceOnly_ReturnsNull()
    {
        Assert.Null("   ".NullIfWhiteSpace());
    }

    [Fact]
    public void NullIfWhiteSpace_EmptyString_ReturnsNull()
    {
        Assert.Null("".NullIfWhiteSpace());
    }

    [Fact]
    public void SubstringUntil_SubstringFound_ReturnsBeforeIt()
    {
        Assert.Equal("hello", "hello world".SubstringUntil(" "));
    }

    [Fact]
    public void SubstringUntil_SubstringNotFound_ReturnsFullString()
    {
        Assert.Equal("hello", "hello".SubstringUntil("x"));
    }

    [Fact]
    public void SubstringUntil_SubstringAtStart_ReturnsEmpty()
    {
        Assert.Equal("", "xhello".SubstringUntil("x"));
    }

    [Fact]
    public void SubstringAfter_SubstringFound_ReturnsAfterIt()
    {
        Assert.Equal("world", "hello world".SubstringAfter(" "));
    }

    [Fact]
    public void SubstringAfter_SubstringNotFound_ReturnsEmpty()
    {
        Assert.Equal("", "hello".SubstringAfter("x"));
    }

    [Fact]
    public void SubstringAfter_SubstringAtEnd_ReturnsEmpty()
    {
        Assert.Equal("", "hellox".SubstringAfter("x"));
    }

    [Fact]
    public void Truncate_StringShorterThanLimit_ReturnsFull()
    {
        Assert.Equal("hi", "hi".Truncate(10));
    }

    [Fact]
    public void Truncate_StringExactlyAtLimit_ReturnsFull()
    {
        Assert.Equal("hello", "hello".Truncate(5));
    }

    [Fact]
    public void Truncate_StringLongerThanLimit_ReturnsTruncated()
    {
        Assert.Equal("hel", "hello".Truncate(3));
    }

    [Fact]
    public void ToSpaceSeparatedWords_PascalCase_InsertsSpacesBeforeUppercase()
    {
        Assert.Equal("Hello World", "HelloWorld".ToSpaceSeparatedWords());
    }

    [Fact]
    public void ToSpaceSeparatedWords_SingleWord_ReturnsUnchanged()
    {
        Assert.Equal("Hello", "Hello".ToSpaceSeparatedWords());
    }

    [Fact]
    public void ToSpaceSeparatedWords_AllLowercase_ReturnsUnchanged()
    {
        Assert.Equal("hello", "hello".ToSpaceSeparatedWords());
    }

    [Fact]
    public void ToSpaceSeparatedWords_EmptyString_ReturnsEmpty()
    {
        Assert.Equal("", "".ToSpaceSeparatedWords());
    }

    [Fact]
    public void ToSpaceSeparatedWords_MultipleWords_SplitsCorrectly()
    {
        Assert.Equal("Foo Bar Baz", "FooBarBaz".ToSpaceSeparatedWords());
    }
}
