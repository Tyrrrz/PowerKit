using System;
using System.Runtime.Versioning;
using FluentAssertions;
using Microsoft.Win32;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class RegistryExtensionsTests
{
    [Fact]
    [SupportedOSPlatform("windows")]
    public void ContainsSubKey_Exists_Test()
    {
        if (!OperatingSystem.IsWindows())
            return;

        // Arrange
        using var key = Registry.CurrentUser.OpenSubKey("Software", false)!;

        // Act & assert
        key.ContainsSubKey("Microsoft").Should().BeTrue();
    }

    [Fact]
    [SupportedOSPlatform("windows")]
    public void ContainsSubKey_NotExists_Test()
    {
        if (!OperatingSystem.IsWindows())
            return;

        // Arrange
        using var key = Registry.CurrentUser.OpenSubKey("Software", false)!;

        // Act & assert
        key.ContainsSubKey("this-sub-key-definitely-does-not-exist").Should().BeFalse();
    }
}
