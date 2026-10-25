# PowerKit.Json

[![Version](https://img.shields.io/nuget/v/PowerKit.Json.svg)](https://nuget.org/packages/PowerKit.Json)
[![Downloads](https://img.shields.io/nuget/dt/PowerKit.Json.svg)](https://nuget.org/packages/PowerKit.Json)

**PowerKit.Json** is an extension package for **PowerKit** that provides a set of helpful utilities for types defined in the [`System.Text.Json`](https://learn.microsoft.com/dotnet/api/system.text.json) namespace.

## Install

- 📦 [NuGet](https://nuget.org/packages/PowerKit.Json): `dotnet add package PowerKit.Json`

> [!TIP]
> You can use [**Binternal**](https://github.com/Tyrrrz/Binternal) to internalize this library if you prefer to avoid taking an external dependency.

## Usage

### Parsing JsonElement

You can use the static extensions methods on the `JsonElement` class to parse it directly from a JSON string, without having to deal with `JsonDocument` in the process:

```csharp
using PowerKit.Json.Extensions;

var jsonRaw =
    """
    {
      "foo": "bar"
    }
    """;

var jsonElement = JsonElement.Parse(jsonRaw); // returns JsonElement
var jsonElement = JsonElement.TryParse(jsonRaw); // returns null in case of invalid JSON
```

### Null-safe reading

This library offers many extension methods for `JsonElement` that allow you to read its content in a more fault-tolerant way:

```csharp
using PowrKit.Json.Extensions;

var jsonElement = ...;

// Gets a property or returns null if:
// - element is not an object
// - property does not exist
// - property value is null
var maybeProperty = jsonElement.GetPropertyOrNull("prop");

// Gets an array child or returns null if:
// - element is not an array
// - index is out of bounds
// - child is null
var maybeChild = jsonElement.GetByIndexOrNull(3);

// Gets the value converted into the specified type or returns null if:
// - element is null
// - element kind does not match the specified type
// - the value cannot be parsed into the specified type
var maybeString = jsonElement.GetStringOrNull();
var maybeInt32 = jsonElement.GetInt32OrNull();
var maybeGuid = jsonElement.GetGuidOrNull();

// Gets the value coerced into the specified type or returns null if:
// - element is null
// - element kind does not match the specified type or a string
// - the value cannot be parsed into the specified type
var maybeInt32Coerced = jsonElement.GetInt32CoercedOrNull();
var maybeDoubleCoerced = jsonElement.GetDoubleCoercedOrNull();

// Enumerates an array or returns null if:
// - element is not an array
var arrayEnumerator = jsonElement.EnumerateArrayOrNull();

// Enumerates an object or returns null if:
// - element is not an object
var objectEnumerator = jsonElement.EnumerateObjectOrNull();

// Enumerates an array or returns an empty enumerator if:
// - element is not an array
foreach (var child in jsonElement.EnumerateArrayOrEmpty())
{
    // ...
}

// Enumerates an object or returns an empty enumerator if:
// - element is not an object
foreach (var (name, child) in jsonElement.EnumerateObjectOrEmpty())
{
    // ...
}
```

Most of these methods can also be chained together using the null-conditional operator:

```csharp
// Returns null if:
// - element is not an object
// - property does not exist
// - property value is null
// - property value cannot be converted into the specified type
var maybeInt32 = jsonElement.GetPropertyOrNull("prop")?.GetInt32OrNull();
```

### Null-safe writing

Similarly, there are also extension methods for `Utf8JsonWriter` that allow writing nullable versions of common value types:

```csharp
using PowerKit.Json.Extensions;

var writer = new Utf8JsonWriter(...);

// Writes "prop":true
writer.WriteBoolean("prop", new bool?(true));

// Writes "prop":null
writer.WriteBoolean("prop", new bool?());
```

### Parsing JSON from HTTP

To make it easier to read JSON that comes from HTTP responses, this library also provides a few extension methods for `HttpClient`:

```csharp
using PowerKit.Json.Extensions;

var http = new HttpClient();

// Send a GET request and retrieve JSON directly
var json = await http.GetJsonAsync("..."); // returns JsonElement
```

As well as `HttpContent`:

```csharp
using PowerKit.Json.Extensions;

var http = new HttpClient();

// Read JSON from content
using var request = new HttpRequestMessage(HttpMethod.Post, "...");
using var response = await http.SendAsync(request);
var json = await response.Content.ReadAsJsonAsync(); // returns JsonElement
```
