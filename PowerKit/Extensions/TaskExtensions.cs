#if !NETFRAMEWORK || NET45_OR_GREATER
using System;
using System.Threading.Tasks;

namespace PowerKit.Extensions;

/// <summary>
/// Extensions for <see cref="Task" />.
/// </summary>
public static class TaskExtensions
{
    extension(Task task)
    {
        /// <summary>
        /// Registers a continuation that observes and suppresses the task's exception,
        /// preventing it from surfacing as an unobserved task exception.
        /// Intended for use on detached (fire-and-forget) tasks.
        /// </summary>
        public void ObserveException() =>
            task.ContinueWith(
                t => _ = t.Exception,
                default,
                TaskContinuationOptions.OnlyOnFaulted,
                TaskScheduler.Default
            );
    }

    extension<T>(Task<T> task)
    {
        /// <summary>
        /// Registers a continuation that observes and suppresses the task's exception,
        /// preventing it from surfacing as an unobserved task exception.
        /// Intended for use on detached (fire-and-forget) tasks.
        /// </summary>
        public void ObserveException() =>
            task.ContinueWith(
                t => _ = t.Exception,
                default,
                TaskContinuationOptions.OnlyOnFaulted,
                TaskScheduler.Default
            );
    }
}
#endif
