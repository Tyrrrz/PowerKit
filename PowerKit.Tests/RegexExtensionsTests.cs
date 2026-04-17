using System.Text.RegularExpressions;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class RegexExtensionsTests
{
    [Fact]
    public void FromWildcardPattern_Star_MatchesAnySequence_Test()
    {
        // Arrange
        var regex = Regex.FromWildcardPattern("*.txt");

        // Act & assert
        regex.IsMatch("file.txt").Should().BeTrue();
        regex.IsMatch("foo.bar.txt").Should().BeTrue();
        regex.IsMatch(".txt").Should().BeTrue();
        regex.IsMatch("file.csv").Should().BeFalse();
    }

    [Fact]
    public void FromWildcardPattern_QuestionMark_MatchesSingleCharacter_Test()
    {
        // Arrange
        var regex = Regex.FromWildcardPattern("file?.txt");

        // Act & assert
        regex.IsMatch("file1.txt").Should().BeTrue();
        regex.IsMatch("fileA.txt").Should().BeTrue();
        regex.IsMatch("file.txt").Should().BeFalse();
        regex.IsMatch("file12.txt").Should().BeFalse();
    }

    [Fact]
    public void FromWildcardPattern_StarAndQuestionMark_Combined_Test()
    {
        // Arrange
        var regex = Regex.FromWildcardPattern("fo?.*");

        // Act & assert
        regex.IsMatch("foo.txt").Should().BeTrue();
        regex.IsMatch("fob.csv").Should().BeTrue();
        regex.IsMatch("fo.txt").Should().BeFalse();
        regex.IsMatch("fooo.txt").Should().BeFalse();
    }

    [Fact]
    public void FromWildcardPattern_NoWildcards_MatchesExactString_Test()
    {
        // Arrange
        var regex = Regex.FromWildcardPattern("hello.world");

        // Act & assert
        regex.IsMatch("hello.world").Should().BeTrue();
        regex.IsMatch("helloXworld").Should().BeFalse();
        regex.IsMatch("hello.worlds").Should().BeFalse();
    }

    [Fact]
    public void FromWildcardPattern_StarOnly_MatchesAnything_Test()
    {
        // Arrange
        var regex = Regex.FromWildcardPattern("*");

        // Act & assert
        regex.IsMatch("").Should().BeTrue();
        regex.IsMatch("anything").Should().BeTrue();
        regex.IsMatch("foo bar baz").Should().BeTrue();
    }

    [Fact]
    public void FromWildcardPattern_WithOptions_Test()
    {
        // Arrange
        var regex = Regex.FromWildcardPattern("*.TXT", RegexOptions.IgnoreCase);

        // Act & assert
        regex.IsMatch("file.txt").Should().BeTrue();
        regex.IsMatch("file.TXT").Should().BeTrue();
        regex.IsMatch("file.Txt").Should().BeTrue();
    }

    [Fact]
    public void FromWildcardPattern_EscapesRegexSpecialChars_Test()
    {
        // Arrange
        var regex = Regex.FromWildcardPattern("(hello).*");

        // Act & assert
        regex.IsMatch("(hello).world").Should().BeTrue();
        regex.IsMatch("hello.world").Should().BeFalse();
    }
}
