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
}
