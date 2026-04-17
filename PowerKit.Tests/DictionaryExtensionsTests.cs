using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class DictionaryExtensionsTests
{
    [Fact]
    public void ToDictionary_Test()
    {
        // Arrange
        IDictionary source = new System.Collections.Hashtable { ["one"] = 1, ["two"] = 2 };

        // Act
        var result = source.ToDictionary<string, int>(StringComparer.Ordinal);

        // Assert
        result.Should().BeOfType<Dictionary<string, int>>();
        result.Comparer.Should().Be(StringComparer.Ordinal);
        result.Should().Contain("one", 1).And.Contain("two", 2);
    }

    [Fact]
    public void ToDictionary_DefaultComparer_Test()
    {
        // Arrange
        IDictionary source = new System.Collections.Hashtable { ["one"] = 1, ["two"] = 2 };

        // Act
        var result = source.ToDictionary<string, int>();

        // Assert
        result.Should().BeOfType<Dictionary<string, int>>();
        result.Comparer.Should().Be(EqualityComparer<string>.Default);
        result.Should().Contain("one", 1).And.Contain("two", 2);
    }
}
