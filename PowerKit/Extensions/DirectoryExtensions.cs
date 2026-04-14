#nullable enable
using System;
using System.IO;

namespace PowerKit.Extensions;

internal static class DirectoryExtensions
{
    extension(Directory)
    {
        /// <summary>
        /// Deletes the directory and all its contents, then recreates it as an empty directory.
        /// </summary>
        public static void Reset(string path)
        {
            try
            {
                Directory.Delete(path, true);
            }
            catch (DirectoryNotFoundException) { }

            Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Attempts to delete the directory at the specified path.
        /// Returns <see langword="true" /> if the directory was successfully deleted, or <see langword="false" /> if an error occurred.
        /// </summary>
        public static bool TryDelete(string path, bool recursive = false)
        {
            try
            {
                Directory.Delete(path, recursive);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if it's possible to write to the specified directory.
        /// </summary>
        public static bool CheckWriteAccess(string path)
        {
            var tempFilePath = Path.Combine(path, Guid.NewGuid().ToString());

            try
            {
                using var tempFile = File.Create(tempFilePath);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            finally
            {
                File.TryDelete(tempFilePath);
            }
        }
    }
}
