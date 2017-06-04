using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library
{
    class SLAE
    {
        private static string infinitCountOfSolutionsString = "Infinit number system's solutions";
        private static string noSolutionsString = "No system's solutions";
        private static string incorrectMatrixAndVectorSizes = "Incorrect matrix and vector sizes!";

        private Matrix AInitial, ACurrent;
        private Vector fInitial, fCurrent;

        private Vector solution = null;

        private int[] order;

        public SLAE(Matrix A, Vector f)
        {
            if (f.Count != A.RowsCount)
                throw new Exception(incorrectMatrixAndVectorSizes);

            AInitial = (double[,])A;
            ACurrent = (double[,])A;

            fInitial = (double[])f;
            fCurrent = (double[])f;

            InitOrder();
        }

        public double this[int index]
        {
            get
            {
                return fCurrent[index];
            }
        }
        public double this[int i, int j]
        {
            get
            {
                return ACurrent[i, j];
            }
        }

        public Matrix SystemMatrixCurrent
        {
            get
            {
                return ACurrent;
            }
        }
        public Matrix RightCurrent
        {
            get
            {
                return fCurrent;
            }
        }

        public double Soltion(int index)
        {
            if (solution == null)
                solution = RetrieveSolution();

            return solution[order[index]];
        }

        public int UnknownVariablesCount
        {
            get
            {
                return AInitial.ColumnsCount;
            }
        }
        public int EquasionsCount
        {
            get
            {
                return AInitial.RowsCount;
            }
        }

        private void InitOrder()
        {
            order = new int[AInitial.ColumnsCount];
            for (int i = 0; i < order.Length; i++)
                order[i] = i;
        }

        public Matrix SystemMatrix
        {
            get
            {
                return AInitial;
            }
        }
        public Vector RightPart
        {
            get
            {
                return fInitial;
            }
        }

        public int FindNoZeroColumn(int row, int startColumn)
        {
            for (int i = startColumn; i < ACurrent.ColumnsCount; i++)
            {
                if (ACurrent[row, i] != 0)
                    return i;
            }

            return -1;
        }
        public void MakeCoeficientEqualToOne(int row, int column)
        {
            if (ACurrent[row, column] == 1)
                return;

            double coef = 1.0 / ACurrent[row, column];

            for (int i = 0; i < ACurrent.ColumnsCount; i++)
                ACurrent[row, i] *= coef;

            fCurrent[row] *= coef;
        }
        public void MakeDownEqualToZero(int row, int column)
        {
            for (int i = row + 1; i < ACurrent.RowsCount; i++)
            {
                double coef = -ACurrent[i, column];

                for (int j = row; j < ACurrent.ColumnsCount; j++)
                    ACurrent[i, j] += ACurrent[row, j] * coef;

                fCurrent[i] += fCurrent[row] * coef;
            }
        }

        public void SwapColumns(int col1, int col2)
        {
            ACurrent.SwapColumns(col1, col2);

            int tmp = order[col1];
            order[col1] = order[col2];
            order[col2] = tmp;
        }

        private Vector RetrieveSolution()
        {
            double[] solution = new double[ACurrent.ColumnsCount];
            bool[] isInitialize = new bool[ACurrent.ColumnsCount];

            for (int i = 0; i < solution.Length; i++)
            {
                solution[i] = 1;
                isInitialize[i] = false;
            }

            for (int i = ACurrent.RowsCount - 1; i >= 0; i--)
            {
                int index = FindNoZeroColumn(i, i);

                double newSolution = 0;

                for (int j = index + 1; j < ACurrent.ColumnsCount; j++)
                {
                    newSolution -= ACurrent[i, j] * solution[j];
                }

                newSolution += fCurrent[i];
                newSolution /= ACurrent[i, index];

                if (newSolution != solution[index] && isInitialize[i])
                    throw new Exception(ResponseAboutExceptionalSolution(false));

                solution[index] = newSolution;
                isInitialize[i] = true;
            }

            return solution;
        }
        public Vector InitialOrderSolution()
        {
            if (this.solution == null)
                this.solution = RetrieveSolution();

            Vector solution = (double[])this.solution;

            for (int i = 0; i < UnknownVariablesCount; i++)
            {
                double tmp = solution[i];
                solution[i] = solution[order[i]];
                solution[order[i]] = tmp;
            }

            return solution;
        }

        public static string ResponseAboutExceptionalSolution(bool hasZeroRow)
        {
            return hasZeroRow ? infinitCountOfSolutionsString : noSolutionsString;
        }

    }
}
