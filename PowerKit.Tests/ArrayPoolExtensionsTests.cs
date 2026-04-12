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
        owner.Memory.Length.Should().Be(16);
    }

    [Fact]
    public void RentOwner_ReturnsBufferOnDispose_Test()
    {
        // Arrange
        var pool = ArrayPool<byte>.Create(16, 1);

        // Act & assert
        using (var owner = pool.RentOwner(16))
        {
            owner.Memory.Length.Should().Be(16);
        }

        // Should be able to rent again after the previous owner was disposed
        using var owner2 = pool.RentOwner(16);
        owner2.Memory.Length.Should().Be(16);
    }
}
