using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using PowerKit;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class FileExtensionsTests
{
    [Fact]
    public void TryDelete_ExistingFile_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();

        // Act
        var result = File.TryDelete(tempFile.Path);

        // Assert
        result.Should().BeTrue();
        File.Exists(tempFile.Path).Should().BeFalse();
    }

    [Fact]
    public void TryDelete_NonExistingFile_Test()
    {
        // Act
        var result = File.TryDelete(Path.GetTempFileName() + ".nonexistent");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task WriteAllZeroes_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();

        // Act
        File.WriteAllZeroes(tempFile.Path, 1024);

        // Assert
        var bytes = await File.ReadAllBytesAsync(tempFile.Path);
        bytes.Should().HaveCount(1024);
        bytes.Should().AllSatisfy(b => b.Should().Be(0));
    }

    [Fact]
    public void ReadAllBytes_WithOffset_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, [1, 2, 3, 4, 5]);

        // Act
        var bytes = File.ReadAllBytes(tempFile.Path, offset: 2L);

        // Assert
        bytes.Should().Equal(3, 4, 5);
    }

    [Fact]
    public void ReadAllBytes_WithOffset_AtEndOfFile_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, [1, 2, 3, 4, 5]);

        // Act
        var bytes = File.ReadAllBytes(tempFile.Path, offset: 5L);

        // Assert
        bytes.Should().BeEmpty();
    }

    [Fact]
    public void ReadAllBytes_WithOffset_PastEndOfFile_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, [1, 2, 3, 4, 5]);

        // Act & Assert
        var bytes = File.ReadAllBytes(tempFile.Path, offset: 10L);
        bytes.Should().BeEmpty();
    }

    [Fact]
    public void ReadAllBytes_WithOffsetAndLength_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, [1, 2, 3, 4, 5]);

        // Act
        var bytes = File.ReadAllBytes(tempFile.Path, offset: 1L, length: 3);

        // Assert
        bytes.Should().Equal(2, 3, 4);
    }

    [Fact]
    public async Task ReadAllBytesAsync_WithOffset_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, [1, 2, 3, 4, 5]);

        // Act
        var bytes = await File.ReadAllBytesAsync(tempFile.Path, offset: 2L);

        // Assert
        bytes.Should().Equal(3, 4, 5);
    }

    [Fact]
    public async Task ReadAllBytesAsync_WithOffset_AtEndOfFile_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, [1, 2, 3, 4, 5]);

        // Act
        var bytes = await File.ReadAllBytesAsync(tempFile.Path, offset: 5L);

        // Assert
        bytes.Should().BeEmpty();
    }

    [Fact]
    public async Task ReadAllBytesAsync_WithOffset_PastEndOfFile_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, [1, 2, 3, 4, 5]);

        // Act & Assert
        var bytes = await File.ReadAllBytesAsync(tempFile.Path, offset: 10L);
        bytes.Should().BeEmpty();
    }

    [Fact]
    public async Task ReadAllBytesAsync_WithOffsetAndLength_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, [1, 2, 3, 4, 5]);

        // Act
        var bytes = await File.ReadAllBytesAsync(tempFile.Path, offset: 1L, length: 3);

        // Assert
        bytes.Should().Equal(2, 3, 4);
    }
}
