using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using PowerKit;
using Xunit;

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
        var disposable = Disposable.Create(() => invoked = true);

        // Act & assert
        invoked.Should().BeFalse();
        disposable.Dispose();
        invoked.Should().BeTrue();
    }

    [Fact]
    public void Merge_Test()
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
