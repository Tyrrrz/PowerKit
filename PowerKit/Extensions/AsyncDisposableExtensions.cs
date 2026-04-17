#if NET40_OR_GREATER || NETSTANDARD || NET
#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class AsyncDisposableExtensions
{
    // Provides a dynamic and uniform way to deal with async disposable.
    // Used as an abstraction to polyfill IAsyncDisposable implementations in BCL types. For example:
    // - Stream class on .NET Framework 4.6.1 -> calls Dispose()
    // - Stream class on .NET Core 3.0 -> calls DisposeAsync()
    // - Stream class on .NET Standard 2.0 -> calls DisposeAsync() or Dispose(), depending on the runtime
    private readonly struct AsyncDisposableAdapter(IDisposable target) : IAsyncDisposable
    {
        public async ValueTask DisposeAsync()
        {
            if (target is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync().ConfigureAwait(false);
            }
            else
            {
                target.Dispose();
            }
        }
    }

    extension(IDisposable disposable)
    {
        /// <summary>
        /// Wraps the disposable in an <see cref="IAsyncDisposable" /> adapter that calls
        /// <see cref="IAsyncDisposable.DisposeAsync" /> if supported, or falls back to
        /// <see cref="IDisposable.Dispose" />.
        /// </summary>
        public IAsyncDisposable ToAsyncDisposable() => new AsyncDisposableAdapter(disposable);
    }
}
#endif
