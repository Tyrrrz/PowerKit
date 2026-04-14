#if NET5_0_OR_GREATER || NETFRAMEWORK

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif
using Microsoft.Win32;

namespace PowerKit.Extensions;

internal static class RegistryExtensions
{
    extension(RegistryKey key)
    {
        /// <summary>
        /// Checks whether a sub-key with the specified name exists under the registry key.
        /// </summary>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
#endif
        public bool ContainsSubKey(string name)
        {
            using var subKey = key.OpenSubKey(name, false);
            return subKey is not null;
        }
    }
}

#endif
