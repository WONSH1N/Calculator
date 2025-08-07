
using Calculator;
using System;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Calculator
{
    public class Cal_ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly CalFunctions _model;
        private string _display = "0";
        private string _topDisplay = "0";
        private double _firstNumber;
        private string _operation;
        private bool _isNewInput;
        private bool _isNewDot;

        public string Display
        {
            get => _display;
            set
            {
                _display = value ?? "0";
                OnPropertyChanged();
            }
        }

        public string CalcExp
        {
            get => _topDisplay;
            set
            {
                _topDisplay = value ?? "0";
                OnPropertyChanged();
            }
        }

        public ICommand EnterNumberCommand { get; }
        public ICommand SetOperationCommand { get; }
        public ICommand CalculateCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand NewDot { get; }
        public ICommand BackCommand { get; }

        public Cal_ViewModel()
        {
            _model = new CalFunctions();
            _isNewInput = true;
            EnterNumberCommand = new RelayCommand(EnterNumber);
            SetOperationCommand = new RelayCommand(SetOperation);
            CalculateCommand = new RelayCommand(Calculate);
            ClearCommand = new RelayCommand(Clear);
            NewDot = new RelayCommand(GetNewDot);
            BackCommand = new RelayCommand(Back);
        }

        private void EnterNumber(object parameter)
        {
            string input = parameter.ToString();

            if (input == "±")
            {
                double current;
                if (double.TryParse(Display, out current))
                {
                    current = -current;
                    Display = current.ToString();
                }
                return;
            }
            if (input == "%")
            {
                double current;
                if (double.TryParse(Display, out current))
                {
                    Display = (_firstNumber * (current / 100)).ToString();
                }
            }

            double temp;
            if (double.TryParse(input, out temp))
            {
                if (_isNewInput)
                {
                    Display = input;
                    _isNewInput = false;
                }
                else
                { 
                    if(Display.StartsWith("0"))
                    {
                        Display = string.Empty;
                    }
                        Display += input;                    
                }
            }
        }

        private void GetNewDot(object parameter)
        {
            if (!Display.Contains("."))
            {
                Display += ".";
            }

        }

        private void SetOperation(object parameter)
        {
            if (!_isNewInput) Calculate(null);
            _firstNumber = double.Parse(Display);
            _operation = parameter.ToString();
            _isNewInput = true;

            CalcExp = Display + "" + parameter;

        }

        private void Calculate(object parameter)
        {
            if (string.IsNullOrEmpty(_operation)) return;
            double secondNumber = double.Parse(Display);
            double result;

            try
            {
                switch (_operation)
                {
                    case "+": result = _model.Add(_firstNumber, secondNumber); break;
                    case "-": result = _model.Sub(_firstNumber, secondNumber); break;
                    case "*": result = _model.Mul(_firstNumber, secondNumber); break;
                    case "/": result = _model.Div(_firstNumber, secondNumber); break;

                    default: return;
                }
                Display = result.ToString("");//소수점3자리 f3
                _isNewInput = true;
            }
            catch (Exception ex)
            {
                Display = $"오류:{ex.Message}";
            }

        }
        private void Clear(object parameter)
        {
            if (parameter.ToString() == "CA")
            {
                Display = "0";
                CalcExp = "0";
                _firstNumber = 0;
                _operation = null;
                _isNewInput = true;
            }
            else if (parameter.ToString() == "CE")
            {
                Display = "0";
                _firstNumber = 0;
                _operation = null;
                _isNewInput = true;
            }

        }
        private void Back(object parameter)
        {
            if (parameter.ToString() == "DL")
            {
                if (Display.Length != 0)
                {
                    Display = Display.ToString().Remove(Display.Length - 1);
                }
                else
                {
                    Display = "0";
                }
            }

        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
