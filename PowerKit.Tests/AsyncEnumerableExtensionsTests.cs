using FluentAssertions;
using PowerKit.Extensions;

namespace PowerKit.Tests;

public class AsyncEnumerableExtensionsTests
{
    private static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(IEnumerable<T> source)
    {
        foreach (var item in source)
        {
            await Task.Yield();
            yield return item;
        }
    }

    [Fact]
    public async Task TakeAsync_Zero_Test()
    {
        // Arrange
        var source = ToAsyncEnumerable([1, 2, 3]);

        // Act
        var result = await source.TakeAsync(0).ToListAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task TakeAsync_Negative_Test()
    {
        // Arrange
        var source = ToAsyncEnumerable([1, 2, 3]);

        // Act
        var result = await source.TakeAsync(-1).ToListAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task TakeAsync_Test()
    {
        // Arrange
        var source = ToAsyncEnumerable([1, 2, 3, 4, 5]);

        // Act
        var result = await source.TakeAsync(3).ToListAsync();

        // Assert
        result.Should().Equal(1, 2, 3);
    }

    [Fact]
    public async Task TakeAsync_CountExceedsSource_Test()
    {
        // Arrange
        var source = ToAsyncEnumerable([1, 2, 3]);

        // Act
        var result = await source.TakeAsync(10).ToListAsync();

        // Assert
        result.Should().Equal(1, 2, 3);
    }

    [Fact]
    public async Task TakeAsync_Zero_DoesNotConsumeElements_Test()
    {
        // Arrange
        var consumed = 0;

        async IAsyncEnumerable<int> Tracked()
        {
            consumed++;
            yield return 1;
        }

        // Act
        await Tracked().TakeAsync(0).ToListAsync();

        // Assert
        consumed.Should().Be(0);
    }

    [Fact]
    public async Task SelectManyAsync_Test()
    {
        // Arrange
        var source = ToAsyncEnumerable(["ab", "cd", "ef"]);

        // Act
        var result = await source.SelectManyAsync(s => s.ToCharArray()).ToListAsync();

        // Assert
        result.Should().Equal('a', 'b', 'c', 'd', 'e', 'f');
    }

    [Fact]
    public async Task SelectManyAsync_Empty_Test()
    {
        // Arrange
        var source = ToAsyncEnumerable(Array.Empty<string>());

        // Act
        var result = await source.SelectManyAsync(s => s.ToCharArray()).ToListAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task ToListAsync_Test()
    {
        // Arrange
        var source = ToAsyncEnumerable([1, 2, 3]);

        // Act
        var result = await source.ToListAsync();

        // Assert
        result.Should().Equal(1, 2, 3);
    }

    [Fact]
    public async Task ToListAsync_Empty_Test()
    {
        // Arrange
        var source = ToAsyncEnumerable(Array.Empty<int>());

        // Act
        var result = await source.ToListAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAwaiter_Test()
    {
        // Arrange
        var source = ToAsyncEnumerable([10, 20, 30]);

        // Act
        var result = await source;

        // Assert
        result.Should().Equal(10, 20, 30);
    }
}
