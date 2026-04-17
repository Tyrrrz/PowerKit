using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class ZipArchiveEntryExtensionsTests
{
    private static ZipArchive CreateArchive(ZipArchiveMode mode = ZipArchiveMode.Update) =>
        new(new MemoryStream(), mode, false);

    [Fact]
    public void ReadAllBytes_Test()
    {
        // Arrange
        using var archive = CreateArchive();
        var entry = archive.CreateEntry("file.bin");
        using (var stream = entry.Open())
        {
            stream.Write([1, 2, 3, 4, 5], 0, 5);
        }

        // Act
        var bytes = entry.ReadAllBytes();

        // Assert
        bytes.Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public async Task ReadAllBytesAsync_Test()
    {
        // Arrange
        using var archive = CreateArchive();
        var entry = archive.CreateEntry("file.bin");
        using (var stream = entry.Open())
        {
            stream.Write([1, 2, 3, 4, 5], 0, 5);
        }

        // Act
        var bytes = await entry.ReadAllBytesAsync();

        // Assert
        bytes.Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void ReadAllLines_Test()
    {
        // Arrange
        using var archive = CreateArchive();
        var entry = archive.CreateEntry("file.txt");
        using (var stream = entry.Open())
        using (var writer = new StreamWriter(stream, Encoding.UTF8))
        {
            writer.WriteLine("line1");
            writer.WriteLine("line2");
            writer.Write("line3");
        }

        // Act
        var lines = entry.ReadAllLines();

        // Assert
        lines.Should().Equal("line1", "line2", "line3");
    }

    [Fact]
    public async Task ReadAllLinesAsync_Test()
    {
        // Arrange
        using var archive = CreateArchive();
        var entry = archive.CreateEntry("file.txt");
        using (var stream = entry.Open())
        using (var writer = new StreamWriter(stream, Encoding.UTF8))
        {
            writer.WriteLine("line1");
            writer.WriteLine("line2");
            writer.Write("line3");
        }

        // Act
        var lines = await entry.ReadAllLinesAsync();

        // Assert
        lines.Should().Equal("line1", "line2", "line3");
    }

    [Fact]
    public void ReadAllText_Test()
    {
        // Arrange
        using var archive = CreateArchive();
        var entry = archive.CreateEntry("file.txt");
        using (var stream = entry.Open())
        using (var writer = new StreamWriter(stream, Encoding.UTF8))
        {
            writer.Write("hello world");
        }

        // Act
        var text = entry.ReadAllText();

        // Assert
        text.Should().Be("hello world");
    }

    [Fact]
    public async Task ReadAllTextAsync_Test()
    {
        // Arrange
        using var archive = CreateArchive();
        var entry = archive.CreateEntry("file.txt");
        using (var stream = entry.Open())
        using (var writer = new StreamWriter(stream, Encoding.UTF8))
        {
            writer.Write("hello world");
        }

        // Act
        var text = await entry.ReadAllTextAsync();

        // Assert
        text.Should().Be("hello world");
    }

    [Fact]
    public void WriteAllBytes_Test()
    {
        // Arrange
        using var archive = CreateArchive();
        var entry = archive.CreateEntry("file.bin");

        // Act
        entry.WriteAllBytes([10, 20, 30]);

        // Assert
        using var stream = entry.Open();
        using var buffer = new MemoryStream();
        stream.CopyTo(buffer);
        buffer.ToArray().Should().Equal(10, 20, 30);
    }

    [Fact]
    public async Task WriteAllBytesAsync_Test()
    {
        // Arrange
        using var archive = CreateArchive();
        var entry = archive.CreateEntry("file.bin");

        // Act
        await entry.WriteAllBytesAsync([10, 20, 30]);

        // Assert
        using var stream = entry.Open();
        using var buffer = new MemoryStream();
        stream.CopyTo(buffer);
        buffer.ToArray().Should().Equal(10, 20, 30);
    }

    [Fact]
    public void WriteAllLines_Test()
    {
        // Arrange
        using var archive = CreateArchive();
        var entry = archive.CreateEntry("file.txt");

        // Act
        entry.WriteAllLines(["line1", "line2", "line3"]);

        // Assert
        using var stream = entry.Open();
        using var reader = new StreamReader(stream, Encoding.UTF8);
        reader.ReadLine().Should().Be("line1");
        reader.ReadLine().Should().Be("line2");
        reader.ReadLine().Should().Be("line3");
    }

    [Fact]
    public async Task WriteAllLinesAsync_Test()
    {
        // Arrange
        using var archive = CreateArchive();
        var entry = archive.CreateEntry("file.txt");

        // Act
        await entry.WriteAllLinesAsync(["line1", "line2", "line3"]);

        // Assert
        using var stream = entry.Open();
        using var reader = new StreamReader(stream, Encoding.UTF8);
        reader.ReadLine().Should().Be("line1");
        reader.ReadLine().Should().Be("line2");
        reader.ReadLine().Should().Be("line3");
    }

    [Fact]
    public void WriteAllText_Test()
    {
        // Arrange
        using var archive = CreateArchive();
        var entry = archive.CreateEntry("file.txt");

        // Act
        entry.WriteAllText("hello world");

        // Assert
        using var stream = entry.Open();
        using var reader = new StreamReader(stream, Encoding.UTF8);
        reader.ReadToEnd().Should().Be("hello world");
    }

    [Fact]
    public async Task WriteAllTextAsync_Test()
    {
        // Arrange
        using var archive = CreateArchive();
        var entry = archive.CreateEntry("file.txt");

        // Act
        await entry.WriteAllTextAsync("hello world");

        // Assert
        using var stream = entry.Open();
        using var reader = new StreamReader(stream, Encoding.UTF8);
        reader.ReadToEnd().Should().Be("hello world");
    }
}
