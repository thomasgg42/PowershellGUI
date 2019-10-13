using System;
using System.ServiceModel.Dispatcher;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PsGui.Converters
{
    /*
    internal class CommandHandlerAsync : ICommand
    {
        private readonly Func<T, Task> _execute;
        private readonly Func<T, bool> _canExecute;
        private bool _isExecuting;

        
        public CommandHandlerAsync(Func<T, Task> execute) : this(execute, null) { }
        {
        }
        

        public CommandHandlerAsync(Func<T, Task> execute, Func<T, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return !(_isExecuting && _canExecute(parameter));
        }

        public event EventHandler CanExecuteChanged;

        public async void Execute(object parameter)
        {
            _isExecuting = true;
            OnCanExecuteChanged();
            try
            {
                await _execute(parameter);
            }
            finally
            {
                _isExecuting = false;
                OnCanExecuteChanged();
            }
        }

        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
*/

    /*
       public class CommandHandlerAsync : ICommand
       {
           private readonly Func<object, Task> executedMethod;
           private readonly Func<object, bool> canExecuteMethod;

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

           public CommandHandlerAsync(Func<object, Task> execute, Func<object, bool> canExecute)
           {
               this.executedMethod = execute;
               this.canExecuteMethod = canExecute;
           }

           public bool CanExecute(object parameter)
           {
               if (canExecuteMethod != null)
               {
                   return canExecuteMethod(parameter);
               }
               else
               {
                   return false;
               }
           }
           public async void Execute(object parameter) => await this.executedMethod(parameter);
       }
       */

    public class CommandHandlerAsync : ICommand
    {
        private readonly Func<object, Task> executedMethod;
        private readonly Func<object, bool> canExecuteMethod;

        public event EventHandler CanExecuteChanged
        {
            add    { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public CommandHandlerAsync(Func<object, Task> execute) : this(execute, null) { }

        public CommandHandlerAsync(Func<object, Task> execute, Func<object, bool> canExecute)
        {
            this.executedMethod = execute ?? throw new ArgumentNullException("execute");
            this.canExecuteMethod = canExecute;
        }

        public bool CanExecute(object parameter) => this.canExecuteMethod == null || this.canExecuteMethod(parameter);
        public async void Execute(object parameter) => await this.executedMethod(parameter);
    }
}




