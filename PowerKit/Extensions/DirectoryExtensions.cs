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
                File.Delete(tempFilePath);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }
    }
}
