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
}
