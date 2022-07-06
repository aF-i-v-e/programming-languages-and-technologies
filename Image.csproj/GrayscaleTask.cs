namespace Recognizer
{
	public static class GrayscaleTask
	{
		public static double GiveColor(char color, int i, int j, Pixel[,] original)
		{
			if (color == 'R')
				return CalculateAmount(0.299, original[i, j].R);
			else if (color == 'G')
				return CalculateAmount(0.587, original[i, j].G);
			else
				return CalculateAmount(0.114, original[i, j].B);
		}

		public static double CalculateAmount(double coefficient, double color)
		{
			return coefficient * color;
		}

		public static double[,] TransformColor(Pixel[,] original)
		{
			var additionAlarray = new double[original.GetLength(0), original.GetLength(1)];
			var arrayWidth = original.GetLength(0);
			var arrayHeight = original.GetLength(1);
			for (int i = 0; i < arrayWidth; i++)
			{
				for (int j = 0; j < arrayHeight; j++)
				{
					var brightness = (GiveColor('R', i, j, original) + GiveColor('G', i, j, original)
						+ GiveColor('B', i, j, original)) / 255;
					additionAlarray[i, j] = brightness;
				}
			}
			return additionAlarray;
		}

		public static double[,] ToGrayscale(Pixel[,] original)
		{
			var grayscale = new double[original.GetLength(0), original.GetLength(1)];
			grayscale = TransformColor(original);
			return grayscale;
		}
	}
}
