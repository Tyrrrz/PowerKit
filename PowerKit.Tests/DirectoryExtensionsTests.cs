using System.IO;
using FluentAssertions;
using PowerKit;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class DirectoryExtensionsTests
{
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
}
