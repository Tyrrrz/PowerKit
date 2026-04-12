using System;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class ParsingExtensionsTests
{
    [Fact]
    public void Int_ParseOrNull_Test()
    {
        // Act & assert
        int.ParseOrNull("42").Should().Be(42);
        int.ParseOrNull("-7").Should().Be(-7);
        int.ParseOrNull("abc").Should().BeNull();
        int.ParseOrNull(null).Should().BeNull();
    }

    [Fact]
    public void Long_ParseOrNull_Test()
    {
        // Act & assert
        long.ParseOrNull("9876543210").Should().Be(9876543210L);
        long.ParseOrNull("-1").Should().Be(-1L);
        long.ParseOrNull("abc").Should().BeNull();
        long.ParseOrNull(null).Should().BeNull();
    }

    [Fact]
    public void Double_ParseOrNull_Test()
    {
        // Act & assert
        double.ParseOrNull("3.14").Should().Be(3.14);
        double.ParseOrNull("-1.5").Should().Be(-1.5);
        double.ParseOrNull("abc").Should().BeNull();
        double.ParseOrNull(null).Should().BeNull();
    }

    [Fact]
    public void Decimal_ParseOrNull_Test()
    {
        // Act & assert
        decimal.ParseOrNull("3.14").Should().Be(3.14m);
        decimal.ParseOrNull("-1.5").Should().Be(-1.5m);
        decimal.ParseOrNull("abc").Should().BeNull();
        decimal.ParseOrNull(null).Should().BeNull();
    }

    [Fact]
    public void Bool_ParseOrNull_Test()
    {
        // Act & assert
        bool.ParseOrNull("true").Should().BeTrue();
        bool.ParseOrNull("false").Should().BeFalse();
        bool.ParseOrNull("yes").Should().BeNull();
        bool.ParseOrNull(null).Should().BeNull();
    }

    [Fact]
    public void DateTime_ParseOrNull_Test()
    {
        // Act & assert
        DateTime.ParseOrNull("2024-06-15").Should().Be(new DateTime(2024, 6, 15));
        DateTime.ParseOrNull("not a date").Should().BeNull();
        DateTime.ParseOrNull(null).Should().BeNull();
    }

    [Fact]
    public void DateTimeOffset_ParseOrNull_Test()
    {
        // Act & assert
        DateTimeOffset.ParseOrNull("2024-06-15T00:00:00+00:00").Should().Be(
            new DateTimeOffset(2024, 6, 15, 0, 0, 0, TimeSpan.Zero)
        );
        DateTimeOffset.ParseOrNull("not a date").Should().BeNull();
        DateTimeOffset.ParseOrNull(null).Should().BeNull();
    }

    [Fact]
    public void TimeSpan_ParseOrNull_Test()
    {
        // Act & assert
        TimeSpan.ParseOrNull("1:30:00").Should().Be(new TimeSpan(1, 30, 0));
        TimeSpan.ParseOrNull("not a timespan").Should().BeNull();
        TimeSpan.ParseOrNull(null).Should().BeNull();
    }

    [Fact]
    public void Guid_ParseOrNull_Test()
    {
        // Act & assert
        Guid.ParseOrNull("12345678-1234-1234-1234-123456789abc").Should().Be(
            new Guid("12345678-1234-1234-1234-123456789abc")
        );
        Guid.ParseOrNull("not-a-guid").Should().BeNull();
        Guid.ParseOrNull(null).Should().BeNull();
    }
}
