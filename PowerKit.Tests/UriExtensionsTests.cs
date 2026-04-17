using System;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class UriExtensionsTests
{
    [Fact]
    public void Domain_Test()
    {
        // Act & assert
        new Uri("https://example.com/path?query=1")
            .Domain.Should()
            .Be("https://example.com");
        new Uri("http://foo.bar.baz").Domain.Should().Be("http://foo.bar.baz");
    }
}
