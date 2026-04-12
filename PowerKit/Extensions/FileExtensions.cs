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
        public static byte[] ReadAllBytes(string path, int offset)
        {
            using var stream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite
            );

            stream.Seek(offset, SeekOrigin.Begin);

            var buffer = new byte[stream.Length - offset];
            stream.ReadExactly(buffer);

            return buffer;
        }

        /// <summary>
        /// Reads all bytes from the specified file starting at the given offset asynchronously.
        /// </summary>
        public static async Task<byte[]> ReadAllBytesAsync(
            string path,
            int offset,
            CancellationToken cancellationToken = default
        )
        {
            using var stream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite
            );

            stream.Seek(offset, SeekOrigin.Begin);

            var buffer = new byte[stream.Length - offset];
            await stream.ReadExactlyAsync(buffer, cancellationToken).ConfigureAwait(false);

            return buffer;
        }
    }
}
