using System;
using System.IO;
using FluentAssertions;
using PowerKit;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class DirectoryExtensionsTests
{
    [Fact]
    public void CheckWriteAccess_Test()
    {
        // Arrange
        using var tempDir = TempDirectory.Create();

        // Act
        var result = Directory.CheckWriteAccess(tempDir.Path);

        // Assert
        result.Should().BeTrue();
    }

    [SkippableFact]
    public void CheckWriteAccess_ReadOnly_Test()
    {
        // FileAttributes.ReadOnly removes write bits on Unix but has no effect on directories on Windows
        Skip.If(OperatingSystem.IsWindows() || Environment.IsPrivilegedProcess);

        // Arrange
        using var tempDir = TempDirectory.Create();
        var dirInfo = new DirectoryInfo(tempDir.Path);
        dirInfo.Attributes |= FileAttributes.ReadOnly;

        try
        {
            // Act
            var result = Directory.CheckWriteAccess(tempDir.Path);

            // Assert
            result.Should().BeFalse();
        }
        finally
        {
            dirInfo.Attributes &= ~FileAttributes.ReadOnly;
        }
    }

    [Fact]
    public void Reset_Test()
    {
        // Arrange
        using var tempDir = TempDirectory.Create();
        File.WriteAllText(Path.Combine(tempDir.Path, "test.txt"), "test");

        // Act
        Directory.Reset(tempDir.Path);

        // Assert
        Directory.Exists(tempDir.Path).Should().BeTrue();
        Directory.GetFileSystemEntries(tempDir.Path).Should().BeEmpty();
    }

    [Fact]
    public void Reset_NonExistent_Test()
    {
        // Arrange
        using var tempDir = TempDirectory.Create();
        var dirPath = Path.Combine(tempDir.Path, "nonexistent");

        // Act
        Directory.Reset(dirPath);

        // Assert
        Directory.Exists(dirPath).Should().BeTrue();
        Directory.GetFileSystemEntries(dirPath).Should().BeEmpty();
    }

    [Fact]
    public void TryDelete_Test()
    {
        // Arrange
        using var tempDir = TempDirectory.Create();

        // Act
        var result = Directory.TryDelete(tempDir.Path);

        // Assert
        result.Should().BeTrue();
        Directory.Exists(tempDir.Path).Should().BeFalse();
    }

    [Fact]
    public void TryDelete_NonExisting_Test()
    {
        // Act
        var result = Directory.TryDelete(
            Path.Combine(Path.GetTempPath(), Path.GetRandomFileName())
        );

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void TryDelete_NonEmpty_Test()
    {
        // Arrange
        using var tempDir = TempDirectory.Create();
        File.WriteAllText(Path.Combine(tempDir.Path, "file.txt"), "hello");

        // Act
        var result = Directory.TryDelete(tempDir.Path, recursive: false);

        // Assert
        result.Should().BeFalse();
        Directory.Exists(tempDir.Path).Should().BeTrue();
    }

    [Fact]
    public void TryDelete_NonEmpty_Recursive_Test()
    {
        // Arrange
        using var tempDir = TempDirectory.Create();
        File.WriteAllText(Path.Combine(tempDir.Path, "file.txt"), "hello");

        // Act
        var result = Directory.TryDelete(tempDir.Path, recursive: true);

        // Assert
        result.Should().BeTrue();
        Directory.Exists(tempDir.Path).Should().BeFalse();
    }
}
