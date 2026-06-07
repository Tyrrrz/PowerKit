#if !NETFRAMEWORK || NET45_OR_GREATER
using System.Windows.Input;

namespace PowerKit.Extensions;

/// <summary>
/// Extensions for <see cref="ICommand" />.
/// </summary>
public static class CommandExtensions
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
