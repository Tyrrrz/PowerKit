using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PowerKit.Extensions;

/// <summary>
/// Extensions for <see cref="Directory" />.
/// </summary>
public static class DirectoryExtensions
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

        /// <summary>
        /// Recursively copies all files from <paramref name="sourcePath" /> to <paramref name="destinationPath" />.
        /// Destination files are opened with exclusive locks before any data is written.
        /// Concurrent readers may be blocked or fail with a sharing violation while a file is being updated,
        /// and this method does not guarantee atomic old-or-new visibility to readers.
        /// </summary>
        public static void Copy(string sourcePath, string destinationPath, bool overwrite = true)
        {
            var sourceStreams = new List<FileStream>();
            var destinationStreams = new List<FileStream>();

            try
            {
                // Create all destination directories
                Directory.CreateDirectory(destinationPath);
                foreach (
                    var sourceDirectoryPath in Directory.GetDirectories(
                        sourcePath,
                        "*",
                        SearchOption.AllDirectories
                    )
                )
                {
                    Directory.CreateDirectory(
                        Path.Combine(
                            destinationPath,
                            Path.GetRelativePath(sourcePath, sourceDirectoryPath)
                        )
                    );
                }

                // Create file stream pairs
                foreach (
                    var sourceFilePath in Directory.GetFiles(
                        sourcePath,
                        "*",
                        SearchOption.AllDirectories
                    )
                )
                {
                    sourceStreams.Add(File.OpenRead(sourceFilePath));

                    var destinationFilePath = Path.Combine(
                        destinationPath,
                        Path.GetRelativePath(sourcePath, sourceFilePath)
                    );

                    destinationStreams.Add(
                        overwrite
                            ? File.OpenWrite(destinationFilePath)
                            : File.Open(
                                destinationFilePath,
                                FileMode.CreateNew,
                                FileAccess.Write,
                                FileShare.None
                            )
                    );
                }

                // Copy the file contents
                foreach (
                    var (sourceStream, destinationStream) in sourceStreams.Zip(
                        destinationStreams,
                        (s, d) => (s, d)
                    )
                )
                {
                    sourceStream.CopyTo(destinationStream);

                    // Truncate the destination file if the source file is shorter
                    destinationStream.SetLength(sourceStream.Length);

                    // Preserve Unix file permissions on non-Windows platforms
                    if (!OperatingSystem.IsWindows())
                    {
                        File.SetUnixFileMode(
                            destinationStream.Name,
                            File.GetUnixFileMode(sourceStream.Name)
                        );
                    }
                }
            }
            finally
            {
                Disposable.Merge([.. sourceStreams, .. destinationStreams]).Dispose();
            }
        }

        /// <summary>
        /// Creates the directory for the specified file path, including all intermediate directories.
        /// Does nothing if the directory already exists or if the path has no directory component.
        /// </summary>
        public static void CreateForFile(string filePath)
        {
            var dirPath = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dirPath))
                Directory.CreateDirectory(dirPath);
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
