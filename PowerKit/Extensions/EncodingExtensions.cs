using System.Text;

namespace PowerKit.Extensions;

file sealed class NoPreambleEncoding : Encoding
{
    // Cloned for isolation — prevents mutations to shared singletons like Encoding.UTF8.
    private readonly Encoding _inner;

    public NoPreambleEncoding(Encoding inner)
    {
        // Clone for isolation — prevents mutations to shared singletons like Encoding.UTF8,
        // and ensures the clone carries the source's fallbacks into all encode/decode operations.
        _inner = (Encoding)inner.Clone();
    }

    public override string BodyName => _inner.BodyName;
    public override string EncodingName => _inner.EncodingName;
    public override string HeaderName => _inner.HeaderName;
    public override string WebName => _inner.WebName;
    public override int CodePage => _inner.CodePage;
    public override bool IsBrowserDisplay => _inner.IsBrowserDisplay;
    public override bool IsBrowserSave => _inner.IsBrowserSave;
    public override bool IsMailNewsDisplay => _inner.IsMailNewsDisplay;
    public override bool IsMailNewsSave => _inner.IsMailNewsSave;
    public override bool IsSingleByte => _inner.IsSingleByte;

    public override byte[] GetPreamble() => new byte[0];

    public override int GetByteCount(char[] chars, int index, int count) =>
        _inner.GetByteCount(chars, index, count);

    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) =>
        _inner.GetBytes(chars, charIndex, charCount, bytes, byteIndex);

    public override int GetCharCount(byte[] bytes, int index, int count) =>
        _inner.GetCharCount(bytes, index, count);

    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) =>
        _inner.GetChars(bytes, byteIndex, byteCount, chars, charIndex);

    public override int GetMaxByteCount(int charCount) => _inner.GetMaxByteCount(charCount);

    public override int GetMaxCharCount(int byteCount) => _inner.GetMaxCharCount(byteCount);

    public override Encoder GetEncoder() => _inner.GetEncoder();

    public override Decoder GetDecoder() => _inner.GetDecoder();
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
            encoding.GetPreamble().Length > 0
                ? new NoPreambleEncoding(encoding)
                : encoding;
    }
}
