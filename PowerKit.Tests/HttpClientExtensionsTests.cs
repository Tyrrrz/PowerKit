using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
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
        var filePath = Path.GetTempFileName();

        try
        {
            // Act
            await http.DownloadAsync("https://example.com", filePath);

            // Assert
            new FileInfo(filePath)
                .Length.Should()
                .BeGreaterThan(0);
        }
        finally
        {
            File.Delete(filePath);
        }
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
