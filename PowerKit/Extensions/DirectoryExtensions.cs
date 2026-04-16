#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
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
        /// Recursively copies all files from <paramref name="sourceDirPath" /> to <paramref name="destDirPath" />.
        /// File locks are acquired on every destination file before any data is written,
        /// so concurrent readers will observe either the old content or the fully updated content.
        /// </summary>
        public static void Copy(string sourceDirPath, string destDirPath, bool overwrite = true)
        {
            var sourceStreams = new List<FileStream>();
            var destStreams = new List<FileStream>();

            try
            {
                var normalizedSourceDir = sourceDirPath.TrimEnd(
                    Path.DirectorySeparatorChar,
                    Path.AltDirectorySeparatorChar
                );

                foreach (
                    var sourceFilePath in Directory.GetFiles(
                        sourceDirPath,
                        "*",
                        SearchOption.AllDirectories
                    )
                )
                {
                    sourceStreams.Add(
                        File.Open(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)
                    );

                    var relativePath = sourceFilePath.Substring(normalizedSourceDir.Length + 1);
                    var destFilePath = Path.Combine(destDirPath, relativePath);

                    // destFilePath is always a full path under destDirPath, so GetDirectoryName is never null
                    Directory.CreateDirectory(Path.GetDirectoryName(destFilePath)!);

                    destStreams.Add(
                        File.Open(
                            destFilePath,
                            overwrite ? FileMode.OpenOrCreate : FileMode.CreateNew,
                            FileAccess.ReadWrite,
                            FileShare.None
                        )
                    );
                }

                for (var i = 0; i < sourceStreams.Count; i++)
                {
                    sourceStreams[i].CopyTo(destStreams[i]);

                    // Truncate the destination file if the source file is shorter
                    destStreams[i].SetLength(sourceStreams[i].Length);
                }
            }
            finally
            {
                foreach (var stream in sourceStreams)
                {
                    try
                    {
                        stream.Dispose();
                    }
                    catch { }
                }

                foreach (var stream in destStreams)
                {
                    try
                    {
                        stream.Dispose();
                    }
                    catch { }
                }
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
