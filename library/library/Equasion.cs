using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library
{
    public class Equasion
    {
        public delegate double Solver(Equasion eq, double a, double b);

        public MathFunction Left { get; private set; }
        public MathFunction Right { get; private set; }

        double? solution;

        public Equasion(MathFunction left, MathFunction right)
        {
            this.Left = left;
            this.Right = right;
        }

        public double Solution
        {
            get
            {
                if (solution == null)
                    throw new Exception("Equasion has not solved yet!");

                return (double)solution;
            }
        }

        public override string ToString()
        {
            return Left.ToString() + " = " + Right.ToString() + "\n";
        }

        public void Solve(Solver method, double a, double b)
        {
            solution = method(this, a, b);
        }

    }
}
