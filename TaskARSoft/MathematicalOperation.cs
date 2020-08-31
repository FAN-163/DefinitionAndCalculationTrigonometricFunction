using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskARSoft
{
    public class MathematicalOperation
    {
        //нужна проверка на 0 после знака /
        
        public string result { get; private set; }
        double res;
        bool flag;
        private char divide = '/';
        private char multyply = '*';
        private char plus = '+';
        private char minus = '-';
        private string trigonometric;
        private string x;
        private string y;
        private string sin = "sin";
        private string cos = "cos";
        private string tg = "tg";
        private string ctg = "ctg";
        private string pow = "pow";
        private string min = "min";
        private string max = "max";
        private Match m;
        Regex regexTrigonometric = new Regex(@"\w+");
        Regex regexMultyplyOrDivide = new Regex(@"((\-)?\d*(\,[0-9]*)?)(\*|\/)((\-)?\d+(\,[0-9]*)?)");
        Regex regexPlusOrMinus = new Regex(@"((\-)?\d*(\,[0-9]*)?)(\+|\-)((\-)?\d+(\,[0-9]*)?)");
        public MathematicalOperation(Match m, string x, string y)
        {
            this.m = m ?? throw new ArgumentNullException(nameof(m));
            this.x = x;
            this.y = y;
            CheckTrigonometric();
            SetValues();
            CalcOperation();
            CalcTrigonometrik();
        }
        public MathematicalOperation(string m)
        {
            this.result = m.Trim('(', ')') ?? throw new ArgumentNullException(nameof(m));
            
            CalcOperation();
        }



        private void CheckTrigonometric()                                                           //определяем название тригонометрический функции
        {
            MatchCollection matchTrigonometric = regexTrigonometric.Matches(m.ToString());         
            trigonometric = matchTrigonometric[0].ToString();
            result = m.ToString().Replace(trigonometric, "").Trim('(', ')');
        }
        private void SetValues()
        {
            result = result.Replace("x", x).Replace("y", y);
        }
        private void CalcOperation()                                                                //определяем какой математически оператор чтоит первый умножение или деление
        {                                                                                           //происходит расчет слева на право
            flag = true;
            while (result.Contains(divide) || result.Contains(multyply))
            {
                MatchCollection matchMultyplyOrDivide = regexMultyplyOrDivide.Matches(result);

                if (matchMultyplyOrDivide[0].ToString().Contains(multyply))
                {
                    SetResult(multyply, matchMultyplyOrDivide);
                }
                else
                {
                    SetResult(divide, matchMultyplyOrDivide);
                }
            }
            while ( flag == true && (result.Contains(plus) || result.Contains(minus)))               //так же и сложение и вычитание, слева на право
            {
                MatchCollection matchPlusOrMinus = regexPlusOrMinus.Matches(result);
                
                if (matchPlusOrMinus[0].ToString().Contains(plus))
                {
                    SetResult(plus, matchPlusOrMinus);
                }
                else
                {
                    SetResult(minus, matchPlusOrMinus);
                }
            }
        }

        private bool SetResult(char c, MatchCollection match)                                        //производим расчет а зависимости от математического оператора
        {
            string[] multipliers = match[0].ToString().Split(new char[] { c });
            if (multipliers[0] != "" && multipliers[1] != "")
            {
                if (c.Equals('*'))
                {
                    res = Convert.ToDouble(multipliers[0]) * Convert.ToDouble(multipliers[1]);
                }
                else if (c.Equals('/'))
                {
                    res = Convert.ToDouble(multipliers[0]) / Convert.ToDouble(multipliers[1]);
                }
                else if (c.Equals('+'))
                {
                    res = Convert.ToDouble(multipliers[0]) + Convert.ToDouble(multipliers[1]);
                }
                else if (c.Equals('-'))
                {
                    res = Convert.ToDouble(multipliers[0]) - Convert.ToDouble(multipliers[1]);
                }
                result = result.Replace(match[0].ToString(), res.ToString());
                return flag = true;
            }
            else
            {
                return flag = false;
            }
        }

        private void CalcTrigonometrik()                                                             //Расчитываем тригонометрические функции в зависимости от переменной определенной выше
        {
            if (trigonometric.Equals(cos))
            {
                result = (Math.Cos(Convert.ToDouble(result))).ToString();
            }
            else if (trigonometric.Equals(sin))
            {
                result = (Math.Sin(Convert.ToDouble(result))).ToString();
            }
            else if (trigonometric.Equals(tg))
            {
                result = (Math.Tan(Convert.ToDouble(result))).ToString();
            }
            else if (trigonometric.Equals(ctg))
            {
                result = (Math.Tan(1 / Convert.ToDouble(result))).ToString();
            }
            else if (trigonometric.Equals(min))
            {
                string[] value = result.Split(new char[] { ',' });
                result = (Math.Min(Convert.ToDouble(value[0]), Convert.ToDouble(value[1]))).ToString();
            }
            else if (trigonometric.Equals(max))
            {
                string[] value = result.Split(new char[] { ',' });
                result = (Math.Max(Convert.ToDouble(value[0]), Convert.ToDouble(value[1]))).ToString();
            }
            else if (trigonometric.Equals(pow))
            {
                string[] value = result.Split(new char[] { ',' });
                result = (Math.Pow(Convert.ToDouble(value[0]), Convert.ToDouble(value[1]))).ToString();
            }
        }
    }
}

