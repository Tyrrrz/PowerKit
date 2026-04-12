using System.IO;
using System.Reflection;
using System.Resources;
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
        /// Extracts the specified manifest resource to a file at the given path.
        /// Throws <see cref="MissingManifestResourceException" /> if the resource is not found.
        /// </summary>
        public void ExtractManifestResource(string resourceName, string filePath)
        {
            var resourceStream =
                assembly.GetManifestResourceStream(resourceName)
                ?? throw new MissingManifestResourceException(
                    $"Failed to find resource '{resourceName}'."
                );

            using (resourceStream)
            using (var fileStream = File.Create(filePath))
            {
                resourceStream.CopyTo(fileStream);
            }
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
            var resourceStream =
                assembly.GetManifestResourceStream(resourceName)
                ?? throw new MissingManifestResourceException(
                    $"Failed to find resource '{resourceName}'."
                );

            await using (resourceStream)
            await using (var fileStream = File.Create(filePath))
            {
                await resourceStream
                    .CopyToAsync(fileStream, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
    }
}
