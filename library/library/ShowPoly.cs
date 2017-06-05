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
    public partial class ShowPoly : Form
    {
        public ShowPoly()
        {
            InitializeComponent();
        }
        public ShowPoly(string LPoly, string PPoly, string QPoly, string standDiviation): this()
        {
            DisplayText(LPoly, PPoly, QPoly, standDiviation);
        }

        private void DisplayText(string t1, string t2, string t3, string t4)
        {
            label1.Text = t1;
            label2.Text = t2;
            label3.Text = t3;
            label4.Text = t4;
        }
    }
}
