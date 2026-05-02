using System;
using FluentAssertions;
using Gress;
using Xunit;

namespace PowerKit.Tests;

public class ProgressMuxerTests
{
    [Fact]
    public void CreateInput_MultipleInputs_Test()
    {
        // Arrange
        var progress = new ProgressCollector<double>();
        var muxer = new ProgressMuxer(progress);
        var input1 = muxer.CreateInput(3);
        var input2 = muxer.CreateInput(1);

        // Act
        input1.Report(1.0); // (3×1.0 + 1×0.0) / (3+1) = 0.75
        input2.Report(1.0); // (3×1.0 + 1×1.0) / (3+1) = 1.0

        // Assert
        var values = progress.GetValues();
        values[0].Should().BeApproximately(0.75, 1e-10);
        values[1].Should().BeApproximately(1.0, 1e-10);
    }

    [Fact]
    public void CreateInput_InvalidWeight_Test()
    {
        // Arrange
        var progress = new ProgressCollector<double>();
        var muxer = new ProgressMuxer(progress);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => muxer.CreateInput(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => muxer.CreateInput(double.NaN));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            muxer.CreateInput(double.PositiveInfinity)
        );
    }
}
