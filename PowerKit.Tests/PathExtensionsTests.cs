using System.IO;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class PathExtensionsTests
{
    [Fact]
    public void EscapeFileName_Test()
    {
        // Act & assert
        Path.EscapeFileName("hello world.txt").Should().Be("hello world.txt");
        Path.EscapeFileName("a/b").Should().Be("a_b");
        Path.EscapeFileName("a\\b").Should().Be("a_b");
        Path.EscapeFileName("C:drive").Should().Be("C_drive");
        Path.EscapeFileName("a\0b/c\\d:e*f?g\"h<i").Should().Be("a_b_c_d_e_f_g_h_i");
        Path.EscapeFileName("hello...").Should().Be("hello");
        Path.EscapeFileName("hello   ").Should().Be("hello");
        Path.EscapeFileName("hello. . ").Should().Be("hello");
        Path.EscapeFileName("hello.world").Should().Be("hello.world");
        Path.EscapeFileName("...").Should().Be("");
        Path.EscapeFileName("").Should().Be("");
    }
}
