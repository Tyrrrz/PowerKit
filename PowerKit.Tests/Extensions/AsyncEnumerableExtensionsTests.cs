using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

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
    public async Task SelectManyAsync_Test()
    {
        // Act & assert
        (await ToAsyncEnumerable(["ab", "cd"]).SelectManyAsync(s => s.ToCharArray()).ToListAsync())
            .Should()
            .Equal('a', 'b', 'c', 'd');
    }

    [Fact]
    public async Task SkipAsync_Test()
    {
        // Act & assert
        (await ToAsyncEnumerable([1, 2, 3, 4, 5]).SkipAsync(2).ToListAsync())
            .Should()
            .Equal(3, 4, 5);

        (await ToAsyncEnumerable([1, 2, 3]).SkipAsync(0).ToListAsync()).Should().Equal(1, 2, 3);

        (await ToAsyncEnumerable([1, 2, 3]).SkipAsync(10).ToListAsync()).Should().BeEmpty();
    }

    [Fact]
    public async Task TakeAsync_Test()
    {
        // Act & assert
        (await ToAsyncEnumerable([1, 2, 3, 4, 5]).TakeAsync(3).ToListAsync())
            .Should()
            .Equal(1, 2, 3);

        (await ToAsyncEnumerable([1, 2, 3]).TakeAsync(0).ToListAsync()).Should().BeEmpty();

        (await ToAsyncEnumerable([1, 2, 3]).TakeAsync(10).ToListAsync()).Should().Equal(1, 2, 3);
    }

    [Fact]
    public async Task ToListAsync_Test()
    {
        // Act & assert
        (await ToAsyncEnumerable([1, 2, 3]).ToListAsync())
            .Should()
            .Equal(1, 2, 3);
    }

    [Fact]
    public async Task GetAwaiter_Test()
    {
        // Act & assert
        (await ToAsyncEnumerable([10, 20, 30]))
            .Should()
            .Equal(10, 20, 30);
    }

    [Fact]
    public async Task OfTypeAsync_Test()
    {
        // Act & assert
        (
            await ToAsyncEnumerable<object>([1, "hello", 2, "world", 3])
                .OfTypeAsync<int>()
                .ToListAsync()
        )
            .Should()
            .Equal(1, 2, 3);

        (
            await ToAsyncEnumerable<object>([1, "hello", 2, "world", 3])
                .OfTypeAsync<string>()
                .ToListAsync()
        )
            .Should()
            .Equal("hello", "world");

        (await ToAsyncEnumerable<object>([1, 2, 3]).OfTypeAsync<string>().ToListAsync())
            .Should()
            .BeEmpty();
    }
}
