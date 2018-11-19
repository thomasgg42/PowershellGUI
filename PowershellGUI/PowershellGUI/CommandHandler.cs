/*
 * Credit: ethicallogics @Stackoverflow
 * https://stackoverflow.com/questions/12422945/how-to-bind-wpf-button-to-a-command-in-viewmodelbase
 * 
 * ser ut til at denne skal inn i view model
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
        private Action             action;
        private bool               canExecute;
        private event EventHandler canExecuteChangedInternal;

        public event EventHandler CanExecuteChanged
            {
            add
                {
                CommandManager.RequerySuggested += value;
                this.canExecuteChangedInternal += value;
                }
            remove
                {
                CommandManager.RequerySuggested -= value;
                this.canExecuteChangedInternal -= value;
                }
            }


        public CommandHandler(Action action, bool canExecute)
            {
            this.action = action;
            this.canExecute = canExecute;
            }

        public bool CanExecute(object parameter)
            {
            return canExecute;
            }

        public void Execute(object parameter)
            {
            action();
            }

        public void OnCanExecuteChanged()
            {
            EventHandler handler = this.canExecuteChangedInternal;
            if(handler != null)
                {
                handler.Invoke(this, EventArgs.Empty);
                }
            }

        }
    }
