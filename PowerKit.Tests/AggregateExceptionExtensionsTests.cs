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
        {
            // Act & assert
            var inner = new Exception("only");
            new AggregateException(inner).TryGetSingle().Should().BeSameAs(inner);
        }

        new AggregateException(new Exception("a"), new Exception("b"))
            .TryGetSingle()
            .Should()
            .BeNull();

        {
            var leaf = new Exception("leaf");
            new AggregateException(new AggregateException(leaf))
                .TryGetSingle()
                .Should()
                .BeSameAs(leaf);
        }

        new AggregateException(
                new AggregateException(new Exception("a"), new Exception("b"))
            )
            .TryGetSingle()
            .Should()
            .BeNull();
    }
}
