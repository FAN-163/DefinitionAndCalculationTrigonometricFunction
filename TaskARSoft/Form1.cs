using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskARSoft
{
    public partial class Form1 : Form
    {
        private string inputFunction;
        private string inputX;
        private string inputY;
        
        public Form1()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            inputFunction = Function.Text.Replace(" ", "").ToLower();
            inputX = X.Text.Replace(" ", "").ToLower();
            inputY = Y.Text.Replace(" ", "").ToLower();

            ParsingFunction calculation = new ParsingFunction(inputFunction, inputX, inputY);
            double result = Math.Round(Convert.ToDouble(calculation.functionResult), 2);
            Result.Text = result.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Function.Text = "sin(x+y-x*y/y)+ 5- ((cos( y)- tg(x + y)+ cos(y*x+y-y)-tg(y)) + pow(x ,y) - min(x,y)) +max(x,y)";
            X.Text = "10";
            Y.Text = "2";
        }
    }
}
