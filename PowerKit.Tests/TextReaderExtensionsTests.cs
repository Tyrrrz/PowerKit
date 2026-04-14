#nullable enable
using System.IO;
using System.Threading.Tasks;
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
        var result = await reader.ReadLinesAsync().ToListAsync();

        // Assert
        result.Should().Equal("line1", "line2", "line3");
    }

    [Fact]
    public async Task ReadLinesAsync_Empty_Test()
    {
        // Arrange
        using var reader = new StringReader("");

        // Act
        var result = await reader.ReadLinesAsync().ToListAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task ReadLinesAsync_SingleLine_Test()
    {
        // Arrange
        using var reader = new StringReader("hello");

        // Act
        var result = await reader.ReadLinesAsync().ToListAsync();

        // Assert
        result.Should().Equal("hello");
    }
}
