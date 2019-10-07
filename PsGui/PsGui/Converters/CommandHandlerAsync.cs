using System;
using System.ServiceModel.Dispatcher;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PsGui.Converters
{
    internal class CommandHandlerAsync : ICommand
    {
        private readonly Func<object, Task> _execute;
        private readonly Func<object, bool> _canExecute;
        private bool _isExecuting;

        /*
        public CommandHandlerAsync(Func<Task> execute) : this(execute, () => true)
        {
        }
        */

        public CommandHandlerAsync(Func<object, Task> execute, Func<object, bool> canExecute)
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
