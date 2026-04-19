using System.Net.Http;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class HttpHeadersExtensionsTests
{
    [Fact]
    public void TryGetValue_Test()
    {
        using var request = new HttpRequestMessage();

        // Single value
        request.Headers.Add("X-Custom", "value");
        request.Headers.TryGetValue("X-Custom").Should().Be("value");

        // Missing header
        request.Headers.TryGetValue("X-Missing").Should().BeEmpty();

        // Multiple values are joined with ", "
        request.Headers.Add("X-Multi", "foo");
        request.Headers.Add("X-Multi", "bar");
        request.Headers.TryGetValue("X-Multi").Should().Be("foo, bar");
    }

    [Fact]
    public void GetValuesOrEmpty_Test()
    {
        using var request = new HttpRequestMessage();

        // Single value
        request.Headers.Add("X-Custom", "value");
        request.Headers.GetValuesOrEmpty("X-Custom").Should().Equal("value");

        // Missing header returns empty sequence
        request.Headers.GetValuesOrEmpty("X-Missing").Should().BeEmpty();

        // Multiple values are returned individually
        request.Headers.Add("X-Multi", "foo");
        request.Headers.Add("X-Multi", "bar");
        request.Headers.GetValuesOrEmpty("X-Multi").Should().Equal("foo", "bar");
    }
}
