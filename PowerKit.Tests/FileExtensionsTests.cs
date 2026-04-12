using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class FileExtensionsTests
{
    [Fact]
    public void WriteAllZeroes_Test()
    {
        // Arrange
        var path = Path.GetTempFileName();

        try
        {
            // Act
            File.WriteAllZeroes(path, 1024);

            // Assert
            var bytes = File.ReadAllBytes(path);
            bytes.Should().HaveCount(1024);
            bytes.Should().AllSatisfy(b => b.Should().Be(0));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void ReadAllBytes_WithOffset_Test()
    {
        // Arrange
        var path = Path.GetTempFileName();

        try
        {
            File.WriteAllBytes(path, [1, 2, 3, 4, 5]);

            // Act
            var bytes = File.ReadAllBytes(path, offset: 2);

            // Assert
            bytes.Should().Equal(3, 4, 5);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public async Task ReadAllBytesAsync_WithOffset_Test()
    {
        // Arrange
        var path = Path.GetTempFileName();

        try
        {
            File.WriteAllBytes(path, [1, 2, 3, 4, 5]);

            // Act
            var bytes = await File.ReadAllBytesAsync(path, offset: 2);

            // Assert
            bytes.Should().Equal(3, 4, 5);
        }
        finally
        {
            File.Delete(path);
        }
    }
}
