﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library
{
    public class Vector
    {
        private const string incorrectVectorString = "Matrix has incorrect lengths!";
        private const string vectorAndMatrixCannotBeMultipliedString = "Matricies cannot be multiplied!";
        private const string vectorsCannotBeAddedString = "Maticies cannot be added!";
        private const string vectorsCannotBeSubstrainedString = "Maticies cannot be substrained!";

        double[] elements;

        public Vector(double[] vector)
        {
            elements = new double[vector.Length];

            for (int i = 0; i < elements.Length; i++)
                elements[i] = vector[i];
        }

        public double this[int i]
        {
            get
            {
                if (i < 0 || i >= Count)
                    throw new IndexOutOfRangeException();
                return elements[i];
            }
            set
            {
                if (i < 0 || i >= Count)
                    throw new IndexOutOfRangeException();
                elements[i] = value;
            }
        }

        public int Count
        {
            get
            {
                return elements.Length;
            }
        }
        public double FirstNorm
        {
            get
            {
                return Math.Max(Math.Abs(MaxValue), Math.Abs(MinValue));
            }
        }

        public double MaxValue
        {
            get
            {
                double max = elements[0];

                for (int i = 0; i < Count; i++)
                    if (elements[i] > max)
                        max = elements[i];

                return max;
            }
        }
        public double MinValue
        {
            get
            {
                double min = elements[0];

                for (int i = 0; i < Count; i++)
                    if (elements[i] < min)
                        min = elements[i];

                return min;
            }
        }

        public void SwapRows(int rowIndex1, int rowIndex2)
        {
            double tmp = elements[rowIndex1];
            elements[rowIndex1] = elements[rowIndex2];
            elements[rowIndex2] = tmp;
        }

        private static Vector AddVectors(Vector vect1, Vector vect2)
        {
            double[] vect = new double[vect1.Count];

            for (int i = 0; i < vect1.Count; i++)
                    vect[i] = vect1[i] + vect2[i];

            return vect;
        }

        public static Vector operator *(double number, Vector vect)
        {
            Vector copyVect = (double[])vect;

            for (int i = 0; i < copyVect.Count; i++)
                    copyVect[i] *= number;

            return copyVect;
        }
        public static Vector operator *(Vector vect, double number)
        {
            return number * vect;
        }
        public static Vector operator *(Vector vect, Matrix matrix)
        {
            if (vect.Count != matrix.RowsCount)
                throw new Exception(vectorAndMatrixCannotBeMultipliedString);

            double[] result = new double[vect.Count];

            int rowCount = matrix.RowsCount;

            int length = vect.Count;

            for (int i = 0; i < length; i++)
            {
                result[i] = 0;
                for (int p = 0; p < rowCount; p++)
                    result[i] += matrix[p, i] * vect[p];
            }

            return result;
        }
        public static Vector operator *(Matrix matrix, Vector vect)
        {
            if (vect.Count != matrix.ColumnsCount)
                throw new Exception(vectorAndMatrixCannotBeMultipliedString);

            double[] result = new double[vect.Count];

            int colCount = matrix.ColumnsCount;

            int length = vect.Count;

            for (int i = 0; i < length; i++)
            {
                result[i] = 0;
                for (int p = 0; p < colCount; p++)
                    result[i] += matrix[i, p] * vect[p];
            }

            return result;
        }
        public static Vector operator +(Vector left, Vector right)
        {
            if (left.Count != right.Count)
                throw new Exception(vectorsCannotBeAddedString);

            return AddVectors(left, right);
        }
        public static Vector operator -(Vector left, Vector right)
        {
            if (left.Count != right.Count)
                throw new Exception(vectorsCannotBeSubstrainedString);

            return AddVectors(left, -right);
        }
        public static Vector operator -(Vector vector)
        {
            Vector copyVector = (double[])vector;

            for (int i = 0; i < copyVector.Count; i++)
                 copyVector[i] = -copyVector[i];

            return copyVector;
        }

        public static explicit operator double[] (Vector vector)
        {
            return vector.elements;
        }
        public static implicit operator Vector(double[] elements)
        {
            return new Vector(elements);
        }
        public static implicit operator Matrix(Vector vect)
        {
            return new Matrix(vect.elements);
        }
    }
}
