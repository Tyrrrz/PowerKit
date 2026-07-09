#if !NETSTANDARD
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using Microsoft.Win32;

namespace PowerKit.Extensions;

/// <summary>
/// Extensions for <see cref="RegistryHive" /> and <see cref="RegistryKey" />.
/// </summary>
public static class RegistryExtensions
{
    extension(RegistryHive hive)
    {
        /// <summary>
        /// Returns the short moniker for the registry hive (e.g., <c>HKCU</c>, <c>HKLM</c>).
        /// </summary>
        [SupportedOSPlatform("windows")]
        public string Moniker =>
            hive switch
            {
                RegistryHive.ClassesRoot => "HKCR",
                RegistryHive.CurrentUser => "HKCU",
                RegistryHive.LocalMachine => "HKLM",
                RegistryHive.Users => "HKU",
                RegistryHive.PerformanceData => "HKPD",
                RegistryHive.CurrentConfig => "HKCC",
                _ => throw new ArgumentOutOfRangeException(nameof(hive)),
            };

#if NET40_OR_GREATER || NET5_0_OR_GREATER
        /// <summary>
        /// Opens the base registry key for the hive using the specified <paramref name="view" />.
        /// </summary>
        [SupportedOSPlatform("windows")]
        public RegistryKey OpenKey(RegistryView view = RegistryView.Default) =>
            RegistryKey.OpenBaseKey(hive, view);
#endif
    }

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
#endif
