using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace PowerKit.Extensions;

#if POWERKIT_EXCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
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
        /// Reads the specified manifest resource as a string using the specified encoding.
        /// Throws <see cref="MissingManifestResourceException" /> if the resource is not found.
        /// </summary>
        public string GetManifestResourceString(string resourceName, Encoding encoding)
        {
            using var stream =
                assembly.GetManifestResourceStream(resourceName)
                ?? throw new MissingManifestResourceException(
                    $"Failed to find resource '{resourceName}'."
                );

            using var reader = new StreamReader(stream, encoding);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Reads the specified manifest resource as a UTF-8 string.
        /// Throws <see cref="MissingManifestResourceException" /> if the resource is not found.
        /// </summary>
        public string GetManifestResourceString(string resourceName) =>
            assembly.GetManifestResourceString(resourceName, Encoding.UTF8);

#if NET40_OR_GREATER || NETSTANDARD || NET
        /// <summary>
        /// Reads the specified manifest resource as a string using the specified encoding asynchronously.
        /// Throws <see cref="MissingManifestResourceException" /> if the resource is not found.
        /// </summary>
        public async Task<string> GetManifestResourceStringAsync(
            string resourceName,
            Encoding encoding,
            CancellationToken cancellationToken = default
        )
        {
            using var stream =
                assembly.GetManifestResourceStream(resourceName)
                ?? throw new MissingManifestResourceException(
                    $"Failed to find resource '{resourceName}'."
                );

            using var reader = new StreamReader(stream, encoding);
            return await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Reads the specified manifest resource as a UTF-8 string asynchronously.
        /// Throws <see cref="MissingManifestResourceException" /> if the resource is not found.
        /// </summary>
        public async Task<string> GetManifestResourceStringAsync(
            string resourceName,
            CancellationToken cancellationToken = default
        ) =>
            await assembly
                .GetManifestResourceStringAsync(resourceName, Encoding.UTF8, cancellationToken)
                .ConfigureAwait(false);
#endif

        /// <summary>
        /// Extracts the specified manifest resource to a file at the given path.
        /// Throws <see cref="MissingManifestResourceException" /> if the resource is not found.
        /// </summary>
        public void ExtractManifestResource(string resourceName, string filePath)
        {
            using var source =
                assembly.GetManifestResourceStream(resourceName)
                ?? throw new MissingManifestResourceException(
                    $"Failed to find resource '{resourceName}'."
                );

            using var destination = File.Create(filePath);
            source.CopyTo(destination);
            destination.Flush();
        }

#if NET40_OR_GREATER || NETSTANDARD || NET
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
            using var source =
                assembly.GetManifestResourceStream(resourceName)
                ?? throw new MissingManifestResourceException(
                    $"Failed to find resource '{resourceName}'."
                );

            using var destination = new FileStream(
                filePath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                81920,
                FileOptions.Asynchronous
            );

            await source.CopyToAsync(destination, 81920, cancellationToken).ConfigureAwait(false);
            await destination.FlushAsync(cancellationToken).ConfigureAwait(false);
        }
#endif
    }
}
