using System.IO;
using FluentAssertions;
using PowerKit;
using Xunit;

namespace PowerKit.Tests;

public class TempFileTests
{
    [Fact]
    public void Create_Test()
    {
        // Act
        using var tempFile = TempFile.Create();

        // Assert
        File.Exists(tempFile.Path).Should().BeTrue();
        tempFile.Path.Should().EndWith(".tmp");
    }

    [Fact]
    public void Dispose_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();

        // Act
        tempFile.Dispose();

        // Assert
        File.Exists(tempFile.Path).Should().BeFalse();
    }

    [Fact]
    public void Dispose_AlreadyDeleted_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();
        File.Delete(tempFile.Path);

        // Act & assert
        var act = tempFile.Dispose;
        act.Should().NotThrow();
    }
}
