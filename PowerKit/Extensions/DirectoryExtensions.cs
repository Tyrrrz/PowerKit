using System.IO;

namespace PowerKit.Extensions;

internal static class DirectoryExtensions
{
    extension(Directory)
    {
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
