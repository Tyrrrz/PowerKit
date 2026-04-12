#if !NET35
using System.Reflection;

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
    }
}
#endif
