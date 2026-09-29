using System.Drawing;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class ColorExtensionsTests
{
    [Fact]
    public void FromHsv_Test()
    {
        // Act & assert
        Color.FromHsv(321, 0.71, 1).Should().Be(Color.FromArgb(255, 255, 74, 192));
        Color.FromHsv(0, 0, 0).Should().Be(Color.FromArgb(255, 0, 0, 0));
        Color.FromHsv(0, 0, 1).Should().Be(Color.FromArgb(255, 255, 255, 255));
        Color.FromHsv(0, 1, 1).Should().Be(Color.FromArgb(255, 255, 0, 0));
        Color.FromHsv(120, 1, 1).Should().Be(Color.FromArgb(255, 0, 255, 0));
        Color.FromHsv(240, 1, 1).Should().Be(Color.FromArgb(255, 0, 0, 255));
        Color.FromHsv(360, 1, 1).Should().Be(Color.FromArgb(255, 255, 0, 0));
        Color.FromHsv(-240, 1, 1).Should().Be(Color.FromArgb(255, 0, 255, 0));
    }

    [Fact]
    public void FromAhsv_Test()
    {
        // Act & assert
        Color.FromAhsv(255, 321, 0.71, 1).Should().Be(Color.FromArgb(255, 255, 74, 192));
        Color.FromAhsv(255, 0, 0, 0).Should().Be(Color.FromArgb(255, 0, 0, 0));
        Color.FromAhsv(255, 0, 0, 1).Should().Be(Color.FromArgb(255, 255, 255, 255));
        Color.FromAhsv(255, 0, 1, 1).Should().Be(Color.FromArgb(255, 255, 0, 0));
        Color.FromAhsv(255, 120, 1, 1).Should().Be(Color.FromArgb(255, 0, 255, 0));
        Color.FromAhsv(255, 240, 1, 1).Should().Be(Color.FromArgb(255, 0, 0, 255));
        Color.FromAhsv(255, 360, 1, 1).Should().Be(Color.FromArgb(255, 255, 0, 0));
        Color.FromAhsv(255, -240, 1, 1).Should().Be(Color.FromArgb(255, 0, 255, 0));
        Color.FromAhsv(128, 0, 1, 1).Should().Be(Color.FromArgb(128, 255, 0, 0));
    }

    [Fact]
    public void ToHexString_Test()
    {
        // Act & assert
        Color.FromArgb(255, 0x12, 0x34, 0x56).ToHexString().Should().Be("#123456");
        Color.FromArgb(128, 0xff, 0x00, 0x00).ToHexString().Should().Be("#FF0000");
        Color.FromArgb(255, 0x00, 0x00, 0x00).ToHexString().Should().Be("#000000");
        Color.FromArgb(255, 0xff, 0xff, 0xff).ToHexString().Should().Be("#FFFFFF");
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

    [Fact]
    public void WithFullAlpha_Test()
    {
        // Act & assert
        Color.FromArgb(0, 0x12, 0x34, 0x56).WithFullAlpha().A.Should().Be(255);
        Color.FromArgb(0, 0x12, 0x34, 0x56).WithFullAlpha().R.Should().Be(0x12);
        Color.FromArgb(0, 0x12, 0x34, 0x56).WithFullAlpha().G.Should().Be(0x34);
        Color.FromArgb(0, 0x12, 0x34, 0x56).WithFullAlpha().B.Should().Be(0x56);
    }
}
