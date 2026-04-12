using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class ProcessExtensionsTests
{
    [Fact]
    public void IsRunning_Running_Test()
    {
        // Act & assert
        Process.IsRunning(Environment.ProcessId).Should().BeTrue();
    }

    [Fact]
    public async Task IsRunning_NotRunning_Test()
    {
        // Arrange
        using var process = Process.Start(new ProcessStartInfo("dotnet", "--version")
        {
            RedirectStandardOutput = true
        })!;

        await process.WaitForExitAsync();
        var processId = process.Id;

        // Act & assert
        Process.IsRunning(processId).Should().BeFalse();
    }
}
