#nullable enable
using System.Diagnostics;

namespace PowerKit.Extensions;

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
    }
}
