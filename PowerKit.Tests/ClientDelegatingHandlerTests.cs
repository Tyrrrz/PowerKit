using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using PowerKit;
using Xunit;

namespace PowerKit.Tests;

public class ClientDelegatingHandlerTests
{
    private sealed class FakeInnerHandler(HttpStatusCode statusCode = HttpStatusCode.OK)
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

    private sealed class PassthroughHandler(HttpClient http, bool disposeClient = false)
        : ClientDelegatingHandler(http, disposeClient);

    [Fact]
    public async Task SendAsync_DelegatesRequestToInnerClient()
    {
        // Arrange
        var inner = new FakeInnerHandler();
        using var innerClient = new HttpClient(inner);
        using var handler = new PassthroughHandler(innerClient);
        using var http = new HttpClient(handler);

        // Act
        using var response = await http.GetAsync("https://example.com");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        inner.LastRequest!.RequestUri!.ToString().Should().Be("https://example.com/");
    }

    [Fact]
    public async Task SendAsync_ClonesRequest()
    {
        // Arrange
        var inner = new FakeInnerHandler();
        using var innerClient = new HttpClient(inner);
        using var handler = new PassthroughHandler(innerClient);
        using var http = new HttpClient(handler);

        // Act — send two requests to the same URI to verify clone reuse works
        using var response1 = await http.GetAsync("https://example.com");
        using var response2 = await http.GetAsync("https://example.com");

        // Assert — both succeed, confirming the clone avoids request-reuse errors
        response1.StatusCode.Should().Be(HttpStatusCode.OK);
        response2.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public void Dispose_DisposesInnerClientWhenRequested()
    {
        // Arrange
        var inner = new FakeInnerHandler();
        var innerClient = new HttpClient(inner);
        var handler = new PassthroughHandler(innerClient, disposeClient: true);

        // Act
        handler.Dispose();

        // Assert — sending on the disposed client throws ObjectDisposedException
        var act = () => innerClient.GetAsync("https://example.com");
        act.Should().ThrowAsync<ObjectDisposedException>();
    }

    [Fact]
    public void Dispose_DoesNotDisposeInnerClientByDefault()
    {
        // Arrange
        var inner = new FakeInnerHandler();
        using var innerClient = new HttpClient(inner);
        var handler = new PassthroughHandler(innerClient);

        // Act
        handler.Dispose();

        // Assert — inner client still usable
        var act = () => innerClient.GetAsync("https://example.com");
        act.Should().NotThrowAsync();
    }
}
