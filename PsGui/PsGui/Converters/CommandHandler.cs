using System;
using System.Windows.Input;

namespace PsGui.Converters
{
    public class CommandHandler : ICommand
    {
        Action<object> _execMethod;
        Func<object, bool> _canExecMethod;

        /// <summary>
        /// Constructor creates a new command.
        /// </summary>
        /// <param name="execMethod">Execution logic.</param>
        /// <param name="canExecMethod">Execution status logic.</param>
        public CommandHandler(Action<object> execMethod, Func<object, bool> canExecMethod)
        {
            _execMethod = execMethod;
            _canExecMethod = canExecMethod;
        }

        /// <summary>
        /// If CanExecute() returns true, then Execute()
        /// fires.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this can be set to null.</param>
        /// <returns>True if the command can be executed; false otherwise.</returns>
        public bool CanExecute(object parameter)
        {
            if (_canExecMethod != null)
            {
                return _canExecMethod(parameter);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Executes the supplied method. Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            _execMethod(parameter);
        }

        /// <summary>
        /// Runs CanExecute continously.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }
    }
}
