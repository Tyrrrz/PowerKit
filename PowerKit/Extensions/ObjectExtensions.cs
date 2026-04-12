using System;

namespace PowerKit.Extensions;

// Kept separate from FunctionalExtensions because C# (CS0111) does not allow two generic
// methods with identical parameter types that differ only by constraint (class vs struct).
internal static class ObjectExtensions
{
    /// <summary>
    /// Returns <see langword="null" /> if the value matches the specified predicate; otherwise, returns the value.
    /// </summary>
    public static T? NullIf<T>(this T value, Func<T, bool> predicate)
        where T : class => !predicate(value) ? value : null;
}
