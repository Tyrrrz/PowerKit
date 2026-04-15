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
    /// The directory is only created on disk when <paramref name="preCreate" /> is <see langword="true" />.
    /// </summary>
    public static TempDirectory Create(bool preCreate = true)
    {
        for (var retriesRemaining = 20; retriesRemaining > 0; retriesRemaining--)
        {
            var dirPath = System.IO.Path.Combine(
                System.IO.Path.GetTempPath(),
                Guid.NewGuid().ToString()
            );

            if (!preCreate)
                return new TempDirectory(dirPath);

            try
            {
                if (Directory.Exists(dirPath))
                    throw new IOException($"Directory '{dirPath}' already exists.");

                Directory.CreateDirectory(dirPath);
                return new TempDirectory(dirPath);
            }
            catch (IOException) when (Directory.Exists(dirPath))
            {
                // Path collision, retry with a new name
            }
        }

        throw new InvalidOperationException(
            "Failed to create a unique temporary directory after 20 attempts."
        );
    }
}
