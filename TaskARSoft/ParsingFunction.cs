using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace TaskARSoft 
{
    public class ParsingFunction
    {
        //@"\w{2|3}\(\d+(\W*\d*)*\)|sin\(\d+(\W\d*)*\)|tg\(\d+(\W\d*)*\)|ctg\(\d+(\W\d*)*\)|pow\(\d+\,\d+\)|min\(\d+\,\d+\)|max\(\d+\,\d+\), (\w{2}|\w{3})\(\d+\,\d+\), |(\w{2}|\w{3})\(\w*|\d+\,\w*|\d+\)"

        public string functionResult { get; private set; }
        private string x;
        private string y;
        MatchCollection match;
        MathematicalOperation mathematicalOperation;
        private Regex regexTrigonometricFunction = new Regex(@"(\w{2}|\w{3})\((\w|\d+)(\W(\w|\d+))*\)");
       
        public ParsingFunction(string function, string x, string y)
        {
            this.functionResult = function ?? throw new ArgumentNullException(nameof(function));
            this.x = x;
            this.y = y;

            CheckTrigonometricFunction();
            CalculateFunction();
            CheckBrackets();
        }


        private MatchCollection CheckTrigonometricFunction()
        {
            match = regexTrigonometricFunction.Matches(functionResult);                                     //определяем тригонометрические функции
            return match;
        }

        private void CalculateFunction()
        {
            if (match.Count > 0)
            {
                foreach (Match m in match)
                {
                    mathematicalOperation = new MathematicalOperation(m, x, y);                            //отправляем для рассчета 
                    functionResult = functionResult.Replace(m.ToString(), mathematicalOperation.result);   //переписываем тригонометрические функции на их результаты
                }
            }
        }

        private void CheckBrackets()
        {
            while (functionResult.Contains("("))                                                           
            {
                PlusOrMinus();                                                                             //убираем двойные минусы и минусплюс
                string worckValue = functionResult.Substring(functionResult.LastIndexOf("("),              
                                                             functionResult.IndexOf(")") -                 //определяем последнюю открывающуюся скобку, и первую закрывающуюся
                                                             functionResult.LastIndexOf("(") + 1);         //все что между ними отправляем на расчет, через перегруженный конструктор
                mathematicalOperation = new MathematicalOperation(worckValue);                             //и переписывае найденное значение между скобок на результат расчета
                functionResult = functionResult.Replace(worckValue, mathematicalOperation.result);
            }
            mathematicalOperation = new MathematicalOperation(functionResult);                             //финальный расчет отдельно, так как скобок больше нет
            functionResult = mathematicalOperation.result;
        }
        
        private void PlusOrMinus()
        {
            while (functionResult.Contains("--") || functionResult.Contains("+-"))
            {
                if (functionResult.Contains("--")) functionResult = functionResult.Replace("--", "+");
                if (functionResult.Contains("+-")) functionResult = functionResult.Replace("+-", "-");
            }
        }
    }
}
