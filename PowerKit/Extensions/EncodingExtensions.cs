using System;
using System.Text;

namespace PowerKit.Extensions;

file sealed class NoPreambleEncoding(Encoding inner) : Encoding
{
    public override string BodyName => inner.BodyName;
    public override string EncodingName => inner.EncodingName;
    public override string HeaderName => inner.HeaderName;
    public override string WebName => inner.WebName;
    public override int CodePage => inner.CodePage;
    public override bool IsBrowserDisplay => inner.IsBrowserDisplay;
    public override bool IsBrowserSave => inner.IsBrowserSave;
    public override bool IsMailNewsDisplay => inner.IsMailNewsDisplay;
    public override bool IsMailNewsSave => inner.IsMailNewsSave;
    public override bool IsSingleByte => inner.IsSingleByte;

    public override byte[] GetPreamble() => Array.Empty<byte>();

    public override int GetByteCount(char[] chars, int index, int count) =>
        inner.GetByteCount(chars, index, count);

    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) =>
        inner.GetBytes(chars, charIndex, charCount, bytes, byteIndex);

    public override int GetCharCount(byte[] bytes, int index, int count) =>
        inner.GetCharCount(bytes, index, count);

    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) =>
        inner.GetChars(bytes, byteIndex, byteCount, chars, charIndex);

    public override int GetMaxByteCount(int charCount) => inner.GetMaxByteCount(charCount);

    public override int GetMaxCharCount(int byteCount) => inner.GetMaxCharCount(byteCount);
}

file static class EncodingEx
{
    public static Encoding Utf8WithoutBom { get; } = Encoding.UTF8.WithoutPreamble();
}

internal static class EncodingExtensions
{
    extension(Encoding encoding)
    {
        /// <summary>
        /// Gets an instance of the UTF-8 encoding that does not emit a byte order mark (BOM).
        /// </summary>
        public static Encoding Utf8WithoutBom => EncodingEx.Utf8WithoutBom;

        /// <summary>
        /// Creates a derived encoding that produces an empty preamble, regardless of the original encoding's preamble.
        /// </summary>
        public Encoding WithoutPreamble() =>
            encoding.GetPreamble().Length == 0
                ? encoding
                : new NoPreambleEncoding(encoding);
    }
}
