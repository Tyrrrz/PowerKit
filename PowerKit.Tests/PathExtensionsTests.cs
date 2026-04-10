using System.IO;
using FluentAssertions;
using PowerKit.Extensions;

namespace PowerKit.Tests;

public class PathExtensionsTests
{
    [Fact]
    public void EscapeFileName_Test()
    {
        // Act & assert
        Path.EscapeFileName("hello world.txt").Should().Be("hello world.txt");
    }

    [Fact]
    public void EscapeFileName_ForwardSlash_Test()
    {
        // Act & assert
        Path.EscapeFileName("a/b").Should().Be("a_b");
    }

    [Fact]
    public void EscapeFileName_Backslash_Test()
    {
        // Act & assert
        Path.EscapeFileName("a\\b").Should().Be("a_b");
    }

    [Fact]
    public void EscapeFileName_Colon_Test()
    {
        // Act & assert
        Path.EscapeFileName("C:drive").Should().Be("C_drive");
    }

    [Fact]
    public void EscapeFileName_AllInvalidChars_Test()
    {
        // Act & assert
        Path.EscapeFileName("a\0b/c\\d:e*f?g\"h<i").Should().Be("a_b_c_d_e_f_g_h_i");
    }

    [Fact]
    public void EscapeFileName_TrailingDots_Test()
    {
        // Act & assert
        Path.EscapeFileName("hello...").Should().Be("hello");
    }

    [Fact]
    public void EscapeFileName_TrailingWhitespace_Test()
    {
        // Act & assert
        Path.EscapeFileName("hello   ").Should().Be("hello");
    }

    [Fact]
    public void EscapeFileName_TrailingDotsAndWhitespace_Test()
    {
        // Act & assert
        Path.EscapeFileName("hello. . ").Should().Be("hello");
    }

    [Fact]
    public void EscapeFileName_DotsInMiddle_Test()
    {
        // Act & assert
        Path.EscapeFileName("hello.world").Should().Be("hello.world");
    }

    [Fact]
    public void EscapeFileName_OnlyDots_Test()
    {
        // Act & assert
        Path.EscapeFileName("...").Should().Be("");
    }

    [Fact]
    public void EscapeFileName_Empty_Test()
    {
        // Act & assert
        Path.EscapeFileName("").Should().Be("");
    }
}
