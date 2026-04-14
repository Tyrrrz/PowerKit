using System;
using System.IO;

namespace PowerKit;

/// <summary>
/// Represents a file-based lock that prevents concurrent access to a shared resource.
/// </summary>
internal partial class LockFile(FileStream fileStream) : IDisposable
{
    /// <inheritdoc />
    public void Dispose() => fileStream.Dispose();
}

internal partial class LockFile
{
    /// <summary>
    /// Tries to acquire a lock on the specified file path.
    /// Returns <see langword="null" /> if the lock could not be acquired.
    /// </summary>
    public static LockFile? TryAcquire(string filePath)
    {
        try
        {
            var fileStream = File.Open(
                filePath,
                FileMode.Create,
                FileAccess.ReadWrite,
                FileShare.None
            );

            return new LockFile(fileStream);
        }
        // This is the most specific exception for "access denied"
        catch (IOException)
        {
            return null;
        }
    }
}
