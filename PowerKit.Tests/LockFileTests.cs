#nullable enable
using System.IO;
using FluentAssertions;
using PowerKit;
using Xunit;

namespace PowerKit.Tests;

public class LockFileTests
{
    [Fact]
    public void TryAcquire_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();

        // Act
        using var lockFile = LockFile.TryAcquire(tempFile.Path);

        // Assert
        lockFile.Should().NotBeNull();
        File.Exists(tempFile.Path).Should().BeTrue();
    }

    [Fact]
    public void TryAcquire_AlreadyLocked_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();
        using var lockFile = LockFile.TryAcquire(tempFile.Path);

        // Act
        using var lockFile2 = LockFile.TryAcquire(tempFile.Path);

        // Assert
        lockFile2.Should().BeNull();
    }

    [Fact]
    public void Dispose_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();
        var lockFile = LockFile.TryAcquire(tempFile.Path);

        // Act
        lockFile!.Dispose();

        // Assert: lock can be reacquired after disposal
        using var lockFile2 = LockFile.TryAcquire(tempFile.Path);
        lockFile2.Should().NotBeNull();
    }
}
