using System;

namespace Recognizer
{
    internal static class SobelFilterTask
    {
        public static double[,] ReturnTransposedMatrix(double[,] firstMatrix)
        {
            var matrixSide = firstMatrix.GetLength(1);
            var transMatrix = new double[matrixSide, matrixSide];
            for (int i = 0; i < matrixSide; i++)
            {
                for (int j = 0; j < matrixSide; j++)
                {
                    transMatrix[i, j] = firstMatrix[j, i];
                }
            }
            return transMatrix;
        }

        public static double ReturnValue(int x, int y, int len, double[,] filterArray, double[,] original)
        {
            var delta = len / 2;
            var sum = 0.0;
            var ii = 0;
            for (int i = x - delta; i <= x + delta; i++)
            {
                var jj = 0;
                for (int j = y - delta; j <= y + delta; j++)
                {
                    sum += original[i, j] * filterArray[ii, jj];
                    jj += 1;
                }
                ii += 1;
            }
            return sum;
        }

        public static double[,] SobelFilter(double[,] g, double[,] sx)
        {
            var sy = ReturnTransposedMatrix(sx);
            var sideFilter = sx.GetLength(0);
            var delta = sideFilter / 2;
            var width = g.GetLength(0);
            var height = g.GetLength(1);
            var result = new double[width, height];
            for (int x = delta; x < width - delta; x++)
                for (int y = delta; y < height - delta; y++)
                {
                    var gx = ReturnValue(x, y, sideFilter, sx, g);
                    var gy = ReturnValue(x, y, sideFilter, sy, g);
                    result[x, y] = Math.Sqrt(gx * gx + gy * gy);
                }
            return result;
        }
    }
}
