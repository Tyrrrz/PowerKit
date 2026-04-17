using FluentAssertions;
using Gress;
using PowerKit;
using Xunit;

namespace PowerKit.Tests;

public class ProgressMuxerTests
{
    [Fact]
    public void CreateInput_Test()
    {
        // Arrange
        var progress = new ProgressCollector<double>();
        var muxer = new ProgressMuxer(progress);
        var input = muxer.CreateInput();

        // Act
        input.Report(0.5);
        input.Report(1.0);

        // Assert
        progress.GetValues().Should().Equal(0.5, 1.0);
    }

    [Fact]
    public void CreateInput_Weight_Test()
    {
        // Arrange
        var progress = new ProgressCollector<double>();
        var muxer = new ProgressMuxer(progress);
        var input = muxer.CreateInput(weight: 0.5);

        // Act
        input.Report(0.5);
        input.Report(1.0);

        // Assert
        progress.GetValues().Should().Equal(0.25, 0.5);
    }

    [Fact]
    public void CreateInput_MultipleInputs_Test()
    {
        // Arrange
        var progress = new ProgressCollector<double>();
        var muxer = new ProgressMuxer(progress);
        var input1 = muxer.CreateInput(weight: 0.6);
        var input2 = muxer.CreateInput(weight: 0.4);

        // Act
        input1.Report(1.0);
        input2.Report(1.0);

        // Assert
        var values = progress.GetValues();
        values[^1].Should().BeApproximately(1.0, 1e-10);
    }
}
