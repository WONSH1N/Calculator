using System;
using System.Windows.Input;

namespace Calculator
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<T> execute) => _execute = execute;

        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => _execute((T)parameter);

    }
    public class RelayCommand : RelayCommand<object>
    {
        public RelayCommand(Action execute) : base(_ => execute()) { }

        public RelayCommand(Action<object> execute) : base(execute)
        {
        }
    }
}