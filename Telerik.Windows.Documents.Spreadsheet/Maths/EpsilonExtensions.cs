using System;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	static class EpsilonExtensions
	{
		public static bool IsVerySmall(this double value)
		{
			return Math.Abs(value) < 1E-08;
		}

		public static bool AreClose(double value1, double value2)
		{
			return value1.IsEqualTo(value2) || (value1 - value2).IsVerySmall();
		}

		public static bool IsLessThanOrClose(double value1, double value2)
		{
			return value1 < value2 || EpsilonExtensions.AreClose(value1, value2);
		}

		public static bool IsLessThanOrCloseTo(this double value1, double value2)
		{
			return EpsilonExtensions.IsLessThanOrClose(value1, value2);
		}

		public static bool IsLessOrEqual(double value1, double value2)
		{
			return value1 < value2 + 1E-08;
		}

		public static bool IsLessOrEqualTo(this double value1, double value2)
		{
			return EpsilonExtensions.IsLessOrEqual(value1, value2);
		}

		public static bool IsLessThan(double value1, double value2)
		{
			return value1 < value2 && !EpsilonExtensions.AreClose(value1, value2);
		}

		public static bool IsGreaterThan(double value1, double value2)
		{
			return value1 > value2 && !EpsilonExtensions.AreClose(value1, value2);
		}

		public static bool IsGreaterThanOrClose(double value1, double value2)
		{
			return value1 > value2 || EpsilonExtensions.AreClose(value1, value2);
		}

		public static bool IsEqualTo(this double value1, double value2)
		{
			return Math.Abs(value1 - value2) < 1E-08;
		}

		public static bool IsZero(this double value, double accuracy = 1E-08)
		{
			return Math.Abs(value) < accuracy;
		}

		public static bool IsNotEqualTo(this double value1, double value2)
		{
			return !value1.IsEqualTo(value2);
		}
	}
}
