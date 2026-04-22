using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class DictionaryExtensionsTests
{
    [Fact]
    public void GetValueOrNull_Found_Test()
    {
        // Arrange
        IDictionary<string, int> source = new Dictionary<string, int> { ["one"] = 1 };

        // Act
        var result = source.GetValueOrNull("one");

        // Assert
        result.Should().Be(1);
    }

    [Fact]
    public void GetValueOrNull_NotFound_Test()
    {
        // Arrange
        IDictionary<string, int> source = new Dictionary<string, int> { ["one"] = 1 };

        // Act
        var result = source.GetValueOrNull("two");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetValueOrNull_ReadOnly_Found_Test()
    {
        // Arrange
        IReadOnlyDictionary<string, int> source = new Dictionary<string, int> { ["one"] = 1 };

        // Act
        var result = source.GetValueOrNull("one");

        // Assert
        result.Should().Be(1);
    }

    [Fact]
    public void GetValueOrNull_ReadOnly_NotFound_Test()
    {
        // Arrange
        IReadOnlyDictionary<string, int> source = new Dictionary<string, int> { ["one"] = 1 };

        // Act
        var result = source.GetValueOrNull("two");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void ToDictionary_Test()
    {
        // Arrange
        var source = (IDictionary)new Hashtable { ["one"] = 1, ["two"] = 2 };

        // Act
        var result = source.ToDictionary<string, int>();

        // Assert
        result.Should().BeOfType<Dictionary<string, int>>();
        result.Should().Contain("one", 1).And.Contain("two", 2);
    }

    [Fact]
    public void ToDictionary_CustomComparer_Test()
    {
        // Arrange
        var source = (IDictionary)new Hashtable { ["one"] = 1, ["two"] = 2 };

        // Act
        var result = source.ToDictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        // Assert
        result.Should().BeOfType<Dictionary<string, int>>();
        result.Should().Contain("ONE", 1).And.Contain("TWO", 2);
    }
}
