using System;
using System.IO;

namespace PowerKit;

/// <summary>
/// Represents a temporary file that is automatically deleted when disposed.
/// </summary>
internal partial class TempFile(string path) : IDisposable
{
    /// <summary>
    /// Gets the path of the temporary file.
    /// </summary>
    public string Path { get; } = path;

    /// <inheritdoc />
    public void Dispose()
    {
        File.Delete(Path);
    }
}

internal partial class TempFile
{
    /// <summary>
    /// Creates a new temporary file.
    /// </summary>
    public static TempFile Create()
    {
        while (true)
        {
            var filePath = System.IO.Path.Combine(
                System.IO.Path.GetTempPath(),
                Guid.NewGuid().ToString() + ".tmp"
            );

            try
            {
                using (new FileStream(filePath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None)) { }
                return new TempFile(filePath);
            }
            catch (IOException) { }
        }
    }
}
