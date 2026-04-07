using PowerKit.Extensions;

namespace PowerKit.Tests;

public class FunctionalExtensionsTests
{
    [Fact]
    public void Pipe_TransformsValue()
    {
        var result = 5.Pipe(x => x * 2);
        Assert.Equal(10, result);
    }

    [Fact]
    public void Pipe_ChainedCalls_AppliesInOrder()
    {
        var result = "hello".Pipe(s => s.ToUpper()).Pipe(s => s + "!");
        Assert.Equal("HELLO!", result);
    }

    [Fact]
    public void NullIf_PredicateMatches_ReturnsNull()
    {
        int value = 0;
        Assert.Null(value.NullIf(v => v == 0));
    }

    [Fact]
    public void NullIf_PredicateDoesNotMatch_ReturnsValue()
    {
        int value = 5;
        Assert.Equal(5, value.NullIf(v => v == 0));
    }

    [Fact]
    public void NullIfDefault_DefaultValue_ReturnsNull()
    {
        int value = 0;
        Assert.Null(value.NullIfDefault());
    }

    [Fact]
    public void NullIfDefault_NonDefaultValue_ReturnsValue()
    {
        int value = 42;
        Assert.Equal(42, value.NullIfDefault());
    }

    [Fact]
    public void NullIfDefault_DefaultGuid_ReturnsNull()
    {
        Assert.Null(Guid.Empty.NullIfDefault());
    }

    [Fact]
    public void NullIfDefault_NonDefaultGuid_ReturnsValue()
    {
        var id = Guid.NewGuid();
        Assert.Equal(id, id.NullIfDefault());
    }
}
