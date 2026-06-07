#if !NETFRAMEWORK || NET45_OR_GREATER
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PowerKit.Extensions;

/// <summary>
/// Extensions for <see cref="ZipArchiveEntry" />.
/// </summary>
public static class ZipArchiveEntryExtensions
{
    extension(ZipArchiveEntry entry)
    {
        /// <summary>
        /// Reads all bytes from the zip archive entry.
        /// </summary>
        public byte[] ReadAllBytes()
        {
            using var stream = entry.Open();
            using var buffer = new MemoryStream();

            stream.CopyTo(buffer);

            return buffer.ToArray();
        }

        /// <summary>
        /// Reads all bytes from the zip archive entry asynchronously.
        /// </summary>
        public async Task<byte[]> ReadAllBytesAsync(CancellationToken cancellationToken = default)
        {
            using var stream = entry.Open();
            using var buffer = new MemoryStream();

            await stream.CopyToAsync(buffer, cancellationToken).ConfigureAwait(false);

            return buffer.ToArray();
        }

        /// <summary>
        /// Reads all lines from the zip archive entry using the specified encoding.
        /// </summary>
        public string[] ReadAllLines(Encoding? encoding = null)
        {
            using var stream = entry.Open();
            using var reader = new StreamReader(stream, encoding ?? Encoding.UTF8);

            var lines = new List<string>();
            while (reader.ReadLine() is { } line)
            {
                lines.Add(line);
            }

            return lines.ToArray();
        }

        /// <summary>
        /// Reads all lines from the zip archive entry using the specified encoding asynchronously.
        /// </summary>
        public async Task<string[]> ReadAllLinesAsync(
            Encoding? encoding = null,
            CancellationToken cancellationToken = default
        )
        {
            using var stream = entry.Open();
            using var reader = new StreamReader(stream, encoding ?? Encoding.UTF8);

            var lines = new List<string>();
            while (await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false) is { } line)
            {
                lines.Add(line);
            }

            return lines.ToArray();
        }

        /// <summary>
        /// Reads all text from the zip archive entry using the specified encoding.
        /// </summary>
        public string ReadAllText(Encoding? encoding = null)
        {
            using var stream = entry.Open();
            using var reader = new StreamReader(stream, encoding ?? Encoding.UTF8);

            return reader.ReadToEnd();
        }

        /// <summary>
        /// Reads all text from the zip archive entry using the specified encoding asynchronously.
        /// </summary>
        public async Task<string> ReadAllTextAsync(
            Encoding? encoding = null,
            CancellationToken cancellationToken = default
        )
        {
            using var stream = entry.Open();
            using var reader = new StreamReader(stream, encoding ?? Encoding.UTF8);

            return await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
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
        /// Writes all bytes to the zip archive entry asynchronously.
        /// </summary>
        public async Task WriteAllBytesAsync(
            byte[] bytes,
            CancellationToken cancellationToken = default
        )
        {
            using var stream = entry.Open();
            await stream
                .WriteAsync(bytes, 0, bytes.Length, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Writes all lines to the zip archive entry using the specified encoding.
        /// </summary>
        public void WriteAllLines(IEnumerable<string> lines, Encoding? encoding = null)
        {
            using var stream = entry.Open();
            using var writer = new StreamWriter(stream, encoding ?? Encoding.Utf8WithoutBom);

            foreach (var line in lines)
            {
                writer.WriteLine(line);
            }
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
            using var stream = entry.Open();
            using var writer = new StreamWriter(stream, encoding ?? Encoding.Utf8WithoutBom);

            foreach (var line in lines)
            {
                await writer
                    .WriteLineAsync(line.AsMemory(), cancellationToken)
                    .ConfigureAwait(false);
            }

            await writer.FlushAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Writes all text to the zip archive entry using the specified encoding.
        /// </summary>
        public void WriteAllText(string text, Encoding? encoding = null)
        {
            using var stream = entry.Open();
            using var writer = new StreamWriter(stream, encoding ?? Encoding.Utf8WithoutBom);

            writer.Write(text);
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
            using var stream = entry.Open();
            using var writer = new StreamWriter(stream, encoding ?? Encoding.Utf8WithoutBom);

            await writer.WriteAsync(text.AsMemory(), cancellationToken).ConfigureAwait(false);
            await writer.FlushAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
#endif
