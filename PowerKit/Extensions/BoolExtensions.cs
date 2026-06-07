namespace PowerKit.Extensions;

/// <summary>
/// Extensions for <see cref="bool" />.
/// </summary>
public static class BoolExtensions
{
    extension(bool)
    {
        /// <summary>
        /// Parses the string as a <see cref="bool" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static bool? ParseOrNull(string? str) =>
            bool.TryParse(str, out var result) ? result : null;

        /// <summary>
        /// Parses the string as a <see cref="bool" />, returning <paramref name="defaultValue" /> if parsing fails.
        /// </summary>
        public static bool ParseOrDefault(string? str, bool defaultValue = default) =>
            bool.ParseOrNull(str) ?? defaultValue;
    }
}
