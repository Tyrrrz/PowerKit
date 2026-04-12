using System;
using System.Collections;
using System.Linq;

namespace PowerKit.Extensions;

internal static class EnvironmentExtensions
{
    extension(Environment)
    {
        /// <summary>
        /// Refreshes the environment variables of the current process by re-applying
        /// the machine-level environment variables.
        /// </summary>
        public static void RefreshEnvironmentVariables()
        {
            var machineEnvironmentVariables = Environment
                .GetEnvironmentVariables(EnvironmentVariableTarget.Machine)
                .Cast<DictionaryEntry>();

            foreach (var environmentVariable in machineEnvironmentVariables)
            {
                var key = (string)environmentVariable.Key;
                var value = (string?)environmentVariable.Value;

                Environment.SetEnvironmentVariable(key, value, EnvironmentVariableTarget.Process);
            }
        }
    }
}
