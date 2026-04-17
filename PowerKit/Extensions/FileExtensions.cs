#nullable enable
using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class FileExtensions
{
    extension(File)
    {
        /// <summary>
        /// Attempts to delete the file at the specified path.
        /// Returns <see langword="true" /> if the delete operation completed without throwing an exception,
        /// or <see langword="false" /> if an error occurred.
        /// </summary>
        /// <remarks>
        /// This method can return <see langword="true" /> even if no file existed at <paramref name="path" />,
        /// because <see cref="File.Delete(string)" /> does not throw when the target file does not exist.
        /// </remarks>
        public static bool TryDelete(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if it's possible to write to the specified file.
        /// </summary>
        public static bool CheckWriteAccess(string path)
        {
            var wasExisting = File.Exists(path);

            try
            {
                File.OpenWrite(path).Dispose();
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            finally
            {
                if (!wasExisting)
                {
                    File.TryDelete(path);
                }
            }
        }

        /// <summary>
        /// Checks whether the file at the specified path contains the given byte sequence.
        /// Returns <see langword="true" /> if <paramref name="bytes" /> is empty.
        /// </summary>
        public static bool ContainsBytes(string path, ReadOnlySpan<byte> bytes)
        {
            if (bytes.IsEmpty)
                return true;

            using var stream = File.OpenRead(path);

            var patternLength = bytes.Length;
            using var bufferOwner = ArrayPool<byte>.Shared.RentOwner(patternLength * 2);
            var bytesInBuffer = 0;

            while (true)
            {
                var bytesRead = stream.Read(bufferOwner.Span.Slice(bytesInBuffer));
                bytesInBuffer += bytesRead;

                for (var i = 0; i <= bytesInBuffer - patternLength; i++)
                {
                    if (bufferOwner.Span.Slice(i, patternLength).SequenceEqual(bytes))
                        return true;
                }

                if (bytesRead == 0)
                    break;

                var overlap = Math.Min(patternLength - 1, bytesInBuffer);
                if (overlap > 0)
                    bufferOwner.Span.Slice(bytesInBuffer - overlap, overlap).CopyTo(bufferOwner.Span);

                bytesInBuffer = overlap;
            }

            return false;
        }

        /// <summary>
        /// Checks whether the file at the specified path contains the given byte sequence.
        /// Returns <see langword="true" /> if <paramref name="bytes" /> is empty.
        /// </summary>
        public static bool ContainsBytes(string path, byte[] bytes) =>
            File.ContainsBytes(path, new ReadOnlySpan<byte>(bytes));

        /// <summary>
        /// Creates a file at the specified path and fills it with zeroes.
        /// </summary>
        public static void WriteAllZeroes(string path, long count)
        {
            using var stream = File.Create(path);
            stream.SetLength(count);
        }

        /// <summary>
        /// Reads all bytes from the specified file starting at the given offset.
        /// </summary>
        public static byte[] ReadAllBytes(string path, long offset)
        {
            using var stream = File.OpenRead(path);
            stream.Seek(offset, SeekOrigin.Begin);

            if (offset >= stream.Length)
            {
                return [];
            }

            var buffer = new byte[checked((int)(stream.Length - offset))];
            stream.ReadExactly(buffer);

            return buffer;
        }

        /// <summary>
        /// Reads the specified number of bytes from the file starting at the given offset.
        /// </summary>
        public static byte[] ReadAllBytes(string path, long offset, int length)
        {
            using var stream = File.OpenRead(path);
            stream.Seek(offset, SeekOrigin.Begin);

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            var buffer = new byte[length];
            stream.ReadExactly(buffer);

            return buffer;
        }

#if NET40_OR_GREATER || NETSTANDARD || NET
        /// <summary>
        /// Reads all bytes from the specified file starting at the given offset asynchronously.
        /// </summary>
        public static async Task<byte[]> ReadAllBytesAsync(
            string path,
            long offset,
            CancellationToken cancellationToken = default
        )
        {
            using var stream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite,
                4096,
                FileOptions.Asynchronous
            );

            stream.Seek(offset, SeekOrigin.Begin);

            if (offset >= stream.Length)
            {
                return [];
            }

            var buffer = new byte[checked((int)(stream.Length - offset))];
            await stream.ReadExactlyAsync(buffer, cancellationToken).ConfigureAwait(false);

            return buffer;
        }

        /// <summary>
        /// Reads the specified number of bytes from the file starting at the given offset asynchronously.
        /// </summary>
        public static async Task<byte[]> ReadAllBytesAsync(
            string path,
            long offset,
            int length,
            CancellationToken cancellationToken = default
        )
        {
            using var stream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite,
                4096,
                FileOptions.Asynchronous
            );

            stream.Seek(offset, SeekOrigin.Begin);

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            var buffer = new byte[length];
            await stream.ReadExactlyAsync(buffer, cancellationToken).ConfigureAwait(false);

            return buffer;
        }
#endif
    }
}
