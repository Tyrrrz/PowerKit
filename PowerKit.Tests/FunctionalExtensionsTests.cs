using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class FunctionalExtensionsTests
{
    [Fact]
    public void Pipe_Test()
    {
        // Act
        var result = 5.Pipe(x => x * 2);

        // Assert
        result.Should().Be(10);
    }

    [Fact]
    public void Pipe_Chained_Test()
    {
        // Act
        var result = "hello".Pipe(s => s.ToUpper()).Pipe(s => s + "!");

        // Assert
        result.Should().Be("HELLO!");
    }

    [Fact]
    public void NullIf_PredicateMatches_Test()
    {
        // Act
        var result = 0.NullIf(v => v == 0);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void NullIf_PredicateDoesNotMatch_Test()
    {
        // Act
        var result = 5.NullIf(v => v == 0);

        // Assert
        result.Should().Be(5);
    }

    [Fact]
    public void NullIfDefault_Default_Test()
    {
        // Act
        var result = 0.NullIfDefault();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void NullIfDefault_NonDefault_Test()
    {
        // Act
        var result = 42.NullIfDefault();

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public void NullIfDefault_DefaultGuid_Test()
    {
        // Act & assert
        Guid.Empty.NullIfDefault().Should().BeNull();
    }

    [Fact]
    public void NullIfDefault_NonDefaultGuid_Test()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act & assert
        id.NullIfDefault().Should().Be(id);
    }

    [Fact]
    public void NullIfEmpty_Test()
    {
        // Act & assert
        "hello".NullIfEmpty().Should().Be("hello");
    }

    [Fact]
    public void NullIfEmpty_Empty_Test()
    {
        // Act & assert
        "".NullIfEmpty().Should().BeNull();
    }

    [Fact]
    public void NullIfEmpty_Whitespace_Test()
    {
        // Act & assert
        "   ".NullIfEmpty().Should().Be("   ");
    }

    [Fact]
    public void NullIfWhiteSpace_Test()
    {
        // Act & assert
        "hello".NullIfWhiteSpace().Should().Be("hello");
    }

    [Fact]
    public void NullIfWhiteSpace_Whitespace_Test()
    {
        // Act & assert
        "   ".NullIfWhiteSpace().Should().BeNull();
    }

    [Fact]
    public void NullIfWhiteSpace_Empty_Test()
    {
        // Act & assert
        "".NullIfWhiteSpace().Should().BeNull();
    }
}
