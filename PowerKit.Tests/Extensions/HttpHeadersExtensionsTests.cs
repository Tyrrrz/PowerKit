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
        request.Headers.TryGetValue("X-Missing").Should().BeNull();

        // Multiple values are concatenated
        request.Headers.Add("X-Multi", "foo");
        request.Headers.Add("X-Multi", "bar");
        request.Headers.TryGetValue("X-Multi").Should().Be("foobar");
    }
}
