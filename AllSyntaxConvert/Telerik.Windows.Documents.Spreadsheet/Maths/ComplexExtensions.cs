using System;
using System.Linq;
using System.Numerics;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	static class ComplexExtensions
	{
		public static bool IsZero(this Complex complex)
		{
			return complex.Real.IsZero(1E-08) && complex.Imaginary.IsZero(1E-08);
		}

		public static double PolarFormModulus(this Complex c)
		{
			double real = c.Real;
			double imaginary = c.Imaginary;
			return Math.Sqrt(Math.Pow(real, 2.0) + Math.Pow(imaginary, 2.0));
		}

		public static double PolarFormAngle(this Complex c)
		{
			double real = c.Real;
			double imaginary = c.Imaginary;
			return Math.Atan2(real, imaginary);
		}

		public static Complex NegateImaginaryNumberPart(this Complex c)
		{
			return new Complex(c.Real, -c.Imaginary);
		}

		public static Complex NegateRealNumberPart(this Complex c)
		{
			return new Complex(-c.Real, c.Imaginary);
		}

		public static bool IsOne(this Complex complex)
		{
			return complex.Real.IsEqualTo(1.0) && complex.Imaginary.IsZero(1E-08);
		}

		public static bool IsImaginaryOne(this Complex complex)
		{
			return complex.Real.IsZero(1E-08) && complex.Imaginary.IsEqualTo(1.0);
		}

		public static bool IsNaN(this Complex complex)
		{
			return double.IsNaN(complex.Real) || double.IsNaN(complex.Imaginary);
		}

		public static bool IsInfinity(this Complex complex)
		{
			return double.IsInfinity(complex.Real) || double.IsInfinity(complex.Imaginary);
		}

		public static bool IsReal(this Complex complex)
		{
			return complex.Imaginary.IsZero(1E-08);
		}

		public static bool IsRealNonNegative(this Complex complex)
		{
			return complex.Imaginary.IsZero(1E-08) && complex.Real >= 0.0;
		}

		public static Complex Conjugate(this Complex complex)
		{
			return new Complex(complex.Real, -complex.Imaginary);
		}

		public static double NormSquared(this Complex complex)
		{
			return complex.Real * complex.Real + complex.Imaginary * complex.Imaginary;
		}

		public static Complex Exp(this Complex complex)
		{
			double num = Math.Exp(complex.Real);
			if (!complex.IsReal())
			{
				return new Complex(num * Math.Cos(complex.Imaginary), num * Math.Sin(complex.Imaginary));
			}
			return new Complex(num, 0.0);
		}

		public static Complex Log(this Complex complex)
		{
			if (!complex.IsRealNonNegative())
			{
				return new Complex(0.5 * Math.Log(complex.NormSquared()), complex.Phase);
			}
			return new Complex(Math.Log(complex.Real), 0.0);
		}

		public static Complex Pow(this Complex complex, Complex exponent)
		{
			if (!complex.IsZero())
			{
				return (exponent * complex.Log()).Exp();
			}
			if (exponent.IsZero())
			{
				return Complex.One;
			}
			if (exponent.Real > 0.0)
			{
				return Complex.Zero;
			}
			if (exponent.Real >= 0.0)
			{
				return double.NaN;
			}
			if (!exponent.Imaginary.IsZero(1E-08))
			{
				return new Complex(double.PositiveInfinity, double.PositiveInfinity);
			}
			return new Complex(double.PositiveInfinity, 0.0);
		}

		public static Complex Root(this Complex complex, Complex rootExponent)
		{
			return complex.Pow(1 / rootExponent);
		}

		public static Complex Squared(this Complex complex)
		{
			if (!complex.IsReal())
			{
				return new Complex(complex.Real * complex.Real - complex.Imaginary * complex.Imaginary, 2.0 * complex.Real * complex.Imaginary);
			}
			return new Complex(complex.Real * complex.Real, 0.0);
		}

		public static Complex SquareRoot(this Complex complex)
		{
			if (complex.IsRealNonNegative())
			{
				return new Complex(Math.Sqrt(complex.Real), 0.0);
			}
			double num = Math.Abs(complex.Real);
			double num2 = Math.Abs(complex.Imaginary);
			double num4;
			if (num >= num2)
			{
				double num3 = complex.Imaginary / complex.Real;
				num4 = Math.Sqrt(num) * Math.Sqrt(0.5 * (1.0 + Math.Sqrt(1.0 + num3 * num3)));
			}
			else
			{
				double num5 = complex.Real / complex.Imaginary;
				num4 = Math.Sqrt(num2) * Math.Sqrt(0.5 * (Math.Abs(num5) + Math.Sqrt(1.0 + num5 * num5)));
			}
			Complex result;
			if (complex.Real >= 0.0)
			{
				result = new Complex(num4, complex.Imaginary / (2.0 * num4));
			}
			else if (complex.Imaginary >= 0.0)
			{
				result = new Complex(num2 / (2.0 * num4), num4);
			}
			else
			{
				result = new Complex(num2 / (2.0 * num4), -num4);
			}
			return result;
		}

		public static double Norm(this Complex complex)
		{
			return Math.Sqrt(complex.Real * complex.Real + complex.Imaginary * complex.Imaginary);
		}

		public static double NormOfDifference(this Complex u, Complex v)
		{
			return (u - v).NormSquared();
		}

		public static Complex ToComplex(this string complexString)
		{
			if (string.IsNullOrEmpty(complexString))
			{
				return new Complex(0.0, 0.0);
			}
			complexString = complexString.Trim();
			Complex result;
			try
			{
				if (complexString.Last<char>() != 'i' && complexString.Last<char>() != 'j')
				{
					double real = double.Parse(complexString.TrimWhiteSpaceBetweenSignAndDigits());
					result = new Complex(real, 0.0);
				}
				else
				{
					result = ComplexExtensions.ParseToComplexWithImaginaryPart(complexString);
				}
			}
			catch (Exception innerException)
			{
				throw new ArgumentException("Cannot parse complexString to Complex number! ", innerException);
			}
			return result;
		}

		static Complex ParseToComplexWithImaginaryPart(string complexString)
		{
			if (complexString.Length == 1)
			{
				return new Complex(0.0, 1.0);
			}
			int num = ComplexExtensions.IndexOfImaginarySign(complexString);
			double real = 0.0;
			if (num > 0)
			{
				string s = complexString.Substring(0, num).TrimWhiteSpaceBetweenSignAndDigits();
				real = double.Parse(s);
			}
			if (num == -1)
			{
				num = 0;
			}
			string text = complexString.Substring(num, complexString.Length - num - 1).TrimWhiteSpaceBetweenSignAndDigits();
			if (text.Length == 1 && text[0].IsPlusOrMinus())
			{
				text += "1";
			}
			double imaginary = double.Parse(text);
			return new Complex(real, imaginary);
		}

		static string TrimWhiteSpaceBetweenSignAndDigits(this string numberString)
		{
			if (numberString.Length < 2)
			{
				return numberString;
			}
			string arg = string.Empty;
			if (numberString[0].IsPlusOrMinus())
			{
				arg = numberString[0].ToString();
				numberString = numberString.Substring(1);
			}
			return string.Format("{0}{1}", arg, numberString.Trim());
		}

		static int IndexOfImaginarySign(string complexString)
		{
			int num = -1;
			for (int i = complexString.Length - 2; i > 0; i--)
			{
				if (complexString[i].IsPlusOrMinus() && !complexString[i - 1].IsScientificE())
				{
					num = i;
					break;
				}
			}
			if (num == -1 && complexString[0].IsPlusOrMinus())
			{
				num = 0;
			}
			return num;
		}

		static bool IsPlusOrMinus(this char symbol)
		{
			return symbol == '+' || symbol == '-';
		}

		static bool IsScientificE(this char symbol)
		{
			return symbol == 'e' || symbol == 'E';
		}
	}
}
