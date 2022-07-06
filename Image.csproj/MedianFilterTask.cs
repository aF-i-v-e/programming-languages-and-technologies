using System.Collections.Generic;

namespace Recognizer
{
    internal static class MedianFilterTask
	{
		public static bool IsOk(int x, int y, double[,] original)
		{
			return x >= 0 && x < original.GetLength(0) && y >= 0 && y < original.GetLength(1);
		}

		public static double ReturnResultValue(List<double> medianList)
		{
			medianList.Sort();
			var length = medianList.Count;
			var divLen = length / 2;
			if (length % 2 != 0)
				return medianList[divLen - (length % 2) / 2];
			else
				return (medianList[divLen] + medianList[divLen - 1]) / 2;
		}

		public static double ReturnMedianList(int i, int j, double[,] original)
		{
			var medianList = new List<double>();
			for (int x = i - 1; x <= i + 1; x++)
			{
				for (int y = j - 1; y <= j + 1; y++)
				{
					if (IsOk(x, y, original))
						medianList.Add(original[x, y]);
				}
			}
			return ReturnResultValue(medianList);
		}

		public static double[,] MedianFilter(double[,] original)
		{
			var arrayWidth = original.GetLength(0);
			var arrayHight = original.GetLength(1);
			var additionalArray = new double[arrayWidth, arrayHight];
			for (int i = 0; i < arrayWidth; i++)
			{
				for (int j = 0; j < arrayHight; j++)
					additionalArray[i, j] = ReturnMedianList(i, j, original);
			}
			return additionalArray;
		}
	}
}