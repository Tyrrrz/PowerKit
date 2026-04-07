using FluentAssertions;
using PowerKit.Extensions;

namespace PowerKit.Tests;

public class EnumerableExtensionsTests
{
    [Fact]
    public void WhereNotNull_ReferenceType_Test()
    {
        // Arrange
        string?[] source = ["a", null, "b", null, "c"];

        // Act
        var result = source.WhereNotNull();

        // Assert
        result.Should().Equal("a", "b", "c");
    }

    [Fact]
    public void WhereNotNull_ReferenceType_Empty_Test()
    {
        // Act & assert
        Array.Empty<string?>().WhereNotNull().Should().BeEmpty();
    }

    [Fact]
    public void WhereNotNull_NullableStruct_Test()
    {
        // Arrange
        int?[] source = [1, null, 2, null, 3];

        // Act
        var result = source.WhereNotNull();

        // Assert
        result.Should().Equal(1, 2, 3);
    }

    [Fact]
    public void WhereNotNull_NullableStruct_Empty_Test()
    {
        // Act & assert
        Array.Empty<int?>().WhereNotNull().Should().BeEmpty();
    }

    [Fact]
    public void WhereNotNullOrWhiteSpace_Test()
    {
        // Arrange
        string?[] source = ["hello", null, "  ", "", "world"];

        // Act
        var result = source.WhereNotNullOrWhiteSpace();

        // Assert
        result.Should().Equal("hello", "world");
    }

    [Fact]
    public void WhereNotNullOrWhiteSpace_Empty_Test()
    {
        // Act & assert
        Array.Empty<string?>().WhereNotNullOrWhiteSpace().Should().BeEmpty();
    }

    [Fact]
    public void FirstOrNull_Test()
    {
        // Arrange
        int[] source = [5, 10, 15];

        // Act
        var result = source.FirstOrNull();

        // Assert
        result.Should().Be(5);
    }

    [Fact]
    public void FirstOrNull_Empty_Test()
    {
        // Act & assert
        Array.Empty<int>().FirstOrNull().Should().BeNull();
    }

    [Fact]
    public void ElementAtOrNull_Test()
    {
        // Arrange
        int[] source = [10, 20, 30];

        // Act
        var result = source.ElementAtOrNull(1);

        // Assert
        result.Should().Be(20);
    }

    [Fact]
    public void ElementAtOrNull_First_Test()
    {
        // Act & assert
        new[] { 42, 99 }.ElementAtOrNull(0).Should().Be(42);
    }

    [Fact]
    public void ElementAtOrNull_Last_Test()
    {
        // Act & assert
        new[] { 1, 2, 3 }.ElementAtOrNull(2).Should().Be(3);
    }

    [Fact]
    public void ElementAtOrNull_OutOfRange_Test()
    {
        // Act & assert
        new[] { 1, 2, 3 }.ElementAtOrNull(10).Should().BeNull();
    }

    [Fact]
    public void ElementAtOrNull_NegativeIndex_Test()
    {
        // Act & assert
        new[] { 1, 2, 3 }.ElementAtOrNull(-1).Should().BeNull();
    }

    [Fact]
    public void ElementAtOrNull_Empty_Test()
    {
        // Act & assert
        Array.Empty<int>().ElementAtOrNull(0).Should().BeNull();
    }
}
