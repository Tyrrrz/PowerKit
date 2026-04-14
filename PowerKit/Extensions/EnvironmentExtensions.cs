using System;
using System.Collections;

namespace PowerKit.Extensions;

internal static class EnvironmentExtensions
{
    extension(Environment)
    {
        /// <summary>
        /// Refreshes the environment variables of the current process by re-applying
        /// the machine-level and user-level environment variables.
        /// On Windows, this first removes any process-level environment variables that
        /// are not present at the machine or user scope, then applies the machine-level
        /// and user-level values to the current process.
        /// This can remove variables inherited from the parent process or set by the
        /// application itself. On other platforms, this method is a no-op.
        /// </summary>
        public static void RefreshEnvironmentVariables()
        {
            if (!OperatingSystem.IsWindows())
            {
                return;
            }

            var machineVariables = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Machine);
            var userVariables = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.User);

            // Remove missing
            foreach (DictionaryEntry environmentVariable in Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process))
            {
                var key = (string)environmentVariable.Key;

                if (!machineVariables.Contains(key) && !userVariables.Contains(key))
                {
                    Environment.SetEnvironmentVariable(key, null, EnvironmentVariableTarget.Process);
                }
            }

            // Add/set machine variables
            foreach (DictionaryEntry environmentVariable in machineVariables)
            {
                var key = (string)environmentVariable.Key;
                var value = (string?)environmentVariable.Value;

                Environment.SetEnvironmentVariable(key, value, EnvironmentVariableTarget.Process);
            }

            // Add/set user variables
            foreach (DictionaryEntry environmentVariable in userVariables)
            {
                var key = (string)environmentVariable.Key;
                var value = (string?)environmentVariable.Value;

                Environment.SetEnvironmentVariable(key, value, EnvironmentVariableTarget.Process);
            }
        }
    }
}
