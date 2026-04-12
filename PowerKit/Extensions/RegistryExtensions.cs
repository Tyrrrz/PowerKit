using System.Runtime.Versioning;
using Microsoft.Win32;

namespace PowerKit.Extensions;

internal static class RegistryExtensions
{
    extension(RegistryKey key)
    {
        /// <summary>
        /// Checks whether a sub-key with the specified name exists under the registry key.
        /// </summary>
        [SupportedOSPlatform("windows")]
        public bool ContainsSubKey(string name)
        {
            using var subKey = key.OpenSubKey(name, false);
            return subKey is not null;
        }
    }
}
