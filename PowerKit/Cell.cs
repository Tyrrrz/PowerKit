#nullable enable
using System.Diagnostics.CodeAnalysis;

namespace PowerKit;

/// <summary>
/// Container for a value that may or may not be set.
/// Essentially <see cref="System.Nullable{T}" />, but for cases where null is also a valid value.
/// </summary>
#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class Cell<T>
{
    private T _value = default!;

    /// <summary>
    /// Gets a value indicating whether the cell has no value stored.
    /// </summary>
    public bool IsEmpty { get; private set; } = true;

    /// <summary>
    /// Clears the value stored in the cell, returning it to an unset state.
    /// </summary>
    public void Clear()
    {
        _value = default!;
        IsEmpty = true;
    }

    /// <summary>
    /// Stores the specified value in the cell.
    /// </summary>
    public void Store(T value)
    {
        _value = value;
        IsEmpty = false;
    }

    /// <summary>
    /// Tries to retrieve the value stored in the cell.
    /// Returns <see langword="true" /> if a value has been stored, <see langword="false" /> otherwise.
    /// </summary>
    public bool TryOpen(out T value)
    {
        if (!IsEmpty)
        {
            value = _value;
            return true;
        }

        value = default!;
        return false;
    }

    /// <summary>
    /// Retrieves the value stored in the cell, or <paramref name="defaultValue" /> if no value has been stored.
    /// </summary>
    public T OpenOrDefault(T defaultValue = default!) =>
        TryOpen(out var value) ? value : defaultValue;
}
