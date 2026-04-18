#if NETSTANDARD || NET
#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class CommandExtensions
{
    extension(ICommand command)
    {
        /// <summary>
        /// Executes the command with the specified parameter if it can be executed.
        /// </summary>
        public void ExecuteIfCan(object? parameter = null)
        {
            if (command.CanExecute(parameter))
                command.Execute(parameter);
        }
    }
}
#endif
