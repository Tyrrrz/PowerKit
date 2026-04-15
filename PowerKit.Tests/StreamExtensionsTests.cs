using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Gress;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class StreamExtensionsTests
{
    [Fact]
    public async Task CopyToAsync_AutoFlush_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var source = new MemoryStream(data);
        using var destination = new MemoryStream();

        // Act
        await source.CopyToAsync(destination, autoFlush: true);

        // Assert
        destination.ToArray().Should().Equal(data);
    }

    [Fact]
    public async Task CopyToAsync_Progress_Test()
    {
        // Arrange
        var data = new byte[1024];
        using var source = new MemoryStream(data);
        using var destination = new MemoryStream();

        var progress = new ProgressCollector<double>();

        // Act
        await source.CopyToAsync(destination, progress: progress);

        // Assert
        var reports = progress.GetValues().ToArray();
        reports.Should().NotBeEmpty();
        reports.Should().AllSatisfy(v => v.Should().BeInRange(0.0, 1.0));
        reports[^1].Should().BeApproximately(1.0, precision: 1e-5);
    }
}
