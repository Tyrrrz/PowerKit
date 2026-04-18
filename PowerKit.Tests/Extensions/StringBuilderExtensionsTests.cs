using System.Text;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class StringBuilderExtensionsTests
{
    [Fact]
    public void AppendIfNotEmpty_Test()
    {
        // Act & assert
        new StringBuilder()
            .AppendIfNotEmpty(',')
            .ToString()
            .Should()
            .Be("");
        new StringBuilder("hello").AppendIfNotEmpty(',').ToString().Should().Be("hello,");
        new StringBuilder("a").AppendIfNotEmpty(',').Append("b").ToString().Should().Be("a,b");
    }

    [Fact]
    public void Trim_Test()
    {
        // Act & assert
        new StringBuilder("  hello  ")
            .Trim()
            .ToString()
            .Should()
            .Be("hello");
        new StringBuilder("  hello").Trim().ToString().Should().Be("hello");
        new StringBuilder("hello  ").Trim().ToString().Should().Be("hello");
        new StringBuilder("hello").Trim().ToString().Should().Be("hello");
        new StringBuilder().Trim().ToString().Should().Be("");
        new StringBuilder("   ").Trim().ToString().Should().Be("");
        new StringBuilder("  hello  ").Trim().Append("!").ToString().Should().Be("hello!");
    }
}
