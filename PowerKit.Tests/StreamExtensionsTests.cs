using System.IO;
using PowerKit.Extensions;

namespace PowerKit.Tests;

public class StreamExtensionsTests
{
    [Fact]
    public async Task CopyToAsync_WithAutoFlush_CopiesAllBytes()
    {
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var source = new MemoryStream(data);
        using var destination = new MemoryStream();

        await source.CopyToAsync(destination, autoFlush: true);

        Assert.Equal(data, destination.ToArray());
    }

    [Fact]
    public async Task CopyToAsync_WithoutAutoFlush_CopiesAllBytes()
    {
        var data = new byte[] { 10, 20, 30 };
        using var source = new MemoryStream(data);
        using var destination = new MemoryStream();

        await source.CopyToAsync(destination, autoFlush: false);

        Assert.Equal(data, destination.ToArray());
    }

    [Fact]
    public async Task CopyToAsync_EmptySource_ProducesEmptyDestination()
    {
        using var source = new MemoryStream();
        using var destination = new MemoryStream();

        await source.CopyToAsync(destination, autoFlush: false);

        Assert.Empty(destination.ToArray());
    }

    [Fact]
    public async Task CopyToAsync_WithProgress_CopiesAllBytes()
    {
        var data = new byte[1024];
        new Random(0).NextBytes(data);
        using var source = new MemoryStream(data);
        using var destination = new MemoryStream();

        await source.CopyToAsync(destination, progress: null);

        Assert.Equal(data, destination.ToArray());
    }

    [Fact]
    public async Task CopyToAsync_WithProgress_ReportsProgress()
    {
        var data = new byte[1024];
        using var source = new MemoryStream(data);
        using var destination = new MemoryStream();

        var reports = new List<double>();
        var progress = new Progress<double>(v => reports.Add(v));

        await source.CopyToAsync(destination, progress: progress);

        // Allow Progress<T> callbacks to fire on the thread pool
        await Task.Delay(50);

        Assert.NotEmpty(reports);
        Assert.All(reports, v => Assert.InRange(v, 0.0, 1.0));
        Assert.Equal(1.0, reports[^1], precision: 5);
    }
}
