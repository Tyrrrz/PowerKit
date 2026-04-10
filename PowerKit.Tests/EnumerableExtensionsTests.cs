using System;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class EnumerableExtensionsTests
{
    [Fact]
    public void WhereNotNull_Test()
    {
        // Act & assert
        new string?[] { "a", null, "b", null, "c" }.WhereNotNull().Should().Equal("a", "b", "c");
        new int?[] { 1, null, 2, null, 3 }.WhereNotNull().Should().Equal(1, 2, 3);
        Array.Empty<string?>().WhereNotNull().Should().BeEmpty();
    }

    [Fact]
    public void WhereNotNullOrWhiteSpace_Test()
    {
        // Act & assert
        new string?[] { "hello", null, "  ", "", "world" }
            .WhereNotNullOrWhiteSpace()
            .Should()
            .Equal("hello", "world");
    }

    [Fact]
    public void FirstOrNull_Test()
    {
        // Act & assert
        new[] { 5, 10, 15 }.FirstOrNull().Should().Be(5);
        Array.Empty<int>().FirstOrNull().Should().BeNull();
    }

    [Fact]
    public void LastOrNull_Test()
    {
        // Act & assert
        new[] { 5, 10, 15 }.LastOrNull().Should().Be(15);
        new[] { 42 }.LastOrNull().Should().Be(42);
        Array.Empty<int>().LastOrNull().Should().BeNull();
    }

    [Fact]
    public void ElementAtOrNull_Test()
    {
        // Act & assert
        new[] { 10, 20, 30 }.ElementAtOrNull(1).Should().Be(20);
        new[] { 10, 20, 30 }.ElementAtOrNull(0).Should().Be(10);
        new[] { 10, 20, 30 }.ElementAtOrNull(10).Should().BeNull();
        new[] { 10, 20, 30 }.ElementAtOrNull(-1).Should().BeNull();
        Array.Empty<int>().ElementAtOrNull(0).Should().BeNull();
    }
}
