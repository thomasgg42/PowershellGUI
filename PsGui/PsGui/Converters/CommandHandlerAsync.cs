using System;
using System.ServiceModel.Dispatcher;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PsGui.Converters
{
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




