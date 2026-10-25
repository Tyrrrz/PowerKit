using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;

namespace PowerKit.Json.Extensions;

/// <summary>
/// Extensions for <see cref="Utf8JsonWriter" />.
/// </summary>
public static class Utf8JsonWriterExtensions
{
    extension(Utf8JsonWriter writer)
    {
        /// <summary>
        /// Writes <paramref name="value" /> as a JSON boolean, or a JSON null if <paramref name="value" /> is null.
        /// </summary>
        public void WriteBooleanValue(bool? value)
        {
            if (value is not null)
                writer.WriteBooleanValue(value.Value);
            else
                writer.WriteNullValue();
        }

        /// <summary>
        /// Writes a property named <paramref name="propertyName" /> with a boolean value,
        /// or a JSON null if <paramref name="value" /> is null.
        /// </summary>
        public void WriteBoolean(string propertyName, bool? value)
        {
            writer.WritePropertyName(propertyName);
            writer.WriteBooleanValue(value);
        }

        /// <summary>
        /// Writes <paramref name="value" /> as a JSON number, or a JSON null if <paramref name="value" /> is null.
        /// </summary>
        public void WriteNumberValue(byte? value)
        {
            if (value is not null)
                writer.WriteNumberValue(value.Value);
            else
                writer.WriteNullValue();
        }

        /// <inheritdoc cref="WriteNumberValue(Utf8JsonWriter, byte?)" />
        public void WriteNumberValue(decimal? value)
        {
            if (value is not null)
                writer.WriteNumberValue(value.Value);
            else
                writer.WriteNullValue();
        }

        /// <inheritdoc cref="WriteNumberValue(Utf8JsonWriter, byte?)" />
        public void WriteNumberValue(double? value)
        {
            if (value is not null)
                writer.WriteNumberValue(value.Value);
            else
                writer.WriteNullValue();
        }

        /// <inheritdoc cref="WriteNumberValue(Utf8JsonWriter, byte?)" />
        public void WriteNumberValue(float? value)
        {
            if (value is not null)
                writer.WriteNumberValue(value.Value);
            else
                writer.WriteNullValue();
        }

        /// <inheritdoc cref="WriteNumberValue(Utf8JsonWriter, byte?)" />
        public void WriteNumberValue(int? value)
        {
            if (value is not null)
                writer.WriteNumberValue(value.Value);
            else
                writer.WriteNullValue();
        }

        /// <inheritdoc cref="WriteNumberValue(Utf8JsonWriter, byte?)" />
        public void WriteNumberValue(long? value)
        {
            if (value is not null)
                writer.WriteNumberValue(value.Value);
            else
                writer.WriteNullValue();
        }

        /// <inheritdoc cref="WriteNumberValue(Utf8JsonWriter, byte?)" />
        public void WriteNumberValue(sbyte? value)
        {
            if (value is not null)
                writer.WriteNumberValue(value.Value);
            else
                writer.WriteNullValue();
        }

        /// <inheritdoc cref="WriteNumberValue(Utf8JsonWriter, byte?)" />
        public void WriteNumberValue(short? value)
        {
            if (value is not null)
                writer.WriteNumberValue(value.Value);
            else
                writer.WriteNullValue();
        }

        /// <inheritdoc cref="WriteNumberValue(Utf8JsonWriter, byte?)" />
        public void WriteNumberValue(uint? value)
        {
            if (value is not null)
                writer.WriteNumberValue(value.Value);
            else
                writer.WriteNullValue();
        }

        /// <inheritdoc cref="WriteNumberValue(Utf8JsonWriter, byte?)" />
        public void WriteNumberValue(ulong? value)
        {
            if (value is not null)
                writer.WriteNumberValue(value.Value);
            else
                writer.WriteNullValue();
        }

        /// <inheritdoc cref="WriteNumberValue(Utf8JsonWriter, byte?)" />
        public void WriteNumberValue(ushort? value)
        {
            if (value is not null)
                writer.WriteNumberValue(value.Value);
            else
                writer.WriteNullValue();
        }

        /// <summary>
        /// Writes a property named <paramref name="propertyName" /> with a numeric value,
        /// or a JSON null if <paramref name="value" /> is null.
        /// </summary>
        public void WriteNumber(string propertyName, byte? value)
        {
            writer.WritePropertyName(propertyName);
            writer.WriteNumberValue(value);
        }

        /// <inheritdoc cref="WriteNumber(Utf8JsonWriter, string, byte?)" />
        public void WriteNumber(string propertyName, decimal? value)
        {
            writer.WritePropertyName(propertyName);
            writer.WriteNumberValue(value);
        }

        /// <inheritdoc cref="WriteNumber(Utf8JsonWriter, string, byte?)" />
        public void WriteNumber(string propertyName, double? value)
        {
            writer.WritePropertyName(propertyName);
            writer.WriteNumberValue(value);
        }

        /// <inheritdoc cref="WriteNumber(Utf8JsonWriter, string, byte?)" />
        public void WriteNumber(string propertyName, float? value)
        {
            writer.WritePropertyName(propertyName);
            writer.WriteNumberValue(value);
        }

        /// <inheritdoc cref="WriteNumber(Utf8JsonWriter, string, byte?)" />
        public void WriteNumber(string propertyName, int? value)
        {
            writer.WritePropertyName(propertyName);
            writer.WriteNumberValue(value);
        }

        /// <inheritdoc cref="WriteNumber(Utf8JsonWriter, string, byte?)" />
        public void WriteNumber(string propertyName, long? value)
        {
            writer.WritePropertyName(propertyName);
            writer.WriteNumberValue(value);
        }

        /// <inheritdoc cref="WriteNumber(Utf8JsonWriter, string, byte?)" />
        public void WriteNumber(string propertyName, sbyte? value)
        {
            writer.WritePropertyName(propertyName);
            writer.WriteNumberValue(value);
        }

        /// <inheritdoc cref="WriteNumber(Utf8JsonWriter, string, byte?)" />
        public void WriteNumber(string propertyName, short? value)
        {
            writer.WritePropertyName(propertyName);
            writer.WriteNumberValue(value);
        }

        /// <inheritdoc cref="WriteNumber(Utf8JsonWriter, string, byte?)" />
        public void WriteNumber(string propertyName, uint? value)
        {
            writer.WritePropertyName(propertyName);
            writer.WriteNumberValue(value);
        }

        /// <inheritdoc cref="WriteNumber(Utf8JsonWriter, string, byte?)" />
        public void WriteNumber(string propertyName, ulong? value)
        {
            writer.WritePropertyName(propertyName);
            writer.WriteNumberValue(value);
        }

        /// <inheritdoc cref="WriteNumber(Utf8JsonWriter, string, byte?)" />
        public void WriteNumber(string propertyName, ushort? value)
        {
            writer.WritePropertyName(propertyName);
            writer.WriteNumberValue(value);
        }

        /// <summary>
        /// Writes <paramref name="value" /> as a JSON string, or a JSON null if <paramref name="value" /> is null.
        /// </summary>
        public void WriteStringValue(DateTime? value)
        {
            if (value is not null)
                writer.WriteStringValue(value.Value);
            else
                writer.WriteNullValue();
        }

        /// <inheritdoc cref="WriteStringValue(Utf8JsonWriter, DateTime?)" />
        public void WriteStringValue(DateTimeOffset? value)
        {
            if (value is not null)
                writer.WriteStringValue(value.Value);
            else
                writer.WriteNullValue();
        }

        /// <inheritdoc cref="WriteStringValue(Utf8JsonWriter, DateTime?)" />
        public void WriteStringValue(Guid? value)
        {
            if (value is not null)
                writer.WriteStringValue(value.Value);
            else
                writer.WriteNullValue();
        }

        /// <summary>
        /// Writes a property named <paramref name="propertyName" /> with a string value,
        /// or a JSON null if <paramref name="value" /> is null.
        /// </summary>
        public void WriteString(string propertyName, DateTime? value)
        {
            writer.WritePropertyName(propertyName);
            writer.WriteStringValue(value);
        }

        /// <inheritdoc cref="WriteString(Utf8JsonWriter, string, DateTime?)" />
        public void WriteString(string propertyName, DateTimeOffset? value)
        {
            writer.WritePropertyName(propertyName);
            writer.WriteStringValue(value);
        }

        /// <inheritdoc cref="WriteString(Utf8JsonWriter, string, DateTime?)" />
        public void WriteString(string propertyName, Guid? value)
        {
            writer.WritePropertyName(propertyName);
            writer.WriteStringValue(value);
        }

        /// <summary>
        /// Writes <paramref name="value" /> as a JSON boolean, or a JSON null if <paramref name="value" /> is null.
        /// </summary>
        public void WriteValue(object? value)
        {
            switch (value)
            {
                case null:
                    writer.WriteNullValue();
                    break;
                case bool actualValue:
                    writer.WriteBooleanValue(actualValue);
                    break;
                case byte actualValue:
                    writer.WriteNumberValue(actualValue);
                    break;
                case decimal actualValue:
                    writer.WriteNumberValue(actualValue);
                    break;
                case double actualValue:
                    writer.WriteNumberValue(actualValue);
                    break;
                case short actualValue:
                    writer.WriteNumberValue(actualValue);
                    break;
                case int actualValue:
                    writer.WriteNumberValue(actualValue);
                    break;
                case long actualValue:
                    writer.WriteNumberValue(actualValue);
                    break;
                case sbyte actualValue:
                    writer.WriteNumberValue(actualValue);
                    break;
                case float actualValue:
                    writer.WriteNumberValue(actualValue);
                    break;
                case ushort actualValue:
                    writer.WriteNumberValue(actualValue);
                    break;
                case uint actualValue:
                    writer.WriteNumberValue(actualValue);
                    break;
                case ulong actualValue:
                    writer.WriteNumberValue(actualValue);
                    break;
                case DateTime actualValue:
                    writer.WriteStringValue(actualValue);
                    break;
                case DateTimeOffset actualValue:
                    writer.WriteStringValue(actualValue);
                    break;
                case Guid actualValue:
                    writer.WriteStringValue(actualValue);
                    break;
                case string actualValue:
                    writer.WriteStringValue(actualValue);
                    break;
                case JsonEncodedText actualValue:
                    writer.WriteStringValue(actualValue);
                    break;
                case IEnumerable<KeyValuePair<string, object?>> actualValue:
                    writer.WriteStartObject();
                    foreach (var item in actualValue)
                    {
                        writer.WritePropertyName(item.Key);
                        WriteValue(writer, item.Value);
                    }
                    writer.WriteEndObject();
                    break;
                case IEnumerable actualValue:
                    writer.WriteStartArray();
                    foreach (var item in actualValue)
                    {
                        WriteValue(writer, item);
                    }
                    writer.WriteEndArray();
                    break;
                default:
                    throw new ArgumentException(
                        $"Unsupported value type: '{value?.GetType().FullName}'.",
                        nameof(value)
                    );
            }
        }

        /// <inheritdoc cref="WriteValue(Utf8JsonWriter, object?)" />.
        public void Write(string propertyName, object? value)
        {
            writer.WritePropertyName(propertyName);
            writer.WriteValue(value);
        }
    }
}
