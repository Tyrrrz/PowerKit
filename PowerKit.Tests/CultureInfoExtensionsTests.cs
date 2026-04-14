#nullable enable
using System.Globalization;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class CultureInfoExtensionsTests
{
    [Fact]
    public void GetParents_Test()
    {
        // Act & assert
        new CultureInfo("en-US")
            .GetParents()
            .Should()
            .Equal(new CultureInfo("en"), CultureInfo.InvariantCulture);

        new CultureInfo("en")
            .GetParents()
            .Should()
            .Equal(CultureInfo.InvariantCulture);

        CultureInfo.InvariantCulture.GetParents().Should().BeEmpty();
    }

    [Fact]
    public void GetSelfAndParents_Test()
    {
        // Act & assert
        new CultureInfo("en-US")
            .GetSelfAndParents()
            .Should()
            .Equal(new CultureInfo("en-US"), new CultureInfo("en"), CultureInfo.InvariantCulture);

        new CultureInfo("en")
            .GetSelfAndParents()
            .Should()
            .Equal(new CultureInfo("en"), CultureInfo.InvariantCulture);

        CultureInfo.InvariantCulture
            .GetSelfAndParents()
            .Should()
            .Equal(CultureInfo.InvariantCulture);
    }
}
