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
    public partial class Solution : Form
    {
        SLAE system;

        public Solution()
        {
            InitializeComponent();
        }
        public Solution(SLAE system): this()
        {
            this.system = system;
        }

        private void Solution_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = system.ToString();
            richTextBox2.Text = system.SolutionString();
        }
    }
}
