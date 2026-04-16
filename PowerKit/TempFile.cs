#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using PowerKit.Extensions;

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
    /// Generates a unique path for a temporary file without creating it.
    /// </summary>
    public static string GeneratePath()
    {
        for (var retriesRemaining = 20; retriesRemaining > 0; retriesRemaining--)
        {
            var filePath = System.IO.Path.Combine(
                System.IO.Path.GetTempPath(),
                Guid.NewGuid() + ".tmp"
            );

            if (!File.Exists(filePath))
                return filePath;
        }

        throw new InvalidOperationException(
            "Failed to generate a unique temporary file path after several attempts."
        );
    }

    /// <summary>
    /// Creates a new temporary file.
    /// The file is only created on disk when <paramref name="preCreate" /> is <see langword="true" />.
    /// </summary>
    public static TempFile Create(bool preCreate = true)
    {
        var filePath = GeneratePath();

        if (preCreate)
            File.WriteAllZeroes(filePath, 0);

        return new TempFile(filePath);
    }
}
