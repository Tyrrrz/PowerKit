using System.IO;
using FluentAssertions;
using PowerKit.Extensions;

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
    public async Task CopyToAsync_NoAutoFlush_Test()
    {
        // Arrange
        var data = new byte[] { 10, 20, 30 };
        using var source = new MemoryStream(data);
        using var destination = new MemoryStream();

        // Act
        await source.CopyToAsync(destination, autoFlush: false);

        // Assert
        destination.ToArray().Should().Equal(data);
    }

    [Fact]
    public async Task CopyToAsync_Empty_Test()
    {
        // Arrange
        using var source = new MemoryStream();
        using var destination = new MemoryStream();

        // Act
        await source.CopyToAsync(destination, autoFlush: false);

        // Assert
        destination.ToArray().Should().BeEmpty();
    }

    [Fact]
    public async Task CopyToAsync_Progress_Test()
    {
        // Arrange
        var data = new byte[1024];
        using var source = new MemoryStream(data);
        using var destination = new MemoryStream();

        // Act
        await source.CopyToAsync(destination, progress: null);

        // Assert
        destination.ToArray().Should().Equal(data);
    }

    [Fact]
    public async Task CopyToAsync_Progress_Reports_Test()
    {
        // Arrange
        var data = new byte[1024];
        using var source = new MemoryStream(data);
        using var destination = new MemoryStream();

        var reports = new List<double>();
        var progress = new Progress<double>(v => reports.Add(v));

        // Act
        await source.CopyToAsync(destination, progress: progress);

        // Allow Progress<T> callbacks to fire on the thread pool
        await Task.Delay(50);

        // Assert
        reports.Should().NotBeEmpty();
        reports.Should().AllSatisfy(v => v.Should().BeInRange(0.0, 1.0));
        reports[^1].Should().BeApproximately(1.0, precision: 1e-5);
    }

    [Fact]
    public async Task CopyToAsync_ContentLength_Test()
    {
        // Arrange
        var data = new byte[1024];
        using var source = new MemoryStream(data);
        using var destination = new MemoryStream();

        // Act
        await source.CopyToAsync(destination, contentLength: 1024);

        // Assert
        destination.ToArray().Should().Equal(data);
    }

    [Fact]
    public async Task CopyToAsync_ContentLength_Progress_Test()
    {
        // Arrange
        var data = new byte[1024];
        using var source = new MemoryStream(data);
        using var destination = new MemoryStream();

        var reports = new List<double>();
        var progress = new Progress<double>(v => reports.Add(v));

        // Act
        await source.CopyToAsync(destination, contentLength: 1024, progress: progress);

        // Allow Progress<T> callbacks to fire on the thread pool
        await Task.Delay(50);

        // Assert
        reports.Should().NotBeEmpty();
        reports.Should().AllSatisfy(v => v.Should().BeInRange(0.0, 1.0));
        reports[^1].Should().BeApproximately(1.0, precision: 1e-5);
    }
}
