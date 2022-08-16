using System;
using System.Collections.Generic;
using System.Linq;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	static class Numbers
	{
		public static bool IsEven(this int number)
		{
			return (number & 1) == 0;
		}

		public static long Truncate(this double number)
		{
			if (Math.Abs(number) < 1E-08)
			{
				return 0L;
			}
			if (number >= 0.0)
			{
				return (long)Math.Floor(number);
			}
			return (long)Math.Ceiling(number);
		}

		public static bool IsOdd(this int number)
		{
			return (number & 1) == 1;
		}

		public static bool IsPowerOfTwo(this int number)
		{
			return number > 0 && (number & (number - 1)) == 0;
		}

		public static bool IsPowerOfTwo(this long number)
		{
			return number > 0L && (number & (number - 1L)) == 0L;
		}

		public static int CeilingToPowerOfTwo(this int number)
		{
			if (number == -2147483648)
			{
				return 0;
			}
			if (number > 1073741824)
			{
				throw new ArgumentOutOfRangeException("number");
			}
			number--;
			number |= number >> 1;
			number |= number >> 2;
			number |= number >> 4;
			number |= number >> 8;
			number |= number >> 16;
			return number + 1;
		}

		public static int PowerOfTwo(this int exponent)
		{
			if (exponent < 0 || exponent >= 31)
			{
				throw new ArgumentOutOfRangeException("exponent");
			}
			return 1 << exponent;
		}

		public static int GreatestCommonDivisor(IList<int> integers)
		{
			if (integers == null)
			{
				throw new ArgumentNullException("integers");
			}
			if (integers.Count == 0)
			{
				return 0;
			}
			int num = Math.Abs(integers[0]);
			int num2 = 1;
			while (num2 < integers.Count && num > 1)
			{
				num = Numbers.GreatestCommonDivisor(new int[]
				{
					num,
					integers[num2]
				});
				num2++;
			}
			return num;
		}

		public static int GreatestCommonDivisor(params int[] integers)
		{
			return Numbers.GreatestCommonDivisor(integers.ToList<int>());
		}

		public static int LeastCommonMultiple(IList<int> integers)
		{
			if (integers == null)
			{
				throw new ArgumentNullException("integers");
			}
			if (integers.Count == 0)
			{
				return 1;
			}
			int num = Math.Abs(integers[0]);
			for (int i = 1; i < integers.Count; i++)
			{
				num = Numbers.LeastCommonMultiple(new int[]
				{
					num,
					integers[i]
				});
			}
			return num;
		}

		public static int LeastCommonMultiple(params int[] integers)
		{
			return Numbers.LeastCommonMultiple((IList<int>)integers);
		}

		public static bool AlmostEqual(double a, double b, int numberOfDigits = 5)
		{
			return Math.Abs((a - b) * Math.Pow(10.0, (double)numberOfDigits) - 1.0) < 1E-08;
		}
	}
}
