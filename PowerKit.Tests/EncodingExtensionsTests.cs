using System.Text;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class EncodingExtensionsTests
{
    [Fact]
    public void Utf8WithoutBom_IsUtf8()
    {
        Encoding.Utf8WithoutBom.WebName.Should().Be("utf-8");
    }

    [Fact]
    public void Utf8WithoutBom_HasNoBom()
    {
        Encoding.Utf8WithoutBom.GetPreamble().Should().BeEmpty();
    }

    [Fact]
    public void Utf8WithoutBom_IsDifferentFromUtf8()
    {
        Encoding.Utf8WithoutBom.Should().NotBeSameAs(Encoding.UTF8);
    }
}
