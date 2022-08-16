using System;
using System.Globalization;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	static class Functions
	{
		public static double BinomialCoefficient(int n, int k)
		{
			if (k >= 0 && n >= 0 && k <= n)
			{
				return Math.Floor(0.5 + Math.Exp(Functions.FactorialLn(n) - Functions.FactorialLn(k) - Functions.FactorialLn(n - k)));
			}
			return 0.0;
		}

		public static double BetaRegularized(double a, double b, double x)
		{
			if (a < 0.0)
			{
				throw new ArgumentOutOfRangeException("a");
			}
			if (b < 0.0)
			{
				throw new ArgumentOutOfRangeException("b");
			}
			if (x < 0.0 || x > 1.0)
			{
				throw new ArgumentOutOfRangeException("x", "0d");
			}
			double num = ((Math.Abs(x) < 1E-08 || Math.Abs(x - 1.0) < 1E-08) ? 0.0 : Math.Exp(Functions.GammaLn(a + b) - Functions.GammaLn(a) - Functions.GammaLn(b) + a * Math.Log(x) + b * Math.Log(1.0 - x)));
			bool flag = x >= (a + 1.0) / (a + b + 2.0);
			double relativeAccuracy = Constants.RelativeAccuracy;
			double num2 = double.Epsilon / relativeAccuracy;
			if (flag)
			{
				x = 1.0 - x;
				double num3 = a;
				a = b;
				b = num3;
			}
			double num4 = a + b;
			double num5 = a + 1.0;
			double num6 = a - 1.0;
			double num7 = 1.0;
			double num8 = 1.0 - num4 * x / num5;
			if (Math.Abs(num8) < num2)
			{
				num8 = num2;
			}
			num8 = 1.0 / num8;
			double num9 = num8;
			int i = 1;
			int num10 = 2;
			while (i <= 100)
			{
				double num11 = (double)i * (b - (double)i) * x / ((num6 + (double)num10) * (a + (double)num10));
				num8 = 1.0 + num11 * num8;
				if (Math.Abs(num8) < num2)
				{
					num8 = num2;
				}
				num7 = 1.0 + num11 / num7;
				if (Math.Abs(num7) < num2)
				{
					num7 = num2;
				}
				num8 = 1.0 / num8;
				num9 *= num8 * num7;
				num11 = -(a + (double)i) * (num4 + (double)i) * x / ((a + (double)num10) * (num5 + (double)num10));
				num8 = 1.0 + num11 * num8;
				if (Math.Abs(num8) < num2)
				{
					num8 = num2;
				}
				num7 = 1.0 + num11 / num7;
				if (Math.Abs(num7) < num2)
				{
					num7 = num2;
				}
				num8 = 1.0 / num8;
				double num12 = num8 * num7;
				num9 *= num12;
				if (Math.Abs(num12 - 1.0) <= relativeAccuracy)
				{
					if (!flag)
					{
						return num * num9 / a;
					}
					return 1.0 - num * num9 / a;
				}
				else
				{
					i++;
					num10 += 2;
				}
			}
			throw new ArgumentException("a,b");
		}

		public static double BinomialCoefficientLn(int n, int k)
		{
			if (k < 0 || n < 0 || k > n)
			{
				return 1.0;
			}
			return Functions.FactorialLn(n) - Functions.FactorialLn(k) - Functions.FactorialLn(n - k);
		}

		public static double EpsilonOf(double value)
		{
			if (double.IsInfinity(value) || double.IsNaN(value))
			{
				return double.NaN;
			}
			long num = BitConverter.DoubleToInt64Bits(value);
			if (num == 0L)
			{
				num += 1L;
				return BitConverter.Int64BitsToDouble(num) - value;
			}
			long num2 = num;
			num = num2 - 1L;
			if (num2 < 0L)
			{
				return BitConverter.Int64BitsToDouble(num) - value;
			}
			return value - BitConverter.Int64BitsToDouble(num);
		}

		public static double Factorial(int n)
		{
			if (n < 0)
			{
				throw new ArgumentOutOfRangeException("n");
			}
			if (n == 0)
			{
				return 1.0;
			}
			if (n == 1)
			{
				return 1.0;
			}
			if (n < 100)
			{
				return Functions.FactorialPrecomp[n];
			}
			return Math.Exp(Functions.GammaLn((double)n + 1.0));
		}

		public static double FactorialLn(int value)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			if (value <= 1)
			{
				return 0.0;
			}
			if (value >= 200)
			{
				return Functions.GammaLn((double)value + 1.0);
			}
			if (Functions.factorialLnCache == null)
			{
				Functions.factorialLnCache = new double[200];
			}
			if (Math.Abs(Functions.factorialLnCache[value] - 0.0) <= 1E-08)
			{
				return Functions.factorialLnCache[value] = Functions.GammaLn((double)value + 1.0);
			}
			return Functions.factorialLnCache[value];
		}

		public static double GammaLn(double x)
		{
			double[] array = new double[]
			{
				57.15623566586292, -59.59796035547549, 14.136097974741746, -0.4919138160976202, 3.399464998481189E-05, 4.652362892704858E-05, -9.837447530487956E-05, 0.0001580887032249125, -0.00021026444172410488, 0.00021743961811521265,
				-0.0001643181065367639, 8.441822398385275E-05, -2.6190838401581408E-05, 3.6899182659531625E-06
			};
			double num = x;
			double num2 = 0.9999999999999971;
			double num3 = x + 5.2421875;
			num3 = (x + 0.5) * Math.Log(num3) - num3;
			for (int i = 0; i < 14; i++)
			{
				num2 += array[i] / (num += 1.0);
			}
			return num3 + Math.Log(2.5066282746310007 * num2 / x);
		}

		public static double Gamma(double x)
		{
			if (Math.Abs(x - 1.0) < 1E-08)
			{
				return 1.0;
			}
			int num;
			if (int.TryParse(x.ToString(CultureInfo.InvariantCulture), out num) && num > 0)
			{
				return Functions.Factorial(num - 1);
			}
			return Math.Exp(Functions.GammaLn(x));
		}

		public static double GammaRegularized(double a, double x)
		{
			double relativeAccuracy = Constants.RelativeAccuracy;
			double num = double.Epsilon / relativeAccuracy;
			if (a < 0.0 || x < 0.0)
			{
				throw new ArgumentOutOfRangeException("a");
			}
			double num2 = Functions.GammaLn(a);
			if (x < a + 1.0)
			{
				if (x <= 0.0)
				{
					return 0.0;
				}
				double num3 = a;
				double num5;
				double num4 = (num5 = 1.0 / a);
				for (int i = 0; i < 100; i++)
				{
					num3 += 1.0;
					num4 *= x / num3;
					num5 += num4;
					if (Math.Abs(num4) < Math.Abs(num5) * relativeAccuracy)
					{
						return num5 * Math.Exp(-x + a * Math.Log(x) - num2);
					}
				}
			}
			else
			{
				double num6 = x + 1.0 - a;
				double num7 = 1.0 / num;
				double num8 = 1.0 / num6;
				double num9 = num8;
				for (int j = 1; j <= 100; j++)
				{
					double num10 = (double)(-(double)j) * ((double)j - a);
					num6 += 2.0;
					num8 = num10 * num8 + num6;
					if (Math.Abs(num8) < num)
					{
						num8 = num;
					}
					num7 = num6 + num10 / num7;
					if (Math.Abs(num7) < num)
					{
						num7 = num;
					}
					num8 = 1.0 / num8;
					double num11 = num8 * num7;
					num9 *= num11;
					if (Math.Abs(num11 - 1.0) <= relativeAccuracy)
					{
						return 1.0 - Math.Exp(-x + a * Math.Log(x) - num2) * num9;
					}
				}
			}
			throw new ArgumentException("a");
		}

		public static double Squared(double value)
		{
			return value * value;
		}

		public static double Sqr(double value)
		{
			return value * value;
		}

		public static double Si(double x)
		{
			double num = 0.0;
			int num2 = 0;
			double num3;
			do
			{
				num3 = Math.Pow(-1.0, (double)num2) * Math.Pow(x, (double)(2 * num2 + 1)) / (double)(2 * num2 + 1) / Functions.Gamma((double)(2 * num2 + 2));
				num += num3;
				num2++;
			}
			while (Math.Abs(num3) > 1E-08);
			return num;
		}

		public static double Ci(double x)
		{
			double num = 0.0;
			int num2 = 1;
			double num3;
			do
			{
				num3 = Math.Pow(-1.0, (double)num2) * Math.Pow(x, (double)(2 * num2)) / (double)(2 * num2) / Functions.Gamma((double)(2 * num2 + 1));
				num += num3;
				num2++;
			}
			while (Math.Abs(num3) > 1E-08);
			return 0.5772156649015329 + Math.Log(x) + num;
		}

		public static double Erf(double x)
		{
			if (x < 0.0)
			{
				return Functions.ErfcCheb(-x) - 1.0;
			}
			return 1.0 - Functions.ErfcCheb(x);
		}

		public static double ErfC(double x)
		{
			if (x < 0.0)
			{
				return 2.0 - Functions.ErfcCheb(-x);
			}
			return Functions.ErfcCheb(x);
		}

		static double ErfcCheb(double x)
		{
			double num = 0.0;
			double num2 = 0.0;
			if (x < 0.0)
			{
				throw new Exception("ErfcCheb requires nonnegative argument");
			}
			double[] array = new double[]
			{
				-1.3026537197817094, 0.6419697923564902, 0.019476473204185836, -0.00956151478680863, -0.000946595344482036, 0.000366839497852761, 4.2523324806907E-05, -2.0278578112534E-05, -1.624290004647E-06, 1.30365583558E-06,
				1.5626441722E-08, -8.5238095915E-08, 6.529054439E-09, 5.059343495E-09, -9.91364156E-10, -2.27365122E-10, 9.6467911E-11, 2.394038E-12, -6.886027E-12, 8.94487E-13,
				3.13092E-13, -1.12708E-13, 3.81E-16, 7.106E-15, -1.523E-15, -9.4E-17, 1.21E-16, -2.8E-17
			};
			double num3 = 2.0 / (2.0 + x);
			double num4 = 4.0 * num3 - 2.0;
			for (int i = array.Length - 1; i > 0; i--)
			{
				double num5 = num;
				num = num4 * num - num2 + array[i];
				num2 = num5;
			}
			return num3 * Math.Exp(-x * x + 0.5 * (array[0] + num4 * num) - num2);
		}

		public static double Laguerre(double x, int order)
		{
			double num = 1.0;
			double num2 = 1.0 - x;
			double num3 = (x * x - 4.0 * x + 2.0) / 2.0;
			int i = 1;
			if (order < 0)
			{
				throw new Exception("Bad Laguerre polynomial: deg < 0");
			}
			if (order == 0)
			{
				return num;
			}
			if (order == 1)
			{
				return num2;
			}
			while (i < order)
			{
				num3 = ((2.0 * (double)i + 1.0 - x) * num2 - (double)i * num) / (double)(i + 1);
				num = num2;
				num2 = num3;
				i++;
			}
			return num3;
		}

		public static double Hermite(double x, int order)
		{
			double num = 1.0;
			double num2 = 2.0 * x;
			double num3 = 4.0 * x * x - 2.0;
			int i = 1;
			if (order < 0)
			{
				throw new Exception("The order of the Hermite polynomial cannot be less than zero.");
			}
			if (order == 0)
			{
				return num;
			}
			if (order == 1)
			{
				return num2;
			}
			if (order == 2)
			{
				return num3;
			}
			while (i < order)
			{
				num3 = 2.0 * (x * num2 - (double)i * num);
				num = num2;
				num2 = num3;
				i++;
			}
			return num3;
		}

		public static double Legendre(double x, int order)
		{
			double num = 1.0;
			double num2 = x;
			double num3 = (3.0 * x * x - 1.0) / 2.0;
			int i = 1;
			if (order < 0)
			{
				throw new Exception("The order of the Legendre polynomial cannot be less than zero.");
			}
			if (order == 0)
			{
				return num;
			}
			if (order == 1)
			{
				return num2;
			}
			if (order == 2)
			{
				return num3;
			}
			while (i < order)
			{
				num3 = ((2.0 * (double)i + 1.0) * x * num2 - (double)i * num) / (double)(i + 1);
				num = num2;
				num2 = num3;
				i++;
			}
			return num3;
		}

		public static bool IsNanOrInfinity(this double value)
		{
			return double.IsNaN(value) || double.IsInfinity(value);
		}

		public static bool IsFinitedouble(double x)
		{
			return !double.IsInfinity(x) && !double.IsNaN(x);
		}

		static long GCD(long value1, long value2)
		{
			long num;
			do
			{
				num = value1 % value2;
				value1 = value2;
				value2 = num;
			}
			while (num != 0L);
			return value1;
		}

		public static long GCD(params long[] numbers)
		{
			if (numbers == null)
			{
				throw new ArgumentNullException("numbers");
			}
			if (numbers.Length == 1)
			{
				return numbers[0];
			}
			long num = Functions.GCD(numbers[0], numbers[1]);
			if (numbers.Length > 2)
			{
				for (int i = 2; i < numbers.Length; i++)
				{
					num = Functions.GCD(num, numbers[i]);
				}
			}
			return num;
		}

		public static double BesselJ(double x, double order)
		{
			if (order % 1.0 != 0.0)
			{
				double result = 0.0;
				double num = 0.0;
				double num2 = 0.0;
				double num3 = 0.0;
				Functions.bessjy(x, order, out result, out num, out num2, out num3);
				return result;
			}
			int num4 = (int)order;
			if (num4 == 0)
			{
				return Functions.BesselJ0(x);
			}
			if (num4 == 1)
			{
				return Functions.BesselJ1(x);
			}
			double num5 = Math.Abs(x);
			if (Math.Abs(num5) < 1E-08)
			{
				return 0.0;
			}
			double num10;
			if (num5 > (double)num4)
			{
				double num6 = 2.0 / num5;
				double num7 = Functions.BesselJ0(num5);
				double num8 = Functions.BesselJ1(num5);
				for (int i = 1; i < num4; i++)
				{
					double num9 = (double)i * num6 * num8 - num7;
					num7 = num8;
					num8 = num9;
				}
				num10 = num8;
			}
			else
			{
				double num6 = 2.0 / num5;
				int num11 = 2 * ((num4 + (int)Math.Sqrt(40.0 * (double)num4)) / 2);
				int num12 = 0;
				double num9;
				double num13;
				num10 = (num9 = (num13 = 0.0));
				double num8 = 1.0;
				for (int i = num11; i > 0; i--)
				{
					double num7 = (double)i * num6 * num8 - num9;
					num9 = num8;
					num8 = num7;
					if (Math.Abs(num8) > 10000000000.0)
					{
						num8 *= 1E-10;
						num9 *= 1E-10;
						num10 *= 1E-10;
						num13 *= 1E-10;
					}
					if (num12 == 1)
					{
						num13 += num8;
					}
					num12 = ((num12 == 0) ? 1 : 0);
					if (i == num4)
					{
						num10 = num9;
					}
				}
				num13 = 2.0 * num13 - num8;
				num10 /= num13;
			}
			if (x >= 0.0 || (num4 & 1) != 1)
			{
				return num10;
			}
			return -num10;
		}

		public static double BesselY1(double x)
		{
			double result;
			if (x < 8.0)
			{
				double num = x * x;
				double num2 = x * (-4900604943000.0 + num * (1275274390000.0 + num * (-51534381390.0 + num * (734926455.1 + num * (-4237922.726 + num * 8511.937935)))));
				double num3 = 24995805700000.0 + num * (424441966400.0 + num * (3733650367.0 + num * (22459040.02 + num * (102042.605 + num * (354.9632885 + num)))));
				result = num2 / num3 + 0.636619772 * (Functions.BesselJ1(x) * Math.Log(x) - 1.0 / x);
			}
			else
			{
				double num4 = 8.0 / x;
				double num = num4 * num4;
				double num5 = x - 2.356194491;
				double num2 = 1.0 + num * (0.00183105 + num * (-3.516396496E-05 + num * (2.457520174E-06 + num * -2.40337019E-07)));
				double num3 = 0.04687499995 + num * (-0.0002002690873 + num * (8.449199096E-06 + num * (-8.8228987E-07 + num * 1.05787412E-07)));
				result = Math.Sqrt(0.636619772 / x) * (Math.Sin(num5) * num2 + num4 * Math.Cos(num5) * num3);
			}
			return result;
		}

		public static double BesselY0(double x)
		{
			double result;
			if (x < 8.0)
			{
				double num = x * x;
				double num2 = -2957821389.0 + num * (7062834065.0 + num * (-512359803.6 + num * (10879881.29 + num * (-86327.92757 + num * 228.4622733))));
				double num3 = 40076544269.0 + num * (745249964.8 + num * (7189466.438 + num * (47447.2647 + num * (226.1030244 + num * 1.0))));
				result = num2 / num3 + 0.636619772 * Functions.BesselJ0(x) * Math.Log(x);
			}
			else
			{
				double num4 = 8.0 / x;
				double num = num4 * num4;
				double num5 = x - 0.785398164;
				double num2 = 1.0 + num * (-0.001098628627 + num * (2.734510407E-05 + num * (-2.073370639E-06 + num * 2.093887211E-07)));
				double num3 = -0.01562499995 + num * (0.0001430488765 + num * (-6.911147651E-06 + num * (7.621095161E-07 + num * -9.34945152E-08)));
				result = Math.Sqrt(0.636619772 / x) * (Math.Sin(num5) * num2 + num4 * Math.Cos(num5) * num3);
			}
			return result;
		}

		public static double BesselJ1(double x)
		{
			double num;
			double num5;
			if ((num = Math.Abs(x)) < 8.0)
			{
				double num2 = x * x;
				double num3 = x * (72362614232.0 + num2 * (-7895059235.0 + num2 * (242396853.1 + num2 * (-2972611.439 + num2 * (15704.4826 + num2 * -30.16036606)))));
				double num4 = 144725228442.0 + num2 * (2300535178.0 + num2 * (18583304.74 + num2 * (99447.43394 + num2 * (376.9991397 + num2 * 1.0))));
				num5 = num3 / num4;
			}
			else
			{
				double num6 = 8.0 / num;
				double num2 = num6 * num6;
				double num7 = num - 2.356194491;
				double num3 = 1.0 + num2 * (0.00183105 + num2 * (-3.516396496E-05 + num2 * (2.457520174E-06 + num2 * -2.40337019E-07)));
				double num4 = 0.04687499995 + num2 * (-0.0002002690873 + num2 * (8.449199096E-06 + num2 * (-8.8228987E-07 + num2 * 1.05787412E-07)));
				num5 = Math.Sqrt(0.636619772 / num) * (Math.Cos(num7) * num3 - num6 * Math.Sin(num7) * num4);
				if (x < 0.0)
				{
					num5 = -num5;
				}
			}
			return num5;
		}

		public static double BesselJ0(double x)
		{
			double num;
			double result;
			if ((num = Math.Abs(x)) < 8.0)
			{
				double num2 = x * x;
				double num3 = 57568490574.0 + num2 * (-13362590354.0 + num2 * (651619640.7 + num2 * (-11214424.18 + num2 * (77392.33017 + num2 * -184.9052456))));
				double num4 = 57568490411.0 + num2 * (1029532985.0 + num2 * (9494680.718 + num2 * (59272.64853 + num2 * (267.8532712 + num2 * 1.0))));
				result = num3 / num4;
			}
			else
			{
				double num5 = 8.0 / num;
				double num2 = num5 * num5;
				double num6 = num - 0.785398164;
				double num3 = 1.0 + num2 * (-0.001098628627 + num2 * (2.734510407E-05 + num2 * (-2.073370639E-06 + num2 * 2.093887211E-07)));
				double num4 = -0.01562499995 + num2 * (0.0001430488765 + num2 * (-6.911147651E-06 + num2 * (7.621095161E-07 - num2 * 9.34945152E-08)));
				result = Math.Sqrt(0.636619772 / num) * (Math.Cos(num6) * num3 - num5 * Math.Sin(num6) * num4);
			}
			return result;
		}

		public static double BesselI(double x, double order)
		{
			if (order % 1.0 != 0.0)
			{
				double result;
				double num;
				double num2;
				double num3;
				Functions.bessik(x, order, out result, out num, out num2, out num3);
				return result;
			}
			int num4 = (int)order;
			if (num4 == 0)
			{
				return Functions.BesselI0(x);
			}
			if (num4 == 1)
			{
				return Functions.BesselI1(x);
			}
			if (Math.Abs(x) < 1E-08)
			{
				return 0.0;
			}
			double num5 = 2.0 / Math.Abs(x);
			double num7;
			double num6 = (num7 = 0.0);
			double num8 = 1.0;
			for (int i = 2 * (num4 + (int)Math.Sqrt(40.0 * (double)num4)); i > 0; i--)
			{
				double num9 = num7 + (double)i * num5 * num8;
				num7 = num8;
				num8 = num9;
				if (Math.Abs(num8) > 10000000000.0)
				{
					num6 *= 1E-10;
					num8 *= 1E-10;
					num7 *= 1E-10;
				}
				if (i == num4)
				{
					num6 = num7;
				}
			}
			num6 *= Functions.BesselI0(x) / num8;
			if (x >= 0.0 || (num4 & 1) != 1)
			{
				return num6;
			}
			return -num6;
		}

		static double BesselK0(double x)
		{
			double result;
			if (x <= 2.0)
			{
				double num = x * x / 4.0;
				result = -Math.Log(x / 2.0) * Functions.BesselI0(x) + (-0.57721566 + num * (0.4227842 + num * (0.23069756 + num * (0.0348859 + num * (0.00262698 + num * (0.0001075 + num * 7.4E-06))))));
			}
			else
			{
				double num = 2.0 / x;
				result = Math.Exp(-x) / Math.Sqrt(x) * (1.25331414 + num * (-0.07832358 + num * (0.02189568 + num * (-0.01062446 + num * (0.00587872 + num * (-0.0025154 + num * 0.00053208))))));
			}
			return result;
		}

		static double BesselK1(double x)
		{
			double result;
			if (x <= 2.0)
			{
				double num = x * x / 4.0;
				result = Math.Log(x / 2.0) * Functions.BesselI1(x) + 1.0 / x * (1.0 + num * (0.15443144 + num * (-0.67278579 + num * (-0.18156897 + num * (-0.01919402 + num * (-0.00110404 + num * -4.686E-05))))));
			}
			else
			{
				double num = 2.0 / x;
				result = Math.Exp(-x) / Math.Sqrt(x) * (1.25331414 + num * (0.23498619 + num * (-0.0365562 + num * (0.01504268 + num * (-0.00780353 + num * (0.00325614 + num * -0.00068245))))));
			}
			return result;
		}

		static double BesselI1(double x)
		{
			double num;
			double num3;
			if ((num = Math.Abs(x)) < 3.75)
			{
				double num2 = x / 3.75;
				num2 *= num2;
				num3 = num * (0.5 + num2 * (0.87890594 + num2 * (0.51498869 + num2 * (0.15084934 + num2 * (0.02658733 + num2 * (0.00301532 + num2 * 0.00032411))))));
			}
			else
			{
				double num2 = 3.75 / num;
				num3 = 0.02282967 + num2 * (-0.02895312 + num2 * (0.01787654 - num2 * 0.00420059));
				num3 = 0.39894228 + num2 * (-0.03988024 + num2 * (-0.00362018 + num2 * (0.00163801 + num2 * (-0.01031555 + num2 * num3))));
				num3 *= Math.Exp(num) / Math.Sqrt(num);
			}
			if (x >= 0.0)
			{
				return num3;
			}
			return -num3;
		}

		static double BesselI0(double x)
		{
			double num;
			double result;
			if ((num = Math.Abs(x)) < 3.75)
			{
				double num2 = x / 3.75;
				num2 *= num2;
				result = 1.0 + num2 * (3.5156229 + num2 * (3.0899424 + num2 * (1.2067492 + num2 * (0.2659732 + num2 * (0.0360768 + num2 * 0.0045813)))));
			}
			else
			{
				double num2 = 3.75 / num;
				result = Math.Exp(num) / Math.Sqrt(num) * (0.39894228 + num2 * (0.01328592 + num2 * (0.00225319 + num2 * (-0.00157565 + num2 * (0.00916281 + num2 * (-0.02057706 + num2 * (0.02635537 + num2 * (-0.01647633 + num2 * 0.00392377))))))));
			}
			return result;
		}

		public static double BesselK(double x, double order)
		{
			if (order % 1.0 != 0.0)
			{
				double num;
				double result;
				double num2;
				double num3;
				Functions.bessik(x, order, out num, out result, out num2, out num3);
				return result;
			}
			int num4 = (int)order;
			if (num4 == 0)
			{
				return Functions.BesselK0(x);
			}
			if (num4 == 1)
			{
				return Functions.BesselK1(x);
			}
			double num5 = 2.0 / x;
			double num6 = Functions.BesselK0(x);
			double num7 = Functions.BesselK1(x);
			for (int i = 1; i < num4; i++)
			{
				double num8 = num6 + (double)i * num5 * num7;
				num6 = num7;
				num7 = num8;
			}
			return num7;
		}

		public static double BesselY(double x, double order)
		{
			if (order % 1.0 != 0.0)
			{
				double num;
				double result;
				double num2;
				double num3;
				Functions.bessjy(x, order, out num, out result, out num2, out num3);
				return result;
			}
			int num4 = (int)order;
			if (num4 == 0)
			{
				return Functions.BesselY0(x);
			}
			if (num4 == 1)
			{
				return Functions.BesselY1(x);
			}
			double num5 = 2.0 / x;
			double num6 = Functions.BesselY1(x);
			double num7 = Functions.BesselY0(x);
			for (int i = 1; i < num4; i++)
			{
				double num8 = (double)i * num5 * num6 - num7;
				num7 = num6;
				num6 = num8;
			}
			return num6;
		}

		public static double SphericalBesselJ(double x, int n)
		{
			double result;
			double num;
			double num2;
			double num3;
			Functions.sphbes(n, x, out result, out num, out num2, out num3);
			return result;
		}

		public static double SphericalBesselY(double x, int n)
		{
			double num;
			double result;
			double num2;
			double num3;
			Functions.sphbes(n, x, out num, out result, out num2, out num3);
			return result;
		}

		static double SIGN(double a, double b)
		{
			if (b < 0.0)
			{
				return -Math.Abs(a);
			}
			return Math.Abs(a);
		}

		static double chebev(double a, double b, double[] c, int m, double x)
		{
			double num = 0.0;
			double num2 = 0.0;
			if ((x - a) * (x - b) > 0.0)
			{
				throw new Exception("x not in range in routine chebev");
			}
			double num4;
			double num3 = 2.0 * (num4 = (2.0 * x - a - b) / (b - a));
			for (int i = m - 1; i >= 1; i--)
			{
				double num5 = num;
				num = num3 * num - num2 + c[i];
				num2 = num5;
			}
			return num4 * num - num2 + 0.5 * c[0];
		}

		static void beschb(double x, out double gam1, out double gam2, out double gampl, out double gammi)
		{
			double[] c = new double[] { -1.142022680371172, 0.006516511267076, 0.000308709017308, -3.470626964E-06, 6.943764E-09, 3.678E-11, -1.36E-13 };
			double[] c2 = new double[] { 1.843740587300906, -0.076852840844786, 0.001271927136655, -4.971736704E-06, -3.312612E-08, 2.4231E-10, -1.7E-13, -1E-15 };
			double x2 = 8.0 * x * x - 1.0;
			gam1 = Functions.chebev(-1.0, 1.0, c, 5, x2);
			gam2 = Functions.chebev(-1.0, 1.0, c2, 5, x2);
			gampl = gam2 - x * gam1;
			gammi = gam2 + x * gam1;
		}

		static void bessjy(double x, double xnu, out double rj, out double ry, out double rjp, out double ryp)
		{
			if (x <= 0.0 || xnu < 0.0)
			{
				throw new Exception("Value and order should be positive.");
			}
			int num = ((x < 2.0) ? ((int)(xnu + 0.5)) : Math.Max(0, (int)(xnu - x + 1.5)));
			double num2 = xnu - (double)num;
			double num3 = num2 * num2;
			double num4 = 1.0 / x;
			double num5 = 2.0 * num4;
			double num6 = num5 / 3.141592653589793;
			int num7 = 1;
			double num8 = xnu * num4;
			if (num8 < 1E-30)
			{
				num8 = 1E-30;
			}
			double num9 = num5 * xnu;
			double num10 = 0.0;
			double num11 = num8;
			int i;
			for (i = 1; i <= 10000; i++)
			{
				num9 += num5;
				num10 = num9 - num10;
				if (Math.Abs(num10) < 1E-30)
				{
					num10 = 1E-30;
				}
				num11 = num9 - 1.0 / num11;
				if (Math.Abs(num11) < 1E-30)
				{
					num11 = 1E-30;
				}
				num10 = 1.0 / num10;
				double num12 = num11 * num10;
				num8 = num12 * num8;
				if (num10 < 0.0)
				{
					num7 = -num7;
				}
				if (Math.Abs(num12 - 1.0) < 1E-10)
				{
					break;
				}
			}
			if (i > 10000)
			{
				throw new Exception("x too large in bessjy; try asymptotic expansion");
			}
			double num13 = (double)num7 * 1E-30;
			double num14 = num8 * num13;
			double num15 = num13;
			double num16 = num14;
			double num17 = xnu * num4;
			for (int j = num; j >= 1; j--)
			{
				double num18 = num17 * num13 + num14;
				num17 -= num4;
				num14 = num17 * num18 - num13;
				num13 = num18;
			}
			if (num13 == 0.0)
			{
				num13 = 1E-10;
			}
			double num19 = num14 / num13;
			double num37;
			double num38;
			double num40;
			if (x < 2.0)
			{
				double num20 = 0.5 * x;
				double num21 = 3.141592653589793 * num2;
				num17 = ((Math.Abs(num21) < 1E-10) ? 1.0 : (num21 / Math.Sin(num21)));
				num10 = -Math.Log(num20);
				double num22 = num2 * num10;
				double num23 = ((Math.Abs(num22) < 1E-10) ? 1.0 : (Math.Sinh(num22) / num22));
				double num24;
				double num25;
				double num26;
				double num27;
				Functions.beschb(num2, out num24, out num25, out num26, out num27);
				double num28 = 0.6366197723675814 * num17 * (num24 * Math.Cosh(num22) + num25 * num23 * num10);
				num22 = Math.Exp(num22);
				double num29 = num22 / (num26 * 3.141592653589793);
				double num30 = 1.0 / (num22 * 3.141592653589793 * num27);
				double num31 = 0.5 * num21;
				double num32 = ((Math.Abs(num31) < 1E-10) ? 1.0 : (Math.Sin(num31) / num31));
				double num33 = 3.141592653589793 * num31 * num32 * num32;
				num11 = 1.0;
				num10 = -num20 * num20;
				double num34 = num28 + num33 * num30;
				double num35 = num29;
				for (i = 1; i <= 10000; i++)
				{
					num28 = ((double)i * num28 + num29 + num30) / ((double)(i * i) - num3);
					num11 *= num10 / (double)i;
					num29 /= (double)i - num2;
					num30 /= (double)i + num2;
					double num12 = num11 * (num28 + num33 * num30);
					num34 += num12;
					double num36 = num11 * num29 - (double)i * num12;
					num35 += num36;
					if (Math.Abs(num12) < (1.0 + Math.Abs(num34)) * 1E-10)
					{
						break;
					}
				}
				if (i > 10000)
				{
					throw new Exception("bessy series failed to converge");
				}
				num37 = -num34;
				num38 = -num35 * num5;
				double num39 = num2 * num4 * num37 - num38;
				num40 = num6 / (num39 - num19 * num37);
			}
			else
			{
				double num41 = 0.25 - num3;
				double num29 = -0.5 * num4;
				double num30 = 1.0;
				double num42 = 2.0 * x;
				double num43 = 2.0;
				num17 = num41 * num4 / (num29 * num29 + num30 * num30);
				double num44 = num42 + num30 * num17;
				double num45 = num43 + num29 * num17;
				double num46 = num42 * num42 + num43 * num43;
				double num47 = num42 / num46;
				double num48 = -num43 / num46;
				double num49 = num44 * num47 - num45 * num48;
				double num50 = num44 * num48 + num45 * num47;
				double num51 = num29 * num49 - num30 * num50;
				num30 = num29 * num50 + num30 * num49;
				num29 = num51;
				for (i = 2; i <= 10000; i++)
				{
					num41 += (double)(2 * (i - 1));
					num43 += 2.0;
					num47 = num41 * num47 + num42;
					num48 = num41 * num48 + num43;
					if (Math.Abs(num47) + Math.Abs(num48) < 1E-30)
					{
						num47 = 1E-30;
					}
					num17 = num41 / (num44 * num44 + num45 * num45);
					num44 = num42 + num44 * num17;
					num45 = num43 - num45 * num17;
					if (Math.Abs(num44) + Math.Abs(num45) < 1E-30)
					{
						num44 = 1E-30;
					}
					num46 = num47 * num47 + num48 * num48;
					num47 /= num46;
					num48 /= -num46;
					num49 = num44 * num47 - num45 * num48;
					num50 = num44 * num48 + num45 * num47;
					num51 = num29 * num49 - num30 * num50;
					num30 = num29 * num50 + num30 * num49;
					num29 = num51;
					if (Math.Abs(num49 - 1.0) + Math.Abs(num50) < 1E-10)
					{
						break;
					}
				}
				if (i > 10000)
				{
					throw new Exception("cf2 failed in bessjy");
				}
				double num52 = (num29 - num19) / num30;
				num40 = Math.Sqrt(num6 / ((num29 - num19) * num52 + num30));
				num40 = Functions.SIGN(num40, num13);
				num37 = num40 * num52;
				double num39 = num37 * (num29 + num30 / num52);
				num38 = num2 * num4 * num37 - num39;
			}
			num17 = num40 / num13;
			rj = num15 * num17;
			rjp = num16 * num17;
			for (i = 1; i <= num; i++)
			{
				double num53 = (num2 + (double)i) * num5 * num38 - num37;
				num37 = num38;
				num38 = num53;
			}
			ry = num37;
			ryp = xnu * num4 * num37 - num38;
		}

		static void bessik(double x, double xnu, out double ri, out double rk, out double rip, out double rkp)
		{
			if (x <= 0.0 || xnu < 0.0)
			{
				throw new Exception("bad arguments in bessik");
			}
			int num = (int)(xnu + 0.5);
			double num2 = xnu - (double)num;
			double num3 = num2 * num2;
			double num4 = 1.0 / x;
			double num5 = 2.0 * num4;
			double num6 = xnu * num4;
			if (num6 < 1E-30)
			{
				num6 = 1E-30;
			}
			double num7 = num5 * xnu;
			double num8 = 0.0;
			double num9 = num6;
			int i;
			for (i = 1; i <= 10000; i++)
			{
				num7 += num5;
				num8 = 1.0 / (num7 + num8);
				num9 = num7 + 1.0 / num9;
				double num10 = num9 * num8;
				num6 = num10 * num6;
				if (Math.Abs(num10 - 1.0) < 1E-10)
				{
					break;
				}
			}
			if (i > 10000)
			{
				throw new Exception("x too large in bessik; try asymptotic expansion");
			}
			double num11 = 1E-30;
			double num12 = num6 * num11;
			double num13 = num11;
			double num14 = num12;
			double num15 = xnu * num4;
			for (int j = num; j >= 1; j--)
			{
				double num16 = num15 * num11 + num12;
				num15 -= num4;
				num12 = num15 * num16 + num11;
				num11 = num16;
			}
			double num17 = num12 / num11;
			double num32;
			double num33;
			if (x < 2.0)
			{
				double num18 = 0.5 * x;
				double num19 = 3.141592653589793 * num2;
				num15 = ((Math.Abs(num19) < 1E-10) ? 1.0 : (num19 / Math.Sin(num19)));
				num8 = -Math.Log(num18);
				double num20 = num2 * num8;
				double num21 = ((Math.Abs(num20) < 1E-10) ? 1.0 : (Math.Sinh(num20) / num20));
				double num22;
				double num23;
				double num24;
				double num25;
				Functions.beschb(num2, out num22, out num23, out num24, out num25);
				double num26 = num15 * (num22 * Math.Cosh(num20) + num23 * num21 * num8);
				double num27 = num26;
				num20 = Math.Exp(num20);
				double num28 = 0.5 * num20 / num24;
				double num29 = 0.5 / (num20 * num25);
				num9 = 1.0;
				num8 = num18 * num18;
				double num30 = num28;
				for (i = 1; i <= 10000; i++)
				{
					num26 = ((double)i * num26 + num28 + num29) / ((double)(i * i) - num3);
					num9 *= num8 / (double)i;
					num28 /= (double)i - num2;
					num29 /= (double)i + num2;
					double num10 = num9 * num26;
					num27 += num10;
					double num31 = num9 * (num28 - (double)i * num26);
					num30 += num31;
					if (Math.Abs(num10) < Math.Abs(num27) * 1E-10)
					{
						break;
					}
				}
				if (i > 10000)
				{
					throw new Exception("bessk series failed to converge");
				}
				num32 = num27;
				num33 = num30 * num5;
			}
			else
			{
				num7 = 2.0 * (1.0 + x);
				num8 = 1.0 / num7;
				double num34 = (num6 = num8);
				double num35 = 0.0;
				double num36 = 1.0;
				double num37 = 0.25 - num3;
				double num29;
				num9 = (num29 = num37);
				double num38 = -num37;
				double num39 = 1.0 + num29 * num34;
				for (i = 2; i <= 10000; i++)
				{
					num38 -= (double)(2 * (i - 1));
					num9 = -num38 * num9 / (double)i;
					double num40 = (num35 - num7 * num36) / num38;
					num35 = num36;
					num36 = num40;
					num29 += num9 * num40;
					num7 += 2.0;
					num8 = 1.0 / (num7 + num38 * num8);
					num34 = (num7 * num8 - 1.0) * num34;
					num6 += num34;
					double num41 = num29 * num34;
					num39 += num41;
					if (Math.Abs(num41 / num39) < 1E-10)
					{
						break;
					}
				}
				if (i > 10000)
				{
					throw new Exception("bessik: failure to converge in cf2");
				}
				num6 = num37 * num6;
				num32 = Math.Sqrt(3.141592653589793 / (2.0 * x)) * Math.Exp(-x) / num39;
				num33 = num32 * (num2 + x + 0.5 - num6) * num4;
			}
			double num42 = num2 * num4 * num32 - num33;
			double num43 = num4 / (num17 * num32 - num42);
			ri = num43 * num13 / num11;
			rip = num43 * num14 / num11;
			for (i = 1; i <= num; i++)
			{
				double num44 = (num2 + (double)i) * num5 * num33 + num32;
				num32 = num33;
				num33 = num44;
			}
			rk = num32;
			rkp = xnu * num4 * num32 - num33;
		}

		static void sphbes(int n, double x, out double sj, out double sy, out double sjp, out double syp)
		{
			if (n < 0 || x <= 0.0)
			{
				throw new Exception("bad arguments in sphbes");
			}
			double xnu = (double)n + 0.5;
			double num;
			double num2;
			double num3;
			double num4;
			Functions.bessjy(x, xnu, out num, out num2, out num3, out num4);
			double num5 = 1.2533141 / Math.Sqrt(x);
			sj = num5 * num;
			sy = num5 * num2;
			sjp = num5 * num3 - sj / (2.0 * x);
			syp = num5 * num4 - sy / (2.0 * x);
		}

		const int FactorialLnCacheSize = 200;

		const int FactorialPrecompSize = 100;

		static readonly double[] FactorialPrecomp = new double[]
		{
			1.0, 1.0, 2.0, 6.0, 24.0, 120.0, 720.0, 5040.0, 40320.0, 362880.0,
			3628800.0, 39916800.0, 479001600.0, 6227020800.0, 87178291200.0, 1307674368000.0, 20922789888000.0, 355687428096000.0, 6402373705728000.0, 1.21645100408832E+17,
			2.43290200817664E+18, 5.109094217170944E+19, 1.1240007277776077E+21, 2.585201673888498E+22, 6.204484017332394E+23, 1.5511210043330986E+25, 4.0329146112660565E+26, 1.0888869450418352E+28, 3.0488834461171387E+29, 8.841761993739702E+30,
			2.6525285981219107E+32, 8.222838654177922E+33, 2.631308369336935E+35, 8.683317618811886E+36, 2.9523279903960416E+38, 1.0333147966386145E+40, 3.7199332678990125E+41, 1.3763753091226346E+43, 5.230226174666011E+44, 2.0397882081197444E+46,
			8.159152832478977E+47, 3.345252661316381E+49, 1.40500611775288E+51, 6.041526306337383E+52, 2.658271574788449E+54, 1.1962222086548019E+56, 5.502622159812089E+57, 2.5862324151116818E+59, 1.2413915592536073E+61, 6.082818640342675E+62,
			3.0414093201713376E+64, 1.5511187532873822E+66, 8.065817517094388E+67, 4.2748832840600255E+69, 2.308436973392414E+71, 1.2696403353658276E+73, 7.109985878048635E+74, 4.0526919504877214E+76, 2.3505613312828785E+78, 1.3868311854568984E+80,
			8.32098711274139E+81, 5.075802138772248E+83, 3.146997326038794E+85, 1.98260831540444E+87, 1.2688693218588417E+89, 8.247650592082472E+90, 5.443449390774431E+92, 3.647111091818868E+94, 2.4800355424368305E+96, 1.711224524281413E+98,
			1.1978571669969892E+100, 8.504785885678623E+101, 6.1234458376886085E+103, 4.4701154615126844E+105, 3.307885441519386E+107, 2.48091408113954E+109, 1.8854947016660504E+111, 1.4518309202828587E+113, 1.1324281178206297E+115, 8.946182130782976E+116,
			7.156945704626381E+118, 5.797126020747368E+120, 4.753643337012842E+122, 3.945523969720659E+124, 3.314240134565353E+126, 2.81710411438055E+128, 2.4227095383672734E+130, 2.107757298379528E+132, 1.8548264225739844E+134, 1.650795516090846E+136,
			1.4857159644817615E+138, 1.352001527678403E+140, 1.2438414054641308E+142, 1.1567725070816416E+144, 1.087366156656743E+146, 1.032997848823906E+148, 9.916779348709496E+149, 9.619275968248212E+151, 9.426890448883248E+153, 9.332621544394415E+155,
			9.332621544394415E+157
		};

		static double[] factorialLnCache;
	}
}
