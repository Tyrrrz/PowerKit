using System.Net.Http;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class HttpHeadersExtensionsTests
{
    [Fact]
    public void TryGetValue_Found_Test()
    {
        // Arrange
        using var request = new HttpRequestMessage();
        request.Headers.Add("X-Custom", "value");

        // Act
        var result = request.Headers.TryGetValue("X-Custom");

        // Assert
        result.Should().Be("value");
    }

    [Fact]
    public void TryGetValue_NotFound_Test()
    {
        // Arrange
        using var request = new HttpRequestMessage();

        // Act
        var result = request.Headers.TryGetValue("X-Custom");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void TryGetValue_MultipleValues_Test()
    {
        // Arrange
        using var request = new HttpRequestMessage();
        request.Headers.Add("X-Custom", "foo");
        request.Headers.Add("X-Custom", "bar");

        // Act
        var result = request.Headers.TryGetValue("X-Custom");

        // Assert
        result.Should().Be("foobar");
    }
}
