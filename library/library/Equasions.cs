﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using library.Equasion_methods;

namespace library
{
    public partial class Equasions : Form
    {
        private static Equasion algebr = new Equasion(new Polynomial(-19, -7, 0, 1), 0);
        private static Equasion trans = new Equasion(new CosFunction(1.0d, new XFunction(1.0d)),
            new XFunction(0.5d) + 1);

        private Equasion eqToSolve = algebr;

        public Equasions()
        {
            InitializeComponent();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            eqToSolve = algebr;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            eqToSolve = trans;
        }

        private void Equasions_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = algebr.ToString();
            richTextBox2.Text = trans.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int index = comboBox1.SelectedIndex;

            double a = Convert.ToDouble(textBox1.Text),
                   b = Convert.ToDouble(textBox2.Text);

            switch (index)
            {
                case 0:
                    eqToSolve.Solve(new SimpleIteration().Solve, a, b);
                    break;
                case 1:
                    eqToSolve.Solve(new NewtonMethod().Solve, a, b);
                    break;
                case 2:
                    eqToSolve.Solve(new ChordMethod().Solve, a, b);
                    break;
                case 3:
                    eqToSolve.Solve(new HalfDivision().Solve, a, b);
                    break;
                default:
                    break;
            }

            MessageBox.Show("Equasion:\n" + eqToSolve.ToString() + "Solution\n X = " + Math.Round(eqToSolve.Solution, 2) + "\n");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new MultiGraph(eqToSolve.Left, eqToSolve.Right).Show();
        }
    }
}
