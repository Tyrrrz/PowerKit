#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace PowerKit;

/// <summary>
/// Represents a temporary file that is automatically deleted when disposed.
/// </summary>
#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal partial class TempFile(string path) : IDisposable
{
    /// <summary>
    /// Gets the path of the temporary file.
    /// </summary>
    public string Path { get; } = path;

    /// <inheritdoc />
    // File.Delete does not throw if the file does not exist
    public void Dispose() => File.Delete(Path);
}

internal partial class TempFile
{
    /// <summary>
    /// Creates a new temporary file.
    /// </summary>
    /// <param name="preCreate">
    /// Whether to pre-create the file at the target location.
    /// If <see langword="false" />, only the file path is generated without creating the file.
    /// </param>
    public static TempFile Create(bool preCreate = true)
    {
        var filePath = System.IO.Path.Combine(
            System.IO.Path.GetTempPath(),
            Guid.NewGuid() + ".tmp"
        );

        if (preCreate)
            File.Create(filePath).Dispose();

        return new TempFile(filePath);
    }
}
