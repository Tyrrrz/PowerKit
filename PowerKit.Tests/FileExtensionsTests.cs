using System;
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
    public void CheckWriteAccess_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();

        // Act
        var result = File.CheckWriteAccess(tempFile.Path);

        // Assert
        result.Should().BeTrue();
    }

    [SkippableFact]
    public void CheckWriteAccess_ReadOnly_Test()
    {
        // Privileged processes can write to read-only files on Unix
        Skip.If(Environment.IsPrivilegedProcess);

        // Arrange
        using var tempFile = TempFile.Create();
        File.SetAttributes(
            tempFile.Path,
            File.GetAttributes(tempFile.Path) | FileAttributes.ReadOnly
        );

        try
        {
            // Act
            var result = File.CheckWriteAccess(tempFile.Path);

            // Assert
            result.Should().BeFalse();
        }
        finally
        {
            File.SetAttributes(
                tempFile.Path,
                File.GetAttributes(tempFile.Path) & ~FileAttributes.ReadOnly
            );
        }
    }

    [Fact]
    public void CheckWriteAccess_NonExistent_Test()
    {
        // Arrange
        using var tempDir = TempDirectory.Create();
        var path = Path.Combine(tempDir.Path, "new-file.txt");

        // Act
        var result = File.CheckWriteAccess(path);

        // Assert
        result.Should().BeTrue();
        File.Exists(path).Should().BeFalse();
    }

    [Fact]
    public void Contains_Found_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, [1, 2, 3, 4, 5]);

        // Act
        var result = File.Contains(tempFile.Path, [2, 3, 4]);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Contains_NotFound_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, [1, 2, 3, 4, 5]);

        // Act
        var result = File.Contains(tempFile.Path, [9, 8, 7]);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void TryDelete_Test()
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
    public void TryDelete_NonExisting_Test()
    {
        // Act
        var result = File.TryDelete(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));

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
        var bytes = File.ReadAllBytes(tempFile.Path, 2L);

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
        var bytes = File.ReadAllBytes(tempFile.Path, 5L);

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
        var bytes = File.ReadAllBytes(tempFile.Path, 10L);
        bytes.Should().BeEmpty();
    }

    [Fact]
    public void ReadAllBytes_WithOffsetAndLength_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, [1, 2, 3, 4, 5]);

        // Act
        var bytes = File.ReadAllBytes(tempFile.Path, 1L, 3);

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
        var bytes = await File.ReadAllBytesAsync(tempFile.Path, 2L);

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
        var bytes = await File.ReadAllBytesAsync(tempFile.Path, 5L);

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
        var bytes = await File.ReadAllBytesAsync(tempFile.Path, 10L);
        bytes.Should().BeEmpty();
    }

    [Fact]
    public async Task ReadAllBytesAsync_WithOffsetAndLength_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, [1, 2, 3, 4, 5]);

        // Act
        var bytes = await File.ReadAllBytesAsync(tempFile.Path, 1L, 3);

        // Assert
        bytes.Should().Equal(2, 3, 4);
    }
}
