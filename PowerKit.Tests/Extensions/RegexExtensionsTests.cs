using System.Text.RegularExpressions;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class RegexExtensionsTests
{
    [Fact]
    public void FromWildcardPattern_Test()
    {
        // Act & assert

        // * matches any sequence (including empty)
        Regex.FromWildcardPattern("*.txt").IsMatch("file.txt").Should().BeTrue();
        Regex.FromWildcardPattern("*.txt").IsMatch("foo.bar.txt").Should().BeTrue();
        Regex.FromWildcardPattern("*.txt").IsMatch(".txt").Should().BeTrue();
        Regex.FromWildcardPattern("*.txt").IsMatch("file.csv").Should().BeFalse();
        Regex.FromWildcardPattern("*").IsMatch("").Should().BeTrue();
        Regex.FromWildcardPattern("*").IsMatch("anything").Should().BeTrue();

        // ? matches exactly one character
        Regex.FromWildcardPattern("file?.txt").IsMatch("file1.txt").Should().BeTrue();
        Regex.FromWildcardPattern("file?.txt").IsMatch("fileA.txt").Should().BeTrue();
        Regex.FromWildcardPattern("file?.txt").IsMatch("file.txt").Should().BeFalse();
        Regex.FromWildcardPattern("file?.txt").IsMatch("file12.txt").Should().BeFalse();

        // No wildcards — exact match
        Regex.FromWildcardPattern("hello.world").IsMatch("hello.world").Should().BeTrue();
        Regex.FromWildcardPattern("hello.world").IsMatch("helloXworld").Should().BeFalse();

        // Regex special characters are treated as literals
        Regex.FromWildcardPattern("(hello).*").IsMatch("(hello).world").Should().BeTrue();
        Regex.FromWildcardPattern("(hello).*").IsMatch("hello.world").Should().BeFalse();
    }

    [Fact]
    public void FromWildcardPattern_WithOptions_Test()
    {
        // Act & assert
        Regex
            .FromWildcardPattern("*.TXT", RegexOptions.IgnoreCase)
            .IsMatch("file.txt")
            .Should()
            .BeTrue();
        Regex
            .FromWildcardPattern("*.TXT", RegexOptions.IgnoreCase)
            .IsMatch("file.TXT")
            .Should()
            .BeTrue();
        Regex
            .FromWildcardPattern("*.TXT", RegexOptions.IgnoreCase)
            .IsMatch("file.Txt")
            .Should()
            .BeTrue();
        Regex.FromWildcardPattern("*.TXT").IsMatch("file.txt").Should().BeFalse();
    }
}
