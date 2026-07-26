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
        /// Returns a <see cref="Task{TResult}" /> that resolves to the observed
        /// <see cref="AggregateException" />, or <see langword="null" /> if the task did not fault.
        /// Intended for use on detached (fire-and-forget) tasks.
        /// </summary>
        public Task<AggregateException?> Catch() =>
            task.ContinueWith(
                static t => t.Exception,
                default,
                TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Default
            );

        /// <inheritdoc cref="Catch" />
        [Obsolete("Use Catch() instead.")]
        public Task<AggregateException?> ObserveException() => task.Catch();
    }
}
#endif
