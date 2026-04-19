using System;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

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

    [Fact]
    public void TryGetFileName_WithFileName_Test()
    {
        // Act & assert
        new Uri("https://example.com/files/document.pdf")
            .TryGetFileName()
            .Should()
            .Be("document.pdf");
        new Uri("https://example.com/files/document.pdf?version=2")
            .TryGetFileName()
            .Should()
            .Be("document.pdf");
        new Uri("https://example.com/files/my%20file.txt")
            .TryGetFileName()
            .Should()
            .Be("my file.txt");
        new Uri("https://example.com/")
            .TryGetFileName()
            .Should()
            .BeNull();
        new Uri("https://example.com").TryGetFileName().Should().BeNull();
    }
}
