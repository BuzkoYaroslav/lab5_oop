using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace library
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MathFunction func = (new CosFunction(1.0d, new XFunction(1.0d)) ^ 2) + (new SinFunction(1.0d, new XFunction(1.0d)) ^ 2);
            MathFunction der = func.Derivative(1);

            label1.Text = func.ToString();
            label2.Text = der.ToString();
        }
    }
}
