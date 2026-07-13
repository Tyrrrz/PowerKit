using FluentAssertions;
using Xunit;

namespace PowerKit.Tests;

public class XmlTests
{
    [Fact]
    public void Escape_Test()
    {
        // Act & assert
        Xml.Escape("hello world").Should().Be("hello world");
        Xml.Escape("foo & bar").Should().Be("foo &amp; bar");
        Xml.Escape("1 < 2").Should().Be("1 &lt; 2");
        Xml.Escape("2 > 1").Should().Be("2 &gt; 1");
        Xml.Escape("say \"hello\"").Should().Be("say &quot;hello&quot;");
        Xml.Escape("it's").Should().Be("it&apos;s");
        Xml.Escape("& < > \" '").Should().Be("&amp; &lt; &gt; &quot; &apos;");
        Xml.Escape("foo\u0001bar").Should().Be("foobar");
        Xml.Escape("foo\t\n\rbar").Should().Be("foo\t\n\rbar");
        Xml.Escape("\U0001F600").Should().Be("\U0001F600");
        Xml.Escape("foo\uD800bar").Should().Be("foobar");
        Xml.Escape("foo\uDC00bar").Should().Be("foobar");
        Xml.Escape("").Should().BeEmpty();
    }
}
