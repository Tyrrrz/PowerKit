using System.Collections.Generic;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class ReadOnlyDictionaryExtensionsTests
{
    [Fact]
    public void GetValueOrNull_Test()
    {
        // Arrange
        var source = (IReadOnlyDictionary<string, int>)new Dictionary<string, int> { ["one"] = 1 };

        // Act & assert
        source.GetValueOrNull("one").Should().Be(1);
        source.GetValueOrNull("two").Should().BeNull();
    }
}
