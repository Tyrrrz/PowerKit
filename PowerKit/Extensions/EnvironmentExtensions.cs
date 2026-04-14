using System;
using System.Collections;

namespace PowerKit.Extensions;

internal static class EnvironmentExtensions
{
    extension(Environment)
    {
        /// <summary>
        /// Refreshes the environment variables of the current process to match the
        /// machine-level environment variables by removing process variables that are
        /// not defined at machine scope and re-applying machine-level values.
        /// This may remove process- or user-only variables from the current process.
        /// Only has an effect on Windows; on other platforms, this method is a no-op.
        /// </summary>
        public static void RefreshEnvironmentVariables()
        {
            if (!OperatingSystem.IsWindows())
            {
                return;
            }

            var machineVariables = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Machine);

            // Handle removed variables
            foreach (DictionaryEntry environmentVariable in Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process))
            {
                var key = (string)environmentVariable.Key;

                if (!machineVariables.Contains(key))
                {
                    Environment.SetEnvironmentVariable(key, null, EnvironmentVariableTarget.Process);
                }
            }

            // Handle added and overwritten variables
            foreach (DictionaryEntry environmentVariable in machineVariables)
            {
                var key = (string)environmentVariable.Key;
                var value = (string?)environmentVariable.Value;

                Environment.SetEnvironmentVariable(key, value, EnvironmentVariableTarget.Process);
            }
        }
    }
}
