namespace PowerKit.Extensions;

internal static class BoolParsingExtensions
{
    extension(bool)
    {
        /// <summary>
        /// Parses the string as a <see cref="bool" />, returning <see langword="null" /> if parsing fails.
        /// </summary>
        public static bool? ParseOrNull(string? str) =>
            bool.TryParse(str, out var result) ? result : null;
    }
}
