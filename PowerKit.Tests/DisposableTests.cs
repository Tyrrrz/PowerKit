using FluentAssertions;
using PowerKit;

namespace PowerKit.Tests;

public class DisposableTests
{
    [Fact]
    public void Null_Test()
    {
        // Act & assert
        Disposable.Null.Dispose();
    }

    [Fact]
    public void Create_Test()
    {
        // Arrange
        var invoked = false;

        // Act
        var disposable = Disposable.Create(() => invoked = true);
        disposable.Dispose();

        // Assert
        invoked.Should().BeTrue();
    }

    [Fact]
    public void Create_NotDisposed_Test()
    {
        // Arrange
        var invoked = false;

        // Act
        Disposable.Create(() => invoked = true);

        // Assert
        invoked.Should().BeFalse();
    }

    [Fact]
    public void Merge_Test()
    {
        // Arrange
        var count = 0;
        var disposables = Enumerable
            .Range(0, 3)
            .Select(_ => Disposable.Create(() => count++))
            .ToArray();

        // Act
        Disposable.Merge(disposables).Dispose();

        // Assert
        count.Should().Be(3);
    }

    [Fact]
    public void Merge_Order_Test()
    {
        // Arrange
        var order = new List<int>();
        var disposables = Enumerable
            .Range(0, 3)
            .Select(i => Disposable.Create(() => order.Add(i)))
            .ToArray();

        // Act
        Disposable.Merge(disposables).Dispose();

        // Assert
        order.Should().Equal(0, 1, 2);
    }
}
