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
    public void CheckWriteAccess_WritableDirectory_Test()
    {
        // Arrange
        using var tempDir = TempDirectory.Create();

        // Act
        var result = Directory.CheckWriteAccess(tempDir.Path);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void CheckWriteAccess_ReadOnlyDirectory_Test()
    {
        // FileAttributes.ReadOnly removes write bits on Unix but has no effect on directories on Windows
        if (OperatingSystem.IsWindows() || Environment.IsPrivilegedProcess)
        {
            return;
        }

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
}
