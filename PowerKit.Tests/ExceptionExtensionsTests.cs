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
        {
            // Act & assert
            var ex = new Exception("root");
            ex.GetSelfAndDescendants().Should().Equal(ex);
        }

        {
            var inner = new Exception("inner");
            var outer = new Exception("outer", inner);
            outer.GetSelfAndDescendants().Should().Equal(outer, inner);
        }

        {
            var leaf = new Exception("leaf");
            var middle = new Exception("middle", leaf);
            var root = new Exception("root", middle);
            root.GetSelfAndDescendants().Should().Equal(root, middle, leaf);
        }

        {
            var inner1 = new Exception("inner1");
            var inner2 = new Exception("inner2");
            var aggregate = new AggregateException("aggregate", inner1, inner2);
            aggregate.GetSelfAndDescendants().Should().Equal(aggregate, inner1, inner2);
        }

        {
            var leaf = new Exception("leaf");
            var inner = new AggregateException("inner", leaf);
            var root = new AggregateException("root", inner);
            root.GetSelfAndDescendants().Should().Equal(root, inner, leaf);
        }
    }
}
