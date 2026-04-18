#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class EnvironmentExtensions
{
    extension(Environment)
    {
        /// <summary>
        /// Refreshes the environment variables of the current process by re-applying
        /// the machine-level and user-level environment variables.
        /// </summary>
        /// <remarks>
        /// On Windows, this first removes any process-level environment variables that
        /// are not present at the machine or user scope, then applies the machine-level
        /// and user-level values to the current process.
        /// This can remove variables inherited from the parent process or set by the
        /// application itself.
        /// On other platforms, this method is a no-op.
        /// </remarks>
        public static void RefreshEnvironmentVariables()
        {
            if (!OperatingSystem.IsWindows())
            {
                return;
            }

            var machineVariables = Environment
                .GetEnvironmentVariables(EnvironmentVariableTarget.Machine)
                .ToDictionary<string, string>(StringComparer.Ordinal);

            var userVariables = Environment
                .GetEnvironmentVariables(EnvironmentVariableTarget.User)
                .ToDictionary<string, string>(StringComparer.Ordinal);

            // Remove missing
            foreach (
                var (key, _) in Environment
                    .GetEnvironmentVariables(EnvironmentVariableTarget.Process)
                    .ToDictionary<string, string>(StringComparer.Ordinal)
            )
            {
                if (!machineVariables.ContainsKey(key) && !userVariables.ContainsKey(key))
                {
                    Environment.SetEnvironmentVariable(
                        key,
                        null,
                        EnvironmentVariableTarget.Process
                    );
                }
            }

            // Add/set machine variables
            foreach (var (key, value) in machineVariables)
            {
                Environment.SetEnvironmentVariable(key, value, EnvironmentVariableTarget.Process);
            }

            // Add/set user variables
            foreach (var (key, value) in userVariables)
            {
                Environment.SetEnvironmentVariable(key, value, EnvironmentVariableTarget.Process);
            }
        }
    }
}
