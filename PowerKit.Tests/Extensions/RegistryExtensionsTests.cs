using System;
using System.Runtime.Versioning;
using FluentAssertions;
using Microsoft.Win32;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class RegistryExtensionsTests
{
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
