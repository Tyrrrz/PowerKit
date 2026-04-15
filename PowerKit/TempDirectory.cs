#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace PowerKit;

/// <summary>
/// Represents a temporary directory that is automatically deleted when disposed.
/// </summary>
#if !POWERKIT_INCLUDE_COVERAGE
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
    /// <param name="preCreate">
    /// Whether to pre-create the directory at the target location.
    /// If <see langword="false" />, only the directory path is generated without creating the directory.
    /// </param>
    public static TempDirectory Create(bool preCreate = true)
    {
        var dirPath = System.IO.Path.Combine(
            System.IO.Path.GetTempPath(),
            Guid.NewGuid().ToString()
        );

        if (preCreate)
            Directory.CreateDirectory(dirPath);

        return new TempDirectory(dirPath);
    }
}
