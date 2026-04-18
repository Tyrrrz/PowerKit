using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class HttpRequestMessageExtensionsTests
{
    [Fact]
    public void Clone_Test()
    {
        // Arrange
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://example.com");
        request.Headers.Add("X-Custom", "value");

        // Act
        using var clone = request.Clone();

        // Assert
        clone.Should().NotBeSameAs(request);
        clone.Method.Should().Be(HttpMethod.Post);
        clone.RequestUri.Should().Be(request.RequestUri);
        clone.Headers.GetValues("X-Custom").Should().Equal("value");
        clone.Content.Should().BeNull();
    }

    [Fact]
    public async Task Clone_WithContent_Test()
    {
        // Arrange
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://example.com");
        request.Content = new StringContent("hello");
        request.Content.Headers.Add("X-Content-Header", "value");

        // Act
        using var clone = request.Clone();

        // Assert — content and content headers are copied
        clone.Content!.Headers.GetValues("X-Content-Header").Should().Equal("value");
        var body = await clone.Content!.ReadAsStringAsync();
        body.Should().Be("hello");

        // Assert — disposing the clone doesn't dispose original content
        clone.Dispose();
        var originalBody = await request.Content!.ReadAsStringAsync();
        originalBody.Should().Be("hello");
    }
}
