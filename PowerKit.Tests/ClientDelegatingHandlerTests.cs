using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using PowerKit;
using Xunit;

namespace PowerKit.Tests;

file class PassthroughClientDelegatingHandler(HttpClient http, bool disposeClient = false)
    : ClientDelegatingHandler(http, disposeClient);

public class ClientDelegatingHandlerTests
{
    [Fact]
    public async Task SendAsync_Test()
    {
        // Arrange
        using var innerClient = new HttpClient();
        using var handler = new PassthroughClientDelegatingHandler(innerClient);
        using var http = new HttpClient(handler);

        // Act
        using var response = await http.GetAsync("https://example.com");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
