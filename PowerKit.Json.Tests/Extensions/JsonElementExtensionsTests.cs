using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using AwesomeAssertions;
using PowerKit.Json.Extensions;
using Xunit;

namespace PowerKit.Json.Tests.Extensions;

public class JsonElementExtensionsTests
{
    [Fact]
    public void GetPropertyOrNull_Test()
    {
        // Arrange
        var json = JsonElement.Parse(
            """
            {
              "foo": 13
            }
            """
        );

        // Act
        var child = json.GetPropertyOrNull("foo");

        // Assert
        child.Should().NotBeNull();
        child?.GetInt32().Should().Be(13);
    }

    [Fact]
    public void GetPropertyOrNull_Undefined_Test()
    {
        // Arrange
        var json = JsonElement.Parse(
            """
            {
              "foo": 13
            }
            """
        );

        // Act
        var child = json.GetPropertyOrNull("bar");

        // Assert
        child.Should().BeNull();
    }

    [Fact]
    public void GetPropertyOrNull_Null_Test()
    {
        // Arrange
        var json = JsonElement.Parse(
            """
            {
              "foo": null
            }
            """
        );

        // Act
        var child = json.GetPropertyOrNull("foo");

        // Assert
        child.Should().BeNull();
    }

    [Fact]
    public void GetPropertyOrNull_NotObject_Test()
    {
        // Arrange
        var json = JsonElement.Parse(
            """
            [
              1,
              2,
              3
            ]
            """
        );

        // Act
        var child = json.GetPropertyOrNull("foo");

        // Assert
        child.Should().BeNull();
    }

    [Fact]
    public void GetByIndexOrNull_Test()
    {
        // Arrange
        var json = JsonElement.Parse(
            """
            [
              1,
              2,
              3
            ]
            """
        );

        // Act
        var child = json.GetByIndexOrNull(1);

        // Assert
        child.Should().NotBeNull();
        child?.GetInt32().Should().Be(2);
    }

    [Fact]
    public void GetByIndexOrNull_OutOfBounds_Test()
    {
        // Arrange
        var json = JsonElement.Parse(
            """
            [
              1,
              2,
              3
            ]
            """
        );

        // Act
        var child = json.GetByIndexOrNull(5);

        // Assert
        child.Should().BeNull();
    }

    [Fact]
    public void GetByIndexOrNull_Null_Test()
    {
        // Arrange
        var json = JsonElement.Parse(
            """
            [
              1,
              null,
              3
            ]
            """
        );

        // Act
        var child = json.GetByIndexOrNull(1);

        // Assert
        child.Should().BeNull();
    }

    [Fact]
    public void GetByIndexOrNull_NotArray_Test()
    {
        // Arrange
        var json = JsonElement.Parse(
            """
            {
              "foo": null
            }
            """
        );

        // Act
        var child = json.GetByIndexOrNull(1);

        // Assert
        child.Should().BeNull();
    }

    [Fact]
    public void EnumerateArrayOrEmpty_Test()
    {
        // Arrange
        var json = JsonElement.Parse(
            """
            [
              1,
              2,
              3
            ]
            """
        );

        // Act
        var count = json.EnumerateArrayOrEmpty().Count();

        // Assert
        count.Should().Be(3);
    }

    [Fact]
    public void EnumerateArrayOrEmpty__NotArray_Test()
    {
        // Arrange
        var json = JsonElement.Parse(
            """
            {"foo":null}
            """
        );

        // Act
        var count = json.EnumerateArrayOrEmpty().Count();

        // Assert
        count.Should().Be(0);
    }

    [Fact]
    public void EnumerateObjectOrEmpty_Test()
    {
        // Arrange
        var json = JsonElement.Parse(
            """
            {
              "foo": 1,
              "bar": true
            }
            """
        );

        // Act
        var count = json.EnumerateObjectOrEmpty().Count();

        // Assert
        count.Should().Be(2);
    }

    [Fact]
    public void EnumerateObjectOrEmpty__NotObject_Test()
    {
        // Arrange
        var json = JsonElement.Parse(
            """
            [
              1,
              2,
              3
            ]
            """
        );

        // Act
        var count = json.EnumerateObjectOrEmpty().Count();

        // Assert
        count.Should().Be(0);
    }

    [Fact]
    public void GetBooleanOrNull_Positive_True_Test()
    {
        // Arrange
        var json = JsonElement.Parse("true");

        // Act
        var value = json.GetBooleanOrNull();

        // Assert
        value.Should().BeTrue();
    }

    [Fact]
    public void GetBooleanOrNull_Positive_False_Test()
    {
        // Arrange
        var json = JsonElement.Parse("false");

        // Act
        var value = json.GetBooleanOrNull();

        // Assert
        value.Should().BeFalse();
    }

    [Fact]
    public void GetBooleanOrNull_Null_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act
        var value = json.GetBooleanOrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetBooleanOrNull_NotBoolean_Test()
    {
        // Arrange
        var json = JsonElement.Parse("13");

        // Act
        var value = json.GetBooleanOrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetBooleanCoerced_Positive_Direct_Test()
    {
        // Arrange
        var json = JsonElement.Parse("true");

        // Act
        var value = json.GetBooleanCoerced();

        // Assert
        value.Should().BeTrue();
    }

    [Fact]
    public void GetBooleanCoerced_Positive_FromString_Test()
    {
        // Arrange
        var json = JsonElement.Parse("\"true\"");

        // Act
        var value = json.GetBooleanCoerced();

        // Assert
        value.Should().BeTrue();
    }

    [Fact]
    public void GetBooleanCoerced_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act
        var act = () => json.GetBooleanCoerced();

        // Act & assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetByteOrNull_Test()
    {
        // Arrange
        var json = JsonElement.Parse("13");

        // Act
        var value = json.GetByteOrNull();

        // Assert
        value.Should().Be(13);
    }

    [Fact]
    public void GetByteOrNull_Null_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act
        var value = json.GetByteOrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetByteOrNull_NotNumber_Test()
    {
        // Arrange
        var json = JsonElement.Parse("true");

        // Act
        var value = json.GetByteOrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetByteOrNull_NotConvertible_Test()
    {
        // Arrange
        var json = JsonElement.Parse("3.14");

        // Act
        var value = json.GetByteOrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetByteCoerced_Positive_Direct_Test()
    {
        // Arrange
        var json = JsonElement.Parse("13");

        // Act
        var value = json.GetByteCoerced();

        // Assert
        value.Should().Be(13);
    }

    [Fact]
    public void GetByteCoerced_Positive_FromString_Test()
    {
        // Arrange
        var json = JsonElement.Parse("\"13\"");

        // Act
        var value = json.GetByteCoerced();

        // Assert
        value.Should().Be(13);
    }

    [Fact]
    public void GetByteCoerced_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act
        var act = () => json.GetByteCoerced();

        // Act & assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetDecimalOrNull_Test()
    {
        // Arrange
        var json = JsonElement.Parse("13");

        // Act
        var value = json.GetDecimalOrNull();

        // Assert
        value.Should().Be(13);
    }

    [Fact]
    public void GetDecimalOrNull_Null_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act
        var value = json.GetDecimalOrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetDecimalOrNull_NotNumber_Test()
    {
        // Arrange
        var json = JsonElement.Parse("true");

        // Act
        var value = json.GetDecimalOrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetDecimalOrNull_NotConvertible_Test()
    {
        // Arrange
        var json = JsonElement.Parse("99999999999999999999999999999999999");

        // Act
        var value = json.GetDecimalOrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetDecimalCoerced_Positive_Direct_Test()
    {
        // Arrange
        var json = JsonElement.Parse("13");

        // Act
        var value = json.GetDecimalCoerced();

        // Assert
        value.Should().Be(13);
    }

    [Fact]
    public void GetDecimalCoerced_Positive_FromString_Test()
    {
        // Arrange
        var json = JsonElement.Parse("\"13\"");

        // Act
        var value = json.GetDecimalCoerced();

        // Assert
        value.Should().Be(13);
    }

    [Fact]
    public void GetDecimalCoerced_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act
        var act = () => json.GetDecimalCoerced();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetDoubleOrNull_Test()
    {
        // Arrange
        var json = JsonElement.Parse("13.11");

        // Act
        var value = json.GetDoubleOrNull();

        // Assert
        value.Should().Be(13.11);
    }

    [Fact]
    public void GetDoubleOrNull_Null_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act
        var value = json.GetDoubleOrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetDoubleOrNull_NotNumber_Test()
    {
        // Arrange
        var json = JsonElement.Parse("true");

        // Act
        var value = json.GetDoubleOrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetDoubleCoerced_Positive_Direct_Test()
    {
        // Arrange
        var json = JsonElement.Parse("13.11");

        // Act
        var value = json.GetDoubleCoerced();

        // Assert
        value.Should().Be(13.11);
    }

    [Fact]
    public void GetDoubleCoerced_Positive_FromString_Test()
    {
        // Arrange
        var json = JsonElement.Parse("\"13.11\"");

        // Act
        var value = json.GetDoubleCoerced();

        // Assert
        value.Should().Be(13.11);
    }

    [Fact]
    public void GetDoubleCoerced_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act
        var act = () => json.GetDoubleCoerced();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetSingleOrNull_Test()
    {
        // Arrange
        var json = JsonElement.Parse("13.11");

        // Act
        var value = json.GetSingleOrNull();

        // Assert
        value.Should().Be(13.11f);
    }

    [Fact]
    public void GetSingleOrNull_Null_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act
        var value = json.GetSingleOrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetSingleOrNull_NotNumber_Test()
    {
        // Arrange
        var json = JsonElement.Parse("true");

        // Act
        var value = json.GetSingleOrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetSingleCoerced_Positive_Direct_Test()
    {
        // Arrange
        var json = JsonElement.Parse("13.11");

        // Act
        var value = json.GetSingleCoerced();

        // Assert
        value.Should().Be(13.11f);
    }

    [Fact]
    public void GetSingleCoerced_Positive_FromString_Test()
    {
        // Arrange
        var json = JsonElement.Parse("\"13.11\"");

        // Act
        var value = json.GetSingleCoerced();

        // Assert
        value.Should().Be(13.11f);
    }

    [Fact]
    public void GetSingleCoerced_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act & assert
        var act = () => json.GetSingleCoerced();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetInt16OrNull_Test()
    {
        // Arrange
        var json = JsonElement.Parse("13");

        // Act
        var value = json.GetInt16OrNull();

        // Assert
        value.Should().Be(13);
    }

    [Fact]
    public void GetInt16OrNull_Null_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act
        var value = json.GetInt16OrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetInt16OrNull_NotNumber_Test()
    {
        // Arrange
        var json = JsonElement.Parse("true");

        // Act
        var value = json.GetInt16OrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetInt16OrNull_NotConvertible_Test()
    {
        // Arrange
        var json = JsonElement.Parse("3.14");

        // Act
        var value = json.GetInt16OrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetInt16Coerced_Positive_Direct_Test()
    {
        // Arrange
        var json = JsonElement.Parse("13");

        // Act
        var value = json.GetInt16Coerced();

        // Assert
        value.Should().Be(13);
    }

    [Fact]
    public void GetInt16Coerced_Positive_FromString_Test()
    {
        // Arrange
        var json = JsonElement.Parse("\"13\"");

        // Act
        var value = json.GetInt16Coerced();

        // Assert
        value.Should().Be(13);
    }

    [Fact]
    public void GetInt16Coerced_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act
        var act = () => json.GetInt16Coerced();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetInt32OrNull_Test()
    {
        // Arrange
        var json = JsonElement.Parse("13");

        // Act
        var value = json.GetInt32OrNull();

        // Assert
        value.Should().Be(13);
    }

    [Fact]
    public void GetInt32OrNull_Null_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act
        var value = json.GetInt32OrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetInt32OrNull_NotNumber_Test()
    {
        // Arrange
        var json = JsonElement.Parse("true");

        // Act
        var value = json.GetInt32OrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetInt32OrNull_NotConvertible_Test()
    {
        // Arrange
        var json = JsonElement.Parse("3.14");

        // Act
        var value = json.GetInt32OrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetInt32Coerced_Positive_Direct_Test()
    {
        // Arrange
        var json = JsonElement.Parse("13");

        // Act
        var value = json.GetInt32Coerced();

        // Assert
        value.Should().Be(13);
    }

    [Fact]
    public void GetInt32Coerced_Positive_FromString_Test()
    {
        // Arrange
        var json = JsonElement.Parse("\"13\"");

        // Act
        var value = json.GetInt32Coerced();

        // Assert
        value.Should().Be(13);
    }

    [Fact]
    public void GetInt32Coerced_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act & assert
        var act = () => json.GetInt32Coerced();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetInt64OrNull_Test()
    {
        // Arrange
        var json = JsonElement.Parse("13");

        // Act
        var value = json.GetInt64OrNull();

        // Assert
        value.Should().Be(13);
    }

    [Fact]
    public void GetInt64OrNull_Null_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act
        var value = json.GetInt64OrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetInt64OrNull_NotNumber_Test()
    {
        // Arrange
        var json = JsonElement.Parse("true");

        // Act
        var value = json.GetInt64OrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetInt64OrNull_NotConvertible_Test()
    {
        // Arrange
        var json = JsonElement.Parse("3.14");

        // Act
        var value = json.GetInt64OrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetInt64Coerced_Positive_Direct_Test()
    {
        // Arrange
        var json = JsonElement.Parse("13");

        // Act
        var value = json.GetInt64Coerced();

        // Assert
        value.Should().Be(13);
    }

    [Fact]
    public void GetInt64Coerced_Positive_FromString_Test()
    {
        // Arrange
        var json = JsonElement.Parse("\"13\"");

        // Act
        var value = json.GetInt64Coerced();

        // Assert
        value.Should().Be(13);
    }

    [Fact]
    public void GetInt64Coerced_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act & assert
        var act = () => json.GetInt64Coerced();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetSByteOrNull_Test()
    {
        // Arrange
        var json = JsonElement.Parse("13");

        // Act
        var value = json.GetSByteOrNull();

        // Assert
        value.Should().Be(13);
    }

    [Fact]
    public void GetSByteOrNull_Null_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act
        var value = json.GetSByteOrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetSByteOrNull_NotNumber_Test()
    {
        // Arrange
        var json = JsonElement.Parse("true");

        // Act
        var value = json.GetSByteOrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetSByteOrNull_NotConvertible_Test()
    {
        // Arrange
        var json = JsonElement.Parse("3.14");

        // Act
        var value = json.GetSByteOrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetSByteCoerced_Positive_Direct_Test()
    {
        // Arrange
        var json = JsonElement.Parse("13");

        // Act
        var value = json.GetSByteCoerced();

        // Assert
        value.Should().Be(13);
    }

    [Fact]
    public void GetSByteCoerced_Positive_FromString_Test()
    {
        // Arrange
        var json = JsonElement.Parse("\"13\"");

        // Act
        var value = json.GetSByteCoerced();

        // Assert
        value.Should().Be(13);
    }

    [Fact]
    public void GetSByteCoerced_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act
        var act = () => json.GetSByteCoerced();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetUInt16OrNull_Test()
    {
        // Arrange
        var json = JsonElement.Parse("13");

        // Act
        var value = json.GetUInt16OrNull();

        // Assert
        value.Should().Be(13);
    }

    [Fact]
    public void GetUInt16OrNull_Null_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act
        var value = json.GetUInt16OrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetUInt16OrNull_NotNumber_Test()
    {
        // Arrange
        var json = JsonElement.Parse("true");

        // Act
        var value = json.GetUInt16OrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetUInt16OrNull_NotConvertible_Test()
    {
        // Arrange
        var json = JsonElement.Parse("3.14");

        // Act
        var value = json.GetUInt16OrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetUInt16Coerced_Positive_Direct_Test()
    {
        // Arrange
        var json = JsonElement.Parse("13");

        // Act
        var value = json.GetUInt16Coerced();

        // Assert
        value.Should().Be(13);
    }

    [Fact]
    public void GetUInt16Coerced_Positive_FromString_Test()
    {
        // Arrange
        var json = JsonElement.Parse("\"13\"");

        // Act
        var value = json.GetUInt16Coerced();

        // Assert
        value.Should().Be(13);
    }

    [Fact]
    public void GetUInt16Coerced_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act
        var act = () => json.GetUInt16Coerced();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetUInt32OrNull_Test()
    {
        // Arrange
        var json = JsonElement.Parse("13");

        // Act
        var value = json.GetUInt32OrNull();

        // Assert
        value.Should().Be(13);
    }

    [Fact]
    public void GetUInt32OrNull_Null_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act
        var value = json.GetUInt32OrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetUInt32OrNull_NotNumber_Test()
    {
        // Arrange
        var json = JsonElement.Parse("true");

        // Act
        var value = json.GetUInt32OrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetUInt32OrNull_NotConvertible_Test()
    {
        // Arrange
        var json = JsonElement.Parse("3.14");

        // Act
        var value = json.GetUInt32OrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetUInt32Coerced_Positive_Direct_Test()
    {
        // Arrange
        var json = JsonElement.Parse("13");

        // Act
        var value = json.GetUInt32Coerced();

        // Assert
        value.Should().Be(13);
    }

    [Fact]
    public void GetUInt32Coerced_Positive_FromString_Test()
    {
        // Arrange
        var json = JsonElement.Parse("\"13\"");

        // Act
        var value = json.GetUInt32Coerced();

        // Assert
        value.Should().Be(13);
    }

    [Fact]
    public void GetUInt32Coerced_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act
        var act = () => json.GetUInt32Coerced();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetUInt64OrNull_Test()
    {
        // Arrange
        var json = JsonElement.Parse("13");

        // Act
        var value = json.GetUInt64OrNull();

        // Assert
        value.Should().Be(13);
    }

    [Fact]
    public void GetUInt64OrNull_Null_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act
        var value = json.GetUInt64OrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetUInt64OrNull_NotNumber_Test()
    {
        // Arrange
        var json = JsonElement.Parse("true");

        // Act
        var value = json.GetUInt64OrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetUInt64OrNull_NotConvertible_Test()
    {
        // Arrange
        var json = JsonElement.Parse("3.14");

        // Act
        var value = json.GetUInt64OrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetUInt64Coerced_Positive_Direct_Test()
    {
        // Arrange
        var json = JsonElement.Parse("13");

        // Act
        var value = json.GetUInt64Coerced();

        // Assert
        value.Should().Be(13);
    }

    [Fact]
    public void GetUInt64Coerced_Positive_FromString_Test()
    {
        // Arrange
        var json = JsonElement.Parse("\"13\"");

        // Act
        var value = json.GetUInt64Coerced();

        // Assert
        value.Should().Be(13);
    }

    [Fact]
    public void GetUInt64Coerced_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act
        var act = () => json.GetUInt64Coerced();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetStringOrNull_Test()
    {
        // Arrange
        var json = JsonElement.Parse("\"foo\"");

        // Act
        var value = json.GetStringOrNull();

        // Assert
        value.Should().Be("foo");
    }

    [Fact]
    public void GetStringOrNull_Null_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act
        var value = json.GetStringOrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetStringOrNull_NotString_Test()
    {
        // Arrange
        var json = JsonElement.Parse("true");

        // Act
        var value = json.GetStringOrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetNonNullString_Test()
    {
        // Arrange
        var json = JsonElement.Parse("\"foo\"");

        // Act
        var value = json.GetNonNullString();

        // Assert
        value.Should().Be("foo");
    }

    [Fact]
    public void GetNonNullString__Null_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act
        var act = () => json.GetNonNullString();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetNonNullString__NotString_Test()
    {
        // Arrange
        var json = JsonElement.Parse("true");

        // Act
        var act = () => json.GetNonNullString();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetNonEmptyString_Test()
    {
        // Arrange
        var json = JsonElement.Parse("\"foo\"");

        // Act
        var value = json.GetNonEmptyString();

        // Assert
        value.Should().Be("foo");
    }

    [Fact]
    public void GetNonEmptyString__Null_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act
        var act = () => json.GetNonEmptyString();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetNonEmptyString__NotString_Test()
    {
        // Arrange
        var json = JsonElement.Parse("true");

        // Act
        var act = () => json.GetNonEmptyString();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetNonEmptyString__EmptyString_Test()
    {
        // Arrange
        var json = JsonElement.Parse("\"\"");

        // Act
        var act = () => json.GetNonEmptyString();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetNonWhiteSpaceString_Test()
    {
        // Arrange
        var json = JsonElement.Parse("\"foo\"");

        // Act
        var value = json.GetNonWhiteSpaceString();

        // Assert
        value.Should().Be("foo");
    }

    [Fact]
    public void GetNonWhiteSpaceString__Null_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act
        var act = () => json.GetNonWhiteSpaceString();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetNonWhiteSpaceString__NotString_Test()
    {
        // Arrange
        var json = JsonElement.Parse("true");

        // Act
        var act = () => json.GetNonWhiteSpaceString();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetNonWhiteSpaceString__EmptyString_Test()
    {
        // Arrange
        var json = JsonElement.Parse("\"\"");

        // Act
        var act = () => json.GetNonWhiteSpaceString();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetNonWhiteSpaceString__WhiteSpaceString_Test()
    {
        // Arrange
        var json = JsonElement.Parse("\"  \"");

        // Act
        var act = () => json.GetNonWhiteSpaceString();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetGuidOrNull_Test()
    {
        // Arrange
        var json = JsonElement.Parse("\"b2567a71-6966-49b1-8976-ad3f4e397b05\"");

        // Act
        var value = json.GetGuidOrNull();

        // Assert
        value.Should().Be(Guid.Parse("b2567a71-6966-49b1-8976-ad3f4e397b05"));
    }

    [Fact]
    public void GetGuidOrNull_Null_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act
        var value = json.GetGuidOrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetGuidOrNull_NotString_Test()
    {
        // Arrange
        var json = JsonElement.Parse("true");

        // Act
        var value = json.GetGuidOrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetGuidOrNull_NotConvertible_Test()
    {
        // Arrange
        var json = JsonElement.Parse("\"foo\"");

        // Act
        var value = json.GetGuidOrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetDateTimeOrNull_Test()
    {
        // Arrange
        var json = JsonElement.Parse("\"2014-01-01T23:28:56.782Z\"");

        // Act
        var value = json.GetDateTimeOrNull();

        // Assert
        value
            .Should()
            .Be(
                DateTime.Parse(
                    "2014-01-01T23:28:56.782Z",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AdjustToUniversal
                )
            );
    }

    [Fact]
    public void GetDateTimeOrNull_Null_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act
        var value = json.GetDateTimeOrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetDateTimeOrNull_NotString_Test()
    {
        // Arrange
        var json = JsonElement.Parse("true");

        // Act
        var value = json.GetDateTimeOrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetDateTimeOrNull_NotConvertible_Test()
    {
        // Arrange
        var json = JsonElement.Parse("\"foo\"");

        // Act
        var value = json.GetDateTimeOrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetDateTimeOffsetOrNull_Test()
    {
        // Arrange
        var json = JsonElement.Parse("\"2014-01-01T23:28:56.782Z\"");

        // Act
        var value = json.GetDateTimeOffsetOrNull();

        // Assert
        value
            .Should()
            .Be(DateTimeOffset.Parse("2014-01-01T23:28:56.782Z", CultureInfo.InvariantCulture));
    }

    [Fact]
    public void GetDateTimeOffsetOrNull_Null_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act
        var value = json.GetDateTimeOffsetOrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetDateTimeOffsetOrNull_NotString_Test()
    {
        // Arrange
        var json = JsonElement.Parse("true");

        // Act
        var value = json.GetDateTimeOffsetOrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetDateTimeOffsetOrNull_NotConvertible_Test()
    {
        // Arrange
        var json = JsonElement.Parse("\"foo\"");

        // Act
        var value = json.GetDateTimeOffsetOrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetBytesFromBase64OrNull_Test()
    {
        // Arrange
        var json = JsonElement.Parse("\"Zm9v\"");

        // Act
        var value = json.GetBytesFromBase64OrNull();

        // Assert
        value.Should().Equal(Encoding.UTF8.GetBytes("foo"));
    }

    [Fact]
    public void GetBytesFromBase64OrNull_Null_Test()
    {
        // Arrange
        var json = JsonElement.Parse("null");

        // Act
        var value = json.GetBytesFromBase64OrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetBytesFromBase64OrNull_NotString_Test()
    {
        // Arrange
        var json = JsonElement.Parse("true");

        // Act
        var value = json.GetBytesFromBase64OrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetBytesFromBase64OrNull_NotConvertible_Test()
    {
        // Arrange
        var json = JsonElement.Parse("\"foo\"");

        // Act
        var value = json.GetBytesFromBase64OrNull();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void Parse_Test()
    {
        // Act
        var json = JsonElement.Parse(
            """
            {"foo":"bar"}
            """
        );

        // Assert
        json.GetProperty("foo").GetString().Should().Be("bar");
    }

    [Fact]
    public void TryParse_Test()
    {
        // Act
        var json = JsonElement.TryParse(
            """
            {"foo":"bar"}
            """
        );

        // Assert
        json.Should().NotBeNull();
        json?.GetProperty("foo").GetString().Should().Be("bar");
    }

    [Fact]
    public void TryParse_Invalid_Test()
    {
        // Act
        var json = JsonElement.TryParse(
            """
            {"foo":bar}
            """
        );

        // Assert
        json.Should().BeNull();
    }

    [Fact]
    public void FromDictionary_Test()
    {
        // Arrange
        var dictionary = new Dictionary<string, object?> { ["foo"] = "Hello world!", ["bar"] = 13 };

        // Act
        var json = JsonElement.FromDictionary(dictionary);

        // Assert
        json.GetProperty("foo").GetString().Should().Be("Hello world!");
        json.GetProperty("bar").GetInt32().Should().Be(13);
    }
}
