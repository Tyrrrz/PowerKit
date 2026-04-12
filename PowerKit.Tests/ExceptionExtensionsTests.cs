using System;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class ExceptionExtensionsTests
{
    [Fact]
    public void GetSelfAndDescendants_Test()
    {
        // Arrange
        var ex = new Exception("root");

        // Act & assert
        ex.GetSelfAndDescendants().Should().Equal(ex);
    }

    [Fact]
    public void GetSelfAndDescendants_Chain_Test()
    {
        // Arrange
        var inner = new Exception("inner");
        var outer = new Exception("outer", inner);

        // Act & assert
        outer.GetSelfAndDescendants().Should().Equal(outer, inner);
    }

    [Fact]
    public void GetSelfAndDescendants_DeepChain_Test()
    {
        // Arrange
        var leaf = new Exception("leaf");
        var middle = new Exception("middle", leaf);
        var root = new Exception("root", middle);

        // Act & assert
        root.GetSelfAndDescendants().Should().Equal(root, middle, leaf);
    }

    [Fact]
    public void GetSelfAndDescendants_Aggregate_Test()
    {
        // Arrange
        var inner1 = new Exception("inner1");
        var inner2 = new Exception("inner2");
        var aggregate = new AggregateException("aggregate", inner1, inner2);

        // Act & assert
        aggregate.GetSelfAndDescendants().Should().Equal(aggregate, inner1, inner2);
    }

    [Fact]
    public void GetSelfAndDescendants_NestedAggregate_Test()
    {
        // Arrange
        var leaf = new Exception("leaf");
        var inner = new AggregateException("inner", leaf);
        var root = new AggregateException("root", inner);

        // Act & assert
        root.GetSelfAndDescendants().Should().Equal(root, inner, leaf);
    }
}
