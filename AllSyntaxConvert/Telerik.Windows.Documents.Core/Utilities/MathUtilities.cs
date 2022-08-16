using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Telerik.Windows.Documents.Utilities
{
	class MathUtilities
	{
		public static Interval FindLocalMinimumAndMaximum(Interval interval, double a, double b, double c, double d)
		{
			return MathUtilities.FindLocalMinimumAndMaximum(MathUtilities.FindLocalExtrema(interval, a, b, c, d));
		}

		public static Interval FindLocalMinimumAndMaximum(Interval interval, double a, double b, double c)
		{
			return MathUtilities.FindLocalMinimumAndMaximum(MathUtilities.FindLocalExtrema(interval, a, b, c));
		}

		static Interval FindLocalMinimumAndMaximum(IEnumerable<Point> localExtrema)
		{
			double num = double.MaxValue;
			double num2 = double.MinValue;
			foreach (Point point in localExtrema)
			{
				if (point.Y < num)
				{
					num = point.Y;
				}
				if (point.Y > num2)
				{
					num2 = point.Y;
				}
			}
			return new Interval(num, num2);
		}

		public static IEnumerable<Point> FindLocalExtrema(Interval interval, double a, double b, double c)
		{
			List<double> pointsToCheck = new List<double>();
			pointsToCheck.Add(interval.Start);
			double zeroDerivative;
			if (MathUtilities.TrySolveLinear(2.0 * a, b, out zeroDerivative) && interval.Start < zeroDerivative && zeroDerivative < interval.End)
			{
				pointsToCheck.Add(zeroDerivative);
			}
			pointsToCheck.Add(interval.End);
			foreach (double num in pointsToCheck)
			{
				double x = num;
				Point localExtremum = new Point(x, a * x * x + b * x + c);
				yield return localExtremum;
			}
			yield break;
		}

		public static IEnumerable<Point> FindLocalExtrema(Interval interval, double a, double b, double c, double d)
		{
			List<double> pointsToCheck = new List<double>();
			pointsToCheck.Add(interval.Start);
			double[] zeroDerivatives;
			if (MathUtilities.TrySolveQuadratic(3.0 * a, 2.0 * b, c, out zeroDerivatives))
			{
				foreach (double num in zeroDerivatives)
				{
					if (interval.Start < num && num < interval.End)
					{
						pointsToCheck.Add(num);
					}
				}
			}
			pointsToCheck.Add(interval.End);
			foreach (double num2 in pointsToCheck)
			{
				double x = num2;
				Point localExtremum = new Point(x, a * x * x * x + b * x * x + c * x + d);
				yield return localExtremum;
			}
			yield break;
		}

		public static bool TrySolveQuadratic(double a, double b, double c, out double[] x)
		{
			x = (from solution in MathUtilities.SolveQuadratic(a, b, c)
				where !double.IsNaN(solution) && !double.IsInfinity(solution)
				select solution).ToArray<double>();
			return x.Length > 0;
		}

		public static IEnumerable<double> SolveQuadratic(double a, double b, double c)
		{
			if (a.IsZero(1E-08))
			{
				double linearSolve = MathUtilities.SolveLinear(b, c);
				if (!double.IsNaN(linearSolve))
				{
					yield return linearSolve;
				}
			}
			else
			{
				double d = b * b - 4.0 * a * c;
				if (d.IsZero(1E-08))
				{
					yield return -b / (2.0 * a);
				}
				else if (d > 0.0)
				{
					double sqrtD = Math.Sqrt(d);
					double x = (-b - sqrtD) / (2.0 * a);
					double x2 = (-b + sqrtD) / (2.0 * a);
					if (a > 0.0)
					{
						yield return x;
						yield return x2;
					}
					else
					{
						yield return x2;
						yield return x;
					}
				}
			}
			yield break;
		}

		public static bool TrySolveLinear(double a, double b, out double x)
		{
			x = MathUtilities.SolveLinear(a, b);
			return !double.IsNaN(x) && !double.IsInfinity(x);
		}

		public static double SolveLinear(double a, double b)
		{
			if (!a.IsZero(1E-08))
			{
				return -b / a;
			}
			if (!b.IsZero(1E-08))
			{
				return double.NaN;
			}
			return double.PositiveInfinity;
		}

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

		public static string IntegerToRomanString(int number)
		{
			if (number < 0)
			{
				throw new ArgumentException("Value must be positive.", "number");
			}
			if (number == 0)
			{
				return "n";
			}
			if (number > 32767)
			{
				number++;
			}
			number %= 32768;
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < 13; i++)
			{
				while (number >= MathUtilities.romanNumeralValues[i])
				{
					number -= MathUtilities.romanNumeralValues[i];
					stringBuilder.Append(MathUtilities.romanNumerals[i]);
				}
			}
			return stringBuilder.ToString();
		}

		public static double GetDistance(Point p1, Point p2)
		{
			return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
		}

		static readonly int[] romanNumeralValues = new int[]
		{
			1000, 900, 500, 400, 100, 90, 50, 40, 10, 9,
			5, 4, 1
		};

		static readonly string[] romanNumerals = new string[]
		{
			"m", "cm", "d", "cd", "c", "xc", "l", "xl", "x", "ix",
			"v", "iv", "i"
		};
	}
}
