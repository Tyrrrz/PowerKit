using System.IO;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class TextReaderExtensionsTests
{
    [Fact]
    public async Task ReadLinesAsync_Test()
    {
        // Arrange
        using var reader = new StringReader("line1\nline2\nline3");

        // Act
        var lines = await reader.ReadLinesAsync().ToListAsync();

        // Assert
        lines.Should().Equal("line1", "line2", "line3");
    }

    [Fact]
    public async Task ReadLinesAsync_Empty_Test()
    {
        // Arrange
        using var reader = new StringReader("");

        // Act
        var lines = await reader.ReadLinesAsync().ToListAsync();

        // Assert
        lines.Should().BeEmpty();
    }

    [Fact]
    public async Task ReadLinesAsync_SingleLine_Test()
    {
        // Arrange
        using var reader = new StringReader("hello");

        // Act
        var lines = await reader.ReadLinesAsync().ToListAsync();

        // Assert
        lines.Should().Equal("hello");
    }
}
