using Calculator.Model;
using Calculator.View;
using Calculator.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Calculator
{
    public class Cal_ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Action<string, string> OnCalculationFinished;
        private readonly CalFunctions _model;
        private string _display = "0";
        private string _topDisplay = "";
        private bool _isNewInput = true; // 새 입력 상태 여부
        private bool _isResultDisplayed = false; // 결과가 표시된 상태여부

        // 연산 우선 순위
        private Stack<double> _numberStack = new Stack<double>();
        private Stack<String> _operatorStack = new Stack<string>();

        // = 반복
        private string _lastOp = null;
        private double _lastNum = 0;
        private bool _hasLastOp = false;

        private readonly Dictionary<string, int> _precedence = new Dictionary<string, int>
        {
            {"+", 1},
            {"-", 1},
            {"×", 2},
            {"÷", 2}
        };
        // 메모지 기능
        //public ObservableCollection<NoteItem> NoteHistory { get; } = new ObservableCollection<NoteItem>();

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
                _topDisplay = value ?? "";
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

        // 메모리 기능
        public ICommand MemoryAddCommand { get; }
        public ICommand MemoryReadCommand { get; }
        public ICommand MemoryClearCommand { get; }


        public Cal_ViewModel()
        {
            _model = new CalFunctions();
            EnterNumberCommand = new RelayCommand(EnterNumber);
            SetOperationCommand = new RelayCommand(SetOperation);
            CalculateCommand = new RelayCommand(Calculate);
            ClearCommand = new RelayCommand(Clear);
            BackCommand = new RelayCommand(Back);
            NewDot = new RelayCommand(GetNewDot);

            MemoryAddCommand = new RelayCommand(MemoryAdd);
            MemoryReadCommand = new RelayCommand(MemoryRead);
            MemoryClearCommand = new RelayCommand(MemoryClear);

        }

        // 숫자, ±, % 입력
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
                if (double.TryParse(Display, out double current))
                {
                    double percentValue = 0; // 퍼센트 값 초기화

                    if (_operatorStack.Count > 0 && _numberStack.Count > 0)
                    {
                        string lastOperator = _operatorStack.Peek();
                        double baseNumber = _numberStack.Peek();

                        if (lastOperator == "+" || lastOperator == "-")
                        {
                            percentValue = baseNumber * current / 100.0;
                        }
                        else
                        {
                            percentValue = current / 100.0;
                        }
                    }

                    Display = percentValue.ToString();
                    _isNewInput = true;
                }
                return;
            }
            
            if (double.TryParse(input, out _))
            {
                if (_isNewInput || _isResultDisplayed || Display == "0")
                {
                    if (_isResultDisplayed)
                    {   
                        // 결과가 표시된 상태에서 새로운 숫자를 입력하면 초기화
                        _numberStack.Clear();
                        _operatorStack.Clear();
                        CalcExp = "";
                        _isResultDisplayed = false;
                    }

                    Display = input;
                    _isNewInput = false;

                }
                else
                {
                    Display += input;
                }
            }
        }

        // 소수점 입력
        private void GetNewDot(object parameter)
        {
            if (_isNewInput || _isResultDisplayed)
            {
                Display = "0.";
                _isNewInput = false;
                _isResultDisplayed = false;
            }
            else if (!Display.Contains("."))
            {
                Display += ".";
            }
        }

        // 연산자 입력
        private void SetOperation(object parameter)
        {
            string newOp = parameter.ToString();
            double currentNumber = double.Parse(Display.Replace(",", "")); // 천단위 쉼표 제거

            // 새로운 연산자를 입력할 때 마지막 연산 반복 초기화
            _lastOp = null; // 마지막 연산자
            _lastNum = 0; // 마지막 숫자
            _hasLastOp = false; // 마지막 연산자 존재 여부

            // 연산자가 연속 입력된 경우
            if (_isNewInput && _operatorStack.Count > 0)
            {
                _operatorStack.Pop();
                _operatorStack.Push(newOp);

                // CalcExp의 마지막 연산자를 새 연산자로 변경
                if (!string.IsNullOrWhiteSpace(CalcExp))
                {
                    CalcExp = CalcExp.TrimEnd();
                    int lastSpaceIdx = CalcExp.LastIndexOf(' ');
                    if (lastSpaceIdx >= 0)
                    {
                        CalcExp = CalcExp.Substring(0, lastSpaceIdx) + " " + newOp + " ";
                    }
                }
                return;
            }

            // 계산 후 연산자를 누를 경우 초기화
            if (_isResultDisplayed)
            {
                _operatorStack.Clear();
                CalcExp = "";
                _isResultDisplayed = false; // 결과 표시 플래그 초기화
            }
            if (!_isNewInput)
            {
                _numberStack.Push(currentNumber);
                _isNewInput = true;
            }
            while (_operatorStack.Count > 0 &&
                _precedence.ContainsKey(newOp) &&
                _precedence[_operatorStack.Peek()] >= _precedence[newOp])
            {
                PriorityCalculation();
            }
            _operatorStack.Push(newOp);
            CalcExp += Display + " " + newOp + " ";
        }

        // 계산, '=' 입력
        private void Calculate(object parameter)
        {
            if (_isResultDisplayed)
            {
                if (_hasLastOp)
                {
                    double currentValue = double.Parse(Display);
                    double result = 0;

                    try
                    {
                        switch (_lastOp)
                        {
                            case "+":
                                result = _model.Add(currentValue, _lastNum);
                                break;
                            case "-":
                                result = _model.Sub(currentValue, _lastNum);
                                break;
                            case "×":
                                result = _model.Mul(currentValue, _lastNum);
                                break;
                            case "÷":
                                if (_lastNum == 0)
                                    throw new DivideByZeroException("0으로 나눌 수 없습니다.");
                                result = _model.Div(currentValue, _lastNum);
                                break;
                        }

                        Display = result.ToString();
                        CalcExp = currentValue + " " + _lastOp + " " + _lastNum + " = " + Display;

 
                        _numberStack.Clear();
                        _operatorStack.Clear();
                        _numberStack.Push(result);

                    }
                    catch (Exception ex)
                    {
                        Display = "오류: " + ex.Message;
                        _numberStack.Clear();
                        _operatorStack.Clear();
                    }

                    _isNewInput = true;
                    _isResultDisplayed = true;
                    return;
                }
            }

            double currentNumber;
            if (double.TryParse(Display, out currentNumber))
            {
                _numberStack.Push(currentNumber);
                CalcExp += Display;
            }

            while (_operatorStack.Count > 0)
            {
                PriorityCalculation();
            }

            if (_numberStack.Count > 0)
            {
                double result = _numberStack.Pop();
                Display = result.ToString();
                CalcExp += " = " + Display;

                if (_operatorStack.Count == 0)
                {
                    _lastNum = currentNumber;

                    int lastOpIdx = CalcExp.LastIndexOfAny(new char[] { '+', '-', '×', '÷' });
                    if (lastOpIdx != -1) // 마지막 연산자 인덱스가 유효한 경우, 없으면 -1
                    {
                        _lastOp = CalcExp.Substring(lastOpIdx, 1);
                        _hasLastOp = true;
                    }
                }
                _numberStack.Clear();
                _operatorStack.Clear();
                _numberStack.Push(result); // 결과를 스택에 저장
            }
            var (expression, resultText) = CalculationFinished();

            OnCalculationFinished?.Invoke(expression, resultText);
            _isNewInput = true;
            _isResultDisplayed = true; // 결과표시 플래그
        }
        // 계산 수행 (우선순위)
        private void PriorityCalculation()
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
                _numberStack.Clear();
                _operatorStack.Clear();
                return;
            }
            Display = result.ToString();
            _numberStack.Push(result);
        }

        // 지우기 기능
        private void Clear(object parameter)
        {
            if (parameter.ToString() == "CA")
            {
                Display = "0";
                CalcExp = "";
                _isNewInput = false;
                _isResultDisplayed = false;
                _numberStack.Clear();
                _operatorStack.Clear();
                _hasLastOp = false;

            }
            else if (parameter.ToString() == "CE")
            {
                Display = "0";
                _isNewInput = false;
                _isResultDisplayed = false;
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

        // DB 기능 Save, Load, Clear
        private readonly Cal_Memory _memory = new Cal_Memory();

        public void MemoryAdd(object parameter)
        {
            double value = double.Parse(Display);
            _memory.Save(value);
        }
        private void MemoryRead(object parameter)
        {
            var value = _memory.Load();
            if (value != null)
            {
                double memoryValue = value.Value;
                if (_operatorStack.Count > 0 && _isNewInput)
                {
                    // 저장된 값 후입력
                    Display = memoryValue.ToString();
                    _isNewInput = true;
                }
                else
                {
                    // 저장된 값 선입력
                    Display = memoryValue.ToString();
                    _isNewInput = false;
                    _numberStack.Clear();
                    _operatorStack.Clear();
                    CalcExp = "";
                }

                _isResultDisplayed = false;
            }

        }
        private void MemoryClear(object parameter)
        {
            _memory.Clear();
            Display = "0";
            CalcExp = "";
            _isNewInput = false;
            _isResultDisplayed = false;
            _numberStack.Clear();
            _operatorStack.Clear();
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // 메모지 기능 (계산 종료 후)

        public (string Expression, string Result) CalculationFinished()
        {
            try
            {
                return (CalcExp, Display);

            }
            catch (Exception ex)
            {
                Display = "오류: " + ex.Message;
                _isResultDisplayed = false;
                return (null, null);
            }
        }

    }

}