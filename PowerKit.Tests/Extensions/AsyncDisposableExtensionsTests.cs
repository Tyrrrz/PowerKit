using System;
using System.Threading.Tasks;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

file class AsyncDisposableStub(Action onDispose) : IDisposable, IAsyncDisposable
{
    public void Dispose() => throw new Exception("DisposeAsync() should've been called instead");

    public ValueTask DisposeAsync()
    {
        onDispose();
        return default;
    }
}

file class DisposableStub(Action onDispose) : IDisposable
{
    public void Dispose() => onDispose();
}

public class AsyncDisposableExtensionsTests
{
    [Fact]
    public async Task ToAsyncDisposable_IAsyncDisposable_Test()
    {
        // Arrange
        var asyncDisposeCalled = false;

        var disposable = new AsyncDisposableStub(() => asyncDisposeCalled = true);

        // Act
        await disposable.ToAsyncDisposable().DisposeAsync();

        // Assert
        asyncDisposeCalled.Should().BeTrue();
    }

    [Fact]
    public async Task ToAsyncDisposable_IDisposable_Test()
    {
        // Arrange
        var disposeCalled = false;

        var disposable = new DisposableStub(() => disposeCalled = true);

        // Act
        await disposable.ToAsyncDisposable().DisposeAsync();

        // Assert
        disposeCalled.Should().BeTrue();
    }
}
