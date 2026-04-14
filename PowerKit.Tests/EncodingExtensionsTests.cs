#nullable enable
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
    public void WithoutPreamble_FallbackIsolation_Test()
    {
        // Arrange — wrap the shared UTF8 singleton (read-only)
        var originalFallback = Encoding.UTF8.EncoderFallback;

        // Act — should not throw even though Encoding.UTF8 is a read-only singleton
        var encoding = Encoding.UTF8.WithoutPreamble();

        // Assert — the original singleton is not mutated and encode/decode still works
        Encoding.UTF8.EncoderFallback.Should().BeSameAs(originalFallback);
        encoding.GetPreamble().Should().BeEmpty();
        encoding.GetString(encoding.GetBytes("hello")).Should().Be("hello");
    }
}
