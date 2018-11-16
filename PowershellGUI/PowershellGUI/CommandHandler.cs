/*
 * Credit: ethicallogics @Stackoverflow
 * https://stackoverflow.com/questions/12422945/how-to-bind-wpf-button-to-a-command-in-viewmodelbase
 */

using System;
using System.Windows.Input;

namespace PowershellGUI
    {
    /// <summary>
    /// Handles the Click-functionality on the button
    /// </summary>
    public class CommandHandler : ICommand
        {
        private Action _action;
        private bool   _canExecute;

        public event EventHandler CanExecuteChanged;

        public CommandHandler(Action action, bool canExecute)
            {
            _action = action;
            _canExecute = canExecute;
            }

        public bool CanExecute(object parameter)
            {
            return _canExecute;
            }

        public void Execute(object parameter)
            {
            _action();
            }
        }
    }
