using System;

namespace Telerik.Documents.SpreadsheetStreaming.Utilities
{
	class MathUtilities
	{
		public static double Clamp(double value, double minimum, double maximum, int precision)
		{
			double num;
			if (value > maximum)
			{
				num = maximum;
			}
			else
			{
				num = value;
			}
			if (num < minimum)
			{
				num = minimum;
			}
			return Math.Round(num, precision);
		}

		public static double CustomMod(double number)
		{
			if (number > 0.0)
			{
				return number - Math.Floor(number);
			}
			if (number < 0.0)
			{
				double num = Math.Abs(number);
				return 1.0 - (num - Math.Floor(num));
			}
			return 0.0;
		}
	}
}
