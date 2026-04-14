using System.IO;
using FluentAssertions;
using PowerKit;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class DirectoryExtensionsTests
{
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
        var result = Directory.TryDelete(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));

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
    public void TryDelete_NonEmpt_Recursive_Test()
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
