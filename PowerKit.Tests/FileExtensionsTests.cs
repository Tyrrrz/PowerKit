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

    [Fact]
    public void ContainsBytes_EmptyPattern_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, [1, 2, 3]);

        // Act
        var result = File.ContainsBytes(tempFile.Path, []);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ContainsBytes_EmptyPattern_EmptyFile_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();

        // Act
        var result = File.ContainsBytes(tempFile.Path, []);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ContainsBytes_Found_AtStart_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, [1, 2, 3, 4, 5]);

        // Act
        var result = File.ContainsBytes(tempFile.Path, [1, 2, 3]);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ContainsBytes_Found_AtMiddle_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, [1, 2, 3, 4, 5]);

        // Act
        var result = File.ContainsBytes(tempFile.Path, [2, 3, 4]);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ContainsBytes_Found_AtEnd_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, [1, 2, 3, 4, 5]);

        // Act
        var result = File.ContainsBytes(tempFile.Path, [3, 4, 5]);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ContainsBytes_Found_ExactMatch_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, [1, 2, 3]);

        // Act
        var result = File.ContainsBytes(tempFile.Path, [1, 2, 3]);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ContainsBytes_NotFound_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, [1, 2, 3, 4, 5]);

        // Act
        var result = File.ContainsBytes(tempFile.Path, [9, 8, 7]);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ContainsBytes_NotFound_EmptyFile_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();

        // Act
        var result = File.ContainsBytes(tempFile.Path, [1, 2, 3]);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ContainsBytes_NotFound_PatternLongerThanFile_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, [1, 2]);

        // Act
        var result = File.ContainsBytes(tempFile.Path, [1, 2, 3]);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ContainsBytes_Found_SpanningChunkBoundary_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();

        // Write a file large enough to span the default 4096-byte buffer boundary
        var data = new byte[4097];
        data[4094] = 0xAA;
        data[4095] = 0xBB;
        data[4096] = 0xCC;
        File.WriteAllBytes(tempFile.Path, data);

        // Act
        var result = File.ContainsBytes(tempFile.Path, [0xAA, 0xBB, 0xCC]);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ContainsBytes_SpanOverload_Found_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, [1, 2, 3, 4, 5]);

        // Act
        var result = File.ContainsBytes(tempFile.Path, new ReadOnlySpan<byte>([2, 3, 4]));

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ContainsBytes_SpanOverload_NotFound_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();
        File.WriteAllBytes(tempFile.Path, [1, 2, 3, 4, 5]);

        // Act
        var result = File.ContainsBytes(tempFile.Path, new ReadOnlySpan<byte>([9, 8, 7]));

        // Assert
        result.Should().BeFalse();
    }
}
