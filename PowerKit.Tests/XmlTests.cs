using FluentAssertions;
using Xunit;

namespace PowerKit.Tests;

public class XmlTests
{
    [Fact]
    public void Escape_NoSpecialChars_Test()
    {
        // Act
        var result = Xml.Escape("hello world");

        // Assert
        result.Should().Be("hello world");
    }

    [Fact]
    public void Escape_Ampersand_Test()
    {
        // Act
        var result = Xml.Escape("foo & bar");

        // Assert
        result.Should().Be("foo &amp; bar");
    }

    [Fact]
    public void Escape_LessThan_Test()
    {
        // Act
        var result = Xml.Escape("1 < 2");

        // Assert
        result.Should().Be("1 &lt; 2");
    }

    [Fact]
    public void Escape_GreaterThan_Test()
    {
        // Act
        var result = Xml.Escape("2 > 1");

        // Assert
        result.Should().Be("2 &gt; 1");
    }

    [Fact]
    public void Escape_DoubleQuote_Test()
    {
        // Act
        var result = Xml.Escape("say \"hello\"");

        // Assert
        result.Should().Be("say &quot;hello&quot;");
    }

    [Fact]
    public void Escape_SingleQuote_Test()
    {
        // Act
        var result = Xml.Escape("it's");

        // Assert
        result.Should().Be("it&apos;s");
    }

    [Fact]
    public void Escape_AllSpecialChars_Test()
    {
        // Act
        var result = Xml.Escape("& < > \" '");

        // Assert
        result.Should().Be("&amp; &lt; &gt; &quot; &apos;");
    }

    [Fact]
    public void Escape_InvalidControlChar_Test()
    {
        // Arrange
        // U+0001 is invalid in XML 1.0 and should be removed
        var result = Xml.Escape("foo\u0001bar");

        // Assert
        result.Should().Be("foobar");
    }

    [Fact]
    public void Escape_ValidWhitespace_Test()
    {
        // Tab, newline, and carriage return are valid XML characters
        var result = Xml.Escape("foo\t\n\rbar");

        // Assert
        result.Should().Be("foo\t\n\rbar");
    }

    [Fact]
    public void Escape_SurrogatePair_Test()
    {
        // Supplementary character U+1F600 (😀), encoded as a surrogate pair in .NET
        var input = "\U0001F600";

        // Act
        var result = Xml.Escape(input);

        // Assert
        result.Should().Be(input);
    }

    [Fact]
    public void Escape_EmptyString_Test()
    {
        // Act
        var result = Xml.Escape("");

        // Assert
        result.Should().BeEmpty();
    }
}
