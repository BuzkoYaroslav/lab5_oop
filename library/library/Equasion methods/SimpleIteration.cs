﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library.Equasion_methods
{
    class SimpleIteration : GenericIteration, EquasionMethod
    {
        public double Solve(Equasion eq, double a, double b)
        {
            MathFunction func = eq.Left - eq.Right;

            MathFunction der1 = func.Derivative(1);

            if (!IsSuitable(der1, a, b))
                throw new Exception("Method can not be applied!");

            MathFunction ksi = IterationFunc(der1, a, b);

            double x0 = InitialX(a, b);

            return GenericIterationMethod(func, ksi, a, b, x0);
        }

        protected double InitialX(double a, double b)
        {
            //return RandomNumber(a, b);
            return (a + b) / 2;
        }
        protected MathFunction IterationFunc(MathFunction func, double a, double b)
        {
            double min = func.MinValue(a, b),
                   max = func.MaxValue(a, b);

            int k = func.IsGreaterThanZero(a, b) ? -1 : 1;

            return k * 2.0 / (min + max);
        }
    }
}
