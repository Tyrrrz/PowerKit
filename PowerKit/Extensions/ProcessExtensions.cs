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

#if NET40_OR_GREATER || NETSTANDARD || NET
        /// <summary>
        /// Starts a new process using the specified file path and optional arguments.
        /// </summary>
        public static Process Start(string path, IReadOnlyList<string>? arguments = null)
        {
            var process = new Process();
            process.StartInfo = new ProcessStartInfo(path);

#if NET || NETCOREAPP
            if (arguments is not null)
            {
                foreach (var argument in arguments)
                    process.StartInfo.ArgumentList.Add(argument);
            }
#else
            if (arguments is not null)
            {
                var sb = new System.Text.StringBuilder();
                foreach (var argument in arguments)
                {
                    if (sb.Length > 0)
                        sb.Append(' ');
                    sb.Append('"');
                    sb.Append(argument.Replace("\\", "\\\\").Replace("\"", "\\\""));
                    sb.Append('"');
                }
                process.StartInfo.Arguments = sb.ToString();
            }
#endif

            process.Start();
            return process;
        }

        /// <summary>
        /// Starts the process associated with the specified file path or URL, using the operating system shell.
        /// </summary>
        public static Process? StartShellExecute(
            string path,
            IReadOnlyList<string>? arguments = null
        )
        {
            var startInfo = new ProcessStartInfo(path) { UseShellExecute = true };

#if NET || NETCOREAPP
            if (arguments is not null)
            {
                foreach (var argument in arguments)
                    startInfo.ArgumentList.Add(argument);
            }
#else
            if (arguments is not null)
            {
                var sb = new System.Text.StringBuilder();
                foreach (var argument in arguments)
                {
                    if (sb.Length > 0)
                        sb.Append(' ');
                    sb.Append('"');
                    sb.Append(argument.Replace("\\", "\\\\").Replace("\"", "\\\""));
                    sb.Append('"');
                }
                startInfo.Arguments = sb.ToString();
            }
#endif

            return Process.Start(startInfo);
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
