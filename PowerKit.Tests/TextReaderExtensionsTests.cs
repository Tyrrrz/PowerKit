using System.IO;
using PowerKit.Extensions;

namespace PowerKit.Tests;

public class TextReaderExtensionsTests
{
    [Fact]
    public async Task ReadLinesAsync_ReadsAllLines()
    {
        using var reader = new StringReader("line1\nline2\nline3");
        var lines = await reader.ReadLinesAsync().ToListAsync();
        Assert.Equal(["line1", "line2", "line3"], lines);
    }

    [Fact]
    public async Task ReadLinesAsync_EmptyReader_ReturnsEmpty()
    {
        using var reader = new StringReader("");
        var lines = await reader.ReadLinesAsync().ToListAsync();
        Assert.Empty(lines);
    }

    [Fact]
    public async Task ReadLinesAsync_SingleLine_ReturnsSingleLine()
    {
        using var reader = new StringReader("hello");
        var lines = await reader.ReadLinesAsync().ToListAsync();
        Assert.Single(lines, "hello");
    }
}
