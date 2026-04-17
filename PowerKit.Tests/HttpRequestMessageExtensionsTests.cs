using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class HttpRequestMessageExtensionsTests
{
    [Fact]
    public void Clone_Test()
    {
        // Arrange
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://example.com/api");

        // Act
        using var clone = request.Clone();

        // Assert
        clone.Method.Should().Be(HttpMethod.Post);
        clone.RequestUri.Should().Be(request.RequestUri);
    }

    [Fact]
    public void Clone_WithHeaders_Test()
    {
        // Arrange
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://example.com");
        request.Headers.Add("X-Custom-Header", "test-value");

        // Act
        using var clone = request.Clone();

        // Assert
        clone.Headers.GetValues("X-Custom-Header").Should().Equal("test-value");
    }

    [Fact]
    public void Clone_WithoutContent_Test()
    {
        // Arrange
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://example.com");

        // Act
        using var clone = request.Clone();

        // Assert
        clone.Content.Should().BeNull();
    }

    [Fact]
    public async Task Clone_WithContent_Test()
    {
        // Arrange
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://example.com");
        request.Content = new StringContent("hello");

        // Act
        using var clone = request.Clone();

        // Assert
        var body = await clone.Content!.ReadAsStringAsync();
        body.Should().Be("hello");
    }

    [Fact]
    public async Task Clone_WithContentHeaders_Test()
    {
        // Arrange
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://example.com");
        request.Content = new StringContent("hello");
        request.Content.Headers.Add("X-Content-Header", "value");

        // Act
        using var clone = request.Clone();

        // Assert
        clone.Content!.Headers.GetValues("X-Content-Header").Should().Equal("value");
        var body = await clone.Content!.ReadAsStringAsync();
        body.Should().Be("hello");
    }

    [Fact]
    public async Task Clone_DoesNotDisposeContent_Test()
    {
        // Arrange
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://example.com");
        request.Content = new StringContent("original");

        // Act
        using var clone = request.Clone();
        clone.Dispose();

        // Assert — original content still readable
        var body = await request.Content!.ReadAsStringAsync();
        body.Should().Be("original");
    }
}
