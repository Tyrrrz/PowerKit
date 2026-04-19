#nullable enable
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class ProcessExtensions
{
    extension(Process)
    {
        /// <summary>
        /// Checks whether the process identified by the specified ID is currently running.
        /// </summary>
        public static bool IsRunning(int processId)
        {
            try
            {
                using var process = Process.GetProcessById(processId);
                return !process.HasExited;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Starts a new process using the specified file path and optional arguments.
        /// </summary>
        public static Process Start(string path, IEnumerable<string>? arguments = null)
        {
            var process = new Process { StartInfo = new ProcessStartInfo(path) };

            if (arguments is not null)
            {
                foreach (var argument in arguments)
                    process.StartInfo.ArgumentList.Add(argument);
            }

            process.Start();

            return process;
        }

        /// <summary>
        /// Starts the process associated with the specified file path or URL, using the operating system shell.
        /// </summary>
        public static Process? StartShellExecute(string path, IEnumerable<string>? arguments = null)
        {
            var startInfo = new ProcessStartInfo(path) { UseShellExecute = true };

            if (arguments is not null)
            {
                foreach (var argument in arguments)
                    startInfo.ArgumentList.Add(argument);
            }

            return Process.Start(startInfo);
        }
    }
}
