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
                using var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
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
        public static byte[] ReadAllBytes(string path, long offset, int length)
        {
            using var stream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite
            );

            stream.Seek(offset, SeekOrigin.Begin);

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            var buffer = new byte[length];
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
            int length,
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

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            var buffer = new byte[length];
            await stream.ReadExactlyAsync(buffer, cancellationToken).ConfigureAwait(false);

            return buffer;
        }
    }
}
