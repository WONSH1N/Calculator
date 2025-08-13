
using Calculator;
using System;
using System.Collections.Generic;
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
        private string _topDisplay = "";
        private double _firstNumber;
        private string _operation;
        private bool _isNewInput;

        // 연산 우선 순위
        private Stack<double> _numberStack = new Stack<double>();
        private Stack<String> _operatorStack = new Stack<string>();

        private readonly Dictionary<string, int> _precedence = new Dictionary<string, int>
        {
            {"+", 1},
            {"-", 1},
            {"×", 2},
            {"÷", 2}
        };

        // 출력 화면
        public string Display
        {
            get => _display;
            set
            {
                _display = value ?? "0";
                OnPropertyChanged();
            }
        }
        // 계산 표현식
        public string CalcExp
        {
            get => _topDisplay;
            set
            {
                _topDisplay = value ?? "0";
                OnPropertyChanged();
            }
        }
        // 인터페이스
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

        private void EnterNumber(object parameter) // object는 최상위 데이터 타입, 어떤 타입이든 받음
        {
            string input = parameter.ToString();

            if (input == "±")
            {
                if (double.TryParse(Display, out double current)) // out 키워드는 출력 매개변수로 매서드에서 값을 반환할 때 사용
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
                    if (_operation == "×")
                    {
                        Display = ((current / 100)).ToString();
                    }
                    else if (_operation == "+" || _operation == "-")
                    {
                        Display = ((_firstNumber) * current / 100).ToString();
                    }
                }
            }

            if (double.TryParse(input, out _)) 
            {
                if (_isNewInput || Display == "0")
                {
                    Display = input;
                    _isNewInput = false;
                }
                else
                {
                    Display += input;
                    _isNewInput = false;
                }
            }           
        }
        
        private void GetNewDot(object parameter)
        {
            if (!Display.Contains("."))
            {
                Display += ".";
                
            }
            else if (_isNewInput)
            {
                Display = "0.";
                _isNewInput = false;
              
            }
        }

        //private void SetOperation(object parameter)
        //{
        //    if (!_isNewInput) Calculate(null);
        //    _firstNumber = double.Parse(Display);
        //    _operation = parameter.ToString();
        //    _isNewInput = true;

        //    CalcExp = Display + "" + parameter;

        //}
        private void SetOperation(object parameter)
        {
            string newOp = parameter.ToString();
            double currentNumber = double.Parse(Display);
            if (!_isNewInput)
            {
                _numberStack.Push(currentNumber);
                _isNewInput = true;
            }
            while (_operatorStack.Count > 0 &&
                _precedence.ContainsKey(newOp) &&
                _precedence[_operatorStack.Peek()] >= _precedence[newOp])
            {
                PerformCalculation();
            }
            _operatorStack.Push(newOp);
            CalcExp += Display + " " + newOp + " ";
        }
        //private void Calculate(object parameter)
        //{
        //    if (string.IsNullOrEmpty(_operation))
        //    {
        //        Display.ToString();
        //        CalcExp = Display + "=";
        //        _isNewInput = true;
        //    }
            
        //    double secondNumber = double.Parse(Display);
        //    double result;
          
        //    try
        //    {
        //        switch (_operation)
        //        {
        //            case "+": result = _model.Add(_firstNumber, secondNumber); break;
        //            case "-": result = _model.Sub(_firstNumber, secondNumber); break;
        //            case "×": result = _model.Mul(_firstNumber, secondNumber); break;
        //            case "÷": result = _model.Div(_firstNumber, secondNumber); break;

        //            default: return;
        //        }
        //        Display = result.ToString("");
        //        CalcExp = _firstNumber + "" + _operation + "" + secondNumber+ "=" + Display;
        //        _isNewInput = true;
        //        _firstNumber = 0;
        //        _operation = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        Display = $"오류:{ex.Message}";
        //    }
           
        //}
        private void Calculate(object parameter)
        {
            if (!_isNewInput)
            {
                _numberStack.Push(double.Parse(Display));
                CalcExp += Display;
            }
            while (_operatorStack.Count > 0)
            {
                PerformCalculation();
                
            }
            if (_numberStack.Count > 0)
            {
                double result = _numberStack.Pop();
                Display = result.ToString();
                CalcExp += "= " + Display;

            }

            _isNewInput = true;
        }

        private void PerformCalculation()
        {
            if (_numberStack.Count < 2 || _operatorStack.Count == 0)
                return;

            double b = _numberStack.Pop();
            double a = _numberStack.Pop();
            string op = _operatorStack.Pop();

            double result = 0;
            try
            {
                switch (op)
                {
                    case "+": result = _model.Add(a, b); break;
                    case "-": result = _model.Sub(a, b); break;
                    case "×": result = _model.Mul(a, b); break;
                    case "÷":
                        if (b == 0)
                            throw new DivideByZeroException("0으로 나눌 수 없습니다.");
                        result = _model.Div(a, b);
                        break;
                }
            }
            catch (Exception ex)
            {
                Display = $"오류: {ex.Message}";
                return;
            }

            _numberStack.Push(result);
        }

        //private void Clear(object parameter)
        //{
        //    if (parameter.ToString() == "CA")
        //    {
        //        Display = "0";
        //        CalcExp = "0";
        //        _firstNumber = 0;
        //        _operation = null;
        //        _isNewInput = false;
        //    }
        //    else if (parameter.ToString() == "CE")
        //    {
        //        Display = "0";
        //        _firstNumber = 0;
        //        _operation = null;
        //        _isNewInput = false;
        //    }

        //}
        private void Clear(object parameter)
        {
            if (parameter.ToString() == "CA")
            {
                Display = "0";
                CalcExp = "";
                _firstNumber = 0;
                _operation = null;
                _isNewInput = false;

                _numberStack.Clear();
                _operatorStack.Clear();
            }
            else if (parameter.ToString() == "CE")
            {
                Display = "0";
                _isNewInput = false;
            }
        }

        private void Back(object parameter)
        {
            if (parameter.ToString() == "DL")
            {
                if (Display.Length > 0)
                {
                    Display = Display.ToString().Remove(Display.Length - 1);
                    if (Display.Length == 0)
                        Display = "0";

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