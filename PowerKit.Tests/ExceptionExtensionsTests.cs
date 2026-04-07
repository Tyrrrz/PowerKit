using PowerKit.Extensions;

namespace PowerKit.Tests;

public class ExceptionExtensionsTests
{
    [Fact]
    public void GetSelfAndDescendants_NoInner_ReturnsSelf()
    {
        var ex = new Exception("root");
        var result = ex.GetSelfAndDescendants();
        Assert.Single(result, ex);
    }

    [Fact]
    public void GetSelfAndDescendants_WithInnerException_ReturnsSelfAndInner()
    {
        var inner = new Exception("inner");
        var outer = new Exception("outer", inner);

        var result = outer.GetSelfAndDescendants();

        Assert.Equal([outer, inner], result);
    }

    [Fact]
    public void GetSelfAndDescendants_WithChainedInnerExceptions_ReturnsAll()
    {
        var leaf = new Exception("leaf");
        var middle = new Exception("middle", leaf);
        var root = new Exception("root", middle);

        var result = root.GetSelfAndDescendants();

        Assert.Equal([root, middle, leaf], result);
    }

    [Fact]
    public void GetSelfAndDescendants_WithAggregateException_FlattensInnerExceptions()
    {
        var inner1 = new Exception("inner1");
        var inner2 = new Exception("inner2");
        var aggregate = new AggregateException("aggregate", inner1, inner2);

        var result = aggregate.GetSelfAndDescendants();

        Assert.Equal([aggregate, inner1, inner2], result);
    }

    [Fact]
    public void GetSelfAndDescendants_NestedAggregateException_ReturnsAllDescendants()
    {
        var leaf = new Exception("leaf");
        var inner = new AggregateException("inner", leaf);
        var root = new AggregateException("root", inner);

        var result = root.GetSelfAndDescendants();

        Assert.Equal([root, inner, leaf], result);
    }
}
