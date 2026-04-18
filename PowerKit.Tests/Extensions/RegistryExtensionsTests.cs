using System;
using System.Runtime.Versioning;
using FluentAssertions;
using Microsoft.Win32;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class RegistryExtensionsTests
{
    [SkippableTheory]
    [InlineData(RegistryHive.ClassesRoot, "HKCR")]
    [InlineData(RegistryHive.CurrentUser, "HKCU")]
    [InlineData(RegistryHive.LocalMachine, "HKLM")]
    [InlineData(RegistryHive.Users, "HKU")]
    [InlineData(RegistryHive.PerformanceData, "HKPD")]
    [InlineData(RegistryHive.CurrentConfig, "HKCC")]
    [SupportedOSPlatform("windows")]
    public void Moniker_Test(RegistryHive hive, string expectedMoniker)
    {
        Skip.IfNot(OperatingSystem.IsWindows());

        // Act & assert
        hive.Moniker.Should().Be(expectedMoniker);
    }

    [SkippableFact]
    [SupportedOSPlatform("windows")]
    public void OpenKey_Test()
    {
        Skip.IfNot(OperatingSystem.IsWindows());

        // Act
        using var key = RegistryHive.CurrentUser.OpenKey(RegistryView.Default);

        // Assert
        key.Should().NotBeNull();
        key.Name.Should().Be(Registry.CurrentUser.Name);
    }

    [SkippableFact]
    [SupportedOSPlatform("windows")]
    public void ContainsSubKey_Exists_Test()
    {
        Skip.IfNot(OperatingSystem.IsWindows());

        // Arrange
        using var key = Registry.CurrentUser.OpenSubKey("Software", true)!;
        var subKeyName = $"PowerKit.Tests.{Guid.NewGuid():N}";

        try
        {
            using var subKey = key.CreateSubKey(subKeyName);

            // Act & assert
            key.ContainsSubKey(subKeyName).Should().BeTrue();
        }
        finally
        {
            key.DeleteSubKeyTree(subKeyName, false);
        }
    }

    [SkippableFact]
    [SupportedOSPlatform("windows")]
    public void ContainsSubKey_NotExists_Test()
    {
        Skip.IfNot(OperatingSystem.IsWindows());

        // Arrange
        using var key = Registry.CurrentUser.OpenSubKey("Software", false)!;

        // Act & assert
        key.ContainsSubKey("this-sub-key-definitely-does-not-exist").Should().BeFalse();
    }
}
