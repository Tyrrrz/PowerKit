using System;
using System.Diagnostics;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class ProcessExtensionsTests
{
    [Fact]
    public void IsRunning_CurrentProcess_Test()
    {
        // Act & assert
        Process.IsRunning(Environment.ProcessId).Should().BeTrue();
    }

    [Fact]
    public void IsRunning_ExitedProcess_Test()
    {
        // Arrange
        using var process = Process.Start(new ProcessStartInfo("dotnet", "--version")
        {
            RedirectStandardOutput = true
        })!;

        process.WaitForExit();
        var processId = process.Id;

        // Act & assert
        Process.IsRunning(processId).Should().BeFalse();
    }
}
