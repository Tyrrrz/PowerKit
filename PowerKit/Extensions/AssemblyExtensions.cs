using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PowerKit.Extensions;

internal static class AssemblyExtensions
{
    extension(Assembly assembly)
    {
        /// <summary>
        /// Returns the informational version string of the assembly, falling back to the
        /// assembly version if the <see cref="AssemblyInformationalVersionAttribute" /> is not set.
        /// Returns <see langword="null" /> if neither is available.
        /// </summary>
        public string? TryGetVersionString() =>
            assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion ?? assembly.GetName().Version?.ToString();

        /// <summary>
        /// Reads the specified manifest resource as a UTF-8 string.
        /// Throws <see cref="MissingManifestResourceException" /> if the resource is not found.
        /// </summary>
        public string GetManifestResourceString(string resourceName)
        {
            using var stream =
                assembly.GetManifestResourceStream(resourceName)
                ?? throw new MissingManifestResourceException(
                    $"Failed to find resource '{resourceName}'."
                );

            using var reader = new StreamReader(stream, Encoding.UTF8);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Reads the specified manifest resource as a UTF-8 string asynchronously.
        /// Throws <see cref="MissingManifestResourceException" /> if the resource is not found.
        /// </summary>
        public async Task<string> GetManifestResourceStringAsync(
            string resourceName,
            CancellationToken cancellationToken = default
        )
        {
            using var stream =
                assembly.GetManifestResourceStream(resourceName)
                ?? throw new MissingManifestResourceException(
                    $"Failed to find resource '{resourceName}'."
                );

            cancellationToken.ThrowIfCancellationRequested();
            using var reader = new StreamReader(stream, Encoding.UTF8);
            return await reader.ReadToEndAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Extracts the specified manifest resource to a file at the given path.
        /// Throws <see cref="MissingManifestResourceException" /> if the resource is not found.
        /// </summary>
        public void ExtractManifestResource(string resourceName, string filePath)
        {
            using var stream =
                assembly.GetManifestResourceStream(resourceName)
                ?? throw new MissingManifestResourceException(
                    $"Failed to find resource '{resourceName}'."
                );

            using var fileStream = File.Create(filePath);
            stream.CopyTo(fileStream);
        }

        /// <summary>
        /// Extracts the specified manifest resource to a file at the given path asynchronously.
        /// Throws <see cref="MissingManifestResourceException" /> if the resource is not found.
        /// </summary>
        public async Task ExtractManifestResourceAsync(
            string resourceName,
            string filePath,
            CancellationToken cancellationToken = default
        )
        {
            using var stream =
                assembly.GetManifestResourceStream(resourceName)
                ?? throw new MissingManifestResourceException(
                    $"Failed to find resource '{resourceName}'."
                );

            using var fileStream = new FileStream(
                filePath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                81920,
                FileOptions.Asynchronous
            );
            await stream.CopyToAsync(fileStream, 81920, cancellationToken).ConfigureAwait(false);
        }
    }
}
