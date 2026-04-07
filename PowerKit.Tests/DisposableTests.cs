using PowerKit;

namespace PowerKit.Tests;

public class DisposableTests
{
    [Fact]
    public void Null_CanBeDisposedWithoutEffect()
    {
        // Should not throw
        Disposable.Null.Dispose();
    }

    [Fact]
    public void Create_InvokesActionOnDispose()
    {
        var invoked = false;
        var disposable = Disposable.Create(() => invoked = true);

        Assert.False(invoked);
        disposable.Dispose();
        Assert.True(invoked);
    }

    [Fact]
    public void Merge_DisposesAllItems()
    {
        var count = 0;
        var disposables = Enumerable
            .Range(0, 3)
            .Select(_ => Disposable.Create(() => count++))
            .ToArray();

        var merged = Disposable.Merge(disposables);
        merged.Dispose();

        Assert.Equal(3, count);
    }

    [Fact]
    public void Merge_DisposesInOrder()
    {
        var order = new List<int>();
        var disposables = Enumerable
            .Range(0, 3)
            .Select(i => Disposable.Create(() => order.Add(i)))
            .ToArray();

        Disposable.Merge(disposables).Dispose();

        Assert.Equal([0, 1, 2], order);
    }
}
