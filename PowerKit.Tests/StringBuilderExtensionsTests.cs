using System.Text;
using FluentAssertions;
using PowerKit.Extensions;

namespace PowerKit.Tests;

public class StringBuilderExtensionsTests
{
    [Fact]
    public void AppendIfNotEmpty_Empty_Test()
    {
        // Arrange
        var builder = new StringBuilder();

        // Act
        builder.AppendIfNotEmpty(',');

        // Assert
        builder.ToString().Should().Be("");
    }

    [Fact]
    public void AppendIfNotEmpty_Test()
    {
        // Arrange
        var builder = new StringBuilder("hello");

        // Act
        builder.AppendIfNotEmpty(',');

        // Assert
        builder.ToString().Should().Be("hello,");
    }

    [Fact]
    public void AppendIfNotEmpty_Chaining_Test()
    {
        // Arrange
        var builder = new StringBuilder("a");

        // Act
        builder.AppendIfNotEmpty(',').Append("b");

        // Assert
        builder.ToString().Should().Be("a,b");
    }

    [Fact]
    public void Trim_Test()
    {
        // Arrange
        var builder = new StringBuilder("  hello  ");

        // Act
        builder.Trim();

        // Assert
        builder.ToString().Should().Be("hello");
    }

    [Fact]
    public void Trim_LeadingOnly_Test()
    {
        // Arrange
        var builder = new StringBuilder("  hello");

        // Act
        builder.Trim();

        // Assert
        builder.ToString().Should().Be("hello");
    }

    [Fact]
    public void Trim_TrailingOnly_Test()
    {
        // Arrange
        var builder = new StringBuilder("hello  ");

        // Act
        builder.Trim();

        // Assert
        builder.ToString().Should().Be("hello");
    }

    [Fact]
    public void Trim_NoWhitespace_Test()
    {
        // Arrange
        var builder = new StringBuilder("hello");

        // Act
        builder.Trim();

        // Assert
        builder.ToString().Should().Be("hello");
    }

    [Fact]
    public void Trim_Empty_Test()
    {
        // Arrange
        var builder = new StringBuilder();

        // Act
        builder.Trim();

        // Assert
        builder.ToString().Should().Be("");
    }

    [Fact]
    public void Trim_OnlyWhitespace_Test()
    {
        // Arrange
        var builder = new StringBuilder("   ");

        // Act
        builder.Trim();

        // Assert
        builder.ToString().Should().Be("");
    }

    [Fact]
    public void Trim_Chaining_Test()
    {
        // Arrange
        var builder = new StringBuilder("  hello  ");

        // Act
        var result = builder.Trim().Append("!");

        // Assert
        result.ToString().Should().Be("hello!");
    }
}
