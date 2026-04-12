using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PowerKit.Extensions;

internal static class PathExtensions
{
    // This is a union of invalid characters from Windows (NTFS/FAT32), Linux (ext4/XFS), and macOS (HFS+/APFS).
    // We use this instead of Path.GetInvalidFileNameChars() because that only returns OS-specific characters,
    // not filesystem-specific characters. It's possible to use, for example, an NTFS drive on Linux,
    // which would make some additional characters invalid that are otherwise valid on Linux.
    private static readonly HashSet<char> InvalidFileNameChars =
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

    extension(Path)
    {
        public static string EscapeFileName(string fileName)
        {
            var buffer = new StringBuilder(fileName.Length);

            foreach (var c in fileName)
            {
                buffer.Append(!InvalidFileNameChars.Contains(c) ? c : '_');
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
