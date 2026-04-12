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
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            Directory.CreateDirectory(path);
        }
    }
}
