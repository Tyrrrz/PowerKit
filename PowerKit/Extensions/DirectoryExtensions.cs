using System;
using System.IO;

namespace PowerKit.Extensions;

internal static class DirectoryExtensions
{
    extension(Directory)
    {
        /// <summary>
        /// Checks if it's possible to write to the specified directory.
        /// </summary>
        public static bool CheckWriteAccess(string path)
        {
            var tempFilePath = Path.Combine(path, Guid.NewGuid().ToString());

            try
            {
                using (File.Create(tempFilePath)) { }

                try
                {
                    File.Delete(tempFilePath);
                }
                catch (IOException)
                {
                    // Best-effort cleanup: inability to delete the temporary file
                    // does not affect the write-access check result.
                }
                catch (UnauthorizedAccessException)
                {
                    // Best-effort cleanup: inability to delete the temporary file
                    // does not affect the write-access check result.
                }

                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

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
    }
}
