using System.Text;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class EncodingExtensionsTests
{
    [Fact]
    public void Utf8WithoutBom_Test()
    {
        // Arrange
        var text = "hello, world! 🌍";

        // Act
        var bytes = Encoding.Utf8WithoutBom.GetBytes(text);

        // Assert
        Encoding.UTF8.GetString(bytes).Should().Be(text);
    }

    [Fact]
    public void WithoutPreamble_Test()
    {
        // Arrange
        var encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true);
        encoding.GetPreamble().Should().NotBeEmpty();

        // Act
        var result = encoding.WithoutPreamble();

        // Assert
        result.GetPreamble().Should().BeEmpty();
        result.GetString(result.GetBytes("hello")).Should().Be("hello");
    }

    [Fact]
    public void WithoutPreamble_WithoutPreamble_Test()
    {
        // Arrange
        var encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
        encoding.GetPreamble().Should().BeEmpty();

        // Act
        var result = encoding.WithoutPreamble();

        // Assert
        result.Should().BeSameAs(encoding);
    }

    [Fact]
    public void WithoutPreamble_RoundTrip_Test()
    {
        // Arrange
        var text = "hello, world! 🌍";
        var encoding = new UTF8Encoding(true).WithoutPreamble();

        // Act
        var bytes = encoding.GetBytes(text);
        var decoded = encoding.GetString(bytes);

        // Assert
        decoded.Should().Be(text);
    }
}
