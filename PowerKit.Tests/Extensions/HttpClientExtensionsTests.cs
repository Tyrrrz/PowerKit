using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AwesomeAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class HttpClientExtensionsTests
{
    [Fact]
    public async Task GetJsonAsync_Test()
    {
        // Arrange
        using var http = new HttpClient();

        // Act
        var json = await http.GetJsonAsync("https://jsonplaceholder.typicode.com/todos/1");

        // Assert
        json.GetProperty("userId").GetInt32().Should().Be(1);
        json.GetProperty("id").GetInt32().Should().Be(1);
        json.GetProperty("title").GetString().Should().Be("delectus aut autem");
        json.GetProperty("completed").GetBoolean().Should().BeFalse();
    }

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
