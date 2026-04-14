using System.Text;

namespace PowerKit.Extensions;

file static class EncodingEx
{
    public static Encoding Utf8WithoutBom { get; } = Encoding.ReadOnly(new UTF8Encoding(false));
}

internal static class EncodingExtensions
{
    extension(Encoding)
    {
        /// <summary>
        /// Gets an instance of the UTF-8 encoding that does not emit a byte order mark (BOM).
        /// </summary>
        public static Encoding Utf8WithoutBom => EncodingEx.Utf8WithoutBom;
    }
}
