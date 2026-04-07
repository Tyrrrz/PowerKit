using PowerKit.Extensions;

namespace PowerKit.Tests;

public class AggregateExceptionExtensionsTests
{
    [Fact]
    public void TryGetSingle_SingleInnerException_ReturnsIt()
    {
        var inner = new Exception("only");
        var aggregate = new AggregateException(inner);

        Assert.Same(inner, aggregate.TryGetSingle());
    }

    [Fact]
    public void TryGetSingle_MultipleInnerExceptions_ReturnsNull()
    {
        var aggregate = new AggregateException(new Exception("a"), new Exception("b"));

        Assert.Null(aggregate.TryGetSingle());
    }

    [Fact]
    public void TryGetSingle_NestedAggregateWithOneLeaf_ReturnsLeaf()
    {
        var leaf = new Exception("leaf");
        var nested = new AggregateException(leaf);
        var outer = new AggregateException(nested);

        Assert.Same(leaf, outer.TryGetSingle());
    }

    [Fact]
    public void TryGetSingle_NestedAggregateWithMultipleLeaves_ReturnsNull()
    {
        var nested = new AggregateException(new Exception("a"), new Exception("b"));
        var outer = new AggregateException(nested);

        Assert.Null(outer.TryGetSingle());
    }
}
