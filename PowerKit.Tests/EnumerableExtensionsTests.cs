using PowerKit.Extensions;

namespace PowerKit.Tests;

public class EnumerableExtensionsTests
{
    [Fact]
    public void WhereNotNull_ReferenceType_FiltersNulls()
    {
        string?[] source = ["a", null, "b", null, "c"];
        Assert.Equal(["a", "b", "c"], source.WhereNotNull());
    }

    [Fact]
    public void WhereNotNull_ReferenceType_EmptySource_ReturnsEmpty()
    {
        Assert.Empty(Array.Empty<string?>().WhereNotNull());
    }

    [Fact]
    public void WhereNotNull_NullableStruct_FiltersNulls()
    {
        int?[] source = [1, null, 2, null, 3];
        Assert.Equal([1, 2, 3], source.WhereNotNull());
    }

    [Fact]
    public void WhereNotNull_NullableStruct_EmptySource_ReturnsEmpty()
    {
        Assert.Empty(Array.Empty<int?>().WhereNotNull());
    }

    [Fact]
    public void WhereNotNullOrWhiteSpace_FiltersNullsAndWhitespace()
    {
        string?[] source = ["hello", null, "  ", "", "world"];
        Assert.Equal(["hello", "world"], source.WhereNotNullOrWhiteSpace());
    }

    [Fact]
    public void WhereNotNullOrWhiteSpace_EmptySource_ReturnsEmpty()
    {
        Assert.Empty(Array.Empty<string?>().WhereNotNullOrWhiteSpace());
    }

    [Fact]
    public void FirstOrNull_NonEmptySource_ReturnsFirst()
    {
        int[] source = [5, 10, 15];
        Assert.Equal(5, source.FirstOrNull());
    }

    [Fact]
    public void FirstOrNull_EmptySource_ReturnsNull()
    {
        Assert.Null(Array.Empty<int>().FirstOrNull());
    }

    [Fact]
    public void ElementAtOrNull_ValidIndex_ReturnsElement()
    {
        int[] source = [10, 20, 30];
        Assert.Equal(20, source.ElementAtOrNull(1));
    }

    [Fact]
    public void ElementAtOrNull_IndexZero_ReturnsFirst()
    {
        int[] source = [42, 99];
        Assert.Equal(42, source.ElementAtOrNull(0));
    }

    [Fact]
    public void ElementAtOrNull_IndexAtLastElement_ReturnsLast()
    {
        int[] source = [1, 2, 3];
        Assert.Equal(3, source.ElementAtOrNull(2));
    }

    [Fact]
    public void ElementAtOrNull_IndexOutOfRange_ReturnsNull()
    {
        int[] source = [1, 2, 3];
        Assert.Null(source.ElementAtOrNull(10));
    }

    [Fact]
    public void ElementAtOrNull_NegativeIndex_ReturnsNull()
    {
        int[] source = [1, 2, 3];
        Assert.Null(source.ElementAtOrNull(-1));
    }

    [Fact]
    public void ElementAtOrNull_EmptySource_ReturnsNull()
    {
        Assert.Null(Array.Empty<int>().ElementAtOrNull(0));
    }
}
