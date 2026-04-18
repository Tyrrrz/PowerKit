#nullable enable
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
        /// Starts the process associated with the specified file path or URL, using the operating system shell.
        /// </summary>
        public static Process? StartShellExecute(string fileName) =>
            Process.Start(new ProcessStartInfo(fileName) { UseShellExecute = true });
    }
}
