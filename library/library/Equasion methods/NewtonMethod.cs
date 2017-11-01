using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library.Equasion_methods
{
    class NewtonMethod : GenericIteration, EquasionMethod
    {
        public double Solve(Equasion eq, double a, double b)
        {
            MathFunction func = eq.Left - eq.Right;

            MathFunction der1 = func.Derivative(1),
                         der2 = func.Derivative(2);

            if (!IsSuitable(der1, a, b) ||
                !IsSuitable(der2, a, b))
                throw new Exception("Method can not be applied!");

            MathFunction ksi = IterationFunc(der1, a, b);

            double x0 = InitialX(func, a, b);

            return GenericIterationMethod(func, ksi, a, b, x0);
        }

        protected double InitialX(MathFunction func, double a, double b)
        {
            return SuitableRandomArgumnent((double arg) => { return func.Calculate(arg) * func.Derivative(2).Calculate(arg) < 0; }, a, b);
        }
        protected MathFunction IterationFunc(MathFunction func, double a, double b)
        {
            return -1.0 / func;
        }
    }
}
