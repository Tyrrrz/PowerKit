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

        /// <summary>
        /// Appends a transformation to the task, returning a new task that resolves to the
        /// result of calling <paramref name="transform" /> after the original task completes.
        /// </summary>
        public async Task<T> Select<T>(Func<T> transform)
        {
            await task.ConfigureAwait(false);
            return transform();
        }
    }

    extension<T>(Task<T> task)
    {
        /// <summary>
        /// Appends a transformation to the task, returning a new task that resolves to the
        /// result of calling <paramref name="transform" /> with the original task's result.
        /// </summary>
        public async Task<TOut> Select<TOut>(Func<T, TOut> transform)
        {
            var result = await task.ConfigureAwait(false);
            return transform(result);
        }
    }
}
#endif
