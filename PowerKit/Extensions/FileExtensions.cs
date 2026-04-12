using System.Buffers;
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

            var size = checked((int)(stream.Length - offset));
            using var buffer = MemoryPool<byte>.Shared.Rent(size);
            var slice = buffer.Memory.Span[..size];

            stream.ReadExactly(slice);

            return slice.ToArray();
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

            var size = checked((int)length);
            using var buffer = MemoryPool<byte>.Shared.Rent(size);
            var slice = buffer.Memory.Span[..size];

            stream.ReadExactly(slice);

            return slice.ToArray();
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

            var size = checked((int)(stream.Length - offset));
            using var buffer = MemoryPool<byte>.Shared.Rent(size);
            var slice = buffer.Memory[..size];

            await stream.ReadExactlyAsync(slice, cancellationToken).ConfigureAwait(false);

            return slice.Span.ToArray();
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

            var size = checked((int)length);
            using var buffer = MemoryPool<byte>.Shared.Rent(size);
            var slice = buffer.Memory[..size];

            await stream.ReadExactlyAsync(slice, cancellationToken).ConfigureAwait(false);

            return slice.Span.ToArray();
        }
    }
}
