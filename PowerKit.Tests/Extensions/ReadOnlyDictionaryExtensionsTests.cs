using System.Collections.Generic;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class ReadOnlyDictionaryExtensionsTests
{
    [Fact]
    public void GetValueOrNull_Found_Test()
    {
        // Arrange
        IReadOnlyDictionary<string, int> source = new Dictionary<string, int> { ["one"] = 1 };

        // Act
        var result = source.GetValueOrNull("one");

        // Assert
        result.Should().Be(1);
    }

    [Fact]
    public void GetValueOrNull_NotFound_Test()
    {
        // Arrange
        IReadOnlyDictionary<string, int> source = new Dictionary<string, int> { ["one"] = 1 };

        // Act
        var result = source.GetValueOrNull("two");

        // Assert
        result.Should().BeNull();
    }
}
