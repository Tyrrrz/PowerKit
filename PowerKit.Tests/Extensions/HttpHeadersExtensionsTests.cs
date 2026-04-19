using System.Net.Http;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class HttpHeadersExtensionsTests
{
    [Fact]
    public void GetValuesOrEmpty_Test()
    {
        // Arrange
        using var request = new HttpRequestMessage();
        request.Headers.Add("X-Custom", "value");
        request.Headers.Add("X-Multi", "foo");
        request.Headers.Add("X-Multi", "bar");

        // Act & assert
        request.Headers.GetValuesOrEmpty("X-Missing").Should().BeEmpty();
        request.Headers.GetValuesOrEmpty("X-Custom").Should().Equal("value");
        request.Headers.GetValuesOrEmpty("X-Multi").Should().Equal("foo", "bar");
    }

    [Fact]
    public void TryGetValue_Test()
    {
        // Arrange
        using var request = new HttpRequestMessage();
        request.Headers.Add("X-Custom", "value");
        request.Headers.Add("X-Multi", "foo");
        request.Headers.Add("X-Multi", "bar");

        // Act & assert
        request.Headers.TryGetValue("X-Missing").Should().BeEmpty();
        request.Headers.TryGetValue("X-Custom").Should().Be("value");
        request.Headers.TryGetValue("X-Multi").Should().Be("foo, bar");
    }
}
