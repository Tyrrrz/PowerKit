using FluentAssertions;
using PowerKit.Extensions;

namespace PowerKit.Tests;

public class AggregateExceptionExtensionsTests
{
    [Fact]
    public void TryGetSingle_Test()
    {
        // Arrange
        var inner = new Exception("only");
        var aggregate = new AggregateException(inner);

        // Act
        var result = aggregate.TryGetSingle();

        // Assert
        result.Should().BeSameAs(inner);
    }

    [Fact]
    public void TryGetSingle_Multiple_Test()
    {
        // Arrange
        var aggregate = new AggregateException(new Exception("a"), new Exception("b"));

        // Act
        var result = aggregate.TryGetSingle();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void TryGetSingle_NestedSingleLeaf_Test()
    {
        // Arrange
        var leaf = new Exception("leaf");
        var nested = new AggregateException(leaf);
        var outer = new AggregateException(nested);

        // Act
        var result = outer.TryGetSingle();

        // Assert
        result.Should().BeSameAs(leaf);
    }

    [Fact]
    public void TryGetSingle_NestedMultipleLeaves_Test()
    {
        // Arrange
        var nested = new AggregateException(new Exception("a"), new Exception("b"));
        var outer = new AggregateException(nested);

        // Act
        var result = outer.TryGetSingle();

        // Assert
        result.Should().BeNull();
    }
}
