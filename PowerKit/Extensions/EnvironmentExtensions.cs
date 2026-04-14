using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace PowerKit.Extensions;

internal static class EnvironmentExtensions
{
    extension(Environment)
    {
        /// <summary>
        /// Refreshes the environment variables of the current process by re-applying
        /// the machine-level environment variables.
        /// Only has an effect on Windows; on other platforms, this method is a no-op.
        /// </summary>
        public static void RefreshEnvironmentVariables()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return;
            }

            foreach (DictionaryEntry environmentVariable in Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Machine))
            {
                var key = (string)environmentVariable.Key;
                var value = (string?)environmentVariable.Value;

                Environment.SetEnvironmentVariable(key, value, EnvironmentVariableTarget.Process);
            }
        }
    }
}
