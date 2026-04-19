#nullable enable
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
#if NET40_OR_GREATER || NETSTANDARD || NET
using System.Collections.Generic;
#endif

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

#if NET40_OR_GREATER || NETSTANDARD || NET
        /// <summary>
        /// Starts a new process using the specified file path and optional arguments.
        /// </summary>
        public static void Start(string path, IReadOnlyList<string>? arguments = null)
        {
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo(path);

#if NET || NETCOREAPP
            if (arguments is not null)
            {
                foreach (var argument in arguments)
                    process.StartInfo.ArgumentList.Add(argument);
            }
#endif

            process.Start();
        }

        /// <summary>
        /// Starts the process associated with the specified file path or URL, using the operating system shell.
        /// </summary>
        public static void StartShellExecute(string path, IReadOnlyList<string>? arguments = null)
        {
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo(path) { UseShellExecute = true };

#if NET || NETCOREAPP
            if (arguments is not null)
            {
                foreach (var argument in arguments)
                    process.StartInfo.ArgumentList.Add(argument);
            }
#endif

            process.Start();
        }
#else
        /// <summary>
        /// Starts the process associated with the specified file path or URL, using the operating system shell.
        /// </summary>
        public static Process? StartShellExecute(string fileName) =>
            Process.Start(new ProcessStartInfo(fileName) { UseShellExecute = true });
#endif
    }
}
