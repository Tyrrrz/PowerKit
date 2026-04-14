using System.IO;
using System.Linq;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class PathExtensionsTests
{
    [Fact]
    public void GetInvalidFileNameChars_Test()
    {
        // Act & assert
        Path.GetInvalidFileNameChars(crossPlatform: true).Should().Contain('/');
        Path.GetInvalidFileNameChars(crossPlatform: true).Should().Contain('\\');
        Path.GetInvalidFileNameChars(crossPlatform: true).Should().Contain('\0');
        Path.GetInvalidFileNameChars(crossPlatform: true).Should().Contain('\x01');
        Path.GetInvalidFileNameChars(crossPlatform: false)
            .Should()
            .BeEquivalentTo(Path.GetInvalidFileNameChars());
    }

    [Fact]
    public void GetInvalidPathChars_Test()
    {
        // Act & assert
        Path.GetInvalidPathChars(crossPlatform: true).Should().Contain('\0');
        Path.GetInvalidPathChars(crossPlatform: true).Should().Contain('|');
        Path.GetInvalidPathChars(crossPlatform: true).Should().NotContain('/');
        Path.GetInvalidPathChars(crossPlatform: true).Should().NotContain('\\');
        Path.GetInvalidPathChars(crossPlatform: false)
            .Should()
            .BeEquivalentTo(Path.GetInvalidPathChars());
    }

    [Fact]
    public void EscapeFileName_Test()
    {
        // Act & assert
        Path.EscapeFileName("hello world.txt").Should().Be("hello world.txt");
        Path.EscapeFileName("a/b").Should().Be("a_b");
        Path.EscapeFileName("a\\b").Should().Be("a_b");
        Path.EscapeFileName("C:drive").Should().Be("C_drive");
        Path.EscapeFileName("a\0b/c\\d:e*f?g\"h<i").Should().Be("a_b_c_d_e_f_g_h_i");
        Path.EscapeFileName("a\u0001b\u001Fc").Should().Be("a_b_c");
        Path.EscapeFileName("hello...").Should().Be("hello");
        Path.EscapeFileName("hello   ").Should().Be("hello");
        Path.EscapeFileName("hello. . ").Should().Be("hello");
        Path.EscapeFileName("hello.world").Should().Be("hello.world");
        Path.EscapeFileName("...").Should().Be("");
        Path.EscapeFileName("").Should().Be("");
    }
}
