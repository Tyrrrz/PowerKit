using System.Drawing;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class ColorExtensionsTests
{
    [Fact]
    public void ToHex_Test()
    {
        // Act & assert
        Color.FromArgb(255, 0x12, 0x34, 0x56).ToHex().Should().Be("#123456");
        Color.FromArgb(128, 0xff, 0x00, 0x00).ToHex().Should().Be("#FF0000");
        Color.FromArgb(255, 0x00, 0x00, 0x00).ToHex().Should().Be("#000000");
        Color.FromArgb(255, 0xff, 0xff, 0xff).ToHex().Should().Be("#FFFFFF");
    }

    [Fact]
    public void ToRgb_Test()
    {
        // Act & assert
        Color.FromArgb(255, 0x12, 0x34, 0x56).ToRgb().Should().Be(0x123456);
        Color.FromArgb(0, 0x12, 0x34, 0x56).ToRgb().Should().Be(0x123456);
        Color.FromArgb(255, 0x00, 0x00, 0x00).ToRgb().Should().Be(0x000000);
        Color.FromArgb(255, 0xff, 0xff, 0xff).ToRgb().Should().Be(0xffffff);
    }

    [Fact]
    public void WithAlpha_Test()
    {
        // Act & assert
        Color.FromArgb(255, 0x12, 0x34, 0x56).WithAlpha(128).A.Should().Be(128);
        Color.FromArgb(255, 0x12, 0x34, 0x56).WithAlpha(128).R.Should().Be(0x12);
        Color.FromArgb(255, 0x12, 0x34, 0x56).WithAlpha(128).G.Should().Be(0x34);
        Color.FromArgb(255, 0x12, 0x34, 0x56).WithAlpha(128).B.Should().Be(0x56);
        Color.FromArgb(0, 0xff, 0xff, 0xff).WithAlpha(255).A.Should().Be(255);
    }
}
