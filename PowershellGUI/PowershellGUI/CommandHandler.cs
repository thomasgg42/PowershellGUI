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
        Action _execAction;
        bool   _canExecAction;


        public CommandHandler(Action execAction, bool canExecAction)
            {
            _execAction = execAction;
            _canExecAction = canExecAction;
            }

        public event EventHandler CanExecuteChanged
            {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
            }

        public void RaiseCanExecuteChanged()
            {

            }


        public bool CanExecute(object parameter)
            {
            return _canExecAction;
            }
    
        public void Execute(object parameter)
            {
            // kjøres om CanExecute returnerer true
            // her kjøres logikken
            _execAction();
            }

        }
    }
