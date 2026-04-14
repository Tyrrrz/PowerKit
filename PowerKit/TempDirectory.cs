using System;
using System.IO;
using System.Diagnostics.CodeAnalysis;

namespace PowerKit;

/// <summary>
/// Represents a temporary directory that is automatically deleted when disposed.
/// </summary>
#if POWERKIT_EXCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal partial class TempDirectory(string path) : IDisposable
{
    /// <summary>
    /// Gets the path of the temporary directory.
    /// </summary>
    public string Path { get; } = path;

    /// <inheritdoc />
    public void Dispose()
    {
        try
        {
            Directory.Delete(Path, true);
        }
        catch (DirectoryNotFoundException) { }
    }
}

internal partial class TempDirectory
{
    /// <summary>
    /// Creates a new temporary directory.
    /// </summary>
    public static TempDirectory Create()
    {
        var dirPath = System.IO.Path.Combine(
            System.IO.Path.GetTempPath(),
            Guid.NewGuid().ToString()
        );

        Directory.CreateDirectory(dirPath);

        return new TempDirectory(dirPath);
    }
}
