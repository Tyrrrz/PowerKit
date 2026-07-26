using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace PowerKit.Tests;

public class DisposableTests
{
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
    public void Create_Idempotent_Test()
    {
        // Arrange
        var count = 0;
        var disposable = Disposable.Create(() => count++);

        // Act
        disposable.Dispose();
        disposable.Dispose();
        disposable.Dispose();

        // Assert
        count.Should().Be(1);
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

    [Fact]
    public void Merge_Exception_Test()
    {
        // Arrange
        var disposed = new List<int>();
        var disposables = new[]
        {
            Disposable.Create(() => disposed.Add(0)),
            Disposable.Create(() =>
            {
                disposed.Add(1);
                throw new InvalidOperationException("fail");
            }),
            Disposable.Create(() => disposed.Add(2)),
        };

        // Act
        var ex = Assert.Throws<AggregateException>(() => Disposable.Merge(disposables).Dispose());

        // Assert
        disposed.Should().Equal(0, 1, 2);
        ex.InnerExceptions.Should().ContainSingle();
        ex.InnerExceptions[0].Should().BeOfType<InvalidOperationException>();
    }
}
