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
        owner.Memory.Length.Should().Be(16);
    }

    [Fact]
    public void RentOwner_Dispose_Test()
    {
        // Arrange
        var pool = new TrackingArrayPool();

        // Act & assert
        using (var owner = pool.RentOwner(16))
        {
            owner.Memory.Length.Should().Be(16);
            pool.LastRentedArray.Should().NotBeNull();
        }

        pool.ReturnCallCount.Should().Be(1);
        pool.LastReturnedArray.Should().BeSameAs(pool.LastRentedArray);

        // Should re-rent the returned array after the previous owner was disposed
        using var owner2 = pool.RentOwner(16);
        owner2.Memory.Length.Should().Be(16);
        pool.LastRentedArray.Should().BeSameAs(pool.LastReturnedArray);
    }

    [Fact]
    public void RentOwner_MemoryAfterDispose_Test()
    {
        // Arrange
        var pool = ArrayPool<byte>.Shared;
        var owner = pool.RentOwner(16);

        // Act
        owner.Dispose();

        // Assert
        Assert.Throws<ObjectDisposedException>(() => owner.Memory);
    }

    private sealed class TrackingArrayPool : ArrayPool<byte>
    {
        private byte[]? available;

        public byte[]? LastRentedArray { get; private set; }

        public byte[]? LastReturnedArray { get; private set; }

        public int ReturnCallCount { get; private set; }

        public override byte[] Rent(int minimumLength)
        {
            var array = this.available is { Length: >= 16 }
                ? this.available
                : new byte[16];

            this.available = null;
            this.LastRentedArray = array;
            return array;
        }

        public override void Return(byte[] array, bool clearArray = false)
        {
            this.ReturnCallCount++;
            this.LastReturnedArray = array;
            this.available = array;
        }
    }
}
