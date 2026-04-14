using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using PowerKit;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class AssemblyExtensionsTests
{
    private static readonly Assembly ThisAssembly = typeof(AssemblyExtensionsTests).Assembly;

    // The embedded resource name follows the default MSBuild convention:
    // <RootNamespace>.<RelativePath> with path separators replaced by dots.
    private const string ResourceName = "PowerKit.Tests.TestData.TestResource.txt";

    [Fact]
    public void TryGetVersionString_Test()
    {
        // Act
        var version = ThisAssembly.TryGetVersionString();

        // Assert
        version.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void GetManifestResourceString_Test()
    {
        // Act
        var content = ThisAssembly.GetManifestResourceString(ResourceName);

        // Assert
        content.Should().Be("hello");
    }

    [Fact]
    public async Task GetManifestResourceStringAsync_Test()
    {
        // Act
        var content = await ThisAssembly.GetManifestResourceStringAsync(ResourceName);

        // Assert
        content.Should().Be("hello");
    }

    [Fact]
    public void ExtractManifestResource_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();

        // Act
        ThisAssembly.ExtractManifestResource(ResourceName, tempFile.Path);

        // Assert
        File.ReadAllText(tempFile.Path).Should().Be("hello");
    }

    [Fact]
    public async Task ExtractManifestResourceAsync_Test()
    {
        // Arrange
        using var tempFile = TempFile.Create();

        // Act
        await ThisAssembly.ExtractManifestResourceAsync(ResourceName, tempFile.Path);

        // Assert
        (await File.ReadAllTextAsync(tempFile.Path)).Should().Be("hello");
    }
}
