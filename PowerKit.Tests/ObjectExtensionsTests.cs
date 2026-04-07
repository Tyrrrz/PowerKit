using PowerKit.Extensions;

namespace PowerKit.Tests;

public class ObjectExtensionsTests
{
    [Fact]
    public void ToSingletonEnumerable_ReturnsEnumerableWithSingleElement()
    {
        var result = 42.ToSingletonEnumerable().ToList();
        Assert.Single(result, 42);
    }

    [Fact]
    public void ToSingletonEnumerable_WorksWithReferenceType()
    {
        var obj = "hello";
        var result = obj.ToSingletonEnumerable().ToList();
        Assert.Single(result, "hello");
    }

    [Fact]
    public void ToSingletonEnumerable_WorksWithNull()
    {
        string? obj = null;
        var result = obj.ToSingletonEnumerable().ToList();
        Assert.Single(result, (string?)null);
    }
}
