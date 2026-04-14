using System.Text;

namespace PowerKit.Extensions;

internal static class EncodingExtensions
{
    private static readonly Encoding Utf8WithoutBomValue = new UTF8Encoding(false);

    extension(Encoding)
    {
        /// <summary>
        /// Gets an instance of the UTF-8 encoding that does not emit a byte order mark (BOM).
        /// </summary>
        public static Encoding Utf8WithoutBom => Utf8WithoutBomValue;
    }
}
