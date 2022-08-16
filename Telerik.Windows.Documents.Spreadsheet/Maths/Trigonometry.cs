using System;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	static class Trigonometry
	{
		public static double DegreesToRadians(double degree)
		{
			return degree * 0.017453292519943295;
		}

		public static double ToRadians(this double degree)
		{
			return Trigonometry.DegreesToRadians(degree);
		}

		public static double ToDegrees(this double radians)
		{
			return Trigonometry.DegreesToRadians(radians);
		}

		public static double RadiansToDegrees(double radian)
		{
			return radian / 0.017453292519943295;
		}

		public static double Sine(double radian)
		{
			return Math.Sin(radian);
		}

		public static double Cosine(double radian)
		{
			return Math.Cos(radian);
		}

		public static double Tangent(double radian)
		{
			return Math.Tan(radian);
		}

		public static double Cotangent(double radian)
		{
			return 1.0 / Math.Tan(radian);
		}

		public static double Secant(double radian)
		{
			return 1.0 / Math.Cos(radian);
		}

		public static double Cosecant(double radian)
		{
			return 1.0 / Math.Sin(radian);
		}

		public static double InverseSine(double real)
		{
			return Math.Asin(real);
		}

		public static double InverseCosine(double real)
		{
			return Math.Acos(real);
		}

		public static double InverseTangent(double real)
		{
			return Math.Atan(real);
		}

		public static double InverseTangentFromRatio(double nominator, double denominator)
		{
			return Math.Atan2(nominator, denominator);
		}

		public static double InverseCotangent(double real)
		{
			return Math.Atan(1.0 / real);
		}

		public static double InverseSecant(double real)
		{
			return Math.Acos(1.0 / real);
		}

		public static double InverseCosecant(double real)
		{
			return Math.Asin(1.0 / real);
		}

		public static double HyperbolicSine(double radian)
		{
			return Math.Sinh(radian);
		}

		public static double HyperbolicCosine(double radian)
		{
			return Math.Cosh(radian);
		}

		public static double HyperbolicTangent(double radian)
		{
			return Math.Tanh(radian);
		}

		public static double HyperbolicCotangent(double radian)
		{
			return 1.0 / Math.Tanh(radian);
		}

		public static double HyperbolicSecant(double radian)
		{
			return 1.0 / Trigonometry.HyperbolicCosine(radian);
		}

		public static double HyperbolicCosecant(double radian)
		{
			return 1.0 / Trigonometry.HyperbolicSine(radian);
		}

		public static double InverseHyperbolicSine(double real)
		{
			return Math.Log(real + Math.Sqrt(real * real + 1.0), 2.718281828459045);
		}

		public static double InverseHyperbolicCosine(double real)
		{
			return Math.Log(real + Math.Sqrt(real - 1.0) * Math.Sqrt(real + 1.0), 2.718281828459045);
		}

		public static double InverseHyperbolicTangent(double real)
		{
			return 0.5 * Math.Log((1.0 + real) / (1.0 - real), 2.718281828459045);
		}

		public static double InverseHyperbolicCotangent(double real)
		{
			return 0.5 * Math.Log((real + 1.0) / (real - 1.0), 2.718281828459045);
		}

		public static double InverseHyperbolicSecant(double real)
		{
			return Trigonometry.InverseHyperbolicCosine(1.0 / real);
		}

		public static double InverseHyperbolicCosecant(double real)
		{
			return Trigonometry.InverseHyperbolicSine(1.0 / real);
		}
	}
}
