using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class HttpClientExtensionsTests
{
    private sealed class FakeHandler(
        HttpStatusCode statusCode = HttpStatusCode.OK,
        string? body = null,
        HttpMethod? expectedMethod = null
    ) : HttpMessageHandler
    {
        public HttpRequestMessage? LastRequest { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken
        )
        {
            LastRequest = request;

            if (expectedMethod is not null)
                request.Method.Should().Be(expectedMethod);

            var response = new HttpResponseMessage(statusCode);
            if (body is not null)
                response.Content = new StringContent(body);

            return Task.FromResult(response);
        }
    }

    [Fact]
    public async Task DownloadAsync_WritesContentToFile()
    {
        // Arrange
        var handler = new FakeHandler(body: "file-content");
        using var http = new HttpClient(handler);
        var filePath = Path.GetTempFileName();

        try
        {
            // Act
            await http.DownloadAsync("https://example.com/file", filePath);

            // Assert
            File.ReadAllText(filePath).Should().Be("file-content");
        }
        finally
        {
            File.Delete(filePath);
        }
    }

    [Fact]
    public async Task DownloadAsync_ThrowsOnNonSuccessStatus()
    {
        // Arrange
        var handler = new FakeHandler(HttpStatusCode.NotFound);
        using var http = new HttpClient(handler);
        var filePath = Path.GetTempFileName();

        try
        {
            // Act & Assert
            await http.Invoking(h => h.DownloadAsync("https://example.com/missing", filePath))
                .Should()
                .ThrowAsync<HttpRequestException>();
        }
        finally
        {
            File.Delete(filePath);
        }
    }

    [Fact]
    public async Task HeadAsync_SendsHeadRequest()
    {
        // Arrange
        var handler = new FakeHandler(expectedMethod: HttpMethod.Head);
        using var http = new HttpClient(handler);

        // Act
        using var response = await http.HeadAsync("https://example.com");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        handler.LastRequest!.Method.Should().Be(HttpMethod.Head);
        handler.LastRequest.RequestUri!.ToString().Should().Be("https://example.com/");
    }
}
