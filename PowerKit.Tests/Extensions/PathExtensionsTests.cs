using System.IO;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class PathExtensionsTests
{
    [Fact]
    public void GetInvalidFileNameChars_Test()
    {
        // Act & assert
        Path.GetInvalidFileNameChars(true).Should().Contain('/');
        Path.GetInvalidFileNameChars(true).Should().Contain('\\');
        Path.GetInvalidFileNameChars(true).Should().Contain('\0');
        Path.GetInvalidFileNameChars(true).Should().Contain('\x01');
        Path.GetInvalidFileNameChars(false).Should().BeEquivalentTo(Path.GetInvalidFileNameChars());
    }

    [Fact]
    public void GetInvalidPathChars_Test()
    {
        // Act & assert
        Path.GetInvalidPathChars(true).Should().Contain('\0');
        Path.GetInvalidPathChars(true).Should().Contain('|');
        Path.GetInvalidPathChars(true).Should().NotContain('/');
        Path.GetInvalidPathChars(true).Should().NotContain('\\');
        Path.GetInvalidPathChars(false).Should().BeEquivalentTo(Path.GetInvalidPathChars());
    }

    [Fact]
    public void AreEqual_Test()
    {
        // Act & assert
        Path.AreEqual(null, null).Should().BeTrue();
        Path.AreEqual(null, "/foo").Should().BeFalse();
        Path.AreEqual("/foo", null).Should().BeFalse();
        Path.AreEqual("/foo/bar", "/foo/bar").Should().BeTrue();
        Path.AreEqual("/foo/bar", "/foo/bar/").Should().BeTrue();
        Path.AreEqual("/foo/./bar", "/foo/bar").Should().BeTrue();
        Path.AreEqual("/foo/baz/../bar", "/foo/bar").Should().BeTrue();
        Path.AreEqual("/foo/bar", "/foo/baz").Should().BeFalse();
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
