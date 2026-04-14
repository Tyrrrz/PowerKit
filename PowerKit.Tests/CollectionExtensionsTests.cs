#nullable enable
using System.Collections.Generic;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class CollectionExtensionsTests
{
    [Fact]
    public void RemoveAll_Test()
    {
        // Arrange
        var collection = (ICollection<int>) new List<int> { 1, 2, 3, 4, 5 };

        // Act
        var removed = collection.RemoveAll(x => x % 2 == 0);

        // Assert
        removed.Should().Be(2);
        collection.Should().Equal(1, 3, 5);
    }
}
