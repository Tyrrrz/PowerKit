#nullable enable
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PowerKit.Extensions;

file static class PathEx
{
    // Characters that are invalid in file names across all major filesystems
    // (Windows NTFS/FAT32, Linux ext4/XFS, macOS HFS+/APFS), beyond what
    // the OS-specific Path.GetInvalidFileNameChars() returns.
    // This is useful when working with files that may be accessed from
    // different operating systems, such as NTFS drives on Linux.
    public static readonly char[] CrossPlatformInvalidFileNameChars =
    [
        '\0', // Null character - invalid on all filesystems
        '\x01', '\x02', '\x03', '\x04', '\x05', '\x06', '\x07', // ASCII control characters -
        '\x08', '\x09', '\x0A', '\x0B', '\x0C', '\x0D', '\x0E', '\x0F', // invalid on Windows
        '\x10', '\x11', '\x12', '\x13', '\x14', '\x15', '\x16', '\x17', // (NTFS/FAT32)
        '\x18', '\x19', '\x1A', '\x1B', '\x1C', '\x1D', '\x1E', '\x1F',
        '/', // Path separator on Unix and Windows
        '\\', // Path separator on Windows
        ':', // Reserved on Windows (drive letters, NTFS streams)
        '*', // Wildcard on Windows
        '?', // Wildcard on Windows
        '"', // Reserved on Windows
        '<', // Redirection on Windows
        '>', // Redirection on Windows
        '|', // Pipe on Windows
    ];

    // Path chars are the same as file name chars, except path separators
    // and the colon (drive letter separator) are valid in paths.
    public static readonly char[] CrossPlatformInvalidPathChars =
        CrossPlatformInvalidFileNameChars
            .Where(ch => ch != '/' && ch != '\\' && ch != ':')
            .ToArray();
}

internal static class PathExtensions
{
    extension(Path)
    {
        /// <summary>
        /// Gets the characters that are invalid in file names.
        /// When <paramref name="crossPlatform" /> is <see langword="true" />, returns characters
        /// invalid across all major filesystems; otherwise, returns the OS-specific set.
        /// </summary>
        public static char[] GetInvalidFileNameChars(bool crossPlatform) =>
            crossPlatform
                ? PathEx.CrossPlatformInvalidFileNameChars
                : Path.GetInvalidFileNameChars();

        /// <summary>
        /// Gets the characters that are invalid in paths.
        /// When <paramref name="crossPlatform" /> is <see langword="true" />, returns characters
        /// invalid across all major filesystems; otherwise, returns the OS-specific set.
        /// </summary>
        public static char[] GetInvalidPathChars(bool crossPlatform) =>
            crossPlatform
                ? PathEx.CrossPlatformInvalidPathChars
                : Path.GetInvalidPathChars();

        /// <summary>
        /// Replaces invalid file name characters with underscores and strips trailing dots and whitespace.
        /// When <paramref name="crossPlatform" /> is <see langword="true" />, considers characters
        /// invalid across all major filesystems.
        /// </summary>
        public static string EscapeFileName(string fileName, bool crossPlatform = true)
        {
            var invalidChars = new HashSet<char>(Path.GetInvalidFileNameChars(crossPlatform));
            var buffer = new StringBuilder(fileName.Length);

            foreach (var ch in fileName)
            {
                buffer.Append(!invalidChars.Contains(ch) ? ch : '_');
            }

            // File names cannot end with a dot or whitespace (invalid on Windows, ambiguous on other filesystems)
            while (buffer.Length > 0 && (buffer[buffer.Length - 1] == '.' || char.IsWhiteSpace(buffer[buffer.Length - 1])))
            {
                buffer.Remove(buffer.Length - 1, 1);
            }

            return buffer.ToString();
        }
    }
}
