#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class ConsoleExtensions
{
    extension(Console)
    {
        /// <summary>
        /// Temporarily changes the console foreground color and returns a handle that restores the original color when disposed.
        /// </summary>
        public static IDisposable WithForegroundColor(ConsoleColor color)
        {
            var lastColor = Console.ForegroundColor;
            Console.ForegroundColor = color;

            return Disposable.Create(() => Console.ForegroundColor = lastColor);
        }

        /// <summary>
        /// Temporarily changes the console background color and returns a handle that restores the original color when disposed.
        /// </summary>
        public static IDisposable WithBackgroundColor(ConsoleColor color)
        {
            var lastColor = Console.BackgroundColor;
            Console.BackgroundColor = color;

            return Disposable.Create(() => Console.BackgroundColor = lastColor);
        }

        /// <summary>
        /// Temporarily changes the console foreground and background colors and returns a handle that restores the original colors when disposed.
        /// </summary>
        public static IDisposable WithColors(ConsoleColor foregroundColor, ConsoleColor backgroundColor) =>
            Disposable.Merge(
                Console.WithForegroundColor(foregroundColor),
                Console.WithBackgroundColor(backgroundColor)
            );
    }
}
