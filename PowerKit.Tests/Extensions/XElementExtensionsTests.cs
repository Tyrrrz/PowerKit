using System.Xml.Linq;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class XElementExtensionsTests
{
    [Fact]
    public void StripNamespaces_Test()
    {
        // Arrange
        var element = XElement.Parse(
            """
            <root xmlns="http://example.com" xmlns:x="http://x.com">
                <x:child x:foo="bar" />
                <otherChild foo="baz" />
            </root>
            """
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
}
