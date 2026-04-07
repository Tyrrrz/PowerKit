using System.IO;
using PowerKit.Extensions;

namespace PowerKit.Tests;

public class PathExtensionsTests
{
    [Fact]
    public void EscapeFileName_ValidName_ReturnsUnchanged()
    {
        Assert.Equal("hello world.txt", Path.EscapeFileName("hello world.txt"));
    }

    [Fact]
    public void EscapeFileName_ReplacesForwardSlash()
    {
        Assert.Equal("a_b", Path.EscapeFileName("a/b"));
    }

    [Fact]
    public void EscapeFileName_ReplacesBackslash()
    {
        Assert.Equal("a_b", Path.EscapeFileName("a\\b"));
    }

    [Fact]
    public void EscapeFileName_ReplacesColon()
    {
        Assert.Equal("C_drive", Path.EscapeFileName("C:drive"));
    }

    [Fact]
    public void EscapeFileName_ReplacesAllInvalidChars()
    {
        Assert.Equal("a_b_c_d_e_f_g_h_i", Path.EscapeFileName("a\0b/c\\d:e*f?g\"h<i"));
    }

    [Fact]
    public void EscapeFileName_StripsTrailingDots()
    {
        Assert.Equal("hello", Path.EscapeFileName("hello..."));
    }

    [Fact]
    public void EscapeFileName_DotsInMiddle_PreservesDots()
    {
        Assert.Equal("hello.world", Path.EscapeFileName("hello.world"));
    }

    [Fact]
    public void EscapeFileName_OnlyDots_ReturnsEmpty()
    {
        Assert.Equal("", Path.EscapeFileName("..."));
    }

    [Fact]
    public void EscapeFileName_EmptyString_ReturnsEmpty()
    {
        Assert.Equal("", Path.EscapeFileName(""));
    }
}
