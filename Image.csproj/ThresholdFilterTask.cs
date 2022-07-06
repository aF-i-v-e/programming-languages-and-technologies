using System.Collections.Generic;

namespace Recognizer
{
	public static class ThresholdFilterTask
	{
		public static List<double> ReturnNewPixelList(double[,] original)
		{
			var pixelList = new List<double>();
			foreach (var pixel in original)
			{
				pixelList.Add(pixel);
			}
			pixelList.Sort();
			return pixelList;
		}

		public static double ReturnThreshold(double[,] original, double whitePixelsFraction)
		{
			var newPixelList = ReturnNewPixelList(original);
			var n = newPixelList.Count;
			var countWhitePix = (int)(n * whitePixelsFraction);
			var threasholdNumber = n - countWhitePix;
			if (countWhitePix != 0)
				return newPixelList[threasholdNumber];
			else
				return newPixelList[n - 1] + 1;
		}

		public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
		{
			var threshold = ReturnThreshold(original, whitePixelsFraction);
			var arrayWidth = original.GetLength(0);
			var arrayHight = original.GetLength(1);
			var additionalArray = new double[arrayWidth, arrayHight];
			for (int i = 0; i < arrayWidth; i++)
			{
				for (int j = 0; j < arrayHight; j++)
				{
					if (original[i, j] < threshold)
						additionalArray[i, j] = 0.0;
					else
						additionalArray[i, j] = 1.0;
				}
			}
			return additionalArray;
		}
	}
}