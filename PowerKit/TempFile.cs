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
        var filePath = System.IO.Path.Combine(
            System.IO.Path.GetTempPath(),
            Guid.NewGuid().ToString() + ".tmp"
        );

        return new TempFile(filePath);
    }
}
