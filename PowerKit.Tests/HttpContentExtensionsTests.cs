using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Gress;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class HttpContentExtensionsTests
{
    [Fact]
    public async Task CopyToStreamAsync_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var content = new ByteArrayContent(data);
        using var destination = new MemoryStream();

        // Act
        await content.CopyToStreamAsync(destination);

        // Assert
        destination.ToArray().Should().Equal(data);
    }

    [Fact]
    public async Task CopyToStreamAsync_Progress_Test()
    {
        // Arrange
        var data = new byte[1024];
        using var content = new ByteArrayContent(data);
        using var destination = new MemoryStream();

        var progress = new ProgressCollector<double>();

        // Act
        await content.CopyToStreamAsync(destination, progress);

        // Assert
        var reports = progress.GetValues().ToArray();
        reports.Should().NotBeEmpty();
        reports.Should().AllSatisfy(v => v.Should().BeInRange(0.0, 1.0));
        reports[^1].Should().BeApproximately(1.0, 1e-5);
    }
}
