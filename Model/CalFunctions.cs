using DevExpress.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    // 사용 계산식
    public class CalFunctions
    {
        public double Add(double x, double y) => x + y;
        public double Sub(double x, double y) => x - y;
        public double Mul(double x, double y) => x * y;
        public double Div(double x, double y) => y == 0 ? throw
            new DivideByZeroException() : x / y;
    }
}
