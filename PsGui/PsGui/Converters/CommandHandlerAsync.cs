using System;
using System.ServiceModel.Dispatcher;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PsGui.Converters
{
    internal class CommandHandlerAsync : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private bool _isExecuting;
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;
        private readonly IErrorHandler _errorHandler;

        public CommandHandlerAsync(
            Func<Task> execute,
            Func<bool> canExecute = null,
            IErrorHandler errorHandler = null)
        {
            _execute = execute;
            _canExecute = canExecute;
            _errorHandler = errorHandler;
        }

        public bool CanExecute()
        {
            return !_isExecuting && (_canExecute?.Invoke() ?? true);
        }

        public async Task ExecuteAsync()
        {
            if (CanExecute())
            {
                try
                {
                    _isExecuting = true;
                    await _execute();
                }
                finally
                {
                    _isExecuting = false;
                }
            }

            RaiseCanExecuteChanged();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #region Explicit implementations
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute();
        }

        void ICommand.Execute(object parameter)
        {
            ExecuteAsync().FireAndForgetSafeAsync(_errorHandler);
        }
        #endregion
    }
}

/*


 public class RelayCommandAsync : ICommand
    {
        private readonly Func<T, Task> executedMethod;
        private readonly Func<T, bool> canExecuteMethod;
 
        public event EventHandler CanExecuteChanged;
        public RelayCommandAsync(Func<T, Task> execute) : this(execute, null) { }
 
        public RelayCommandAsync(Func<T, Task> execute, Func<T, bool> canExecute)
        {
            this.executedMethod = execute ?? throw new ArgumentNullException("execute");
            this.canExecuteMethod = canExecute;
        }
 
        public bool CanExecute(object parameter) => this.canExecuteMethod == null || this.canExecuteMethod((T)parameter);
        public async void Execute(object parameter) => await this.executedMethod((T)parameter);
        public void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }


*/
