using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using PowerKit;
using Xunit;

namespace PowerKit.Tests;

file class FakeClientDelegatingHandler(HttpStatusCode statusCode = HttpStatusCode.OK)
    : HttpMessageHandler
{
    public HttpRequestMessage? LastRequest { get; private set; }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        LastRequest = request;
        return Task.FromResult(new HttpResponseMessage(statusCode));
    }
}

file class PassthroughClientDelegatingHandler(HttpClient http, bool disposeClient = false)
    : ClientDelegatingHandler(http, disposeClient);

public class ClientDelegatingHandlerTests
{
    [Fact]
    public async Task SendAsync_Test()
    {
        // Arrange
        var inner = new FakeClientDelegatingHandler();
        using var innerClient = new HttpClient(inner);
        using var handler = new PassthroughClientDelegatingHandler(innerClient);
        using var http = new HttpClient(handler);

        // Act
        using var response = await http.GetAsync("https://example.com");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        inner.LastRequest!.RequestUri!.ToString().Should().Be("https://example.com/");
    }
}
