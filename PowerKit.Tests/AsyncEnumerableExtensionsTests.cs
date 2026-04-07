using PowerKit.Extensions;

namespace PowerKit.Tests;

public class AsyncEnumerableExtensionsTests
{
    private static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(IEnumerable<T> source)
    {
        foreach (var item in source)
            yield return item;
    }

    [Fact]
    public async Task TakeAsync_ZeroCount_ReturnsEmpty()
    {
        var source = ToAsyncEnumerable([1, 2, 3]);
        var result = await source.TakeAsync(0).ToListAsync();
        Assert.Empty(result);
    }

    [Fact]
    public async Task TakeAsync_NegativeCount_ReturnsEmpty()
    {
        var source = ToAsyncEnumerable([1, 2, 3]);
        var result = await source.TakeAsync(-1).ToListAsync();
        Assert.Empty(result);
    }

    [Fact]
    public async Task TakeAsync_CountLessThanSource_ReturnsThatMany()
    {
        var source = ToAsyncEnumerable([1, 2, 3, 4, 5]);
        var result = await source.TakeAsync(3).ToListAsync();
        Assert.Equal([1, 2, 3], result);
    }

    [Fact]
    public async Task TakeAsync_CountGreaterThanSource_ReturnsAll()
    {
        var source = ToAsyncEnumerable([1, 2, 3]);
        var result = await source.TakeAsync(10).ToListAsync();
        Assert.Equal([1, 2, 3], result);
    }

    [Fact]
    public async Task TakeAsync_ZeroCount_DoesNotConsumeAnyElements()
    {
        var consumed = 0;

        async IAsyncEnumerable<int> Tracked()
        {
            consumed++;
            yield return 1;
        }

        await Tracked().TakeAsync(0).ToListAsync();
        Assert.Equal(0, consumed);
    }

    [Fact]
    public async Task SelectManyAsync_FlattensResults()
    {
        var source = ToAsyncEnumerable(["ab", "cd", "ef"]);
        var result = await source.SelectManyAsync(s => s.ToCharArray()).ToListAsync();
        Assert.Equal(['a', 'b', 'c', 'd', 'e', 'f'], result);
    }

    [Fact]
    public async Task SelectManyAsync_EmptySource_ReturnsEmpty()
    {
        var source = ToAsyncEnumerable(Array.Empty<string>());
        var result = await source.SelectManyAsync(s => s.ToCharArray()).ToListAsync();
        Assert.Empty(result);
    }

    [Fact]
    public async Task ToListAsync_CollectsAllElements()
    {
        var source = ToAsyncEnumerable([1, 2, 3]);
        var result = await source.ToListAsync();
        Assert.Equal([1, 2, 3], result);
    }

    [Fact]
    public async Task ToListAsync_EmptySource_ReturnsEmptyList()
    {
        var source = ToAsyncEnumerable(Array.Empty<int>());
        var result = await source.ToListAsync();
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAwaiter_DirectAwait_ReturnsAllElements()
    {
        var source = ToAsyncEnumerable([10, 20, 30]);
        var result = await source;
        Assert.Equal([10, 20, 30], result);
    }
}
