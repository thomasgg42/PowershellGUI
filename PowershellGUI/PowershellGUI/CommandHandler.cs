using System;
using System.Windows.Input;

namespace PowershellGUI
    {
    /// <summary>
    /// Handles the Click-functionality on the button
    /// </summary>
    public class CommandHandler : ICommand
        {
        private Action<object>     _executeAction;
        private Func<object, bool> _canExecuteAction;
        private event EventHandler CanExecuteChangedInternal;

        public CommandHandler(Action<object> execAction, Func<object, bool> canExecAction)
            {
            _executeAction = execAction;
            _canExecuteAction = canExecAction;
            }

        public event EventHandler CanExecuteChanged
            {
            // event logic which runs canExecute continously
            add
                {
                CommandManager.RequerySuggested += value;
                CanExecuteChangedInternal += value;
                }
            remove {
                CommandManager.RequerySuggested -= value;
                CanExecuteChangedInternal += value;
                }
            }

        public bool CanExecute(object parameter)
            {
            // return _canExecuteAction != null && canExecute(parameter);
            // return _canExecuteAction;
            if(_canExecuteAction != null)
                {
                return _canExecuteAction(parameter);
                }
            else
                {
                return false;
                }
            }

        public void Execute(object parameter)
            {
            // kjøres om CanExecute returnerer true
            // her kjøres logikken
            _executeAction(parameter);
            }

        public void OnCanExecuteChanged()
            {
            EventHandler handler = CanExecuteChangedInternal;
            if(handler != null)
                {
                handler.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
