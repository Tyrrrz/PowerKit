using System.Linq;
using System.Text.Json;
using AwesomeAssertions;
using PowerKit.Json.Extensions;
using Xunit;

namespace PowerKit.Json.Tests.Extensions;

public class JsonPropertyExtensionsTests
{
    [Fact]
    public void Deconstruct_Test()
    {
        // Arrange
        var jsonProperty = JsonElement
            .Parse(
                """
                {"name":"value"}
                """
            )
            .EnumerateObject()
            .First();

        // Act
        var (name, value) = jsonProperty;

        // Assert
        name.Should().Be("name");
        value.Should().Be("value");
    }
}
