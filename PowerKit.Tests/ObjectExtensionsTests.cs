using System.Linq;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class ObjectExtensionsTests
{
    [Fact]
    public void ToSingletonEnumerable_Test()
    {
        // Act & assert
        42.ToSingletonEnumerable().ToList().Should().Equal(42);
        "hello".ToSingletonEnumerable().ToList().Should().Equal("hello");
        ((string?)null).ToSingletonEnumerable().ToList().Should().Equal((string?)null);
    }
}
