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
        Action<object>     _execAction;
        Func<bool, object> _canExecAction;

        public CommandHandler(Action<object> execAction, Func<bool, object> canExecAction)
            {
            _execAction = execAction;
            _canExecAction = canExecAction;
            }

        public EventHandler CanExecuteChanged
            {

            }

        public bool CanExecute(object parameter)
            {
            // bestemmer om utføre kommando
            }
    
        public void Execute(object parameter)
            {
            // kjøres om CanExecute returnerer true
            // her kjøres logikken
            }

        }
    }
