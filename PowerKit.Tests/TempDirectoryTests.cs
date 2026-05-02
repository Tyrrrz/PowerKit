using System.IO;
using FluentAssertions;
using Xunit;

namespace PowerKit.Tests;

public class TempDirectoryTests
{
    [Fact]
    public void GeneratePath_Test()
    {
        // Act
        var path = TempDirectory.GeneratePath();

        // Assert
        path.Should().NotBeNullOrEmpty();
        Directory.Exists(path).Should().BeFalse();
    }

    [Fact]
    public void Create_Test()
    {
        // Act
        using var tempDir = TempDirectory.Create();

        // Assert
        Directory.Exists(tempDir.Path).Should().BeTrue();
    }

    [Fact]
    public void Create_WithoutPreCreate_Test()
    {
        // Act
        using var tempDir = TempDirectory.Create(false);

        // Assert
        Directory.Exists(tempDir.Path).Should().BeFalse();
    }

    [Fact]
    public void Dispose_Test()
    {
        // Arrange
        using var tempDir = TempDirectory.Create();
        File.WriteAllText(Path.Combine(tempDir.Path, "test.txt"), "test");

        // Act
        tempDir.Dispose();

        // Assert
        Directory.Exists(tempDir.Path).Should().BeFalse();
    }

    [Fact]
    public void Dispose_AlreadyDeleted_Test()
    {
        // Arrange
        using var tempDir = TempDirectory.Create();
        Directory.Delete(tempDir.Path);

        // Act & assert
        var act = tempDir.Dispose;
        act.Should().NotThrow();
    }
}
