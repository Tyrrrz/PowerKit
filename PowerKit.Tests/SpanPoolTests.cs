using System;
using FluentAssertions;
using Xunit;

namespace PowerKit.Tests;

public class SpanPoolTests
{
    [Fact]
    public void Rent_Test()
    {
        // Arrange
        var pool = SpanPool<byte>.Shared;

        // Act
        using var owner = pool.Rent(16);

        // Assert
        owner.Span.Length.Should().Be(16);
    }

    [Fact]
    public void Rent_Dispose_Test()
    {
        // Arrange
        var pool = SpanPool<byte>.Shared;
        var owner = pool.Rent(16);

        // Act
        owner.Dispose();
        var act = () => owner.Span.Length;

        // Assert
        act.Should().Throw<ObjectDisposedException>();
    }
}
