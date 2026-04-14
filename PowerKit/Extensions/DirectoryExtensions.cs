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
    }
}
