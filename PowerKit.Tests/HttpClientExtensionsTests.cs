using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using PowerKit;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class HttpClientExtensionsTests
{
    [Fact]
    public async Task DownloadAsync_Test()
    {
        // Arrange
        using var http = new HttpClient();
        using var tempFile = TempFile.Create(false);

        // Act
        await http.DownloadAsync("https://example.com", tempFile.Path);

        // Assert
        new FileInfo(tempFile.Path)
            .Length.Should()
            .BeGreaterThan(0);
    }

    [Fact]
    public async Task HeadAsync_Test()
    {
        // Arrange
        using var http = new HttpClient();

        // Act
        using var response = await http.HeadAsync("https://example.com");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
