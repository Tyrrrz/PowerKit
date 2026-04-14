using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PowerKit.Extensions;

internal static class ZipArchiveEntryExtensions
{
    extension(ZipArchiveEntry entry)
    {
        /// <summary>
        /// Reads all bytes from the zip archive entry.
        /// </summary>
        public byte[] ReadAllBytes()
        {
            var length = checked((int)entry.Length);
            if (length <= 0)
            {
                return [];
            }

            using var stream = entry.Open();
            var bytes = new byte[length];
            stream.ReadExactly(bytes);

            return bytes;
        }

        /// <summary>
        /// Writes all bytes to the zip archive entry.
        /// </summary>
        public void WriteAllBytes(byte[] bytes)
        {
            using var stream = entry.Open();
            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Reads all text from the zip archive entry using the specified encoding.
        /// </summary>
        public string ReadAllText(Encoding? encoding = null)
        {
            using var stream = entry.Open();
            using var reader = new StreamReader(
                stream,
                encoding ?? new UTF8Encoding(false),
                detectEncodingFromByteOrderMarks: true);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Writes all text to the zip archive entry using the specified encoding.
        /// </summary>
        public void WriteAllText(string text, Encoding? encoding = null)
        {
            var bytes = (encoding ?? new UTF8Encoding(false)).GetBytes(text);
            entry.WriteAllBytes(bytes);
        }

        /// <summary>
        /// Reads all lines from the zip archive entry using the specified encoding.
        /// </summary>
        public string[] ReadAllLines(Encoding? encoding = null)
        {
            using var stream = entry.Open();
            using var reader = new StreamReader(
                stream,
                encoding ?? new UTF8Encoding(false),
                detectEncodingFromByteOrderMarks: true);

            var lines = new List<string>();
            string? line;
            while ((line = reader.ReadLine()) is not null)
                lines.Add(line);

            return lines.ToArray();
        }

        /// <summary>
        /// Writes all lines to the zip archive entry using the specified encoding.
        /// </summary>
        public void WriteAllLines(IEnumerable<string> lines, Encoding? encoding = null)
        {
            var text = string.Join(Environment.NewLine, lines);
            entry.WriteAllText(text, encoding);
        }

        /// <summary>
        /// Reads all bytes from the zip archive entry asynchronously.
        /// </summary>
        public async Task<byte[]> ReadAllBytesAsync(
            CancellationToken cancellationToken = default
        )
        {
            var length = checked((int)entry.Length);
            if (length == 0)
                return [];

            using var stream = entry.Open();
            var bytes = new byte[length];
            await stream.ReadExactlyAsync(bytes, cancellationToken).ConfigureAwait(false);

            return bytes;
        }

        /// <summary>
        /// Writes all bytes to the zip archive entry asynchronously.
        /// </summary>
        public async Task WriteAllBytesAsync(
            byte[] bytes,
            CancellationToken cancellationToken = default
        )
        {
            using var stream = entry.Open();
            await stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken).ConfigureAwait(false);
            await stream.FlushAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Reads all text from the zip archive entry using the specified encoding asynchronously.
        /// </summary>
        public async Task<string> ReadAllTextAsync(
            Encoding? encoding = null,
            CancellationToken cancellationToken = default
        )
        {
            var bytes = await entry.ReadAllBytesAsync(cancellationToken).ConfigureAwait(false);
            return (encoding ?? new UTF8Encoding(false)).GetString(bytes);
        }

        /// <summary>
        /// Writes all text to the zip archive entry using the specified encoding asynchronously.
        /// </summary>
        public async Task WriteAllTextAsync(
            string text,
            Encoding? encoding = null,
            CancellationToken cancellationToken = default
        )
        {
            var bytes = (encoding ?? new UTF8Encoding(false)).GetBytes(text);
            await entry.WriteAllBytesAsync(bytes, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Reads all lines from the zip archive entry using the specified encoding asynchronously.
        /// </summary>
        public async Task<string[]> ReadAllLinesAsync(
            Encoding? encoding = null,
            CancellationToken cancellationToken = default
        )
        {
            var text = await entry.ReadAllTextAsync(encoding, cancellationToken).ConfigureAwait(false);
            return text.Split(["\r\n", "\n", "\r"]);
        }

        /// <summary>
        /// Writes all lines to the zip archive entry using the specified encoding asynchronously.
        /// </summary>
        public async Task WriteAllLinesAsync(
            IEnumerable<string> lines,
            Encoding? encoding = null,
            CancellationToken cancellationToken = default
        )
        {
            var text = string.Join(Environment.NewLine, lines);
            await entry.WriteAllTextAsync(text, encoding, cancellationToken).ConfigureAwait(false);
        }
    }
}
