using System.Reflection;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class AssemblyExtensionsTests
{
    [Fact]
    public void TryGetVersionString_Test()
    {
        // Arrange
        var assembly = typeof(AssemblyExtensionsTests).Assembly;

        // Act
        var version = assembly.TryGetVersionString();

        // Assert
        version.Should().NotBeNullOrWhiteSpace();
    }
}
