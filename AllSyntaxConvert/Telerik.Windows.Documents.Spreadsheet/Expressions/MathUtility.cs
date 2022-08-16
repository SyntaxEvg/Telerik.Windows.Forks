using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Formatting;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	static class MathUtility
	{
		public static double Acosh(double value)
		{
			return Math.Log(value + Math.Sqrt(Math.Pow(value, 2.0) - 1.0));
		}

		public static double Asinh(double value)
		{
			return Math.Log(value + Math.Sqrt(Math.Pow(value, 2.0) + 1.0));
		}

		public static double Atanh(double value)
		{
			return 0.5 * Math.Log((1.0 + value) / (1.0 - value));
		}

		public static double Acoth(double value)
		{
			return 0.5 * Math.Log((value + 1.0) / (value - 1.0));
		}

		public static double Average(double[] values)
		{
			double num = 0.0;
			for (int i = 0; i < values.Length; i++)
			{
				num += values[i];
			}
			return num / (double)values.Length;
		}

		public static double RoundUp(double value, int decimals)
		{
			int num = Math.Sign(value);
			value *= (double)num;
			return (double)num * Math.Ceiling(value * Math.Pow(10.0, (double)decimals)) / Math.Pow(10.0, (double)decimals);
		}

		public static double RoundDown(double value, int decimals)
		{
			int num = Math.Sign(value);
			value *= (double)num;
			return (double)num * Math.Floor(value * Math.Pow(10.0, (double)decimals)) / Math.Pow(10.0, (double)decimals);
		}

		public static double Ceiling(double value, double significance)
		{
			if (significance == 0.0)
			{
				return 0.0;
			}
			double num = value / significance;
			double d = value + (Math.Ceiling(num) - num) * significance;
			return d.DoubleWithPrecision();
		}

		public static double CeilingPrecise(double value, double significance)
		{
			if (significance == 0.0)
			{
				return 0.0;
			}
			return MathUtility.Ceiling(value, Math.Abs(significance));
		}

		public static double Floor(double value, double significance)
		{
			if (significance == 0.0)
			{
				return 0.0;
			}
			double num = value / significance;
			double d = value - (num - Math.Floor(num)) * significance;
			return d.DoubleWithPrecision();
		}

		public static double FloorPrecise(double value, double significance)
		{
			if (significance == 0.0)
			{
				return 0.0;
			}
			return MathUtility.Floor(value, Math.Abs(significance));
		}

		public static double Mod(double number, double divisor)
		{
			double d = Math.Abs(number % divisor) * (double)Math.Sign(divisor);
			return d.DoubleWithPrecision();
		}

		public static double MRound(double value, double multiple)
		{
			bool flag = Math.Abs(MathUtility.Mod(value, multiple)) >= Math.Abs(multiple / 2.0);
			if (flag)
			{
				return MathUtility.Ceiling(value, multiple);
			}
			return MathUtility.Floor(value, multiple);
		}

		public static double Round(double value, int numDigits)
		{
			decimal num = (decimal)value;
			if (numDigits > 0)
			{
				return (double)Math.Round(num, numDigits, MidpointRounding.AwayFromZero);
			}
			decimal d = (decimal)Math.Pow(10.0, (double)numDigits);
			return (double)(Math.Round(num * d) / d);
		}

		public static double Factorial(double number)
		{
			double num = 1.0;
			int num2 = 2;
			while ((double)num2 <= number)
			{
				num *= (double)num2;
				num2++;
			}
			return num;
		}

		public static int GreatestCommonDivisor(double[] arguments)
		{
			return MathUtility.LcmGcdCommon(arguments, new Func<double, double, int>(MathUtility.GreatestCommonDivisor));
		}

		public static int LeastCommonMultiple(double[] arguments)
		{
			return MathUtility.LcmGcdCommon(arguments, new Func<double, double, int>(MathUtility.LeastCommonMultiple));
		}

		static int LcmGcdCommon(double[] arguments, Func<double, double, int> func)
		{
			List<double> list = arguments.ToList<double>();
			if (list.Contains(0.0))
			{
				return 0;
			}
			if (list.Count == 1)
			{
				return (int)list[0];
			}
			list.Sort();
			int num = func(list[0], list[1]);
			for (int i = 2; i < list.Count; i++)
			{
				num = func((double)num, list[i]);
			}
			return num;
		}

		static int GreatestCommonDivisor(double a, double b)
		{
			int result = 0;
			int i = (int)((a > b) ? a : b);
			int num = (int)((a > b) ? b : a);
			while (i > 0)
			{
				int num2 = i % num;
				if (num2 == 0)
				{
					result = num;
					break;
				}
				i = num;
				num = num2;
			}
			return result;
		}

		static int LeastCommonMultiple(double a, double b)
		{
			double num = (double)MathUtility.GreatestCommonDivisor(a, b);
			return (int)(Math.Abs(a * b) / num);
		}

		public static double StandardDeviation(double[] values, bool isEntirePopulation)
		{
			double num = MathUtility.Average(values);
			double num2 = 0.0;
			for (int i = 0; i < values.Length; i++)
			{
				double x = values[i] - num;
				num2 += Math.Pow(x, 2.0);
			}
			int num3 = (isEntirePopulation ? values.Length : (values.Length - 1));
			return Math.Sqrt(num2 / (double)num3);
		}

		public static bool GetTimeSerialNumber(double hours, double minutes, double seconds, out double timeSerialNumber)
		{
			bool result = false;
			timeSerialNumber = 0.0;
			int num = (int)hours;
			int num2 = (int)minutes;
			int num3 = (int)seconds;
			if (num >= 0 && num2 >= 0 && num3 >= 0 && num <= 32767 && num2 <= 32767 && num3 <= 32767)
			{
				DateTime dateTime = default(DateTime);
				try
				{
					bool flag = false;
					bool flag2 = false;
					bool flag3 = false;
					if (num > 0)
					{
						dateTime = dateTime.AddHours((double)num);
						flag = true;
					}
					if (num2 > 0)
					{
						dateTime = dateTime.AddMinutes((double)num2);
						flag2 = true;
					}
					if (num3 > 0)
					{
						dateTime = dateTime.AddSeconds((double)num3);
						flag3 = true;
					}
					if (!flag)
					{
						dateTime = dateTime.AddHours((double)num);
					}
					if (!flag2)
					{
						dateTime = dateTime.AddMinutes((double)num2);
					}
					if (!flag3)
					{
						dateTime = dateTime.AddSeconds((double)num3);
					}
					dateTime = new DateTime(1, 1, 1, dateTime.Hour, dateTime.Minute, dateTime.Second);
					timeSerialNumber = FormatHelper.ConvertDateTimeToDouble(dateTime);
					result = true;
				}
				catch (ArgumentOutOfRangeException)
				{
				}
			}
			return result;
		}

		public static bool GetDateSerialNumber(double year, double month, double day, out double dateSerianNumber)
		{
			bool result = false;
			dateSerianNumber = 0.0;
			if (year > 0.0 || month > 0.0 || day > 0.0)
			{
				int num = (int)year;
				int num2 = (int)month;
				int num3 = (int)day;
				if (num * 12 + num2 > 0)
				{
					num2--;
					num3 = ((num3 >= 0) ? (num3 - 1) : num3);
					if (num < 1900)
					{
						num += 1900;
					}
					num--;
					DateTime dateTime = default(DateTime);
					try
					{
						if (num > num2)
						{
							dateTime = dateTime.AddYears(num).AddMonths(num2);
						}
						else
						{
							dateTime = dateTime.AddMonths(num2).AddYears(num);
						}
						if (!(dateTime == FormatHelper.StartDate) || num3 >= 0)
						{
							dateTime = dateTime.AddDays((double)num3);
							dateSerianNumber = FormatHelper.ConvertDateTimeToDouble(dateTime);
							result = true;
						}
					}
					catch (ArgumentOutOfRangeException)
					{
					}
				}
			}
			return result;
		}

		public static double DoubleWithPrecision(this double d)
		{
			string s = d.ToString(CultureInfo.InvariantCulture);
			double result;
			double.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, out result);
			return result;
		}

		public static bool AreEqual(double a, double b)
		{
			double num = 1E-08;
			return Math.Abs(a - b) <= ((Math.Abs(a) > Math.Abs(b)) ? Math.Abs(b) : Math.Abs(a)) * num;
		}

		public static bool IsOddEgyptianFraction(double number)
		{
			bool result = false;
			if (number != 0.0 && number > -0.5 && number < 0.5)
			{
				int num = (int)Math.Round(1.0 / number);
				if (Math.Abs(num % 2) == 1)
				{
					result = MathUtility.AreEqual(number, 1.0 / (double)num);
				}
			}
			return result;
		}

		public static int IndexOfBiggestBit(long number)
		{
			int num = 63;
			while (num >= 0 && number.GetBitAtIndex(num) != 1L)
			{
				num--;
			}
			return num;
		}

		public static long GetBitAtIndex(this long number, int index)
		{
			return (number >> index) & 1L;
		}

		public static string GetImaginarySymbol(string complexNumberString)
		{
			string result = "i";
			if (string.IsNullOrEmpty(complexNumberString))
			{
				return result;
			}
			string text = complexNumberString.Trim();
			if (text.Length > 0 && text.Last<char>() == 'j')
			{
				result = "j";
			}
			return result;
		}

		public static bool TryGetImaginarySymbol(string[] complexNumberStrings, out string imaginarySymbol)
		{
			bool result = true;
			imaginarySymbol = "i";
			string text = null;
			for (int i = 0; i < complexNumberStrings.Length; i++)
			{
				if (!string.IsNullOrEmpty(complexNumberStrings[i]))
				{
					string text2 = complexNumberStrings[i].Trim();
					if (!string.IsNullOrEmpty(text2))
					{
						char c = text2.Last<char>();
						if (c == 'i' || c == 'j')
						{
							imaginarySymbol = c.ToString();
							if (string.IsNullOrEmpty(text))
							{
								text = imaginarySymbol;
							}
							else if (text != imaginarySymbol)
							{
								return false;
							}
						}
					}
				}
			}
			return result;
		}

		static readonly DateTime Mar1st1900 = new DateTime(1900, 3, 1);

		static readonly DateTime Jan1st1900 = new DateTime(1900, 1, 1);

		public static readonly double CotangentLimit = 134217728.0;

		public static readonly double CeilingFloorUpperLimit = 9.99E+307;

		public static readonly double CeilingFloorLowerLimit = -9.99E+307;
	}
}
