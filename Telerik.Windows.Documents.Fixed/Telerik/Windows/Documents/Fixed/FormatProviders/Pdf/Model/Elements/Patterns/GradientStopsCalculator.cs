using System;
using System.Collections.Generic;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Common.Functions;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Patterns
{
	static class GradientStopsCalculator
	{
		public static IEnumerable<double> GetGradientStopPositionCoeficients(FunctionBase function, Matrix matrix, double t0, double t1, double distance, string colorspaceName)
		{
			yield return t0;
			IEnumerable<double> tCoeficients;
			if (!GradientStopsCalculator.IsColorSpaceComplientToMinimalStopsInterpolation(colorspaceName) || !GradientStopsCalculator.TryGetMinimalNumberOfGradientStops(function, t0, t1, out tCoeficients))
			{
				tCoeficients = GradientStopsCalculator.CalculateEvenlyDistributedStopCoeficients(t0, t1, matrix, distance);
			}
			foreach (double num in tCoeficients)
			{
				double coeficient = num;
				yield return coeficient;
			}
			yield return t1;
			yield break;
		}

		public static double CalculateLinearGradientStopsMaximalStep(IGradient gradient)
		{
			double distance = MathUtilities.GetDistance(gradient.StartPoint, gradient.EndPoint);
			return GradientStopsCalculator.CalculateGradientStopsMaximalStep(gradient.Position, distance);
		}

		public static double CalculateRadialGradientStopsMaximalStep(IRadialGradient gradient)
		{
			double distance = MathUtilities.GetDistance(gradient.StartPoint, gradient.EndPoint);
			double num = Math.Abs(gradient.EndRadius - gradient.StartRadius);
			return GradientStopsCalculator.CalculateGradientStopsMaximalStep(gradient.Position, num + distance);
		}

		static double CalculateGradientStopsMaximalStep(Matrix matrix, double distance)
		{
			double val = Math.Sqrt(Math.Pow(matrix.M11 + matrix.M21, 2.0) + Math.Pow(matrix.M12 + matrix.M22, 2.0));
			double val2 = Math.Sqrt(Math.Pow(matrix.M11 - matrix.M21, 2.0) + Math.Pow(matrix.M12 - matrix.M22, 2.0));
			double num = distance * Math.Max(val, val2);
			return 1.0 / (num * 3.0);
		}

		static bool IsColorSpaceComplientToMinimalStopsInterpolation(string colorSpaceName)
		{
			return colorSpaceName == "DeviceRGB" || colorSpaceName == "DeviceGray";
		}

		static bool TryGetMinimalNumberOfGradientStops(FunctionBase function, double t0, double t1, out IEnumerable<double> tCoeficients)
		{
			if (function.FunctionType == FunctionType.Sampled)
			{
				tCoeficients = GradientStopsCalculator.GetGradientStopCoeficients((SampledFunction)function, t0, t1);
				return true;
			}
			if (function.FunctionType == FunctionType.Stitching)
			{
				return GradientStopsCalculator.TryGetMinimalNumberOfGradientStops((StitchingFunction)function, t0, t1, out tCoeficients);
			}
			tCoeficients = null;
			return false;
		}

		static bool TryGetMinimalNumberOfGradientStops(StitchingFunction function, double t0, double t1, out IEnumerable<double> tCoeficients)
		{
			int num = 0;
			double t2 = t0;
			List<double> list = new List<double>();
			tCoeficients = null;
			IEnumerable<double> collection;
			foreach (double boundValue in function.Bounds)
			{
				double num2 = GradientStopsCalculator.CalculateBoundsGradientStopCoeficient(function, boundValue, t0, t1);
				FunctionBase function2 = function.Functions[num];
				if (!GradientStopsCalculator.TryGetMinimalNumberOfGradientStops(function2, t2, num2, out collection))
				{
					return false;
				}
				list.AddRange(collection);
				list.Add(num2);
				num++;
				t2 = num2;
			}
			FunctionBase function3 = function.Functions[num];
			if (!GradientStopsCalculator.TryGetMinimalNumberOfGradientStops(function3, t2, t1, out collection))
			{
				return false;
			}
			list.AddRange(collection);
			tCoeficients = list;
			return true;
		}

		static double CalculateBoundsGradientStopCoeficient(StitchingFunction function, double boundValue, double t0, double t1)
		{
			double num = function.Domain[1] - function.Domain[0];
			if (num == 0.0)
			{
				return t0;
			}
			double num2 = (boundValue - function.Domain[0]) / num;
			double num3 = (function.Domain[1] - boundValue) / num;
			return t0 * num3 + t1 * num2;
		}

		static IEnumerable<double> GetGradientStopCoeficients(SampledFunction function, double t0, double t1)
		{
			int numberOfSamples = function.Size[0];
			int numberOfGradientStops = numberOfSamples - 2;
			double step = (t1 - t0) / (double)(numberOfGradientStops + 1);
			for (int i = 1; i <= numberOfGradientStops; i++)
			{
				double t2 = t0 + (double)i * step;
				yield return t2;
			}
			yield break;
		}

		static IEnumerable<double> CalculateEvenlyDistributedStopCoeficients(double t0, double t1, Matrix matrix, double distance)
		{
			double step = GradientStopsCalculator.CalculateGradientStopsMaximalStep(matrix, distance);
			if (t0 < t1)
			{
				for (double t2 = t0 + step; t2 < t1; t2 += step)
				{
					yield return t2;
				}
			}
			else
			{
				for (double t3 = t0 - step; t3 > t1; t3 -= step)
				{
					yield return t3;
				}
			}
			yield break;
		}
	}
}
