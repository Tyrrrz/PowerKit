using System;
using System.IO;

namespace PowerKit;

/// <summary>
/// Represents a temporary directory that is automatically deleted when disposed.
/// </summary>
public partial class TempDirectory(string path) : IDisposable
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

public partial class TempDirectory
{
    /// <summary>
    /// Generates a unique path for a temporary directory without creating it.
    /// </summary>
    public static string GeneratePath()
    {
        for (var retriesRemaining = 20; retriesRemaining > 0; retriesRemaining--)
        {
            var dirPath = System.IO.Path.Combine(
                System.IO.Path.GetTempPath(),
                Guid.NewGuid().ToString()
            );

            if (!Directory.Exists(dirPath))
                return dirPath;
        }

        throw new InvalidOperationException(
            "Failed to generate a unique temporary directory path after several attempts."
        );
    }

    /// <summary>
    /// Creates a new temporary directory.
    /// The directory is only created on disk when <paramref name="preCreate" /> is <see langword="true" />.
    /// </summary>
    public static TempDirectory Create(bool preCreate = true)
    {
        var dirPath = GeneratePath();

        if (preCreate)
            Directory.CreateDirectory(dirPath);

        return new TempDirectory(dirPath);
    }
}
