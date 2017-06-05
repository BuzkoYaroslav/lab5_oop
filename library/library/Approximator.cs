using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library
{
    static class Approximator
    {
        
        private static string methodIsNotApplyableString = "Method is not applyable!";
        private static string incorrectMatrixString = "Matrix has incorrect lengths!";
        private static string matricesCannotBeMultipliedString = "Matricies cannot be multiplied!";
        private static string iterationOverflowString = "Iteration overflow! (Iteration count = {0})";

        public static double epsilan = Math.Pow(10, -4);
        public static double epsilanIter = Math.Pow(10, -3);
        public static int maxIterationCount = 10000000;

        public static Random rnd = new Random();

        #region Accessory methods for equasion's methods

        #region Random for equasion's methods
        public static double RandomNumber(double a, double b)
        {
            return rnd.Next(Convert.ToInt32(a / epsilan), Convert.ToInt32(b / epsilan)) * epsilan;
        }
        public static double SuitableRandomArgumnent(Condition cond, double a, double b)
        {
            double arg;

            do
            {
                arg = RandomNumber(a, b);
            } while (!cond(arg));

            return arg;
        }
        #endregion

        #region Delegates for equasion's methods
        public delegate bool Condition(double func);
        #endregion

        #region Funcs for equasion
        private static MathFunction FuncSimpleIteration(MathFunction func, double a, double b)
        {
            double min = func.MinValue(a, b),
                   max = func.MaxValue(a, b);

            int k = func.IsGreaterThanZero(a, b) ? -1 : 1;

            return k * 2.0 / (min + max);
        }
        private static MathFunction FuncNuitonMethod(MathFunction func, double a, double b)
        {
            return -1.0 / func;
        }
        private static MathFunction FuncHordMethod(MathFunction func, double a, double b, double c)
        {
            return -(new XFunction(1.0d) - c) / (func - func.Calculate(c));
        }
        #endregion

        #region Initial X for equasion
        private static double InitialXSimpleIteration(double a, double b)
        {
            //return RandomNumber(a, b);
            return (a + b) / 2;
        }
        private static double InitialXNuitonMethod(MathFunction func, double a, double b)
        {
            return SuitableRandomArgumnent((double arg) => { return func.Calculate(arg) * func.Derivative(2).Calculate(arg) < 0; }, a, b);
        }
        private static double InitialXHordMethod(MathFunction func, double a, double b, double c)
        {
            double val = func.Calculate(c);
            return SuitableRandomArgumnent((double x) => { return func.Calculate(x) * val < 0; }, a, b);
        }
        #endregion

        private static double GenericIteration(MathFunction func, MathFunction ksi, double a, double b, double x0)
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

        #endregion

        #region Main methods for equasion
        private static bool IsSuitable(MathFunction func, double a, double b)
        {
            return func.IsContinuous(a, b) && func.IsWithConstSign(a, b);
        }

        public static double SimpleIteration(Equasion eq, double a, double b)
        {
            MathFunction func = eq.Left - eq.Right;

            MathFunction der1 = func.Derivative(1);

            if (!IsSuitable(der1, a, b))
                throw new Exception("Method can not be applied!");

            MathFunction ksi = FuncSimpleIteration(der1, a, b);

            double x0 = InitialXSimpleIteration(a, b);

            return GenericIteration(func, ksi, a, b, x0);
        }
        public static double NewtonMethod(Equasion eq, double a, double b)
        {
            MathFunction func = eq.Left - eq.Right;

            MathFunction der1 = func.Derivative(1),
                         der2 = func.Derivative(2);

            if (!IsSuitable(der1, a, b) ||
                !IsSuitable(der2, a, b))
                throw new Exception("Method can not be applied!");

            MathFunction ksi = FuncNuitonMethod(der1, a, b);

            double x0 = InitialXNuitonMethod(func, a, b);

            return GenericIteration(func, ksi, a, b, x0);
        }
        public static double ChordsMethod(Equasion eq, double a, double b)
        {
            MathFunction func = eq.Left - eq.Right;

            MathFunction der1 = func.Derivative(1),
                         der2 = func.Derivative(2);

            if (!IsSuitable(der1, a, b) ||
                !IsSuitable(der2, a, b))
                throw new Exception("Method can not be applied!");

            double c = SuitableRandomArgumnent((double x) => { return func.Calculate(x) * der2.Calculate(x) > 0; }, a, b);

            MathFunction ksi = FuncHordMethod(func, a, b, c);

            double x0 = InitialXHordMethod(func, a, b, c);

            return GenericIteration(func, ksi, a, b, x0);
        }
        public static double HalfDivision(Equasion eq, double a, double b)
        {
            MathFunction func = eq.Left - eq.Right;

            if (func.Calculate(a) * func.Calculate(b) > 0)
                return double.PositiveInfinity;

            if (Math.Abs(func.Calculate(a)) <= epsilanIter) return a;
            if (Math.Abs(func.Calculate(b)) <= epsilanIter) return b;

            double xn = (a + b) / 2;

            while (Math.Abs(func.Calculate(xn)) > epsilanIter)
            {
                if (func.Calculate(xn) * func.Calculate(a) < 0) b = xn;
                else if (func.Calculate(xn) * func.Calculate(b) < 0) a = xn;
                xn = (a + b) / 2;
            }

            return xn;
        }

        public static int SolutionCount(Equasion eq, double a, double b)
        {
            return 0;
        }
        public static Vector ShturmMethod(Equasion eq)
        {
            return null;
        }

        #endregion

        #region Gauss's method

        public static Vector GaussMethod(SLAE system)
        {
            for (int i = 0; i < system.EquasionsCount; i++)
            {
                if (system[i, i] == 0)
                {
                    int index = system.FindNoZeroColumn(i, i);

                    if (index == -1)
                        throw new Exception(SLAE.ResponseAboutExceptionalSolution(system[i] == 0));

                    system.SwapColumns(i, index);
                }

                system.MakeCoeficientEqualToOne(i, i);
                system.MakeDownEqualToZero(i, i);
            }

            return system.InitialOrderSolution(false);
        }
        #endregion

        #region Holeckii's method

        public static Vector HoleckiiMethod(SLAE system)
        {
            if (!system.SystemMatrix.IsNonDegenerate())
                throw new Exception(methodIsNotApplyableString);

            Matrix low, up;

            DetermineLowAndUpMatrix(system.SystemMatrix, out low, out up);

            return new SLAE(up, new SLAE(low, system.RightPart).InitialOrderSolution(true)).InitialOrderSolution(false);
        }

        private static void DetermineLowAndUpMatrix(Matrix matrix, out Matrix low, out Matrix up)
        {
            low = new double[matrix.RowsCount, matrix.ColumnsCount];
            up = new double[matrix.RowsCount, matrix.ColumnsCount];

            int index = 0;

            while (index < matrix.ColumnsCount)
            {
                for (int i = index; i < matrix.ColumnsCount; i++)
                {
                    low[i, index] = matrix[i, index];
                    for (int k = 0; k < index; k++)
                        low[i, index] -= low[i, k] * up[k, index];
                }

                up[index, index] = 1;
                for (int j = index + 1; j < matrix.ColumnsCount; j++)
                {
                    up[index, j] = matrix[index, j];
                    for (int k = 0; k < index; k++)
                        up[index, j] -= low[index, k] * up[k, j];

                    up[index, j] /= low[index, index];
                }

                index++;
            }
        }
        private static Vector GetYSolution(Matrix low, Vector f)
        {
            double[] solution = new double[low.ColumnsCount];

            for (int i = 0; i < low.RowsCount; i++)
            {
                solution[i] = f[i];
                for (int j = 0; j < i; j++)
                {
                    solution[i] -= low[i, j] * solution[j];
                }
                solution[i] /= low[i, i];
            }

            return solution;
        }
        private static Vector GetXSolution(Matrix up, Vector y)
        {
            double[] solution = new double[up.ColumnsCount];

            for (int i = up.RowsCount - 1; i >= 0; i--)
            {
                solution[i] = y[i];
                for (int j = i + 1; j < up.ColumnsCount; j++)
                {
                    solution[i] -= up[i, j] * solution[j];
                }
                solution[i] /= up[i, i];
            }

            return solution;
        }

        #endregion

        #region Iteration methods

        public static Vector SimpleIteration(SLAE system)
        {
            if (!system.SystemMatrix.IsNonDegenerate())
                throw new Exception(methodIsNotApplyableString);

            double alpha = RandomNumber(0, 2.0 / (system.SystemMatrix.Transposed() * system.SystemMatrix).FirstNorm);

            Matrix bMatrix = Matrix.UnaryMatrix(system.SystemMatrix.ColumnsCount) - alpha * system.SystemMatrix.Transposed() * system.SystemMatrix;
                   
            Vector gVector = alpha * (system.SystemMatrix.Transposed() * system.RightPart);

            Vector xCurrent = new double[system.SystemMatrix.ColumnsCount],
                   xPrev;

            int count = 0;

            do
            {
                xPrev = xCurrent;
                xCurrent = bMatrix * xPrev + gVector;

                count++;
            } while (!(bMatrix.FirstNorm > 0.5 && bMatrix.FirstNorm < 1 &&
                       bMatrix.FirstNorm / (1 - bMatrix.FirstNorm) * (xCurrent - xPrev).FirstNorm <= epsilan ||
                    (xCurrent - xPrev).FirstNorm <= epsilan ||
                    count == maxIterationCount));

            if (count == maxIterationCount)
                throw new Exception(string.Format(iterationOverflowString, count));

            return xCurrent;
        }
        public static Vector ZeidelIteration(SLAE system)
        {
            if (!system.SystemMatrix.IsNonDegenerate())
                throw new Exception(methodIsNotApplyableString);

            double alpha = RandomNumber(0, 2.0 / (system.SystemMatrix.Transposed() * system.SystemMatrix).FirstNorm);

            Matrix bMatrix = Matrix.UnaryMatrix(system.SystemMatrix.ColumnsCount) - alpha * system.SystemMatrix.Transposed() * system.SystemMatrix;
            Matrix HMatrix, FMatrix;

            DivideMatrix(bMatrix, out HMatrix, out FMatrix);

            Vector gVector = alpha * system.SystemMatrix.Transposed() * system.RightPart;

            Vector xCurrent = new double[system.SystemMatrix.ColumnsCount],
                   xPrev;

            int count = 0;

            do
            {
                xPrev = xCurrent;
                xCurrent = new double[system.SystemMatrix.ColumnsCount];

                Vector prevPart = FMatrix * xPrev + gVector;

                for (int i = 0; i < xCurrent.Count; i++)
                {
                    xCurrent[i] = prevPart[i];

                    for (int j = 0; j < i; j++)
                        xCurrent[i] += xCurrent[j] * HMatrix[i, j];
                }

                count++;
            } while (!(bMatrix.FirstNorm > 0.5 && bMatrix.FirstNorm < 1 &&
                       bMatrix.FirstNorm / (1 - bMatrix.FirstNorm) * (xCurrent - xPrev).FirstNorm <= epsilan ||
                    (xCurrent - xPrev).FirstNorm <= epsilan ||
                    count == maxIterationCount));

            if (count == maxIterationCount)
                throw new Exception(string.Format(iterationOverflowString, count));

            return xCurrent;
        }

        private static void DivideMatrix(Matrix A, out Matrix H, out Matrix F)
        {
            H = new double[A.RowsCount, A.ColumnsCount];
            F = new double[A.RowsCount, A.ColumnsCount];

            for (int i = 0; i < A.RowsCount; i++)
                for (int j = 0; j < A.ColumnsCount; j++)
                    if (i > j)
                        H[i, j] = A[i, j];
                    else
                        F[i, j] = A[i, j];
        }

        #endregion

        #region Function approximation

        public static Polynomial LagranzPolynomial(double[] x, double[] f)
        {
            if (x.Length != f.Length)
                throw new Exception("Incorrect input data!");

            Polynomial result = new Polynomial();

            for (int i = 0; i < x.Length; i++)
            {
                Polynomial multi = new Polynomial(1);

                for (int j = 0; j < x.Length; j++)
                    if (i != j)
                        multi *= 1.0d / (x[i] - x[j]) * new Polynomial(-x[j], 1);

                multi *= f[i];

                result += multi;
            }

            return result;
        }
        public static Polynomial NewtonPolynomial(double[] x, double[] f)
        {
            if (x.Length != f.Length)
                throw new ArgumentOutOfRangeException("Incorrect arguments!");

            Polynomial result = new Polynomial(f[0]);

            for (int i = 1; i < x.Length; i++)
            {
                Polynomial multi = new Polynomial(1);
                for (int j = 0; j < i; j++)
                    multi *= new Polynomial(-x[j], 1);

                multi *= DividedDifference(x, f, 0, i);

                result += multi;
            }

            return result;
        }
        private static double DividedDifference(double[] x, double[] f, int left, int right)
        {
            if (left < 0 || right < 0 ||
                right < left)
                throw new Exception("Incorrect arguments!");

            if (right == left)
                return f[left];

            return (DividedDifference(x, f, left + 1, right) - DividedDifference(x, f, left, right - 1)) /
                (x[right] - x[left]);
        }

        public static Polynomial MinSquares(double[] x, double[] f, int m) 
        {
            if (x.Length != f.Length)
                throw new ArgumentOutOfRangeException("Incorrect input data!");
            if (m < 0)
                throw new Exception("m index cannot be less than zero!");

            double[,] aMatrix = new double[m + 1, m + 1];
            double[] right = new double[m + 1];

            for (int i = 0; i <= m; i++)
            {
                for (int p = 0; p < x.Length; p++)
                    right[i] += Math.Pow(x[p], i) * f[p];

                for (int j = 0; j <= m; j++)
                    for (int p = 0; p < x.Length; p++)
                        aMatrix[i, j] += Math.Pow(x[p], i + j);
            }

            Matrix A = aMatrix; 
            Vector F = right;

            return new Polynomial((double[])GaussMethod(new SLAE(A, F)));
        }
        public static double StandardDeviation(double[] x, double[] f, Polynomial Q)
        {
            if (x.Length != f.Length)
                throw new ArgumentOutOfRangeException("Incorrect input data!");

            double s = 0;

            for (int i = 0; i < x.Length; i++)
                s += Math.Pow(f[i] - Q.GetValue(x[i]), 2);

            return Math.Sqrt(s / x.Length);
        }

        #endregion
    }
}
