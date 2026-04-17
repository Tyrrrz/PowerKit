using System;
using System.Buffers;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class ArrayPoolExtensionsTests
{
    [Fact]
    public void RentOwner_Test()
    {
        // Arrange
        var pool = ArrayPool<byte>.Shared;

        // Act
        using var owner = pool.RentOwner(16);

        // Assert
        owner.Array.Should().NotBeNull();
        owner.Length.Should().Be(16);
    }

    [Fact]
    public void RentOwner_Dispose_Test()
    {
        // Arrange
        var pool = ArrayPool<byte>.Shared;
        var owner = pool.RentOwner(16);

        // Act
        owner.Dispose();
        var act = () => owner.Array;

        // Assert
        act.Should().Throw<ObjectDisposedException>();
    }
}
