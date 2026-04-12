using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PowerKit.Extensions;

internal static class FileExtensions
{
    extension(File)
    {
        /// <summary>
        /// Creates a file at the specified path and fills it with zeroes.
        /// </summary>
        public static void WriteAllZeroes(string path, long count)
        {
            using var stream = new FileStream(
                path,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None
            );

            stream.SetLength(count);
        }

        /// <summary>
        /// Reads all bytes from the specified file starting at the given offset.
        /// </summary>
        public static byte[] ReadAllBytes(string path, long offset)
        {
            using var stream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite
            );

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
        public static byte[] ReadAllBytes(string path, long offset, long length)
        {
            using var stream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite
            );

            stream.Seek(offset, SeekOrigin.Begin);

            if (length < 0 || length > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            var buffer = new byte[(int)length];
            stream.ReadExactly(buffer);

            return buffer;
        }

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
                bufferSize: 4096,
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
            long length,
            CancellationToken cancellationToken = default
        )
        {
            using var stream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite,
                bufferSize: 4096,
                FileOptions.Asynchronous
            );

            stream.Seek(offset, SeekOrigin.Begin);

            if (length < 0 || length > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            var buffer = new byte[(int)length];
            await stream.ReadExactlyAsync(buffer, cancellationToken).ConfigureAwait(false);

            return buffer;
        }
    }
}
