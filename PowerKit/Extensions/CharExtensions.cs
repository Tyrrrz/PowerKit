namespace PowerKit.Extensions;

/// <summary>
/// Extensions for <see cref="char" />.
/// </summary>
public static class CharExtensions
{
    extension(char c)
    {
        /// <summary>
        /// Returns a string that contains the character repeated the specified number of times.
        /// </summary>
        public string Repeat(int count) => new(c, count);
    }
}
