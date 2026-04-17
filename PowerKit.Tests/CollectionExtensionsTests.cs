using System.Collections.Generic;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class CollectionExtensionsTests
{
    [Fact]
    public void AddRange_Test()
    {
        // Arrange
        var collection = (ICollection<int>)new List<int> { 1, 2, 3 };

        // Act
        collection.AddRange([4, 5, 6]);

        // Assert
        collection.Should().Equal(1, 2, 3, 4, 5, 6);
    }

    [Fact]
    public void AddRange_SameCollection_Test()
    {
        // Arrange
        var collection = (ICollection<int>)new List<int> { 1, 2, 3 };

        // Act
        collection.AddRange(collection);

        // Assert
        collection.Should().Equal(1, 2, 3, 1, 2, 3);
    }

    [Fact]
    public void RemoveRange_Test()
    {
        // Arrange
        var collection = (ICollection<int>)new List<int> { 1, 2, 3, 4, 5 };

        // Act
        collection.RemoveRange([2, 4]);

        // Assert
        collection.Should().Equal(1, 3, 5);
    }

    [Fact]
    public void RemoveRange_SameCollection_Test()
    {
        // Arrange
        var collection = (ICollection<int>)new List<int> { 1, 2, 3, 4, 5 };

        // Act
        collection.RemoveRange(collection);

        // Assert
        collection.Should().BeEmpty();
    }

    [Fact]
    public void RemoveAll_Test()
    {
        // Arrange
        var collection = (ICollection<int>)new List<int> { 1, 2, 3, 4, 5 };

        // Act
        var removed = collection.RemoveAll(x => x % 2 == 0);

        // Assert
        removed.Should().Be(2);
        collection.Should().Equal(1, 3, 5);
    }
}
