using System;
using System.Windows.Input;

namespace Calculator
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        public event EventHandler CanExecuteChanged; // ICommand 인터페이스를 구현하기 위해서 필요. WPF에서 UI 요소의 활성화 상태를 변경할 때 사용됨.


        public RelayCommand(Action<T> execute) => _execute = execute;

        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => _execute((T)parameter);

    }
    public class RelayCommand : RelayCommand<object>
    {
        public RelayCommand(Action execute) : base(_ => execute()) 
        {
        }

        public RelayCommand(Action<object> execute) : base(execute)
        {
        }
    }


}