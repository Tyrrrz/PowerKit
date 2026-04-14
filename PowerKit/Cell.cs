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
    private bool _isValueSet;
    private T _value = default!;

    /// <summary>
    /// Stores the specified value in the cell.
    /// </summary>
    public void Store(T value)
    {
        _value = value;
        _isValueSet = true;
    }

    /// <summary>
    /// Tries to retrieve the value stored in the cell.
    /// Returns <see langword="true" /> if a value has been stored, <see langword="false" /> otherwise.
    /// </summary>
    public bool TryOpen(out T value)
    {
        if (_isValueSet)
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
