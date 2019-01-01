using System;
using System.Windows.Input;

namespace PsGui.Converters
    {
    public class CommandHandler : ICommand
        {
        Action<object>             _execMethod;
        Func<object, bool>         _canExecMethod;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="execMethod"></param>
        /// <param name="canExecMethod"></param>
        public CommandHandler(Action<object> execMethod, Func<object, bool> canExecMethod)
            {
            _execMethod = execMethod;
            _canExecMethod = canExecMethod;
            }

        /// <summary>
        /// If CanExecute() returns true, then Execute()
        /// fires.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
            {
            if(_canExecMethod != null)
                {
                return _canExecMethod(parameter);
                }
            else
                {
                return false;
                }
            }

        /// <summary>
        /// Executes the supplied method.
        /// </summary>
        /// <param name="parameter"></param>
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
