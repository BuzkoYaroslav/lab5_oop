﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library.Equasion_methods
{
    class GenericIteration: Method
    {
        protected virtual double GenericIterationMethod(MathFunction func, MathFunction ksi, double a, double b, double x0)
        {
            double xn = x0, counter = 0;
            double fDerivativeMin = func.Derivative(1).MinValue(a, b);

            MathFunction iterFunc = new XFunction(1.0d) + ksi * func;

            do
            {
                xn = iterFunc.Calculate(xn);
                counter++;
            } while (counter < maxIterationCount && !(Math.Abs(func.Calculate(xn)) / fDerivativeMin <= epsilanIter));

            if (counter == maxIterationCount) return xn;//  double.PositiveInfinity;

            return xn;
        }
        protected bool IsSuitable(MathFunction func, double a, double b)
        {
            return func.IsContinuous(a, b) && func.IsWithConstSign(a, b);
        }
    }
}
