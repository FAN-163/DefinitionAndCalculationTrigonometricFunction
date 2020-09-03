using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskARSoft
{
    public partial class Form1 : Form
    {
        private string inputFunction;
        private string inputX;
        private string inputY;
        Regex regexOperator = new Regex(@"(\+|\-|\*|\/)(\+|\-|\*|\/)+");
        Regex regexAbsenceOperator= new Regex(@"[0-9][a-z]|[a-z][0-9]|\)([0-9]|[a-z])|[0-9]\(|\)\(|([b-z]|[0-9])(x|y)|(x|y)([a-z]|[0-9])");
        public Form1()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            inputFunction = Function.Text.Replace(" ", "").ToLower();
            MatchCollection matchOperator = regexOperator.Matches(inputFunction);
            MatchCollection matchAbsenceOperator = regexAbsenceOperator.Matches(inputFunction);
            if (matchOperator.Count > 0)
            {
                MessageBox.Show("Не может быть два оператора '+, -, *, /' подрят", "ошибка", MessageBoxButtons.OK);
            }
            if (matchAbsenceOperator.Count > 0)
            {
                MessageBox.Show("Пропущен оператор '+, -, *, /' ", "ошибка", MessageBoxButtons.OK);
            }
            inputX = X.Text.Replace(" ", "").ToLower();
            inputY = Y.Text.Replace(" ", "").ToLower();
            try
            {
                ParsingFunction calculation = new ParsingFunction(inputFunction, inputX, inputY);
                double result = Math.Round(Convert.ToDouble(calculation.functionResult), 2);
                Result.Text = result.ToString();
            }
            catch
            {
                MessageBox.Show("Проверьте правильность написания функции", "Ошибка", MessageBoxButtons.OK);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Function.Text = "sin(x+y-x*y/y)+ 5- ((cos( y)- tg(x + y)+ cos(y*x+y-y)-tg(y)) + pow(x ,y) - min(x,y)) +max(x,y) +x -y";
            X.Text = "10";
            Y.Text = "2";
        }
    }
}
