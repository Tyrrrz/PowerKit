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
            using var stream = entry.Open();
            var bytes = new byte[checked((int)entry.Length)];
            var offset = 0;

            while (offset < bytes.Length)
            {
                var read = stream.Read(bytes, offset, bytes.Length - offset);
                if (read == 0)
                {
                    throw new EndOfStreamException($"Expected to read {bytes.Length} bytes from zip archive entry '{entry.FullName}', but only read {offset}.");
                }

                offset += read;
            }

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
            using var reader = new StreamReader(stream, encoding ?? Encoding.UTF8);

            return reader.ReadToEnd();
        }

        /// <summary>
        /// Writes all text to the zip archive entry using the specified encoding.
        /// </summary>
        public void WriteAllText(string text, Encoding? encoding = null)
        {
            using var stream = entry.Open();
            using var writer = new StreamWriter(stream, encoding ?? Encoding.UTF8);
            writer.Write(text);
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
        /// Writes all lines to the zip archive entry using the specified encoding.
        /// </summary>
        public void WriteAllLines(IEnumerable<string> lines, Encoding? encoding = null)
        {
            using var stream = entry.Open();
            using var writer = new StreamWriter(stream, encoding ?? Encoding.UTF8);

            foreach (var line in lines)
            {
                writer.WriteLine(line);
            }
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
            {
                return Array.Empty<byte>();
            }

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
        /// Writes all text to the zip archive entry using the specified encoding asynchronously.
        /// </summary>
        public async Task WriteAllTextAsync(
            string text,
            Encoding? encoding = null,
            CancellationToken cancellationToken = default
        )
        {
            using var stream = entry.Open();
            using var writer = new StreamWriter(stream, encoding ?? Encoding.UTF8);
            await writer.WriteAsync(text.AsMemory(), cancellationToken).ConfigureAwait(false);
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
        /// Writes all lines to the zip archive entry using the specified encoding asynchronously.
        /// </summary>
        public async Task WriteAllLinesAsync(
            IEnumerable<string> lines,
            Encoding? encoding = null,
            CancellationToken cancellationToken = default
        )
        {
            using var stream = entry.Open();
            using var writer = new StreamWriter(stream, encoding ?? Encoding.UTF8);

            foreach (var line in lines)
            {
                await writer.WriteLineAsync(line.AsMemory(), cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
