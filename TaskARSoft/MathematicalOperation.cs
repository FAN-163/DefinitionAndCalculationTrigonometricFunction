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
    public class MathematicalOperation : Form1
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

        Form1 Form1 = new Form1(); 
        
        Regex regexTrigonometric = new Regex(@"\w+");
        Regex regexMultyplyOrDivideCount = new Regex(@"\*+|\/+");
        Regex regexPlusOrMinusCount = new Regex(@"\-+|\++");
        Regex regexMultyplyOrDivide = new Regex(@"((\-)?\d+(\,[0-9]*)?)(\*|\/)((\-)?\d+(\,[0-9]*)?)");
        Regex regexPlusOrMinus = new Regex(@"((\-)?\d+(\,[0-9]*)?)(\+|\-)(\d+(\,[0-9]*)?)");
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
        public MathematicalOperation(string m, string x, string y)
        {
            this.result = m.Trim('(', ')') ?? throw new ArgumentNullException(nameof(m));
            this.x = x;
            this.y = y;
            SetValues();
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
        private void CalcOperation()                                                                //определяем какой математически оператор cтоит первый умножение или деление
        {                                                                                           //происходит расчет слева на право

            MatchCollection matchMultyplyOrDivideCount = regexMultyplyOrDivideCount.Matches(result);
            MatchCollection matchPlusOrMinusCount = regexPlusOrMinusCount.Matches(result);

            if (result.Contains(divide) || result.Contains(multyply))
            {
                int numberOfIterations = matchMultyplyOrDivideCount.Count;
                for (int i = 0; i < numberOfIterations; i++)
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
            }
            if(result.Contains(plus) || result.Contains(minus))                                    //так же и сложение и вычитание, слева на право
            {
                int numberOfIterations;
                if (matchPlusOrMinusCount[0].ToString().Equals("-") && matchPlusOrMinusCount.Count > 1) numberOfIterations = (matchPlusOrMinusCount.Count - 1);
                else numberOfIterations = matchPlusOrMinusCount.Count;

                for (int i = 0; i < numberOfIterations; i++)
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
        }

        private void SetResult(char c, MatchCollection match)                                        //производим расчет а зависимости от математического оператора
        {
            
            string[] multipliers = match[0].ToString().Split(new char[] { c });
            if (multipliers[0] != "")
            {
                if (c.Equals('*'))
                {
                    res = Convert.ToDouble(multipliers[0]) * Convert.ToDouble(multipliers[1]);
                }
                else if (c.Equals('/'))
                {
                    if (Convert.ToDouble(multipliers[1]) == 0)
                    {
                        MessageBox.Show("В результате подъсчета возникла ситуация, деление на 0","Измените функцию", MessageBoxButtons.OK);
                    }
                    else
                    {
                        res = Convert.ToDouble(multipliers[0]) / Convert.ToDouble(multipliers[1]);
                    }
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
                
            }
            else
            {
                res = - Convert.ToDouble(multipliers[1]) - Convert.ToDouble(multipliers[2]);
                result = result.Replace(match[0].ToString(), res.ToString());
                
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

