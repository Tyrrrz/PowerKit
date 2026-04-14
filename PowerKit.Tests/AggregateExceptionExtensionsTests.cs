#nullable enable
using System;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class AggregateExceptionExtensionsTests
{
    [Fact]
    public void TryGetSingle_Test()
    {
        // Arrange
        var inner = new Exception("only");

        // Act & assert
        new AggregateException(inner).TryGetSingle().Should().BeSameAs(inner);
    }

    [Fact]
    public void TryGetSingle_Multiple_Test()
    {
        // Act & assert
        new AggregateException(new Exception("a"), new Exception("b"))
            .TryGetSingle()
            .Should()
            .BeNull();
    }

    [Fact]
    public void TryGetSingle_Nested_Test()
    {
        // Arrange
        var leaf = new Exception("leaf");

        // Act & assert
        new AggregateException(new AggregateException(leaf))
            .TryGetSingle()
            .Should()
            .BeSameAs(leaf);
    }

    [Fact]
    public void TryGetSingle_NestedMultiple_Test()
    {
        // Act & assert
        new AggregateException(
                new AggregateException(new Exception("a"), new Exception("b"))
            )
            .TryGetSingle()
            .Should()
            .BeNull();
    }
}
