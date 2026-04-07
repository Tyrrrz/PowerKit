using PowerKit.Extensions;

namespace PowerKit.Tests;

public class TimeSpanExtensionsTests
{
    [Fact]
    public void Clamp_ValueWithinRange_ReturnsValue()
    {
        var value = TimeSpan.FromSeconds(5);
        var result = value.Clamp(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10));
        Assert.Equal(value, result);
    }

    [Fact]
    public void Clamp_ValueBelowMin_ReturnsMin()
    {
        var min = TimeSpan.FromSeconds(3);
        var result = TimeSpan.FromSeconds(1).Clamp(min, TimeSpan.FromSeconds(10));
        Assert.Equal(min, result);
    }

    [Fact]
    public void Clamp_ValueAboveMax_ReturnsMax()
    {
        var max = TimeSpan.FromSeconds(7);
        var result = TimeSpan.FromSeconds(20).Clamp(TimeSpan.FromSeconds(1), max);
        Assert.Equal(max, result);
    }

    [Fact]
    public void Clamp_ValueEqualToMin_ReturnsMin()
    {
        var min = TimeSpan.FromSeconds(3);
        var result = min.Clamp(min, TimeSpan.FromSeconds(10));
        Assert.Equal(min, result);
    }

    [Fact]
    public void Clamp_ValueEqualToMax_ReturnsMax()
    {
        var max = TimeSpan.FromSeconds(10);
        var result = max.Clamp(TimeSpan.FromSeconds(1), max);
        Assert.Equal(max, result);
    }
}
