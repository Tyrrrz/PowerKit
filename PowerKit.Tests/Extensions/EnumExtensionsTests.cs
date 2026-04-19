using System;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

file enum TestEnum
{
    Foo,
    Bar,
}

public class EnumExtensionsTests
{
    [Fact]
    public void ParseOrNull_Test()
    {
        // Act & assert
        Enum.ParseOrNull<TestEnum>("Foo").Should().Be(TestEnum.Foo);
        Enum.ParseOrNull<TestEnum>("foo").Should().BeNull();
        Enum.ParseOrNull<TestEnum>("foo", true).Should().Be(TestEnum.Foo);
        Enum.ParseOrNull<TestEnum>("invalid").Should().BeNull();
        Enum.ParseOrNull<TestEnum>(null).Should().BeNull();
    }

    [Fact]
    public void ParseOrDefault_Test()
    {
        // Act & assert
        Enum.ParseOrDefault<TestEnum>("Foo").Should().Be(TestEnum.Foo);
        Enum.ParseOrDefault<TestEnum>("foo").Should().Be(default);
        Enum.ParseOrDefault<TestEnum>("foo", true).Should().Be(TestEnum.Foo);
        Enum.ParseOrDefault<TestEnum>("invalid", TestEnum.Bar).Should().Be(TestEnum.Bar);
        Enum.ParseOrDefault<TestEnum>(null, TestEnum.Bar).Should().Be(TestEnum.Bar);
    }
}
