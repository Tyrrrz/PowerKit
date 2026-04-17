using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class XElementExtensionsTests
{
    [Fact]
    public void StripNamespaces_NoNamespaces_Test()
    {
        // Arrange
        var element = XElement.Parse("<root><child foo=\"bar\" /></root>");

        // Act
        var result = element.StripNamespaces();

        // Assert
        result.Name.LocalName.Should().Be("root");
        result.Descendants().Should().ContainSingle(d => d.Name.LocalName == "child");
        result.Descendants().Single().Attribute("foo")!.Value.Should().Be("bar");
    }

    [Fact]
    public void StripNamespaces_WithNamespaces_Test()
    {
        // Arrange
        var element = XElement.Parse(
            "<root xmlns=\"http://example.com\" xmlns:x=\"http://x.com\"><x:child x:foo=\"bar\" /></root>"
        );

        // Act
        var result = element.StripNamespaces();

        // Assert
        result.Name.NamespaceName.Should().BeEmpty();
        result.Attributes().Should().BeEmpty();
        result
            .Descendants()
            .Should()
            .AllSatisfy(d =>
            {
                d.Name.NamespaceName.Should().BeEmpty();
                d.Attributes().Should().AllSatisfy(a => a.Name.NamespaceName.Should().BeEmpty());
            });
    }

    [Fact]
    public void StripNamespaces_DoesNotModifyOriginal_Test()
    {
        // Arrange
        var element = XElement.Parse("<root xmlns=\"http://example.com\"><child /></root>");
        var originalXml = element.ToString();

        // Act
        element.StripNamespaces();

        // Assert
        element.ToString().Should().Be(originalXml);
    }
}
