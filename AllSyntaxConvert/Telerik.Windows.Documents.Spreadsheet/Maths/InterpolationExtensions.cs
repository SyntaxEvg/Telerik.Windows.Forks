using System;
using System.Collections.Generic;
using System.Linq;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	static class InterpolationExtensions
	{
		static double[] ExtractRange(double?[] y)
		{
			List<double> list = new List<double>();
			for (int i = 0; i < y.Length; i++)
			{
				if (y[i] != null)
				{
					list.Add((double)(i + 1));
				}
			}
			return list.ToArray();
		}

		static double[] ExtractValues(double?[] y)
		{
			List<double> list = new List<double>();
			for (int i = 0; i < y.Length; i++)
			{
				if (y[i] != null)
				{
					list.Add(y[i].Value);
				}
			}
			return list.ToArray();
		}

		static double[] FillLinearInternal(double[] range, double[] y, int howMany = 10, double stepValue = 1.0, bool fromStart = false)
		{
			if (y.Length == 0)
			{
				throw new Exception("No values specified.");
			}
			if (howMany <= 0)
			{
				throw new ArgumentOutOfRangeException("howMany", "The amount of values to extrapolate should be greater than zero.");
			}
			if (y.Length == 1)
			{
				return Range.Create(1, howMany).Map((int x) => y[0]).ToArray<double>();
			}
			double[] interpol = InterpolationExtensions.LeastSquaresBestFitLine(range, y);
			if (interpol == null || interpol.Length != 3)
			{
				throw new Exception("The linear interpolation failed.");
			}
			Func<double, double> f = (double i) => interpol[0] + interpol[1] * i;
			if (!fromStart)
			{
				return Range.Create((double)(y.Length + 1), stepValue, howMany).Map((double x) => f(x)).ToArray<double>();
			}
			return Range.Create(1.0, stepValue, howMany).Map((double x) => f(x)).ToArray<double>();
		}

		public static double[] FillLinear(double?[] y, int howMany = 10, double stepValue = 1.0, bool fromStart = false)
		{
			double[] range = InterpolationExtensions.ExtractRange(y);
			double[] y2 = InterpolationExtensions.ExtractValues(y);
			return InterpolationExtensions.FillLinearInternal(range, y2, howMany, stepValue, fromStart);
		}

		public static double[] FillLinear(double[] y, int howMany = 10, double stepValue = 1.0, bool fromStart = false)
		{
			if (y == null)
			{
				throw new ArgumentNullException("y");
			}
			double[] range = Range.Create(1.0, stepValue, y.Length).ToArray<double>();
			return InterpolationExtensions.FillLinearInternal(range, y, howMany, stepValue, fromStart);
		}

		public static double[] FillExponential(double?[] y, int howMany = 10, double stepValue = 1.0, bool fromStart = false)
		{
			if (y == null)
			{
				throw new ArgumentNullException("y");
			}
			double[] range = InterpolationExtensions.ExtractRange(y);
			double[] y2 = InterpolationExtensions.ExtractValues(y);
			return InterpolationExtensions.FillExponentialInternal(range, y2, howMany, stepValue, fromStart);
		}

		static double[] FillExponentialInternal(double[] range, double[] y, int howMany = 10, double stepValue = 1.0, bool fromStart = false)
		{
			if (y.Length == 0)
			{
				throw new Exception("No values specified.");
			}
			if (howMany <= 0)
			{
				throw new ArgumentOutOfRangeException("howMany", "The amount of values to extrapolate should be greater than zero.");
			}
			int sign = Math.Sign(y[0]);
			if (y.Any((double x) => Math.Sign(x) != sign))
			{
				throw new ArgumentException("In order to have an exponential interpolation the values should all have the same sign.", "y");
			}
			if (y.Length == 1)
			{
				return Range.Create(1, howMany).Map((int x) => y[0]).ToArray<double>();
			}
			double[] y2 = y.Map((double d) => Math.Log(Math.Abs(d))).ToArray<double>();
			double[] interpol = InterpolationExtensions.LeastSquaresBestFitLine(range, y2);
			if (interpol == null || interpol.Length != 3)
			{
				throw new Exception("The linear interpolation failed.");
			}
			Func<double, double> f = (double i) => (double)sign * Math.Exp(interpol[0] + interpol[1] * i);
			if (!fromStart)
			{
				return Range.Create((double)(y.Length + 1), stepValue, howMany).Map((double x) => f(x)).ToArray<double>();
			}
			return Range.Create(1.0, stepValue, howMany).Map((double x) => f(x)).ToArray<double>();
		}

		public static double[] FillExponential(double[] y, int howMany = 10, double stepValue = 1.0, bool fromStart = false)
		{
			if (y == null)
			{
				throw new ArgumentNullException("y");
			}
			double[] range = Range.Create(1.0, stepValue, y.Length).ToArray<double>();
			return InterpolationExtensions.FillExponentialInternal(range, y, howMany, stepValue, fromStart);
		}

		public static double[] LeastSquaresBestFitExponential(double[] x, double[] y)
		{
			double[] y2 = y.Map(new Converter<double, double>(Math.Log)).ToArray<double>();
			double[] array = InterpolationExtensions.LeastSquaresBestFitLine(x, y2);
			return new double[]
			{
				Math.Exp(array[0]),
				array[1],
				array[2]
			};
		}

		public static double[] LeastSquaresBestFitLine(double[] x, double[] y)
		{
			if (x.Length != y.Length)
			{
				throw new ArgumentException("x,y", "The length of the arrays should be equal.");
			}
			int num = x.Length;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			double num5 = 0.0;
			double num6 = 0.0;
			for (int i = 0; i < num; i++)
			{
				num2 += x[i] / (double)num;
				num3 += y[i] / (double)num;
			}
			for (int j = 0; j < num; j++)
			{
				num4 += y[j] * (x[j] - num2);
				num5 += x[j] * (x[j] - num2);
			}
			double num7 = num4 / num5;
			double num8 = num3 - num2 * num7;
			for (int k = 0; k < num; k++)
			{
				num6 += (y[k] - num8 - num7 * x[k]) * (y[k] - num8 - num7 * x[k]);
			}
			double num9 = Math.Sqrt(num6 / (double)(num - 2));
			return new double[] { num8, num7, num9 };
		}

		public static double[] LeastSquaresWeightedBestFitLine(double[] x, double[] y, double[] w)
		{
			int num = x.Length;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			double num5 = 0.0;
			double num6 = 0.0;
			double num7 = 0.0;
			for (int i = 0; i < num; i++)
			{
				num4 += w[i];
			}
			for (int j = 0; j < num; j++)
			{
				num2 += w[j] * x[j] / num4;
				num3 += w[j] * y[j] / num4;
			}
			for (int k = 0; k < num; k++)
			{
				num5 += w[k] * y[k] * (x[k] - num2);
				num6 += w[k] * x[k] * (x[k] - num2);
			}
			double num8 = num5 / num6;
			double num9 = num3 - num2 * num8;
			for (int l = 0; l < num; l++)
			{
				num7 += w[l] * (y[l] - num9 - num8 * x[l]) * (y[l] - num9 - num8 * x[l]);
			}
			double num10 = Math.Sqrt(num7 / (double)(num - 2));
			return new double[] { num9, num8, num10 };
		}

		internal static RVector LinearRegression(double[] x, double[] y, ModelFunction[] f, out double sigma)
		{
			int num = f.Length;
			RMatrix a = new RMatrix(num, num);
			RVector b = new RVector(num);
			int num2 = x.Length;
			for (int i = 0; i < num; i++)
			{
				b[i] = 0.0;
				for (int j = 0; j < num2; j++)
				{
					ref RVector ptr = ref b;
					int i2;
					b[i2 = i] = ptr[i2] + f[i](x[j]) * y[j];
				}
			}
			for (int k = 0; k < num; k++)
			{
				for (int l = 0; l < num; l++)
				{
					a[k, l] = 0.0;
					for (int m = 0; m < num2; m++)
					{
						ref RMatrix ptr2 = ref a;
						int m2;
						int n;
						a[m2 = k, n = l] = ptr2[m2, n] + f[k](x[m]) * f[l](x[m]);
					}
				}
			}
			RVector result = AlgebraExtensions.GaussJordan(a, b);
			double num3 = 0.0;
			for (int num4 = 0; num4 < num2; num4++)
			{
				double num5 = 0.0;
				for (int num6 = 0; num6 < num; num6++)
				{
					num5 += result[num6] * f[num6](x[num4]);
				}
				num3 += (y[num4] - num5) * (y[num4] - num5);
			}
			sigma = Math.Sqrt(num3 / (double)(num2 - num));
			return result;
		}

		public static RVector PolynomialFit(double[] x, double[] y, int m, out double sigma)
		{
			m++;
			RMatrix a = new RMatrix(m, m);
			RVector b = new RVector(m);
			int num = x.Length;
			for (int i = 0; i < m; i++)
			{
				b[i] = 0.0;
				for (int j = 0; j < num; j++)
				{
					ref RVector ptr = ref b;
					int i2;
					b[i2 = i] = ptr[i2] + Math.Pow(x[j], (double)i) * y[j];
				}
			}
			for (int k = 0; k < m; k++)
			{
				for (int l = 0; l < m; l++)
				{
					a[k, l] = 0.0;
					for (int n = 0; n < num; n++)
					{
						ref RMatrix ptr2 = ref a;
						int m2;
						int n2;
						a[m2 = k, n2 = l] = ptr2[m2, n2] + Math.Pow(x[n], (double)(k + l));
					}
				}
			}
			RVector result = AlgebraExtensions.GaussJordan(a, b);
			double num2 = 0.0;
			for (int num3 = 0; num3 < num; num3++)
			{
				double num4 = 0.0;
				for (int num5 = 0; num5 < m; num5++)
				{
					num4 += result[num5] * Math.Pow(x[num3], (double)num5);
				}
				num2 += (y[num3] - num4) * (y[num3] - num4);
			}
			sigma = Math.Sqrt(num2 / (double)(num - m));
			return result;
		}
	}
}
