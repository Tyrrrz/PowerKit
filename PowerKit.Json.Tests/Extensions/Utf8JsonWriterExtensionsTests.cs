using System;
using System.Collections.Generic;
using System.Text.Json;
using AwesomeAssertions;
using PowerKit.Json.Extensions;
using PowerKit.Json.Tests.Utils;
using Xunit;

namespace PowerKit.Json.Tests.Extensions;

public class Utf8JsonWriterExtensionsTests
{
    [Fact]
    public void WriteBoolean_Actual_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteBoolean("foo", new bool?(true));
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":true}
                """
            );
    }

    [Fact]
    public void WriteBoolean_Null_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteBoolean("foo", new bool?());
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":null}
                """
            );
    }

    [Fact]
    public void WriteNumber_Byte_Actual_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteNumber("foo", new byte?(13));
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":13}
                """
            );
    }

    [Fact]
    public void WriteNumber_Byte_Null_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteNumber("foo", new byte?());
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":null}
                """
            );
    }

    [Fact]
    public void WriteNumber_Decimal_Actual_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteNumber("foo", new decimal?(13));
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":13}
                """
            );
    }

    [Fact]
    public void WriteNumber_Decimal_Null_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteNumber("foo", new decimal?());
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":null}
                """
            );
    }

    [Fact]
    public void WriteNumber_Double_Actual_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteNumber("foo", new double?(13));
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":13}
                """
            );
    }

    [Fact]
    public void WriteNumber_Double_Null_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteNumber("foo", new double?());
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":null}
                """
            );
    }

    [Fact]
    public void WriteNumber_Float_Actual_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteNumber("foo", new float?(13));
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":13}
                """
            );
    }

    [Fact]
    public void WriteNumber_Float_Null_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteNumber("foo", new float?());
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":null}
                """
            );
    }

    [Fact]
    public void WriteNumber_Short_Actual_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteNumber("foo", new short?(13));
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":13}
                """
            );
    }

    [Fact]
    public void WriteNumber_Short_Null_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteNumber("foo", new short?());
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":null}
                """
            );
    }

    [Fact]
    public void WriteNumber_Int_Actual_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteNumber("foo", new int?(13));
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":13}
                """
            );
    }

    [Fact]
    public void WriteNumber_Int_Null_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteNumber("foo", new int?());
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":null}
                """
            );
    }

    [Fact]
    public void WriteNumber_Long_Actual_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteNumber("foo", new long?(13));
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":13}
                """
            );
    }

    [Fact]
    public void WriteNumber_Long_Null_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteNumber("foo", new long?());
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":null}
                """
            );
    }

    [Fact]
    public void WriteNumber_SByte_Actual_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteNumber("foo", new sbyte?(13));
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":13}
                """
            );
    }

    [Fact]
    public void WriteNumber_SByte_Null_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteNumber("foo", new sbyte?());
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":null}
                """
            );
    }

    [Fact]
    public void WriteNumber_UShort_Actual_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteNumber("foo", new ushort?(13));
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":13}
                """
            );
    }

    [Fact]
    public void WriteNumber_UShort_Null_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteNumber("foo", new ushort?());
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":null}
                """
            );
    }

    [Fact]
    public void WriteNumber_UInt_Actual_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteNumber("foo", new uint?(13));
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":13}
                """
            );
    }

    [Fact]
    public void WriteNumber_UInt_Null_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteNumber("foo", new uint?());
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":null}
                """
            );
    }

    [Fact]
    public void WriteNumber_ULong_Actual_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteNumber("foo", new ulong?(13));
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":13}
                """
            );
    }

    [Fact]
    public void WriteNumber_ULong_Null_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteNumber("foo", new ulong?());
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":null}
                """
            );
    }

    [Fact]
    public void WriteString_Guid_Actual_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteString("foo", new Guid?(Guid.Empty));
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":"00000000-0000-0000-0000-000000000000"}
                """
            );
    }

    [Fact]
    public void WriteString_Guid_Null_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteString("foo", new Guid?());
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":null}
                """
            );
    }

    [Fact]
    public void WriteString_DateTime_Actual_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteString("foo", new DateTime?(DateTime.UnixEpoch));
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":"1970-01-01T00:00:00Z"}
                """
            );
    }

    [Fact]
    public void WriteString_DateTime_Null_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteString("foo", new DateTime?());
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":null}
                """
            );
    }

    [Fact]
    public void WriteString_DateTimeOffset_Actual_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteString("foo", new DateTimeOffset?(DateTimeOffset.UnixEpoch));
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":"1970-01-01T00:00:00+00:00"}
                """
            );
    }

    [Fact]
    public void WriteString_DateTimeOffset_Null_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.WriteString("foo", new DateTimeOffset?());
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":null}
                """
            );
    }

    [Fact]
    public void Write_Null_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.Write("foo", (int?)null);
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":null}
                """
            );
    }

    [Fact]
    public void Write_Number_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.Write("foo", 42);
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":42}
                """
            );
    }

    [Fact]
    public void Write_String_Test()
    {
        // Arrange
        var context = new InMemoryJsonWriterContext();

        // Act
        context.Writer.WriteStartObject();
        context.Writer.Write("foo", "bar");
        context.Writer.WriteEndObject();

        // Assert
        context
            .GetString()
            .Should()
            .Be(
                """
                {"foo":"bar"}
                """
            );
    }

    [Fact]
    public void Write_Dictionary_Test()
    {
        // Arrange
        var dictionary = new Dictionary<string, object?>
        {
            ["null"] = null,
            ["boolean"] = true,
            ["byte"] = (byte)1,
            ["decimal"] = (decimal)3.14,
            ["double"] = 3.14,
            ["short"] = (short)2,
            ["int"] = -1111,
            ["long"] = 1000000000000L,
            ["sbyte"] = (sbyte)9,
            ["float"] = 1.979f,
            ["ushort"] = (ushort)91,
            ["uint"] = (uint)999999999,
            ["ulong"] = (ulong)888888888888888,
            ["dateTime"] = new DateTime(2024, 1, 1),
            ["dateTimeOffset"] = new DateTimeOffset(new DateTime(2024, 1, 1), TimeSpan.Zero),
            ["guid"] = new Guid("00000000-0000-0000-0000-000000000000"),
            ["string"] = "Hello world!",
            ["encodedText"] = JsonEncodedText.Encode("Hello cruel world!"),
            ["dictionary"] = new Dictionary<string, object?>
            {
                ["key"] = "value",
                ["anotherKey"] = 123,
                ["nested"] = new Dictionary<string, object?> { ["innerKey"] = "innerValue" },
            },
            ["array"] = new object[]
            {
                1,
                "two",
                3.0,
                new Dictionary<string, object?>
                {
                    ["key"] = "value",
                    ["innerArray"] = new object[] { 4, 5, 6 },
                },
            },
        };

        // Act
        var json = JsonElement.FromDictionary(dictionary);

        // Assert
        json.GetProperty("null").ValueKind.Should().Be(JsonValueKind.Null);
        json.GetProperty("boolean").GetBoolean().Should().BeTrue();
        json.GetProperty("byte").GetByte().Should().Be(1);
        json.GetProperty("decimal").GetDecimal().Should().Be(3.14m);
        json.GetProperty("double").GetDouble().Should().Be(3.14);
        json.GetProperty("short").GetInt16().Should().Be(2);
        json.GetProperty("int").GetInt32().Should().Be(-1111);
        json.GetProperty("long").GetInt64().Should().Be(1000000000000L);
        json.GetProperty("sbyte").GetSByte().Should().Be(9);
        json.GetProperty("float").GetSingle().Should().Be(1.979f);
        json.GetProperty("ushort").GetUInt16().Should().Be(91);
        json.GetProperty("uint").GetUInt32().Should().Be(999999999);
        json.GetProperty("ulong").GetUInt64().Should().Be(888888888888888);
        json.GetProperty("dateTime").GetDateTime().Should().Be(new DateTime(2024, 1, 1));
        json.GetProperty("dateTimeOffset")
            .GetDateTimeOffset()
            .Should()
            .Be(new DateTimeOffset(new DateTime(2024, 1, 1), TimeSpan.Zero));
        json.GetProperty("guid")
            .GetGuid()
            .Should()
            .Be(new Guid("00000000-0000-0000-0000-000000000000"));
        json.GetProperty("string").GetString().Should().Be("Hello world!");
        json.GetProperty("encodedText").GetString().Should().Be("Hello cruel world!");
        json.GetProperty("dictionary").GetProperty("key").GetString().Should().Be("value");
        json.GetProperty("dictionary").GetProperty("anotherKey").GetInt32().Should().Be(123);
        json.GetProperty("dictionary")
            .GetProperty("nested")
            .GetProperty("innerKey")
            .GetString()
            .Should()
            .Be("innerValue");
        json.GetProperty("array")[0].GetInt32().Should().Be(1);
        json.GetProperty("array")[1].GetString().Should().Be("two");
        json.GetProperty("array")[2].GetDouble().Should().Be(3.0);
        json.GetProperty("array")[3].GetProperty("key").GetString().Should().Be("value");
        json.GetProperty("array")[3].GetProperty("innerArray")[0].GetInt32().Should().Be(4);
        json.GetProperty("array")[3].GetProperty("innerArray")[1].GetInt32().Should().Be(5);
        json.GetProperty("array")[3].GetProperty("innerArray")[2].GetInt32().Should().Be(6);
    }

    [Fact]
    public void Write_UnknownType_Test()
    {
        // Arrange
        var unknownTypeObject = new { Property = "Value" };

        // Act
        var act = () =>
            JsonElement.FromDictionary(
                new Dictionary<string, object> { { "unknown", unknownTypeObject } }
            );

        // Assert
        act.Should().Throw<NotSupportedException>();
    }
}
