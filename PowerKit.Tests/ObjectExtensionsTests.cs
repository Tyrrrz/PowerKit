using FluentAssertions;
using PowerKit.Extensions;

namespace PowerKit.Tests;

public class ObjectExtensionsTests
{
    [Fact]
    public void ToSingletonEnumerable_Test()
    {
        // Act
        var result = 42.ToSingletonEnumerable().ToList();

        // Assert
        result.Should().Equal(42);
    }

    [Fact]
    public void ToSingletonEnumerable_ReferenceType_Test()
    {
        // Act
        var result = "hello".ToSingletonEnumerable().ToList();

        // Assert
        result.Should().Equal("hello");
    }

    [Fact]
    public void ToSingletonEnumerable_Null_Test()
    {
        // Arrange
        string? obj = null;

        // Act
        var result = obj.ToSingletonEnumerable().ToList();

        // Assert
        result.Should().Equal((string?)null);
    }
}
